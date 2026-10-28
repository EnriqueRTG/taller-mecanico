# Atención, selección parcial, hallazgos y materiales del cliente

## Alcance

Una atención conserva la solicitud, evaluación, autorizaciones, ejecución y entrega del mismo vehículo. Una solicitud definida puede presupuestarse sin diagnóstico; un síntoma requiere informe técnico. El técnico confirma la viabilidad incluso cuando el cliente pide una tarea concreta. Registrar una propuesta no autoriza ejecutarla.

## Responsabilidades

| Operación | Administrativo | Técnico |
| --- | --- | --- |
| Abrir atención, registrar solicitud y comunicar propuestas | Sí | Consulta |
| Preparar informe, propuesta y adicionales | No | Sí |
| Registrar selección, aceptación o rechazo del cliente | Sí | Consulta |
| Validar una selección parcial y proponer alternativa viable | No | Sí |
| Registrar hallazgos | Consulta | Sí |
| Pausar por solicitud del cliente u otra causa | Sí | Sí |
| Resolver una pausa y determinar condiciones de continuidad | No | Sí |
| Recibir y conformar técnicamente material del cliente | Consulta | Sí |
| Registrar ejecución y finalización técnica | No | Sí |
| Liquidar, cobrar y confirmar entrega con conformidad | Sí | No |

Los servicios revalidan usuario/rol activos contra la base. La interfaz no sustituye esos controles. El administrador conserva sus funciones administrativas de seguridad, reportes y respaldos; no obtiene permisos operativos nuevos.

## Selección parcial

1. Recepción informa y registra que el cliente autoriza los ítems elegidos, a cantidades/precios ofrecidos, condicionado a viabilidad técnica. Se conserva persona que decide, medio y usuario que registró.
2. La propuesta queda PendienteRevisionTecnica: aún no puede ejecutarse.
3. El técnico abre Revisar selección parcial. Si conserva exactamente conceptos, descripciones, cantidades, precios, origen de material y solicitudes, usa Validar selección del cliente. Queda Aceptado con AlcanceValidado; no se pide otra aceptación y se registra el técnico en el historial. Los ítems excluidos siguen conservados y no se ejecutan ni cobran.
4. Si modifica la selección, la acción cambia a Enviar nueva propuesta a recepción. Se crea otra versión enlazada, con justificación, pendiente de presentación y decisión. Tampoco se acepta automáticamente cuando el técnico cambió el origen a material del cliente.
5. Una corrección técnica no es una decisión del cliente. Las solicitudes de corrección sin selección parcial real siempre producen otra propuesta para presentar y aceptar.

La viabilidad es una decisión del técnico registrada explícitamente; no se pretende deducir automáticamente dependencias mecánicas desde nombres de conceptos.

## Hallazgos, pausas y adicionales

Desde **Atenciones → Ver detalle → Ver progreso y seguimiento**, o desde la evaluación/ejecución, se consulta el expediente con cuatro pestañas:

- **Avance:** consulta de trabajos autorizados y cantidades realizadas; la edición se hace en ejecución.

- **Hallazgos e informes:** descripción y recomendación obligatorias, fecha y autor. Un hallazgo no genera deuda. Si impide continuar, crea una pausa de la tarea seleccionada o de toda la intervención.
- **Pausas y continuidad:** espera de repuestos, decisión pendiente, solicitud de detener u otra causa. Solo el técnico resuelve una pausa, dejando su motivo y responsabilidad. Resolver no ejecuta ni autoriza trabajos.
- **Materiales del cliente:** requeridos y recibidos, conformidad técnica y observaciones.

La acción **Informe de intervención** permite previsualizar e imprimir el diagnóstico disponible, los trabajos autorizados y su ejecución, los hallazgos/recomendaciones, las pausas y los materiales aportados. También se puede guardar como PDF mediante una impresora PDF de Windows. Es una consulta del expediente, no una nueva autorización ni un recibo.

Las tareas independientes pueden continuar con otra tarea pausada. Para finalizar se resuelven pausas y propuestas pendientes y se completan todos los ítems autorizados. No existe cierre con interrupción en esta entrega; no se simula una tarea completada ni una entrega para resolver un faltante.

Durante la ejecución, el técnico puede preparar un presupuesto adicional vinculado al hallazgo seleccionado. Se incluyen **solo conceptos nuevos**, sin duplicar lo ya autorizado. Recepción comunica y registra su decisión de manera independiente. Aceptarlo agrega sus ítems a la ejecución; rechazarlo conserva los presupuestos anteriores. La pausa no se resuelve automáticamente al aceptar: el técnico debe verificar que efectivamente puede continuar.

Un adicional sin ejecutar también puede ser enviado a revisión por recepción. Se conserva su documento anterior; el técnico prepara la alternativa desde Presupuestos. Los ítems ya ejecutados no se reescriben ni se eliminan.

Si se necesita más inspección/desarme con costo, desde la evaluación técnica se prepara un **adicional de inspección**, limitado a servicios. Se presenta y acepta antes de que aparezca habilitado para registrar su ejecución en Inspecciones autorizadas. Rechazar la reparación posterior no borra ese servicio autorizado y realizado.

## Materiales aportados por el cliente

Al preparar la propuesta, seleccionar **Material del cliente / Aporta cliente** para un repuesto o consumible. La cantidad y descripción expresan lo requerido; su precio de venta del taller es cero. Los servicios no pueden tener ese origen.

El técnico registra cantidad recibida acumulada, conformidad y observación (incluyendo descripción/especificación y reemplazo de una pieza no apta). No se permite disminuir lo recibido ni invalidar material ya utilizado. No se admite consumir más de lo recibido ni utilizar material no conforme. Si el material falta y afecta otras tareas, se pausa explícitamente ese alcance.

No se incorpora inventario, compras ni deuda del taller con el proveedor del cliente. El costo que el cliente pague por su cuenta queda fuera del cobro del taller. La mano de obra y materiales del taller mantienen sus precios autorizados. El registro de materiales permanece en el expediente aunque se excluya de la liquidación.

## Ejecución, cobro y cierre

La ejecución reúne todos los presupuestos de reparación vigentes y aceptados; los servicios de inspección se registran durante la evaluación. Las propuestas pendientes o rechazadas no aparecen como trabajo autorizado.

Se liquida únicamente cantidad efectivamente realizada de ítems autorizados, excluyendo materiales aportados por el cliente. No se cobra lo pendiente. La liquidación normal se habilita tras completar el alcance autorizado. Los pagos parciales/mixtos y comprobantes internos conservan su funcionamiento. La entrega exige liquidación, pagos consistentes, saldo cero y conformidad registrada por recepción. **Finalizar trabajo** registra la conformidad del técnico; no equivale a entregar el vehículo.

## Integridad y concurrencia

Cada mutación comparte la versión de Atención y una única confirmación transaccional con su historial. Las decisiones/revisiones también verifican la versión del presupuesto. Una reserva ajena impide intervenciones técnicas concurrentes; el administrativo puede registrar una pausa solicitada por el cliente. Las referencias a tareas/hallazgos se validan contra la atención correspondiente.

SQL conserva claves externas sin borrado en cascada, restricciones de textos, cantidades, origen de materiales, precio cero de aportes y coherencia de decisiones. El esquema se crea desde cero mediante EsquemaInicialCircuitoActual; no se convierte el lote anterior.

## Verificación

Preparar la base siguiendo [entornos](../instalacion/entornos.md). Las pruebas SQL ejercitan el recorrido selección parcial → hallazgo → adicional → material del cliente → resolución → ejecución → liquidación con rowversion nativa. Las pruebas de reglas verifican aceptación/autorización, permisos, pausas y cantidades. Repetir manualmente con dos roles, cierre/reapertura y documentos de varias páginas; comprobar que la pausa administrativa se avisa también directamente al taller.
