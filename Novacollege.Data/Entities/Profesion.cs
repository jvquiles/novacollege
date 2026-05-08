namespace Novacollege.Data.Entities;

public class Profesion
{
    public int Id { get; set; }

    public string NombPro { get; set; } = string.Empty;

    public ICollection<Docente> Docentes { get; set; } = [];
}