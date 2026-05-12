namespace Novacollege.WebApi.Dtos;

public class CursoRespone
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public ProvinciaResponse[] Provincias { get; set; } = [];
}