using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Cobros;
/// <summary>Expediente de liquidación antes o después de emitir el documento interno.</summary>
public sealed class CuentaAtencionDto
{
    public int IdAtencion { get; set; }
    public byte[] VersionAtencion { get; set; } = [];
    public EstadoAtencion Estado { get; set; }
    public string Resumen { get; set; } = string.Empty;
    public List<ComprobanteItemDto> ItemsPorLiquidar { get; set; } = [];
    public decimal TotalPorLiquidar => ItemsPorLiquidar.Sum(x => x.Subtotal);
    public ComprobanteDto? Comprobante { get; set; }
}
