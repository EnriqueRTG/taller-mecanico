using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller.Infraestructura.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class AgregarUnicidadConceptos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "UX_Conceptos_Categoria_Tipo_Nombre",
                table: "Conceptos",
                columns: new[] { "IdCategoria", "Tipo", "Nombre" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Conceptos_Categoria_Tipo_Nombre",
                table: "Conceptos");
        }
    }
}
