# Seguridad e instalación

## Qué protege la aplicación

- Los servicios revalidan el usuario y su rol en persistencia antes de cada operación protegida. La visibilidad del menú no constituye autorización.
- Las ediciones y transiciones del circuito conservan los tokens rowversion. Los cobros conservan su identificador de intento para no duplicarlos al reintentar.
- Contraseñas mediante PasswordHasher de Microsoft Identity; no se almacenan en texto ni se incluyen en la consulta de auditoría.
- Cinco rechazos de credenciales por identidad normalizada dentro de 15 minutos bloquean nuevos intentos durante 15 minutos. El contador es SQL compartido entre estaciones. No se informa si la cuenta existe.
- Auditoría de autenticaciones correctas/rechazadas en `EventosAcceso`, consultable exclusivamente por administrador (máximo 500 resultados por período). Los nombres no reconocidos no se guardan en claro; la clave del contador es SHA-256. El registro refleja intentos procesados; solicitudes ya bloqueadas o sintácticamente inválidas no generan nuevos eventos.
- Sesión sin vencimiento temporal: se conserva hasta cerrar sesión o salir de la aplicación. Los servicios revalidan usuario habilitado y rol persistido antes de autorizar operaciones. Al dejar el puesto sin supervisión, bloquear Windows o cerrar sesión. Las aprobaciones y la vigencia de planes de restauración mantienen sus verificaciones independientes.
- Restauraciones con ensayo separado, comprobaciones de integridad, doble aprobación y coordinación SQL ya existentes. Credenciales de nube mediante identidad Entra, sin SAS ni claves en el catálogo de copias.

## Primera instalación

Sobre una base nueva, aplicar EsquemaInicialCircuitoActual según [entornos](entornos.md). Incluye las tablas de seguridad; no se aplican migraciones retiradas sobre una base antigua. Para Demo, el cargador aplica el esquema antes de crear los usuarios.

Una base sin usuarios necesita una credencial inicial suministrada por el instalador. En PowerShell, solicítela sin escribirla literalmente en el historial:

```powershell
$claveSegura = Read-Host 'Contraseña inicial del administrador' -AsSecureString
$env:SGTM_ADMIN_INICIAL_PASSWORD = [System.Net.NetworkCredential]::new('', $claveSegura).Password
# Ejecute la aplicación desde esta misma terminal, con SGTM_CONN configurada.
dotnet run --project Taller/Taller.Presentacion
Remove-Item Env:SGTM_ADMIN_INICIAL_PASSWORD
```

Mínimo 12 caracteres con mayúsculas, minúsculas, números y símbolo; máximo 100. El nombre inicial es `admin`. Nunca se recrea automáticamente una cuenta deshabilitada ni se recupera un acceso cambiando contraseñas silenciosamente. Si ya hay usuarios y ningún administrador activo, debe intervenir el responsable técnico.

Cambiar credenciales temporales desde Usuarios / Credenciales antes de usar datos reales. No publicar contraseñas en una guía ni en Git.

## Límites y configuración del equipo

Esta es una aplicación de escritorio que se conecta directamente a SQL Server: quien controle la PC, el proceso o credenciales SQL con permisos de escritura puede eludir las reglas del cliente. No es una API con un servidor de autorización independiente. La auditoría comercial y de accesos no es inviolable frente a un administrador de base de datos. Para un despliegue con estaciones no confiables se requiere una API intermedia y credenciales de base exclusivamente en el servidor.

- No otorgar `sysadmin` a todos los operadores. Separar la identidad de negocio de la de mantenimiento/restauración; conceder solo los permisos necesarios y proteger el servicio de respaldos.
- Restringir red/firewall de SQL, cuentas de Windows, carpeta de respaldos y carpeta de registros. En producción instalar un certificado válido de SQL y usar `Encrypt=True;TrustServerCertificate=False`. `TrustServerCertificate=True` queda para la prueba local con certificado no confiable.
- Aplicar bloqueo de pantalla de Windows. La sesión persistente requiere proteger físicamente la estación.
- Mantener sistema operativo, SQL Server y dependencias actualizados. Revisar accesos rechazados y guardar respaldos fuera del equipo.
- La limitación por nombre reduce intentos de adivinación; no reemplaza controles de red contra ataques masivos o distribuidos y puede bloquear temporalmente una cuenta atacada.
- Los registros técnicos pueden contener información operativa en excepciones SQL. No se deben publicar. La presentación no muestra excepciones técnicas completas al iniciar.
- Proteger con permisos SQL `ControlesAcceso` y `EventosAcceso`; definir con el responsable técnico una retención acorde al uso y a los requisitos de datos personales. No se elimina auditoría automáticamente.

## Verificación

Hay pruebas de sesión sin vencimiento temporal y revocación de autorización, instalación sin contraseña fija, validación del destino Azure y pruebas SQL del contador persistente y lote completo. La autorización, integridad del circuito y concurrencia cuentan además con las pruebas de integración existentes. La cuenta Azure real y la aceptación visual en Windows deben probarse en la instalación; compilar no demuestra seguridad absoluta.

## Política común de contraseñas

Alta y gestión de credenciales utilizan `UsuarioServicio.ValidarPassword`: contraseña no vacía, de 8 a 100 caracteres, conservada exactamente como fue ingresada. No se exige una composición específica. Los dos formularios usan los mismos límites y generador; las contraseñas se almacenan mediante hash. Modificar únicamente el nombre de acceso no obliga a reemplazar la contraseña.
