using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Novacollege.Data.Data;
using Novacollege.Data.Entities;
using Novacollege.WebApi.Authentication;
using Novacollege.WebApi.Dtos;
using Novacollege.WebApi.Validators;

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

app.MapGet("/provincias/summary", async (
    NovacollegeDbContext context) =>
{
    return await context.ObtenerEstudiantesPorProvincia();
})
.WithName("GetEstudiantesPorProvincias")
.RequireAuthorization();

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

    return Results.Created($"/estudiantes/{estudiante.Id}", estudiante);
})
.WithName("CreateEstudiante")
.RequireAuthorization();

var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = serviceScopeFactory.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<NovacollegeDbContext>();
await context.Database.MigrateAsync();

await app.RunAsync();