namespace Novacollege.WebApi.Dtos;

public class CreateEstudianteResponse
{
    public int Id { get; init; }
    public required string ApelEst { get; init; }
    public required string NombEst { get; init; }
    public DateOnly? FnacEst { get; init; }
    public string? SexoEst { get; init; }
    public string? DireEst { get; init; }
    public string? TcolEst { get; init; }
    public DateTime? GinsEst { get; init; }
    public int IdDistrito { get; init; }
}