using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller.Infraestructura.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class AgregarAtenciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaAlta",
                table: "Usuarios",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.CreateTable(
                name: "Atenciones",
                columns: table => new
                {
                    IdAtencion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdVehiculo = table.Column<int>(type: "int", nullable: false),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioRecepcion = table.Column<int>(type: "int", nullable: false),
                    FechaApertura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MotivoConsulta = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atenciones", x => x.IdAtencion);
                    table.ForeignKey(
                        name: "FK_Atenciones_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Atenciones_Usuarios_IdUsuarioRecepcion",
                        column: x => x.IdUsuarioRecepcion,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Atenciones_Vehiculos_IdVehiculo",
                        column: x => x.IdVehiculo,
                        principalTable: "Vehiculos",
                        principalColumn: "IdVehiculo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atenciones_IdCliente",
                table: "Atenciones",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Atenciones_IdUsuarioRecepcion",
                table: "Atenciones",
                column: "IdUsuarioRecepcion");

            migrationBuilder.CreateIndex(
                name: "IX_Atenciones_IdVehiculo",
                table: "Atenciones",
                column: "IdVehiculo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Atenciones");

            migrationBuilder.DropColumn(
                name: "FechaAlta",
                table: "Usuarios");
        }
    }
}
