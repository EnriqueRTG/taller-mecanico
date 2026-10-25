using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Atenciones;
/// <summary>Documento de trabajo del técnico. La versión evita sobrescribir avances ajenos.</summary>
public sealed class EvaluacionAtencionDto
{
    public List<SolicitudIngresoDto> Solicitudes { get; set; } = [];
    public string? ObservacionesRecepcion { get; set; }
    public int IdAtencion { get; set; }
    public byte[] VersionAtencion { get; set; } = [];
    public string Cliente { get; set; } = string.Empty;
    public string Vehiculo { get; set; } = string.Empty;
    public string? MotivoRevision { get; set; }
    public string Motivo { get; set; } = string.Empty;
    public TipoSolicitudAtencion TipoSolicitud { get; set; }
    public bool InformeHistorico { get; set; }
    public bool IncluyeDiagnostico { get; set; }
    public string FallaDetectada { get; set; } = string.Empty;
    public string TrabajoRecomendado { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public int? Kilometraje { get; set; }
    public bool SoloEvaluacion { get; set; }
    public List<ItemPropuestaDto> Items { get; set; } = [];
    public List<ServicioEvaluadoDto> ServiciosIniciales { get; set; } = [];
}
