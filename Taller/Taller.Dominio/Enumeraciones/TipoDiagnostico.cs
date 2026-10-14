namespace Taller.Dominio.Enumeraciones;

/// <summary>
/// Distingue el informe inicial de los hallazgos adicionales.
/// </summary>
public enum TipoDiagnostico
{
    /// <summary>
    /// Primera evaluación técnica de la atención.
    /// </summary>
    Inicial = 1,

    /// <summary>
    /// Evaluación de un nuevo hallazgo durante la reparación.
    /// </summary>
    Adicional = 2
}
