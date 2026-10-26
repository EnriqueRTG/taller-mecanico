> Actualización del circuito: consultar [hallazgos, adicionales y materiales del cliente](../hallazgos-adicionales-materiales.md). Esa ampliación reemplaza la aceptación parcial inmediata y la limitación de una sola propuesta durante la ejecución descritas en versiones anteriores.

# Adenda funcional v1.0 — Presupuestos, ejecución y cobros

Fecha: 21/10/2026. Esta adenda documenta las decisiones de alcance adoptadas para la implementación y sustituye los puntos incompatibles del ERS v0.9. No es una aprobación docente del documento completo ni modifica silenciosamente el PDF anterior.

## Decisiones de alcance

- Se permiten pagos parciales y la combinación de distintos medios dentro de un mismo cobro.
- La entrega exige saldo exactamente cero.
- El cliente puede excluir ítems de una propuesta. La propuesta completa permanece como antecedente; la aceptación se registra por ítem.
- Atención es el expediente principal; se registra la ejecución sobre sus ítems autorizados, sin introducir una entidad Orden de Trabajo.
- El sistema administra documentos internos persistidos y numerados, sin integración fiscal ni validación bancaria automática.
- Esta versión cobra después de finalizar/interrumpir la actividad técnica y liquidarla. No administra anticipos, crédito posterior a la entrega ni devoluciones bancarias.

## Reemplazo de requisitos y reglas anteriores

| Referencia v0.9 | Regla vigente para esta implementación |
| --- | --- |
| RF-24 y RN-11/12 | Recepción registra la decisión comunicada por el cliente, indicando cada ítem aceptado. Solo esos ítems habilitan ejecución; el técnico valida la viabilidad del alcance antes de tomarlo. No se modifica el precio ni la cantidad original desde Recepción. |
| RF-40 y RN-15 | El técnico registra cantidades realizadas y estados. Puede finalizar al resolver todos los ítems como completados u omitidos con motivo; una interrupción admite ejecución parcial justificada. Terminar técnicamente no implica pago ni entrega. |
| RF-41 y RN-16 | Recepción registra uno o varios cobros parciales hasta cancelar el total liquidado. Un cobro puede distribuirse en varios medios de pago. |
| RF-42 y RN-22 | Se liquida la suma de los subtotales de cantidades efectivamente realizadas e inicialmente autorizadas, a precios históricos. Incluye inspecciones extraordinarias autorizadas y realizadas y excluye lo rechazado, omitido o pendiente. El total no tiene que coincidir con el presupuesto completo original si se redujo el alcance o se interrumpió el trabajo. |
| RF-43 | No se admiten sobrepagos ni nuevos cobros con saldo cero. Reintentar el mismo identificador de operación y contenido recupera el mismo recibo; no duplica el cobro. |
| RF-44 y RN-17 | Primero se emite una liquidación interna con los conceptos e importes congelados. Cada cobro confirmado genera su recibo interno en el mismo guardado que actualiza el saldo. La liquidación no acredita recepción de dinero. |
| RF-45/46 y RN-18 | Se consultan e imprimen liquidaciones y recibos, con identificación única y leyenda sin validez fiscal. La impresión requiere una impresora instalada; puede ser una impresora PDF de Windows. |
| RN-14/20 | Propuestas, decisiones y movimientos económicos conservan sus antecedentes. No se borran ítems para ocultar lo rechazado ni pagos para corregir errores. |
| RN-19 y RF-25 | La entrega requiere actividad técnica terminada, liquidación emitida y saldo cero. Una atención sin cargos también requiere justificación y liquidación en cero; no se inventa un pago de importe cero. Se registra usuario, fecha y condición de entrega. |
| RI-06 | Una atención tiene como máximo una liquidación; esta tiene múltiples conceptos y múltiples pagos. Un pago puede usar múltiples medios. Cada pago tiene un número de recibo independiente. Los presupuestos pueden tener revisiones enlazadas. |

## Alcance reducido y revisiones

En la decisión inicial se pueden desmarcar ítems. Si no se acepta ninguno, la propuesta queda rechazada. Tanto la exclusión parcial como el rechazo requieren motivo. Lo previamente autorizado y realizado, por ejemplo una inspección, no se elimina al rechazar la reparación.

Una revisión formal se solicita mientras no se ejecutaron trabajos de esa propuesta y no hay reserva técnica ni liquidación. El presupuesto anterior conserva su contenido y decisión y queda fuera de vigencia; se genera un nuevo borrador enlazado. El técnico continúa desde Evaluaciones técnicas, presenta una nueva propuesta y Recepción registra una nueva decisión. Un informe diagnóstico histórico no se reescribe.

Con la ejecución iniciada no se vuelve a escribir la propuesta aceptada. Si se retira un trabajo todavía no realizado, el técnico registra su omisión y el motivo. Si un ítem ya tuvo ejecución parcial, no puede borrarse ni reducirse lo ya registrado: corresponde terminar como interrupción justificada y liquidar la cantidad real. No se permite convertir un trabajo realizado en omitido para hacer desaparecer el cargo. Las ampliaciones que requieran nuevos trabajos/precios después de iniciada la ejecución quedan para un circuito de adicionales posterior.

## Cobros y correcciones

Cada importe debe ser positivo y expresarse hasta en dos decimales; el total de los medios debe coincidir con el cobro y no superar el saldo. Transferencias, tarjetas y billetera requieren referencia de la operación. La referencia deja constancia administrativa; no verifica el movimiento contra un banco.

Un pago registrado incorrectamente puede anularse con motivo antes de la entrega. Se conserva el recibo original marcado como anulado, usuario y fecha de la corrección, y se repone el saldo. Esto corrige el registro interno, no ejecuta una devolución de dinero. Después de la entrega se bloquean nuevas alteraciones económicas en este circuito.

Se conserva en cada recibo la denominación de los medios, sus referencias, el nombre del usuario que registró el cobro y el saldo resultante en ese momento. La liquidación conserva cliente, documento, dominio, cantidades y precios; cambios posteriores del catálogo o de los usuarios no reescriben esos datos. Todos los importes de esta versión se expresan en pesos argentinos (ARS).
