# Respaldo transportable con Google Drive

No requiere claves API. Google Drive para escritorio sincroniza los paquetes generados por SGTM. SGTM verifica el archivo escrito y su integridad, pero no consulta el estado remoto de Google. Compruebe que Drive finalizó la sincronización y que el paquete aparece en la web antes de considerarlo una copia externa disponible.

## Preparación

En el equipo Windows que ejecuta SQL Server:

1. Instalar Google Drive para escritorio e iniciar sesión. No hace falta sincronizar Documentos, Escritorio ni Descargas.
2. Crear `SGTM-Respaldos` dentro de Mi unidad. La ruta depende del equipo; ejemplo: `G:\My Drive\SGTM-Respaldos`.
3. Preparar los respaldos locales de SGTM según [operación](operacion.md). SQL escribe primero en una carpeta local con permisos, por ejemplo `C:\SGTM\Respaldos`. No usar la unidad virtual de Drive como destino de BACKUP/RESTORE SQL.

## Copiar a Drive

Como administrador de SGTM, crear y verificar una copia local. Seleccionarla en Copias manuales. En **Drive / copia transportable**, pulsar **Exportar copia seleccionada a Drive** y elegir la carpeta SGTM-Respaldos.

Se genera una carpeta única con `sgtm-respaldo.json` y uno o dos `.bak`. Una diferencial incluye su completa base. Las carpetas `.pendiente` no son paquetes terminados. No se borran originales ni paquetes anteriores automáticamente. Revise espacio disponible en la cuenta.

Espere a que Drive indique sincronización completada. La confirmación de SGTM solo acredita la generación e integridad local del paquete.

## Recuperar la misma instalación

1. Descargar desde Drive la carpeta completa del paquete, o hacerla disponible sin conexión. Mantener juntos manifiesto y archivos.
2. En **Recuperar paquete desde Drive**, elegir esa carpeta y una carpeta local con permisos de lectura para SQL Server.
3. SGTM comprueba tamaño, SHA-256, familia, versión, cabeceras SQL, CHECKSUM y dependencia completa/diferencial. Registra la cadena en el catálogo sin reemplazar datos activos.
4. En Restaurar, ensayar la copia seleccionada. El ensayo crea una base temporal y ejecuta CHECKDB.
5. Si corresponde, autorizar la restauración mediante el circuito existente de dos aprobadores y copia protectora. Reiniciar los puestos después.

## Alcance actual

La importación es manual y requiere ejecutar SGTM junto a SQL Server en Windows. La exportación puede hacerse manualmente o desde el agente de sesión configurado a continuación.

Se admiten paquetes de la misma familia de base y versión. La reconstrucción tras perder toda la instalación (base y control) sigue siendo un procedimiento técnico separado; esta pantalla no adopta arbitrariamente la familia de un archivo externo. El paquete no incluye la base auxiliar de control. No se debe presentar esta función como recuperación automática de un equipo perdido.

## Automatización con Drive

En **Programación automática**, indique la carpeta local (por ejemplo `C:\SGTM\Respaldos`), la carpeta de Drive (por ejemplo `G:\My Drive\SGTM-Respaldos`), la hora y el tipo de planificación. Guarde el plan. Dejar Drive vacío programa únicamente copias locales.

Publique el agente en una carpeta estable, fuera del repositorio y de Drive:

```powershell
dotnet publish Taller/Taller.Respaldos -c Release -r win-x64 --self-contained true -o C:\SGTM\AgenteRespaldos
```

Prepare `C:\SGTM\AgenteRespaldos\appsettings.Local.json` con las mismas conexiones Default y Mantenimiento y configuración Respaldos de la instalación preparada. No distribuya este archivo. Instale desde PowerShell con la cuenta Windows que usa Drive:

```powershell
.\scripts\Instalar-AgenteDrive.ps1 -DirectorioPublicacion C:\SGTM\AgenteRespaldos
```

La tarea **SGTM-Respaldos-Drive** inicia el agente ahora y en los siguientes inicios de sesión, sin almacenar contraseñas. Si existe el antiguo servicio SGTMRespaldos, el instalador se detiene para que un administrador lo retire primero. No se ejecuta ninguna restauración durante la instalación.

El equipo debe estar encendido, con Windows y Drive iniciados. SGTM y Visual Studio pueden cerrarse. El horario pendiente se atiende una vez cuando el agente vuelve; no se generan todas las copias de los días perdidos. Si Drive no está disponible, se conserva la copia SQL y se reintenta exportar el mismo respaldo cada 15 minutos. El paquete diferencial incluye su completa base. El agente no borra respaldos.

Comprobar: señal reciente en SGTM, próxima ejecución, resultado y archivo local; luego paquete en Drive y sincronización terminada. “Paquete verificado” certifica los archivos escritos, no la subida a Internet. La auditoría conserva exportaciones y restauraciones con sus aprobadores.

El instalador debe ejecutarse en el mismo Windows que SQL Server. La cuenta necesita permisos SQL de mantenimiento y lectura de los archivos locales; SQL Server necesita escritura en la carpeta local. La unidad virtual de Drive se usa para paquetes, no como destino del comando BACKUP de SQL.
