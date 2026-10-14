using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Taller.Infraestructura.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class AgregarDiagnosticosHistorialYCatalogo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Atenciones_IdVehiculo",
                table: "Atenciones");

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
                name: "Categorias",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.IdCategoria);
                    table.CheckConstraint("CK_Categorias_Nombre", "LEN(LTRIM(RTRIM(Nombre))) > 0");
                });

            migrationBuilder.CreateTable(
                name: "Diagnosticos",
                columns: table => new
                {
                    IdDiagnostico = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAtencion = table.Column<int>(type: "int", nullable: false),
                    IdTecnicoAutor = table.Column<int>(type: "int", nullable: false),
                    FechaDiagnostico = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    FallaDetectada = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    TrabajoRecomendado = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Kilometraje = table.Column<int>(type: "int", nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ImpideContinuar = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
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
                name: "Conceptos",
                columns: table => new
                {
                    IdConcepto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCategoria = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PrecioReferencia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conceptos", x => x.IdConcepto);
                    table.CheckConstraint("CK_Conceptos_Nombre", "LEN(LTRIM(RTRIM(Nombre))) > 0");
                    table.CheckConstraint("CK_Conceptos_PrecioReferencia", "[PrecioReferencia] IS NULL OR \r\n[PrecioReferencia] >= 0");
                    table.CheckConstraint("CK_Conceptos_Tipo", "[Tipo] IN (\r\n    N'Servicio', \r\n    N'Repuesto', \r\n    N'Consumible')");
                    table.ForeignKey(
                        name: "FK_Conceptos_Categorias_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "Categorias",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistorialAtenciones",
                columns: table => new
                {
                    IdHistorialAtencion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAtencion = table.Column<int>(type: "int", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdDiagnostico = table.Column<int>(type: "int", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    Accion = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    EstadoAnterior = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    EstadoNuevo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
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
                name: "UX_Categorias_Nombre",
                table: "Categorias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Conceptos_IdCategoria",
                table: "Conceptos",
                column: "IdCategoria");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Atenciones_Usuarios_IdTecnicoEnActividad",
                table: "Atenciones",
                column: "IdTecnicoEnActividad",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Atenciones_Usuarios_IdTecnicoEnActividad",
                table: "Atenciones");

            migrationBuilder.DropTable(
                name: "Conceptos");

            migrationBuilder.DropTable(
                name: "HistorialAtenciones");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Diagnosticos");

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

            migrationBuilder.CreateIndex(
                name: "IX_Atenciones_IdVehiculo",
                table: "Atenciones",
                column: "IdVehiculo");
        }
    }
}
