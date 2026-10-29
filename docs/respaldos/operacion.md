# Respaldos y restauración de SGTM

Para la demostración Windows local, existe una [preparación asistida](../instalacion/respaldos-locales-demo.md#preparación-asistida-de-demo-en-un-solo-equipo-windows) que habilita la configuración sin restaurar ni cargar datos.

## Alcance y decisiones

El administrador dispone de cuatro apartados en **Respaldos y recuperación**: copias manuales, programación automática, restaurar e historial/aprobaciones. La misma pantalla muestra resultado, archivos del servidor y estado del agente. Los controles y eventos están en Designer; los servicios concentran autorización y reglas; Infraestructura ejecuta SQL nativo con conexiones independientes del contexto de negocio.

Se implementan **completas y diferenciales**. Una diferencial contiene los cambios desde su completa base; para restaurarla se necesitan esos dos archivos. No se etiqueta como una cadena incremental de log ni como recuperación a cualquier instante. Los backups de log y recuperación punto en el tiempo quedan para una política DBA específica; no se cambia automáticamente el modelo de recuperación de SQL Server.

Las copias nativas se hacen con CHECKSUM y se someten a RESTORE VERIFYONLY. Esto valida lectura/conjunto/checksums, pero no reemplaza una recuperación real: antes de autorizar una restauración, SGTM recupera la cadena en una **base temporal**, ejecuta **DBCC CHECKDB sin reparar datos** y comprueba el esquema de migraciones.

Las copias están en el **servidor SQL**, no en la PC que abre el formulario. El explorador se habilita solo cuando se confirma el mismo equipo Windows; en otro puesto se indica la ruta del servidor. La carpeta debe existir y permitir escritura a la cuenta del servicio SQL Server. Se usan nombres únicos, un conjunto por archivo y no se sobrescriben/eliminan copias anteriores desde la aplicación. El administrador debe disponer además de almacenamiento externo al equipo y una política de conservación supervisada: guardar todas las copias en el mismo disco no protege contra la pérdida de ese disco.

## Base de control separada

La instalación crea una base adicional, por ejemplo `Taller_Control`, con:

- estado de mantenimiento y generación de sesiones;
- programación, control de versión y señal del agente;
- catálogo de copias y su dependencia de una completa;
- auditoría de operaciones y las dos identidades aprobadoras.

No se coloca esta información dentro de la base de negocio: restaurarla no debe borrar las aprobaciones ni hacer retroceder el calendario. Los documentos de catálogo y configuración se guardan como snapshots JSON tipados y versionados en tablas SQL de control; no son entidades del negocio ni requieren una migración EF en la base recuperable.

Después de cada copia de negocio se intenta una copia auxiliar COPY_ONLY de la base de control y se verifica. Si falla, la copia principal sigue registrada como verificada, pero la interfaz y el historial informan la advertencia; debe resolverse con el técnico. Conserve ambos archivos y las completas de las que dependen las diferenciales. Restaurar la base de control se reserva a personal especializado, no se ofrece como opción ordinaria del formulario.

El historial no guarda contraseñas. Los motivos son obligatorios y se registran las identidades aprobadoras. Es una auditoría operativa protegida por permisos SQL, no un registro inviolable frente a un DBA con privilegios totales.

## Instalación inicial por personal técnico

La funcionalidad está deshabilitada por defecto. No se crea una base auxiliar ni se instala un servicio solo por iniciar una copia actualizada del cliente. Debe coordinarse la actualización de **todos** los clientes que acceden a la base.

1. Actualizar previamente el circuito de las ramas anteriores y verificar un respaldo independiente mediante SQL Server. Detener las versiones anteriores de SGTM durante el despliegue.
2. Revisar con el responsable SQL el servidor, base de negocio, carpeta de copias, espacio para una recuperación temporal y las identidades que van a administrar y programar copias. SQL Server Express puede utilizar el agente externo; SQL Server Agent no es requisito. Para ejecución desatendida se recomienda una instancia de servicio SQL Server/Express, no depender de una LocalDB personal.
3. Publicar el agente en un equipo Windows que esté encendido a la hora prevista:

```powershell
dotnet publish Taller/Taller.Respaldos/Taller.Respaldos.csproj -c Release -r win-x64 --self-contained true -o C:\SGTM\AgenteRespaldos
```

4. Crear su `appsettings.Local.json` protegido, tomando [el ejemplo](appsettings.Local.ejemplo.json). Sustituir servidor/base por los reales; no copiar una cadena ajena ni subir contraseñas a Git. Para instalar puede dejar `Respaldos:Habilitado` en false. La conexión `Default` identifica siempre la base de negocio; `Mantenimiento` puede usar una identidad SQL con los permisos técnicos requeridos, pero debe usar el mismo nombre de servidor. Alternativamente se leen `SGTM_CONN` y `SGTM_MANTENIMIENTO_CONN`; si se usan en un servicio deben pertenecer a su entorno, no solamente a la terminal interactiva.
5. Desde la publicación, con una identidad SQL autorizada para crear la base de control:

```powershell
C:\SGTM\AgenteRespaldos\Taller.Respaldos.exe --instalar-control
```

El instalador es explícito e idempotente. Si ya existe la base de control, verifica que pertenezca a la base de negocio configurada; no la reasigna a otra. No ejecutar el instalador durante una restauración.

6. Configurar los permisos SQL antes de habilitar:

| Identidad | Permisos necesarios |
| --- | --- |
| Clientes de negocio | Sus permisos habituales sobre SGTM; CONNECT a control, SELECT de `dbo.EstadoSGTM` y uso del bloqueo de aplicación `sys.sp_getapplock` como miembro de public. No requieren modificar el catálogo ni la programación. |
| Agente | Leer usuarios/roles/migraciones y metadatos de la base de negocio, BACKUP de negocio/control y leer/escribir las tablas de control. No necesita ejecutar restauraciones para su tarea diaria. |
| Estación administrativa para recuperar | Además de acceso al catálogo y BACKUP: crear/eliminar la base temporal, RESTORE/ALTER de la base de negocio y consultar sesiones/estado del servidor. Son permisos técnicos potentes: otorgarlos solamente a las identidades y equipos previstos; no conceder sysadmin a todos los operadores para evitar configurar permisos. |
| Servicio SQL Server | Escritura/lectura sobre la carpeta de copias y ubicación de datos/log; el usuario que abre SGTM no sustituye estos permisos de archivos. |

El DBA debe crear los usuarios de control que correspondan a los logins reales. Por ejemplo, en la base auxiliar puede crear un rol de lectura y concederle SELECT solo sobre `dbo.EstadoSGTM`, agregar los usuarios de las estaciones a ese rol y verificar el uso de `sp_getapplock`. La cuenta administrativa y el agente requieren acceso adicional a las tablas del módulo. Revisar los requisitos de permisos nativos según versión de SQL Server; en SQL Server 2022 algunas vistas de sesiones requieren VIEW SERVER PERFORMANCE STATE.

7. Configurar `Respaldos:Habilitado=true` y el mismo `Respaldos:BaseControl` en **todos los clientes** y en el agente. Reiniciar clientes. El interceptor de conexiones debe estar activo en todos ellos antes de utilizar restauración; las versiones antiguas no participan del protocolo. Una base de control inaccesible bloquea las operaciones, no se ignora silenciosamente.
8. Para el servicio de Windows, usar una cuenta técnica con acceso SQL y derecho **Iniciar sesión como servicio**, y proteger la carpeta de publicación/configuración mediante permisos de Windows. En PowerShell elevado:

```powershell
.\scripts\Instalar-AgenteRespaldos.ps1 -DirectorioPublicacion C:\SGTM\AgenteRespaldos
```

El script solicita una credencial de Windows mediante el mecanismo seguro de PowerShell; no la guarda en un archivo de texto ni la imprime. No reemplaza ni detiene un servicio existente. La contraseña de una cuenta de Windows usada por el servicio es distinta de los usuarios internos de SGTM. Revisar SQL/Windows al cambiar esa contraseña.

9. Desde SGTM como administrador, crear una completa manual, revisar el archivo verificado y el historial, configurar el plan y comprobar **señal reciente del agente** y una ejecución real. Cerrar la interfaz y comprobar que el servicio continúa ejecutándolo. El mero estado Running del servicio no confirma que sus conexiones/permisos funcionen.

No se debe ejecutar SGTM contra una base de producción para probar esta instalación por primera vez. Hacer el recorrido sobre una copia y validar la recuperación con el responsable técnico.

## Uso cotidiano

### Copias manuales

Ingresar carpeta del servidor, elegir **Completa** o **Diferencial**, y presionar **Crear y verificar copia**. La copia completa normal establece una nueva base diferencial. Una diferencial se rechaza si falta la completa registrada o si otra herramienta cambió la base diferencial; se debe crear una completa nueva. Los trabajos de negocio pueden continuar durante la copia.

Se conserva ruta, identidad del conjunto, familia de la base, esquema, fecha del servidor, solicitante y relación con la completa. Un archivo cambiado, una copia sin checksum, varios conjuntos en un dispositivo o una copia de otra familia no se acepta para restaurar mediante este módulo.

### Programación

El plan corre una vez por día, a una hora y zona explícitas. Admite completa diaria o completa semanal con diferenciales diarias. El día semanal y los horarios se muestran separados. La zona Argentina equivale a America/Argentina/Buenos_Aires; UTC también se puede seleccionar.

El agente revisa cada minuto, informa su señal, ejecuta una sola vez un horario pendiente y calcula la próxima fecha futura; no dispara una cascada de todos los horarios perdidos. Si falta la base, cambió externamente o la completa tiene siete días o más, genera una completa en lugar de una diferencial. Los fallos dejan resultado e historial y reintentan a los quince minutos. Se revalida que el administrador que autorizó el plan siga activo y con su rol; de lo contrario otro administrador debe revisar/guardar el plan.

Un bloqueo SQL evita dos ejecuciones concurrentes entre agentes, copias manuales y restauraciones. No instalar varios agentes por decisión de usuarios individuales; aunque se coordinen, aumenta complejidad y tráfico sin necesidad. Sin señal reciente, la pantalla advierte que no puede garantizar copias automáticas. Un corte eléctrico, SQL detenido, permisos insuficientes o disco lleno pueden impedirlas.

Los horarios inexistentes por un salto de cambio horario se omiten para ese día y los ambiguos se convierten al instante estándar de la zona; no se promete ejecución dos veces en la misma hora local. Se deben mantener sincronizados los relojes de los equipos.

## Restauración planificada

1. Coordinar una ventana de mantenimiento y acompañamiento del personal técnico. Informar que se reemplaza la **base completa**, no registros aislados. Se retroceden atenciones, tareas, cobros, usuarios y contraseñas al punto recuperado. Evaluar antes si el problema se resuelve con una corrección de datos ordinaria; un pago incorrecto no justifica por sí solo restaurar toda la base.
2. Seleccionar una copia registrada. Si es diferencial, el sistema incorpora su completa base. Se requieren los archivos de ambas.
3. **Ensayar restauración:** verificar identidad/checksum, recuperar en una base temporal con archivos nuevos, ejecutar CHECKDB y comparar el esquema. No se modifica la base operativa. Se rechazan esquemas de otra versión; las migraciones posteriores deben tratarse por una recuperación técnica en otra base.
4. El plan ensayado dura diez minutos y pertenece al administrador solicitante; el formulario no puede sustituir sus rutas ni su copia. Cambiar la selección obliga a ensayar otra vez.
5. **Autorizar y restaurar:** el administrador de la sesión vuelve a ingresar su contraseña. Otro usuario distinto ingresa sus credenciales: otro administrador o un administrativo. Deben estar activos, con rol válido. Se exige motivo y reconocer las consecuencias y la coordinación de la ventana. No basta con escribir dos nombres o usar dos veces la misma cuenta. No se cambia la sesión al autenticar al segundo aprobador.
6. Registrar las aprobaciones fuera de la base y entrar en mantenimiento durable. Se bloquean nuevas conexiones SGTM y se espera hasta treinta segundos a que terminen las operaciones existentes. No se usa ROLLBACK IMMEDIATE sobre la base de negocio. Si no terminan, se cancela sin reemplazar datos. Las conexiones externas abiertas también hacen rechazar la operación.
7. Revalidar las identidades, las credenciales y el esquema después de obtener exclusividad. Crear/verificar una **completa COPY_ONLY protectora**, sin alterar la cadena diferencial, y registrar su ruta antes de modificar la base.
8. Pasar a SINGLE_USER sin espera forzada, aplicar la completa y su diferencial si corresponde, comprobar integridad/esquema y volver a MULTI_USER. No se reparan errores con pérdida de datos automática.
9. Confirmar auditoría y salida de mantenimiento en una transacción de control, con identificador idempotente: un reintento de confirmación no duplica el evento ni la generación de sesiones. **Cerrar y reiniciar todas las sesiones.** Las credenciales son las existentes en el punto recuperado.

Con la coordinación habilitada, las conexiones de negocio no usan pooling: al terminar la operación se cierra también la sesión física. Esto evita que sesiones dormidas del pool impidan el mantenimiento sin tener que desconectarlas a la fuerza.

Los bloqueos compartidos de los clientes duran toda la conexión/operación de negocio; el exclusivo de restauración evita que se reemplace mientras hay una transacción activa. El protocolo protege clientes de esta versión. No sustituye el aislamiento administrativo de herramientas externas, versiones anteriores o un DBA que tenga permisos para saltarse el flujo; se deben cerrar/coordinar esas herramientas.

La restauración completa requiere interrupción temporal del trabajo. SGTM no ofrece recuperación en caliente sin ventana, alta disponibilidad, reemplazo parcial de tablas, FILESTREAM, archivos de otra estructura, ni cadenas de log. Las copias de otra versión y recuperación tras un fallo total de SQL/control deben tratarse con personal especializado.

## Fallos y asistencia técnica

Si falla antes del reemplazo, se conserva la base y se retira el mantenimiento. Si falla después de comenzar a reemplazar, el sistema intenta recuperar la copia protectora y verificarla. Si lo logra, informa que la restauración solicitada falló y que se recuperó el estado anterior; igualmente exige reiniciar sesiones.

Si no puede recuperar/verificar la protectora, **el estado durable mantiene bloqueado SGTM** e informa su ruta. No se intenta seguir operando con una base parcialmente restaurada. Si los datos ya fueron recuperados y verificados pero se pierde la confirmación del control, no se vuelve a sobrescribir la base a ciegas: se reintenta la confirmación idempotente y, si no se puede confirmar, se requiere revisión técnica.

Un cierre forzado, caída del servidor o pérdida eléctrica durante el reemplazo puede dejar la base en recuperación y el mantenimiento activo. No hay un botón ordinario para quitar el bloqueo ni un desbloqueo por tiempo. Conservar los archivos y solicitar al desarrollador/DBA que revise la base, CHECKDB, esquema, estado de operación y copia protectora. El sistema no puede garantizar disponibilidad ante falla de hardware o pérdida de todos los archivos.

Para diagnóstico técnico, sin abrir la base de negocio ni saltear sus validaciones, la publicación del agente permite:

```powershell
C:\SGTM\AgenteRespaldos\Taller.Respaldos.exe --estado-control
```

Requiere la configuración y permisos SQL técnicos; muestra estado/catálogo/auditoría sin contraseñas. Si la base de negocio no permite autenticar usuarios o SGTM no puede iniciar, no se simula una doble autorización contra datos inexistentes: la recuperación queda a cargo del personal especializado mediante sus herramientas SQL.

Las copias auxiliares de control también deben conservarse fuera del servidor. Si se pierden control y catálogos, la recuperación técnica puede usar los backups nativos disponibles; la importación automática de archivos externos no es parte de la interfaz operativa de esta versión.

## Validación

La evidencia de la versión de entrega está en [tercera revisión](../pruebas/revision-integral-3.md). No extrapolar resultados de una versión anterior a esta instalación.

- Pruebas de aplicación: administrador activo, dos identidades distintas, roles admitidos, claves erróneas y revocadas, plan ligado al solicitante/expiración/un solo uso, motivo, reconocimiento, rutas SQL seguras y calendario con zona.
- Pruebas con SQL Server aislado en GitHub Actions: completa/diferencial, ensayo temporal y CHECKDB, recuperación real, copia protectora, auditoría externa, conexiones activas, generaciones de sesión, archivo reemplazado, esquema distinto y ejecución del agente.
- Las pruebas SQL solo se habilitan con `SGTM_SQL_PRUEBAS` a una instancia aislada (catálogo master), `SGTM_PERMITIR_PRUEBAS_SQL=1` y carpeta del servidor. Crean/eliminan exclusivamente bases con prefijo de prueba y GUID. Nunca proporcionar la conexión de producción. En ejecución local sin esa instancia se omiten explícitamente.
- Pendiente inspección manual en Windows de las pestañas, credenciales y progreso, 1366×768/1920×1080, DPI 100/125/150 %, SQL Server/Express y las identidades reales del despliegue. Probar primero en una copia de la base.

## Referencias técnicas

- [Copias diferenciales](https://learn.microsoft.com/en-us/sql/relational-databases/backup-restore/differential-backups-sql-server).
- [COPY_ONLY](https://learn.microsoft.com/en-us/sql/relational-databases/backup-restore/copy-only-backups-sql-server).
- [RESTORE VERIFYONLY](https://learn.microsoft.com/en-us/sql/t-sql/statements/restore-statements-verifyonly-transact-sql).
- [DBCC CHECKDB](https://learn.microsoft.com/en-us/sql/t-sql/database-console-commands/dbcc-checkdb-transact-sql).
- [RESTORE y efecto de REPLACE](https://learn.microsoft.com/en-us/sql/t-sql/statements/restore-statements-transact-sql).
- [BackgroundService como servicio de Windows](https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service).

### Selección visual de carpeta

Cuando SGTM se abre en el mismo equipo Windows que SQL Server, aparece **Elegir carpeta…** junto a la ruta de las copias manuales y de la programación. Abre el explorador de Windows, permite seleccionar una carpeta interna, externa o accesible en red y completa el campo. Cancelar conserva la ruta anterior; no se inicia ninguna copia ni se guarda el plan hasta confirmar la operación correspondiente. La ruta sigue siendo editable.

El sistema consulta el nombre y plataforma del servidor SQL real para evitar confundir el disco del cliente con el del servidor. En equipos remotos, Linux o entornos donde no se puede confirmar que comparten el sistema de archivos, se mantiene la entrada de la ruta del servidor sin explorador local. Para usar el selector, abrir SGTM en el servidor Windows. Elegir o crear una carpeta no concede permisos a la cuenta del servicio SQL; verificar acceso antes de usarla.
