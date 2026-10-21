using Taller.Dominio.Enumeraciones;

namespace Taller.Dominio.Entidades;

/// <summary>
/// Representa un concepto en el sistema.
/// </summary>
public sealed class Concepto
{
    public int IdConcepto { get; set; }
    public TipoConcepto Tipo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; } 
    public decimal? PrecioReferencia { get; set; }
    public bool Activo { get; set; } = true;
    //public bool DisponibleParaIngreso { get; set; }
    public int IdCategoria { get; set; }

    /// <summary>
    /// Representa la categoria asociado a este concepto.
    /// </summary>
    public Categoria Categoria { get; set; } = null!;    
}