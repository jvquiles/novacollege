using Bogus;
using Microsoft.EntityFrameworkCore;
using Novacollege.Data.Data;
using Novacollege.Data.Entities;
using Testcontainers.MsSql;

namespace Novacollege.WebApi.Tests;

public class EstudiantesPorProvinciaShould : IAsyncLifetime
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
        if (_context == null) return;

        var provinciaValencia = new Provincia
        {
            NombPro = "Valencia"
        };

        var provinciaMadrid = new Provincia
        {
            NombPro = "Madrid"
        };
        Provincia[] provincias = [provinciaValencia, provinciaMadrid];
        _context.Provincias.AddRange(provincias);
        await _context.SaveChangesAsync();

        var distritoFaker = new Faker<Distrito>()
            .RuleFor(d => d.NombDis, f => f.Address.City())
            .RuleFor(d => d.IdProvincia, f => provinciaMadrid.Id);

        var distritos = distritoFaker.Generate(2);
        _context.Distritos.AddRange(distritos);
        await _context.SaveChangesAsync();

        var estudianteFaker = new Faker<Estudiante>()
            .RuleFor(e => e.ApelEst, f => f.Person.LastName)
            .RuleFor(e => e.NombEst, f => f.Person.FirstName)
            .RuleFor(e => e.FnacEst, f => DateOnly.FromDateTime(f.Date.Past(25)))
            .RuleFor(e => e.SexoEst, f => f.PickRandom(new[] { "M", "F" }))
            .RuleFor(e => e.DireEst, f => f.Address.StreetAddress())
            .RuleFor(e => e.TcolEst, f => f.Phone.PhoneNumber("##########"))
            .RuleFor(e => e.GinsEst, f => f.Date.Recent(1))
            .RuleFor(e => e.IdDistrito, f => f.PickRandom(distritos).Id);

        var estudiantes = estudianteFaker.Generate(5);
        _context.Estudiantes.AddRange(estudiantes);
        await _context.SaveChangesAsync();
    }

    [Fact]
    public async Task ReturnStudentsCountByProvince_WhenDataExists()
    {
        var result = await _context!.
            ObtenerEstudiantesPorProvincia();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        var madridProvincia = result.First();
        Assert.Equal("Madrid", madridProvincia.Provincia);
        Assert.Equal(5, madridProvincia.NumeroEstudiantes);

        var valenciaProvincia = result.Last();
        Assert.Equal("Valencia", valenciaProvincia.Provincia);
        Assert.Equal(0, valenciaProvincia.NumeroEstudiantes);
    }
}