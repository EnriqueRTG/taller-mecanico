using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Cobros;
/// <summary>Contrato de datos para cobros, independiente de los controles de presentación.</summary>
public sealed class RegistrarPagoDto
{
    public Guid IdOperacion { get; set; } = Guid.NewGuid();
    public int IdComprobante { get; set; }
    public byte[] Version { get; set; } = [];
    public List<PagoMedioDto> Medios { get; set; } = [];
}
