#Requires -RunAsAdministrator
[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)][string]$DirectorioPublicacion,
    [System.Management.Automation.PSCredential]$Credencial
)
$ErrorActionPreference = 'Stop'
$directorio = (Resolve-Path -LiteralPath $DirectorioPublicacion).Path
$ejecutable = Join-Path $directorio 'Taller.Respaldos.exe'
$configuracion = Join-Path $directorio 'appsettings.Local.json'
if (-not (Test-Path -LiteralPath $ejecutable -PathType Leaf)) { throw 'Falta publicar Taller.Respaldos.exe para Windows.' }
if (-not (Test-Path -LiteralPath $configuracion -PathType Leaf)) { throw 'Falta la configuración local protegida del agente.' }
$config = Get-Content -LiteralPath $configuracion -Raw | ConvertFrom-Json
if ($config.Respaldos.Habilitado -ne $true) { throw 'El agente está deshabilitado. Termine la instalación SQL y configure Respaldos:Habilitado.' }
if (Get-Service -Name 'SGTMRespaldos' -ErrorAction SilentlyContinue) { throw 'El servicio ya existe. Revise su instalación con el técnico; este script no lo reemplaza ni lo detiene.' }
if ($null -eq $Credencial) { $Credencial = Get-Credential -Message 'Cuenta técnica de Windows autorizada para el agente, con acceso SQL y derecho de iniciar como servicio.' }
if ($null -eq $Credencial) { throw 'Instalación cancelada; no se creó el servicio.' }
New-Service -Name 'SGTMRespaldos' -DisplayName 'SGTM - Respaldos automáticos' -BinaryPathName ('"' + $ejecutable + '"') -StartupType Automatic -Credential $Credencial -Description 'Ejecuta la programación de copias SGTM independientemente de las sesiones de escritorio.' | Out-Null
Start-Service -Name 'SGTMRespaldos'
Get-Service -Name 'SGTMRespaldos'
Write-Host 'Servicio instalado. Verifique la señal reciente del agente y una copia real desde SGTM. Un servicio iniciado no garantiza que SQL o la carpeta tengan permisos correctos.'
