namespace Taller.Dominio.Enumeraciones;

/// <summary>
/// Define la situación de una propuesta respecto de su
/// presentación y de la decisión del cliente.
/// </summary>
/// <remarks>
/// La aceptación de un presupuesto autoriza sus conceptos.
/// No significa que los trabajos ya estén ejecutados ni pagados.
/// </remarks>
public enum EstadoPresupuesto
{
    /// <summary>Propuesta en elaboración, todavía no disponible para presentar.</summary>
    Borrador = 0,
    /// <summary>
    /// La propuesta está registrada y espera ser comunicada
    /// al cliente por Recepción.
    /// </summary>
    PendientePresentacion = 1,

    /// <summary>
    /// Recepción comunicó la propuesta y se espera registrar
    /// la decisión del cliente.
    /// </summary>
    PendienteDecision = 2,

    /// <summary>
    /// El cliente autorizó los conceptos y precios propuestos.
    /// </summary>
    /// <remarks>
    /// El presupuesto del diagnóstico inicial se registra
    /// directamente en este estado al confirmar su autorización.
    /// </remarks>
    Aceptado = 3,

    /// <summary>
    /// El cliente rechazó la propuesta.
    /// Sus conceptos no quedan autorizados para ejecutarse.
    /// </summary>
    /// <remarks>
    /// El rechazo no elimina los cargos ni los trabajos
    /// previamente autorizados mediante otros presupuestos.
    /// </remarks>
    Rechazado = 4
}