using System.Security.Cryptography;
using System.Text.Json;
using Azure;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Respaldos;

namespace Taller.Infraestructura.Respaldos;
/// <summary>Azure Blob privado con identidad Entra. SQL y este proceso deben compartir acceso al archivo de staging.</summary>
public sealed class ArchivoRespaldoAzure(IConfiguration configuracion) : IArchivoRespaldoNube
{
    private Uri Contenedor()
    {
        var valor = configuracion["Respaldos:Azure:ContenedorUri"];
        if (!Uri.TryCreate(valor, UriKind.Absolute, out var u) || u.Scheme != "https" || !u.Host.EndsWith(".blob.core.windows.net", StringComparison.OrdinalIgnoreCase) || u.UserInfo.Length != 0 || u.Query.Length != 0 || u.Fragment.Length != 0 || u.Segments.Length != 2 || u.Segments[1].Trim('/').Length < 3)
            throw new InvalidOperationException("Configure un contenedor Azure privado mediante Respaldos:Azure:ContenedorUri (HTTPS, sin claves ni SAS). Consulte la guía de instalación.");
        return u;
    }

    public void ValidarConfiguracion() => _ = Contenedor();
    private static BlobContainerClient Cliente(Uri uri) => new(uri, new DefaultAzureCredential(new DefaultAzureCredentialOptions { ExcludeInteractiveBrowserCredential = true }));
    private static async Task<string> HuellaAsync(string ruta, CancellationToken ct)
    {
        await using var f = new FileStream(ruta, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 1024, true);
        return Convert.ToHexString(await SHA256.HashDataAsync(f, ct));
    }

    public async Task<CopiaRespaldoDto> SubirAsync(CopiaRespaldoDto copia, CancellationToken ct = default)
    {
        var uri = Contenedor();
        var contenedor = Cliente(uri);
        if ((await contenedor.GetPropertiesAsync(cancellationToken: ct)).Value.PublicAccess != PublicAccessType.None)
            throw new InvalidOperationException("El contenedor debe ser privado.");
        if (!File.Exists(copia.RutaServidor))
            throw new InvalidOperationException("El proceso de respaldos no puede leer el archivo local del servidor SQL. Instálelo junto al servidor o configure una carpeta compartida con permisos.");
        var hash = await HuellaAsync(copia.RutaServidor, ct);
        var longitud = new FileInfo(copia.RutaServidor).Length;
        var nombre = $"sgtm/{copia.Familia:N}/{copia.Id:N}/negocio.bak";
        var blob = contenedor.GetBlobClient(nombre);
        try
        {
            await using var f = new FileStream(copia.RutaServidor, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 1024, true);
            await blob.UploadAsync(f, new BlobUploadOptions { Conditions = new BlobRequestConditions { IfNoneMatch = ETag.All }, Metadata = new Dictionary<string, string> { ["sha256"] = hash, ["copia"] = copia.Id.ToString("N") }, TransferValidation = new UploadTransferValidationOptions { ChecksumAlgorithm = StorageChecksumAlgorithm.Auto } }, ct);
        }
        catch (RequestFailedException ex)when (ex.Status is 409 or 412)
        { /* Reintento: verificar el objeto existente, nunca sobrescribir. */
        }

        var propiedades = (await blob.GetPropertiesAsync(cancellationToken: ct)).Value;
        if (propiedades.ContentLength != longitud || !propiedades.Metadata.TryGetValue("sha256", out var remoto) || remoto != hash)
            throw new InvalidOperationException("La copia remota existente no coincide. Se conserva el respaldo local y no se sobrescribe Azure.");
        // Lectura completa con ETag: no se confunde una metadata declarada con una comprobación del contenido.
        var descarga = await blob.DownloadStreamingAsync(new BlobDownloadOptions { Conditions = new BlobRequestConditions { IfMatch = propiedades.ETag } }, ct);
        await using (var contenido = descarga.Value.Content)
            if (Convert.ToHexString(await SHA256.HashDataAsync(contenido, ct)) != hash)
                throw new InvalidOperationException("La comprobación SHA-256 del contenido remoto falló.");
        var resultado = copia with
        {
            AzureSolicitado = true,
            EstadoAzure = "Verificada",
            ContenedorAzure = uri.AbsoluteUri.TrimEnd('/'),
            BlobAzure = nombre,
            Sha256 = hash,
            Longitud = longitud,
            ETagAzure = propiedades.ETag.ToString(),
            VerificadaAzureUtc = DateTimeOffset.UtcNow
        };
        // Manifiesto portable sin credenciales; el catálogo de control sigue siendo la autoridad para restaurar.
        await contenedor.GetBlobClient($"sgtm/{copia.Familia:N}/{copia.Id:N}/manifiesto.json").UploadAsync(BinaryData.FromString(JsonSerializer.Serialize(resultado)), overwrite: true, cancellationToken: ct);
        if (File.Exists(copia.RutaControl))
            await contenedor.GetBlobClient($"sgtm/{copia.Familia:N}/{copia.Id:N}/control.bak").UploadAsync(copia.RutaControl, overwrite: true, cancellationToken: ct);
        return resultado;
    }

    public async Task RecuperarArchivoAsync(CopiaRespaldoDto copia, CancellationToken ct = default)
    {
        if (File.Exists(copia.RutaServidor))
            return;
        if (copia.EstadoAzure != "Verificada" || copia.Sha256.Length != 64 || copia.Longitud <= 0 || copia.ETagAzure.Length == 0)
            throw new InvalidOperationException("No hay una réplica Azure verificada de este archivo.");
        // Solo se admiten el contenedor instalado y nombres generados por SGTM, nunca URLs arbitrarias del catálogo.
        var uri = Contenedor();
        if (copia.ContenedorAzure != uri.AbsoluteUri.TrimEnd('/') || copia.BlobAzure != $"sgtm/{copia.Familia:N}/{copia.Id:N}/negocio.bak")
            throw new InvalidOperationException("La copia remota no corresponde al destino instalado.");
        var temporal = copia.RutaServidor + ".descarga-" + Guid.NewGuid().ToString("N");
        try
        {
            await Cliente(uri).GetBlobClient(copia.BlobAzure).DownloadToAsync(temporal, new BlobDownloadToOptions { Conditions = new BlobRequestConditions { IfMatch = new ETag(copia.ETagAzure) } }, ct);
            if (new FileInfo(temporal).Length != copia.Longitud || await HuellaAsync(temporal, ct) != copia.Sha256)
                throw new InvalidOperationException("La descarga no superó la comprobación de integridad.");
            File.Move(temporal, copia.RutaServidor, overwrite: false);
        }
        finally
        {
            if (File.Exists(temporal))
                File.Delete(temporal);
        }
    }
}
