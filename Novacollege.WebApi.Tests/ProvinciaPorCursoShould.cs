using Bogus;
using Microsoft.EntityFrameworkCore;
using Novacollege.Data.Data;
using Novacollege.Data.Entities;
using Testcontainers.MsSql;

namespace Novacollege.WebApi.Tests;

public class ProvinciaPorCursoShould : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("Your_password123")
        .Build();

    private NovacollegeDbContext? _context;

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        var options = new DbContextOptionsBuilder<NovacollegeDbContext>()
            .UseSqlServer(_msSqlContainer.GetConnectionString())
            .Options;

        _context = new NovacollegeDbContext(options);
        await _context.Database.MigrateAsync();
        await SeedDataAsync();
    }

    public async Task DisposeAsync()
    {
        if (_context != null)
            await _context.DisposeAsync();
        await _msSqlContainer.DisposeAsync();
    }

    private async Task SeedDataAsync()
    {
        Randomizer.Seed = new Random(1234);

        if (_context == null)
        {
            return;
        }

        var provinciaValencia = new Provincia
        {
            NombPro = "Valencia"
        };

        var provinciaMadrid = new Provincia
        {
            NombPro = "Madrid"
        };

        Provincia[] provincias =
        [
            provinciaValencia,
            provinciaMadrid
        ];

        _context.Provincias.AddRange(provincias);

        await _context.SaveChangesAsync();

        var distritoFaker = new Faker<Distrito>()
            .RuleFor(d => d.NombDis, f => f.Address.City());

        var distritosValencia = distritoFaker
            .Clone()
            .RuleFor(d => d.IdProvincia, _ => provinciaValencia.Id)
            .Generate(4);

        var distritosMadrid = distritoFaker
            .Clone()
            .RuleFor(d => d.IdProvincia, _ => provinciaMadrid.Id)
            .Generate(1);

        _context.Distritos.AddRange([.. distritosValencia, .. distritosMadrid]);

        await _context.SaveChangesAsync();

        var cursoNet = new Curso
        {
            NombCur = "Programación .NET",
            CostCur = 350,
            DuraCur = 60
        };
        var cursoSql = new Curso
        {
            NombCur = "SQL Server",
            CostCur = 250,
            DuraCur = 40
        };
        var cursoAngular = new Curso
        {
            NombCur = "Angular",
            CostCur = 300,
            DuraCur = 50
        };

        _context.Cursos.AddRange([cursoNet, cursoSql, cursoAngular]);

        await _context.SaveChangesAsync();

        IList<Profesion> profesiones =
        [
            new Profesion { NombPro = "Ingeniero" },
            new Profesion { NombPro = "Matemático" },
            new Profesion { NombPro = "Informático" }
        ];

        _context.Profesiones.AddRange(profesiones);

        await _context.SaveChangesAsync();

        var docenteFaker = new Faker<Docente>()
            .RuleFor(d => d.ApelDoc, f => f.Person.LastName)
            .RuleFor(d => d.NombDoc, f => f.Person.FirstName)
            .RuleFor(d => d.DireDoc, f => f.Address.FullAddress())
            .RuleFor(d => d.NtelDoc, f => f.Phone.PhoneNumber("#########"))
            .RuleFor(d => d.NcelDoc, f => f.Phone.PhoneNumber("#########"))
            .RuleFor(d => d.GradDoc, f => f.Name.JobTitle())
            .RuleFor(d => d.IdProfesion,
                f => f.PickRandom(profesiones).Id);

        var docentes = docenteFaker.Generate(20);

        _context.Docentes.AddRange(docentes);

        await _context.SaveChangesAsync();

        var estudianteFaker = new Faker<Estudiante>()
            .RuleFor(e => e.ApelEst, f => f.Person.LastName)
            .RuleFor(e => e.NombEst, f => f.Person.FirstName)
            .RuleFor(e => e.FnacEst, f => DateOnly.FromDateTime(f.Date.Past(25)))
            .RuleFor(e => e.SexoEst,
                f => f.PickRandom(new[] { "M", "F" }))
            .RuleFor(e => e.DireEst, f => f.Address.StreetAddress())
            .RuleFor(e => e.TcolEst,
                f => f.Phone.PhoneNumber("#########"))
            .RuleFor(e => e.GinsEst, f => f.Date.Recent(365))
            .RuleFor(e => e.IdDistrito,
                f => f.PickRandom(distritosValencia).Id);

        var estudiantes = estudianteFaker.Generate(500);

        _context.Estudiantes.AddRange(estudiantes);

        await _context.SaveChangesAsync();

        var matriculFaker = new Faker<Matricula>()
            .RuleFor(m => m.FechMat,
                f => f.Date.Recent(365))
            .RuleFor(m => m.IdEstudiante,
                f => f.PickRandom(estudiantes).Id);

        var matriculNetFaker = matriculFaker
            .Clone()
            .RuleFor(m => m.IdCurso,
                f => f.PickRandom(new[] { cursoNet }).Id)
            .Generate(1000);

        var matriculOthersFaker = matriculFaker
            .Clone()
            .RuleFor(m => m.IdCurso,
                f => f.PickRandom(new[] { cursoSql, cursoAngular }).Id)
            .Generate(500);

        _context.Matriculas.AddRange([.. matriculNetFaker, .. matriculOthersFaker]);

        await _context.SaveChangesAsync();

        var asignacionFaker = new Faker<Asignacion>()
            .RuleFor(a => a.FechAsi,
                f => f.Date.Recent(365))
            .RuleFor(a => a.IdCurso,
                f => f.PickRandom(new[] { cursoNet, cursoSql, cursoAngular }).Id)
            .RuleFor(a => a.IdDocente,
                f => f.PickRandom(docentes).Id);

        var asignaciones = asignacionFaker.Generate(50);

        _context.Asignaciones.AddRange(asignaciones);

        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task ReturnStudentsCountByProvince_WhenDataExists()
    {
        var provincia = await _context!
            .ObtenerProvinciasConMasEstudiantesPorCurso(1);

        Assert.NotNull(provincia);
        Assert.Equal("Valencia", provincia.Provincia);
        Assert.Equal(1000, provincia.NumeroEstudiantes);
    }
}