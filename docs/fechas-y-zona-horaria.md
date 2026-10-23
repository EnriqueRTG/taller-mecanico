# Fechas y zona horaria del taller

La instalación actual corresponde a un taller de Argentina. `FechaHoraTaller` centraliza la zona America/Argentina/Buenos_Aires (Argentina Standard Time en Windows), la fecha de hoy, la hora operativa y las conversiones de instantes. Ningún módulo debe usar la zona local de la estación para interpretar datos del negocio. Cambiar la zona de esta instalación requiere una revisión técnica del histórico; no es una preferencia visual individual.

| Datos | Almacenamiento y uso |
|---|---|
| Clientes, vehículos y usuarios: fecha de alta | DateTime civil de Argentina. |
| Apertura/cierre, evaluaciones, diagnósticos, presupuestos y decisiones | DateTime civil de Argentina. |
| Historial, hallazgos, pausas, materiales y ejecución | DateTime civil de Argentina. |
| Comprobantes, cobros y anulaciones | DateTime civil de Argentina. |
| Accesos, bloqueo de intentos, control, réplica y plazos de respaldo | DateTimeOffset UTC. Se convierten al mostrarlos al operador. |
| Fecha nativa de finalización del archivo .bak | Hora que declara SQL Server; se etiqueta como hora del servidor, porque el encabezado no contiene zona. No se deduce su offset. |
| Programación automática | Hora civil y zona del plan; próxima ejecución y plazos se calculan/comparan como instantes UTC. |
| Año del vehículo y filtros de días | Valores civiles, no instantes ni conversiones automáticas a UTC. |

## Integridad del histórico

Los campos DateTime del negocio ya se guardaban como hora local. Se preservan sin moverlos tres horas ni cambiar su tipo. Desde este ajuste las nuevas fechas se generan explícitamente en Argentina, incluso si una PC está en otra zona. No se realizó una migración de datos ni se afirma que los campos comerciales existentes estén almacenados en UTC.

Este criterio es compatible con el alcance de una única sede argentina. Migrar toda la historia comercial a UTC requeriría una migración específica, con respaldo, identificación de la zona de los registros antiguos y adaptación coordinada de procedimientos/consultas/clientes. No se mezcla UTC y hora civil dentro de una misma columna.

## Consulta y presentación

- Inicio, calendario, alta de atención, fechas de referencia y emisión de documentos usan la hora operativa argentina.
- Auditoría muestra fecha y hora de Argentina. El repositorio conserva UTC; un filtro de un día se transforma en el intervalo [00:00 del taller, 00:00 del día siguiente), convertido a UTC. Incluye todo el último día sin depender de la zona de Windows.
- Los filtros de los reportes comerciales operan directamente sobre fechas civiles del taller. Saldos/carga actuales mantienen su alcance sin fecha según la metodología del reporte.
- Las comparaciones de caducidad de sesión, autorización de restauración, bloqueos y señales del agente conservan el uso de instantes UTC/TimeProvider; no se comparan con una fecha civil.
- La señal y el vencimiento del plan de restauración se muestran convertidos, conservando UTC internamente. La próxima copia muestra explícitamente la zona elegida en su programación.
- Los registros técnicos/CLI pueden conservar UTC si lo identifican expresamente.

Mantener sincronizados Windows y el servidor SQL. Convertir una zona no corrige un reloj del sistema adelantado/atrasado. Si registros anteriores se generaron en una estación con zona incorrecta, no pueden corregirse automáticamente sin identificar los casos.

## Comprobación manual

1. Realizar un acceso y comparar su hora en Inicio y Auditoría: debe coincidir en Argentina.
2. Filtrar Auditoría por ese día y comprobar accesos correctos y rechazados.
3. Crear una atención y comprobar apertura e historial en detalle, listado y reporte del mismo día.
4. Durante el recorrido de prueba, contrastar evaluación, decisión, ejecución, comprobante, pago y eventual anulación con su historial.
5. En Demo, cambiar solamente la zona de una estación (sin cambiar el instante del reloj), reiniciar SGTM y comprobar que sigue mostrando la hora de Argentina. Restaurar luego la configuración de Windows.
6. Los encabezados de copias siguen indicando hora del servidor SQL; el plan de programación muestra su propia zona. Nunca inferir una hora UTC de un encabezado sin zona.
