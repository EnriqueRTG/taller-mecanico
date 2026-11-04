> Registro histórico de la etapa indicada. Estado vigente y alcance de las pruebas: [cierre de coherencia del 04/11/2026](cierre-coherencia-2026-11-04.md).

# Revisión general de código y preparación documental

Fecha: 04/11/2026. Base revisada: `f6d3ab2`, rama `mejora/rendimiento-operaciones`, posterior a la prueba manual satisfactoria del usuario. Esta revisión complementa la optimización; no representa una nueva aceptación manual de todas las pantallas ni una certificación de ausencia de errores.

## Resultado y cambios

| Área | Comprobación y decisión |
|---|---|
| Solución y capas | Ocho proyectos con funciones distintas: cuatro capas, agente, cargador Demo y dos proyectos de pruebas. Referencias de proyectos válidas; pruebas de arquitectura para el sentido de las dependencias. No se retiran herramientas de instalación o pruebas por no ejecutarse durante el uso habitual. |
| Código antiguo | Se retiran `ListarAtencionesParaCobrosAsync` y `ListarComprobantesAsync` del contrato y repositorio: no tienen consumidores y quedaron sustituidos por la proyección de la bandeja económica. Se conserva la consulta completa de un comprobante para operaciones que necesitan sus detalles y coherencia transaccional. |
| Autorización | `ExigirAsync` delega en `ExigirRolesAsync`: una sola implementación revalida usuario y rol persistidos. El permiso predeterminado sigue limitado a recepción/técnico; el administrador no recibe permisos operativos nuevos. |
| Vehículos | Las marcas disponibles se consultan con `Any` sobre modelos activos; al elegir una marca se filtran los modelos en SQL. Antes se materializaban todos los modelos disponibles para luego filtrarlos en memoria. La edición conserva su consulta especial para un modelo histórico inactivo. |
| Dependencias | Se retira `Serilog.Settings.Configuration`: no existen llamadas a `ReadFrom.Configuration` ni configuración Serilog consumida desde JSON; el registro real se construye por código con `Serilog` y `Serilog.Sinks.File`, que se conservan. |
| Validación automática | Los cambios en `scripts/**` o `global.json` también disparan los workflows. Antes un cambio exclusivo de scripts podía no ejecutar la validación de sintaxis PowerShell ni las comprobaciones de la solución. |
| Referencias documentales | Se reemplazan tres enlaces locales rotos a guías Azure eliminadas y el procedimiento obsoleto de nube de la guía de prueba local por el recorrido de Google Drive vigente. La revisión académica completa se realiza después. |

## Estructuras que deben conservarse

- Las configuraciones `IEntityTypeConfiguration` se descubren con `ApplyConfigurationsFromAssembly`; la ausencia de referencias directas a sus nombres no las hace prescindibles.
- `TallerDbContextFactory` se utiliza mediante las herramientas de EF. Las seis migraciones y sus archivos generados describen el esquema y su evolución: no se borran ni se vuelve a generar la base.
- Los `Designer.cs`, `.resx`, recursos de imagen y perfiles locales tienen usos del diseñador, compilador o configuración por equipo. No se eliminan por una búsqueda textual sin resultados.
- Demo, agente Windows, tarea interactiva de Drive y pruebas de interfaz cumplen funciones distintas. Las alternativas de ejecución del agente no deben ejecutarse simultáneamente para el mismo plan, pero no son código duplicado que pueda borrarse sin afectar instalaciones.
- Los documentos de revisiones anteriores conservan evidencia y limitaciones de cada etapa. No son especificaciones actuales alternativas ni se usan como base para cambiar reglas del negocio.

## Lógica con coordinación necesaria

| Componente | Responsabilidad que justifica conservar la coordinación |
|---|---|
| `EvaluacionServicio` | Reserva técnica, solicitudes de ingreso, informe, diagnóstico y propuesta asociados a la misma atención. |
| `SeguimientoTrabajoServicio` | Selección parcial, reformulación, hallazgos, pausas y adicionales; diferencia entre validar lo elegido y cambiar el acuerdo. |
| `EjecucionServicio` | Alcance autorizado, cantidades realizadas, materiales y finalización. |
| `PresupuestoServicio` | Presentación, decisión del cliente, vigencia e historial. |
| `CobroServicio` | Liquidación, saldos, medios, idempotencia de cobros, anulaciones y entrega. |
| `UnidadDeTrabajo` | Guardado atómico, concurrencia, descarte de cambios fallidos y lectura consistente cuando se requiere. |
| `RespaldoServicio`, motor SQL e interceptor | Ensayo, doble aprobación, mantenimiento, generaciones y protección de sesiones antes del reemplazo real. |

Se revisaron las entradas y los puntos de guardado/descarte de estos servicios. No se introducen envoltorios genéricos adicionales ni se reescribe su flujo ya probado solo para reducir líneas. La complejidad de concurrencia, pagos y recuperación no es un indicador suficiente de código obsoleto.

La inicialización todavía comprueba marcas/modelos básicos individualmente bajo coordinación de instalación. Puede ser objeto de una optimización específica si se mide una demora de arranque restante; no se retiran esas verificaciones durante una limpieza ni se atribuye a ellas un problema de rendimiento ya resuelto en las operaciones habituales.

## Verificación

- Compilación de los ocho proyectos con .NET SDK 10.0.403 y Windows targeting desde Linux.
- Pruebas de reglas, arquitectura, coincidencia del modelo con las seis migraciones, documentos, reportes, maestros y componentes.
- Caso adicional de selectores de vehículos: marcas sin modelos, modelos inactivos y marcas deshabilitadas no se ofrecen en altas; un modelo inactivo actual sigue disponible en edición; las consultas no dejan entidades rastreadas.
- Traducción SQL Server de las dos nuevas consultas, deteniendo la conexión después de traducir. Esto no sustituye su ejecución con SQL real.
- Validación estructural de proyectos, recursos XML, filtro de solución y archivos SVG/drawio; comprobación de enlaces Markdown locales. Esta revisión de diagramas es de integridad, no de contenido ni de legibilidad visual.
- El PR ejecuta además la construcción WinForms en Windows/STA y la batería SQL con bases temporales, incluyendo recuperación. Consultar los checks del último commit para su resultado; SQL y WinForms no se ejecutan en el entorno Linux local.

La prueba manual satisfactoria informada por el usuario corresponde a la versión anterior a esta limpieza. Conviene repetir alta de vehículo, cambio de marca y apertura de los módulos principales tras actualizar. Los cambios no requieren migraciones, recarga Demo ni cambios de configuración.

## Siguiente revisión: documentación académica

Contrastar ERS, apéndice A, diccionario y diagramas contra esta versión del código. Priorizar estados reales, selección parcial validada sin una segunda aceptación, adicionales, pausas/materiales, liquidación/cobros/entrega, permisos por rol y respaldo mediante Drive. Actualizar trazabilidad y referencias cruzadas conjuntamente. Los manuales de usuario e instalación se redactan al final sobre esa base estabilizada.
