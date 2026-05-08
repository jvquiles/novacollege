using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Novacollege.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_CURSO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombCur = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CostCur = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DuraCur = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CURSO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_PROFESION",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombPro = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PROFESION", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_PROVINCIA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombPro = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_PROVINCIA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_DOCENTE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApelDoc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NombDoc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DireDoc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NtelDoc = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    NcelDoc = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    GradDoc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IdProfesion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_DOCENTE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_DOCENTE_TB_PROFESION_IdProfesion",
                        column: x => x.IdProfesion,
                        principalTable: "TB_PROFESION",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_DISTRITO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombDis = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IdProvincia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_DISTRITO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_DISTRITO_TB_PROVINCIA_IdProvincia",
                        column: x => x.IdProvincia,
                        principalTable: "TB_PROVINCIA",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_ASIGNACION",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechAsi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdCurso = table.Column<int>(type: "int", nullable: false),
                    IdDocente = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ASIGNACION", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_ASIGNACION_TB_CURSO_IdCurso",
                        column: x => x.IdCurso,
                        principalTable: "TB_CURSO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_ASIGNACION_TB_DOCENTE_IdDocente",
                        column: x => x.IdDocente,
                        principalTable: "TB_DOCENTE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_ESTUDIANTE",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApelEst = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NombEst = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FnacEst = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SexoEst = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    DireEst = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TcolEst = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    GinsEst = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdDistrito = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_ESTUDIANTE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_ESTUDIANTE_TB_DISTRITO_IdDistrito",
                        column: x => x.IdDistrito,
                        principalTable: "TB_DISTRITO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_MATRICULA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechMat = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IdEstudiante = table.Column<int>(type: "int", nullable: false),
                    IdCurso = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MATRICULA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_MATRICULA_TB_CURSO_IdCurso",
                        column: x => x.IdCurso,
                        principalTable: "TB_CURSO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_MATRICULA_TB_ESTUDIANTE_IdEstudiante",
                        column: x => x.IdEstudiante,
                        principalTable: "TB_ESTUDIANTE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_ASIGNACION_IdCurso",
                table: "TB_ASIGNACION",
                column: "IdCurso");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ASIGNACION_IdDocente",
                table: "TB_ASIGNACION",
                column: "IdDocente");

            migrationBuilder.CreateIndex(
                name: "IX_TB_DISTRITO_IdProvincia",
                table: "TB_DISTRITO",
                column: "IdProvincia");

            migrationBuilder.CreateIndex(
                name: "IX_TB_DOCENTE_IdProfesion",
                table: "TB_DOCENTE",
                column: "IdProfesion");

            migrationBuilder.CreateIndex(
                name: "IX_TB_ESTUDIANTE_IdDistrito",
                table: "TB_ESTUDIANTE",
                column: "IdDistrito");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MATRICULA_IdCurso",
                table: "TB_MATRICULA",
                column: "IdCurso");

            migrationBuilder.CreateIndex(
                name: "IX_TB_MATRICULA_IdEstudiante",
                table: "TB_MATRICULA",
                column: "IdEstudiante");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_ASIGNACION");

            migrationBuilder.DropTable(
                name: "TB_MATRICULA");

            migrationBuilder.DropTable(
                name: "TB_DOCENTE");

            migrationBuilder.DropTable(
                name: "TB_CURSO");

            migrationBuilder.DropTable(
                name: "TB_ESTUDIANTE");

            migrationBuilder.DropTable(
                name: "TB_PROFESION");

            migrationBuilder.DropTable(
                name: "TB_DISTRITO");

            migrationBuilder.DropTable(
                name: "TB_PROVINCIA");
        }
    }
}
