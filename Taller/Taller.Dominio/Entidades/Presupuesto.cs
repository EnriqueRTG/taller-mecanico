using Taller.Dominio.Enumeraciones;

namespace Taller.Dominio.Entidades;
/// <summary>
/// Registra una propuesta económica y la decisión del cliente.
/// </summary>
/// <remarks>
/// Toda propuesta pertenece a una atención. Puede vincular un informe
/// de diagnóstico o describir directamente un servicio definido.
/// </remarks>
public sealed class Presupuesto
{
    public string? PersonaDecision { get; set; }
    public string? MedioDecision { get; set; }
    public int IdPresupuesto { get; set; }
    /// <summary>Propuesta anterior conservada al solicitar una revisión.</summary>
    public int? IdPresupuestoAnterior { get; set; }
    public Presupuesto? PresupuestoAnterior { get; set; }
    /// <summary>Adicional independiente: conserva las autorizaciones de otros presupuestos.</summary>
    public bool EsInspeccion { get; set; }
    public bool EsAdicional { get; set; }
    public int? IdHallazgoAtencion { get; set; }
    public HallazgoAtencion? Hallazgo { get; set; }
    public bool Vigente { get; set; } = true;
    public string? MotivoRevision { get; set; }
    public string? ObservacionesDecision { get; set; }
    public bool AlcanceValidado { get; set; }
    /// <summary>
    /// Atención a la que pertenece la propuesta, con o sin diagnóstico.
    /// </summary>
    public int IdAtencion { get; set; }
    /// <summary>
    /// Informe que fundamenta una propuesta de reparación.
    /// Es opcional: los servicios definidos pueden presupuestarse sin diagnóstico.
    /// </summary>
    public int? IdDiagnostico { get; set; }
    /// <summary>
    /// Usuario que creó el presupuesto.
    /// Puede ser distinto del autor del diagnóstico.
    /// </summary>
    public int IdUsuarioCreacion { get; set; }
    public DateTime FechaCreacion { get; set; }
    public EstadoPresupuesto Estado { get; set; } = EstadoPresupuesto.PendientePresentacion;
    /// <summary>
    /// Fecha en que se comunicó la propuesta al cliente.
    /// </summary>
    public DateTime? FechaPresentacion { get; set; }
    /// <summary>
    /// Usuario de Recepción que registró la decisión del cliente.
    /// Es null mientras no exista una decisión.
    /// </summary>
    public int? IdUsuarioDecision { get; set; }
    public DateTime? FechaDecision { get; set; }
    /// <summary>
    /// Información complementaria de la propuesta o su decisión.
    /// </summary>
    public string? Observaciones { get; set; }
    /// <summary>
    /// Versión generada por SQL Server para detectar
    /// modificaciones concurrentes.
    /// </summary>
    public byte[] Version { get; set; } = [];
    public Atencion Atencion { get; set; } = null!;
    public Diagnostico? Diagnostico { get; set; }
    public Usuario UsuarioCreacion { get; set; } = null!;
    public Usuario? UsuarioDecision { get; set; }
    /// <summary>
    /// Conceptos que integran la propuesta económica.
    /// </summary>
    public ICollection<PresupuestoDetalle> Detalles { get; set; } = new List<PresupuestoDetalle>();
}
