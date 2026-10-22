using Taller.Aplicacion.DTOs.Catalogo;

namespace Taller.Aplicacion.Consultas;
/// <summary>Aplica criterios de consulta sin alterar el catálogo ni inferir stock.</summary>
public static class ConsultaCatalogo
{
    public static IEnumerable<ConceptoCatalogoDto> Aplicar(IEnumerable<ConceptoCatalogoDto> datos, FiltroCatalogoDto f) => datos.Where(d => (f.IdCategoria == 0 || d.IdCategoria == f.IdCategoria) && (!f.Tipo.HasValue || d.Tipo == f.Tipo) && (f.Disponibilidad <= 0 || f.Disponibilidad == 1 && d.Disponible || f.Disponibilidad == 2 && !d.Disponible || f.Disponibilidad == 3 && d.DisponibleParaIngreso) && BusquedaTexto.Coincide($"{d.IdConcepto} {d.Nombre} {d.Descripcion} {d.Categoria} {d.Tipo}", f.Busqueda)).OrderByDescending(d => d.Disponible).ThenBy(d => d.Categoria).ThenBy(d => d.Nombre).ThenBy(d => d.IdConcepto);
}
