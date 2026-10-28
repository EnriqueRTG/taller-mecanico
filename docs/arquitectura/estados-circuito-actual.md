# Estados del circuito actual

La base se construye desde cero mediante EsquemaInicialCircuitoActual. No se conservan estados ni rutas de compatibilidad: Abierta, PendienteDiagnostico y Pausada fueron retirados del dominio, las consultas, los filtros y las restricciones del esquema e historial. La recepción se guarda directamente en EnEvaluacionTecnica con un único evento RecepcionRegistrada. No se convierten historiales anteriores: este esquema requiere una base nueva.

| Estado de atención | Uso y operación responsable |
|---|---|
| EnEvaluacionTecnica | Recepción individual; disponible/reservada se distingue por IdTecnicoEnActividad y ActividadTecnica. También revisiones antes de ejecutar. |
| PendienteDecision | Propuesta principal informada, esperando respuesta; una selección parcial sigue sin autorizar ejecución. |
| EnEjecucion | Reparación autorizada; permite trabajo, pausas y adicionales sin borrar el acuerdo original. |
| TrabajoFinalizado | Terminación normal y conformidad técnica. Recepción liquida/cobra/entrega. |
| PendienteLiquidacion | Rechazo de reparación o evaluación sin propuesta de reparación. Se cobra solo lo autorizado y realizado, incluso si el resultado es cero. |
| Cerrada | Entrega registrada con liquidación saldada. |
| Cancelada | Desistimiento previo a actividad técnica, presupuestos o servicios autorizados; conserva motivo e historial. |


## Presupuesto y ejecución

Los seis EstadoPresupuesto tienen funciones diferentes y se conservan: Borrador, PendientePresentacion, PendienteDecision, PendienteRevisionTecnica, Aceptado y Rechazado. Vigente distingue la versión aplicable sin inventar un estado de reemplazo. Una reformulación conserva el original y genera otra propuesta. Validar sin cambios la selección del cliente conserva el mismo presupuesto y su decisión original. El presupuesto aceptado no equivale a trabajo ejecutado ni cobro recibido.

Los tres EstadoEjecucionDetalle son NoIniciado, EnCurso y Completado. Una cantidad realizada menor que la autorizada permanece EnCurso; no es un cierre definitivo. Completado exige toda la cantidad autorizada. No se disminuye el avance registrado ni se revierte un completado. Finalizar exige todos los ítems autorizados completados y conformidad técnica. Una pausa conserva el avance y bloquea el alcance; no es otro estado de ejecución. El cierre con interrupción queda fuera de esta entrega.

La migración SimplificarEjecucionTresEstados retira las restricciones antiguas sin reinterpretar datos: si existen Parcial/Omitido rechaza la actualización. Ver [actualización](../instalacion/actualizacion-coloquio.md).

## Otras enumeraciones y atributos

- TipoSolicitudIngreso: TrabajoSolicitado / ProblemaInformado, expresado por cada pedido. TipoSolicitudAtencion (Problema, ServicioDefinido, Mixta) es el resumen derivado, no una tercera pregunta de ingreso.
- ActividadTecnica: Diagnostico, Presupuesto, Ejecucion. Identifica reserva y permisos, no duplica el estado de la atención.
- Presupuesto no necesita un enum de tipo: todos son propuestas. EsAdicional/EsInspeccion distinguen alcances realmente usados. Se retiraron TipoPresupuesto y su columna Tipo.
- Diagnostico es el informe definitivo de evaluación, único por atención. Los hallazgos usan HallazgoAtencion. Se retiraron TipoDiagnostico, su columna Tipo y el indicador ImpideContinuar del diagnóstico, que duplicaba un comportamiento exclusivo del hallazgo.
- TipoConcepto: Servicio / Repuesto / Consumible. Determina validación de materiales aportados y la oferta del catálogo.
- OrigenMaterial: Taller / Cliente; el aporte del cliente tiene precio de venta cero y requiere recepción/conformidad antes de utilizarse.
- Cliente, vehículo, usuario y catálogos emplean Activo; no necesitan un enum adicional. Pago conserva Anulado y su motivo, no un borrado. Comprobante conserva total/pagado/saldo, no un estado de pago redundante. Pausa conserva resolución/fecha; no necesita otro enum.
- TipoRespaldo: Completa / Diferencial (no respaldo de log ni incremental encadenado). Ambos tienen implementación.
- TipoReporte: siete consultas diferentes; TipoGraficoReporte: Barras, Columnas, Torta; PermisoAplicacion conserva las capacidades de navegación por rol. Son tipos/capacidades, no estados comerciales.


## Opciones agrupadas del listado

Finalizadas no es un enum ni un estado guardado: es un filtro de consulta que selecciona Cerrada o Cancelada. En curso selecciona las restantes; Pendientes de entrega se deriva de cierre técnico, liquidación y saldo cero. TrabajoFinalizado es distinto de Cerrada: el técnico terminó, pero todavía puede faltar liquidar/cobrar/entregar. La agrupación visual no conserva estados retirados ni altera los registros.
