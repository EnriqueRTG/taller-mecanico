using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Ejecuciones;
/// <summary>Contrato de datos para ejecuciones, independiente de los controles de presentación.</summary>
public sealed class EjecucionAtencionDto
{
    public int IdAtencion { get; set; }
    public byte[] Version { get; set; } = [];
    public string Resumen { get; set; } = string.Empty;
    public int? IdTecnico { get; set; }
    public EstadoAtencion Estado { get; set; }
    public bool TieneExclusiones { get; set; }
    public bool ConformidadTecnica { get; set; }
    public List<EjecucionItemDto> Items { get; set; } = [];
    public string Observaciones { get; set; } = string.Empty;
}
