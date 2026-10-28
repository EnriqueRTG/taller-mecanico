#Requires -Version 5.1
#Requires -RunAsAdministrator
<#
.SYNOPSIS
Prepara respaldos locales de SGTM_DB_Demo en el servidor Windows de demostración.
.DESCRIPTION
No restaura, borra ni carga datos. Crea la carpeta y la base de control, concede
Modify solo a la cuenta del servicio SQL y habilita los perfiles F5 de este equipo.
Debe ejecutarse con SGTM cerrado. No configura otros equipos ni instala un servicio.
#>
[CmdletBinding()]
param(
    [ValidatePattern('^(localhost|\.|\(local\))(\\[A-Za-z0-9_]+)?$')]
    [string]$Servidor = 'localhost\SQLEXPRESS',
    [ValidatePattern('^[A-Za-z]:\\')]
    [string]$Carpeta = 'C:\SGTM\Respaldos'
)
$ErrorActionPreference = 'Stop'
$raiz = Split-Path -Parent $PSScriptRoot
$proyecto = Join-Path $raiz 'Taller\Taller.Respaldos\Taller.Respaldos.csproj'
$perfil = Join-Path $raiz 'Taller\Taller.Presentacion\Properties\launchSettings.json'
$configAgente = Join-Path $raiz 'Taller\Taller.Respaldos\appsettings.Local.json'
if (-not (Test-Path -LiteralPath $proyecto)) { throw 'Ejecute el script desde un repositorio SGTM completo.' }
if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) { throw 'Instale el SDK .NET indicado en global.json.' }
if (Get-Process -Name 'Taller.Presentacion','Taller.Respaldos' -ErrorAction SilentlyContinue) { throw 'Cierre SGTM y el agente antes de preparar la instalación.' }
if (Get-Service -Name 'SGTMRespaldos' -ErrorAction SilentlyContinue) { throw 'Ya existe un agente instalado. Revise su configuración con el técnico; este asistente no lo modifica.' }
# Validar los archivos antes de hacer cambios en SQL o permisos.
$config = if (Test-Path -LiteralPath $perfil) { Get-Content -LiteralPath $perfil -Raw | ConvertFrom-Json } else { [pscustomobject]@{ profiles = [pscustomobject]@{} } }
if ($null -eq $config.profiles) { throw 'El archivo launchSettings.json no contiene profiles válidos.' }
if (Test-Path -LiteralPath $configAgente) {
    $previa = Get-Content -LiteralPath $configAgente -Raw | ConvertFrom-Json
    if ($previa.ConnectionStrings.Default -and $previa.ConnectionStrings.Default -notmatch '(?i)Database=SGTM_DB_Demo(?:;|$)') {
        throw 'El agente tiene una configuración previa distinta. Consérvela y solicite revisión técnica antes de reemplazarla.'
    }
}
$perfilesProyecto = @($config.profiles.PSObject.Properties | Where-Object { $_.Value.commandName -eq 'Project' })
if (@($config.profiles.PSObject.Properties).Count -gt 0 -and $perfilesProyecto.Count -eq 0) { throw 'No existe un perfil Project en launchSettings.json. Seleccione o cree el perfil del proyecto antes de continuar.' }
foreach ($p in $perfilesProyecto) {
    $conexionPrevia = $p.Value.environmentVariables.SGTM_CONN
    if ($conexionPrevia -and $conexionPrevia -notmatch '(?i)(?:^|;)\s*(?:Database|Initial Catalog)\s*=\s*SGTM_DB_Demo\s*(?:;|$)') {
        throw 'Existe un perfil Project dirigido a otra base. Este asistente no reemplaza conexiones reales; revise los perfiles con el técnico.'
    }
}
$instancia = ($Servidor -split '\\', 2)
$servicioSql = if ($instancia.Count -gt 1) { 'MSSQL$' + $instancia[1] } else { 'MSSQLSERVER' }
$servicio = Get-CimInstance Win32_Service | Where-Object { $_.Name -eq $servicioSql }
if ($null -eq $servicio -or $servicio.State -ne 'Running') { throw "No está iniciado el servicio SQL local $servicioSql." }
$cuentaSql = switch ($servicio.StartName) {
    'LocalSystem' { 'NT AUTHORITY\SYSTEM' }
    'NT AUTHORITY\NetworkService' { 'NT AUTHORITY\NETWORK SERVICE' }
    'NT AUTHORITY\LocalService' { 'NT AUTHORITY\LOCAL SERVICE' }
    default { $servicio.StartName }
}
$destino = [System.IO.Path]::GetFullPath($Carpeta)
if ($destino.TrimEnd('\') -eq [System.IO.Path]::GetPathRoot($destino).TrimEnd('\')) { throw 'Seleccione una carpeta específica, no la raíz de una unidad.' }
if ($destino.Length -gt 145) { throw 'La carpeta admite hasta 145 caracteres.' }
$cadena = "Server=$Servidor;Database=SGTM_DB_Demo;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"
Write-Host "Servidor local: $Servidor | Base: SGTM_DB_Demo | Control: SGTM_DB_Demo_Control"
Write-Host "Carpeta: $destino | Cuenta del servicio SQL: $cuentaSql"
Write-Host 'Se conservarán los datos del taller y las copias existentes. No se ejecutará ninguna restauración.'

$variables = @('SGTM_CONN','SGTM_MANTENIMIENTO_CONN','Respaldos__BaseControl','Respaldos__Habilitado')
$anteriores = @{}
foreach ($nombre in $variables) { $anteriores[$nombre] = [Environment]::GetEnvironmentVariable($nombre, 'Process') }
try {
    $env:SGTM_CONN = $cadena
    $env:SGTM_MANTENIMIENTO_CONN = $cadena
    $env:Respaldos__BaseControl = 'SGTM_DB_Demo_Control'
    $env:Respaldos__Habilitado = 'false'
    Push-Location $raiz
    try {
        & dotnet run --project $proyecto -- --verificar-base
        if ($LASTEXITCODE -ne 0) { throw 'No se modificaron perfiles ni permisos de carpeta. Revise la base Demo y el acceso SQL.' }
        # El directorio y sus permisos se aplican al servicio real detectado, nunca a Everyone.
        New-Item -ItemType Directory -Path $destino -Force | Out-Null
        $acl = Get-Acl -LiteralPath $destino
        $regla = New-Object System.Security.AccessControl.FileSystemAccessRule($cuentaSql, 'Modify', 'ContainerInherit,ObjectInherit', 'None', 'Allow')
        $acl.AddAccessRule($regla)
        Set-Acl -LiteralPath $destino -AclObject $acl

        & dotnet run --project $proyecto -- --instalar-control
        if ($LASTEXITCODE -ne 0) { throw 'Falló la instalación del control. No se habilitaron los perfiles. Revise los permisos SQL de su cuenta Windows.' }
        $env:Respaldos__Habilitado = 'true'
        & dotnet run --project $proyecto -- --estado-control
        if ($LASTEXITCODE -ne 0) { throw 'No se pudo consultar el control. No se habilitaron los perfiles.' }
    } finally { Pop-Location }

    # Copia de configuración previa fuera del repositorio; no se publica en Git.
    $reserva = Join-Path $env:LOCALAPPDATA ('SGTM\Configuracion\' + [guid]::NewGuid().ToString('N'))
    New-Item -ItemType Directory -Path $reserva -Force | Out-Null
    foreach ($archivo in @($perfil,$configAgente)) {
        if (Test-Path -LiteralPath $archivo) { Copy-Item -LiteralPath $archivo -Destination (Join-Path $reserva ([IO.Path]::GetFileName($archivo))) }
    }
    if (@($config.profiles.PSObject.Properties).Count -eq 0) {
        $config.profiles | Add-Member -MemberType NoteProperty -Name 'Taller.Presentacion' -Value ([pscustomobject]@{ commandName = 'Project' })
    }
    foreach ($p in $config.profiles.PSObject.Properties) {
        if ($p.Value.commandName -ne 'Project') { continue }
        if ($null -eq $p.Value.environmentVariables) { $p.Value | Add-Member -MemberType NoteProperty -Name environmentVariables -Value ([pscustomobject]@{}) -Force }
        foreach ($par in @{ SGTM_CONN=$cadena; SGTM_MANTENIMIENTO_CONN=$cadena; Respaldos__BaseControl='SGTM_DB_Demo_Control'; Respaldos__Habilitado='true' }.GetEnumerator()) {
            $p.Value.environmentVariables | Add-Member -MemberType NoteProperty -Name $par.Key -Value $par.Value -Force
        }
    }
    New-Item -ItemType Directory -Path (Split-Path -Parent $perfil) -Force | Out-Null
    $utf8 = New-Object System.Text.UTF8Encoding($false)
    [IO.File]::WriteAllText($perfil, ($config | ConvertTo-Json -Depth 50), $utf8)
    $agente = @{ ConnectionStrings=@{Default=$cadena;Mantenimiento=$cadena}; Respaldos=@{Habilitado=$true;BaseControl='SGTM_DB_Demo_Control'} }
    [IO.File]::WriteAllText($configAgente, ($agente | ConvertTo-Json -Depth 10), $utf8)
    Write-Host "Preparación terminada. Configuración anterior: $reserva"
    Write-Host "Reabra Visual Studio, ejecute el perfil del proyecto e ingrese como administrador. Cree una copia Completa en $destino, sin Azure."
    Write-Host 'La copia real comprueba los permisos efectivos de SQL sobre el archivo; preparar el control no sustituye esta prueba.'
    Write-Host 'Para probar programación, ejecute scripts\Iniciar-AgenteDemo.ps1 en otra terminal. Para operación desatendida instale el servicio documentado.'
    Write-Host 'Antes de habilitar otra PC, configure en ella la misma base compartida, base de control y Respaldos:Habilitado=true.'
} finally {
    foreach ($nombre in $variables) { [Environment]::SetEnvironmentVariable($nombre, $anteriores[$nombre], 'Process') }
}
