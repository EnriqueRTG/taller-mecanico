using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Atenciones;
/// <summary>Concepto activo que puede incorporarse a una propuesta; el precio es orientativo.</summary>
public sealed class ConceptoPropuestaDto
{
    public int IdConcepto { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string? Descripcion { get; init; }
    public string Categoria { get; init; } = string.Empty;
    public TipoConcepto Tipo { get; init; }
    public decimal? PrecioReferencia { get; init; }
    public string Etiqueta => $"{Nombre} · {Tipo} · {Categoria}";
}
