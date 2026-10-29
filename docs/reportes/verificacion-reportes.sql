/* SGTM v2.0. SOLO LECTURA. Ejecutar sobre la misma base y con los mismos
   filtros del formulario. Fechas civiles del taller; Hasta es exclusivo.
   Las consultas muestran la población base: aplicar luego los mismos filtros
   de estado/búsqueda. No comparar un total global con un informe filtrado.
   @Tecnico NULL para alcance global; para Técnico indicar su Id real. */
DECLARE @Desde datetime2 = '2026-10-01';
DECLARE @Hasta datetime2 = '2026-11-01';
DECLARE @Tecnico int = NULL;

-- 1. Atenciones recibidas: cohorte por apertura y estado actual.
SELECT a.IdAtencion, a.FechaApertura, a.Estado, v.Dominio,
       c.Apellido, c.Nombre, a.MotivoConsulta
FROM dbo.Atenciones a
JOIN dbo.Vehiculos v ON v.IdVehiculo=a.IdVehiculo
JOIN dbo.Clientes c ON c.IdCliente=a.IdCliente
WHERE a.FechaApertura>=@Desde AND a.FechaApertura<@Hasta;

-- 2. Propuestas vigentes, suma de redondeos por ítem, no precio del catálogo.
SELECT p.IdPresupuesto,p.IdAtencion,p.Estado,p.FechaCreacion,
       CASE WHEN p.Estado=N'Aceptado' AND EXISTS
          (SELECT 1 FROM dbo.PresupuestoDetalles x WHERE x.IdPresupuesto=p.IdPresupuesto AND x.AceptadoPorCliente=0)
          THEN N'Aceptado parcialmente' ELSE p.Estado END AS EstadoVisible,
       COALESCE(SUM(ROUND(d.Cantidad*d.PrecioUnitario,2)),0) AS ImportePropuesto,
       COALESCE(SUM(CASE WHEN p.Estado=N'Aceptado' AND d.AceptadoPorCliente=1
          THEN ROUND(d.Cantidad*d.PrecioUnitario,2) ELSE 0 END),0) AS ImporteAceptado
FROM dbo.Presupuestos p LEFT JOIN dbo.PresupuestoDetalles d ON d.IdPresupuesto=p.IdPresupuesto
WHERE p.Vigente=1 AND p.FechaCreacion>=@Desde AND p.FechaCreacion<@Hasta
  AND (@Tecnico IS NULL OR p.IdUsuarioCreacion=@Tecnico)
GROUP BY p.IdPresupuesto,p.IdAtencion,p.Estado,p.FechaCreacion;

-- 3. Movimientos: un cobro y su anulación tienen fechas propias.
SELECT p.IdPago, m.Orden, p.Fecha AS Fecha, N'Cobro' AS Movimiento,
       m.NombreMedio, m.Importe
FROM dbo.Pagos p JOIN dbo.PagoMedios m ON m.IdPago=p.IdPago
WHERE p.Fecha>=@Desde AND p.Fecha<@Hasta
UNION ALL
SELECT p.IdPago,m.Orden,p.FechaAnulacion,N'Anulación',m.NombreMedio,-m.Importe
FROM dbo.Pagos p JOIN dbo.PagoMedios m ON m.IdPago=p.IdPago
WHERE p.Anulado=1 AND p.FechaAnulacion>=@Desde AND p.FechaAnulacion<@Hasta;

-- 4. Saldos actuales: SIN filtro de período.
SELECT IdComprobante,IdAtencion,FechaEmision,Total,TotalPagado,
       Total-TotalPagado AS Saldo,
       CASE WHEN TotalPagado=0 THEN N'Sin cobros' ELSE N'Cobrado parcialmente' END AS Situacion
FROM dbo.Comprobantes WHERE Total>TotalPagado;

-- 5. Carga actual: SIN filtro de período; disponible o reserva propia.
SELECT a.IdAtencion,a.FechaApertura,a.Estado,a.IdTecnicoEnActividad,
       CASE WHEN EXISTS(SELECT 1 FROM dbo.PausasTrabajo x WHERE x.IdAtencion=a.IdAtencion AND x.FechaResolucion IS NULL)
       THEN N'Con pausas pendientes' WHEN a.IdTecnicoEnActividad IS NULL
       THEN N'Disponible para tomar' ELSE N'Reservada' END AS Situacion
FROM dbo.Atenciones a WHERE a.Estado IN(N'EnEvaluacionTecnica',N'EnEjecucion')
AND (@Tecnico IS NULL OR a.IdTecnicoEnActividad IS NULL OR a.IdTecnicoEnActividad=@Tecnico);

-- 6. Actividad técnica: acciones admitidas por ReporteServicio, eventos, no horas.
SELECT IdHistorialAtencion,IdAtencion,Fecha,IdUsuario,Accion,Observaciones
FROM dbo.HistorialAtenciones
WHERE Fecha>=@Desde AND Fecha<@Hasta AND (@Tecnico IS NULL OR IdUsuario=@Tecnico)
AND Accion IN(N'ActividadTomada',N'ActividadLiberada',N'DiagnosticoRegistrado',N'EvaluacionTomada',N'EvaluacionLiberada',
N'EvaluacionAvanceGuardado',N'EvaluacionFinalizada',N'EjecucionTomada',N'EjecucionLiberada',N'EjecucionAvanceGuardado',
N'TrabajoFinalizado',N'HallazgoRegistrado',N'PausaResuelta',N'MaterialClienteRevisado',N'PresupuestoAdicionalPreparado',
N'SeleccionParcialRevisada',N'SeleccionClienteValidada');

-- 7. Conceptos realizados: cantidad acumulada por fecha/autor de último registro.
SELECT d.IdPresupuestoDetalle,p.IdAtencion,d.FechaRegistroEjecucion,d.IdTecnicoRegistro,
       d.Descripcion,d.EstadoEjecucion,c.Tipo,d.CantidadEjecutada,
       ROUND(d.CantidadEjecutada*d.PrecioUnitario,2) AS Importe
FROM dbo.PresupuestoDetalles d JOIN dbo.Presupuestos p ON p.IdPresupuesto=d.IdPresupuesto
JOIN dbo.Conceptos c ON c.IdConcepto=d.IdConcepto
WHERE d.CantidadEjecutada>0 AND d.FechaRegistroEjecucion>=@Desde AND d.FechaRegistroEjecucion<@Hasta
AND (@Tecnico IS NULL OR d.IdTecnicoRegistro=@Tecnico);

-- Comprobación adicional de saldo almacenado frente a pagos no anulados.
SELECT c.IdComprobante,c.Total,c.TotalPagado,
       COALESCE(SUM(CASE WHEN p.Anulado=0 THEN p.Importe ELSE 0 END),0) AS SumaCobrosVigentes
FROM dbo.Comprobantes c LEFT JOIN dbo.Pagos p ON p.IdComprobante=c.IdComprobante
GROUP BY c.IdComprobante,c.Total,c.TotalPagado;
