namespace Taller.Dominio.Enumeraciones;

/// <summary>
/// Define los estados del ciclo de atención de un vehículo,
/// desde su registro hasta la entrega o cancelación.
/// </summary>
/// <remarks>
/// Los cambios de estado se realizan mediante las operaciones
/// autorizadas de los servicios de aplicación.
/// Entity Framework Core persiste el nombre del estado como texto.
/// </remarks>
public enum EstadoAtencion
{
    /// <summary>
    /// La atención fue registrada y permanece pendiente
    /// de que Recepción envíe el vehículo a diagnóstico.
    /// </summary>
    Abierta = 1,

    /// <summary>
    /// Recepción confirmó el ingreso del vehículo al circuito
    /// técnico y se encuentra pendiente de iniciar el diagnóstico.
    /// </summary>
    PendienteDiagnostico = 2,

    /// <summary>
    /// El técnico está elaborando el diagnóstico o preparando
    /// presupuestos a partir del diagnóstico confirmado.
    /// </summary>
    EnEvaluacionTecnica = 3,

    /// <summary>
    /// Existe al menos un presupuesto presentado y se espera
    /// que Recepción registre la decisión comunicada por el cliente.
    /// </summary>
    PendienteDecision = 4,

    /// <summary>
    /// Existe un presupuesto aceptado y los trabajos autorizados
    /// se encuentran pendientes de finalización técnica.
    /// </summary>
    EnEjecucion = 5,

    /// <summary>
    /// El técnico confirmó la finalización de los trabajos.
    /// Recepción puede gestionar el comprobante, los pagos y la entrega.
    /// </summary>
    TrabajoFinalizado = 6,

    /// <summary>
    /// El vehículo fue entregado con los trabajos finalizados,
    /// el comprobante emitido y el saldo pendiente igual a cero.
    /// </summary>
    Cerrada = 7,

    /// <summary>
    /// La atención terminó por desistimiento antes de la aceptación
    /// de un presupuesto, conservando los antecedentes registrados.
    /// </summary>
    Cancelada = 8
}