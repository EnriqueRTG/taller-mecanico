# Estados y clasificaciones: revisión del circuito vigente

## Resultado

La atención tiene siete estados operativos actuales: EnEvaluacionTecnica, PendienteDecision, EnEjecucion, TrabajoFinalizado, PendienteLiquidacion, Cerrada y Cancelada. Los valores numéricos/nombres persistidos no se renumeran. El filtro de Atenciones no ofrece los tres valores de compatibilidad salvo que existan registros con ellos. Las pausas actuales se representan mediante PausaTrabajo por alcance, no mediante otro estado general.

No se eliminan valores del enum que pueden estar almacenados en historiales, diagnósticos, presupuestos o copias anteriores: EF convierte esos nombres/valores al leerlos. Eliminarlos sin conocer y transformar todos esos registros rompería la lectura y recuperación. Compatibilidad no significa que se generen nuevos registros con el circuito antiguo.

| Estado de atención | Uso y operación responsable |
|---|---|
| EnEvaluacionTecnica | Recepción individual; disponible/reservada se distingue por IdTecnicoEnActividad y ActividadTecnica. También revisiones antes de ejecutar. |
| PendienteDecision | Propuesta principal informada, esperando respuesta; una selección parcial sigue sin autorizar ejecución. |
| EnEjecucion | Reparación autorizada; permite trabajo, pausas y adicionales sin borrar el acuerdo original. |
| TrabajoFinalizado | Terminación normal y conformidad técnica. Recepción liquida/cobra/entrega. |
| PendienteLiquidacion | Rechazo, cierre sin reparación completa o interrupción. Se cobra solo lo autorizado y realizado, incluso si el resultado es cero. |
| Cerrada | Entrega registrada con liquidación saldada. |
| Cancelada | Desistimiento previo a actividad técnica, presupuestos o servicios autorizados; conserva motivo e historial. |
| Abierta | Compatibilidad con aperturas antiguas y origen de eventos de recepción del historial. |
| PendienteDiagnostico | Compatibilidad con ingreso/servicio inicial del circuito anterior; permite retomar mediante preparación técnica. |
| Pausada | Lectura histórica. No se asigna en el circuito actual; no se interpreta automáticamente como una pausa por tarea. Su eventual conversión necesita revisión de los registros afectados. |

## Presupuesto y ejecución

Los seis EstadoPresupuesto tienen funciones diferentes y se conservan: Borrador, PendientePresentacion, PendienteDecision, PendienteRevisionTecnica, Aceptado y Rechazado. Vigente distingue la versión aplicable sin inventar un estado de reemplazo. Una revisión conserva el original y genera otra propuesta. El presupuesto aceptado no equivale a trabajo ejecutado ni cobro recibido.

Los cinco EstadoEjecucionDetalle son necesarios: NoIniciado, EnCurso, Completado, Parcial y Omitido. Parcial significa fin incompleto justificado, no un porcentaje intermedio; exige cantidad positiva y se utiliza en interrupción. Omitido exige cero y motivo. Completado exige la cantidad autorizada; no permite disminuir lo registrado. El lote nuevo muestra los cinco.

## Otras enumeraciones y atributos

- TipoSolicitudIngreso: TrabajoSolicitado / ProblemaInformado, expresado por cada pedido. TipoSolicitudAtencion (Problema, ServicioDefinido, Mixta) es el resumen derivado, no una tercera pregunta de ingreso.
- ActividadTecnica: Diagnostico, Presupuesto, Ejecucion. Identifica reserva y permisos, no duplica el estado de la atención.
- TipoPresupuesto.Reparacion incluye la propuesta inicial y adicionales; EsAdicional/EsInspeccion distinguen su alcance. TipoPresupuesto.Diagnostico se conserva para servicios iniciales del circuito anterior.
- TipoDiagnostico.Inicial es el informe actual. Adicional se conserva para lectura de informes anteriores; los hallazgos actuales utilizan HallazgoAtencion, no generan automáticamente otro Diagnostico.Adicional.
- TipoConcepto: Servicio / Repuesto / Consumible. Determina validación de materiales aportados y la oferta del catálogo.
- OrigenMaterial: Taller / Cliente; el aporte del cliente tiene precio de venta cero y requiere recepción/conformidad antes de utilizarse.
- Cliente, vehículo, usuario y catálogos emplean Activo; no necesitan un enum adicional. Pago conserva Anulado y su motivo, no un borrado. Comprobante conserva total/pagado/saldo, no un estado de pago redundante. Pausa conserva resolución/fecha; no necesita otro enum.
- TipoRespaldo: Completa / Diferencial (no respaldo de log ni incremental encadenado). Ambos tienen implementación.
- TipoReporte: siete consultas diferentes; TipoGraficoReporte: Barras, Columnas, Torta; PermisoAplicacion conserva las capacidades de navegación por rol. Son tipos/capacidades, no estados comerciales.

Se revisaron los lectores, asignaciones, servicios, consultas y selectores fuera de migraciones. No se retiraron validaciones de autorización, concurrencia, importes o estados solo porque una ruta alternativa no aparezca en el recorrido ideal. Se eliminó el método de interfaz MostrarFuncionEnDesarrollo sin llamadas, que contenía un mensaje de simulación ajeno al circuito implementado.
