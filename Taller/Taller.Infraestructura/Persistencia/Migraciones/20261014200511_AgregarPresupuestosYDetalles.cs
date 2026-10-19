using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller.Infraestructura.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class AgregarPresupuestosYDetalles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Presupuestos",
                columns: table => new
                {
                    IdPresupuesto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IdAtencion = table.Column<int>(type: "int", nullable: true),
                    IdDiagnostico = table.Column<int>(type: "int", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FechaPresentacion = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    IdUsuarioDecision = table.Column<int>(type: "int", nullable: true),
                    FechaDecision = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presupuestos", x => x.IdPresupuesto);
                    table.CheckConstraint("CK_Presupuestos_Decision", "(\r\n    [Estado] IN (\r\n        N'PendientePresentacion',\r\n        N'PendienteDecision'\r\n    )\r\n    AND [IdUsuarioDecision] IS NULL\r\n    AND [FechaDecision] IS NULL\r\n)\r\nOR\r\n(\r\n    [Estado] IN (N'Aceptado', N'Rechazado')\r\n    AND [IdUsuarioDecision] IS NOT NULL\r\n    AND [FechaDecision] IS NOT NULL\r\n)");
                    table.CheckConstraint("CK_Presupuestos_DiagnosticoAceptado", "[Tipo] <> N'Diagnostico' OR [Estado] = N'Aceptado'");
                    table.CheckConstraint("CK_Presupuestos_Estado", "[Estado] IN (\r\n    N'PendientePresentacion',\r\n    N'PendienteDecision',\r\n    N'Aceptado',\r\n    N'Rechazado'\r\n)");
                    table.CheckConstraint("CK_Presupuestos_Fechas", "(\r\n    [FechaPresentacion] IS NULL\r\n    OR [FechaPresentacion] >= [FechaCreacion]\r\n)\r\nAND\r\n(\r\n    [FechaDecision] IS NULL\r\n    OR (\r\n        [FechaPresentacion] IS NOT NULL\r\n        AND [FechaDecision] >= [FechaPresentacion]\r\n    )\r\n)");
                    table.CheckConstraint("CK_Presupuestos_Origen", "(\r\n    [Tipo] = N'Diagnostico'\r\n    AND [IdAtencion] IS NOT NULL\r\n    AND [IdDiagnostico] IS NULL\r\n)\r\nOR\r\n(\r\n    [Tipo] = N'Reparacion'\r\n    AND [IdAtencion] IS NULL\r\n    AND [IdDiagnostico] IS NOT NULL\r\n)");
                    table.CheckConstraint("CK_Presupuestos_Presentacion", "(\r\n    [Estado] = N'PendientePresentacion'\r\n    AND [FechaPresentacion] IS NULL\r\n)\r\nOR\r\n(\r\n    [Estado] IN (\r\n        N'PendienteDecision',\r\n        N'Aceptado',\r\n        N'Rechazado'\r\n    )\r\n    AND [FechaPresentacion] IS NOT NULL\r\n)");
                    table.ForeignKey(
                        name: "FK_Presupuestos_Atenciones_IdAtencion",
                        column: x => x.IdAtencion,
                        principalTable: "Atenciones",
                        principalColumn: "IdAtencion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Presupuestos_Diagnosticos_IdDiagnostico",
                        column: x => x.IdDiagnostico,
                        principalTable: "Diagnosticos",
                        principalColumn: "IdDiagnostico",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Presupuestos_Usuarios_IdUsuarioCreacion",
                        column: x => x.IdUsuarioCreacion,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Presupuestos_Usuarios_IdUsuarioDecision",
                        column: x => x.IdUsuarioDecision,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PresupuestoDetalles",
                columns: table => new
                {
                    IdPresupuestoDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPresupuesto = table.Column<int>(type: "int", nullable: false),
                    IdConcepto = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    EstadoEjecucion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CantidadEjecutada = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    IdTecnicoRegistro = table.Column<int>(type: "int", nullable: true),
                    FechaRegistroEjecucion = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    ObservacionesEjecucion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IdPresupuestoCambio = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresupuestoDetalles", x => x.IdPresupuestoDetalle);
                    table.CheckConstraint("CK_PresupuestoDetalles_Avance", "(\r\n    [EstadoEjecucion] IN (N'NoIniciado', N'Omitido')\r\n    AND [CantidadEjecutada] = 0\r\n)\r\nOR\r\n(\r\n    [EstadoEjecucion] IN (N'EnCurso', N'Parcial')\r\n    AND [CantidadEjecutada] < [Cantidad]\r\n)\r\nOR\r\n(\r\n    [EstadoEjecucion] = N'Completado'\r\n    AND [CantidadEjecutada] = [Cantidad]\r\n)");
                    table.CheckConstraint("CK_PresupuestoDetalles_Cantidad", "[Cantidad] > 0");
                    table.CheckConstraint("CK_PresupuestoDetalles_CantidadEjecutada", "[CantidadEjecutada] >= 0\r\nAND [CantidadEjecutada] <= [Cantidad]");
                    table.CheckConstraint("CK_PresupuestoDetalles_Descripcion", "LEN(LTRIM(RTRIM([Descripcion]))) > 0");
                    table.CheckConstraint("CK_PresupuestoDetalles_EstadoEjecucion", "[EstadoEjecucion] IN (\r\n    N'NoIniciado',\r\n    N'EnCurso',\r\n    N'Parcial',\r\n    N'Completado',\r\n    N'Omitido'\r\n)");
                    table.CheckConstraint("CK_PresupuestoDetalles_Justificacion", "[EstadoEjecucion] NOT IN (N'Parcial', N'Omitido')\r\nOR (\r\n    [ObservacionesEjecucion] IS NOT NULL\r\n    AND LEN(LTRIM(RTRIM([ObservacionesEjecucion]))) > 0\r\n)");
                    table.CheckConstraint("CK_PresupuestoDetalles_PrecioUnitario", "[PrecioUnitario] >= 0");
                    table.CheckConstraint("CK_PresupuestoDetalles_PresupuestoCambio", "[IdPresupuestoCambio] IS NULL\r\nOR [IdPresupuestoCambio] <> [IdPresupuesto]");
                    table.CheckConstraint("CK_PresupuestoDetalles_RegistroTecnico", "(\r\n    [IdTecnicoRegistro] IS NULL\r\n    AND [FechaRegistroEjecucion] IS NULL\r\n)\r\nOR\r\n(\r\n    [IdTecnicoRegistro] IS NOT NULL\r\n    AND [FechaRegistroEjecucion] IS NOT NULL\r\n)");
                    table.ForeignKey(
                        name: "FK_PresupuestoDetalles_Conceptos_IdConcepto",
                        column: x => x.IdConcepto,
                        principalTable: "Conceptos",
                        principalColumn: "IdConcepto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PresupuestoDetalles_Presupuestos_IdPresupuesto",
                        column: x => x.IdPresupuesto,
                        principalTable: "Presupuestos",
                        principalColumn: "IdPresupuesto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PresupuestoDetalles_Presupuestos_IdPresupuestoCambio",
                        column: x => x.IdPresupuestoCambio,
                        principalTable: "Presupuestos",
                        principalColumn: "IdPresupuesto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PresupuestoDetalles_Usuarios_IdTecnicoRegistro",
                        column: x => x.IdTecnicoRegistro,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PresupuestoDetalles_IdConcepto",
                table: "PresupuestoDetalles",
                column: "IdConcepto");

            migrationBuilder.CreateIndex(
                name: "IX_PresupuestoDetalles_IdPresupuesto",
                table: "PresupuestoDetalles",
                column: "IdPresupuesto");

            migrationBuilder.CreateIndex(
                name: "IX_PresupuestoDetalles_IdPresupuestoCambio",
                table: "PresupuestoDetalles",
                column: "IdPresupuestoCambio");

            migrationBuilder.CreateIndex(
                name: "IX_PresupuestoDetalles_IdTecnicoRegistro",
                table: "PresupuestoDetalles",
                column: "IdTecnicoRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_IdDiagnostico",
                table: "Presupuestos",
                column: "IdDiagnostico");

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_IdUsuarioCreacion",
                table: "Presupuestos",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_IdUsuarioDecision",
                table: "Presupuestos",
                column: "IdUsuarioDecision");

            migrationBuilder.CreateIndex(
                name: "UX_Presupuestos_DiagnosticoPorAtencion",
                table: "Presupuestos",
                column: "IdAtencion",
                unique: true,
                filter: "[IdAtencion] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PresupuestoDetalles");

            migrationBuilder.DropTable(
                name: "Presupuestos");
        }
    }
}
