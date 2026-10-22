# Integración de catálogo y protección de datos maestros

## Alcance

Esta rama incluye el circuito atención/evaluación/presupuesto/ejecución/cobros, reportes y respaldos de los PR anteriores, y agrega la integración compatible de catálogo y las protecciones de clientes/vehículos. No aplica ni incorpora `AjustarCatalogoActual` de la rama paralela: esa migración elimina tablas y columnas del circuito.

La intención del catálogo paralelo se conserva: consulta por categoría, tipo y búsqueda, con controles separados en Designer. Se implementan los filtros combinados, disponibilidad, actualización y limpieza, detalle legible y precios reales a partir del esquema vigente. No se copian constantes de categorías fijas ni snapshots/migraciones incompatibles. No es un ABM de stock ni inventario: la disponibilidad representa concepto y categoría habilitados; los precios son de referencia, no costos de compra ni importes de presupuestos históricos. Un precio nulo se muestra como «A cotizar», nunca se reemplaza por cero. Administrativo y técnico pueden consultarlo; el administrador mantiene sus funciones de seguridad, reportes y respaldos.

## Clientes y vehículos

- Consultas persistidas, búsqueda sin distinguir mayúsculas/tildes y filtros de habilitados/deshabilitados/todos. Los registros activos tienen prioridad y luego se ordenan por alta más reciente e identificador.
- Las altas y modificaciones de clientes/vehículos exigen recepción con cuenta y rol vigentes comprobados en base de datos. El técnico solo puede consultar un vehículo para su historial; no administra los datos maestros.
- Alta y edición comparten validaciones de aplicación: obligatorios, longitudes de las columnas, DNI de 7/8 dígitos, CUIT/CUIL con verificador, pasaporte alfanumérico, correo y teléfono opcionales con formato, dominio normalizado, año permitido y marca/modelo disponible.
- Los clientes de tipo persona/empresa conservan nombre/apellido como está definido el modelo vigente; no se incorporan nuevas entidades de titularidad.
- Se agrega rowversion a Clientes y Vehiculos. El editor conserva la versión inicial; no la renueva antes del guardado. Bajas/reactivaciones también envían la versión de la fila seleccionada.
- La edición recibe DTOs con campos reales separados. No se reconstruyen nombres ni marcas dividiendo el texto presentado en una grilla. El modelo del vehículo se selecciona entre modelos reales; puede conservarse el actual si fue inactivado, pero no asignarse uno inactivo distinto.
- Las actualizaciones solo modifican campos editables; conservan fecha de alta e historial. Los índices únicos siguen siendo la última defensa contra altas duplicadas concurrentes y sus errores se traducen en mensajes de validación.

## Instalación

1. Probar primero en una copia y conservar un respaldo independiente.
2. Descargar esta versión completa y compilar. No copiar la migración destructiva de catálogo.
3. Aplicar `ProtegerEdicionClientesVehiculos` mediante Update-Database o la herramienta EF habitual. Su Up agrega únicamente dos columnas rowversion; no elimina tablas ni registros. Si se parte de una versión anterior, aplicar la cadena completa de migraciones del proyecto.
4. Mantener sincronizados todos los clientes que comparten la base. Eliminar el uso de versiones anteriores del editor, que no conocen estas protecciones.
5. Probar con dos instancias: abrir el mismo cliente/vehículo en ambas; guardar desde una; guardar desde la otra debe informar conflicto sin sobrescribir.

La restauración solo admite copias compatibles con el esquema instalado. Después de migrar, crear una nueva completa y ensayar la restauración en la instalación de prueba, conforme a la guía de respaldos. No se restaura una copia de esquema anterior sobre esta versión por el flujo operativo.

## Autorización y errores

Los servicios operativos revalidan usuario activo, rol activo, identificador y nombre del rol esperado; no confían en el menú ni en un RolId modificado en memoria. Usuarios exige sesión administrativa y vincula el ejecutor a esa sesión. También se protegen las consultas de atenciones e informes.

Los formularios usan un tratamiento común para validaciones, conflictos y errores de SQL/persistencia. El detalle técnico se escribe con una referencia en `%LOCALAPPDATA%/SGTM/Logs`, con rotación diaria, límite de tamaño y retención de siete archivos. No se registran credenciales de formularios; los archivos de diagnóstico se deben tratar como información interna. La pantalla de restauración conserva sus avisos específicos de mantenimiento y rutas protectoras para la asistencia técnica.

## Código conservado para decisión del equipo

| Archivo | Evaluación | Acción |
| --- | --- | --- |
| `Taller.Aplicacion/Servicios/BackupServicio.cs` | Clase vacía anterior a la implementación real; reemplazada por RespaldoServicio. | Marcada Obsolete y conservada. No registrada en DI. Candidata a retiro. |
| `Taller.Presentacion/Formularios/Backup/FrmBackup.*` | Prototipo sin lógica, reemplazado por FrmRespaldoRestauracion. | Marcado Obsolete y conservado junto con Designer y recursos. Sin acceso desde menú ni DI. Candidato a retiro. |
| `DiagnosticoServicio` / `DiagnosticoRepositorio` | Consulta de informes definitivos e históricos. EvaluacionServicio coordina el registro técnico y la propuesta. | Se mantienen con responsabilidad explícita; no son clases vacías. |
| `CategoriaServicio` | Consulta auxiliar por identificador, protegida por sesión operativa. | Se conserva para consultas auxiliares; no obliga categorías fijas al catálogo. |

No se elimina código legado sin la decisión solicitada por el usuario. Los formularios y servicios vigentes no generan información ficticia de negocio.

## Verificación antes de integrar a main

- Compilación Release y pruebas generales.
- SQL aislado: migración completa conservando el esquema del circuito, actualización no destructiva con datos existentes, altas simultáneas protegidas por índice único, rowversion real de clientes/vehículos y flujo desde alta hasta entrega con cobros mixtos y saldo cero.
- Se conservan las pruebas nativas de copias/restauración y fallos protectores.
- Inspección visual pendiente en Windows a 1366×768/1920×1080 y DPI 100/125/150%, usando la base migrada: catálogo, filtros, detalle, edición de modelo, permisos por rol y mensajes de conflicto.

Validación local del 22/10/2026: compilación sin advertencias, 95 pruebas generales aprobadas y 16 pruebas sobre SQL Server 2025 Developer aislado aprobadas. La comprobación de EF no detectó diferencias entre modelo y migraciones. Las pruebas SQL incluyen alta simultánea, actualización con datos previos y el circuito de negocio completo. La validación visual en Windows sigue siendo un control de aceptación pendiente.
