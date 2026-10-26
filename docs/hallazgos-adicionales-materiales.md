# Atención, selección parcial, hallazgos y materiales del cliente

## Alcance

Una atención conserva la solicitud, evaluación, autorizaciones, ejecución y entrega del mismo vehículo. Una solicitud definida puede presupuestarse sin diagnóstico; un síntoma requiere informe técnico. El técnico confirma la viabilidad incluso cuando el cliente pide una tarea concreta. Registrar una propuesta no autoriza ejecutarla.

Esta ampliación se construye sobre `feature/integracion-catalogo-validaciones` (PR #34). La rama contiene esa integración completa; no se deben copiar archivos de las ramas antiguas de catálogo ni aplicar su migración destructiva.

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

1. Recepción comunica el presupuesto y registra los ítems que el cliente solicita conservar, con motivo.
2. La propuesta pasa a `PendienteRevisionTecnica`. Esa selección **no es autorización de ejecución**.
3. En **Presupuestos → Revisar selección parcial**, el técnico evalúa dependencias y condiciones. Puede quitar, reemplazar o agregar conceptos y ajustar cantidades/precios antes de confirmar.
4. Se conserva el documento anterior y se crea una propuesta nueva, vinculada por `IdPresupuestoAnterior`, con justificación técnica obligatoria.
5. Recepción vuelve a comunicar el alcance y precio definitivos y registra la aceptación. Una nueva selección parcial vuelve a revisión; un rechazo no autoriza trabajos.

La viabilidad es una decisión del técnico registrada explícitamente; no se pretende deducir automáticamente dependencias mecánicas desde nombres de conceptos.

## Hallazgos, pausas y adicionales

En **Atenciones → Seguimiento y hallazgos**, o desde la evaluación/ejecución, se consulta el expediente con tres pestañas:

- **Hallazgos e informes:** descripción y recomendación obligatorias, fecha y autor. Un hallazgo no genera deuda. Si impide continuar, crea una pausa de la tarea seleccionada o de toda la intervención.
- **Pausas y continuidad:** espera de repuestos, decisión pendiente, solicitud de detener u otra causa. Solo el técnico resuelve una pausa, dejando su motivo y responsabilidad. Resolver no ejecuta ni autoriza trabajos.
- **Materiales del cliente:** requeridos y recibidos, conformidad técnica y observaciones.

La acción **Informe de intervención** permite previsualizar e imprimir el diagnóstico disponible, los trabajos autorizados y su ejecución, los hallazgos/recomendaciones, las pausas y los materiales aportados. También se puede guardar como PDF mediante una impresora PDF de Windows. Es una consulta del expediente, no una nueva autorización ni un recibo.

Las tareas independientes pueden continuar con otra tarea pausada. Para terminar técnicamente se deben resolver las pausas y todas las propuestas pendientes. Para interrumpir definitivamente: registrar la decisión pendiente (por ejemplo, rechazo del adicional), resolver la pausa explicando el cierre sin continuar, omitir las tareas no realizadas o registrar cantidades parciales justificadas y **Terminar con interrupción**. Eso conserva y liquida lo realizado.

Durante la ejecución, el técnico puede preparar un presupuesto adicional vinculado al hallazgo seleccionado. Se incluyen **solo conceptos nuevos**, sin duplicar lo ya autorizado. Recepción comunica y registra su decisión de manera independiente. Aceptarlo agrega sus ítems a la ejecución; rechazarlo conserva los presupuestos anteriores. La pausa no se resuelve automáticamente al aceptar: el técnico debe verificar que efectivamente puede continuar.

Un adicional sin ejecutar también puede ser enviado a revisión por recepción. Se conserva su documento anterior; el técnico prepara la alternativa desde Presupuestos. Los ítems ya ejecutados no se reescriben ni se eliminan.

Si se necesita más inspección/desarme con costo, desde la evaluación técnica se prepara un **adicional de inspección**, limitado a servicios. Se presenta y acepta antes de que aparezca habilitado para registrar su ejecución en Inspecciones autorizadas. Rechazar la reparación posterior no borra ese servicio autorizado y realizado.

## Materiales aportados por el cliente

Al preparar la propuesta, seleccionar **Material del cliente / Aporta cliente** para un repuesto o consumible. La cantidad y descripción expresan lo requerido; su precio de venta del taller es cero. Los servicios no pueden tener ese origen.

El técnico registra cantidad recibida acumulada, conformidad y observación (incluyendo descripción/especificación y reemplazo de una pieza no apta). No se permite disminuir lo recibido ni invalidar material ya utilizado. No se admite consumir más de lo recibido ni utilizar material no conforme. Si el material falta y afecta otras tareas, se pausa explícitamente ese alcance.

No se incorpora inventario, compras ni deuda del taller con el proveedor del cliente. El costo que el cliente pague por su cuenta queda fuera del cobro del taller. La mano de obra y materiales del taller mantienen sus precios autorizados. El registro de materiales permanece en el expediente aunque se excluya de la liquidación.

## Ejecución, cobro y cierre

La ejecución reúne todos los presupuestos de reparación vigentes y aceptados; los servicios de inspección se registran durante la evaluación. Las propuestas pendientes o rechazadas no aparecen como trabajo autorizado.

Se liquida únicamente cantidad efectivamente realizada de ítems autorizados, excluyendo materiales aportados por el cliente. No se cobra lo omitido ni lo pendiente. Los pagos parciales/mixtos y comprobantes internos conservan su funcionamiento. La entrega exige liquidación, pagos consistentes, saldo cero y conformidad registrada por recepción. **Finalizar trabajo** registra la conformidad del técnico; no equivale a entregar el vehículo.

## Integridad y concurrencia

Cada mutación comparte la versión de Atención y una única confirmación transaccional con su historial. Las decisiones/revisiones también verifican la versión del presupuesto. Una reserva ajena impide intervenciones técnicas concurrentes; el administrativo puede registrar una pausa solicitada por el cliente. Las referencias a tareas/hallazgos se validan contra la atención correspondiente.

SQL conserva claves externas sin borrado en cascada, restricciones de textos, cantidades, origen de materiales, precio cero de aportes y coherencia de decisiones. La migración no elimina datos existentes: agrega tablas/columnas y actualiza restricciones e índice de borradores. Los ítems anteriores quedan con origen `Taller`, preservando sus importes.

## Instalación y pruebas

1. Respaldar la base y probar primero una copia. Cerrar los clientes de versiones anteriores.
2. Compilar y aplicar la cadena de migraciones de esta rama, incluida `20261022205915_HallazgosAdicionalesYMaterialesCliente`, con el proyecto de infraestructura y la configuración de conexión correcta.
3. No revertir esta migración después de registrar evidencias nuevas: su `Down` elimina las columnas/tablas añadidas. Para recuperación conservar una copia completa y el código compatible.
4. Si se usa el módulo de respaldos, crear una nueva copia completa después de migrar; las restauraciones comprueban compatibilidad de esquema.
5. Las ventanas de evaluación, ejecución y seguimiento limitan su tamaño al área útil después del escalado de Windows; las nuevas pantallas conservan desplazamiento y columnas redimensionables. Probar en Windows HD/FHD y escalas 100 %, 125 % y 150 %. La compilación cruzada y las pruebas de servicios no verifican visualmente las ventanas.

Pruebas automáticas: selección parcial sin autorización prematura; revisión técnica y nueva aceptación; adicional aceptado/rechazado; pausa selectiva; material recibido no conforme/conforme sin cobro; bloqueo de finalización con pendientes; revalidación de permisos/reserva/versiones; ampliación de inspección; revisión de adicional. SQL optativo aislado cubre migraciones con documentos previos y el recorrido selección parcial → hallazgo bloqueante → adicional → material aportado → resolución → ejecución → liquidación, con rowversion real. Las bases SQL de pruebas son temporales; nunca se usa la base operativa.

Pruebas manuales de aceptación: repetir esos casos por rol, cerrar/reabrir formularios sin guardar, simular dos instancias con datos desactualizados, confirmar que las grillas se actualizan y verificar el expediente final, comprobante e informe al cliente. Esta revisión visual sigue siendo una validación del equipo en Windows antes de fusionar a main.
