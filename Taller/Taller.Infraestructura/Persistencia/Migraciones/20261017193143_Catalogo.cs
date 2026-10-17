using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Taller.Infraestructura.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class Catalogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "Conceptos",
            columns: table => new
            {
                IdConcepto = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Tipo = table.Column<int>(type: "int", nullable: false), 
                Nombre = table.Column<string>(type: "nvarchar(150)", nullable: false), 
                Descripcion = table.Column<string>(type: "nvarchar(500)", nullable: true),
                PrecioReferencia = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                Activo = table.Column<bool>(type: "bit", nullable: false),
                IdCategoria = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Conceptos", x => x.IdConcepto);
                table.ForeignKey(
                    name: "FK_Conceptos_Categorias_IdCategoria",
                    column: x => x.IdCategoria,
                    principalTable: "Categorias",
                    principalColumn: "IdCategoria"); 
            });

            migrationBuilder.CreateIndex(
                name: "IX_Conceptos_IdCategoria",
                table: "Conceptos",
                column: "IdCategoria");

            migrationBuilder.CreateTable(
            name: "Categorias",
            columns: table => new
            {
                IdCategoria = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Nombre = table.Column<string>(type: "nvarchar(100)", nullable: false),
                Descripcion = table.Column<string>(type: "nvarchar(250)", nullable: true),
                Activo = table.Column<bool>(type: "bit", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Categorias", x => x.IdCategoria);
            });
        }                            

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                        name: "Conceptos");

            migrationBuilder.DropTable(
            name: "Categorias");
        }
    }
}