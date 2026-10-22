using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.DTOs.Catalogo;
/// <summary>Consulta de oferta comercial. Un concepto activo no representa stock físico disponible.</summary>
public sealed record ConceptoCatalogoDto(int IdConcepto, int IdCategoria, string Categoria, TipoConcepto Tipo, string Nombre, string Descripcion, decimal? PrecioReferencia, bool Disponible, bool DisponibleParaIngreso)
{
    public string Estado => Disponible ? "Disponible en catálogo" : "No disponible";
    public string Precio => PrecioReferencia is decimal p ? p.ToString("N2", System.Globalization.CultureInfo.GetCultureInfo("es-AR")) : "A cotizar";
}
