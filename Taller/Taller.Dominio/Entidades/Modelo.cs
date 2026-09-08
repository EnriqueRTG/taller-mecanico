namespace Taller.Dominio.Entidades;

/// <summary>
/// Representa un modelo de vehículo en el sistema.
/// </summary>
public sealed class Modelo
{
    public int IdModelo { get; set; }
    public string Nombre { get; set; } = null!;
    public bool Activo { get; set; } = true;
    public int IdMarca { get; set; }

    /// <summary>
    /// Representa la marca asociada a este modelo.
    /// </summary>
    public Marca Marca { get; set; } = null!;

    /// <summary>
    /// Representa la colección de vehículos asociados a este modelo.
    /// </summary>
    public ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
