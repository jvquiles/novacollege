namespace Novacollege.Data.Entities;

public class Asignacion
{
    public int Id { get; set; }

    public DateTime FechAsi { get; set; }

    public int IdCurso { get; set; }

    public Curso Curso { get; set; } = null!;

    public int IdDocente { get; set; }

    public Docente Docente { get; set; } = null!;
}