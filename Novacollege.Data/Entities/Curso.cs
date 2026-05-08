namespace Novacollege.Data.Entities;

public class Curso
{
    public int Id { get; set; }

    public string NombCur { get; set; } = string.Empty;

    public decimal CostCur { get; set; }

    public int DuraCur { get; set; }

    public ICollection<Asignacion> Asignaciones { get; set; } = [];

    public ICollection<Matricula> Matriculas { get; set; } = [];
}