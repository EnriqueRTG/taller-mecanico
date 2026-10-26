using Taller.Aplicacion.Consultas;
using Taller.Demostracion;
using Taller.Dominio.Enumeraciones;
using Xunit;
namespace Taller.Pruebas;
public sealed class CatalogoDemostracionPruebas
{
    [Fact]
    public void CatalogoExplicitoTieneUnidadesPreciosYClavesDistintas()
    {
        var datos = CatalogoDemostracion.Entradas;
        Assert.True(datos.Count >= 150);
        Assert.Equal(datos.Count, datos.Select(c => (c.Tipo, c.Nombre.ToUpperInvariant())).Distinct().Count());
        Assert.All(datos, c => { Assert.InRange(c.Nombre.Length, 1, 150); Assert.InRange(c.Categoria.Length, 1, 80); Assert.True(c.Precio > 0); Assert.False(string.IsNullOrWhiteSpace(c.Unidad)); });
        foreach (var nombre in new[] { "Filtro de aceite", "Filtro de aire", "Filtro de combustible", "Filtro de habitáculo", "Disco de freno delantero", "Disco de freno trasero", "Reemplazo de discos traseros" })
            Assert.Contains(datos, c => c.Nombre == nombre);
        Assert.Contains(datos, c => c.Tipo == TipoConcepto.Consumible && c.Unidad.Contains("litro"));
    }
    [Fact]
    public void EstadosRepresentanSoloLasSieteEtapasVigentes()
    {
        Assert.Equal(new[] { "EnEvaluacionTecnica", "PendienteDecision", "EnEjecucion", "TrabajoFinalizado", "PendienteLiquidacion", "Cerrada", "Cancelada" }, Enum.GetNames<EstadoAtencion>());
    }
}
