namespace Taller.Dominio.Enumeraciones;

/// <summary>
/// Define la clasificación operativa de los conceptos en el taller.
/// </summary>
public enum TipoConcepto
{
    // <summary>
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