using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.DTOs.Presupuestos;
/// <summary>Contrato de datos para presupuestos, independiente de los controles de presentación.</summary>
public sealed class PresupuestoGestionDto
{
    public bool EsAdicional { get; set; }
    public bool EsInspeccion { get; set; }
    public int IdPresupuesto { get; set; }
    public int IdAtencion { get; set; }
    public EstadoPresupuesto Estado { get; set; }
    public bool Vigente { get; set; }
    public byte[] Version { get; set; } = [];
    public string Resumen { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public List<DecisionItemDto> Items { get; set; } = [];
}
