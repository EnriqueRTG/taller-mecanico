using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Taller.Infraestructura.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class AjustarCatalogoActual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atenciones_Usuarios_IdTecnicoEnActividad",
                table: "Atenciones");

            migrationBuilder.DropTable(
                name: "HistorialAtenciones");

            migrationBuilder.DropTable(
                name: "PresupuestoDetalles");

            migrationBuilder.DropTable(
                name: "Presupuestos");

            migrationBuilder.DropTable(
                name: "Diagnosticos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Categorias_Nombre",
                table: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Atenciones_IdTecnicoEnActividad",
                table: "Atenciones");

            migrationBuilder.DropIndex(
                name: "UX_Atenciones_VehiculoActivo",
                table: "Atenciones");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Atenciones_ActividadTecnica",
                table: "Atenciones");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Atenciones_Estado",
                table: "Atenciones");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Atenciones_ReservaTecnica",
                table: "Atenciones");

            migrationBuilder.DropColumn(
                name: "ActividadTecnica",
                table: "Atenciones");

            migrationBuilder.DropColumn(
                name: "IdTecnicoEnActividad",
                table: "Atenciones");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Atenciones");

            migrationBuilder.RenameIndex(
                name: "UX_Categorias_Nombre",
                table: "Categorias",
                newName: "IX_Categorias_Nombre");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Categorias",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Categorias",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "IdCategoria", "Activo", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Servicios de mantenimiento preventivo y cambio de fluidos.", "Mantenimiento preventivo" },
                    { 2, true, "Servicios de reparación y sustitución de componentes mecánicos.", "Reparación mecánica" },
                    { 3, true, "Servicios de diagnóstico y reparación de sistemas electrónicos.", "Diagnóstico electrónico" },
                    { 4, true, "Servicios de reparación y mantenimiento del sistema de frenos.", "Sistema de frenos" },
                    { 5, true, "Servicios de reparación y mantenimiento de la suspensión.", "Suspensión" },
                    { 6, true, "Servicios de reparación y mantenimiento de la dirección.", "Dirección" },
                    { 7, true, "Servicios de reparación y mantenimiento de la transmisión.", "Transmisión" },
                    { 8, true, "Servicios de reparación y mantenimiento del embrague.", "Embrague" },
                    { 9, true, "Servicios de reparación y mantenimiento del sistema de refrigeración.", "Sistema de refrigeración" },
                    { 10, true, "Servicios de diagnóstico computarizado del vehículo.", "Diagnóstico computarizado" },
                    { 11, true, "Servicios de reparación del sistema de arranque y carga.", "Sistema de arranque y carga" },
                    { 12, true, "Servicios de revisión y reparación de baterías.", "Baterías" },
                    { 13, true, "Servicios de reparación de luces y componentes eléctricos.", "Electricidad e iluminación" },
                    { 14, true, "Servicios de reparación del sistema de climatización.", "Aire acondicionado" },
                    { 15, true, "Servicios de revisión y reparación de la calefacción.", "Calefacción" },
                    { 16, true, "Servicios de inspección y evaluación general del vehículo.", "Diagnóstico técnico inicial" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atenciones_IdVehiculo",
                table: "Atenciones",
                column: "IdVehiculo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Atenciones_IdVehiculo",
                table: "Atenciones");

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 16);

            migrationBuilder.RenameIndex(
                name: "IX_Categorias_Nombre",
                table: "Categorias",
                newName: "UX_Categorias_Nombre");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Categorias",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Categorias",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActividadTecnica",
                table: "Atenciones",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdTecnicoEnActividad",
                table: "Atenciones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "Atenciones",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateTable(
                name: "Diagnosticos",
                columns: table => new
                {
                    IdDiagnostico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAtencion = table.Column<int>(type: "int", nullable: false),
                    IdTecnicoAutor = table.Column<int>(type: "int", nullable: false),
                    FallaDetectada = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FechaDiagnostico = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    ImpideContinuar = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Kilometraje = table.Column<int>(type: "int", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TrabajoRecomendado = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnosticos", x => x.IdDiagnostico);
                    table.CheckConstraint("CK_Diagnosticos_FallaDetectada", "LEN(LTRIM(RTRIM([FallaDetectada]))) > 0");
                    table.CheckConstraint("CK_Diagnosticos_ImpideContinuar", "[Tipo] <> N'Inicial' OR [ImpideContinuar] = 0");
                    table.CheckConstraint("CK_Diagnosticos_Kilometraje", "[Kilometraje] IS NULL OR [Kilometraje] >= 0");
                    table.CheckConstraint("CK_Diagnosticos_Tipo", "Tipo IN (N'Inicial', N'Adicional')");
                    table.CheckConstraint("CK_Diagnosticos_TrabajoRecomendado", "LEN(LTRIM(RTRIM([TrabajoRecomendado]))) > 0");
                    table.ForeignKey(
                        name: "FK_Diagnosticos_Atenciones_IdAtencion",
                        column: x => x.IdAtencion,
                        principalTable: "Atenciones",
                        principalColumn: "IdAtencion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Diagnosticos_Usuarios_IdTecnicoAutor",
                        column: x => x.IdTecnicoAutor,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistorialAtenciones",
                columns: table => new
                {
                    IdHistorialAtencion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAtencion = table.Column<int>(type: "int", nullable: false),
                    IdDiagnostico = table.Column<int>(type: "int", nullable: true),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    Accion = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    EstadoAnterior = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    EstadoNuevo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialAtenciones", x => x.IdHistorialAtencion);
                    table.CheckConstraint("CK_HistorialAtenciones_Accion", "LEN(LTRIM(RTRIM([Accion]))) > 0");
                    table.CheckConstraint("CK_HistorialAtenciones_EstadoAnterior", "[EstadoAnterior] IS NULL\r\nOR [EstadoAnterior] IN (\r\n    N'Abierta',\r\n    N'PendienteDiagnostico',\r\n    N'EnEvaluacionTecnica',\r\n    N'PendienteDecision',\r\n    N'EnEjecucion',\r\n    N'Pausada',\r\n    N'TrabajoFinalizado',\r\n    N'PendienteLiquidacion',\r\n    N'Cerrada',\r\n    N'Cancelada'\r\n)");
                    table.CheckConstraint("CK_HistorialAtenciones_EstadoNuevo", "[EstadoNuevo] IN (\r\n    N'Abierta',\r\n    N'PendienteDiagnostico',\r\n    N'EnEvaluacionTecnica',\r\n    N'PendienteDecision',\r\n    N'EnEjecucion',\r\n    N'Pausada',\r\n    N'TrabajoFinalizado',\r\n    N'PendienteLiquidacion',\r\n    N'Cerrada',\r\n    N'Cancelada'\r\n)");
                    table.ForeignKey(
                        name: "FK_HistorialAtenciones_Atenciones_IdAtencion",
                        column: x => x.IdAtencion,
                        principalTable: "Atenciones",
                        principalColumn: "IdAtencion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistorialAtenciones_Diagnosticos_IdDiagnostico",
                        column: x => x.IdDiagnostico,
                        principalTable: "Diagnosticos",
                        principalColumn: "IdDiagnostico",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistorialAtenciones_Usuarios_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Presupuestos",
                columns: table => new
                {
                    IdPresupuesto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAtencion = table.Column<int>(type: "int", nullable: true),
                    IdDiagnostico = table.Column<int>(type: "int", nullable: true),
                    IdUsuarioCreacion = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioDecision = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    FechaDecision = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    FechaPresentacion = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
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
                    IdConcepto = table.Column<int>(type: "int", nullable: false),
                    IdPresupuesto = table.Column<int>(type: "int", nullable: false),
                    IdPresupuestoCambio = table.Column<int>(type: "int", nullable: true),
                    IdTecnicoRegistro = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    CantidadEjecutada = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EstadoEjecucion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FechaRegistroEjecucion = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    ObservacionesEjecucion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
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

            migrationBuilder.AddCheckConstraint(
                name: "CK_Categorias_Nombre",
                table: "Categorias",
                sql: "LEN(LTRIM(RTRIM(Nombre))) > 0");

            migrationBuilder.CreateIndex(
                name: "IX_Atenciones_IdTecnicoEnActividad",
                table: "Atenciones",
                column: "IdTecnicoEnActividad");

            migrationBuilder.CreateIndex(
                name: "UX_Atenciones_VehiculoActivo",
                table: "Atenciones",
                column: "IdVehiculo",
                unique: true,
                filter: "[Estado] <> N'Cerrada' AND [Estado] <> N'Cancelada'");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Atenciones_ActividadTecnica",
                table: "Atenciones",
                sql: "[ActividadTecnica] IS NULL \r\nOR [ActividadTecnica] IN (\r\n    N'Diagnostico',\r\n    N'Presupuesto',\r\n    N'Ejecucion'\r\n)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Atenciones_Estado",
                table: "Atenciones",
                sql: "[Estado] IN (\r\n    N'Abierta',\r\n    N'PendienteDiagnostico',\r\n    N'EnEvaluacionTecnica',\r\n    N'PendienteDecision',\r\n    N'EnEjecucion',\r\n    N'Pausada',\r\n    N'TrabajoFinalizado',\r\n    N'PendienteLiquidacion',\r\n    N'Cerrada',\r\n    N'Cancelada'\r\n)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Atenciones_ReservaTecnica",
                table: "Atenciones",
                sql: "(\r\n    [IdTecnicoEnActividad] IS NULL\r\n    AND [ActividadTecnica] IS NULL\r\n)\r\nOR\r\n(\r\n    [IdTecnicoEnActividad] IS NOT NULL\r\n    AND [ActividadTecnica] IS NOT NULL\r\n)");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnosticos_IdTecnicoAutor",
                table: "Diagnosticos",
                column: "IdTecnicoAutor");

            migrationBuilder.CreateIndex(
                name: "UX_Diagnosticos_InicialPorAtencion",
                table: "Diagnosticos",
                column: "IdAtencion",
                unique: true,
                filter: "[Tipo] = N'Inicial'");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialAtenciones_Atencion_Fecha_Id",
                table: "HistorialAtenciones",
                columns: new[] { "IdAtencion", "Fecha", "IdHistorialAtencion" });

            migrationBuilder.CreateIndex(
                name: "IX_HistorialAtenciones_IdDiagnostico",
                table: "HistorialAtenciones",
                column: "IdDiagnostico");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialAtenciones_IdUsuario",
                table: "HistorialAtenciones",
                column: "IdUsuario");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Atenciones_Usuarios_IdTecnicoEnActividad",
                table: "Atenciones",
                column: "IdTecnicoEnActividad",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
