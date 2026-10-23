using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller.Infraestructura.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class SeguridadAccesoPersistente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ControlesAcceso",
                columns: table => new
                {
                    Clave = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    Intentos = table.Column<int>(type: "int", nullable: false),
                    VentanaUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    BloqueadoHastaUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlesAcceso", x => x.Clave);
                });

            migrationBuilder.CreateTable(
                name: "EventosAcceso",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Clave = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: true),
                    Correcto = table.Column<bool>(type: "bit", nullable: false),
                    FechaUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosAcceso", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventosAcceso_FechaUtc",
                table: "EventosAcceso",
                column: "FechaUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ControlesAcceso");

            migrationBuilder.DropTable(
                name: "EventosAcceso");
        }
    }
}
