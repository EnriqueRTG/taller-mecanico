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
}
