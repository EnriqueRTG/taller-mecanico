# Rendimiento de operaciones y protección de restauración

## Criterio de los servicios

Cada operación pública autoriza, obtiene los datos actuales que necesita, valida,
aplica cambios e historial y confirma una vez. Los auxiliares reutilizan esos datos:
no vuelven a cargar una atención o un catálogo ya disponible dentro de esa operación.
La interfaz orienta al usuario; las reglas siguen comprobándose en Aplicación y las
restricciones y versiones siguen protegiendo el guardado en SQL.

No se cachean permisos durante toda la sesión. Evaluación, reportes y respaldos usan
la misma política `AutorizacionOperativa`. Inicio hace una autorización para sus tres
tarjetas y pendientes. Las tarjetas comparten las fórmulas de los reportes, pero utilizan
proyecciones pequeñas y no crean gráficos ni filas detalladas para la pantalla inicial.
El redondeo de presupuestos se conserva por ítem, igual que en el documento.

Las bandejas de cobros y ejecución consultan columnas y contadores, sin traer el grafo
completo de pagos o historiales. La lectura de la cuenta conserva su transacción
consistente; los guardados mantienen atomicidad, control de versión e idempotencia.
La selección parcial que el técnico considera viable mantiene la decisión original:
no se altera su circuito ni se exige otra aceptación si no cambian las condiciones.

## Conexiones y mantenimiento: orden importante

1. Antes de abrir negocio, el interceptor toma un permiso SQL compartido en la base
   de control y comprueba estado y generación en un solo comando.
2. La consulta o transacción de negocio utiliza una conexión reutilizable.
3. Antes de devolver negocio al pool, cambia su catálogo a `master`. Una sesión
   física dormida ya no queda usando la base que una restauración debe reemplazar.
   SqlClient restablece el catálogo original en la siguiente apertura, que vuelve
   a pasar primero por el control de mantenimiento.
4. Sólo después del cierre/disposición real de negocio se libera explícitamente
   el permiso de control y se devuelve esa conexión a su pool independiente.
5. Si falla el estacionamiento o la liberación, se marca el pool correspondiente
   para descartar la sesión al cerrarse. No se reutiliza una sesión de estado incierto.

Esto permite reutilizar conexiones sin eliminar el protocolo de restauración:
mantenimiento durable, exclusivo SQL, operaciones activas, sesiones externas,
protectora y generación siguen vigentes. El interceptor cubre cierres y disposiciones
síncronos y asíncronos. No se mantiene una conexión abierta durante toda la interfaz.
Las conexiones administrativas de backup/restore siguen dedicadas y sin pool.

## Verificación

La suite SQL usa bases aleatorias de prueba. Incluye apertura activa que impide
restaurar, cierre y disposición que liberan el permiso, reutilización de SPID,
restauración real con conexiones ociosas estacionadas y rechazo de sesiones anteriores
por cambio de generación. La suite de componentes compara las tarjetas livianas con
los reportes de los tres roles y comprueba traducción SQL de las nuevas consultas.

Para comprobar la mejora en Windows, usar el mismo lote y perfil antes/después y
registrar cinco cargas de Inicio, Catálogo, Vehículos, Atenciones, Ejecución y
presupuesto. Separar primera carga de las siguientes. `FrmDetalleAtencion` conserva
sus mediciones locales. El resultado medido por el usuario y la prueba funcional del
circuito principal complementan las suites; no se promete una latencia de hardware
que no se ha medido.
