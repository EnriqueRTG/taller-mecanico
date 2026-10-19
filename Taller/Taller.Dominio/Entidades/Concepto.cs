using Taller.Dominio.Enumeraciones;

namespace Taller.Dominio.Entidades;

/// <summary>
/// Representa un servicio, repuesto o consumible del catálogo.
/// Sus datos actuales no modifican los valores históricos
/// guardados en presupuestos y comprobantes.
/// </summary>
public sealed class Concepto
{
    public int IdConcepto { get; set; }
    public int IdCategoria { get; set; }
    public TipoConcepto Tipo { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public decimal? PrecioReferencia { get; set; }
    public bool Activo { get; set; } = true;

    // Relacion con Categoria
    public Categoria Categoria { get; set; } = null!;
}