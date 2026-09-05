namespace Taller.Dominio.Entidades;

/// <summary>
/// Representa una marca de vehículo en el sistema.
/// </summary>
public sealed class Marca
{
    public int IdMarca { get; set; }
    public string Nombre { get; set; } = null!;
    public bool Activo { get; set; } = true;

    /// <summary>
    /// Representa la colección de modelos asociados a esta marca.
    /// </summary>
    public ICollection<Modelo> Modelos { get; set; } = new List<Modelo>();
}
