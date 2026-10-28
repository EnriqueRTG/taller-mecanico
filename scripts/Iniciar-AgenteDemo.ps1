#Requires -Version 5.1
<# Inicia el agente en primer plano para demostrar la programación, sin instalar servicios ni restaurar datos. #>
[CmdletBinding()]
param()
$ErrorActionPreference = 'Stop'
$raiz = Split-Path -Parent $PSScriptRoot
$config = Join-Path $raiz 'Taller\Taller.Respaldos\appsettings.Local.json'
if (-not (Test-Path -LiteralPath $config)) { throw 'Primero ejecute Preparar-RespaldosDemo.ps1 en el servidor.' }
$datos = Get-Content -LiteralPath $config -Raw | ConvertFrom-Json
if ($datos.Respaldos.Habilitado -ne $true -or $datos.Respaldos.BaseControl -ne 'SGTM_DB_Demo_Control') { throw 'Se requiere la configuración local de demostración.' }
if (Get-Service -Name SGTMRespaldos -ErrorAction SilentlyContinue) { throw 'Ya existe un servicio instalado. Use ese agente y revise su señal desde SGTM.' }
if (Get-Process -Name Taller.Respaldos -ErrorAction SilentlyContinue) { throw 'Ya hay un agente en ejecución. No inicie otro.' }
$variables = @('SGTM_CONN','SGTM_MANTENIMIENTO_CONN','Respaldos__BaseControl','Respaldos__Habilitado')
$anteriores = @{}
foreach ($nombre in $variables) { $anteriores[$nombre] = [Environment]::GetEnvironmentVariable($nombre, 'Process') }
try {
    $env:SGTM_CONN = $datos.ConnectionStrings.Default
    $env:SGTM_MANTENIMIENTO_CONN = $datos.ConnectionStrings.Mantenimiento
    $env:Respaldos__BaseControl = $datos.Respaldos.BaseControl
    $env:Respaldos__Habilitado = 'true'
    Write-Host 'Agente de demostración activo mientras esta terminal permanezca abierta. Ctrl+C lo detiene.'
    Write-Host 'Guarde el plan como administrador y verifique señal, próxima ejecución, historial y archivo real. Sin plan activo no se crean copias.'
    Push-Location $raiz
    try {
        & dotnet run --project (Join-Path $raiz 'Taller\Taller.Respaldos')
        if ($LASTEXITCODE -ne 0) { throw 'El agente terminó con error. Revise su configuración y permisos.' }
    } finally { Pop-Location }
} finally {
    foreach ($nombre in $variables) { [Environment]::SetEnvironmentVariable($nombre, $anteriores[$nombre], 'Process') }
}
