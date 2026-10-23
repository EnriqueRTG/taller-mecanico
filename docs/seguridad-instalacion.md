# Seguridad e instalación

## Qué protege la aplicación

- Los servicios revalidan el usuario y su rol en persistencia antes de cada operación protegida. La visibilidad del menú no constituye autorización.
- Las ediciones y transiciones del circuito conservan los tokens rowversion. Los cobros conservan su identificador de intento para no duplicarlos al reintentar.
- Contraseñas mediante PasswordHasher de Microsoft Identity; no se almacenan en texto ni se incluyen en la consulta de auditoría.
- Cinco rechazos de credenciales por identidad normalizada dentro de 15 minutos bloquean nuevos intentos durante 15 minutos. El contador es SQL compartido entre estaciones. No se informa si la cuenta existe.
- Auditoría de autenticaciones correctas/rechazadas en `EventosAcceso`, consultable exclusivamente por administrador (máximo 500 resultados por período). Los nombres no reconocidos no se guardan en claro; la clave del contador es SHA-256. El registro refleja intentos procesados; solicitudes ya bloqueadas o sintácticamente inválidas no generan nuevos eventos.
- Sesión con 30 minutos de inactividad y ocho horas de duración máxima. Teclado, clics y rueda de la aplicación renuevan actividad; los temporizadores y consultas automáticas no la renuevan. Vencida, la presentación bloquea la navegación y las operaciones y solicita cerrar sesión para reingresar. Una operación ya iniciada termina con su tratamiento habitual.
- Restauraciones con ensayo separado, comprobaciones de integridad, doble aprobación y coordinación SQL ya existentes. Credenciales de nube mediante identidad Entra, sin SAS ni claves en el catálogo de copias.

## Primera instalación

Aplique las migraciones existentes antes de abrir la aplicación. La migración `SeguridadAccesoPersistente` agrega dos tablas; no elimina ni cambia los datos comerciales.

Una base sin usuarios necesita una credencial inicial suministrada por el instalador. En PowerShell, solicítela sin escribirla literalmente en el historial:

```powershell
$claveSegura = Read-Host 'Contraseña inicial del administrador' -AsSecureString
$env:SGTM_ADMIN_INICIAL_PASSWORD = [System.Net.NetworkCredential]::new('', $claveSegura).Password
# Ejecute la aplicación desde esta misma terminal, con SGTM_CONN configurada.
dotnet run --project Taller/Taller.Presentacion
Remove-Item Env:SGTM_ADMIN_INICIAL_PASSWORD
```

Mínimo 12 caracteres con mayúsculas, minúsculas, números y símbolo; máximo 100. El nombre inicial es `admin`. Nunca se recrea automáticamente una cuenta deshabilitada ni se recupera un acceso cambiando contraseñas silenciosamente. Si ya hay usuarios y ningún administrador activo, debe intervenir el responsable técnico.

Las cuentas existentes conservan sus contraseñas. Si una instalación antigua aún usa `Admin123!` u otra clave conocida, el administrador debe cambiarla desde Usuarios / Credenciales antes de usar datos reales. No publique sus credenciales en una guía ni en Git.

## Límites y configuración del equipo

Esta es una aplicación de escritorio que se conecta directamente a SQL Server: quien controle la PC, el proceso o credenciales SQL con permisos de escritura puede eludir las reglas del cliente. No es una API con un servidor de autorización independiente. La auditoría comercial y de accesos no es inviolable frente a un administrador de base de datos. Para un despliegue con estaciones no confiables se requiere una API intermedia y credenciales de base exclusivamente en el servidor.

- No otorgar `sysadmin` a todos los operadores. Separar la identidad de negocio de la de mantenimiento/restauración; conceder solo los permisos necesarios y proteger el servicio de respaldos.
- Restringir red/firewall de SQL, cuentas de Windows, carpeta de respaldos y carpeta de registros. En producción instalar un certificado válido de SQL y usar `Encrypt=True;TrustServerCertificate=False`. `TrustServerCertificate=True` queda para la prueba local con certificado no confiable.
- Aplicar bloqueo de pantalla de Windows. El vencimiento de sesión no reemplaza la protección física de la estación.
- Mantener sistema operativo, SQL Server y dependencias actualizados. Revisar accesos rechazados y guardar respaldos fuera del equipo.
- La limitación por nombre reduce intentos de adivinación; no reemplaza controles de red contra ataques masivos o distribuidos y puede bloquear temporalmente una cuenta atacada.
- Los registros técnicos pueden contener información operativa en excepciones SQL. No se deben publicar. La presentación no muestra excepciones técnicas completas al iniciar.
- Proteger con permisos SQL `ControlesAcceso` y `EventosAcceso`; definir con el responsable técnico una retención acorde al uso y a los requisitos de datos personales. No se elimina auditoría automáticamente.

## Verificación

Hay pruebas de expiración de sesiones, instalación sin contraseña fija, validación del destino Azure y pruebas SQL del contador persistente y lote completo. La autorización, integridad del circuito y concurrencia cuentan además con las pruebas de integración existentes. La cuenta Azure real y la aceptación visual en Windows deben probarse en la instalación; compilar no demuestra seguridad absoluta.
