# Consulta y filtros de las bandejas

Los filtros de texto, situación y fechas se combinan sobre los registros ya cargados. Cambiarlos no consulta otra vez SQL; **Actualizar** obtiene los datos actuales. **Limpiar filtros** vuelve a todas las situaciones, elimina el texto y desactiva ambos extremos de fecha. No queda una fila seleccionada automáticamente.

## Preparación técnica: informe y reserva

- **Ver informe técnico** consulta el diagnóstico definitivo de la atención seleccionada. Solo aparece cuando existe ese informe. Recepción puede leerlo; no puede escribir ni reservar una evaluación.
- Una preparación de servicio solicitado puede producir una propuesta sin informe de diagnóstico. Que esté finalizada no implica que deba existir un informe ni que el vehículo esté entregado.
- **Mis actividades** corresponde exclusivamente al técnico y a sus reservas pendientes. Recepción dispone de Todas las actividades, Disponibles (sin reserva), Reservadas y Finalizadas.
- Los botones para tomar, continuar o liberar la preparación siguen reservados al técnico, con autorización nuevamente comprobada por los servicios.

## Fechas opcionales

Activar la casilla del selector **Desde** o **Hasta** para aplicar ese extremo. Se pueden usar de forma independiente. El filtro incluye el día completo, también las horas finales del día Hasta. Un rango invertido muestra una indicación y no devuelve filas; corregirlo o limpiar los filtros.

| Listado | Fecha que se filtra: la columna Último movimiento |
|---|---|
| Preparación técnica | Último evento del historial de la atención; si no tiene eventos, su apertura. |
| Presupuestos | Fecha de decisión; si no existe, fecha de presentación; si tampoco existe, fecha de creación. |
| Trabajos en curso | Último evento del historial de la atención; si no tiene eventos, su apertura. |
| Comprobantes y pagos | Último movimiento económico del comprobante (emisión, cobro o anulación). Si todavía no se liquidó, apertura de la atención. |

La fecha acota qué filas se muestran; los estados, avances y saldos son los actuales. No reconstruye una situación histórica al día elegido ni sustituye los reportes de movimientos por período. Las fechas empiezan desactivadas para no esconder pendientes antiguos.

Catálogo no usa este filtro temporal: se consulta por texto, categoría, tipo y disponibilidad, sin confundir disponibilidad comercial con stock físico.

## Distribución

Los cinco listados usan campos etiquetados que aprovechan el ancho de la pantalla y acciones en una fila independiente. En una ventana angosta los campos se distribuyen en dos filas. La adaptación visual no cambia permisos ni reglas del circuito.
