using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Novacollege.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEstudianteFnacEstAddProvinciaValenciaAndDistrito1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "FnacEst",
                table: "TB_ESTUDIANTE",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.InsertData(
                table: "TB_PROVINCIA",
                columns: new[] { "Id", "NombPro" },
                values: new object[] { 1, "Valencia" });

            migrationBuilder.InsertData(
                table: "TB_DISTRITO",
                columns: new[] { "Id", "IdProvincia", "NombDis" },
                values: new object[] { 1, 1, "Distrito 1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TB_DISTRITO",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TB_PROVINCIA",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "FnacEst",
                table: "TB_ESTUDIANTE",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
