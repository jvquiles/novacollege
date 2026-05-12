using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Novacollege.Data.Data;
using Novacollege.Data.Entities;
using Novacollege.WebApi.Dtos;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Testcontainers.MsSql;
using Xunit.Abstractions;

namespace Novacollege.WebApi.Tests.Integration;

public class DocentesCursosProvinciasShould : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("Your_password123")
        .Build();

    private WebApplicationFactory<Program>? _factory;
    private NovacollegeDbContext? _context;

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        var connectionString = _msSqlContainer.GetConnectionString();

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.Remove(services.Single(d => d.ServiceType == typeof(DbContextOptions<NovacollegeDbContext>)));
                    services.AddDbContext<NovacollegeDbContext>(options =>
                        options.UseSqlServer(connectionString));
                });
            });

        var options = new DbContextOptionsBuilder<NovacollegeDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        _context = new NovacollegeDbContext(options);
        await _context.Database.MigrateAsync();
        await SeedDataAsync();
    }

    public async Task DisposeAsync()
    {
        if (_context != null)
            await _context.DisposeAsync();
        if (_factory != null)
            await _factory.DisposeAsync();
        await _msSqlContainer.DisposeAsync();
    }

    private async Task SeedDataAsync()
    {
        if (_context == null) return;

        var profesionFaker = new Faker<Profesion>()
            .RuleFor(p => p.NombPro, f => f.Name.JobTitle());
        var profesiones = profesionFaker.Generate(1);
        _context.Profesiones.AddRange(profesiones);
        await _context.SaveChangesAsync();

        var docenteFaker = new Faker<Docente>()
            .RuleFor(d => d.ApelDoc, f => f.Person.LastName)
            .RuleFor(d => d.NombDoc, f => f.Person.FirstName)
            .RuleFor(d => d.DireDoc, f => f.Address.StreetAddress())
            .RuleFor(d => d.NtelDoc, f => f.Phone.PhoneNumber())
            .RuleFor(d => d.NcelDoc, f => f.Phone.PhoneNumber())
            .RuleFor(d => d.GradDoc, f => f.Name.JobTitle())
            .RuleFor(d => d.IdProfesion, f => profesiones[f.IndexFaker % profesiones.Count].Id);
        var docentes = docenteFaker.Generate(10);
        _context.Docentes.AddRange(docentes);
        await _context.SaveChangesAsync();

        var cursoFaker = new Faker<Curso>()
            .RuleFor(c => c.NombCur, f => f.Commerce.ProductName())
            .RuleFor(c => c.CostCur, f => f.Random.Decimal(100, 1000))
            .RuleFor(c => c.DuraCur, f => f.Random.Int(20, 100));
        var cursos = cursoFaker.Generate(30);
        _context.Cursos.AddRange(cursos);
        await _context.SaveChangesAsync();

        var asignacionFaker = new Faker<Asignacion>()
            .RuleFor(a => a.FechAsi, f => f.Date.Past(1))
            .RuleFor(a => a.IdCurso, f => cursos[f.IndexFaker % cursos.Count].Id)
            .RuleFor(a => a.IdDocente, f => docentes[f.IndexFaker % docentes.Count].Id);
        var asignaciones = asignacionFaker.Generate(30);
        _context.Asignaciones.AddRange(asignaciones);
        await _context.SaveChangesAsync();

        var provinciaFaker = new Faker<Provincia>()
            .RuleFor(p => p.NombPro, f => f.Address.State());
        var provincias = provinciaFaker.Generate(5);
        _context.Provincias.AddRange(provincias);
        await _context.SaveChangesAsync();

        var distritoFaker = new Faker<Distrito>()
            .RuleFor(d => d.NombDis, f => f.Address.City())
            .RuleFor(d => d.IdProvincia, f => provincias[f.IndexFaker % provincias.Count].Id);
        var distritos = distritoFaker.Generate(20);
        _context.Distritos.AddRange(distritos);
        await _context.SaveChangesAsync();

        var estudianteFaker = new Faker<Estudiante>()
            .RuleFor(e => e.ApelEst, f => f.Person.LastName)
            .RuleFor(e => e.NombEst, f => f.Person.FirstName)
            .RuleFor(e => e.FnacEst, f => DateOnly.FromDateTime(f.Date.Past(25)))
            .RuleFor(e => e.SexoEst, f => f.PickRandom(new[] { "M", "F" }))
            .RuleFor(e => e.DireEst, f => f.Address.StreetAddress())
            .RuleFor(e => e.TcolEst, f => f.Phone.PhoneNumber("##########"))
            .RuleFor(e => e.GinsEst, f => f.Date.Recent(2000))
            .RuleFor(e => e.IdDistrito, f => distritos[f.IndexFaker % distritos.Count].Id);
        var estudiantes = estudianteFaker.Generate(500);
        _context.Estudiantes.AddRange(estudiantes);
        await _context.SaveChangesAsync();

        var matriculaFaker = new Faker<Matricula>()
            .RuleFor(m => m.FechMat, f => f.Date.Past(1))
            .RuleFor(m => m.IdEstudiante, f => estudiantes[f.IndexFaker % estudiantes.Count].Id)
            .RuleFor(m => m.IdCurso, f => cursos[f.IndexFaker % cursos.Count].Id);
        var matriculas = matriculaFaker.Generate(1200);
        _context.Matriculas.AddRange(matriculas);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task ReturnDocentesWithCoursesAndProvincias_WhenDataExists()
    {
        var client = _factory!.CreateClient();
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:admin123"));
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

        var response = await client.GetAsync("/docentes");

        response.EnsureSuccessStatusCode();

        var docentes = await response.Content.ReadFromJsonAsync<DocentesResponse[]>();
        Assert.NotNull(docentes);
        Assert.Equal(10, docentes.Length);
        Assert.Equal(30, docentes.Sum(d => d.Cursos.Length));
        Assert.Equal(1200, docentes.Sum(d => d.Cursos.Sum(c => c.Provincias.Sum(p => p.Estudiantes.Length))));
    }
}