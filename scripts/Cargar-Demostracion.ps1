[CmdletBinding()]
param([Parameter(Mandatory=$true)][string]$Servidor, [string]$BaseDatos='SGTM_DB_Demo')
$ErrorActionPreference='Stop'
if (!$BaseDatos.EndsWith('_Demo',[StringComparison]::OrdinalIgnoreCase)) { throw 'El nombre debe terminar en _Demo.' }
$raiz=Split-Path $PSScriptRoot -Parent
$anteriorConexion=$env:SGTM_CONN
$anteriorClave=$env:SGTM_DEMO_PASSWORD
$anteriorInicial=$env:SGTM_ADMIN_INICIAL_PASSWORD
try {
    $clave=Read-Host 'Contraseña de las cuentas de demostración (mínimo 12 caracteres, mayúscula, minúscula, número y símbolo)' -AsSecureString
    $env:SGTM_DEMO_PASSWORD=[System.Net.NetworkCredential]::new('',$clave).Password
    Remove-Item Env:SGTM_ADMIN_INICIAL_PASSWORD -ErrorAction SilentlyContinue
    # Se prohíben separadores de propiedades, para no permitir inyección de otra base en la cadena.
    if($Servidor.Contains(';') -or $BaseDatos -notmatch '^[A-Za-z0-9_]+$'){throw 'Servidor o nombre de base inválido.'}
    $env:SGTM_CONN="Server=$Servidor;Database=$BaseDatos;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"
    & dotnet run --project (Join-Path $raiz 'Taller/Taller.Demostracion') -- --cargar-demo
    if($LASTEXITCODE -ne 0){throw 'El cargador no terminó correctamente. Revise el error; no se confirmó el lote.'}
} finally {
    $env:SGTM_CONN=$anteriorConexion
    $env:SGTM_DEMO_PASSWORD=$anteriorClave
    $env:SGTM_ADMIN_INICIAL_PASSWORD=$anteriorInicial
}
