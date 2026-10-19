namespace Taller.Dominio.Enumeraciones;

/// <summary>
/// Indica el avance de ejecución de un ítem presupuestado.
/// </summary>
/// <remarks>
/// Este estado no autoriza trabajos. Para ejecutarlos,
/// el presupuesto correspondiente debe estar aceptado.
/// </remarks>
public enum EstadoEjecucionDetalle
{
    /// <summary>
    /// El ítem todavía no comenzó a ejecutarse.
    /// </summary>
    NoIniciado = 1,

    /// <summary>
    /// El ítem está en ejecución.
    /// </summary>
    EnCurso = 2,

    /// <summary>
    /// Se ejecutó una parte y se dio por terminado
    /// el ítem sin completar la cantidad prevista.
    /// </summary>
    Parcial = 3,

    /// <summary>
    /// Se completó la cantidad prevista.
    /// </summary>
    Completado = 4,

    /// <summary>
    /// El ítem se descartó sin ejecutar ninguna cantidad.
    /// Debe conservarse la justificación.
    /// </summary>
    Omitido = 5
}