using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JuezSeguro.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddExpedientesYMensajesChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Expedientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroExpediente = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Materia = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaApertura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HashIntegridad = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    FirmaDigital = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    CreadoPorId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ModificadoPorId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expedientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MensajesChat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Texto = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpedienteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MensajesChat", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Expedientes");

            migrationBuilder.DropTable(
                name: "MensajesChat");
        }
    }
}
