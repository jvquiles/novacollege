using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Novacollege.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProvinciaPorCurso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"
EXEC('
CREATE PROCEDURE sp_ProvinciaConMasEstudiantesPorCurso
    @IdCurso INT
AS
BEGIN
    SET NOCOUNT ON;

    WITH ProvinciasAgrupadas AS
    (
        SELECT
            p.NombPro AS Provincia,
            COUNT(e.Id) AS NumeroEstudiantes
        FROM TB_MATRICULA m
        INNER JOIN TB_ESTUDIANTE e
            ON e.Id = m.IdEstudiante
        INNER JOIN TB_DISTRITO d
            ON d.Id = e.IdDistrito
        INNER JOIN TB_PROVINCIA p
            ON p.Id = d.IdProvincia
        WHERE m.IdCurso = @IdCurso
        GROUP BY p.NombPro
    )
    SELECT *
    FROM ProvinciasAgrupadas
    WHERE NumeroEstudiantes =
    (
        SELECT MAX(NumeroEstudiantes)
        FROM ProvinciasAgrupadas
    );
END;
')
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"
EXEC('
DROP PROCEDURE sp_ProvinciaConMasEstudiantesPorCurso
')
");
        }
    }
}