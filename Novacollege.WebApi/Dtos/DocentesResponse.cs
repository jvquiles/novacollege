using Novacollege.Data.Entities;

namespace Novacollege.WebApi.Dtos;

public class DocentesResponse
{
    public int Id { get; set; }
    public string NombDoc { get; set; }
    public string ApelDoc { get; set; }
    public CursoRespone[] Cursos { get; set; } = [];
}