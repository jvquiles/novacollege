using Microsoft.EntityFrameworkCore;
using Novacollege.Data.Entities;
using Novacollege.Data.StoredProcedures;

namespace Novacollege.Data.Data;

public class NovacollegeDbContext(
    DbContextOptions<NovacollegeDbContext> options)
    : DbContext(options)
{
    public DbSet<Distrito> Distritos { get; set; }
    public DbSet<Estudiante> Estudiantes { get; set; }
    public DbSet<Profesion> Profesiones { get; set; }
    public DbSet<Docente> Docentes { get; set; }
    public DbSet<Curso> Cursos { get; set; }
    public DbSet<Asignacion> Asignaciones { get; set; }
    public DbSet<Matricula> Matriculas { get; set; }
    public DbSet<Provincia> Provincias { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    public async Task<IList<EstudiantesPorProvincia>> ObtenerEstudiantesPorProvincia() => await Set<EstudiantesPorProvincia>()
        .FromSqlRaw("EXEC sp_ObtenerEstudiantesPorProvincia")
        .ToListAsync();

    public async Task<ProvinciaPorCurso?> ObtenerProvinciasConMasEstudiantesPorCurso(
        int idCurso)
    {
        var result = await Set<ProvinciaPorCurso>()
            .FromSqlInterpolated($"EXEC sp_ProvinciaConMasEstudiantesPorCurso @IdCurso = {idCurso}")
            .ToListAsync();
        return result.FirstOrDefault();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.ToTable("TB_PROVINCIA");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombPro)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Distrito>(entity =>
        {
            entity.ToTable("TB_DISTRITO");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombDis)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.IdProvincia)
                .IsRequired();
            entity.HasOne(e => e.Provincia)
                .WithMany(p => p.Distritos)
                .HasForeignKey(e => e.IdProvincia)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => e.IdProvincia);
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.ToTable("TB_ESTUDIANTE");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ApelEst)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.NombEst)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.FnacEst)
                .IsRequired();
            entity.Property(e => e.SexoEst)
                .IsRequired()
                .HasMaxLength(1);
            entity.Property(e => e.DireEst)
                .HasMaxLength(200);
            entity.Property(e => e.TcolEst)
                .HasMaxLength(30);
            entity.Property(e => e.IdDistrito)
                .IsRequired();
            entity.HasOne(e => e.Distrito)
                .WithMany(d => d.Estudiantes)
                .HasForeignKey(e => e.IdDistrito)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => e.IdDistrito);
        });

        modelBuilder.Entity<Profesion>(entity =>
        {
            entity.ToTable("TB_PROFESION");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombPro)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Docente>(entity =>
        {
            entity.ToTable("TB_DOCENTE");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ApelDoc)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.NombDoc)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.DireDoc)
                .HasMaxLength(200);
            entity.Property(e => e.NtelDoc)
                .HasMaxLength(30);
            entity.Property(e => e.NcelDoc)
                .HasMaxLength(30);
            entity.Property(e => e.GradDoc)
                .HasMaxLength(100);
            entity.Property(e => e.IdProfesion)
                .IsRequired();
            entity.HasOne(e => e.Profesion)
                .WithMany(p => p.Docentes)
                .HasForeignKey(e => e.IdProfesion)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => e.IdProfesion);
        });

        modelBuilder.Entity<Curso>(entity =>
        {
            entity.ToTable("TB_CURSO");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombCur)
                .IsRequired()
                .HasMaxLength(150);
            entity.Property(e => e.CostCur)
                .IsRequired()
                .HasColumnType("decimal(10,2)");
            entity.Property(e => e.DuraCur)
                .IsRequired();
        });

        modelBuilder.Entity<Asignacion>(entity =>
        {
            entity.ToTable("TB_ASIGNACION");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FechAsi)
                .IsRequired();
            entity.Property(e => e.IdCurso)
                .IsRequired();
            entity.HasOne(e => e.Curso)
                .WithMany(c => c.Asignaciones)
                .HasForeignKey(e => e.IdCurso)
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(e => e.IdDocente)
                .IsRequired();
            entity.HasOne(e => e.Docente)
                .WithMany(d => d.Asignaciones)
                .HasForeignKey(e => e.IdDocente)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => e.IdCurso);
            entity.HasIndex(e => e.IdDocente);
        });

        modelBuilder.Entity<Matricula>(entity =>
        {
            entity.ToTable("TB_MATRICULA");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FechMat)
                .IsRequired();
            entity.Property(e => e.IdEstudiante)
                .IsRequired();
            entity.HasOne(e => e.Estudiante)
                .WithMany(e => e.Matriculas)
                .HasForeignKey(e => e.IdEstudiante)
                .OnDelete(DeleteBehavior.Restrict);
            entity.Property(e => e.IdCurso)
                .IsRequired();
            entity.HasOne(e => e.Curso)
                .WithMany(c => c.Matriculas)
                .HasForeignKey(e => e.IdCurso)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(e => e.IdEstudiante);
            entity.HasIndex(e => e.IdCurso);
        });

        ModelStoredProcedures(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("TB_USUARIO");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(256);
            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.FechaCreacion)
                .IsRequired();
            entity.HasIndex(e => e.Username).IsUnique();
        });

        SeedData(modelBuilder);
    }

    private static void ModelStoredProcedures(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EstudiantesPorProvincia>()
            .HasNoKey();

        modelBuilder.Entity<ProvinciaPorCurso>()
            .HasNoKey();
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                Id = 1,
                Username = "admin",
                PasswordHash = "240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9",
                Nombre = "Administrador",
                FechaCreacion = new DateTimeOffset(
                    2026, 5, 8,
                    12, 0, 0,
                    TimeSpan.Zero)
            }
        );
    }
}