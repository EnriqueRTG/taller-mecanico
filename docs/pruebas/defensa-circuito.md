# Recorridos para defender el backend

Este guion acompaña al código; no sustituye ERS, casos de uso, DER ni manuales formales.

| Operación | Presentación | Aplicación | Infraestructura / persistencia | Qué demostrar |
|---|---|---|---|---|
| Apertura | FrmNuevaAtencion y EditorSolicitudes | AtencionServicio | AtencionRepositorio, configuraciones, unidad de trabajo | Cliente/vehículo habilitados, solicitudes individuales y tipo derivado; alta e historial coherentes. |
| Preparar propuesta | FrmEvaluarAtencion | EvaluacionServicio | EvaluacionRepositorio | Informe obligatorio solo para síntomas, precios acordados, borrador/confirmación y reserva técnica. |
| Decisión parcial | FrmDecisionPresupuesto / FrmPrepararPropuesta | PresupuestoServicio.DecidirAsync / SeguimientoTrabajoServicio.RevisarSeleccionAsync | CircuitoRepositorio / EvaluacionRepositorio / unidad | Persona/medio/usuario de recepción preservados. Misma selección habilita sin otra decisión; cambios crean versión y vuelven al cliente. |
| Ejecución | FrmEjecutarAtencion | EjecucionServicio | Repositorios y CHECK de PresupuestoDetalles | Tres estados, cantidades monotónicas, pausas, reserva, completitud y conformidad final. |
| Liquidación/pago | FrmComprobantePagos / FrmRegistrarPago | CobroServicio | CircuitoRepositorio | Valores desde ejecución y precio acordado, suma de medios, saldo, transacción, rowversion e intento idempotente. |
| Reporte | FrmReportes | ReporteServicio | ReporteRepositorio | Consulta con filtros/relaciones, proyección DTO, redondeo/indicadores, metodología y exportación. |
| Recuperación | FrmRespaldoRestauracion | RespaldoServicio | RespaldoSqlServer / RegistroRespaldosSql | Archivo verificado, ensayo, dos identidades, control separado, mantenimiento y copia protectora. |

## Ejercicio de reporte verificable

Elegir Movimientos de cobro, registrar dos medios y una anulación. Fijar el mismo rango inclusivo en UI y SQL. En ReporteRepositorio.ListarMovimientosAsync, explicar cobro positivo en Fecha y anulación negativa en FechaAnulacion; un pago anulado sigue existiendo y debe verse como dos eventos si ambas fechas caen dentro del período. Comparar total por medio con indicadores y exportaciones. No comparar contra TotalPagado actual para reconstruir un movimiento de otro período.

```sql
DECLARE @Desde datetime2 = '2026-10-01', @HastaExclusivo datetime2 = '2026-11-01';
WITH Movimientos AS (
    SELECT m.NombreMedio, m.Importe
    FROM PagoMedios m JOIN Pagos p ON p.IdPago=m.IdPago
    WHERE p.Fecha>=@Desde AND p.Fecha<@HastaExclusivo
    UNION ALL
    SELECT m.NombreMedio, -m.Importe
    FROM PagoMedios m JOIN Pagos p ON p.IdPago=m.IdPago
    WHERE p.Anulado=1 AND p.FechaAnulacion>=@Desde AND p.FechaAnulacion<@HastaExclusivo
)
SELECT NombreMedio, SUM(Importe) AS Neto FROM Movimientos GROUP BY NombreMedio;
```

Ajustar fechas y, si se aplicó búsqueda/estado en pantalla, reproducir esos filtros o quitarlos antes de comparar. Para actividad técnica, un evento no equivale a una hora ni a un trabajo completo. Para presupuestos, se excluyen versiones reemplazadas y el importe autorizado no equivale al cobrado.

## Preguntas de práctica

- ¿Por qué ocultar un botón no autoriza ni protege por sí solo? Mostrar validación de usuario/rol persistido en servicio.
- ¿Qué cambia al guardar frente a finalizar? Mostrar validaciones anteriores a GuardarCambiosAsync y el historial en la misma unidad de trabajo.
- ¿Qué evita dos reservas o cobros concurrentes? Mostrar rowversion, conflicto y descarte de cambios; el pago además identifica un intento para no duplicar un reintento.
- ¿Por qué SQL tiene CHECK/índices además de validaciones C#? Probar una operación inválida tanto desde servicio como directamente en una base de prueba.
- ¿Por qué la base de control está separada? Restaurar negocio no debe hacer retroceder aprobación, agenda ni bitácora.

La implementación usa cuatro capas principales; Demo, Pruebas y Respaldos son ejecutables/herramientas auxiliares. Revisar las referencias y DI en las clases DependencyInjection y [estructura](../arquitectura/estructura-solucion.md). No memorizar nombres: seguir un dato real desde formulario hasta SQL y explicar el contrato de cada paso.
