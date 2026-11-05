using System.Security.Cryptography;
using System.Text.Json;
using Taller.Aplicacion.DTOs.Respaldos;

namespace Taller.Infraestructura.Respaldos;

/// <summary>Paquete transportable. Verifica archivos locales; no certifica la sincronización del proveedor.</summary>
public static class PaqueteRespaldo
{
    public sealed record Archivo(CopiaRespaldoDto Copia, string Nombre, long Longitud, string Sha256);
    public sealed record Manifiesto(int Version, Guid Seleccionada, List<Archivo> Archivos);
    internal static async Task<string> Hash(string ruta, CancellationToken ct)
    {
        await using var s = File.OpenRead(ruta);
        return Convert.ToHexString(await SHA256.HashDataAsync(s, ct));
    }
    public static async Task<string> ExportarAsync(string destino, Guid seleccionada, IReadOnlyList<CopiaRespaldoDto> copias, CancellationToken ct = default)
    {
        if (!Directory.Exists(destino)) throw new InvalidOperationException("La carpeta de destino no está disponible. Abra Drive e inicie sesión.");
        var final = Path.Combine(destino, "SGTM-" + seleccionada.ToString("N") + "-" + Guid.NewGuid().ToString("N"));
        var temporal = final + ".pendiente";
        Directory.CreateDirectory(temporal);
        try
        {
            var archivos = new List<Archivo>();
            foreach (var copia in copias)
            {
                var nombre = copia.Id.ToString("N") + ".bak";
                var ruta = Path.Combine(temporal, nombre);
                await CopiarAsync(copia.RutaServidor, ruta, ct);
                var hash = await Hash(ruta, ct);
                if (hash != await Hash(copia.RutaServidor, ct)) throw new InvalidOperationException("La copia transportable no coincide con el respaldo original.");
                // No transportar rutas ni referencias a credenciales de la instalación original.
                archivos.Add(new(copia with { RutaServidor = "", RutaControl = "", RutaPaqueteDrive = "", EstadoDrive = "", UltimoIntentoDriveUtc = null }, nombre, new FileInfo(ruta).Length, hash));
            }
            await File.WriteAllTextAsync(Path.Combine(temporal, "sgtm-respaldo.json"), JsonSerializer.Serialize(new Manifiesto(1, seleccionada, archivos)), ct);
            Directory.Move(temporal, final);
            return final;
        }
        finally { if (Directory.Exists(temporal)) Directory.Delete(temporal, true); }
    }
    public static async Task<Manifiesto> LeerAsync(string carpeta, CancellationToken ct = default)
    {
        var ruta = Path.Combine(carpeta, "sgtm-respaldo.json");
        if (!File.Exists(ruta) || new FileInfo(ruta).Length > 1024 * 1024) throw new InvalidOperationException("Seleccione una carpeta de respaldo SGTM terminada, con su manifiesto.");
        Manifiesto m;
        try
        {
            m = JsonSerializer.Deserialize<Manifiesto>(await File.ReadAllTextAsync(ruta, ct))
                ?? throw new InvalidOperationException("El manifiesto del paquete no es válido.");
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("El manifiesto está incompleto o no es válido. Espere a que Drive termine de descargar el paquete y vuelva a intentar.", ex);
        }
        if (m.Version != 1 || m.Archivos is null || m.Archivos.Count is < 1 or > 2 || m.Archivos.Where(a => a?.Copia != null).Select(a => a.Copia.Id).Distinct().Count() != m.Archivos.Count || !m.Archivos.Any(a => a.Copia.Id == m.Seleccionada)) throw new InvalidOperationException("El paquete no tiene una cadena válida.");
        foreach (var a in m.Archivos)
        {
            if (a?.Copia == null || !Enum.IsDefined(a.Copia.Tipo) || a.Sha256 == null || a.Nombre != a.Copia.Id.ToString("N") + ".bak" || a.Longitud <= 0 || a.Sha256.Length != 64) throw new InvalidOperationException("Datos de archivo inválidos.");
            var f = Path.Combine(carpeta, a.Nombre);
            if (!File.Exists(f) || new FileInfo(f).Length != a.Longitud || await Hash(f, ct) != a.Sha256) throw new InvalidOperationException("El respaldo está incompleto o dañado. Espere la descarga de Drive y vuelva a intentar.");
        }
        var elegida = m.Archivos.Single(a => a.Copia.Id == m.Seleccionada).Copia;
        if (elegida.Tipo == TipoRespaldo.Completa && m.Archivos.Count != 1 || elegida.Tipo == TipoRespaldo.Diferencial && (m.Archivos.Count != 2 || !m.Archivos.Any(a => a.Copia.Id == elegida.BaseId && a.Copia.Tipo == TipoRespaldo.Completa))) throw new InvalidOperationException("Falta la copia completa base de la diferencial.");
        return m;
    }
    public static async Task CopiarAsync(string origen, string destino, CancellationToken ct)
    {
        await using var entrada = new FileStream(origen, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 1024, true);
        await using var salida = new FileStream(destino, FileMode.CreateNew, FileAccess.Write, FileShare.None, 1024 * 1024, true);
        await entrada.CopyToAsync(salida, ct);
        await salida.FlushAsync(ct);
    }
}
