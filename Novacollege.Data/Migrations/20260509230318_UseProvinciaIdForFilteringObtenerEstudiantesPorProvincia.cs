using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Novacollege.Data.Migrations
{
    /// <inheritdoc />
    public partial class UseProvinciaIdForFilteringObtenerEstudiantesPorProvincia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
@"
EXEC('
DROP PROCEDURE sp_ObtenerEstudiantesPorProvincia
')
");

            migrationBuilder.Sql(
@"
EXEC('
CREATE PROCEDURE sp_ObtenerEstudiantesPorProvincia
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.NombPro AS Provincia,
        COUNT(e.Id) AS NumeroEstudiantes
    FROM TB_PROVINCIA p
    LEFT JOIN TB_DISTRITO d
        ON d.IdProvincia = p.Id
    LEFT JOIN TB_ESTUDIANTE e
        ON e.IdDistrito = d.Id
    GROUP BY
        p.NombPro
    ORDER BY
        NumeroEstudiantes DESC;
END
')
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
@"
EXEC('
DROP PROCEDURE sp_ObtenerEstudiantesPorProvincia
')
");

            migrationBuilder.Sql(
@"
EXEC('
CREATE PROCEDURE sp_ObtenerEstudiantesPorProvincia
    @NombreProvincia NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.NombPro AS Provincia,
        COUNT(e.Id) AS NumeroEstudiantes
    FROM TB_PROVINCIA p
    LEFT JOIN TB_DISTRITO d
        ON d.IdProvincia = p.Id
    LEFT JOIN TB_ESTUDIANTE e
        ON e.IdDistrito = d.Id
    WHERE
        @NombreProvincia IS NULL
        OR p.NombPro = @NombreProvincia
    GROUP BY
        p.NombPro
    ORDER BY
        NumeroEstudiantes DESC;
END
')
");
        }
    }
}
