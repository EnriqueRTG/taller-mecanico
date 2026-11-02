using System.Text.Json;
using Taller.Aplicacion.DTOs.Respaldos;
using Taller.Infraestructura.Respaldos;
using Xunit;

namespace Taller.Pruebas.ReglasYComponentes;
public sealed class PaqueteRespaldoPruebas : IDisposable
{
    private readonly string _carpeta = Path.Combine(Path.GetTempPath(), "SGTM-Paquete-" + Guid.NewGuid().ToString("N"));
    public PaqueteRespaldoPruebas() => Directory.CreateDirectory(_carpeta);
    public void Dispose() => Directory.Delete(_carpeta, true);
    [Fact]
    public async Task ExportaYLeeCompletaSinRutasOriginales()
    {
        var original = Path.Combine(_carpeta, "original.bak");
        await File.WriteAllTextAsync(original, "contenido para prueba de transporte");
        var copia = new CopiaRespaldoDto { Id = Guid.NewGuid(), Tipo = TipoRespaldo.Completa, RutaServidor = original };
        var paquete = await PaqueteRespaldo.ExportarAsync(_carpeta, copia.Id, [copia]);
        var leido = await PaqueteRespaldo.LeerAsync(paquete);
        Assert.Equal(copia.Id, leido.Seleccionada);
        Assert.Equal("", leido.Archivos.Single().Copia.RutaServidor);
        await File.AppendAllTextAsync(Path.Combine(paquete, leido.Archivos.Single().Nombre), "alterado");
        await Assert.ThrowsAsync<InvalidOperationException>(() => PaqueteRespaldo.LeerAsync(paquete));
    }
    [Fact]
    public async Task RechazaNombreConSalidaDeCarpeta()
    {
        var copia = new CopiaRespaldoDto { Id = Guid.NewGuid(), Tipo = TipoRespaldo.Completa };
        var m = new PaqueteRespaldo.Manifiesto(1, copia.Id, [new(copia, "../ajeno.bak", 3, new string('A', 64))]);
        await File.WriteAllTextAsync(Path.Combine(_carpeta, "sgtm-respaldo.json"), JsonSerializer.Serialize(m));
        await Assert.ThrowsAsync<InvalidOperationException>(() => PaqueteRespaldo.LeerAsync(_carpeta));
    }
    [Fact]
    public async Task DiferencialSinCompletaNoSePuedeImportar()
    {
        var original = Path.Combine(_carpeta, "original.bak");
        await File.WriteAllTextAsync(original, "diferencial");
        var copia = new CopiaRespaldoDto { Id = Guid.NewGuid(), BaseId = Guid.NewGuid(), Tipo = TipoRespaldo.Diferencial, RutaServidor = original };
        var paquete = await PaqueteRespaldo.ExportarAsync(_carpeta, copia.Id, [copia]);
        await Assert.ThrowsAsync<InvalidOperationException>(() => PaqueteRespaldo.LeerAsync(paquete));
    }
    [Fact]
    public async Task PaqueteDiferencialIncluyeBaseYSePuedeVerificar()
    {
        var original = Path.Combine(_carpeta, "base.bak");
        var cambio = Path.Combine(_carpeta, "diferencial.bak");
        await File.WriteAllTextAsync(original, "base completa");
        await File.WriteAllTextAsync(cambio, "cambios posteriores");
        var completa = new CopiaRespaldoDto { Id = Guid.NewGuid(), Tipo = TipoRespaldo.Completa, RutaServidor = original };
        var diferencial = new CopiaRespaldoDto { Id = Guid.NewGuid(), BaseId = completa.Id, Tipo = TipoRespaldo.Diferencial, RutaServidor = cambio };
        var paquete = await PaqueteRespaldo.ExportarAsync(_carpeta, diferencial.Id, [completa, diferencial]);
        var leido = await PaqueteRespaldo.LeerAsync(paquete);
        Assert.Equal(diferencial.Id, leido.Seleccionada);
        Assert.Equal(2, leido.Archivos.Count);
        Assert.Contains(leido.Archivos, a => a.Copia.Id == completa.Id);
        Assert.All(leido.Archivos, a => Assert.Empty(a.Copia.RutaPaqueteDrive));
    }
    [Fact]
    public async Task DestinoAusenteNoEliminaRespaldoOriginal()
    {
        var original = Path.Combine(_carpeta, "local.bak");
        await File.WriteAllTextAsync(original, "copia local conservada");
        var copia = new CopiaRespaldoDto { Id = Guid.NewGuid(), Tipo = TipoRespaldo.Completa, RutaServidor = original };
        await Assert.ThrowsAsync<InvalidOperationException>(() => PaqueteRespaldo.ExportarAsync(Path.Combine(_carpeta, "ausente"), copia.Id, [copia]));
        Assert.Equal("copia local conservada", await File.ReadAllTextAsync(original));
    }
}
