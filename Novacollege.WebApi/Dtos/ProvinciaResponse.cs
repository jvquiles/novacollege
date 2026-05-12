namespace Novacollege.WebApi.Dtos;

public class ProvinciaResponse
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public EstudianteResponse[] Estudiantes { get; set; } = [];
}