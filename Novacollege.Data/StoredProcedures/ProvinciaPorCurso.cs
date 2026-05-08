namespace Novacollege.Data.StoredProcedures;

public record ProvinciaPorCurso
{
    public required string Provincia { get; set; }
    public required int NumeroEstudiantes { get; set; }
}