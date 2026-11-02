[CmdletBinding()]
param([Parameter(Mandatory=$true)][string]$DirectorioPublicacion)
$ErrorActionPreference = 'Stop'
$directorio = (Resolve-Path -LiteralPath $DirectorioPublicacion).Path
$ejecutable = Join-Path $directorio 'Taller.Respaldos.exe'
$configuracion = Join-Path $directorio 'appsettings.Local.json'
if (-not (Test-Path -LiteralPath $ejecutable -PathType Leaf)) { throw 'Publique Taller.Respaldos para Windows antes de instalar el agente.' }
if (-not (Test-Path -LiteralPath $configuracion -PathType Leaf)) { throw 'Falta appsettings.Local.json con las conexiones SQL y Respaldos:Habilitado.' }
$config = Get-Content -LiteralPath $configuracion -Raw | ConvertFrom-Json
if ($config.Respaldos.Habilitado -ne $true) { throw 'Habilite respaldos y prepare la base de control antes de instalar.' }
if (Get-Service -Name 'SGTMRespaldos' -ErrorAction SilentlyContinue) { throw 'Existe el servicio anterior SGTMRespaldos. Un administrador debe detenerlo y desinstalarlo antes de usar el agente de sesión, para evitar dos agentes.' }
$nombre = 'SGTM-Respaldos-Drive'
if (Get-ScheduledTask -TaskName $nombre -ErrorAction SilentlyContinue) { throw 'La tarea ya existe. Revise su estado en el Programador de tareas; no se reemplazó.' }
$usuario = [System.Security.Principal.WindowsIdentity]::GetCurrent().Name
$accion = New-ScheduledTaskAction -Execute $ejecutable -WorkingDirectory $directorio
$inicio = New-ScheduledTaskTrigger -AtLogOn -User $usuario
$principal = New-ScheduledTaskPrincipal -UserId $usuario -LogonType Interactive -RunLevel Limited
$ajustes = New-ScheduledTaskSettingsSet -MultipleInstances IgnoreNew -ExecutionTimeLimit ([TimeSpan]::Zero) -RestartCount 3 -RestartInterval (New-TimeSpan -Minutes 1) -AllowStartIfOnBatteries -DontStopIfGoingOnBatteries
Register-ScheduledTask -TaskName $nombre -Action $accion -Trigger $inicio -Principal $principal -Settings $ajustes -Description 'Respaldo SQL y paquetes para Drive con la sesión Windows iniciada; no depende de la ventana SGTM.' | Out-Null
Start-ScheduledTask -TaskName $nombre
Write-Host 'Agente registrado para esta sesión y los próximos inicios de sesión. Revise la señal reciente y el resultado de una ejecución en SGTM. Mantenga Windows y Drive iniciados. No se guardó ninguna contraseña.'
