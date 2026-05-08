namespace Novacollege.Data.StoredProcedures;

public record EstudiantesPorProvincia
{
    public required string Provincia { get; init; }

    public required int NumeroEstudiantes { get; init; }
}