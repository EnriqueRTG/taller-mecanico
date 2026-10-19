namespace Taller.Dominio.Enumeraciones;

/// <summary>
/// Identifica la actividad técnica reservada en una atención.
/// </summary>
public enum ActividadTecnica
{
    /// <summary>
    /// Elaboración del informe técnico inicial o adicional.
    /// </summary>
    Diagnostico = 1,

    /// <summary>
    /// Preparación de una propuesta técnica.
    /// </summary>
    Presupuesto = 2,

    /// <summary>
    /// Ejecución y registro de los trabajos autorizados.
    /// </summary>
    Ejecucion = 3,
}
