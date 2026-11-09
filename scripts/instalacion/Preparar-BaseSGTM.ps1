<#
.SYNOPSIS
Prepara la conexion de SGTM a una base nueva o a una base existente.

.DESCRIPTION
En el modo de creacion, crea la base, aplica el esquema y carga el
catalogo inicial. Con -UsarBaseExistente, consulta el catalogo ya cargado.

En ambos modos genera appsettings.Local.json junto al ejecutable,
con la cadena de conexion y el ID del concepto de ingreso por diagnostico.
#>
[CmdletBinding()]
param(
    # Instancia de SQL Server a la que se conecta el script.
    [string]$Servidor = 'localhost\SQLEXPRESS',

    # El nombre se usa como identificador SQL al crear la base.
    # La validacion admite una letra inicial y luego letras, numeros o guion bajo.
    [ValidatePattern('^[a-zA-Z][a-zA-Z0-9_]{0,127}$')]
    [string]$Base = 'SGTM_DB',

    # Carpeta instalada por el MSI o carpeta que contiene Aplicacion y BaseDatos.
    [Parameter(Mandatory)]
    [string]$CarpetaPreparacion,

    # Activa el modo de configuracion de una base ya inicializada.
    [switch]$UsarBaseExistente
)

# Hace que los errores de los comandos de PowerShell interrumpan el flujo.
$ErrorActionPreference = 'Stop'

# Usa la instancia indicada para la instalación si no se pasó -Servidor.
if (-not $PSBoundParameters.ContainsKey('Servidor') -and
    -not [string]::IsNullOrWhiteSpace($env:SGTM_SQL_SERVIDOR)) {
    $Servidor = $env:SGTM_SQL_SERVIDOR.Trim()
}

# En el MSI, el ejecutable esta en la carpeta indicada. En la estructura
# de preparacion, esta dentro de la subcarpeta Aplicacion.
$carpeta = (Resolve-Path -LiteralPath $CarpetaPreparacion).ProviderPath
$aplicacion = $carpeta
$ejecutable = Join-Path $aplicacion 'Taller.Presentacion.exe'

if (!(Test-Path -LiteralPath $ejecutable -PathType Leaf)) {
    $aplicacion = Join-Path $carpeta 'Aplicacion'
    $ejecutable = Join-Path $aplicacion 'Taller.Presentacion.exe'
}

if (!(Test-Path -LiteralPath $ejecutable -PathType Leaf)) {
    throw "No se encontro Taller.Presentacion.exe en '$carpeta' ni en su subcarpeta Aplicacion."
}

# Protege la configuracion de una instalacion que ya fue configurada.
$destino = Join-Path $aplicacion 'appsettings.Local.json'
if (Test-Path -LiteralPath $destino) {
    throw "Ya existe una configuracion local en '$destino'."
}

# La conexion inicial entra en master con la cuenta de Windows que ejecuta
# el script. TrustServerCertificate permite usar el certificado local de
# SQL Express sin validar su cadena de confianza.
$cadena = [System.Data.SqlClient.SqlConnectionStringBuilder]::new()
$cadena['Data Source'] = $Servidor
$cadena['Initial Catalog'] = 'master'
$cadena['Integrated Security'] = $true
$cadena['Encrypt'] = $true
$cadena['TrustServerCertificate'] = $true

# Esta conexion es temporal. Desactivar el pool permite cerrar la sesion
# antes de intentar retirar una base creada por una ejecucion fallida.
$cadenaPreparacion = [System.Data.SqlClient.SqlConnectionStringBuilder]::new($cadena.ConnectionString)
$cadenaPreparacion['Pooling'] = $false
$conexion = [System.Data.SqlClient.SqlConnection]::new($cadenaPreparacion.ConnectionString)

# Estas marcas identifican los recursos que pertenecen a esta ejecucion.
$creada = $false
$configuracionCreada = $false

try {
    $conexion.Open()
    $consulta = $conexion.CreateCommand()
    $consulta.CommandTimeout = 120

    if ($UsarBaseExistente) {
        # Lee el ID real en la base seleccionada. Esta rama solo consulta SQL
        # y funciona aunque la carpeta instalada no contenga los archivos SQL.
        $conexion.ChangeDatabase($Base)
        $consulta.CommandText = @'
SELECT IdConcepto
FROM dbo.Conceptos
WHERE Nombre = N'Diagn' + NCHAR(243) + N'stico electr' + NCHAR(243) + N'nico';
'@
        $id = $consulta.ExecuteScalar()
    }
    else {
        # Carga ambos archivos antes de crear la base. Si falta un archivo,
        # el error ocurre antes de modificar la instancia.
        $sql = Join-Path $carpeta 'BaseDatos'
        $esquema = [IO.File]::ReadAllText((Join-Path $sql 'SGTM-Esquema.sql'))
        $catalogo = [IO.File]::ReadAllText((Join-Path $sql 'SGTM-CatalogoInicial.sql'))

        # Crea la base desde master. Marca la base como propia solo cuando
        # SQL Server confirma que CREATE DATABASE termino correctamente.
        $consulta.CommandText = "CREATE DATABASE [$Base]"
        [void]$consulta.ExecuteNonQuery()
        $creada = $true
        $conexion.ChangeDatabase($Base)

        # GO separa los lotes del esquema. Cada lote se envia a SQL Server
        # sin el separador y dentro de la misma transaccion.
        $consulta.Transaction = $conexion.BeginTransaction()
        foreach ($lote in [regex]::Split($esquema, '(?im)^[ \t]*GO[ \t]*\r?$')) {
            if (![string]::IsNullOrWhiteSpace($lote)) {
                $consulta.CommandText = $lote
                [void]$consulta.ExecuteNonQuery()
            }
        }

        # El catalogo devuelve el ID del concepto de ingreso por diagnostico.
        $consulta.CommandText = $catalogo
        $id = $consulta.ExecuteScalar()
    }

    # Ambos modos necesitan un ID valido para generar la configuracion.
    if ($id -isnot [int] -or $id -le 0) {
        throw 'No se obtuvo un identificador valido para Diagnostico electronico.'
    }

    if ($creada) {
        $consulta.Transaction.Commit()
    }

    # La cadena de la aplicacion apunta a la base de SGTM en lugar de master.
    $cadena['Initial Catalog'] = $Base
    $configuracion = @{
        ConnectionStrings = @{ Default = $cadena.ConnectionString }
        IngresoDiagnostico = @{ IdConcepto = $id }
    } | ConvertTo-Json

    # CreateNew evita sobrescribir el JSON si se creo entre la verificacion
    # inicial y este punto. El bloque finally libera el archivo al terminar.
    $archivo = [IO.File]::Open($destino, [IO.FileMode]::CreateNew)
    $configuracionCreada = $true
    try {
        $bytes = [Text.Encoding]::UTF8.GetBytes($configuracion)
        $archivo.Write($bytes, 0, $bytes.Length)
    }
    finally {
        $archivo.Dispose()
    }
}
catch {
    # Cierra la sesion de trabajo y revierte una transaccion sin confirmar.
    $conexion.Close()

    # La limpieza de SQL solo se activa si esta ejecucion creo la base.
    # Al volver a abrir la conexion, el catalogo inicial vuelve a ser master.
    if ($creada) {
        try {
            $conexion.Open()
            $limpieza = $conexion.CreateCommand()
            $limpieza.CommandText = "DROP DATABASE [$Base]"
            [void]$limpieza.ExecuteNonQuery()
        }
        catch {
            Write-Warning "No se pudo retirar la base recien creada '$Base'. $($_.Exception.Message)"
        }
    }

    # Si fallo la escritura, retira el archivo parcial creado por el script.
    if ($configuracionCreada) {
        try {
            Remove-Item -LiteralPath $destino
        }
        catch {
            Write-Warning "No se pudo retirar el archivo '$destino'. $($_.Exception.Message)"
        }
    }

    # Devuelve el error original al proceso que invoco el script.
    throw
}
finally {
    $conexion.Dispose()
}

if ($UsarBaseExistente) {
    Write-Host "Base existente configurada: $Base. Concepto de diagnostico: $id."
}
else {
    Write-Host "Base creada e inicializada: $Base. Concepto de diagnostico: $id."
}
Write-Host "Configuracion guardada en: $destino"