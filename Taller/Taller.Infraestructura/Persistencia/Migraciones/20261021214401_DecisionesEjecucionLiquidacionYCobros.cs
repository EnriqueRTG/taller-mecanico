using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Taller.Infraestructura.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class DecisionesEjecucionLiquidacionYCobros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Presupuestos_BorradorPorAtencion",
                table: "Presupuestos");

            migrationBuilder.AddColumn<bool>(
                name: "AlcanceValidado",
                table: "Presupuestos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "IdPresupuestoAnterior",
                table: "Presupuestos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoRevision",
                table: "Presupuestos",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ObservacionesDecision",
                table: "Presupuestos",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Vigente",
                table: "Presupuestos",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "AceptadoPorCliente",
                table: "PresupuestoDetalles",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "HistorialAtenciones",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            // La aceptación histórica era por el presupuesto completo. No inventa
            // decisiones nuevas ni convierte servicios autorizados en ejecutados.
            migrationBuilder.Sql("""
                UPDATE d SET AceptadoPorCliente = CASE WHEN p.Estado = N'Aceptado' THEN 1 ELSE 0 END
                FROM PresupuestoDetalles d JOIN Presupuestos p ON p.IdPresupuesto = d.IdPresupuesto
                WHERE p.Estado IN (N'Aceptado', N'Rechazado');
                WITH Historial AS (
                    SELECT IdPresupuesto, ROW_NUMBER() OVER (PARTITION BY IdAtencion ORDER BY IdPresupuesto DESC) AS rn
                    FROM Presupuestos WHERE Tipo = N'Reparacion'
                )
                UPDATE p SET Vigente = CASE WHEN h.rn = 1 THEN 1 ELSE 0 END
                FROM Presupuestos p JOIN Historial h ON p.IdPresupuesto = h.IdPresupuesto;
                UPDATE p SET AlcanceValidado = 1
                FROM Presupuestos p WHERE p.Estado = N'Aceptado' AND EXISTS (
                    SELECT 1 FROM PresupuestoDetalles d WHERE d.IdPresupuesto=p.IdPresupuesto
                    AND (d.CantidadEjecutada > 0 OR d.EstadoEjecucion <> N'NoIniciado'));
                """);

            migrationBuilder.CreateTable(
                name: "Comprobantes",
                columns: table => new
                {
                    IdComprobante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAtencion = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioEmision = table.Column<int>(type: "int", nullable: false),
                    FechaEmision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cliente = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DocumentoCliente = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Dominio = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Moneda = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalPagado = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comprobantes", x => x.IdComprobante);
                    table.CheckConstraint("CK_Comprobantes_Importes", "[Total] >= 0 AND [TotalPagado] >= 0 AND [TotalPagado] <= [Total]");
                    table.CheckConstraint("CK_Comprobantes_SinCargo", "[Total] > 0 OR LEN(LTRIM(RTRIM(COALESCE([Observaciones], N'')))) > 0");
                    table.ForeignKey(
                        name: "FK_Comprobantes_Atenciones_IdAtencion",
                        column: x => x.IdAtencion,
                        principalTable: "Atenciones",
                        principalColumn: "IdAtencion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comprobantes_Usuarios_IdUsuarioEmision",
                        column: x => x.IdUsuarioEmision,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MediosPago",
                columns: table => new
                {
                    IdMedioPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    RequiereReferencia = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediosPago", x => x.IdMedioPago);
                });

            migrationBuilder.CreateTable(
                name: "ComprobanteDetalles",
                columns: table => new
                {
                    IdComprobanteDetalle = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdComprobante = table.Column<int>(type: "int", nullable: false),
                    IdPresupuestoDetalle = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComprobanteDetalles", x => x.IdComprobanteDetalle);
                    table.CheckConstraint("CK_ComprobanteDetalles_Importes", "[Cantidad] > 0 AND [PrecioUnitario] >= 0 AND [Subtotal] = ROUND([Cantidad] * [PrecioUnitario], 2)");
                    table.ForeignKey(
                        name: "FK_ComprobanteDetalles_Comprobantes_IdComprobante",
                        column: x => x.IdComprobante,
                        principalTable: "Comprobantes",
                        principalColumn: "IdComprobante",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComprobanteDetalles_PresupuestoDetalles_IdPresupuestoDetalle",
                        column: x => x.IdPresupuestoDetalle,
                        principalTable: "PresupuestoDetalles",
                        principalColumn: "IdPresupuestoDetalle",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    IdPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdOperacion = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdComprobante = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUsuarioRegistro = table.Column<int>(type: "int", nullable: false),
                    UsuarioRegistroNombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Importe = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    SaldoPosterior = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Anulado = table.Column<bool>(type: "bit", nullable: false),
                    MotivoAnulacion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FechaAnulacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdUsuarioAnulacion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.IdPago);
                    table.CheckConstraint("CK_Pagos_Anulacion", "([Anulado] = 0 AND [MotivoAnulacion] IS NULL AND [FechaAnulacion] IS NULL AND [IdUsuarioAnulacion] IS NULL) OR ([Anulado] = 1 AND LEN(LTRIM(RTRIM(COALESCE([MotivoAnulacion], N'')))) > 0 AND [FechaAnulacion] IS NOT NULL AND [IdUsuarioAnulacion] IS NOT NULL)");
                    table.CheckConstraint("CK_Pagos_Importe", "[Importe] > 0 AND [SaldoPosterior] >= 0");
                    table.ForeignKey(
                        name: "FK_Pagos_Comprobantes_IdComprobante",
                        column: x => x.IdComprobante,
                        principalTable: "Comprobantes",
                        principalColumn: "IdComprobante",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagos_Usuarios_IdUsuarioAnulacion",
                        column: x => x.IdUsuarioAnulacion,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagos_Usuarios_IdUsuarioRegistro",
                        column: x => x.IdUsuarioRegistro,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PagoMedios",
                columns: table => new
                {
                    IdPagoMedio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPago = table.Column<int>(type: "int", nullable: false),
                    NombreMedio = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    IdMedioPago = table.Column<int>(type: "int", nullable: false),
                    Importe = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Referencia = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagoMedios", x => x.IdPagoMedio);
                    table.CheckConstraint("CK_PagoMedios_Importe", "[Importe] > 0 AND [Orden] >= 0");
                    table.ForeignKey(
                        name: "FK_PagoMedios_MediosPago_IdMedioPago",
                        column: x => x.IdMedioPago,
                        principalTable: "MediosPago",
                        principalColumn: "IdMedioPago",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PagoMedios_Pagos_IdPago",
                        column: x => x.IdPago,
                        principalTable: "Pagos",
                        principalColumn: "IdPago",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "MediosPago",
                columns: new[] { "IdMedioPago", "Activo", "Nombre", "RequiereReferencia" },
                values: new object[,]
                {
                    { 1, true, "Efectivo", false },
                    { 2, true, "Transferencia", true },
                    { 3, true, "Tarjeta de débito", true },
                    { 4, true, "Tarjeta de crédito", true },
                    { 5, true, "Billetera virtual", true }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_IdPresupuestoAnterior",
                table: "Presupuestos",
                column: "IdPresupuestoAnterior");

            migrationBuilder.CreateIndex(
                name: "UX_Presupuestos_BorradorPorAtencion",
                table: "Presupuestos",
                column: "IdAtencion",
                unique: true,
                filter: "[Tipo] = N'Reparacion' AND [Estado] = N'Borrador' AND [Vigente] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteDetalles_IdComprobante",
                table: "ComprobanteDetalles",
                column: "IdComprobante");

            migrationBuilder.CreateIndex(
                name: "IX_ComprobanteDetalles_IdPresupuestoDetalle",
                table: "ComprobanteDetalles",
                column: "IdPresupuestoDetalle",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_IdAtencion",
                table: "Comprobantes",
                column: "IdAtencion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comprobantes_IdUsuarioEmision",
                table: "Comprobantes",
                column: "IdUsuarioEmision");

            migrationBuilder.CreateIndex(
                name: "IX_MediosPago_Nombre",
                table: "MediosPago",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PagoMedios_IdMedioPago",
                table: "PagoMedios",
                column: "IdMedioPago");

            migrationBuilder.CreateIndex(
                name: "IX_PagoMedios_IdPago_Orden",
                table: "PagoMedios",
                columns: new[] { "IdPago", "Orden" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdComprobante",
                table: "Pagos",
                column: "IdComprobante");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdOperacion",
                table: "Pagos",
                column: "IdOperacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdUsuarioAnulacion",
                table: "Pagos",
                column: "IdUsuarioAnulacion");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdUsuarioRegistro",
                table: "Pagos",
                column: "IdUsuarioRegistro");

            migrationBuilder.AddForeignKey(
                name: "FK_Presupuestos_Presupuestos_IdPresupuestoAnterior",
                table: "Presupuestos",
                column: "IdPresupuestoAnterior",
                principalTable: "Presupuestos",
                principalColumn: "IdPresupuesto",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                IF EXISTS (SELECT 1 FROM Comprobantes) OR EXISTS (SELECT 1 FROM Pagos)
                   OR EXISTS (SELECT 1 FROM HistorialAtenciones WHERE LEN(Observaciones) > 1000)
                   OR EXISTS (SELECT 1 FROM Presupuestos WHERE IdPresupuestoAnterior IS NOT NULL OR ObservacionesDecision IS NOT NULL OR Vigente = 0)
                    THROW 51002, 'Hay decisiones o movimientos económicos registrados. Restaure el respaldo previo para volver sin perder información.', 1;
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_Presupuestos_Presupuestos_IdPresupuestoAnterior",
                table: "Presupuestos");

            migrationBuilder.DropTable(
                name: "ComprobanteDetalles");

            migrationBuilder.DropTable(
                name: "PagoMedios");

            migrationBuilder.DropTable(
                name: "MediosPago");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Comprobantes");

            migrationBuilder.DropIndex(
                name: "IX_Presupuestos_IdPresupuestoAnterior",
                table: "Presupuestos");

            migrationBuilder.DropIndex(
                name: "UX_Presupuestos_BorradorPorAtencion",
                table: "Presupuestos");

            migrationBuilder.DropColumn(
                name: "AlcanceValidado",
                table: "Presupuestos");

            migrationBuilder.DropColumn(
                name: "IdPresupuestoAnterior",
                table: "Presupuestos");

            migrationBuilder.DropColumn(
                name: "MotivoRevision",
                table: "Presupuestos");

            migrationBuilder.DropColumn(
                name: "ObservacionesDecision",
                table: "Presupuestos");

            migrationBuilder.DropColumn(
                name: "Vigente",
                table: "Presupuestos");

            migrationBuilder.DropColumn(
                name: "AceptadoPorCliente",
                table: "PresupuestoDetalles");

            migrationBuilder.AlterColumn<string>(
                name: "Observaciones",
                table: "HistorialAtenciones",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "UX_Presupuestos_BorradorPorAtencion",
                table: "Presupuestos",
                column: "IdAtencion",
                unique: true,
                filter: "[Tipo] = N'Reparacion' AND [Estado] = N'Borrador'");
        }
    }
}
