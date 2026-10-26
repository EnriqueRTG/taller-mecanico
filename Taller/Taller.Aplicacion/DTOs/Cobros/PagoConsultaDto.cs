using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Cobros;
/// <summary>Contrato de datos para cobros, independiente de los controles de presentación.</summary>
public sealed class PagoConsultaDto
{
    public int IdPago { get; set; }
    public string Numero => $"REC-{IdPago:D8}";
    public DateTime Fecha { get; set; }
    public decimal Importe { get; set; }
    public decimal SaldoPosterior { get; set; }
    public bool Anulado { get; set; }
    public string? MotivoAnulacion { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public List<PagoMedioDto> DetalleMedios { get; set; } = [];
    public string Medios { get; set; } = string.Empty;
}
