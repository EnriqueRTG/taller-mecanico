using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Cobros;
/// <summary>Contrato de datos para cobros, independiente de los controles de presentación.</summary>
public sealed class PagoMedioDto
{
    public int IdMedioPago { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public decimal Importe { get; set; }
    public string? Referencia { get; set; }
}
