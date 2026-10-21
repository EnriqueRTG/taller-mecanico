namespace Taller.Aplicacion.DTOs.Atenciones;

/// <summary>
/// Solicita confirmar el ingreso de una atención
/// con los servicios autorizados por el cliente.
/// </summary>
public sealed class ConfirmarIngresoDto
{
    public int IdAtencion { get; init; }

    public byte[] VersionAtencion { get; init; } = [];

    /// <summary>
    /// Indica que Recepción registró la autorización del cliente
    /// para el conjunto de servicios e importes seleccionados.
    /// </summary>
    public bool ClienteAutoriza { get; init; }

    public List<ServicioIngresoSeleccionadoDto> Servicios { get; init; } = [];
}