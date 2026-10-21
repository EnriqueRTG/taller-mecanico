using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller.Infraestructura.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class HabilitarServiciosInicialesIngreso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE c
                SET c.[DisponibleParaIngreso] = 1
                FROM [Conceptos] AS c
                INNER JOIN [Categorias] AS cat
                    ON cat.[IdCategoria] = c.[IdCategoria]
                WHERE cat.[Nombre] = N'Diagnóstico e inspección'
                  AND c.[Tipo] = N'Servicio'
                  AND c.[Nombre] IN (
                      N'Ingreso e inspección diagnóstica inicial',
                      N'Diagnóstico electrónico',
                      N'Inspección del sistema de frenos',
                      N'Inspección del sistema de refrigeración'
                  );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                UPDATE c
                SET c.[DisponibleParaIngreso] = 0
                FROM [Conceptos] AS c
                INNER JOIN [Categorias] AS cat
                    ON cat.[IdCategoria] = c.[IdCategoria]
                WHERE cat.[Nombre] = N'Diagnóstico e inspección'
                  AND c.[Tipo] = N'Servicio'
                  AND c.[Nombre] IN (
                      N'Ingreso e inspección diagnóstica inicial',
                      N'Diagnóstico electrónico',
                      N'Inspección del sistema de frenos',
                      N'Inspección del sistema de refrigeración'
                  );
                """);
        }
    }
}
