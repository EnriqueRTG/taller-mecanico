# Actualización para el circuito de coloquio

Esta versión cambia reglas y restricciones: no es solamente una reorganización visual. Guardar los cambios propios, actualizar main y comprobar que ambos equipos usan el mismo commit antes de probar. No generar migraciones en cada PC.

## Base existente

Cerrar SGTM en todas las estaciones y detener el agente antes de actualizar el esquema. Conservar un respaldo independiente de la base que se desee preservar. En SSMS, sobre la conexión confirmada, comprobar:

```sql
USE SGTM_DB_Demo;
SELECT MigrationId FROM dbo.__EFMigrationsHistory ORDER BY MigrationId;
SELECT EstadoEjecucion, COUNT(*) AS Cantidad
FROM dbo.PresupuestoDetalles GROUP BY EstadoEjecucion;
```

El esquema actual tiene EsquemaInicialCircuitoActual y SimplificarEjecucionTresEstados. Si existen Parcial u Omitido, la segunda migración rechaza la actualización antes de cambiar datos. No se convierten en Completado ni se reabren atenciones cerradas silenciosamente.

- **Demo anterior descartable:** conservar una copia si interesa y, confirmado el destino, eliminar únicamente SGTM_DB_Demo desde SSMS y cargar el lote v4 según [replicar Demo](replicar-demo.txt). Se mantiene el nombre acordado, no crear otra base para cada prueba. La eliminación pierde los registros propios; no es necesaria si ya tiene v4.
- **Base con información que se quiere conservar y sin estados retirados:** aplicar la migración existente. Revisar primero sobre una copia. Desde la raíz, con SGTM_CONN hacia el destino correcto y dotnet-ef instalado en la versión del proyecto:

```powershell
dotnet ef database update --project Taller/Taller.Infraestructura --startup-project Taller/Taller.Infraestructura
```

- **Datos reales con estados retirados:** detener la actualización y definir una conversión supervisada de esos casos. No borrar datos comerciales para hacer pasar una migración.

En Demo v4 el cargador aplica ambas migraciones antes del lote. Mantiene 19 clientes, 48 vehículos, 48 atenciones y 158 conceptos. Incluye selección validada sin cambios, reformulaciones, tres estados de ejecución, inspecciones y pagos. Los conteos de reportes pueden variar respecto de v3. No volver a cargar sobre una base poblada.

## Respaldos y todas las estaciones

Actualizar el esquema una sola vez desde el servidor, con los clientes cerrados. La aplicación de escritorio no aplica migraciones al arrancar. Actualizar después todos los ejecutables/perfiles antes de reanudar operaciones. No mezclar clientes de las dos versiones.

Si se recreó Demo, sus respaldos previos pertenecen a otra familia/esquema: no restaurarlos sobre v4. Si ya se instaló una base de control de Demo, conservar su copia/catálogo anterior y coordinar su preparación con el responsable técnico antes de habilitar el módulo para la base recreada. No borrar ni reutilizar a ciegas una base de control real. Ver [respaldos](respaldos-locales-demo.md).

Los tres DOCX académicos se conservan sin cambios. Deben actualizarse después de validar esta versión; no se afirma que ya reflejen estas reglas.
