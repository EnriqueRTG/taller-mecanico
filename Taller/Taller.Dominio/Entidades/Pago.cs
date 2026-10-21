namespace Taller.Dominio.Entidades;
/// <summary>Registro persistente del circuito de liquidación y cobros internos, sin integración fiscal.</summary>
public sealed class Pago
{
    public int IdPago { get; set; }
    /// <summary>Identificador de intento generado antes de guardar: impide duplicar un cobro al reintentar.</summary>
    public Guid IdOperacion { get; set; }
    public int IdComprobante { get; set; }
    public Comprobante Comprobante { get; set; } = null!;
    public DateTime Fecha { get; set; }
    public int IdUsuarioRegistro { get; set; }
    public Usuario UsuarioRegistro { get; set; } = null!;
    public string UsuarioRegistroNombre { get; set; } = string.Empty;
    public decimal Importe { get; set; }
    public decimal SaldoPosterior { get; set; }
    public bool Anulado { get; set; }
    public string? MotivoAnulacion { get; set; }
    public DateTime? FechaAnulacion { get; set; }
    public int? IdUsuarioAnulacion { get; set; }
    public Usuario? UsuarioAnulacion { get; set; }
    public ICollection<PagoMedio> Medios { get; set; } = new List<PagoMedio>();
}
