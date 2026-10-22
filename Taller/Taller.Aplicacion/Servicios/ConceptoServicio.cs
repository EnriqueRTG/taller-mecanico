using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Catalogo;
using Taller.Aplicacion.Seguridad;

namespace Taller.Aplicacion.Servicios;
/// <summary>Consulta autorizada del catálogo vigente; precios de referencia, no stock ni importes históricos de presupuestos.</summary>
public sealed class ConceptoServicio(IConceptoRepositorio conceptos, AutorizacionOperativa acceso)
{
    public async Task<List<ConceptoCatalogoDto>> ListarAsync()
    {
        await acceso.ExigirAsync();
        return (await conceptos.ListarCatalogoAsync()).Select(c => new ConceptoCatalogoDto(c.IdConcepto, c.IdCategoria, c.Categoria.Nombre, c.Tipo, c.Nombre, c.Descripcion ?? "", c.PrecioReferencia, c.Activo && c.Categoria.Activo, c.DisponibleParaIngreso && c.Activo && c.Categoria.Activo)).ToList();
    }
}
