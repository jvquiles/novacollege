namespace Novacollege.WebApi.Dtos;

public class DocenteCursoProvinciaDto
{
    public int IdDocente { get; init; }
    public required string ApelDoc { get; init; }
    public required string NombDoc { get; init; }
    public required string NombreCurso { get; init; }
    public List<ProvinciaEstudiantesDto> Provincias { get; init; } = [];
}

public class ProvinciaEstudiantesDto
{
    public required string NombreProvincia { get; init; }
    public int NumeroEstudiantes { get; init; }
    public List<EstudianteInfoDto> Estudiantes { get; init; } = [];
}

public class EstudianteInfoDto
{
    public int Id { get; init; }
    public required string ApelEst { get; init; }
    public required string NombEst { get; init; }
}