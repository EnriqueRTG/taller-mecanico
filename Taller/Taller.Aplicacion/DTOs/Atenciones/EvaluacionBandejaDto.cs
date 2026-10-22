using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Atenciones;
/// <summary>Datos del circuito técnico y su propuesta económica.</summary>
public sealed class EvaluacionBandejaDto
{
    public int IdAtencion { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public string Vehiculo { get; set; } = string.Empty;
    public string Motivo { get; set; } = string.Empty;
    public TipoSolicitudAtencion TipoSolicitud { get; set; }
    public DateTime Fecha { get; set; }
    public int? IdTecnico { get; set; }
    public string Tecnico { get; set; } = string.Empty;
    public bool Finalizada { get; set; }
    public int? IdDiagnostico { get; set; }
}
