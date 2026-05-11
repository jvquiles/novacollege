namespace Novacollege.Data.Entities;

public class Matricula
{
    public int Id { get; set; }

    public DateTimeOffset FechMat { get; set; }

    public int IdEstudiante { get; set; }

    public Estudiante Estudiante { get; set; } = null!;

    public int IdCurso { get; set; }

    public Curso? Curso { get; set; } = null!;
}