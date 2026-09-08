namespace Taller.Dominio.Entidades;

/// <summary>
/// Representa un vehículo en el sistema.
/// </summary>
public sealed class Vehiculo
{
    public int IdVehiculo { get; set; }
    public string Dominio { get; set; } = null!;
    public int Anio { get; set; }
    public string Color { get; set; } = null!;
    public bool Activo { get; set; } = true;
    public DateTime FechaAlta { get; set; } = DateTime.Now;
    public int IdModelo { get; set; }

    /// <summary>
    /// Representa el modelo asociado a este vehículo.
    /// </summary>
    public Modelo Modelo { get; set; } = null!;

    /// <summary>
    /// Representa la colección de atenciones asociadas a este vehículo.
    /// </summary>
    public ICollection<Atencion> Atenciones { get; set; } = new List<Atencion>();
}
