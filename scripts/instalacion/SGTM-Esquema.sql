IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Categorias] (
    [IdCategoria] int NOT NULL IDENTITY,
    [Nombre] nvarchar(80) NOT NULL,
    [Descripcion] nvarchar(500) NULL,
    [Activo] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_Categorias] PRIMARY KEY ([IdCategoria]),
    CONSTRAINT [CK_Categorias_Nombre] CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0)
);

CREATE TABLE [Clientes] (
    [IdCliente] int NOT NULL IDENTITY,
    [Documento] nvarchar(20) NOT NULL,
    [TipoDocumento] nvarchar(20) NOT NULL,
    [Nombre] nvarchar(80) NOT NULL,
    [Apellido] nvarchar(80) NOT NULL,
    [Telefono] nvarchar(30) NULL,
    [Email] nvarchar(120) NULL,
    [Direccion] nvarchar(200) NULL,
    [Activo] bit NOT NULL,
    [Version] rowversion NOT NULL,
    [FechaAlta] datetime2 NOT NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY ([IdCliente])
);

CREATE TABLE [ControlesAcceso] (
    [Clave] varchar(64) NOT NULL,
    [Intentos] int NOT NULL,
    [VentanaUtc] datetimeoffset NOT NULL,
    [BloqueadoHastaUtc] datetimeoffset NULL,
    CONSTRAINT [PK_ControlesAcceso] PRIMARY KEY ([Clave])
);

CREATE TABLE [EventosAcceso] (
    [Id] bigint NOT NULL IDENTITY,
    [Clave] varchar(64) NOT NULL,
    [IdUsuario] int NULL,
    [Correcto] bit NOT NULL,
    [FechaUtc] datetimeoffset NOT NULL,
    CONSTRAINT [PK_EventosAcceso] PRIMARY KEY ([Id])
);

CREATE TABLE [Marcas] (
    [IdMarca] int NOT NULL IDENTITY,
    [Nombre] nvarchar(50) NOT NULL,
    [Activo] bit NOT NULL,
    CONSTRAINT [PK_Marcas] PRIMARY KEY ([IdMarca])
);

CREATE TABLE [MediosPago] (
    [IdMedioPago] int NOT NULL IDENTITY,
    [Nombre] nvarchar(60) NOT NULL,
    [Activo] bit NOT NULL,
    [RequiereReferencia] bit NOT NULL,
    CONSTRAINT [PK_MediosPago] PRIMARY KEY ([IdMedioPago])
);

CREATE TABLE [Roles] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(50) NOT NULL,
    [Descripcion] nvarchar(150) NULL,
    [Activo] bit NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);

CREATE TABLE [Conceptos] (
    [IdConcepto] int NOT NULL IDENTITY,
    [IdCategoria] int NOT NULL,
    [Tipo] nvarchar(20) NOT NULL,
    [Nombre] nvarchar(150) NOT NULL,
    [Descripcion] nvarchar(500) NULL,
    [PrecioReferencia] decimal(18,2) NULL,
    [Activo] bit NOT NULL,
    CONSTRAINT [PK_Conceptos] PRIMARY KEY ([IdConcepto]),
    CONSTRAINT [CK_Conceptos_Nombre] CHECK (LEN(LTRIM(RTRIM(Nombre))) > 0),
    CONSTRAINT [CK_Conceptos_PrecioReferencia] CHECK ([PrecioReferencia] IS NULL OR
[PrecioReferencia] >= 0),
    CONSTRAINT [CK_Conceptos_Tipo] CHECK ([Tipo] IN (
    N'Servicio',
    N'Repuesto',
    N'Consumible')),
    CONSTRAINT [FK_Conceptos_Categorias_IdCategoria] FOREIGN KEY ([IdCategoria]) REFERENCES [Categorias] ([IdCategoria]) ON DELETE NO ACTION
);

CREATE TABLE [Modelos] (
    [IdModelo] int NOT NULL IDENTITY,
    [Nombre] nvarchar(50) NOT NULL,
    [Activo] bit NOT NULL,
    [IdMarca] int NOT NULL,
    CONSTRAINT [PK_Modelos] PRIMARY KEY ([IdModelo]),
    CONSTRAINT [FK_Modelos_Marcas_IdMarca] FOREIGN KEY ([IdMarca]) REFERENCES [Marcas] ([IdMarca]) ON DELETE NO ACTION
);

CREATE TABLE [Usuarios] (
    [Id] int NOT NULL IDENTITY,
    [NombreUsuario] nvarchar(50) NOT NULL,
    [PasswordHash] nvarchar(255) NOT NULL,
    [Nombre] nvarchar(100) NOT NULL,
    [Apellido] nvarchar(100) NOT NULL,
    [Activo] bit NOT NULL,
    [FechaAlta] datetime2 NOT NULL,
    [RolId] int NOT NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Usuarios_Roles_RolId] FOREIGN KEY ([RolId]) REFERENCES [Roles] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Vehiculos] (
    [IdVehiculo] int NOT NULL IDENTITY,
    [Dominio] nvarchar(10) NOT NULL,
    [Anio] int NOT NULL,
    [Color] nvarchar(50) NOT NULL,
    [Activo] bit NOT NULL,
    [Version] rowversion NOT NULL,
    [FechaAlta] datetime2 NOT NULL,
    [IdModelo] int NOT NULL,
    CONSTRAINT [PK_Vehiculos] PRIMARY KEY ([IdVehiculo]),
    CONSTRAINT [FK_Vehiculos_Modelos_IdModelo] FOREIGN KEY ([IdModelo]) REFERENCES [Modelos] ([IdModelo]) ON DELETE NO ACTION
);

CREATE TABLE [Atenciones] (
    [IdAtencion] int NOT NULL IDENTITY,
    [KilometrajeRecepcion] int NULL,
    [ObservacionesRecepcion] nvarchar(1000) NULL,
    [IdCliente] int NOT NULL,
    [IdVehiculo] int NOT NULL,
    [IdUsuarioRecepcion] int NOT NULL,
    [IdTecnicoEnActividad] int NULL,
    [ActividadTecnica] nvarchar(20) NULL,
    [FechaApertura] datetime2 NOT NULL,
    [TipoSolicitud] nvarchar(20) NOT NULL,
    [MotivoConsulta] nvarchar(500) NOT NULL,
    [Estado] nvarchar(30) NOT NULL,
    [FechaCierre] datetime2 NULL,
    [Version] rowversion NOT NULL,
    CONSTRAINT [PK_Atenciones] PRIMARY KEY ([IdAtencion]),
    CONSTRAINT [CK_Atenciones_ActividadTecnica] CHECK ([ActividadTecnica] IS NULL
OR [ActividadTecnica] IN (
    N'Diagnostico',
    N'Presupuesto',
    N'Ejecucion'
)),
    CONSTRAINT [CK_Atenciones_Estado] CHECK ([Estado] IN (
    N'EnEvaluacionTecnica',
    N'PendienteDecision',
    N'EnEjecucion',
    N'TrabajoFinalizado',
    N'PendienteLiquidacion',
    N'Cerrada',
    N'Cancelada'
)),
    CONSTRAINT [CK_Atenciones_KilometrajeRecepcion] CHECK ([KilometrajeRecepcion] IS NULL OR [KilometrajeRecepcion] >= 0),
    CONSTRAINT [CK_Atenciones_ReservaTecnica] CHECK ((
    [IdTecnicoEnActividad] IS NULL
    AND [ActividadTecnica] IS NULL
)
OR
(
    [IdTecnicoEnActividad] IS NOT NULL
    AND [ActividadTecnica] IS NOT NULL
)),
    CONSTRAINT [CK_Atenciones_TipoSolicitud] CHECK ([TipoSolicitud] IN (N'Problema', N'ServicioDefinido', N'Mixta')),
    CONSTRAINT [FK_Atenciones_Clientes_IdCliente] FOREIGN KEY ([IdCliente]) REFERENCES [Clientes] ([IdCliente]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Atenciones_Usuarios_IdTecnicoEnActividad] FOREIGN KEY ([IdTecnicoEnActividad]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Atenciones_Usuarios_IdUsuarioRecepcion] FOREIGN KEY ([IdUsuarioRecepcion]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Atenciones_Vehiculos_IdVehiculo] FOREIGN KEY ([IdVehiculo]) REFERENCES [Vehiculos] ([IdVehiculo]) ON DELETE NO ACTION
);

CREATE TABLE [Comprobantes] (
    [IdComprobante] int NOT NULL IDENTITY,
    [IdAtencion] int NOT NULL,
    [IdUsuarioEmision] int NOT NULL,
    [FechaEmision] datetime2 NOT NULL,
    [Cliente] nvarchar(200) NOT NULL,
    [DocumentoCliente] nvarchar(60) NOT NULL,
    [Dominio] nvarchar(20) NOT NULL,
    [Moneda] nvarchar(3) NOT NULL,
    [Total] decimal(18,2) NOT NULL,
    [TotalPagado] decimal(18,2) NOT NULL,
    [Observaciones] nvarchar(1000) NULL,
    [Version] rowversion NOT NULL,
    CONSTRAINT [PK_Comprobantes] PRIMARY KEY ([IdComprobante]),
    CONSTRAINT [CK_Comprobantes_Importes] CHECK ([Total] >= 0 AND [TotalPagado] >= 0 AND [TotalPagado] <= [Total]),
    CONSTRAINT [CK_Comprobantes_SinCargo] CHECK ([Total] > 0 OR LEN(LTRIM(RTRIM(COALESCE([Observaciones], N'')))) > 0),
    CONSTRAINT [FK_Comprobantes_Atenciones_IdAtencion] FOREIGN KEY ([IdAtencion]) REFERENCES [Atenciones] ([IdAtencion]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Comprobantes_Usuarios_IdUsuarioEmision] FOREIGN KEY ([IdUsuarioEmision]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Diagnosticos] (
    [IdDiagnostico] int NOT NULL IDENTITY,
    [IdAtencion] int NOT NULL,
    [IdTecnicoAutor] int NOT NULL,
    [FechaDiagnostico] datetime2(0) NOT NULL,
    [FallaDetectada] nvarchar(1000) NOT NULL,
    [TrabajoRecomendado] nvarchar(1000) NOT NULL,
    [Kilometraje] int NULL,
    [Observaciones] nvarchar(1000) NULL,
    CONSTRAINT [PK_Diagnosticos] PRIMARY KEY ([IdDiagnostico]),
    CONSTRAINT [AK_Diagnosticos_IdDiagnostico_IdAtencion] UNIQUE ([IdDiagnostico], [IdAtencion]),
    CONSTRAINT [CK_Diagnosticos_FallaDetectada] CHECK (LEN(LTRIM(RTRIM([FallaDetectada]))) > 0),
    CONSTRAINT [CK_Diagnosticos_Kilometraje] CHECK ([Kilometraje] IS NULL OR [Kilometraje] >= 0),
    CONSTRAINT [CK_Diagnosticos_TrabajoRecomendado] CHECK (LEN(LTRIM(RTRIM([TrabajoRecomendado]))) > 0),
    CONSTRAINT [FK_Diagnosticos_Atenciones_IdAtencion] FOREIGN KEY ([IdAtencion]) REFERENCES [Atenciones] ([IdAtencion]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Diagnosticos_Usuarios_IdTecnicoAutor] FOREIGN KEY ([IdTecnicoAutor]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [EvaluacionesAtencion] (
    [IdAtencion] int NOT NULL,
    [IncluyeDiagnostico] bit NOT NULL,
    [FallaDetectada] nvarchar(1000) NOT NULL,
    [TrabajoRecomendado] nvarchar(1000) NOT NULL,
    [Observaciones] nvarchar(1000) NULL,
    [Kilometraje] int NULL,
    [SoloEvaluacion] bit NOT NULL,
    [Finalizada] bit NOT NULL,
    [IdTecnicoUltimaEdicion] int NOT NULL,
    [FechaActualizacion] datetime2 NOT NULL,
    [Version] rowversion NOT NULL,
    CONSTRAINT [PK_EvaluacionesAtencion] PRIMARY KEY ([IdAtencion]),
    CONSTRAINT [CK_EvaluacionesAtencion_Kilometraje] CHECK ([Kilometraje] IS NULL OR [Kilometraje] >= 0),
    CONSTRAINT [FK_EvaluacionesAtencion_Atenciones_IdAtencion] FOREIGN KEY ([IdAtencion]) REFERENCES [Atenciones] ([IdAtencion]) ON DELETE NO ACTION,
    CONSTRAINT [FK_EvaluacionesAtencion_Usuarios_IdTecnicoUltimaEdicion] FOREIGN KEY ([IdTecnicoUltimaEdicion]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [SolicitudesAtencion] (
    [IdSolicitudAtencion] int NOT NULL IDENTITY,
    [IdAtencion] int NOT NULL,
    [Orden] int NOT NULL,
    [Tipo] nvarchar(20) NOT NULL,
    [Descripcion] nvarchar(500) NOT NULL,
    CONSTRAINT [PK_SolicitudesAtencion] PRIMARY KEY ([IdSolicitudAtencion]),
    CONSTRAINT [CK_SolicitudesAtencion_Descripcion] CHECK (LEN(LTRIM(RTRIM([Descripcion]))) > 0),
    CONSTRAINT [CK_SolicitudesAtencion_Orden] CHECK ([Orden] > 0),
    CONSTRAINT [CK_SolicitudesAtencion_Tipo] CHECK ([Tipo] IN (N'TrabajoSolicitado', N'ProblemaInformado')),
    CONSTRAINT [FK_SolicitudesAtencion_Atenciones_IdAtencion] FOREIGN KEY ([IdAtencion]) REFERENCES [Atenciones] ([IdAtencion]) ON DELETE NO ACTION
);

CREATE TABLE [Pagos] (
    [IdPago] int NOT NULL IDENTITY,
    [IdOperacion] uniqueidentifier NOT NULL,
    [IdComprobante] int NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [IdUsuarioRegistro] int NOT NULL,
    [UsuarioRegistroNombre] nvarchar(200) NOT NULL,
    [Importe] decimal(18,2) NOT NULL,
    [SaldoPosterior] decimal(18,2) NOT NULL,
    [Anulado] bit NOT NULL,
    [MotivoAnulacion] nvarchar(1000) NULL,
    [FechaAnulacion] datetime2 NULL,
    [IdUsuarioAnulacion] int NULL,
    CONSTRAINT [PK_Pagos] PRIMARY KEY ([IdPago]),
    CONSTRAINT [CK_Pagos_Anulacion] CHECK (([Anulado] = 0 AND [MotivoAnulacion] IS NULL AND [FechaAnulacion] IS NULL AND [IdUsuarioAnulacion] IS NULL) OR ([Anulado] = 1 AND LEN(LTRIM(RTRIM(COALESCE([MotivoAnulacion], N'')))) > 0 AND [FechaAnulacion] IS NOT NULL AND [IdUsuarioAnulacion] IS NOT NULL)),
    CONSTRAINT [CK_Pagos_Importe] CHECK ([Importe] > 0 AND [SaldoPosterior] >= 0),
    CONSTRAINT [FK_Pagos_Comprobantes_IdComprobante] FOREIGN KEY ([IdComprobante]) REFERENCES [Comprobantes] ([IdComprobante]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Pagos_Usuarios_IdUsuarioAnulacion] FOREIGN KEY ([IdUsuarioAnulacion]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Pagos_Usuarios_IdUsuarioRegistro] FOREIGN KEY ([IdUsuarioRegistro]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [HistorialAtenciones] (
    [IdHistorialAtencion] int NOT NULL IDENTITY,
    [IdAtencion] int NOT NULL,
    [IdUsuario] int NOT NULL,
    [IdDiagnostico] int NULL,
    [Fecha] datetime2(0) NOT NULL,
    [Accion] nvarchar(80) NOT NULL,
    [EstadoAnterior] nvarchar(30) NULL,
    [EstadoNuevo] nvarchar(30) NOT NULL,
    [Observaciones] nvarchar(2000) NULL,
    CONSTRAINT [PK_HistorialAtenciones] PRIMARY KEY ([IdHistorialAtencion]),
    CONSTRAINT [CK_HistorialAtenciones_Accion] CHECK (LEN(LTRIM(RTRIM([Accion]))) > 0),
    CONSTRAINT [CK_HistorialAtenciones_EstadoAnterior] CHECK ([EstadoAnterior] IS NULL
OR [EstadoAnterior] IN (
    N'EnEvaluacionTecnica',
    N'PendienteDecision',
    N'EnEjecucion',
    N'TrabajoFinalizado',
    N'PendienteLiquidacion',
    N'Cerrada',
    N'Cancelada'
)),
    CONSTRAINT [CK_HistorialAtenciones_EstadoNuevo] CHECK ([EstadoNuevo] IN (
    N'EnEvaluacionTecnica',
    N'PendienteDecision',
    N'EnEjecucion',
    N'TrabajoFinalizado',
    N'PendienteLiquidacion',
    N'Cerrada',
    N'Cancelada'
)),
    CONSTRAINT [FK_HistorialAtenciones_Atenciones_IdAtencion] FOREIGN KEY ([IdAtencion]) REFERENCES [Atenciones] ([IdAtencion]) ON DELETE NO ACTION,
    CONSTRAINT [FK_HistorialAtenciones_Diagnosticos_IdDiagnostico] FOREIGN KEY ([IdDiagnostico]) REFERENCES [Diagnosticos] ([IdDiagnostico]) ON DELETE NO ACTION,
    CONSTRAINT [FK_HistorialAtenciones_Usuarios_IdUsuario] FOREIGN KEY ([IdUsuario]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [PagoMedios] (
    [IdPagoMedio] int NOT NULL IDENTITY,
    [IdPago] int NOT NULL,
    [NombreMedio] nvarchar(60) NOT NULL,
    [Orden] int NOT NULL,
    [IdMedioPago] int NOT NULL,
    [Importe] decimal(18,2) NOT NULL,
    [Referencia] nvarchar(200) NULL,
    CONSTRAINT [PK_PagoMedios] PRIMARY KEY ([IdPagoMedio]),
    CONSTRAINT [CK_PagoMedios_Importe] CHECK ([Importe] > 0 AND [Orden] >= 0),
    CONSTRAINT [FK_PagoMedios_MediosPago_IdMedioPago] FOREIGN KEY ([IdMedioPago]) REFERENCES [MediosPago] ([IdMedioPago]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PagoMedios_Pagos_IdPago] FOREIGN KEY ([IdPago]) REFERENCES [Pagos] ([IdPago]) ON DELETE NO ACTION
);

CREATE TABLE [ComprobanteDetalles] (
    [IdComprobanteDetalle] int NOT NULL IDENTITY,
    [IdComprobante] int NOT NULL,
    [IdPresupuestoDetalle] int NOT NULL,
    [Descripcion] nvarchar(500) NOT NULL,
    [Cantidad] decimal(18,3) NOT NULL,
    [PrecioUnitario] decimal(18,2) NOT NULL,
    [Subtotal] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_ComprobanteDetalles] PRIMARY KEY ([IdComprobanteDetalle]),
    CONSTRAINT [CK_ComprobanteDetalles_Importes] CHECK ([Cantidad] > 0 AND [PrecioUnitario] >= 0 AND [Subtotal] = ROUND([Cantidad] * [PrecioUnitario], 2)),
    CONSTRAINT [FK_ComprobanteDetalles_Comprobantes_IdComprobante] FOREIGN KEY ([IdComprobante]) REFERENCES [Comprobantes] ([IdComprobante]) ON DELETE NO ACTION
);

CREATE TABLE [HallazgosAtencion] (
    [IdHallazgoAtencion] int NOT NULL IDENTITY,
    [IdAtencion] int NOT NULL,
    [IdTecnico] int NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [Descripcion] nvarchar(1000) NOT NULL,
    [Recomendacion] nvarchar(1000) NOT NULL,
    [ImpideContinuar] bit NOT NULL,
    [IdDetalleAfectado] int NULL,
    CONSTRAINT [PK_HallazgosAtencion] PRIMARY KEY ([IdHallazgoAtencion]),
    CONSTRAINT [CK_Hallazgos_Textos] CHECK (LEN(LTRIM(RTRIM([Descripcion]))) > 0 AND LEN(LTRIM(RTRIM([Recomendacion]))) > 0),
    CONSTRAINT [FK_HallazgosAtencion_Atenciones_IdAtencion] FOREIGN KEY ([IdAtencion]) REFERENCES [Atenciones] ([IdAtencion]) ON DELETE NO ACTION,
    CONSTRAINT [FK_HallazgosAtencion_Usuarios_IdTecnico] FOREIGN KEY ([IdTecnico]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [Presupuestos] (
    [IdPresupuesto] int NOT NULL IDENTITY,
    [PersonaDecision] nvarchar(150) NULL,
    [MedioDecision] nvarchar(30) NULL,
    [IdPresupuestoAnterior] int NULL,
    [EsInspeccion] bit NOT NULL,
    [EsAdicional] bit NOT NULL,
    [IdHallazgoAtencion] int NULL,
    [Vigente] bit NOT NULL,
    [MotivoRevision] nvarchar(1000) NULL,
    [ObservacionesDecision] nvarchar(1000) NULL,
    [AlcanceValidado] bit NOT NULL,
    [IdAtencion] int NOT NULL,
    [IdDiagnostico] int NULL,
    [IdUsuarioCreacion] int NOT NULL,
    [FechaCreacion] datetime2(0) NOT NULL,
    [Estado] nvarchar(30) NOT NULL,
    [FechaPresentacion] datetime2(0) NULL,
    [IdUsuarioDecision] int NULL,
    [FechaDecision] datetime2(0) NULL,
    [Observaciones] nvarchar(1000) NULL,
    [Version] rowversion NOT NULL,
    CONSTRAINT [PK_Presupuestos] PRIMARY KEY ([IdPresupuesto]),
    CONSTRAINT [CK_Presupuestos_Adicional] CHECK (([EsInspeccion] = 0 OR [EsAdicional] = 1) AND ([IdHallazgoAtencion] IS NULL OR [EsAdicional] = 1)),
    CONSTRAINT [CK_Presupuestos_Decision] CHECK ((
    [Estado] IN (
        N'Borrador',
    N'PendientePresentacion',
        N'PendienteDecision'
    )
    AND [IdUsuarioDecision] IS NULL
    AND [FechaDecision] IS NULL
)
OR
(
    [Estado] IN (N'Aceptado', N'Rechazado', N'PendienteRevisionTecnica')
    AND [IdUsuarioDecision] IS NOT NULL
    AND [FechaDecision] IS NOT NULL
)),
    CONSTRAINT [CK_Presupuestos_Estado] CHECK ([Estado] IN (
    N'Borrador',
    N'PendientePresentacion',
    N'PendienteDecision',
    N'Aceptado',
    N'Rechazado', N'PendienteRevisionTecnica'
)),
    CONSTRAINT [CK_Presupuestos_Fechas] CHECK ((
    [FechaPresentacion] IS NULL
    OR [FechaPresentacion] >= [FechaCreacion]
)
AND
(
    [FechaDecision] IS NULL
    OR (
        [FechaPresentacion] IS NOT NULL
        AND [FechaDecision] >= [FechaPresentacion]
    )
)),
    CONSTRAINT [CK_Presupuestos_Presentacion] CHECK ((
    [Estado] IN (N'Borrador', N'PendientePresentacion')
    AND [FechaPresentacion] IS NULL
)
OR
(
    [Estado] IN (
        N'PendienteDecision',
        N'Aceptado',
        N'Rechazado', N'PendienteRevisionTecnica'
    )
    AND [FechaPresentacion] IS NOT NULL
)),
    CONSTRAINT [FK_Presupuestos_Atenciones_IdAtencion] FOREIGN KEY ([IdAtencion]) REFERENCES [Atenciones] ([IdAtencion]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Presupuestos_Diagnosticos_IdDiagnostico_IdAtencion] FOREIGN KEY ([IdDiagnostico], [IdAtencion]) REFERENCES [Diagnosticos] ([IdDiagnostico], [IdAtencion]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Presupuestos_HallazgosAtencion_IdHallazgoAtencion] FOREIGN KEY ([IdHallazgoAtencion]) REFERENCES [HallazgosAtencion] ([IdHallazgoAtencion]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Presupuestos_Presupuestos_IdPresupuestoAnterior] FOREIGN KEY ([IdPresupuestoAnterior]) REFERENCES [Presupuestos] ([IdPresupuesto]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Presupuestos_Usuarios_IdUsuarioCreacion] FOREIGN KEY ([IdUsuarioCreacion]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Presupuestos_Usuarios_IdUsuarioDecision] FOREIGN KEY ([IdUsuarioDecision]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [PresupuestoDetalles] (
    [IdPresupuestoDetalle] int NOT NULL IDENTITY,
    [OrigenMaterial] nvarchar(20) NOT NULL DEFAULT N'Taller',
    [CantidadRecibidaCliente] decimal(18,3) NOT NULL,
    [MaterialClienteConforme] bit NOT NULL,
    [ObservacionesMaterial] nvarchar(1000) NULL,
    [IdTecnicoMaterial] int NULL,
    [FechaRecepcionMaterial] datetime2 NULL,
    [IdPresupuesto] int NOT NULL,
    [IdSolicitudAtencion] int NULL,
    [IdConcepto] int NOT NULL,
    [AceptadoPorCliente] bit NULL,
    [Descripcion] nvarchar(500) NOT NULL,
    [Cantidad] decimal(18,3) NOT NULL,
    [PrecioUnitario] decimal(18,2) NOT NULL,
    [EstadoEjecucion] nvarchar(20) NOT NULL,
    [CantidadEjecutada] decimal(18,3) NOT NULL,
    [IdTecnicoRegistro] int NULL,
    [FechaRegistroEjecucion] datetime2(0) NULL,
    [ObservacionesEjecucion] nvarchar(1000) NULL,
    [IdPresupuestoCambio] int NULL,
    CONSTRAINT [PK_PresupuestoDetalles] PRIMARY KEY ([IdPresupuestoDetalle]),
    CONSTRAINT [CK_Detalles_MaterialCliente] CHECK ([OrigenMaterial] IN (N'Taller', N'Cliente') AND [CantidadRecibidaCliente] >= 0 AND [CantidadRecibidaCliente] <= [Cantidad] AND ([OrigenMaterial] <> N'Cliente' OR [PrecioUnitario] = 0) AND ([MaterialClienteConforme] = 0 OR ([OrigenMaterial] = N'Cliente' AND [CantidadRecibidaCliente] > 0 AND [IdTecnicoMaterial] IS NOT NULL AND [FechaRecepcionMaterial] IS NOT NULL))),
    CONSTRAINT [CK_PresupuestoDetalles_Avance] CHECK ((
    [EstadoEjecucion] IN (N'NoIniciado', N'Omitido')
    AND [CantidadEjecutada] = 0
)
OR
(
    [EstadoEjecucion] IN (N'EnCurso', N'Parcial')
    AND [CantidadEjecutada] < [Cantidad]
)
OR
(
    [EstadoEjecucion] = N'Completado'
    AND [CantidadEjecutada] = [Cantidad]
)),
    CONSTRAINT [CK_PresupuestoDetalles_Cantidad] CHECK ([Cantidad] > 0),
    CONSTRAINT [CK_PresupuestoDetalles_CantidadEjecutada] CHECK ([CantidadEjecutada] >= 0
AND [CantidadEjecutada] <= [Cantidad]),
    CONSTRAINT [CK_PresupuestoDetalles_Descripcion] CHECK (LEN(LTRIM(RTRIM([Descripcion]))) > 0),
    CONSTRAINT [CK_PresupuestoDetalles_EstadoEjecucion] CHECK ([EstadoEjecucion] IN (
    N'NoIniciado',
    N'EnCurso',
    N'Parcial',
    N'Completado',
    N'Omitido'
)),
    CONSTRAINT [CK_PresupuestoDetalles_Justificacion] CHECK ([EstadoEjecucion] NOT IN (N'Parcial', N'Omitido')
OR (
    [ObservacionesEjecucion] IS NOT NULL
    AND LEN(LTRIM(RTRIM([ObservacionesEjecucion]))) > 0
)),
    CONSTRAINT [CK_PresupuestoDetalles_PrecioUnitario] CHECK ([PrecioUnitario] >= 0),
    CONSTRAINT [CK_PresupuestoDetalles_PresupuestoCambio] CHECK ([IdPresupuestoCambio] IS NULL
OR [IdPresupuestoCambio] <> [IdPresupuesto]),
    CONSTRAINT [CK_PresupuestoDetalles_RegistroTecnico] CHECK ((
    [IdTecnicoRegistro] IS NULL
    AND [FechaRegistroEjecucion] IS NULL
)
OR
(
    [IdTecnicoRegistro] IS NOT NULL
    AND [FechaRegistroEjecucion] IS NOT NULL
)),
    CONSTRAINT [FK_PresupuestoDetalles_Conceptos_IdConcepto] FOREIGN KEY ([IdConcepto]) REFERENCES [Conceptos] ([IdConcepto]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PresupuestoDetalles_Presupuestos_IdPresupuesto] FOREIGN KEY ([IdPresupuesto]) REFERENCES [Presupuestos] ([IdPresupuesto]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PresupuestoDetalles_Presupuestos_IdPresupuestoCambio] FOREIGN KEY ([IdPresupuestoCambio]) REFERENCES [Presupuestos] ([IdPresupuesto]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PresupuestoDetalles_SolicitudesAtencion_IdSolicitudAtencion] FOREIGN KEY ([IdSolicitudAtencion]) REFERENCES [SolicitudesAtencion] ([IdSolicitudAtencion]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PresupuestoDetalles_Usuarios_IdTecnicoMaterial] FOREIGN KEY ([IdTecnicoMaterial]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PresupuestoDetalles_Usuarios_IdTecnicoRegistro] FOREIGN KEY ([IdTecnicoRegistro]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [PausasTrabajo] (
    [IdPausaTrabajo] int NOT NULL IDENTITY,
    [IdAtencion] int NOT NULL,
    [IdDetalle] int NULL,
    [IdUsuario] int NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [Motivo] nvarchar(1000) NOT NULL,
    [FechaResolucion] datetime2 NULL,
    [IdTecnicoResolucion] int NULL,
    [Resolucion] nvarchar(1000) NULL,
    CONSTRAINT [PK_PausasTrabajo] PRIMARY KEY ([IdPausaTrabajo]),
    CONSTRAINT [CK_Pausas_Motivo] CHECK (LEN(LTRIM(RTRIM([Motivo]))) > 0),
    CONSTRAINT [CK_Pausas_Resolucion] CHECK (([FechaResolucion] IS NULL AND [IdTecnicoResolucion] IS NULL AND [Resolucion] IS NULL) OR ([FechaResolucion] >= [Fecha] AND [IdTecnicoResolucion] IS NOT NULL AND LEN(LTRIM(RTRIM([Resolucion]))) > 0)),
    CONSTRAINT [FK_PausasTrabajo_Atenciones_IdAtencion] FOREIGN KEY ([IdAtencion]) REFERENCES [Atenciones] ([IdAtencion]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PausasTrabajo_PresupuestoDetalles_IdDetalle] FOREIGN KEY ([IdDetalle]) REFERENCES [PresupuestoDetalles] ([IdPresupuestoDetalle]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PausasTrabajo_Usuarios_IdTecnicoResolucion] FOREIGN KEY ([IdTecnicoResolucion]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PausasTrabajo_Usuarios_IdUsuario] FOREIGN KEY ([IdUsuario]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdMedioPago', N'Activo', N'Nombre', N'RequiereReferencia') AND [object_id] = OBJECT_ID(N'[MediosPago]'))
    SET IDENTITY_INSERT [MediosPago] ON;
INSERT INTO [MediosPago] ([IdMedioPago], [Activo], [Nombre], [RequiereReferencia])
VALUES (1, CAST(1 AS bit), N'Efectivo', CAST(0 AS bit)),
(2, CAST(1 AS bit), N'Transferencia', CAST(1 AS bit)),
(3, CAST(1 AS bit), N'Tarjeta de débito', CAST(1 AS bit)),
(4, CAST(1 AS bit), N'Tarjeta de crédito', CAST(1 AS bit)),
(5, CAST(1 AS bit), N'Billetera virtual', CAST(1 AS bit));
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'IdMedioPago', N'Activo', N'Nombre', N'RequiereReferencia') AND [object_id] = OBJECT_ID(N'[MediosPago]'))
    SET IDENTITY_INSERT [MediosPago] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activo', N'Descripcion', N'Nombre') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] ON;
INSERT INTO [Roles] ([Id], [Activo], [Descripcion], [Nombre])
VALUES (1, CAST(1 AS bit), N'Administración general del sistema', N'Administrador'),
(2, CAST(1 AS bit), N'Gestión administrativa y atención al cliente', N'Administrativo'),
(3, CAST(1 AS bit), N'Gestión técnica de los trabajos del taller', N'Técnico');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activo', N'Descripcion', N'Nombre') AND [object_id] = OBJECT_ID(N'[Roles]'))
    SET IDENTITY_INSERT [Roles] OFF;

CREATE INDEX [IX_Atenciones_IdCliente] ON [Atenciones] ([IdCliente]);

CREATE INDEX [IX_Atenciones_IdTecnicoEnActividad] ON [Atenciones] ([IdTecnicoEnActividad]);

CREATE INDEX [IX_Atenciones_IdUsuarioRecepcion] ON [Atenciones] ([IdUsuarioRecepcion]);

CREATE UNIQUE INDEX [UX_Atenciones_VehiculoActivo] ON [Atenciones] ([IdVehiculo]) WHERE [Estado] <> N'Cerrada' AND [Estado] <> N'Cancelada';

CREATE UNIQUE INDEX [UX_Categorias_Nombre] ON [Categorias] ([Nombre]);

CREATE UNIQUE INDEX [IX_Clientes_TipoDocumento_Documento] ON [Clientes] ([TipoDocumento], [Documento]);

CREATE INDEX [IX_ComprobanteDetalles_IdComprobante] ON [ComprobanteDetalles] ([IdComprobante]);

CREATE UNIQUE INDEX [IX_ComprobanteDetalles_IdPresupuestoDetalle] ON [ComprobanteDetalles] ([IdPresupuestoDetalle]);

CREATE UNIQUE INDEX [IX_Comprobantes_IdAtencion] ON [Comprobantes] ([IdAtencion]);

CREATE INDEX [IX_Comprobantes_IdUsuarioEmision] ON [Comprobantes] ([IdUsuarioEmision]);

CREATE INDEX [IX_Conceptos_IdCategoria] ON [Conceptos] ([IdCategoria]);

CREATE UNIQUE INDEX [UX_Conceptos_Categoria_Tipo_Nombre] ON [Conceptos] ([IdCategoria], [Tipo], [Nombre]);

CREATE INDEX [IX_Diagnosticos_IdTecnicoAutor] ON [Diagnosticos] ([IdTecnicoAutor]);

CREATE UNIQUE INDEX [UX_Diagnosticos_Atencion] ON [Diagnosticos] ([IdAtencion]);

CREATE INDEX [IX_EvaluacionesAtencion_IdTecnicoUltimaEdicion] ON [EvaluacionesAtencion] ([IdTecnicoUltimaEdicion]);

CREATE INDEX [IX_EventosAcceso_FechaUtc] ON [EventosAcceso] ([FechaUtc]);

CREATE INDEX [IX_HallazgosAtencion_IdAtencion] ON [HallazgosAtencion] ([IdAtencion]);

CREATE INDEX [IX_HallazgosAtencion_IdDetalleAfectado] ON [HallazgosAtencion] ([IdDetalleAfectado]);

CREATE INDEX [IX_HallazgosAtencion_IdTecnico] ON [HallazgosAtencion] ([IdTecnico]);

CREATE INDEX [IX_HistorialAtenciones_Atencion_Fecha_Id] ON [HistorialAtenciones] ([IdAtencion], [Fecha], [IdHistorialAtencion]);

CREATE INDEX [IX_HistorialAtenciones_IdDiagnostico] ON [HistorialAtenciones] ([IdDiagnostico]);

CREATE INDEX [IX_HistorialAtenciones_IdUsuario] ON [HistorialAtenciones] ([IdUsuario]);

CREATE UNIQUE INDEX [IX_Marcas_Nombre] ON [Marcas] ([Nombre]);

CREATE UNIQUE INDEX [IX_MediosPago_Nombre] ON [MediosPago] ([Nombre]);

CREATE UNIQUE INDEX [IX_Modelos_IdMarca_Nombre] ON [Modelos] ([IdMarca], [Nombre]);

CREATE INDEX [IX_PagoMedios_IdMedioPago] ON [PagoMedios] ([IdMedioPago]);

CREATE UNIQUE INDEX [IX_PagoMedios_IdPago_Orden] ON [PagoMedios] ([IdPago], [Orden]);

CREATE INDEX [IX_Pagos_IdComprobante] ON [Pagos] ([IdComprobante]);

CREATE UNIQUE INDEX [IX_Pagos_IdOperacion] ON [Pagos] ([IdOperacion]);

CREATE INDEX [IX_Pagos_IdUsuarioAnulacion] ON [Pagos] ([IdUsuarioAnulacion]);

CREATE INDEX [IX_Pagos_IdUsuarioRegistro] ON [Pagos] ([IdUsuarioRegistro]);

CREATE INDEX [IX_PausasTrabajo_IdAtencion] ON [PausasTrabajo] ([IdAtencion]);

CREATE INDEX [IX_PausasTrabajo_IdDetalle] ON [PausasTrabajo] ([IdDetalle]);

CREATE INDEX [IX_PausasTrabajo_IdTecnicoResolucion] ON [PausasTrabajo] ([IdTecnicoResolucion]);

CREATE INDEX [IX_PausasTrabajo_IdUsuario] ON [PausasTrabajo] ([IdUsuario]);

CREATE INDEX [IX_PresupuestoDetalles_IdConcepto] ON [PresupuestoDetalles] ([IdConcepto]);

CREATE INDEX [IX_PresupuestoDetalles_IdPresupuesto] ON [PresupuestoDetalles] ([IdPresupuesto]);

CREATE INDEX [IX_PresupuestoDetalles_IdPresupuestoCambio] ON [PresupuestoDetalles] ([IdPresupuestoCambio]);

CREATE INDEX [IX_PresupuestoDetalles_IdSolicitudAtencion] ON [PresupuestoDetalles] ([IdSolicitudAtencion]);

CREATE INDEX [IX_PresupuestoDetalles_IdTecnicoMaterial] ON [PresupuestoDetalles] ([IdTecnicoMaterial]);

CREATE INDEX [IX_PresupuestoDetalles_IdTecnicoRegistro] ON [PresupuestoDetalles] ([IdTecnicoRegistro]);

CREATE INDEX [IX_Presupuestos_Atencion] ON [Presupuestos] ([IdAtencion]);

CREATE INDEX [IX_Presupuestos_IdDiagnostico] ON [Presupuestos] ([IdDiagnostico]);

CREATE INDEX [IX_Presupuestos_IdDiagnostico_IdAtencion] ON [Presupuestos] ([IdDiagnostico], [IdAtencion]);

CREATE INDEX [IX_Presupuestos_IdHallazgoAtencion] ON [Presupuestos] ([IdHallazgoAtencion]);

CREATE INDEX [IX_Presupuestos_IdPresupuestoAnterior] ON [Presupuestos] ([IdPresupuestoAnterior]);

CREATE INDEX [IX_Presupuestos_IdUsuarioCreacion] ON [Presupuestos] ([IdUsuarioCreacion]);

CREATE INDEX [IX_Presupuestos_IdUsuarioDecision] ON [Presupuestos] ([IdUsuarioDecision]);

CREATE UNIQUE INDEX [UX_Presupuestos_BorradorPorAtencion] ON [Presupuestos] ([IdAtencion]) WHERE [Estado] = N'Borrador' AND [Vigente] = 1 AND [EsAdicional] = 0;

CREATE UNIQUE INDEX [IX_Roles_Nombre] ON [Roles] ([Nombre]);

CREATE UNIQUE INDEX [IX_SolicitudesAtencion_IdAtencion_Orden] ON [SolicitudesAtencion] ([IdAtencion], [Orden]);

CREATE UNIQUE INDEX [IX_Usuarios_NombreUsuario] ON [Usuarios] ([NombreUsuario]);

CREATE INDEX [IX_Usuarios_RolId] ON [Usuarios] ([RolId]);

CREATE UNIQUE INDEX [IX_Vehiculos_Dominio] ON [Vehiculos] ([Dominio]);

CREATE INDEX [IX_Vehiculos_IdModelo] ON [Vehiculos] ([IdModelo]);

ALTER TABLE [ComprobanteDetalles] ADD CONSTRAINT [FK_ComprobanteDetalles_PresupuestoDetalles_IdPresupuestoDetalle] FOREIGN KEY ([IdPresupuestoDetalle]) REFERENCES [PresupuestoDetalles] ([IdPresupuestoDetalle]) ON DELETE NO ACTION;

ALTER TABLE [HallazgosAtencion] ADD CONSTRAINT [FK_HallazgosAtencion_PresupuestoDetalles_IdDetalleAfectado] FOREIGN KEY ([IdDetalleAfectado]) REFERENCES [PresupuestoDetalles] ([IdPresupuestoDetalle]) ON DELETE NO ACTION;

EXEC(N'
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
            THROW 51001, ''El nombre de usuario ya esta registrado.'', 1;
        END;

        THROW;

    END CATCH;

END;
');

EXEC(N'
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
            THROW 51002, ''El usuario no existe.'', 1;

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
            THROW 51003, ''No se puede cambiar el rol del ultimo administrador activo.'', 1;

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
');

EXEC(N'
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
            THROW 51002, ''El usuario no existe.'', 1;
    
    END TRY

    BEGIN CATCH
        
        IF ERROR_NUMBER() IN (2601, 2627)
            THROW 51001, ''El nombre de usuario ya esta registrado.'', 1;

        THROW;

    END CATCH;

END;
');

EXEC(N'
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
            THROW 51002, ''El usuario no existe.'', 1;

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
            THROW 51004, ''No se puede desactivar al ultimo administrador activo.'', 1;

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
');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261026155115_EsquemaInicialCircuitoActual', N'10.0.11');

COMMIT;
GO

BEGIN TRANSACTION;
IF EXISTS (SELECT 1 FROM dbo.PresupuestoDetalles WHERE EstadoEjecucion IN (N'Parcial', N'Omitido'))
    THROW 51140, N'Hay ejecuciones Parcial/Omitido del alcance anterior. No se modificaron. Para demostración prepare SGTM_DB_Demo vacía con el lote v4; para datos reales solicite revisión técnica antes de migrar.', 1;

ALTER TABLE [PresupuestoDetalles] DROP CONSTRAINT [CK_PresupuestoDetalles_Avance];

ALTER TABLE [PresupuestoDetalles] DROP CONSTRAINT [CK_PresupuestoDetalles_EstadoEjecucion];

ALTER TABLE [PresupuestoDetalles] DROP CONSTRAINT [CK_PresupuestoDetalles_Justificacion];

ALTER TABLE [PresupuestoDetalles] ADD CONSTRAINT [CK_PresupuestoDetalles_Avance] CHECK ((
    [EstadoEjecucion] = N'NoIniciado'
    AND [CantidadEjecutada] = 0
)
OR
(
    [EstadoEjecucion] = N'EnCurso'
    AND [CantidadEjecutada] < [Cantidad]
)
OR
(
    [EstadoEjecucion] = N'Completado'
    AND [CantidadEjecutada] = [Cantidad]
));

ALTER TABLE [PresupuestoDetalles] ADD CONSTRAINT [CK_PresupuestoDetalles_EstadoEjecucion] CHECK ([EstadoEjecucion] IN (
    N'NoIniciado',
    N'EnCurso',
    N'Completado'
));

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261028164124_SimplificarEjecucionTresEstados', N'10.0.11');

COMMIT;
GO

BEGIN TRANSACTION;
IF EXISTS (SELECT 1 FROM dbo.PresupuestoDetalles WHERE IdPresupuestoCambio IS NOT NULL)
    THROW 51141, N'Hay vínculos de presupuesto de cambio ajenos al circuito actual. Revise esos datos antes de aplicar la limpieza; no se modificaron.', 1;

ALTER TABLE [PresupuestoDetalles] DROP CONSTRAINT [FK_PresupuestoDetalles_Presupuestos_IdPresupuestoCambio];

ALTER TABLE [PresupuestoDetalles] DROP CONSTRAINT [CK_PresupuestoDetalles_PresupuestoCambio];

DROP INDEX [IX_PresupuestoDetalles_IdPresupuestoCambio] ON [PresupuestoDetalles];

DECLARE @var nvarchar(max);
SELECT @var = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[PresupuestoDetalles]') AND [c].[name] = N'IdPresupuestoCambio');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [PresupuestoDetalles] DROP CONSTRAINT ' + @var + ';');
ALTER TABLE [PresupuestoDetalles] DROP COLUMN [IdPresupuestoCambio];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261029090000_RetirarVinculoCambioSinUso', N'10.0.11');

COMMIT;
GO

BEGIN TRANSACTION;
IF EXISTS (SELECT 1 FROM dbo.Clientes WHERE Telefono IS NULL OR NOT ([Telefono] COLLATE Latin1_General_100_BIN2 NOT LIKE N'%[^0-9 +()-]%' AND LEN(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE([Telefono], N' ', N''), N'+', N''), N'(', N''), N')', N''), N'-', N'')) BETWEEN 7 AND 15 AND (CHARINDEX(N'+', [Telefono]) = 0 OR (LEFT(LTRIM([Telefono]), 1) = N'+' AND CHARINDEX(N'+', LTRIM([Telefono]), 2) = 0))))
    THROW 51142, N'Hay clientes sin teléfono de contacto válido. Complete los teléfonos reales (7 a 15 dígitos) antes de actualizar. No se modificaron los registros.', 1;

ALTER TABLE dbo.Clientes ALTER COLUMN Telefono nvarchar(30) NOT NULL;

ALTER TABLE [Clientes] ADD CONSTRAINT [CK_Clientes_TelefonoContacto] CHECK ([Telefono] COLLATE Latin1_General_100_BIN2 NOT LIKE N'%[^0-9 +()-]%' AND LEN(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE([Telefono], N' ', N''), N'+', N''), N'(', N''), N')', N''), N'-', N'')) BETWEEN 7 AND 15 AND (CHARINDEX(N'+', [Telefono]) = 0 OR (LEFT(LTRIM([Telefono]), 1) = N'+' AND CHARINDEX(N'+', LTRIM([Telefono]), 2) = 0)));

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261029190000_ExigirTelefonoContactoCliente', N'10.0.11');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [Usuarios] ADD [Version] rowversion NOT NULL;

EXEC(N'
ALTER PROCEDURE dbo.Usuario_ActualizarDatos
    @Id int,
    @Nombre nvarchar(100),
    @Apellido nvarchar(100),
    @RolId int,
    @Version binary(8)
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
            THROW 51002, ''El usuario no existe.'', 1;

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
            THROW 51003, ''No se puede cambiar el rol del ultimo administrador activo.'', 1;

        UPDATE dbo.Usuarios
        SET Nombre = @Nombre,
            Apellido = @Apellido,
            RolId = @RolId
        WHERE Id = @Id AND Version = @Version;
        IF @@ROWCOUNT = 0 THROW 51005, ''El usuario fue modificado por otra operacion.'', 1;

        COMMIT TRANSACTION;

    END TRY

    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH

END;
');

EXEC(N'
ALTER PROCEDURE dbo.Usuario_ActualizarCredenciales
    @Id int,
    @NombreUsuario nvarchar(50),
    @PasswordHash nvarchar(255) = NULL,
    @Version binary(8)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        UPDATE dbo.Usuarios
        SET NombreUsuario = @NombreUsuario,
            PasswordHash = COALESCE(@PasswordHash, PasswordHash)
        WHERE Id = @Id AND Version = @Version;
        IF @@ROWCOUNT = 0 THROW 51005, ''El usuario fue modificado por otra operacion.'', 1;

    END TRY

    BEGIN CATCH

        IF ERROR_NUMBER() IN (2601, 2627)
            THROW 51001, ''El nombre de usuario ya esta registrado.'', 1;

        THROW;

    END CATCH;

END;
');

EXEC(N'
ALTER PROCEDURE dbo.Usuario_CambiarEstado
    @Id int,
    @Activo bit,
    @Version binary(8)
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
            THROW 51002, ''El usuario no existe.'', 1;

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
            THROW 51004, ''No se puede desactivar al ultimo administrador activo.'', 1;

        UPDATE dbo.Usuarios
        SET Activo = @Activo
        WHERE Id = @Id AND Version = @Version;
        IF @@ROWCOUNT = 0 THROW 51005, ''El usuario fue modificado por otra operacion.'', 1;

        COMMIT TRANSACTION;

    END TRY

    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;

    END CATCH;

END
');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261029210000_AgregarConcurrenciaUsuarios', N'10.0.11');

COMMIT;
GO

BEGIN TRANSACTION;
IF EXISTS (SELECT 1 FROM dbo.Presupuestos WHERE NOT (
    [Estado] NOT IN (N'Aceptado', N'Rechazado', N'PendienteRevisionTecnica') OR ([PersonaDecision] IS NOT NULL AND LEN(LTRIM(RTRIM([PersonaDecision]))) BETWEEN 1 AND 150 AND [MedioDecision] IS NOT NULL AND [MedioDecision] IN (N'Presencial', N'Teléfono', N'Correo', N'Mensajería'))))
    THROW 51144, 'Hay decisiones de presupuesto sin persona o medio válido. Revise los datos reales antes de actualizar.', 1;

ALTER TABLE [Presupuestos] ADD CONSTRAINT [CK_Presupuestos_ContactoDecision] CHECK ([Estado] NOT IN (N'Aceptado', N'Rechazado', N'PendienteRevisionTecnica') OR ([PersonaDecision] IS NOT NULL AND LEN(LTRIM(RTRIM([PersonaDecision]))) BETWEEN 1 AND 150 AND [MedioDecision] IS NOT NULL AND [MedioDecision] IN (N'Presencial', N'Teléfono', N'Correo', N'Mensajería')));

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261029220000_ExigirContactoDecisionPresupuesto', N'10.0.11');

COMMIT;
GO

