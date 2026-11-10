# Scripts y recursos de SGTM

Ejecutar desde la raíz del repositorio en Windows PowerShell. Para compilar o publicar, instalar el SDK de `global.json`. Los ejemplos usan SQL Server Express local con autenticación integrada; ajustar la instancia según el equipo. Ver [instructivo de la Demo](recursos/replicar-demo.txt) para la secuencia completa.

| Script | Cuándo usarlo | Qué cambia |
|---|---|---|
| [Preparar-BaseSGTM.ps1](instalacion/Preparar-BaseSGTM.ps1) | Preparar una instalación desde los archivos publicados. El lanzador lo invoca en la instalación inicial. | Aplica esquema y catálogo de [BaseDatos](instalacion/BaseDatos/) en una base nueva y genera la configuración local. |
| [Cargar-Demostracion.ps1](Cargar-Demostracion.ps1) | Preparar el lote para una base cuyo nombre termina en `_Demo`. Requiere permisos SQL y solicita la contraseña de las cuentas Demo. | Ejecuta el cargador del proyecto. Ejemplo: `.\scripts\Cargar-Demostracion.ps1 -Servidor 'localhost\SQLEXPRESS'`. |
| [Preparar-RespaldosDemo.ps1](Preparar-RespaldosDemo.ps1) | En el servidor de la Demo, con SGTM cerrado y PowerShell como administrador. | Crea o prepara la carpeta y la base de control, ajusta permisos del servicio SQL y configura los perfiles locales. No restaura datos. |
| [Iniciar-AgenteDemo.ps1](Iniciar-AgenteDemo.ps1) | Prueba opcional de programación, después de preparar respaldos. | Ejecuta el agente en primer plano; Ctrl+C lo detiene. Las copias dependen del plan guardado en SGTM. |
| [Preparar-AgenteDriveDemo.ps1](Preparar-AgenteDriveDemo.ps1) | Prueba opcional con Drive para escritorio, en la cuenta Windows que sincroniza. Requiere respaldos Demo preparados. | Publica y configura el agente Demo y registra su tarea de sesión. No aplica migraciones ni restaura datos. |
| [Instalar-AgenteDrive.ps1](Instalar-AgenteDrive.ps1) | Instalación manual del agente de sesión a partir de un directorio publicado y configurado. | Registra e inicia `SGTM-Respaldos-Drive` para el usuario actual. |
| [Instalar-AgenteRespaldos.ps1](Instalar-AgenteRespaldos.ps1) | Instalación como servicio Windows, con elevación y cuenta técnica con permisos SQL, de carpeta y de inicio como servicio. | Crea e inicia `SGTMRespaldos`. Usar esta modalidad o la tarea de sesión, según la instalación. |

Para la entrega basta con el respaldo manual y el ensayo de recuperación: el agente es opcional. Si se configura programación, verificar señal reciente, historial y archivo generado; si se usa Drive, comprobar también la sincronización.

En [recursos](recursos/) están el instructivo de réplica, el ejemplo JSON sin credenciales y el SQL de contraste de reportes. El ejemplo JSON se adapta localmente; no versionar la configuración real. Ejecutar el SQL de contraste en la base que se quiere verificar. Para permisos y procedimiento completo, consultar la [referencia técnica](../REFERENCIA-TECNICA.md#respaldos-operacion).
