namespace Novacollege.Data.Entities;

public class Distrito
{
    public int Id { get; set; }

    public string NombDis { get; set; } = string.Empty;

    public int IdProvincia { get; set; }

    public Provincia? Provincia { get; set; } = null!;

    public ICollection<Estudiante> Estudiantes { get; set; } = [];
}