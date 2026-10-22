using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller.Infraestructura.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class HallazgosAdicionalesYMaterialesCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Presupuestos_BorradorPorAtencion",
                table: "Presupuestos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Presupuestos_Decision",
                table: "Presupuestos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Presupuestos_Estado",
                table: "Presupuestos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Presupuestos_Presentacion",
                table: "Presupuestos");

            migrationBuilder.AddColumn<bool>(
                name: "EsAdicional",
                table: "Presupuestos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EsInspeccion",
                table: "Presupuestos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "IdHallazgoAtencion",
                table: "Presupuestos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CantidadRecibidaCliente",
                table: "PresupuestoDetalles",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRecepcionMaterial",
                table: "PresupuestoDetalles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdTecnicoMaterial",
                table: "PresupuestoDetalles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MaterialClienteConforme",
                table: "PresupuestoDetalles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ObservacionesMaterial",
                table: "PresupuestoDetalles",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrigenMaterial",
                table: "PresupuestoDetalles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Taller");

            migrationBuilder.CreateTable(
                name: "HallazgosAtencion",
                columns: table => new
                {
                    IdHallazgoAtencion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAtencion = table.Column<int>(type: "int", nullable: false),
                    IdTecnico = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Recomendacion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    ImpideContinuar = table.Column<bool>(type: "bit", nullable: false),
                    IdDetalleAfectado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HallazgosAtencion", x => x.IdHallazgoAtencion);
                    table.CheckConstraint("CK_Hallazgos_Textos", "LEN(LTRIM(RTRIM([Descripcion]))) > 0 AND LEN(LTRIM(RTRIM([Recomendacion]))) > 0");
                    table.ForeignKey(
                        name: "FK_HallazgosAtencion_Atenciones_IdAtencion",
                        column: x => x.IdAtencion,
                        principalTable: "Atenciones",
                        principalColumn: "IdAtencion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HallazgosAtencion_PresupuestoDetalles_IdDetalleAfectado",
                        column: x => x.IdDetalleAfectado,
                        principalTable: "PresupuestoDetalles",
                        principalColumn: "IdPresupuestoDetalle",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HallazgosAtencion_Usuarios_IdTecnico",
                        column: x => x.IdTecnico,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PausasTrabajo",
                columns: table => new
                {
                    IdPausaTrabajo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAtencion = table.Column<int>(type: "int", nullable: false),
                    IdDetalle = table.Column<int>(type: "int", nullable: true),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FechaResolucion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdTecnicoResolucion = table.Column<int>(type: "int", nullable: true),
                    Resolucion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PausasTrabajo", x => x.IdPausaTrabajo);
                    table.CheckConstraint("CK_Pausas_Motivo", "LEN(LTRIM(RTRIM([Motivo]))) > 0");
                    table.CheckConstraint("CK_Pausas_Resolucion", "([FechaResolucion] IS NULL AND [IdTecnicoResolucion] IS NULL AND [Resolucion] IS NULL) OR ([FechaResolucion] >= [Fecha] AND [IdTecnicoResolucion] IS NOT NULL AND LEN(LTRIM(RTRIM([Resolucion]))) > 0)");
                    table.ForeignKey(
                        name: "FK_PausasTrabajo_Atenciones_IdAtencion",
                        column: x => x.IdAtencion,
                        principalTable: "Atenciones",
                        principalColumn: "IdAtencion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PausasTrabajo_PresupuestoDetalles_IdDetalle",
                        column: x => x.IdDetalle,
                        principalTable: "PresupuestoDetalles",
                        principalColumn: "IdPresupuestoDetalle",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PausasTrabajo_Usuarios_IdTecnicoResolucion",
                        column: x => x.IdTecnicoResolucion,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PausasTrabajo_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_IdHallazgoAtencion",
                table: "Presupuestos",
                column: "IdHallazgoAtencion");

            migrationBuilder.CreateIndex(
                name: "UX_Presupuestos_BorradorPorAtencion",
                table: "Presupuestos",
                column: "IdAtencion",
                unique: true,
                filter: "[Tipo] = N'Reparacion' AND [Estado] = N'Borrador' AND [Vigente] = 1 AND [EsAdicional] = 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Presupuestos_Adicional",
                table: "Presupuestos",
                sql: "([EsInspeccion] = 0 OR [EsAdicional] = 1) AND ([EsAdicional] = 0 OR [Tipo] = N'Reparacion') AND ([IdHallazgoAtencion] IS NULL OR [EsAdicional] = 1)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Presupuestos_Decision",
                table: "Presupuestos",
                sql: "(\n    [Estado] IN (\n        N'Borrador',\n    N'PendientePresentacion',\n        N'PendienteDecision'\n    )\n    AND [IdUsuarioDecision] IS NULL\n    AND [FechaDecision] IS NULL\n)\nOR\n(\n    [Estado] IN (N'Aceptado', N'Rechazado', N'PendienteRevisionTecnica')\n    AND [IdUsuarioDecision] IS NOT NULL\n    AND [FechaDecision] IS NOT NULL\n)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Presupuestos_Estado",
                table: "Presupuestos",
                sql: "[Estado] IN (\n    N'Borrador',\n    N'PendientePresentacion',\n    N'PendienteDecision',\n    N'Aceptado',\n    N'Rechazado', N'PendienteRevisionTecnica'\n)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Presupuestos_Presentacion",
                table: "Presupuestos",
                sql: "(\n    [Estado] IN (N'Borrador', N'PendientePresentacion')\n    AND [FechaPresentacion] IS NULL\n)\nOR\n(\n    [Estado] IN (\n        N'PendienteDecision',\n        N'Aceptado',\n        N'Rechazado', N'PendienteRevisionTecnica'\n    )\n    AND [FechaPresentacion] IS NOT NULL\n)");

            migrationBuilder.CreateIndex(
                name: "IX_PresupuestoDetalles_IdTecnicoMaterial",
                table: "PresupuestoDetalles",
                column: "IdTecnicoMaterial");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Detalles_MaterialCliente",
                table: "PresupuestoDetalles",
                sql: "[OrigenMaterial] IN (N'Taller', N'Cliente') AND [CantidadRecibidaCliente] >= 0 AND [CantidadRecibidaCliente] <= [Cantidad] AND ([OrigenMaterial] <> N'Cliente' OR [PrecioUnitario] = 0) AND ([MaterialClienteConforme] = 0 OR ([OrigenMaterial] = N'Cliente' AND [CantidadRecibidaCliente] > 0 AND [IdTecnicoMaterial] IS NOT NULL AND [FechaRecepcionMaterial] IS NOT NULL))");

            migrationBuilder.CreateIndex(
                name: "IX_HallazgosAtencion_IdAtencion",
                table: "HallazgosAtencion",
                column: "IdAtencion");

            migrationBuilder.CreateIndex(
                name: "IX_HallazgosAtencion_IdDetalleAfectado",
                table: "HallazgosAtencion",
                column: "IdDetalleAfectado");

            migrationBuilder.CreateIndex(
                name: "IX_HallazgosAtencion_IdTecnico",
                table: "HallazgosAtencion",
                column: "IdTecnico");

            migrationBuilder.CreateIndex(
                name: "IX_PausasTrabajo_IdAtencion",
                table: "PausasTrabajo",
                column: "IdAtencion");

            migrationBuilder.CreateIndex(
                name: "IX_PausasTrabajo_IdDetalle",
                table: "PausasTrabajo",
                column: "IdDetalle");

            migrationBuilder.CreateIndex(
                name: "IX_PausasTrabajo_IdTecnicoResolucion",
                table: "PausasTrabajo",
                column: "IdTecnicoResolucion");

            migrationBuilder.CreateIndex(
                name: "IX_PausasTrabajo_IdUsuario",
                table: "PausasTrabajo",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_PresupuestoDetalles_Usuarios_IdTecnicoMaterial",
                table: "PresupuestoDetalles",
                column: "IdTecnicoMaterial",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Presupuestos_HallazgosAtencion_IdHallazgoAtencion",
                table: "Presupuestos",
                column: "IdHallazgoAtencion",
                principalTable: "HallazgosAtencion",
                principalColumn: "IdHallazgoAtencion",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PresupuestoDetalles_Usuarios_IdTecnicoMaterial",
                table: "PresupuestoDetalles");

            migrationBuilder.DropForeignKey(
                name: "FK_Presupuestos_HallazgosAtencion_IdHallazgoAtencion",
                table: "Presupuestos");

            migrationBuilder.DropTable(
                name: "HallazgosAtencion");

            migrationBuilder.DropTable(
                name: "PausasTrabajo");

            migrationBuilder.DropIndex(
                name: "IX_Presupuestos_IdHallazgoAtencion",
                table: "Presupuestos");

            migrationBuilder.DropIndex(
                name: "UX_Presupuestos_BorradorPorAtencion",
                table: "Presupuestos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Presupuestos_Adicional",
                table: "Presupuestos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Presupuestos_Decision",
                table: "Presupuestos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Presupuestos_Estado",
                table: "Presupuestos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Presupuestos_Presentacion",
                table: "Presupuestos");

            migrationBuilder.DropIndex(
                name: "IX_PresupuestoDetalles_IdTecnicoMaterial",
                table: "PresupuestoDetalles");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Detalles_MaterialCliente",
                table: "PresupuestoDetalles");

            migrationBuilder.DropColumn(
                name: "EsAdicional",
                table: "Presupuestos");

            migrationBuilder.DropColumn(
                name: "EsInspeccion",
                table: "Presupuestos");

            migrationBuilder.DropColumn(
                name: "IdHallazgoAtencion",
                table: "Presupuestos");

            migrationBuilder.DropColumn(
                name: "CantidadRecibidaCliente",
                table: "PresupuestoDetalles");

            migrationBuilder.DropColumn(
                name: "FechaRecepcionMaterial",
                table: "PresupuestoDetalles");

            migrationBuilder.DropColumn(
                name: "IdTecnicoMaterial",
                table: "PresupuestoDetalles");

            migrationBuilder.DropColumn(
                name: "MaterialClienteConforme",
                table: "PresupuestoDetalles");

            migrationBuilder.DropColumn(
                name: "ObservacionesMaterial",
                table: "PresupuestoDetalles");

            migrationBuilder.DropColumn(
                name: "OrigenMaterial",
                table: "PresupuestoDetalles");

            migrationBuilder.CreateIndex(
                name: "UX_Presupuestos_BorradorPorAtencion",
                table: "Presupuestos",
                column: "IdAtencion",
                unique: true,
                filter: "[Tipo] = N'Reparacion' AND [Estado] = N'Borrador' AND [Vigente] = 1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Presupuestos_Decision",
                table: "Presupuestos",
                sql: "(\n    [Estado] IN (\n        N'Borrador',\n    N'PendientePresentacion',\n        N'PendienteDecision'\n    )\n    AND [IdUsuarioDecision] IS NULL\n    AND [FechaDecision] IS NULL\n)\nOR\n(\n    [Estado] IN (N'Aceptado', N'Rechazado')\n    AND [IdUsuarioDecision] IS NOT NULL\n    AND [FechaDecision] IS NOT NULL\n)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Presupuestos_Estado",
                table: "Presupuestos",
                sql: "[Estado] IN (\n    N'Borrador',\n    N'PendientePresentacion',\n    N'PendienteDecision',\n    N'Aceptado',\n    N'Rechazado'\n)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Presupuestos_Presentacion",
                table: "Presupuestos",
                sql: "(\n    [Estado] IN (N'Borrador', N'PendientePresentacion')\n    AND [FechaPresentacion] IS NULL\n)\nOR\n(\n    [Estado] IN (\n        N'PendienteDecision',\n        N'Aceptado',\n        N'Rechazado'\n    )\n    AND [FechaPresentacion] IS NOT NULL\n)");
        }
    }
}
