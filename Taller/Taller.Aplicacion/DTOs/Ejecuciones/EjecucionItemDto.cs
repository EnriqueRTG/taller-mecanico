using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Ejecuciones;
/// <summary>Contrato de datos para ejecuciones, independiente de los controles de presentación.</summary>
public sealed class EjecucionItemDto
{
    public int IdDetalle { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public decimal CantidadAutorizada { get; set; }
    public decimal CantidadEjecutada { get; set; }
    public EstadoEjecucionDetalle Estado { get; set; }
    public string? Observaciones { get; set; }
}
