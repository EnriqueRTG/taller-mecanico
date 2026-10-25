using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.DTOs.Ejecuciones;
/// <summary>Contrato de datos del seguimiento operativo; independiente de los controles de presentación.</summary>
public sealed class SeguimientoTrabajoDto
{
    public int IdAtencion { get; set; }
    public byte[] Version { get; set; } = [];
    public string Resumen { get; set; } = "";
    public EstadoAtencion Estado { get; set; }
    public string InformeInicial { get; set; } = "";
    public string TrabajosRegistrados { get; set; } = "";
    public List<EjecucionItemDto> Progreso { get; set; } = [];
    public bool PuedePausar { get; set; }
    public bool PuedeRegistrar { get; set; }
    public List<HallazgoConsultaDto> Hallazgos { get; set; } = [];
    public List<PausaConsultaDto> Pausas { get; set; } = [];
    public List<MaterialTrabajoDto> Items { get; set; } = [];
}
