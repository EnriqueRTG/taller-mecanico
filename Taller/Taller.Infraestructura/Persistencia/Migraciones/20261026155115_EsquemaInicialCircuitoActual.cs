using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Taller.Infraestructura.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class EsquemaInicialCircuitoActual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Clientes",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Documento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TipoDocumento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "ControlesAcceso",
                columns: table => new
                {
                    Clave = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    Intentos = table.Column<int>(type: "int", nullable: false),
                    VentanaUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    BloqueadoHastaUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlesAcceso", x => x.Clave);
                });

            migrationBuilder.CreateTable(
                name: "EventosAcceso",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Clave = table.Column<string>(type: "varchar(64)", unicode: false, maxLength: 64, nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: true),
                    Correcto = table.Column<bool>(type: "bit", nullable: false),
                    FechaUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventosAcceso", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Marcas",
                columns: table => new
                {
                    IdMarca = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marcas", x => x.IdMarca);
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
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
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
                    table.CheckConstraint("CK_Conceptos_PrecioReferencia", "[PrecioReferencia] IS NULL OR\n[PrecioReferencia] >= 0");
                    table.CheckConstraint("CK_Conceptos_Tipo", "[Tipo] IN (\n    N'Servicio',\n    N'Repuesto',\n    N'Consumible')");
                    table.ForeignKey(
                        name: "FK_Conceptos_Categorias_IdCategoria",
                        column: x => x.IdCategoria,
                        principalTable: "Categorias",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Modelos",
                columns: table => new
                {
                    IdModelo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    IdMarca = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modelos", x => x.IdModelo);
                    table.ForeignKey(
                        name: "FK_Modelos_Marcas_IdMarca",
                        column: x => x.IdMarca,
                        principalTable: "Marcas",
                        principalColumn: "IdMarca",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculos",
                columns: table => new
                {
                    IdVehiculo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dominio = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdModelo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculos", x => x.IdVehiculo);
                    table.ForeignKey(
                        name: "FK_Vehiculos_Modelos_IdModelo",
                        column: x => x.IdModelo,
                        principalTable: "Modelos",
                        principalColumn: "IdModelo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Atenciones",
                columns: table => new
                {
                    IdAtencion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KilometrajeRecepcion = table.Column<int>(type: "int", nullable: true),
                    ObservacionesRecepcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IdCliente = table.Column<int>(type: "int", nullable: false),
                    IdVehiculo = table.Column<int>(type: "int", nullable: false),
                    IdUsuarioRecepcion = table.Column<int>(type: "int", nullable: false),
                    IdTecnicoEnActividad = table.Column<int>(type: "int", nullable: true),
                    ActividadTecnica = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FechaApertura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoSolicitud = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MotivoConsulta = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atenciones", x => x.IdAtencion);
                    table.CheckConstraint("CK_Atenciones_ActividadTecnica", "[ActividadTecnica] IS NULL\nOR [ActividadTecnica] IN (\n    N'Diagnostico',\n    N'Presupuesto',\n    N'Ejecucion'\n)");
                    table.CheckConstraint("CK_Atenciones_Estado", "[Estado] IN (\n    N'EnEvaluacionTecnica',\n    N'PendienteDecision',\n    N'EnEjecucion',\n    N'TrabajoFinalizado',\n    N'PendienteLiquidacion',\n    N'Cerrada',\n    N'Cancelada'\n)");
                    table.CheckConstraint("CK_Atenciones_KilometrajeRecepcion", "[KilometrajeRecepcion] IS NULL OR [KilometrajeRecepcion] >= 0");
                    table.CheckConstraint("CK_Atenciones_ReservaTecnica", "(\n    [IdTecnicoEnActividad] IS NULL\n    AND [ActividadTecnica] IS NULL\n)\nOR\n(\n    [IdTecnicoEnActividad] IS NOT NULL\n    AND [ActividadTecnica] IS NOT NULL\n)");
                    table.CheckConstraint("CK_Atenciones_TipoSolicitud", "[TipoSolicitud] IN (N'Problema', N'ServicioDefinido', N'Mixta')");
                    table.ForeignKey(
                        name: "FK_Atenciones_Clientes_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Atenciones_Usuarios_IdTecnicoEnActividad",
                        column: x => x.IdTecnicoEnActividad,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Atenciones_Usuarios_IdUsuarioRecepcion",
                        column: x => x.IdUsuarioRecepcion,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Atenciones_Vehiculos_IdVehiculo",
                        column: x => x.IdVehiculo,
                        principalTable: "Vehiculos",
                        principalColumn: "IdVehiculo",
                        onDelete: ReferentialAction.Restrict);
                });

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
                    Observaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnosticos", x => x.IdDiagnostico);
                    table.UniqueConstraint("AK_Diagnosticos_IdDiagnostico_IdAtencion", x => new { x.IdDiagnostico, x.IdAtencion });
                    table.CheckConstraint("CK_Diagnosticos_FallaDetectada", "LEN(LTRIM(RTRIM([FallaDetectada]))) > 0");
                    table.CheckConstraint("CK_Diagnosticos_Kilometraje", "[Kilometraje] IS NULL OR [Kilometraje] >= 0");
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

            migrationBuilder.CreateTable(
                name: "SolicitudesAtencion",
                columns: table => new
                {
                    IdSolicitudAtencion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAtencion = table.Column<int>(type: "int", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesAtencion", x => x.IdSolicitudAtencion);
                    table.CheckConstraint("CK_SolicitudesAtencion_Descripcion", "LEN(LTRIM(RTRIM([Descripcion]))) > 0");
                    table.CheckConstraint("CK_SolicitudesAtencion_Orden", "[Orden] > 0");
                    table.CheckConstraint("CK_SolicitudesAtencion_Tipo", "[Tipo] IN (N'TrabajoSolicitado', N'ProblemaInformado')");
                    table.ForeignKey(
                        name: "FK_SolicitudesAtencion_Atenciones_IdAtencion",
                        column: x => x.IdAtencion,
                        principalTable: "Atenciones",
                        principalColumn: "IdAtencion",
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
                    Observaciones = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialAtenciones", x => x.IdHistorialAtencion);
                    table.CheckConstraint("CK_HistorialAtenciones_Accion", "LEN(LTRIM(RTRIM([Accion]))) > 0");
                    table.CheckConstraint("CK_HistorialAtenciones_EstadoAnterior", "[EstadoAnterior] IS NULL\nOR [EstadoAnterior] IN (\n    N'EnEvaluacionTecnica',\n    N'PendienteDecision',\n    N'EnEjecucion',\n    N'TrabajoFinalizado',\n    N'PendienteLiquidacion',\n    N'Cerrada',\n    N'Cancelada'\n)");
                    table.CheckConstraint("CK_HistorialAtenciones_EstadoNuevo", "[EstadoNuevo] IN (\n    N'EnEvaluacionTecnica',\n    N'PendienteDecision',\n    N'EnEjecucion',\n    N'TrabajoFinalizado',\n    N'PendienteLiquidacion',\n    N'Cerrada',\n    N'Cancelada'\n)");
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
                });

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
                        name: "FK_HallazgosAtencion_Usuarios_IdTecnico",
                        column: x => x.IdTecnico,
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
                    PersonaDecision = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    MedioDecision = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IdPresupuestoAnterior = table.Column<int>(type: "int", nullable: true),
                    EsInspeccion = table.Column<bool>(type: "bit", nullable: false),
                    EsAdicional = table.Column<bool>(type: "bit", nullable: false),
                    IdHallazgoAtencion = table.Column<int>(type: "int", nullable: true),
                    Vigente = table.Column<bool>(type: "bit", nullable: false),
                    MotivoRevision = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ObservacionesDecision = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AlcanceValidado = table.Column<bool>(type: "bit", nullable: false),
                    IdAtencion = table.Column<int>(type: "int", nullable: false),
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
                    table.CheckConstraint("CK_Presupuestos_Adicional", "([EsInspeccion] = 0 OR [EsAdicional] = 1) AND ([IdHallazgoAtencion] IS NULL OR [EsAdicional] = 1)");
                    table.CheckConstraint("CK_Presupuestos_Decision", "(\n    [Estado] IN (\n        N'Borrador',\n    N'PendientePresentacion',\n        N'PendienteDecision'\n    )\n    AND [IdUsuarioDecision] IS NULL\n    AND [FechaDecision] IS NULL\n)\nOR\n(\n    [Estado] IN (N'Aceptado', N'Rechazado', N'PendienteRevisionTecnica')\n    AND [IdUsuarioDecision] IS NOT NULL\n    AND [FechaDecision] IS NOT NULL\n)");
                    table.CheckConstraint("CK_Presupuestos_Estado", "[Estado] IN (\n    N'Borrador',\n    N'PendientePresentacion',\n    N'PendienteDecision',\n    N'Aceptado',\n    N'Rechazado', N'PendienteRevisionTecnica'\n)");
                    table.CheckConstraint("CK_Presupuestos_Fechas", "(\n    [FechaPresentacion] IS NULL\n    OR [FechaPresentacion] >= [FechaCreacion]\n)\nAND\n(\n    [FechaDecision] IS NULL\n    OR (\n        [FechaPresentacion] IS NOT NULL\n        AND [FechaDecision] >= [FechaPresentacion]\n    )\n)");
                    table.CheckConstraint("CK_Presupuestos_Presentacion", "(\n    [Estado] IN (N'Borrador', N'PendientePresentacion')\n    AND [FechaPresentacion] IS NULL\n)\nOR\n(\n    [Estado] IN (\n        N'PendienteDecision',\n        N'Aceptado',\n        N'Rechazado', N'PendienteRevisionTecnica'\n    )\n    AND [FechaPresentacion] IS NOT NULL\n)");
                    table.ForeignKey(
                        name: "FK_Presupuestos_Atenciones_IdAtencion",
                        column: x => x.IdAtencion,
                        principalTable: "Atenciones",
                        principalColumn: "IdAtencion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Presupuestos_Diagnosticos_IdDiagnostico_IdAtencion",
                        columns: x => new { x.IdDiagnostico, x.IdAtencion },
                        principalTable: "Diagnosticos",
                        principalColumns: new[] { "IdDiagnostico", "IdAtencion" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Presupuestos_HallazgosAtencion_IdHallazgoAtencion",
                        column: x => x.IdHallazgoAtencion,
                        principalTable: "HallazgosAtencion",
                        principalColumn: "IdHallazgoAtencion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Presupuestos_Presupuestos_IdPresupuestoAnterior",
                        column: x => x.IdPresupuestoAnterior,
                        principalTable: "Presupuestos",
                        principalColumn: "IdPresupuesto",
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
                    OrigenMaterial = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Taller"),
                    CantidadRecibidaCliente = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    MaterialClienteConforme = table.Column<bool>(type: "bit", nullable: false),
                    ObservacionesMaterial = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IdTecnicoMaterial = table.Column<int>(type: "int", nullable: true),
                    FechaRecepcionMaterial = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdPresupuesto = table.Column<int>(type: "int", nullable: false),
                    IdSolicitudAtencion = table.Column<int>(type: "int", nullable: true),
                    IdConcepto = table.Column<int>(type: "int", nullable: false),
                    AceptadoPorCliente = table.Column<bool>(type: "bit", nullable: true),
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
                    table.CheckConstraint("CK_Detalles_MaterialCliente", "[OrigenMaterial] IN (N'Taller', N'Cliente') AND [CantidadRecibidaCliente] >= 0 AND [CantidadRecibidaCliente] <= [Cantidad] AND ([OrigenMaterial] <> N'Cliente' OR [PrecioUnitario] = 0) AND ([MaterialClienteConforme] = 0 OR ([OrigenMaterial] = N'Cliente' AND [CantidadRecibidaCliente] > 0 AND [IdTecnicoMaterial] IS NOT NULL AND [FechaRecepcionMaterial] IS NOT NULL))");
                    table.CheckConstraint("CK_PresupuestoDetalles_Avance", "(\n    [EstadoEjecucion] IN (N'NoIniciado', N'Omitido')\n    AND [CantidadEjecutada] = 0\n)\nOR\n(\n    [EstadoEjecucion] IN (N'EnCurso', N'Parcial')\n    AND [CantidadEjecutada] < [Cantidad]\n)\nOR\n(\n    [EstadoEjecucion] = N'Completado'\n    AND [CantidadEjecutada] = [Cantidad]\n)");
                    table.CheckConstraint("CK_PresupuestoDetalles_Cantidad", "[Cantidad] > 0");
                    table.CheckConstraint("CK_PresupuestoDetalles_CantidadEjecutada", "[CantidadEjecutada] >= 0\nAND [CantidadEjecutada] <= [Cantidad]");
                    table.CheckConstraint("CK_PresupuestoDetalles_Descripcion", "LEN(LTRIM(RTRIM([Descripcion]))) > 0");
                    table.CheckConstraint("CK_PresupuestoDetalles_EstadoEjecucion", "[EstadoEjecucion] IN (\n    N'NoIniciado',\n    N'EnCurso',\n    N'Parcial',\n    N'Completado',\n    N'Omitido'\n)");
                    table.CheckConstraint("CK_PresupuestoDetalles_Justificacion", "[EstadoEjecucion] NOT IN (N'Parcial', N'Omitido')\nOR (\n    [ObservacionesEjecucion] IS NOT NULL\n    AND LEN(LTRIM(RTRIM([ObservacionesEjecucion]))) > 0\n)");
                    table.CheckConstraint("CK_PresupuestoDetalles_PrecioUnitario", "[PrecioUnitario] >= 0");
                    table.CheckConstraint("CK_PresupuestoDetalles_PresupuestoCambio", "[IdPresupuestoCambio] IS NULL\nOR [IdPresupuestoCambio] <> [IdPresupuesto]");
                    table.CheckConstraint("CK_PresupuestoDetalles_RegistroTecnico", "(\n    [IdTecnicoRegistro] IS NULL\n    AND [FechaRegistroEjecucion] IS NULL\n)\nOR\n(\n    [IdTecnicoRegistro] IS NOT NULL\n    AND [FechaRegistroEjecucion] IS NOT NULL\n)");
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
                        name: "FK_PresupuestoDetalles_SolicitudesAtencion_IdSolicitudAtencion",
                        column: x => x.IdSolicitudAtencion,
                        principalTable: "SolicitudesAtencion",
                        principalColumn: "IdSolicitudAtencion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PresupuestoDetalles_Usuarios_IdTecnicoMaterial",
                        column: x => x.IdTecnicoMaterial,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PresupuestoDetalles_Usuarios_IdTecnicoRegistro",
                        column: x => x.IdTecnicoRegistro,
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

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Activo", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Administración general del sistema", "Administrador" },
                    { 2, true, "Gestión administrativa y atención al cliente", "Administrativo" },
                    { 3, true, "Gestión técnica de los trabajos del taller", "Técnico" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atenciones_IdCliente",
                table: "Atenciones",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Atenciones_IdTecnicoEnActividad",
                table: "Atenciones",
                column: "IdTecnicoEnActividad");

            migrationBuilder.CreateIndex(
                name: "IX_Atenciones_IdUsuarioRecepcion",
                table: "Atenciones",
                column: "IdUsuarioRecepcion");

            migrationBuilder.CreateIndex(
                name: "UX_Atenciones_VehiculoActivo",
                table: "Atenciones",
                column: "IdVehiculo",
                unique: true,
                filter: "[Estado] <> N'Cerrada' AND [Estado] <> N'Cancelada'");

            migrationBuilder.CreateIndex(
                name: "UX_Categorias_Nombre",
                table: "Categorias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_TipoDocumento_Documento",
                table: "Clientes",
                columns: new[] { "TipoDocumento", "Documento" },
                unique: true);

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
                name: "IX_Conceptos_IdCategoria",
                table: "Conceptos",
                column: "IdCategoria");

            migrationBuilder.CreateIndex(
                name: "UX_Conceptos_Categoria_Tipo_Nombre",
                table: "Conceptos",
                columns: new[] { "IdCategoria", "Tipo", "Nombre" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Diagnosticos_IdTecnicoAutor",
                table: "Diagnosticos",
                column: "IdTecnicoAutor");

            migrationBuilder.CreateIndex(
                name: "UX_Diagnosticos_Atencion",
                table: "Diagnosticos",
                column: "IdAtencion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EvaluacionesAtencion_IdTecnicoUltimaEdicion",
                table: "EvaluacionesAtencion",
                column: "IdTecnicoUltimaEdicion");

            migrationBuilder.CreateIndex(
                name: "IX_EventosAcceso_FechaUtc",
                table: "EventosAcceso",
                column: "FechaUtc");

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
                name: "IX_Marcas_Nombre",
                table: "Marcas",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediosPago_Nombre",
                table: "MediosPago",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modelos_IdMarca_Nombre",
                table: "Modelos",
                columns: new[] { "IdMarca", "Nombre" },
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
                name: "IX_PresupuestoDetalles_IdSolicitudAtencion",
                table: "PresupuestoDetalles",
                column: "IdSolicitudAtencion");

            migrationBuilder.CreateIndex(
                name: "IX_PresupuestoDetalles_IdTecnicoMaterial",
                table: "PresupuestoDetalles",
                column: "IdTecnicoMaterial");

            migrationBuilder.CreateIndex(
                name: "IX_PresupuestoDetalles_IdTecnicoRegistro",
                table: "PresupuestoDetalles",
                column: "IdTecnicoRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_Atencion",
                table: "Presupuestos",
                column: "IdAtencion");

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_IdDiagnostico",
                table: "Presupuestos",
                column: "IdDiagnostico");

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_IdDiagnostico_IdAtencion",
                table: "Presupuestos",
                columns: new[] { "IdDiagnostico", "IdAtencion" });

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_IdHallazgoAtencion",
                table: "Presupuestos",
                column: "IdHallazgoAtencion");

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_IdPresupuestoAnterior",
                table: "Presupuestos",
                column: "IdPresupuestoAnterior");

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_IdUsuarioCreacion",
                table: "Presupuestos",
                column: "IdUsuarioCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Presupuestos_IdUsuarioDecision",
                table: "Presupuestos",
                column: "IdUsuarioDecision");

            migrationBuilder.CreateIndex(
                name: "UX_Presupuestos_BorradorPorAtencion",
                table: "Presupuestos",
                column: "IdAtencion",
                unique: true,
                filter: "[Estado] = N'Borrador' AND [Vigente] = 1 AND [EsAdicional] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Nombre",
                table: "Roles",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesAtencion_IdAtencion_Orden",
                table: "SolicitudesAtencion",
                columns: new[] { "IdAtencion", "Orden" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_NombreUsuario",
                table: "Usuarios",
                column: "NombreUsuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_Dominio",
                table: "Vehiculos",
                column: "Dominio",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_IdModelo",
                table: "Vehiculos",
                column: "IdModelo");

            migrationBuilder.AddForeignKey(
                name: "FK_ComprobanteDetalles_PresupuestoDetalles_IdPresupuestoDetalle",
                table: "ComprobanteDetalles",
                column: "IdPresupuestoDetalle",
                principalTable: "PresupuestoDetalles",
                principalColumn: "IdPresupuestoDetalle",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HallazgosAtencion_PresupuestoDetalles_IdDetalleAfectado",
                table: "HallazgosAtencion",
                column: "IdDetalleAfectado",
                principalTable: "PresupuestoDetalles",
                principalColumn: "IdPresupuestoDetalle",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("""
                CREATE PROCEDURE dbo.Usuario_Crear
                    @NombreUsuario nvarchar(50),
                    @PasswordHash nvarchar(255),
                    @Nombre nvarchar(100),
                    @Apellido nvarchar(100),
                    @RolId int,
                    @FechaAlta datetime2,
                    @Id int OUTPUT
                AS
                BEGIN

                    SET NOCOUNT ON;

                    BEGIN TRY

                        INSERT INTO dbo.Usuarios
                            (NombreUsuario, PasswordHash, Nombre, Apellido, RolId, FechaAlta, Activo)
                        VALUES
                            (@NombreUsuario, @PasswordHash, @Nombre, @Apellido, @RolId, @FechaAlta, 1);

                        SET @Id = CONVERT(int, SCOPE_IDENTITY());
                    END TRY

                    BEGIN CATCH

                        IF ERROR_NUMBER() IN (2601, 2627)
                        
                        BEGIN
                            THROW 51001, 'El nombre de usuario ya esta registrado.', 1;
                        END;

                        THROW;

                    END CATCH;

                END;
                """);

            migrationBuilder.Sql(
                """
                CREATE PROCEDURE dbo.Usuario_ActualizarDatos
                    @Id int,
                    @Nombre nvarchar(100),
                    @Apellido nvarchar(100),
                    @RolId int
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;

                    BEGIN TRY
                        
                        BEGIN TRANSACTION;

                            DECLARE @RolActualId int;
                            DECLARE @Activo bit;

                            SELECT
                                @RolActualId = RolId,
                                @Activo = Activo
                            FROM dbo.Usuarios WITH (TABLOCKX, HOLDLOCK)
                            WHERE Id = @Id;              

                        IF @RolActualId IS NULL
                            THROW 51002, 'El usuario no existe.', 1;

                        IF @RolActualId = 1
                            AND @Activo = 1
                            AND @RolId <> 1
                            AND NOT EXISTS
                            (
                                SELECT 1
                                FROM dbo.Usuarios
                                WHERE RolId = 1
                                    AND Activo = 1
                                    AND Id <> @Id
                            )
                            THROW 51003, 'No se puede cambiar el rol del ultimo administrador activo.', 1;

                        UPDATE dbo.Usuarios
                        SET Nombre = @Nombre,
                            Apellido = @Apellido,
                            RolId = @RolId
                        WHERE Id = @Id;

                        COMMIT TRANSACTION;

                    END TRY

                    BEGIN CATCH
                        
                        IF @@TRANCOUNT > 0
                            ROLLBACK TRANSACTION;

                        THROW;
                    
                    END CATCH

                END;
                """);

            migrationBuilder.Sql(
                """
                CREATE PROCEDURE dbo.Usuario_ActualizarCredenciales
                    @Id int,
                    @NombreUsuario nvarchar(50),
                    @PasswordHash nvarchar(255) = NULL
                AS
                BEGIN
                    SET NOCOUNT ON;

                    BEGIN TRY

                        UPDATE dbo.Usuarios
                        SET NombreUsuario = @NombreUsuario,
                            PasswordHash = COALESCE(@PasswordHash, PasswordHash)
                        WHERE Id = @Id;

                        IF @@ROWCOUNT = 0
                            THROW 51002, 'El usuario no existe.', 1;
                    
                    END TRY

                    BEGIN CATCH
                        
                        IF ERROR_NUMBER() IN (2601, 2627)
                            THROW 51001, 'El nombre de usuario ya esta registrado.', 1;

                        THROW;

                    END CATCH;

                END;
                """);

            migrationBuilder.Sql(
                """
                CREATE PROCEDURE dbo.Usuario_CambiarEstado
                    @Id int,
                    @Activo bit
                AS
                BEGIN
                    SET NOCOUNT ON;
                    SET XACT_ABORT ON;

                    BEGIN TRY

                        BEGIN TRANSACTION;

                        DECLARE @RolActualId int;
                        DECLARE @ActivoActual bit;

                        SELECT
                            @RolActualId = RolId,
                            @ActivoActual = Activo
                        FROM dbo.Usuarios WITH (TABLOCKX, HOLDLOCK)
                        WHERE Id = @Id;

                        IF @RolActualId IS NULL
                            THROW 51002, 'El usuario no existe.', 1;

                        IF @Activo = 0
                           AND @ActivoActual = 1
                           AND @RolActualId = 1
                           AND NOT EXISTS
                           (
                               SELECT 1
                               FROM dbo.Usuarios
                               WHERE RolId = 1
                                 AND Activo = 1
                                 AND Id <> @Id
                           )
                            THROW 51004, 'No se puede desactivar al ultimo administrador activo.', 1;

                        UPDATE dbo.Usuarios
                        SET Activo = @Activo
                        WHERE Id = @Id;

                        COMMIT TRANSACTION;

                    END TRY

                    BEGIN CATCH

                        IF @@TRANCOUNT > 0
                            ROLLBACK TRANSACTION;

                        THROW;

                    END CATCH;

                END
                """);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DROP PROCEDURE dbo.Usuario_CambiarEstado;
                """
             );

            migrationBuilder.Sql(
                """
                DROP PROCEDURE dbo.Usuario_ActualizarCredenciales;
                """
             );

            migrationBuilder.Sql(
                """
                DROP PROCEDURE dbo.Usuario_ActualizarDatos;
                """
            );

            migrationBuilder.Sql(
                """
                DROP PROCEDURE dbo.Usuario_Crear;           
                """
            );


            migrationBuilder.DropForeignKey(
                name: "FK_Atenciones_Clientes_IdCliente",
                table: "Atenciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Atenciones_Usuarios_IdTecnicoEnActividad",
                table: "Atenciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Atenciones_Usuarios_IdUsuarioRecepcion",
                table: "Atenciones");

            migrationBuilder.DropForeignKey(
                name: "FK_Diagnosticos_Usuarios_IdTecnicoAutor",
                table: "Diagnosticos");

            migrationBuilder.DropForeignKey(
                name: "FK_HallazgosAtencion_Usuarios_IdTecnico",
                table: "HallazgosAtencion");

            migrationBuilder.DropForeignKey(
                name: "FK_PresupuestoDetalles_Usuarios_IdTecnicoMaterial",
                table: "PresupuestoDetalles");

            migrationBuilder.DropForeignKey(
                name: "FK_PresupuestoDetalles_Usuarios_IdTecnicoRegistro",
                table: "PresupuestoDetalles");

            migrationBuilder.DropForeignKey(
                name: "FK_Presupuestos_Usuarios_IdUsuarioCreacion",
                table: "Presupuestos");

            migrationBuilder.DropForeignKey(
                name: "FK_Presupuestos_Usuarios_IdUsuarioDecision",
                table: "Presupuestos");

            migrationBuilder.DropForeignKey(
                name: "FK_Atenciones_Vehiculos_IdVehiculo",
                table: "Atenciones");

            migrationBuilder.DropForeignKey(
                name: "FK_HallazgosAtencion_PresupuestoDetalles_IdDetalleAfectado",
                table: "HallazgosAtencion");

            migrationBuilder.DropTable(
                name: "ComprobanteDetalles");

            migrationBuilder.DropTable(
                name: "ControlesAcceso");

            migrationBuilder.DropTable(
                name: "EvaluacionesAtencion");

            migrationBuilder.DropTable(
                name: "EventosAcceso");

            migrationBuilder.DropTable(
                name: "HistorialAtenciones");

            migrationBuilder.DropTable(
                name: "PagoMedios");

            migrationBuilder.DropTable(
                name: "PausasTrabajo");

            migrationBuilder.DropTable(
                name: "MediosPago");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "Comprobantes");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Vehiculos");

            migrationBuilder.DropTable(
                name: "Modelos");

            migrationBuilder.DropTable(
                name: "Marcas");

            migrationBuilder.DropTable(
                name: "PresupuestoDetalles");

            migrationBuilder.DropTable(
                name: "Conceptos");

            migrationBuilder.DropTable(
                name: "Presupuestos");

            migrationBuilder.DropTable(
                name: "SolicitudesAtencion");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Diagnosticos");

            migrationBuilder.DropTable(
                name: "HallazgosAtencion");

            migrationBuilder.DropTable(
                name: "Atenciones");
        }
    }
}
