using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.DTOs.Ejecuciones;
/// <summary>Contrato de datos del seguimiento operativo; independiente de los controles de presentación.</summary>
public sealed class HallazgoConsultaDto
{
    public int IdTecnico { get; set; }
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public string Descripcion { get; set; } = "";
    public string Recomendacion { get; set; } = "";
    public bool ImpideContinuar { get; set; }
    public int? IdDetalle { get; set; }
}
