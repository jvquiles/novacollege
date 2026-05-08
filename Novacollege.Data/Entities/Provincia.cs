namespace Novacollege.Data.Entities;

public class Provincia
{
    public int Id { get; set; }

    public string NombPro { get; set; } = string.Empty;

    public ICollection<Distrito> Distritos { get; set; } = [];
}