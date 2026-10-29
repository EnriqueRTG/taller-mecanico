# Evidencia de revisión de cierre

Fecha: 29/10/2026. Base de código: main 253e7d6 y cambios de esta rama de cierre documental. La versión exacta se identifica con el commit que contiene este documento; no extrapolar resultados a una modificación posterior.

## Comprobaciones realizadas

- Compilación Release de la solución completa con .NET SDK 10.0.400: cero errores y cero advertencias. En Linux se habilita EnableWindowsTargeting para compilar los proyectos Windows; no ejecuta sus formularios.
- Pruebas de reglas/componentes y arquitectura: 149 aprobadas. Las pruebas de SQL real requieren instancia desechable y quedan explícitamente omitidas al no configurarla. No se informa como aprobado lo omitido.
- Modelo SQL configurado frente a snapshot: sin cambios pendientes. La prueba ModeloPersistenciaPruebas genera el DDL sin conectarse y comprueba relaciones e inventario de migraciones.
- EF CLI 10.0.11: has-pending-model-changes sin diferencias; generado script SQL de la migración de limpieza sin conectarse ni modificar bases de usuarios.
- Documentos: lectura posterior de originales, exportación DOCX y renderizado para revisar disposición. Diagramas: XML editable y vistas SVG/PNG revisados.
- Referencias internas: revisión de rutas locales y enlaces entre guías, requisitos, servicios y pruebas.

## Comprobaciones que pertenecen al despliegue

Ejecutar [plan integral](aceptacion-version-actual.md) en Windows y SQL Server del entorno de demostración: operación entre dos puestos, concurrencia, DPI, vista previa/impresión, lote, informes frente a SQL y configuración de agente/Azure. Probar recuperación en una copia descartable y conservar respaldo independiente. No se modificó la base real del usuario durante esta revisión.

Las pruebas SQL de CI crean sus propias bases temporales y requieren SGTM_SQL_PRUEBAS, SGTM_PERMITIR_PRUEBAS_SQL y una carpeta accesible por el servicio SQL. Su resultado se consulta en el PR correspondiente; este documento no sustituye ese resultado ni afirma aprobación futura.

## Limpieza y elementos conservados

Se retira IdPresupuestoCambio del dominio/configuración/snapshot mediante migración nueva. Su guardia impide descartar valores antiguos no nulos. Las migraciones previas permanecen porque ya pudieron aplicarse; constituyen el historial ejecutable para crear/actualizar bases, no rutas de negocio obsoletas.

Demostración prepara el lote explícito; Respaldos ejecuta calendario y control; ambos proyectos de pruebas verifican capacidades distintas. Tienen uso y las capas no dependen de ellos. No se eliminan archivos por carecer de un botón directo cuando sirven a migración, composición, pruebas o herramientas.

Se retiran las notas de análisis anteriores y los DOCX del repositorio reemplazados por v2.0. Las guías vigentes describen el mismo alcance. No se afirma ausencia universal de fallos ni se reemplaza la aceptación manual del taller por esta revisión.
