namespace Taller.Aplicacion.DTOs.Respaldos;
/// <summary>Estado durable de coordinación, independiente de los datos que se pueden restaurar.</summary>
public sealed record EstadoRespaldosDto(string BaseDatos, bool EnMantenimiento, long Generacion, string Detalle, IReadOnlyList<CopiaRespaldoDto> Copias, IReadOnlyList<EventoRespaldoDto> Historial)
{
    /// <summary>Solo habilita el explorador local cuando SQL realmente corre en este Windows.</summary>
    public bool PuedeExplorarCarpetaLocal { get; init; }
    public string EquipoServidor { get; init; } = string.Empty;
}
