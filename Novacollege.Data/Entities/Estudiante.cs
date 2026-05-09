namespace Novacollege.Data.Entities;

public class Estudiante
{
    public int Id { get; set; }

    public string ApelEst { get; set; } = string.Empty;

    public string NombEst { get; set; } = string.Empty;

    public DateOnly? FnacEst { get; set; }

    public string? SexoEst { get; set; }

    public string? DireEst { get; set; }

    public string? TcolEst { get; set; }

    public DateTime? GinsEst { get; set; }

    public int IdDistrito { get; set; }

    public Distrito? Distrito { get; set; } = null!;

    public ICollection<Matricula> Matriculas { get; set; } = [];
}