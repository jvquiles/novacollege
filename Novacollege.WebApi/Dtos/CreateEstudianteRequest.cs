using System.ComponentModel.DataAnnotations;

namespace Novacollege.WebApi.Dtos;

public class CreateEstudianteRequest
{
    [Required]
    [MaxLength(100)]
    public required string ApelEst { get; init; }

    [Required]
    [MaxLength(100)]
    public required string NombEst { get; init; }

    public DateOnly? FnacEst { get; init; }

    [MaxLength(1)]
    public string? SexoEst { get; init; }

    [MaxLength(200)]
    public string? DireEst { get; init; }

    [MaxLength(30)]
    public string? TcolEst { get; init; }

    public DateTime? GinsEst { get; init; }

    [Required]
    public required int IdDistrito { get; init; }
}