namespace Taller.Dominio.Entidades;

/// <summary>
/// Agrupa los conceptos del catálogo del taller, como repuestos, insumos y servicios.
/// </summary>
public sealed class Categoria
{
    public int IdCategoria { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;

    // Relacion uno a muchos con Concepto
    public ICollection<Concepto> Conceptos { get; set; } = new List<Concepto>();
}
