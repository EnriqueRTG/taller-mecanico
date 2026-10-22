using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller.Infraestructura.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class AgregarDisponibilidadServiciosIngreso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DisponibleParaIngreso",
                table: "Conceptos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Conceptos_DisponibleParaIngreso",
                table: "Conceptos",
                sql: "[DisponibleParaIngreso] = 0\r\nOR [Tipo] = N'Servicio'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Conceptos_DisponibleParaIngreso",
                table: "Conceptos");

            migrationBuilder.DropColumn(
                name: "DisponibleParaIngreso",
                table: "Conceptos");
        }
    }
}
