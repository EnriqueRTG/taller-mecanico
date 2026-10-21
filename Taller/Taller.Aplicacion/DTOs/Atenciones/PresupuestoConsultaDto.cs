using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Atenciones;
/// <summary>Datos del circuito técnico y su propuesta económica.</summary>
public sealed class PresupuestoConsultaDto
{
    public int IdPresupuesto { get; set; }
    public int IdAtencion { get; set; }
    public bool Vigente { get; set; }
    public int? IdPresupuestoAnterior { get; set; }
    public decimal TotalAceptado { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public string Vehiculo { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public EstadoPresupuesto Estado { get; set; }
    public TipoPresupuesto Tipo { get; set; }
    public string TipoDescripcion => Tipo == TipoPresupuesto.Diagnostico ? "Servicios iniciales" : "Propuesta de trabajo";
    public string EstadoDescripcion => Estado switch
    {
        EstadoPresupuesto.Aceptado when Items.Any(i => i.AceptadoPorCliente == false) => "Aceptado parcialmente",
        EstadoPresupuesto.PendientePresentacion => "Por presentar",
        EstadoPresupuesto.PendienteDecision => "Esperando decisión",
        _ => Estado.ToString()
    };
    public byte[] Version { get; set; } = [];
    public string? Observaciones { get; set; }
    public List<ItemPropuestaDto> Items { get; set; } = [];
    public decimal Total => Items.Sum(i => i.Subtotal);
}
