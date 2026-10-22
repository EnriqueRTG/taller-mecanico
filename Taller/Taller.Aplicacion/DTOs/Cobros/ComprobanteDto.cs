using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Cobros;
/// <summary>Contrato de datos para cobros, independiente de los controles de presentación.</summary>
public sealed class ComprobanteDto
{
    public int IdComprobante { get; set; }
    public int IdAtencion { get; set; }
    public string Numero => $"LIQ-{IdComprobante:D8}";
    public DateTime Fecha { get; set; }
    public byte[] Version { get; set; } = [];
    public string Cliente { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;
    public string Dominio { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public string Moneda { get; set; } = "ARS";
    public decimal Total { get; set; }
    public decimal Pagado { get; set; }
    public decimal Saldo => Total - Pagado;
    public EstadoAtencion EstadoAtencion { get; set; }
    public List<ComprobanteItemDto> Items { get; set; } = [];
    public List<PagoConsultaDto> Pagos { get; set; } = [];
}
