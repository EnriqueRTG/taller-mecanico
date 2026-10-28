# Prueba local de respaldo y recuperación

Usar Demo v4 y el servidor seleccionado. Azure no es requisito para este recorrido. Distinguir usuarios internos SGTM de identidades SQL, servicio Windows y permisos de archivos. Estos pasos requieren acceso técnico al servidor, no se ejecutan automáticamente al actualizar Git.

## Preparación técnica

1. Cerrar las aplicaciones. Crear C:\SGTM\Respaldos en el servidor SQL. Identificar la cuenta del servicio SQL Server desde su administrador de configuración y concederle lectura/escritura sobre esa carpeta. La identidad de mantenimiento necesita los permisos SQL detallados en [operación](../respaldos/operacion.md). No dar control total a Everyone como solución.
2. Desde PowerShell en la raíz del servidor, configurar SGTM_CONN para SGTM_DB_Demo, SGTM_MANTENIMIENTO_CONN para la identidad técnica del mismo servidor, Respaldos__BaseControl=SGTM_DB_Demo_Control y Respaldos__Habilitado=false. No compartir la contraseña por chat ni guardarla en Git.
3. Ejecutar dotnet run --project Taller/Taller.Respaldos -- --instalar-control. Confirmar instalación y permisos de lectura de EstadoSGTM para cada estación; el agente y la estación que restaura necesitan los permisos adicionales documentados.
4. Solo después, establecer Respaldos__Habilitado=true y Respaldos__BaseControl=SGTM_DB_Demo_Control en los perfiles de todas las estaciones y del agente. Reiniciar clientes. Una variable del perfil prevalece sobre appsettings.Local.json. Conservar la identidad técnica en el equipo autorizado.

## Aceptación manual y recuperación

1. Administrador: abrir Respaldos y recuperación, elegir C:\SGTM\Respaldos, Completa y Azure desmarcado. Crear y verificar copia; comprobar ruta, historial y archivo .bak en el servidor. Verificar también el respaldo auxiliar del control o resolver su advertencia.
2. Crear un cliente ficticio identificable posterior a la copia. Generar una Diferencial y comprobar su dependencia de la completa. No eliminar ni reemplazar la completa.
3. Seleccionar la completa y Ensayar restauración. Debe recuperar una base temporal y comprobar integridad/esquema, sin borrar el cliente posterior en la base operativa.
4. Para la restauración real, acordar mantenimiento, cerrar la segunda aplicación y consultas externas sobre Demo. Administrador confirma con su contraseña y una segunda identidad distinta, administrador o administrativo, aprueba. Registrar motivo y consecuencias. No hacer este ensayo en una base real.
5. Tras restaurar, reiniciar ambas aplicaciones e ingresar con las credenciales del punto recuperado. Comprobar que el cliente posterior ya no existe al restaurar la completa; el catálogo/auditoría de control deben conservar el evento. Verificar las atenciones y saldos del punto esperado, no solo el mensaje de éxito.
6. Ensayar y restaurar completa+diferencial: el cliente incluido en la diferencial debe reaparecer. Verificar que un cliente conectado/operación activa no sea desconectado a la fuerza ni pierda transacciones; la restauración debe esperar o rechazarse según el caso.
7. Probar aprobación repetida con la misma cuenta, clave errónea, copia faltante y selección modificada después del ensayo: debe bloquearse sin reemplazar la base.

## Automatización

Publicar e instalar un solo servicio de Windows siguiendo [operación](../respaldos/operacion.md) y scripts/Instalar-AgenteRespaldos.ps1. La cuenta del servicio necesita acceso SQL y a la carpeta, independientemente de Visual Studio. Configurar un plan cercano con zona Argentina, guardar y comprobar señal reciente. Cerrar SGTM y verificar un archivo nuevo, fecha/historial y siguiente ejecución. Running del servicio por sí solo no acredita un respaldo.

## Nube después de la prueba local

Azure Blob Storage es réplica opcional de archivos, no el alojamiento de la base. Seguir [Azure](../respaldos/azure.md): contenedor privado, identidad Entra autorizada y ContenedorUri sin secretos. Probar copia local+remota verificada, fallo de transferencia y reintento. La identidad desatendida del servicio es distinta de una sesión interactiva de Visual Studio. No se afirma que exista una cuenta/contenedor real por disponer de las clases implementadas.
