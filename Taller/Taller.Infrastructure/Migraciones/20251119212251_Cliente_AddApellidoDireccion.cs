using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller.Infraestructura.Migraciones
{
    /// <inheritdoc />
    public partial class Cliente_AddApellidoDireccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_clientes_Nombre",
                table: "clientes");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "clientes",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(120)",
                oldMaxLength: 120);

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "clientes",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "clientes",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_clientes_Apellido_Nombre",
                table: "clientes",
                columns: new[] { "Apellido", "Nombre" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_clientes_Apellido_Nombre",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "clientes");

            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "clientes");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "clientes",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.CreateIndex(
                name: "IX_clientes_Nombre",
                table: "clientes",
                column: "Nombre");
        }
    }
}
