using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.DTOs.Ejecuciones;
/// <summary>Contrato de datos del seguimiento operativo; independiente de los controles de presentación.</summary>
public sealed class PausaConsultaDto
{
    public int IdUsuario { get; set; }
    public int? IdTecnicoResolucion { get; set; }
    public string Alcance => IdDetalle is int id ? $"Ítem {id}" : "Toda la intervención";
    public int Id { get; set; }
    public int? IdDetalle { get; set; }
    public string Motivo { get; set; } = "";
    public DateTime Fecha { get; set; }
    public DateTime? FechaResolucion { get; set; }
    public string? Resolucion { get; set; }
}
