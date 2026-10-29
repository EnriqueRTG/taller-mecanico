# Dos entornos de negocio: instalación y demostración

## Bases previstas

- SGTM_DB: instalación del taller, con datos reales. Migraciones, catálogos iniciales y administrador inicial; no ejecutar el cargador académico.
- SGTM_DB_Demo: un solo lote académico coherente para pruebas y coloquio. El programa de demostración exige sufijo _Demo y base sin usuarios/clientes/vehículos/atenciones. No mezcla ni reemplaza lotes.

Los nombres SGTM_Pruebas_* son bases temporales de CI, no instalaciones que deba mantener el usuario. Al configurar respaldos existe además una base técnica de control separada del negocio: conserva programación, aprobaciones y bitácora, para no perder esa información al restaurar SGTM_DB. No constituye otro lote comercial. Cada entorno debe tener su control/configuración independiente para no apuntar a la base incorrecta.

## Preparar una base nueva (cambio de esquema)

El esquema vigente contiene seis migraciones: EsquemaInicialCircuitoActual, SimplificarEjecucionTresEstados, RetirarVinculoCambioSinUso, ExigirTelefonoContactoCliente, AgregarConcurrenciaUsuarios y ExigirContactoDecisionPresupuesto. Ver [actualización](actualizacion-coloquio.md) antes de cambiar una base existente. **Una base de una línea de migraciones retirada requiere conversión técnica; no aplicar estas migraciones a ciegas. Una base de la línea actual se actualiza sin recrearla según la guía.** Se retiraron columnas, clasificaciones y rutas antiguas por decisión de alcance del proyecto.

1. Guardar y actualizar el código. Cerrar aplicación y agente de respaldos del entorno que va a reemplazarse.
2. En SSMS confirmar la instancia y base Demo. Para los equipos de demostración se usa SGTM_DB_Demo. Si aún contiene el esquema anterior y se decidió descartar ese lote, eliminar únicamente esa base desde SSMS, confirmando el destino. El cargador la recrea; no hace falta inventar otro nombre.
3. Configurar la conexión a esa base vacía y ejecutar el cargador siguiente. Aplica el esquema actual y construye el lote completo. No ejecutar primero la aplicación: esta crearía el administrador y el cargador exige una base sin usuarios.

Después, desde la raíz del repositorio, en PowerShell:

```powershell
$env:SGTM_CONN = 'Server=localhost\SQLEXPRESS;Database=SGTM_DB_Demo;Integrated Security=True;Encrypt=True;TrustServerCertificate=True'
Remove-Item Env:SGTM_ADMIN_INICIAL_PASSWORD -ErrorAction SilentlyContinue
$claveDemo = Read-Host 'Clave de demostración' -AsSecureString
$env:SGTM_DEMO_PASSWORD = [System.Net.NetworkCredential]::new('', $claveDemo).Password
dotnet run --project Taller/Taller.Demostracion -- --cargar-demo
Remove-Item Env:SGTM_DEMO_PASSWORD
```

Exige clave de 12 a 100 caracteres, mayúscula, minúscula, número y símbolo. Retirar SGTM_ADMIN_INICIAL_PASSWORD si quedó de una instalación anterior para que el lote use la clave elegida. No registrar contraseñas en Git. Las migraciones se aplican antes del lote; si falla la transacción comercial, no continuar con supuestos de carga completa. Una segunda ejecución rechaza la mezcla.

Visual Studio debe usar SGTM_CONN de su propio perfil/proceso o ConnectionStrings:Default en appsettings.Local.json; cambiar una variable de otra terminal no cambia un Visual Studio ya abierto. Para copias independientes del coloquio, cada equipo carga su SGTM_DB_Demo local con el mismo main. Si se desea trabajar simultáneamente sobre una sola atención, ambos puestos deben apuntar a una instancia/base compartida; localhost de cada equipo no sincroniza datos. No activar respaldos hasta verificar configuración de mantenimiento y control del entorno seleccionado.

## Instalación del taller sin demostración

1. Publicar la aplicación para Windows con el runtime correspondiente al proyecto o autocontenida; preparar SQL Server y su identidad de acceso.
2. Configurar SGTM_CONN o ConnectionStrings:Default para SGTM_DB. No distribuir las credenciales del desarrollador. Configurar nombre/contacto/logo del taller.
3. Aplicar todas las migraciones de Infraestructura sobre el destino nuevo confirmado mediante `dotnet ef database update --project Taller/Taller.Infraestructura --startup-project Taller/Taller.Infraestructura`. La base vacía debe recibir las seis migraciones, incluida ExigirContactoDecisionPresupuesto.
4. Suministrar SGTM_ADMIN_INICIAL_PASSWORD de forma local y temporal para crear el administrador al primer inicio; retirarla después. Crear usuarios operativos con sus roles y credenciales.
5. Confirmar conexión, catálogos, permisos y un recorrido de aceptación en el entorno de prueba antes de operar datos reales.
6. Configurar el agente de respaldos y su base de control con permisos separados. Probar recuperación en una copia antes de habilitarla como procedimiento operativo.

## Azure: siguiente etapa

El sistema de negocio sigue conectado a SQL Server. Los archivos de respaldo se almacenan en Azure Blob Storage privado mediante Respaldos:Azure:ContenedorUri e identidad Microsoft Entra/DefaultAzureCredential; no requiere trasladar automáticamente la base a Azure SQL ni publicar una contraseña de almacenamiento en la cadena SGTM_CONN. Se configurará la cuenta/contenedor, identidad y permisos con el usuario. No se crean recursos Azure ni se incurre en gastos por cargar este lote.

## Verificación antes de F5

El cargador informa DEMO v4 completo: 19 clientes (3 inactivos), 48 vehículos (2 inactivos) y 48 atenciones. El catálogo contiene 157 conceptos de oferta más uno inactivo para filtros. Cada precio y unidad se define expresamente, sin precios genéricos heredados del catálogo mínimo.

En SSMS, sobre la base elegida:

```sql
SELECT MigrationId FROM dbo.__EFMigrationsHistory;
SELECT Estado, COUNT(*) AS Cantidad FROM dbo.Atenciones GROUP BY Estado;
SELECT COUNT(*) AS Conceptos FROM dbo.Conceptos;
```

Deben existir las seis migraciones indicadas al principio, hasta ExigirContactoDecisionPresupuesto, y siete estados de atención admitidos por las restricciones. No debe generar migraciones nuevas en su PC: aplicar las existentes. Al abrir Visual Studio configurar la misma base del cargador, reconstruir y ejecutar. La producción vacía lleva solo inicialización mínima y cuentas reales, no el lote académico.

Los respaldos anteriores no corresponden al esquema actual. El registro y la base técnica de respaldos se configurarán para este entorno nuevo al preparar Azure. La conexión a SQL Server y la identidad/contenedor de Azure son configuraciones distintas.
