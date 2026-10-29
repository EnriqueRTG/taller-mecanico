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

4. Instalar y comprobar el control y el agente siguiendo [operación de respaldos](operacion.md). Ambos procesos que suban archivos deben tener acceso de lectura al MISMO archivo producido por SQL Server. La opción más sencilla es instalar el agente/estación de respaldo junto al servidor SQL. Si están separados, el técnico debe configurar una carpeta UNC accesible con los permisos adecuados tanto para SQL como para la identidad de respaldos. Una ruta `D:\...` de otra PC no equivale a una carpeta local.
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

## Recorrido guiado para la PC de demostración

Antes de habilitar el módulo, mantener la ejecución conectada a `SGTM_DB_Demo`. Los datos de prueba no crean la base auxiliar ni instalan el agente.

1. En el portal Azure, crear un grupo de recursos para pruebas. Crear una cuenta de almacenamiento **StorageV2**, rendimiento **Standard**, redundancia **LRS** para esta prueba y nivel **Hot**. Revisar el costo estimado de la región antes de confirmar. Esta elección de prueba no equivale a una estrategia de redundancia de producción.
2. En Seguridad/Configuración, deshabilitar acceso anónimo a blobs y exigir HTTPS/TLS 1.2 o superior. Crear en Almacenamiento de datos → Contenedores uno llamado `sgtm-respaldos`, acceso privado.
3. En el contenedor, Control de acceso (IAM) → Agregar asignación de rol, conceder **Storage Blob Data Contributor** a la cuenta Microsoft Entra que se utilizará desde Visual Studio. No es suficiente el rol Contributor de administración de recursos. La asignación puede tardar en propagarse.
4. Iniciar sesión en Visual Studio con esa misma cuenta. Copiar solamente la URL del contenedor, sin claves ni SAS. En una instalación permanente, configurar por separado la identidad del servicio de Windows; no hereda esta sesión.
5. Publicar `Taller.Respaldos` y crear la base de control siguiendo [operación de respaldos](operacion.md), pasos 3 a 6. Usar como base de negocio `SGTM_DB_Demo` y como `Respaldos:BaseControl` **SGTM_DB_Demo_Control**. Crear una carpeta exclusiva de pruebas en el servidor SQL y asignar permisos a la cuenta del servicio SQL. El proceso que replica en Azure también necesita leer los archivos.
6. En el perfil de inicio de Presentación, conservar las dos conexiones Demo y agregar `Respaldos__BaseControl=SGTM_DB_Demo_Control`, `Respaldos__Azure__ContenedorUri=https://SUCUENTA.blob.core.windows.net/sgtm-respaldos`. Solo después de instalar/verificar el control, cambiar `Respaldos__Habilitado=true`. Una variable de entorno prevalece sobre JSON: dejarla en false mantiene bloqueados los botones aunque JSON diga true.
7. Reiniciar SGTM, ingresar como administrador y crear una completa con réplica Azure. Confirmar local verificada, Azure verificada y objeto visible en el contenedor. Conservar completa y diferencial: no subir solamente una diferencial aislada.
8. Probar primero **Ensayar restauración** en Demo. La restauración real exige doble aprobación, afecta toda la base y se prueba aparte. Azure aporta el archivo; SQL Server realiza la recuperación. No se restaura directamente sobre una base alojada en Blob Storage.

No habilitar planes automáticos hasta publicar/configurar el agente y comprobar su identidad SQL y Azure. Una copia manual exitosa desde Visual Studio no demuestra que el servicio pueda subirla sin sesión interactiva.

## Verificación de los archivos remotos

El estado Verificada corresponde al archivo de negocio: se comprueban longitud, SHA-256 y una lectura completa protegida con ETag. La copia auxiliar de control registra EstadoControlAzure, Sha256Control y LongitudControl de forma independiente; si no existe localmente, el resultado conserva una advertencia y no declara verificado ese auxiliar. Los archivos .bak remotos no se sobrescriben. El manifiesto informativo puede actualizarse en reintentos y se relee con su ETag para comprobar su contenido. Un fallo conserva la copia local y deja el intento pendiente. Estas comprobaciones no sustituyen un ensayo de restauración ni una prueba con la identidad Azure del despliegue.
