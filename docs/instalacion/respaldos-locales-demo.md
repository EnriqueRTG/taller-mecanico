# Prueba local de respaldo y recuperación

Usar Demo v5 y el servidor seleccionado. Azure no es requisito para este recorrido. Distinguir usuarios internos SGTM de identidades SQL, servicio Windows y permisos de archivos. Estos pasos requieren acceso técnico al servidor, no se ejecutan automáticamente al actualizar Git.

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

## Preparación asistida de Demo en un solo equipo Windows

Esta herramienta está limitada a `SGTM_DB_Demo` e instancia SQL local con autenticación Windows. No usarla para configurar producción ni para reemplazar datos. Cerrar SGTM, agente y Visual Studio. Actualizar main antes de ejecutarla.

Abrir **Windows PowerShell como administrador de Windows**, entrar en la raíz del repositorio y ejecutar:

```powershell
powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\scripts\Preparar-RespaldosDemo.ps1
```

`Bypass` se limita a ese proceso; no cambia permanentemente la política del equipo. Revisar el script antes de ejecutarlo. La cuenta Windows debe tener acceso SQL a la Demo y permisos de crear la base de control; ser administrador de Windows no concede automáticamente permisos en SQL Server.

La herramienta verifica acceso/esquema de Demo sin modificarlo, detecta la cuenta real del servicio SQL, crea `C:\SGTM\Respaldos` y concede a esa cuenta permiso Modify en esa carpeta. Instala y consulta `SGTM_DB_Demo_Control`; solo después habilita los perfiles Project F5 y prepara la configuración local del agente. Conserva las configuraciones anteriores fuera del repositorio, en `%LOCALAPPDATA%\SGTM\Configuracion`. No elimina copias, no restaura, no carga registros de negocio y no activa Azure. Si falla puede quedar la carpeta o la base de control creada; corregir el problema y repetir es admisible. No borre bases para resolver un fallo de preparación.

Reabrir Visual Studio, reconstruir y ejecutar el perfil Project. Entrar como `demo.admin` y abrir Respaldos y recuperación. Crear una **Completa**, carpeta `C:\SGTM\Respaldos`, Azure desmarcado. Comprobar el mensaje, el archivo `.bak` y el catálogo. Esta copia real es la que comprueba los permisos efectivos de respaldo SQL y del sistema de archivos. Una carpeta creada o control consultable por sí solos no garantizan una copia correcta.

### Comprobación de programación antes de instalar un servicio

En otra PowerShell, con la misma cuenta Windows autorizada, desde la raíz:

```powershell
powershell.exe -NoProfile -ExecutionPolicy Bypass -File .\scripts\Iniciar-AgenteDemo.ps1
```

Dejar esa terminal abierta. Como administrador SGTM, guardar un plan diario completo a unos minutos de la hora actual, zona Argentina y carpeta anterior. Actualizar estado hasta ver señal reciente y comprobar que aparece una copia nueva al llegar el horario, incluso cerrando SGTM. El agente consulta cada minuto. `Ctrl+C` o cerrar esa terminal detiene este modo de demostración; no presentarlo como servicio permanente. Para ejecución automática al arrancar Windows sin terminal, publicar e instalar el servicio con `Instalar-AgenteRespaldos.ps1` y su cuenta técnica, según la guía de operación.

Para probar recuperación, seguir «Aceptación manual y recuperación» de esta guía. Primero ensayo sobre base temporal; después, si se decide reemplazar la Demo, doble aprobación y ventana de mantenimiento. No ejecutar una restauración mientras la segunda PC opera. En dos equipos, solo el servidor prepara carpeta/control/agente; todas las aplicaciones deben tener la misma base compartida y coordinación de mantenimiento activada. La configuración local F5 no configura conexiones de una aplicación publicada.
