# Preparar respaldos Azure para la demostración

Azure Blob Storage conserva archivos de respaldo; SQL Server sigue ejecutándose en la notebook elegida como servidor. No es necesario contratar Azure SQL ni publicar SQL Server en Internet para usar la réplica. La LAN de los puestos y el acceso HTTPS a Azure son configuraciones independientes.

## Datos que se deben definir

| Dato | Ejemplo o decisión |
|---|---|
| Equipo y servidor SQL | Notebook que se mantendrá encendida; instancia real, por ejemplo EQUIPO\\SQLEXPRESS |
| Base de negocio/control | SGTM_DB_Demo y SGTM_DB_Demo_Control, separadas de una instalación real |
| Carpeta de staging | C:\SGTM\Respaldos en el servidor; SQL escribe y el proceso de réplica lee/descarga |
| Contenedor privado | https://SUCUENTA.blob.core.windows.net/sgtm-respaldos, sin SAS ni claves |
| Identidad de operación manual | Cuenta Microsoft Entra autenticada en Visual Studio o Azure CLI en ese equipo |
| Identidad automática | Cuenta técnica del servicio Windows, con su configuración Entra propia |
| Plan | Hora, zona Argentina, completa diaria o completa semanal con diferenciales diarias y réplica Azure |

No compartir contraseñas, claves de almacenamiento, SAS, archivos PFX ni su clave por chat o Git. La URL sin parámetros del contenedor y el nombre del equipo/instancia son suficientes para comenzar la guía; las credenciales se configuran localmente.

## Preparación local y SQL

1. Elegir la notebook servidor. Mantener la base y los archivos en ella; los otros puestos apuntarán a esa misma instancia por LAN. Primero comprobar una completa local con [prueba local](../instalacion/respaldos-locales-demo.md).
2. Instalar la base de control y el agente mediante [operación](operacion.md). El usuario interno administrador de SGTM no concede permisos a Windows ni a SQL Server.
3. Crear una carpeta y conceder lectura/escritura a las identidades correspondientes: SQL Server genera .bak; el cliente/agente que replica debe leerlos y debe escribir en staging si descarga desde Azure. Para simplificar, administrar respaldos y ejecutar el agente en la propia notebook servidor.
4. Verificar la copia local, los permisos y espacio para el ensayo de restauración antes de habilitar Azure.

## Cuenta e identidad Azure para la prueba manual

1. En una suscripción propia, crear cuenta de almacenamiento y contenedor privado. Revisar región, redundancia y costos antes de confirmar. El almacenamiento y las transferencias pueden tener costo; la verificación relee el contenido completo.
2. Deshabilitar acceso anónimo y usar HTTPS. Conceder a la identidad operadora **Storage Blob Data Contributor** con alcance limitado al contenedor. Un rol de administración de recursos no sustituye el permiso sobre blobs.
3. Autenticar esa misma cuenta en Visual Studio o Azure CLI del equipo de respaldos. El código usa DefaultAzureCredential y no abre un navegador de autenticación desde el formulario. Si se utiliza CLI, iniciar sesión explícitamente con `az login` en ese equipo y comprobar la cuenta/suscripción seleccionada.
4. Configurar Presentación y el agente con sus conexiones reales y este apartado en appsettings.Local.json:

```json
{
  "Respaldos": {
    "Habilitado": true,
    "BaseControl": "SGTM_DB_Demo_Control",
    "Azure": {
      "ContenedorUri": "https://SUCUENTA.blob.core.windows.net/sgtm-respaldos"
    }
  }
}
```

No activar Habilitado hasta instalar el control y configurar todos los clientes. Revisar variables del perfil de Visual Studio: Respaldos__Habilitado=false prevalece sobre el JSON. Conservar ConnectionStrings:Default y Mantenimiento correctas; Azure no reemplaza ninguna de ellas.

5. Reiniciar SGTM. Crear una completa manual marcando réplica Azure. Verificar las columnas **Azure · negocio** y **Azure · control**, las advertencias y la presencia de negocio.bak, control.bak y manifiesto.json cuando el auxiliar esté disponible.

## Ejecución automática

El servicio Windows no hereda la sesión interactiva de Visual Studio ni basta con tener iniciada la cuenta personal en el navegador. Debe contar con una identidad técnica SQL/archivos y una identidad Entra válidas.

Para una instalación permanente fuera de Azure puede utilizarse una aplicación Entra con certificado: se registra el certificado público en la aplicación y se protege su archivo privado en el servidor. DefaultAzureCredential admite la configuración local AZURE_TENANT_ID, AZURE_CLIENT_ID y AZURE_CLIENT_CERTIFICATE_PATH, y la contraseña del certificado cuando corresponda. El personal técnico debe preparar permisos de archivo y el entorno efectivo del servicio; no almacenar secretos en el repositorio. En un recurso Azure compatible puede preferirse identidad administrada. La elección no cambia el formulario de SGTM.

Una vez configurada esa identidad, guardar el plan desde SGTM, habilitar réplica Azure, comprobar señal reciente y esperar una ejecución real con SGTM/Visual Studio cerrados. Confirmar archivo local, réplica y próxima ejecución. El estado Running del servicio por sí solo no demuestra un respaldo correcto.

## Recuperación desde las copias replicadas

1. En una Demo descartable, seleccionar una copia registrada en el catálogo de control. Una diferencial necesita también su completa base.
2. Si el archivo local no está disponible, el motor puede descargar su réplica Azure registrada y verificada a la ruta de staging, comprobando ETag, tamaño y SHA-256. Si existe un archivo local, no lo sobrescribe ciegamente.
3. Ensayar restauración: SQL recupera en una base temporal, ejecuta CHECKDB y compara esquema. Azure aporta los archivos; no ejecuta RESTORE de SQL Server.
4. Para sustituir la base operativa se requieren ventana de mantenimiento, dos aprobadores, motivo y copia protectora. Se retroceden también usuarios, contraseñas y cobros al punto recuperado. Reiniciar todos los puestos después.
5. Si se perdieron servidor y control/catálogo, no es una recuperación ordinaria de la pantalla: el responsable técnico reconstruye la instalación con los archivos/manifiestos preservados. No se ofrece importar cualquier .bak arbitrario ni saltar las comprobaciones de esquema/familia.

## Orden de comprobación

Completa local → diferencial local → completa replicada → diferencial con su base replicada → plan automático → ensayo de descarga/recuperación en una copia → restauración real únicamente en entorno descartable. Las pruebas de Azure requieren la cuenta y permisos reales del equipo; las pruebas de CI no certifican esa configuración personal.

Referencias de Microsoft: [conexión sin contraseñas](https://learn.microsoft.com/azure/storage/common/migrate-azure-credentials), [cliente Blob .NET](https://learn.microsoft.com/azure/storage/blobs/storage-quickstart-blobs-dotnet), [autenticación mediante entidad de servicio](https://learn.microsoft.com/dotnet/azure/sdk/authentication/local-development-service-principal).
