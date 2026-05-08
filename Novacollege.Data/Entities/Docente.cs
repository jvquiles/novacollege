namespace Novacollege.Data.Entities;

public class Docente
{
    public int Id { get; set; }

    public string ApelDoc { get; set; } = string.Empty;

    public string NombDoc { get; set; } = string.Empty;

    public string? DireDoc { get; set; }

    public string? NtelDoc { get; set; }

    public string? NcelDoc { get; set; }

    public string? GradDoc { get; set; }

    public int IdProfesion { get; set; }

    public Profesion? Profesion { get; set; } = null!;

    public ICollection<Asignacion> Asignaciones { get; set; } = [];
}