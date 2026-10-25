namespace Taller.Aplicacion.DTOs.Cobros;
/// <summary>Datos mínimos de retiro. El resultado técnico y la cuenta se verifican en el expediente.</summary>
public sealed class RegistrarEntregaDto
{
    public int IdAtencion { get; init; }
    public byte[] Version { get; init; } = [];
    public string PersonaRetira { get; init; } = "";
    public bool ConformidadRecepcion { get; init; }
    public string? Observaciones { get; init; }
}
