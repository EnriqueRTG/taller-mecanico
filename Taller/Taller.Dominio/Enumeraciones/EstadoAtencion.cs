namespace Taller.Dominio.Enumeraciones;

/// <summary>
/// Define las etapas del ciclo de atención de un vehículo,
/// desde su registro hasta la entrega o cancelación.
/// </summary>
/// <remarks>
/// Las transiciones se realizan mediante operaciones autorizadas
/// de los servicios de aplicación, que validan las condiciones
/// de negocio correspondientes.
///
/// Entity Framework Core persiste el nombre del estado como texto.
/// Los valores numéricos identifican los estados; no obligan
/// a recorrerlos consecutivamente.
///
/// El estado indica la etapa actual. Los documentos y el historial
/// conservan las decisiones, los trabajos realizados y el resultado
/// de la atención.
/// </remarks>
public enum EstadoAtencion
{
    /// <summary>
    /// La atención está disponible o reservada para preparación técnica.
    /// La reserva identifica si un técnico ya comenzó. Incluye la evaluación,
    /// registrando el diagnóstico o preparando un presupuesto
    /// de reparación.
    /// </summary>
    /// <remarks>
    /// También se utiliza cuando el cliente rechaza una propuesta
    /// y se acuerda preparar otra antes de iniciar la reparación.
    /// </remarks>
    EnEvaluacionTecnica = 3,

    /// <summary>
    /// Existe un presupuesto de reparación inicial presentado
    /// y se espera que Recepción registre la aceptación
    /// o el rechazo comunicado por el cliente.
    /// </summary>
    /// <remarks>
    /// La aceptación del servicio de diagnóstico inicial
    /// no autoriza la ejecución de la reparación.
    /// </remarks>
    PendienteDecision = 4,

    /// <summary>
    /// El técnico responsable puede ejecutar los trabajos
    /// autorizados mediante presupuestos aceptados.
    /// </summary>
    /// <remarks>
    /// Los trabajos adicionales requieren una nueva aceptación
    /// antes de ejecutarse.
    /// </remarks>
    EnEjecucion = 5,

    /// <summary>
    /// El técnico confirmó la finalización de los trabajos
    /// autorizados y no quedan propuestas ni hallazgos
    /// pendientes de resolución.
    /// </summary>
    /// <remarks>
    /// Recepción puede gestionar la liquidación, el comprobante,
    /// los pagos y la entrega.
    ///
    /// Una atención de solo diagnóstico
    /// utiliza <see cref="PendienteLiquidacion"/>.
    /// </remarks>
    TrabajoFinalizado = 7,

    /// <summary>
    /// La actividad técnica terminó sin completar una reparación
    /// y Recepción debe resolver la liquidación y la entrega.
    /// </summary>
    /// <remarks>
    /// Comprende la atención de solo diagnóstico, el desistimiento
    /// después de realizar servicios.
    ///
    /// Se liquidan los servicios autorizados efectivamente realizados,
    /// los repuestos instalados y los consumibles utilizados,
    /// con las justificaciones correspondientes.
    /// </remarks>
    PendienteLiquidacion = 8,

    /// <summary>
    /// Recepción registró la entrega del vehículo y completó
    /// la liquidación y el cobro, con saldo pendiente igual a cero.
    /// </summary>
    /// <remarks>
    /// Puede corresponder a una reparación completada,
    /// o una atención de solo diagnóstico.
    ///
    /// Cuando existe un importe a cobrar, debe haberse emitido
    /// el comprobante correspondiente. Una liquidación en cero
    /// requiere dejar registrada su justificación.
    ///
    /// El resultado y la condición de entrega se conservan
    /// en los registros de la atención.
    /// </remarks>
    Cerrada = 9,

    /// <summary>
    /// La atención terminó por desistimiento antes de realizar
    /// servicios o utilizar repuestos y consumibles.
    /// </summary>
    /// <remarks>
    /// Se conserva el motivo de la cancelación.
    ///
    /// Si ya se realizó el diagnóstico o parte del trabajo,
    /// corresponde utilizar <see cref="PendienteLiquidacion"/>
    /// antes de registrar la entrega.
    /// </remarks>
    Cancelada = 10
}
