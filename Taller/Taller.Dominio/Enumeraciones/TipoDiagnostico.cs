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
    /// Valor histórico para lectura de informes adicionales anteriores.
    /// Los hallazgos actuales se registran en HallazgoAtencion.
    /// </summary>
    Adicional = 2
}
