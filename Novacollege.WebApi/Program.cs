using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Novacollege.Data.Data;
using Novacollege.Data.Entities;
using Novacollege.WebApi.Authentication;
using Novacollege.WebApi.Dtos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NovacollegeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NovaCollege")));


builder.Services.AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

builder.Services.AddAuthorization();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = ["index.html"]
});
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "wwwroot", "browser"))
});

// Endpoint para: Obtener las diferentes provincias a las que pertenecen los estudiantes y el número de
// estudiantes de cada provincia (SQL)
app.MapGet("/provincias/info", async (
    NovacollegeDbContext context) =>
{
    var provincias = await context.ObtenerEstudiantesPorProvincia();
    var provinciasDtos = provincias.Select(p => new ProvinciaInfoResponse
    {
        Provincia = p.Provincia,
        NumeroEstudiantes = p.NumeroEstudiantes
    }).ToList();
    return Results.Ok(provinciasDtos);
})
.WithName("GetProvinciasInfo")
.RequireAuthorization();

// Endpoint para:  Obtener la provincia que tiene más estudiantes en el curso (curso debe ser un
// parámetro del procedimiento) y posteriormente como se ejecutaría en sqlserver ese
// procedimiento
app.MapGet("/provincias/curso/{id:int}/estudiantes", async (
    NovacollegeDbContext context,
    [FromRoute]int id) =>
{
    var provincia = await context.ObtenerProvinciasConMasEstudiantesPorCurso(id);
    
    if (provincia == null)
        return Results.NotFound($"Curso con ID {id} no encontrado");

    var provinciaDto = new ProvinciaInfoResponse
    {
        Provincia = provincia.Provincia,
        NumeroEstudiantes = provincia.NumeroEstudiantes
    };
    return Results.Ok(provinciaDto);
})
.WithName("GetEstudiantesPorCurso")
.RequireAuthorization();

 // Endpoint para: Insertar estudiantes
app.MapPost("/estudiantes", async (
    NovacollegeDbContext context,
    IValidator<CreateEstudianteRequest> validator,
    [FromBody] CreateEstudianteRequest request) =>
{
    var validationResult = await validator.ValidateAsync(request);
    if (!validationResult.IsValid)
        return Results.BadRequest(validationResult.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));

    var estudiante = new Estudiante
    {
        ApelEst = request.ApelEst,
        NombEst = request.NombEst,
        FnacEst = request.FnacEst,
        SexoEst = request.SexoEst,
        DireEst = request.DireEst,
        TcolEst = request.TcolEst,
        GinsEst = request.GinsEst,
        IdDistrito = request.IdDistrito
    };

    context.Estudiantes.Add(estudiante);
    await context.SaveChangesAsync();

    var estudianteDto = new CreateEstudianteResponse()
    {
        Id = estudiante.Id,
        ApelEst = estudiante.ApelEst,
        NombEst = estudiante.NombEst,
        FnacEst = estudiante.FnacEst,
        SexoEst = estudiante.SexoEst,
        DireEst = estudiante.DireEst,
        TcolEst = estudiante.TcolEst,
        GinsEst = estudiante.GinsEst,
        IdDistrito = estudiante.IdDistrito
    };
    return Results.Created($"/estudiantes/{estudiante.Id}", estudianteDto);
})
.WithName("CreateEstudiante")
.RequireAuthorization();

// Endpoint para: Nos devuelva los estudiantes de una provincia (objetivo filtros api)
app.MapGet("/estudiantes", async (
    NovacollegeDbContext context,
    [FromQuery] int? idProvincia) =>
{
    IQueryable<Estudiante> query = context.Estudiantes
        .AsSplitQuery()
        .Include(e => e.Distrito)
        .ThenInclude(d => d!.Provincia);

    if (idProvincia.HasValue)
    {
        query = query.Where(e => e.Distrito!.Provincia!.Id == idProvincia.Value);
    }

    var estudiantes = await query.ToListAsync();
    var estudiantesDto = estudiantes.Select(e => new CreateEstudianteResponse()
    {
        Id = e.Id,
        ApelEst = e.ApelEst,
        NombEst = e.NombEst,
        FnacEst = e.FnacEst,
        SexoEst = e.SexoEst,
        DireEst = e.DireEst,
        TcolEst = e.TcolEst,
        GinsEst = e.GinsEst,
        IdDistrito = e.IdDistrito
    }).ToList();
    return Results.Ok(estudiantesDto);
})
.WithName("GetEstudiantesFiltrados")
.RequireAuthorization();

// Endpoint para: Api que obtenga los datos y por LINQ haga lo siguiente: Tener una lista que por docente
// nos de una lista de Información de Cursos , y para cada curso tengamos el listado de
// provincias en las que tiene alumnos y para cada provincia la información de alumnos. El
// objetivo es utilizar lo menos posible sentencias while, for o foreach. 
app.MapGet("/docentes", async (NovacollegeDbContext context) =>
{
    var cursos = await context.Cursos
        .AsNoTrackingWithIdentityResolution()
        .AsSplitQuery()
        .Include(c => c.Matriculas)
            .ThenInclude(m => m.Estudiante)
                .ThenInclude(e => e.Distrito)
                    .ThenInclude(d => d.Provincia)
        .Include(c => c.Asignaciones)
            .ThenInclude(a => a.Docente)
        .ToListAsync();

    var docentes = cursos
        .SelectMany(curso =>
            curso.Asignaciones.Select(asignacion => new
            {
                Docente = asignacion.Docente,
                Curso = curso,

                Matriculas = curso.Matriculas.Select(m => new
                {
                    Estudiante = m.Estudiante,
                    ProvinciaId = m.Estudiante.Distrito.Provincia.Id,
                    ProvinciaNombre = m.Estudiante.Distrito.Provincia.NombPro
                })
            }))
        .GroupBy(x => x.Docente.Id)
        .Select(docenteGroup =>
        {
            var docente = docenteGroup.First().Docente;

            return new DocentesResponse
            {
                Id = docente.Id,
                NombDoc = docente.NombDoc,
                ApelDoc = docente.ApelDoc,
                Cursos = docenteGroup
                    .GroupBy(x => x.Curso.Id)
                    .Select(cursoGroup =>
                    {
                        var curso = cursoGroup.First().Curso;

                        return new CursoRespone
                        {
                            Id = curso.Id,
                            Nombre = curso.NombCur,
                            Provincias = cursoGroup
                                .SelectMany(x => x.Matriculas)
                                .GroupBy(x => x.ProvinciaId)
                                .Select(provinciaGroup =>
                                {
                                    var provincia = provinciaGroup.First();

                                    return new ProvinciaResponse
                                    {
                                        Id = provincia.ProvinciaId,
                                        Nombre = provincia.ProvinciaNombre,
                                        Estudiantes = provinciaGroup
                                            .Select(x => x.Estudiante)
                                            .DistinctBy(e => e.Id)
                                            .Select(e => new EstudianteResponse
                                            {
                                                Id = e.Id,
                                                Nombre = e.NombEst,
                                                Apellidos = e.ApelEst
                                            })
                                            .ToArray()
                                    };
                                })
                                .ToArray()
                        };
                    })
                    .ToArray()
            };
        })
        .ToArray();

    return Results.Ok(docentes);
})
.WithName("GetDocentesCursosProvincias")
.RequireAuthorization();


var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = serviceScopeFactory.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<NovacollegeDbContext>();
await context.Database.MigrateAsync();

await app.RunAsync();