using System.Reflection;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;
using Taller.Infraestructura.Persistencia;
using Xunit;

namespace Taller.Pruebas;

/// <summary>Impide que las capas internas dependan de formularios o de herramientas auxiliares.</summary>
public sealed class ArquitecturaPruebas
{
    [Fact]
    public void DependenciasDelSistemaRespetanElSentidoDeLasCapas()
    {
        Verificar(typeof(Atencion).Assembly, []);
        Verificar(typeof(AtencionServicio).Assembly, ["Taller.Dominio"]);
        Verificar(typeof(TallerDbContext).Assembly, ["Taller.Dominio", "Taller.Aplicacion"]);
    }

    private static void Verificar(Assembly ensamblado, string[] permitidas)
    {
        foreach (var referencia in ensamblado.GetReferencedAssemblies().Where(r => r.Name!.StartsWith("Taller.", StringComparison.Ordinal)))
            Assert.Contains(referencia.Name!, permitidas);
    }
}
