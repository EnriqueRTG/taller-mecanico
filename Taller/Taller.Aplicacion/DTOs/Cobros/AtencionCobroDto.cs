using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Cobros;
/// <summary>Contrato de datos para cobros, independiente de los controles de presentación.</summary>
public sealed class AtencionCobroDto
{
    public int IdAtencion { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public string Dominio { get; set; } = string.Empty;
    public EstadoAtencion Estado { get; set; }
    public DateTime Fecha { get; set; }
    public int? IdComprobante { get; set; }
    public decimal? Total { get; set; }
    public decimal? Pagado { get; set; }
    public decimal? Saldo => Total - Pagado;
    public string Situacion => IdComprobante == null ? "Por liquidar" : Saldo == 0 ? "Sin saldo" : Pagado > 0 ? "Pago parcial" : "Sin pagos";
}
