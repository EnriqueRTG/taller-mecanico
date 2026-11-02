[CmdletBinding()]
param(
    [string]$ServidorSql = 'localhost\SQLEXPRESS',
    [string]$DirectorioPublicacion = 'C:\SGTM\AgenteRespaldos'
)
$ErrorActionPreference = 'Stop'
# The executable gives environment overrides priority over JSON; never silently target another database.
foreach ($variable in @('SGTM_CONN', 'SGTM_MANTENIMIENTO_CONN', 'ConnectionStrings__Default', 'ConnectionStrings__Mantenimiento', 'Respaldos__BaseControl', 'Respaldos__Habilitado')) {
    foreach ($ambito in @('Process', 'User', 'Machine')) {
        if (-not [string]::IsNullOrWhiteSpace([Environment]::GetEnvironmentVariable($variable, $ambito))) {
            throw "Existe una configuración de entorno $variable ($ambito). Revise la guía para instalar el agente con su configuración existente; no se publicó ni modificó la Demo."
        }
    }
}
$proyecto = Join-Path $PSScriptRoot '..\Taller\Taller.Respaldos\Taller.Respaldos.csproj'
$configuracion = Join-Path $DirectorioPublicacion 'appsettings.Local.json'
if (Test-Path -LiteralPath $configuracion) { throw 'Ya existe una configuración del agente. Se conserva sin cambios; revise la guía Google Drive para instalar la tarea con esa configuración.' }
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) { throw 'Instale el SDK .NET 10 para publicar el agente.' }
# Only the prepared demo database. Never runs migrations, loads demo records or restores data.
$conexion = 'Server=' + $ServidorSql + ';Database=SGTM_DB_Demo;Integrated Security=True;Encrypt=True;TrustServerCertificate=True'
& dotnet publish $proyecto -c Release -r win-x64 --self-contained true -o $DirectorioPublicacion
if ($LASTEXITCODE -ne 0) { throw 'No se pudo publicar el agente.' }
@{
    ConnectionStrings = @{ Default = $conexion; Mantenimiento = $conexion }
    Respaldos = @{ Habilitado = $true; BaseControl = 'SGTM_DB_Demo_Control' }
} | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath $configuracion -Encoding UTF8
$ejecutable = Join-Path $DirectorioPublicacion 'Taller.Respaldos.exe'
& $ejecutable --verificar-base
if ($LASTEXITCODE -ne 0) { throw 'SQL no pasó la verificación. No se instaló la tarea; revise servidor y permisos Windows.' }
& $ejecutable --estado-control
if ($LASTEXITCODE -ne 0) { throw 'Falta preparar la base de control Demo. No se instaló la tarea.' }
& (Join-Path $PSScriptRoot 'Instalar-AgenteDrive.ps1') -DirectorioPublicacion $DirectorioPublicacion
