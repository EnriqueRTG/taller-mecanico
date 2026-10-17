namespace Taller.Dominio.Entidades;

/// <summary>
/// Representa una categoria en el sistema.
/// </summary>
public sealed class Categoria
{
    public int IdCategoria { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;

    /// <summary>
    /// Representa la colección de conceptos asociados a esta categoría.
    /// </summary>
    public ICollection<Concepto> Conceptos { get; set; } = new List<Concepto>();    
}