using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller.Infraestructura.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class CircuitoAtencionEvaluacionPropuesta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Presupuestos_Diagnosticos_IdDiagnostico",
                table: "Presupuestos");

            migrationBuilder.DropIndex(
                name: "UX_Presupuestos_DiagnosticoPorAtencion",
                table: "Presupuestos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Presupuestos_Decision",
                table: "Presupuestos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Presupuestos_Estado",
                table: "Presupuestos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Presupuestos_Origen",
                table: "Presupuestos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Presupuestos_Presentacion",
                table: "Presupuestos");

            // El modelo anterior guardaba la atención del presupuesto de reparación
            // solamente a través del diagnóstico. Recuperamos la FK antes de exigirla.
            migrationBuilder.Sql("""
                UPDATE p SET IdAtencion = d.IdAtencion
                FROM Presupuestos p
                INNER JOIN Diagnosticos d ON d.IdDiagnostico = p.IdDiagnostico
                WHERE p.IdAtencion IS NULL;
                IF EXISTS (SELECT 1 FROM Presupuestos WHERE IdAtencion IS NULL)
                    THROW 51000, 'Hay presupuestos sin atención recuperable. Revise esos registros antes de migrar.', 1;
                """);

            migrationBuilder.AlterColumn<int>(
                name: "IdAtencion",
                table: "Presupuestos",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoSolicitud",
                table: "Atenciones",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Problema");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Diagnosticos_IdDiagnostico_IdAtencion",
                table: "Diagnosticos",
                columns: new[] { "IdDiagnostico", "IdAtencion" });

            migrationBuilder.CreateTable(
                name: "EvaluacionesAtencion",
                columns: table => new
                {
                    IdAtencion = table.Column<int>(type: "int", nullable: false),
                    IncluyeDiagnostico = table.Column<bool>(type: "bit", nullable: false),
                    FallaDetectada = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    TrabajoRecomendado = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Kilometraje = table.Column<int>(type: "int", nullable: true),
                    SoloEvaluacion = table.Column<bool>(type: "bit", nullable: false),
                    Finalizada = table.Column<bool>(type: "bit", nullable: false),
                    IdTecnicoUltimaEdicion = table.Column<int>(type: "int", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvaluacionesAtencion", x => x.IdAtencion);
                    table.CheckConstraint("CK_EvaluacionesAtencion_Kilometraje", "[Kilometraje] IS NULL OR [Kilometraje] >= 0");
                    table.ForeignKey(
                        name: "FK_EvaluacionesAtencion_Atenciones_IdAtencion",
                        column: x => x.IdAtencion,
                        principalTable: "Atenciones",
                        principalColumn: "IdAtencion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvaluacionesAtencion_Usuarios_IdTecnicoUltimaEdicion",
                        column: x => x.IdTecnicoUltimaEdicion,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            // Una propuesta histórica confirmada ya no debe ofrecerse como trabajo disponible.
            migrationBuilder.Sql("""
                INSERT INTO EvaluacionesAtencion
                    (IdAtencion, IncluyeDiagnostico, FallaDetectada, TrabajoRecomendado,
                     Observaciones, Kilometraje, SoloEvaluacion, Finalizada,
                     IdTecnicoUltimaEdicion, FechaActualizacion)
                SELECT d.IdAtencion, 1, d.FallaDetectada, d.TrabajoRecomendado,
                       d.Observaciones, d.Kilometraje, 0, 1, d.IdTecnicoAutor, d.FechaDiagnostico
                FROM Diagnosticos d
                WHERE d.Tipo = N'Inicial'
                  AND EXISTS (SELECT 1 FROM Presupuestos p
                              WHERE p.IdAtencion = d.IdAtencion AND p.Tipo = N'Reparacion')
                  AND NOT EXISTS (SELECT 1 FROM EvaluacionesAtencion e WHERE e.IdAtencion = d.IdAtencion);
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_Atencion",
                table: "Presupuestos",
                column: "IdAtencion");

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_IdDiagnostico_IdAtencion",
                table: "Presupuestos",
                columns: new[] { "IdDiagnostico", "IdAtencion" });

            migrationBuilder.CreateIndex(
                name: "UX_Presupuestos_BorradorPorAtencion",
                table: "Presupuestos",
                column: "IdAtencion",
                unique: true,
                filter: "[Tipo] = N'Reparacion' AND [Estado] = N'Borrador'");

            migrationBuilder.CreateIndex(
                name: "UX_Presupuestos_DiagnosticoPorAtencion",
                table: "Presupuestos",
                column: "IdAtencion",
                unique: true,
                filter: "[Tipo] = N'Diagnostico'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Presupuestos_Decision",
                table: "Presupuestos",
                sql: "(\n    [Estado] IN (\n        N'Borrador',\n    N'PendientePresentacion',\n        N'PendienteDecision'\n    )\n    AND [IdUsuarioDecision] IS NULL\n    AND [FechaDecision] IS NULL\n)\nOR\n(\n    [Estado] IN (N'Aceptado', N'Rechazado')\n    AND [IdUsuarioDecision] IS NOT NULL\n    AND [FechaDecision] IS NOT NULL\n)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Presupuestos_Estado",
                table: "Presupuestos",
                sql: "[Estado] IN (\n    N'Borrador',\n    N'PendientePresentacion',\n    N'PendienteDecision',\n    N'Aceptado',\n    N'Rechazado'\n)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Presupuestos_Origen",
                table: "Presupuestos",
                sql: "[Tipo] IN (N'Diagnostico', N'Reparacion') AND ([Tipo] <> N'Diagnostico' OR [IdDiagnostico] IS NULL)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Presupuestos_Presentacion",
                table: "Presupuestos",
                sql: "(\n    [Estado] IN (N'Borrador', N'PendientePresentacion')\n    AND [FechaPresentacion] IS NULL\n)\nOR\n(\n    [Estado] IN (\n        N'PendienteDecision',\n        N'Aceptado',\n        N'Rechazado'\n    )\n    AND [FechaPresentacion] IS NOT NULL\n)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Atenciones_TipoSolicitud",
                table: "Atenciones",
                sql: "[TipoSolicitud] IN (N'Problema', N'ServicioDefinido', N'Mixta')");

            migrationBuilder.CreateIndex(
                name: "IX_EvaluacionesAtencion_IdTecnicoUltimaEdicion",
                table: "EvaluacionesAtencion",
                column: "IdTecnicoUltimaEdicion");

            migrationBuilder.AddForeignKey(
                name: "FK_Presupuestos_Diagnosticos_IdDiagnostico_IdAtencion",
                table: "Presupuestos",
                columns: new[] { "IdDiagnostico", "IdAtencion" },
                principalTable: "Diagnosticos",
                principalColumns: new[] { "IdDiagnostico", "IdAtencion" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // El esquema anterior no representa borradores ni propuestas sin diagnóstico.
            // No descartamos avances o clasificaciones silenciosamente al retroceder.
            migrationBuilder.Sql("""
                IF EXISTS (SELECT 1 FROM EvaluacionesAtencion)
                   OR EXISTS (SELECT 1 FROM Atenciones WHERE TipoSolicitud <> N'Problema')
                   OR EXISTS (SELECT 1 FROM Presupuestos WHERE Estado = N'Borrador'
                              OR (Tipo = N'Reparacion' AND IdDiagnostico IS NULL))
                    THROW 51001, 'La nueva funcionalidad contiene datos. Restaure el respaldo previo para regresar sin perder información.', 1;
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_Presupuestos_Diagnosticos_IdDiagnostico_IdAtencion",
                table: "Presupuestos");

            migrationBuilder.DropTable(
                name: "EvaluacionesAtencion");

            migrationBuilder.DropIndex(
                name: "IX_Presupuestos_Atencion",
                table: "Presupuestos");

            migrationBuilder.DropIndex(
                name: "IX_Presupuestos_IdDiagnostico_IdAtencion",
                table: "Presupuestos");

            migrationBuilder.DropIndex(
                name: "UX_Presupuestos_BorradorPorAtencion",
                table: "Presupuestos");

            migrationBuilder.DropIndex(
                name: "UX_Presupuestos_DiagnosticoPorAtencion",
                table: "Presupuestos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Presupuestos_Decision",
                table: "Presupuestos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Presupuestos_Estado",
                table: "Presupuestos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Presupuestos_Origen",
                table: "Presupuestos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Presupuestos_Presentacion",
                table: "Presupuestos");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Diagnosticos_IdDiagnostico_IdAtencion",
                table: "Diagnosticos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Atenciones_TipoSolicitud",
                table: "Atenciones");

            migrationBuilder.DropColumn(
                name: "TipoSolicitud",
                table: "Atenciones");

            migrationBuilder.AlterColumn<int>(
                name: "IdAtencion",
                table: "Presupuestos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.Sql("UPDATE Presupuestos SET IdAtencion = NULL WHERE Tipo = N'Reparacion';");

            migrationBuilder.CreateIndex(
                name: "UX_Presupuestos_DiagnosticoPorAtencion",
                table: "Presupuestos",
                column: "IdAtencion",
                unique: true,
                filter: "[IdAtencion] IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Presupuestos_Decision",
                table: "Presupuestos",
                sql: "(\r\n    [Estado] IN (\r\n        N'PendientePresentacion',\r\n        N'PendienteDecision'\r\n    )\r\n    AND [IdUsuarioDecision] IS NULL\r\n    AND [FechaDecision] IS NULL\r\n)\r\nOR\r\n(\r\n    [Estado] IN (N'Aceptado', N'Rechazado')\r\n    AND [IdUsuarioDecision] IS NOT NULL\r\n    AND [FechaDecision] IS NOT NULL\r\n)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Presupuestos_Estado",
                table: "Presupuestos",
                sql: "[Estado] IN (\r\n    N'PendientePresentacion',\r\n    N'PendienteDecision',\r\n    N'Aceptado',\r\n    N'Rechazado'\r\n)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Presupuestos_Origen",
                table: "Presupuestos",
                sql: "(\r\n    [Tipo] = N'Diagnostico'\r\n    AND [IdAtencion] IS NOT NULL\r\n    AND [IdDiagnostico] IS NULL\r\n)\r\nOR\r\n(\r\n    [Tipo] = N'Reparacion'\r\n    AND [IdAtencion] IS NULL\r\n    AND [IdDiagnostico] IS NOT NULL\r\n)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Presupuestos_Presentacion",
                table: "Presupuestos",
                sql: "(\r\n    [Estado] = N'PendientePresentacion'\r\n    AND [FechaPresentacion] IS NULL\r\n)\r\nOR\r\n(\r\n    [Estado] IN (\r\n        N'PendienteDecision',\r\n        N'Aceptado',\r\n        N'Rechazado'\r\n    )\r\n    AND [FechaPresentacion] IS NOT NULL\r\n)");

            migrationBuilder.AddForeignKey(
                name: "FK_Presupuestos_Diagnosticos_IdDiagnostico",
                table: "Presupuestos",
                column: "IdDiagnostico",
                principalTable: "Diagnosticos",
                principalColumn: "IdDiagnostico",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
