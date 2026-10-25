using Taller.Dominio.Tiempo;
using Taller.Dominio.Enumeraciones;

namespace Taller.Dominio.Entidades;
/// <summary>
/// Representa una atención en el sistema.
/// </summary>
public sealed class Atencion
{
    public ICollection<HallazgoAtencion> Hallazgos { get; set; } = new List<HallazgoAtencion>();
    public ICollection<PausaTrabajo> Pausas { get; set; } = new List<PausaTrabajo>();
    public ICollection<SolicitudAtencion> Solicitudes { get; set; } = new List<SolicitudAtencion>();
    public int? KilometrajeRecepcion { get; set; }
    public string? ObservacionesRecepcion { get; set; }
    public int IdAtencion { get; set; }
    public int IdCliente { get; set; }
    public int IdVehiculo { get; set; }
    public int IdUsuarioRecepcion { get; set; }
    /// <summary>
    /// Identificador del técnico que tiene reservada la actividad actual.
    /// Es null cuando la atención no tiene una actividad reservada.
    /// </summary>
    public int? IdTecnicoEnActividad { get; set; }
    /// <summary>
    /// Actividad reservada por el técnico.
    /// Es null cuando no existe una reserva.
    /// </summary>
    public ActividadTecnica? ActividadTecnica { get; set; }
    public DateTime FechaApertura { get; set; } = FechaHoraTaller.Ahora;
    public TipoSolicitudAtencion TipoSolicitud { get; set; } = TipoSolicitudAtencion.Problema;
    public string MotivoConsulta { get; set; } = null!;
    public EstadoAtencion Estado { get; set; } = EstadoAtencion.Abierta;
    public DateTime? FechaCierre { get; set; }
    /// <summary>
    /// Identifica la versión del registro para detectar
    /// modificaciones realizadas por otra operación.
    /// </summary>
    /// <remarks>
    /// SQL Server genera y actualiza este valor automáticamente.
    /// No debe asignarse manualmente.
    /// </remarks>
    public byte[] Version { get; set; } = [];
    /// <summary>
    /// Representa el vehiculo asociado a esta atención.
    /// </summary>
    public Vehiculo Vehiculo { get; set; } = null!;
    /// <summary>
    /// Representa el cliente asociado a esta atención.
    /// </summary>
    public Cliente Cliente { get; set; } = null!;
    /// <summary>
    /// Representa al usuario de recepción que registró esta atención.
    /// </summary>
    public Usuario UsuarioRecepcion { get; set; } = null!;
    /// <summary>
    /// Usuario técnico que tiene reservada la actividad actual.
    /// No representa un responsable permanente de toda la atención.
    /// </summary>
    public Usuario? TecnicoEnActividad { get; set; }
    /// <summary>
    /// Lista de diagnósticos asociados a esta atención.
    /// </summary>
    public ICollection<Diagnostico> Diagnosticos { get; set; } = new List<Diagnostico>();
    /// <summary>Avance o cierre de la evaluación y propuesta técnica.</summary>
    public EvaluacionAtencion? Evaluacion { get; set; }
    /// <summary>Eventos registrados durante el ciclo de la atención.</summary>
    public ICollection<HistorialAtencion> Historial { get; set; } = new List<HistorialAtencion>();
}
