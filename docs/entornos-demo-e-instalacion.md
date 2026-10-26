# Dos entornos de negocio: instalación y demostración

## Bases previstas

- SGTM_DB: instalación del taller, con datos reales. Migraciones, catálogos iniciales y administrador inicial; no ejecutar el cargador académico.
- SGTM_DB_Demo: un solo lote académico coherente para pruebas y coloquio. El programa de demostración exige sufijo _Demo y base sin usuarios/clientes/vehículos/atenciones. No mezcla ni reemplaza lotes.

Los nombres SGTM_Pruebas_* son bases temporales de CI, no instalaciones que deba mantener el usuario. Al configurar respaldos existe además una base técnica de control separada del negocio: conserva programación, aprobaciones y bitácora, para no perder esa información al restaurar SGTM_DB. No constituye otro lote comercial. Cada entorno debe tener su control/configuración independiente para no apuntar a la base incorrecta.

## Sustituir el lote anterior sin perder datos

Actualizar el código no cambia el lote que ya está cargado. Primero conservar un respaldo de SGTM_DB_Demo y anotar versión de código/migración. Cerrar las aplicaciones conectadas. Solo cuando se confirme que es una base exclusivamente de prueba y que la copia es utilizable, recrear esa base Demo vacía bajo el procedimiento técnico de SQL Server. El cargador no incluye DROP DATABASE ni TRUNCATE, ni se ejecuta destructivamente al iniciar.

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
3. Aplicar las migraciones de Infraestructura sobre el destino confirmado, mediante el procedimiento de instalación. Este ajuste de catálogo Demo/estados no agrega migraciones.
4. Suministrar SGTM_ADMIN_INICIAL_PASSWORD de forma local y temporal para crear el administrador al primer inicio; retirarla después. Crear usuarios operativos con sus roles y credenciales.
5. Confirmar conexión, catálogos, permisos y un recorrido de aceptación en el entorno de prueba antes de operar datos reales.
6. Configurar el agente de respaldos y su base de control con permisos separados. Probar recuperación en una copia antes de habilitarla como procedimiento operativo.

## Azure: siguiente etapa

El sistema de negocio sigue conectado a SQL Server. Los archivos de respaldo se almacenan en Azure Blob Storage privado mediante Respaldos:Azure:ContenedorUri e identidad Microsoft Entra/DefaultAzureCredential; no requiere trasladar automáticamente la base a Azure SQL ni publicar una contraseña de almacenamiento en la cadena SGTM_CONN. Se configurará la cuenta/contenedor, identidad y permisos con el usuario. No se crean recursos Azure ni se incurre en gastos por cargar este lote.
