namespace Taller.Dominio.Enumeraciones;

/// <summary>
/// Clasifica los conceptos que pueden incluirse en el catálogo del taller.
/// </summary>
public enum TipoConcepto
{
    /// <summary>
    /// Trabajo o servicio brindado por el taller.
    /// </summary>
    Servicio = 1,
    /// <summary>
    /// Pieza destinada a instalarse o reemplazarse en un vehículo.
    /// </summary>
    Repuesto = 2,
    /// <summary>
    /// Material utilizado o consumido durante la intervención.
    /// </summary>
    Consumible = 3,
}
