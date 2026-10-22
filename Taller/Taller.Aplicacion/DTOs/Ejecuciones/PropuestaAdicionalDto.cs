using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.DTOs.Ejecuciones;
/// <summary>Contrato de datos del seguimiento operativo; independiente de los controles de presentación.</summary>
public sealed class PropuestaAdicionalDto
{
    public int IdAtencion { get; set; }
    public byte[] Version { get; set; } = [];
    public int? IdHallazgo { get; set; }
    public string Motivo { get; set; } = "";
    public List<ItemPropuestaDto> Items { get; set; } = [];
}
