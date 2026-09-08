namespace Taller.Dominio.Enumeraciones;

/// <summary>
/// Define los posibles estados de una atención durante
/// su ciclo de vida dentro del taller.
/// </summary>
public enum EstadoAtencion
{
    /// <summary>
    /// La atención fue registrada y se encuentra pendiente
    /// de iniciar los procesos posteriores.
    /// </summary>
    Abierta = 1,

    /// <summary>
    /// La atención se encuentra avanzando por alguno de los
    /// procesos asociados, como diagnóstico, presupuesto
    /// o ejecución del trabajo.
    /// </summary>
    EnProceso = 2,

    /// <summary>
    /// La atención finalizó y el vehículo fue entregado al cliente.
    /// </summary>
    Cerrada = 3,

    /// <summary>
    /// La atención fue cancelada antes de completar
    /// normalmente su ciclo de trabajo.
    /// </summary>
    Cancelada = 4
}
