# Respaldos locales y réplica Azure

## Modelo de operación

SQL Server crea una copia nativa completa o diferencial en una carpeta del servidor, con checksum y verificación. El administrador puede marcar **Replicar también en Azure privado** tanto para una copia manual como para el plan automático. La copia local se conserva: sirve de staging y de recuperación inmediata. No se ofrece eliminación automática de archivos ni una política de retención destructiva.

Azure es almacenamiento de archivos, no una base SQL nueva. La copia diferencial exige su completa base; el sistema replica también esa base si aún falta en la nube. Una transferencia fallida no invalida la copia local ni se anuncia como copia remota verificada. La grilla muestra `EstadoAzure`. El administrador puede seleccionar la copia y pulsar **Reintentar Azure**. Con el plan automático activo, el agente también reintenta hasta dos réplicas pendientes por ciclo, dejando al menos 15 minutos entre intentos; no genera nuevamente el respaldo SQL para ese reintento.

## Instalación técnica inicial

1. Crear una suscripción Azure, cuenta Storage de propósito general y un contenedor privado, por ejemplo `sgtm-respaldos`. Deshabilitar acceso público a blobs. Considerar región, costos, transferencia de lectura y retención; esta solución verifica leyendo de vuelta todo el respaldo.
2. Usar Microsoft Entra. Asignar **Storage Blob Data Contributor** limitado al contenedor a la identidad operadora de respaldos. Para la estación de pruebas puede usarse la identidad iniciada en Visual Studio o Azure CLI; el servicio de Windows necesita una identidad propia, no depende de la sesión de Visual Studio del desarrollador. Para instalación permanente preferir una identidad administrada si se ejecuta en Azure, o una aplicación Entra con certificado protegido en el servidor; `DefaultAzureCredential` también admite credenciales de entorno cuando el instalador las configure. Nunca incluir secretos en Git ni pedirlos por chat.
3. Configurar en `appsettings.Local.json` de Presentación y del agente:

```json
{
  "Respaldos": {
    "Habilitado": true,
    "BaseControl": "SGTM_DB_Demo_Control",
    "Azure": { "ContenedorUri": "https://SUCUENTA.blob.core.windows.net/sgtm-respaldos" }
  }
}
```

Ajustar `BaseControl` a la instalación elegida, conservando las conexiones `Default` y `Mantenimiento` que ya usa. El endpoint no admite SAS, query strings, HTTP ni dominios externos. Las claves no se introducen en los formularios de operación.

4. Instalar y comprobar el control y el agente siguiendo `respaldos-restauracion.md`. Ambos procesos que suban archivos deben tener acceso de lectura al MISMO archivo producido por SQL Server. La opción más sencilla es instalar el agente/estación de respaldo junto al servidor SQL. Si están separados, el técnico debe configurar una carpeta UNC accesible con los permisos adecuados tanto para SQL como para la identidad de respaldos. Una ruta `D:\...` de otra PC no equivale a una carpeta local.
5. Conceder escritura en staging para descargar una copia remota perdida. Proteger la carpeta y permitir a SQL leer los archivos recuperados. Las credenciales SQL y de Azure son independientes.

## Operación manual

1. Entrar como administrador y abrir Respaldos.
2. En Copias manuales, indicar carpeta del servidor, elegir Completa o Diferencial y marcar Azure si corresponde.
3. Crear la copia y revisar el resultado y la columna Azure. Si está pendiente, revisar permisos/conexión y reintentar la copia seleccionada. La columna de advertencia explica que el respaldo local se conserva.
4. Primero crear una completa. Una diferencial aislada no permite recuperar todos los datos.

## Programación

Indicar carpeta, horario, zona, esquema de completas/diferenciales y si se replica en Azure. Guardar el plan no instala el agente. Confirmar la señal reciente y una ejecución efectiva. Los campos de planificación y destino se guardan con control de concurrencia en la base de control separada.

## Integridad y recuperación

Los objetos se identifican por familia de base e ID de copia. La subida principal es condicional: no sobrescribe un objeto existente. Se comprueban longitud, metadata y SHA-256 leyendo de vuelta el contenido con ETag. Cada copia conserva su manifiesto sin credenciales. Se sube además la copia auxiliar de control si está disponible; no reemplaza la necesidad de conservar y proteger la base de control activa.

Al ensayar una copia registrada cuyo archivo local ya no existe, el sistema descarga su réplica verificada al staging, exige el ETag guardado y compara longitud y SHA-256 antes de mover el archivo. No sobrescribe archivos locales existentes. Después se aplican las verificaciones y ensayo SQL normales, incluida la completa base de una diferencial. La restauración real mantiene la doble aprobación y la copia protectora.

Una pérdida total del servidor y de la base de control requiere intervención técnica: usar los manifiestos y la copia de control para reconstruir la instalación en un entorno separado antes de recuperar negocio. No existe un botón que importe manifiestos arbitrarios ni se elude la validación de familia/esquema.

## Prueba de aceptación con la cuenta real

- Crear una completa con Azure, comprobar estado Verificada y objetos en contenedor privado.
- Crear una diferencial y comprobar que su completa también está replicada.
- Cortar conectividad al subir: debe conservarse la local y quedar pendiente. Recuperar conexión y reintentar sin duplicar el respaldo SQL.
- En una base descartable, retirar el archivo de staging y ensayar restauración: debe descargarse, verificarse y recuperarse en una base temporal.
- Comprobar ejecución del agente sin Visual Studio abierto.

La configuración y pruebas con una suscripción real deben realizarse en el equipo del usuario. Las pruebas automatizadas de coordinación usan un proveedor de transferencia controlado; no afirman haber comprobado sus credenciales Azure.

Referencias: https://learn.microsoft.com/azure/storage/blobs/storage-quickstart-blobs-dotnet y https://learn.microsoft.com/azure/storage/blobs/storage-blob-upload
