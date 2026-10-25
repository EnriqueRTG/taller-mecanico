using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Presupuestos;
/// <summary>Contrato de datos para presupuestos, independiente de los controles de presentación.</summary>
public sealed class DecisionPresupuestoDto
{
    public string PersonaDecision { get; set; } = string.Empty;
    public string MedioDecision { get; set; } = string.Empty;
    public int IdPresupuesto { get; set; }
    public byte[] Version { get; set; } = [];
    public List<int> ItemsAceptados { get; set; } = [];
    public string Observaciones { get; set; } = string.Empty;
}
