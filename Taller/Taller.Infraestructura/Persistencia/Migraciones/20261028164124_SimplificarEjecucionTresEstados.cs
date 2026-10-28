using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller.Infraestructura.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class SimplificarEjecucionTresEstados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // No reinterpretar una omisión ni un cierre parcial como trabajo completado.
            // Las instalaciones Demo con esos ejemplos deben preparar el lote v4 en una base vacía.
            migrationBuilder.Sql("""
                IF EXISTS (SELECT 1 FROM dbo.PresupuestoDetalles WHERE EstadoEjecucion IN (N'Parcial', N'Omitido'))
                    THROW 51140, N'Hay ejecuciones Parcial/Omitido del alcance anterior. No se modificaron. Para demostración prepare SGTM_DB_Demo vacía con el lote v4; para datos reales solicite revisión técnica antes de migrar.', 1;
                """);

            migrationBuilder.DropCheckConstraint(
                name: "CK_PresupuestoDetalles_Avance",
                table: "PresupuestoDetalles");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PresupuestoDetalles_EstadoEjecucion",
                table: "PresupuestoDetalles");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PresupuestoDetalles_Justificacion",
                table: "PresupuestoDetalles");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PresupuestoDetalles_Avance",
                table: "PresupuestoDetalles",
                sql: "(\n    [EstadoEjecucion] = N'NoIniciado'\n    AND [CantidadEjecutada] = 0\n)\nOR\n(\n    [EstadoEjecucion] = N'EnCurso'\n    AND [CantidadEjecutada] < [Cantidad]\n)\nOR\n(\n    [EstadoEjecucion] = N'Completado'\n    AND [CantidadEjecutada] = [Cantidad]\n)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PresupuestoDetalles_EstadoEjecucion",
                table: "PresupuestoDetalles",
                sql: "[EstadoEjecucion] IN (\n    N'NoIniciado',\n    N'EnCurso',\n    N'Completado'\n)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_PresupuestoDetalles_Avance",
                table: "PresupuestoDetalles");

            migrationBuilder.DropCheckConstraint(
                name: "CK_PresupuestoDetalles_EstadoEjecucion",
                table: "PresupuestoDetalles");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PresupuestoDetalles_Avance",
                table: "PresupuestoDetalles",
                sql: "(\n    [EstadoEjecucion] IN (N'NoIniciado', N'Omitido')\n    AND [CantidadEjecutada] = 0\n)\nOR\n(\n    [EstadoEjecucion] IN (N'EnCurso', N'Parcial')\n    AND [CantidadEjecutada] < [Cantidad]\n)\nOR\n(\n    [EstadoEjecucion] = N'Completado'\n    AND [CantidadEjecutada] = [Cantidad]\n)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PresupuestoDetalles_EstadoEjecucion",
                table: "PresupuestoDetalles",
                sql: "[EstadoEjecucion] IN (\n    N'NoIniciado',\n    N'EnCurso',\n    N'Parcial',\n    N'Completado',\n    N'Omitido'\n)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_PresupuestoDetalles_Justificacion",
                table: "PresupuestoDetalles",
                sql: "[EstadoEjecucion] NOT IN (N'Parcial', N'Omitido')\nOR (\n    [ObservacionesEjecucion] IS NOT NULL\n    AND LEN(LTRIM(RTRIM([ObservacionesEjecucion]))) > 0\n)");
        }
    }
}
