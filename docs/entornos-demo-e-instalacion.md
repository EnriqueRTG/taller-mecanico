# Dos entornos de negocio: instalación y demostración

## Bases previstas

- SGTM_DB: instalación del taller, con datos reales. Migraciones, catálogos iniciales y administrador inicial; no ejecutar el cargador académico.
- SGTM_DB_Demo: un solo lote académico coherente para pruebas y coloquio. El programa de demostración exige sufijo _Demo y base sin usuarios/clientes/vehículos/atenciones. No mezcla ni reemplaza lotes.

Los nombres SGTM_Pruebas_* son bases temporales de CI, no instalaciones que deba mantener el usuario. Al configurar respaldos existe además una base técnica de control separada del negocio: conserva programación, aprobaciones y bitácora, para no perder esa información al restaurar SGTM_DB. No constituye otro lote comercial. Cada entorno debe tener su control/configuración independiente para no apuntar a la base incorrecta.

## Preparar una base nueva (cambio de esquema)

Esta versión elimina la cadena de migraciones anterior y la reemplaza por una única migración inicial. **No ejecutar Update-Database sobre una base creada por versiones anteriores ni restaurar un respaldo de esa versión para usarlo con este código.** Se retiraron columnas, clasificaciones y rutas antiguas por decisión de alcance del proyecto.

1. Guardar y actualizar el código. Cerrar aplicación y agente de respaldos del entorno que va a reemplazarse.
2. En SSMS confirmar la instancia y base Demo. Puede crear una base con nombre nuevo terminado en _Demo (por ejemplo SGTM_CircuitoActual_Demo); es la alternativa directa para no confundirla con la anterior. No necesita borrar la anterior para preparar la nueva.
3. Si decide reutilizar el mismo nombre, eliminar la base anterior desde SSMS solo después de verificar el destino. Sus registros y copias anteriores no forman parte del lote nuevo. No ejecutar scripts de borrado sobre SGTM_DB por confusión.
4. Configurar la conexión a esa base vacía y ejecutar el cargador siguiente. Aplica el esquema actual y construye el lote completo. No ejecutar primero la aplicación: esta crearía el administrador y el cargador exige una base sin usuarios.

Después, desde la raíz del repositorio, en PowerShell:

```powershell
$env:SGTM_CONN = 'Server=localhost\SQLEXPRESS;Database=SGTM_DB_Demo;Integrated Security=True;Encrypt=True;TrustServerCertificate=True'
$claveDemo = Read-Host 'Clave de demostración' -AsSecureString
$env:SGTM_DEMO_PASSWORD = [System.Net.NetworkCredential]::new('', $claveDemo).Password
dotnet run --project Taller/Taller.Demostracion -- --cargar-demo
Remove-Item Env:SGTM_DEMO_PASSWORD
```

Exige clave de 12 a 100 caracteres, mayúscula, minúscula, número y símbolo. Retirar SGTM_ADMIN_INICIAL_PASSWORD si quedó de una instalación anterior para que el lote use la clave elegida. No registrar contraseñas en Git. Las migraciones se aplican antes del lote; si falla la transacción comercial, no continuar con supuestos de carga completa. Una segunda ejecución rechaza la mezcla.

Visual Studio debe usar SGTM_CONN de su propio perfil/proceso o ConnectionStrings:Default en appsettings.Local.json; cambiar una variable de otra terminal no cambia un Visual Studio ya abierto. Dos puestos deben usar la misma instancia SQL y la misma base, no localhost de cada equipo por separado. No activar respaldos hasta verificar configuración de mantenimiento y control del entorno seleccionado.

## Instalación del taller sin demostración

1. Publicar la aplicación para Windows con el runtime correspondiente al proyecto o autocontenida; preparar SQL Server y su identidad de acceso.
2. Configurar SGTM_CONN o ConnectionStrings:Default para SGTM_DB. No distribuir las credenciales del desarrollador. Configurar nombre/contacto/logo del taller.
3. Aplicar la migración inicial de Infraestructura sobre el destino nuevo confirmado, mediante el procedimiento de instalación. La base de destino debe estar vacía y recibir EsquemaInicialCircuitoActual.
4. Suministrar SGTM_ADMIN_INICIAL_PASSWORD de forma local y temporal para crear el administrador al primer inicio; retirarla después. Crear usuarios operativos con sus roles y credenciales.
5. Confirmar conexión, catálogos, permisos y un recorrido de aceptación en el entorno de prueba antes de operar datos reales.
6. Configurar el agente de respaldos y su base de control con permisos separados. Probar recuperación en una copia antes de habilitarla como procedimiento operativo.

## Azure: siguiente etapa

El sistema de negocio sigue conectado a SQL Server. Los archivos de respaldo se almacenan en Azure Blob Storage privado mediante Respaldos:Azure:ContenedorUri e identidad Microsoft Entra/DefaultAzureCredential; no requiere trasladar automáticamente la base a Azure SQL ni publicar una contraseña de almacenamiento en la cadena SGTM_CONN. Se configurará la cuenta/contenedor, identidad y permisos con el usuario. No se crean recursos Azure ni se incurre en gastos por cargar este lote.

## Verificación antes de F5

El cargador informa DEMO v3 completo: 19 clientes (3 inactivos), 48 vehículos (2 inactivos) y 48 atenciones. El catálogo contiene 157 conceptos de oferta más uno inactivo para filtros. Cada precio y unidad se define expresamente, sin precios genéricos heredados del catálogo mínimo.

En SSMS, sobre la base elegida:

```sql
SELECT MigrationId FROM dbo.__EFMigrationsHistory;
SELECT Estado, COUNT(*) AS Cantidad FROM dbo.Atenciones GROUP BY Estado;
SELECT COUNT(*) AS Conceptos FROM dbo.Conceptos;
```

Debe existir únicamente la migración terminada en _EsquemaInicialCircuitoActual y siete estados admitidos por las restricciones. No necesita crear otra migración en su PC. Al abrir Visual Studio configurar la misma base del cargador, reconstruir y ejecutar. La producción vacía lleva solo inicialización mínima y cuentas reales, no el lote académico.

Los respaldos anteriores no corresponden al esquema actual. El registro y la base técnica de respaldos se configurarán para este entorno nuevo al preparar Azure. La conexión a SQL Server y la identidad/contenedor de Azure son configuraciones distintas.
