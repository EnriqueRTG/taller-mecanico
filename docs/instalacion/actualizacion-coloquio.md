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

El esquema actual tiene seis migraciones. Las dos primeras son EsquemaInicialCircuitoActual y SimplificarEjecucionTresEstados; las restantes se detallan más abajo. Si existen Parcial u Omitido, la segunda migración rechaza la actualización antes de cambiar datos. No se convierten en Completado ni se reabren atenciones cerradas silenciosamente.

- **Demo anterior descartable:** conservar una copia si interesa y, confirmado el destino, eliminar únicamente SGTM_DB_Demo desde SSMS y cargar el lote v5 según [replicar Demo](replicar-demo.txt). Se mantiene el nombre acordado, no crear otra base para cada prueba. La eliminación pierde los registros propios; no es necesaria para actualizar el código; solo para sustituir el lote anterior por v5.
- **Base con información que se quiere conservar y sin estados retirados:** aplicar todas las migraciones pendientes existentes. Revisar primero sobre una copia. Desde la raíz, con SGTM_CONN hacia el destino correcto y dotnet-ef instalado en la versión del proyecto:

```powershell
dotnet ef database update --project Taller/Taller.Infraestructura --startup-project Taller/Taller.Infraestructura
```

- **Datos reales con estados retirados:** detener la actualización y definir una conversión supervisada de esos casos. No borrar datos comerciales para hacer pasar una migración.

En Demo v5 el cargador aplica las seis migraciones antes del lote. Mantiene 88 clientes, 256 vehículos, 240 atenciones y 354 conceptos. Incluye selección validada sin cambios, reformulaciones, tres estados de ejecución, inspecciones y pagos. Los conteos de reportes pueden variar respecto de v3. No volver a cargar sobre una base poblada.

## Respaldos y todas las estaciones

Actualizar el esquema una sola vez desde el servidor, con los clientes cerrados. La aplicación de escritorio no aplica migraciones al arrancar. Actualizar después todos los ejecutables/perfiles antes de reanudar operaciones. No mezclar clientes de las dos versiones.

Si se recreó Demo, sus respaldos previos pertenecen a otra familia/esquema: no restaurarlos sobre una base recreada. Si ya se instaló una base de control de Demo, conservar su copia/catálogo anterior y coordinar su preparación con el responsable técnico antes de habilitar el módulo para la base recreada. No borrar ni reutilizar a ciegas una base de control real. Ver [respaldos](respaldos-locales-demo.md).

Los tres DOCX académicos y los diagramas v2.0 describen las reglas actuales; ver la matriz de trazabilidad.

## Limpieza documental y de esquema del 29/10/2026

La tercera migración `20261029090000_RetirarVinculoCambioSinUso` retira únicamente IdPresupuestoCambio, su FK, índice y CHECK. Las revisiones válidas usan Presupuesto.IdPresupuestoAnterior. No cambia estados, precios, solicitudes ni cobros del lote.

Para una base ya actualizada a tres estados: respaldar, cerrar todos los puestos y agente, confirmar destino y aplicar `dotnet ef database update` con los proyectos indicados arriba. No crear otra migración ni recrear la base por esta limpieza. Si hay vínculos antiguos no nulos, la migración se detiene sin borrarlos; solicitar revisión de esos registros. La reversión técnica reconstruye la columna nullable, no recupera valores descartados; por eso se impide descartarlos en Up.

Aplicar el mismo código en todas las estaciones. Las copias anteriores a la tercera migración no se deben restaurar como si tuvieran el esquema actual: el ensayo comprueba compatibilidad. Conservarlas identificadas y crear una completa nueva tras la actualización verificada.

## Teléfono obligatorio del cliente

La migración `20261029190000_ExigirTelefonoContactoCliente` exige contacto telefónico de 7 a 15 dígitos. Antes de actualizar, completar los contactos faltantes o inválidos con datos reales mediante la versión anterior. La migración se detiene con el error 51142 si detecta alguno; no elimina clientes ni inventa teléfonos. Después de corregirlos, volver a ejecutar la actualización. El lote de demostración ya incluye contacto en clientes activos e inactivos.

Consulta previa, de solo lectura, para localizar contactos ausentes o evidentemente incompletos:

```sql
SELECT IdCliente, Nombre, Apellido, Telefono
FROM dbo.Clientes
WHERE Telefono IS NULL OR LEN(LTRIM(RTRIM(Telefono))) < 7;
```

Esta consulta inicial no reemplaza la validación completa de formato aplicada por la migración. Respaldar la base y cerrar los puestos antes de actualizar el esquema.

## Concurrencia de usuarios

`20261029210000_AgregarConcurrenciaUsuarios` agrega Usuarios.Version y actualiza los SP de edición, credenciales y estado. Conserva datos y procedimientos de alta. Los tres SP exigen la versión consultada. Una edición obsoleta produce un conflicto y no sobrescribe cambios.

Respaldar previamente, cerrar todos los puestos y aplicar todas las migraciones pendientes desde una única estación de mantenimiento. Actualizar luego todos los clientes. No mezclar ejecutables antiguos con los SP nuevos ni generar otra migración por cuenta propia. No es necesario recrear una base actualizada ni recargar Demo.

## Contacto de la decisión del presupuesto

La sexta migración `20261029220000_ExigirContactoDecisionPresupuesto` agrega CK_Presupuestos_ContactoDecision. Las decisiones aceptadas, rechazadas o pendientes de revisión técnica requieren persona y medio Presencial, Teléfono, Correo o Mensajería. Si hay decisiones incompletas se detiene con error 51144, sin inventar datos ni eliminar propuestas. Revisar con el responsable técnico la evidencia real de esos registros y corregirlos de forma supervisada; después repetir la actualización. No recrear una base válida ni recargar el lote para evitar el control.

Consulta previa de solo lectura:

```sql
SELECT IdPresupuesto, Estado, PersonaDecision, MedioDecision
FROM dbo.Presupuestos
WHERE Estado IN (N'Aceptado', N'Rechazado', N'PendienteRevisionTecnica')
AND (PersonaDecision IS NULL OR LEN(LTRIM(RTRIM(PersonaDecision)))=0
     OR MedioDecision IS NULL
     OR MedioDecision NOT IN (N'Presencial', N'Teléfono', N'Correo', N'Mensajería'));
```

Cerrar clientes y agente, respaldar, confirmar el destino y ejecutar la actualización completa una sola vez. Verificar seis filas en __EFMigrationsHistory, actualizar todos los ejecutables y crear una completa nueva tras comprobar el circuito. Las copias con un esquema anterior se conservan identificadas para recuperación técnica, no se presentan como compatibles con el nuevo esquema.

## Lote ampliado Demo v5

La ampliación del lote no agrega una migración: se mantienen las seis del esquema actual. No borrar la base para actualizar el programa. Una Demo ya poblada conserva sus datos y no se mezcla con el lote nuevo.

Para sustituir voluntariamente una Demo v4 descartable por v5, respaldar, cerrar puestos/agente y confirmar en SSMS que se trata únicamente de SGTM_DB_Demo. Solo entonces eliminar esa Demo y ejecutar el cargador de la guía de réplica para que la cree con el mismo nombre. Esta decisión pierde las operaciones de prueba anteriores; no aplicarla a SGTM_DB ni a información real. Si ya existe un control de respaldos, su catálogo conserva otra familia de base: coordinar con el responsable técnico su configuración y conservar las copias anteriores identificadas, sin reutilizarlo a ciegas.

La preparación v5 corrige la coordinación con transacciones explícitas del cargador, que ahora utiliza el mismo recorrido que la prueba SQL. Los cambios no alteran estados, relaciones ni reglas de presupuesto/pago/entrega.
