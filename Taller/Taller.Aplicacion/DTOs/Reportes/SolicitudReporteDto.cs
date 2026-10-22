namespace Taller.Aplicacion.DTOs.Reportes;
/// <summary>Período inclusivo; saldos y reservas muestran situación actual y no reconstruyen el pasado.</summary>
public sealed class SolicitudReporteDto
{
    public TipoReporte Tipo { get; init; }
    public DateTime Desde { get; init; }
    public DateTime Hasta { get; init; }
    public string Busqueda { get; init; } = "";
    public string Estado { get; init; } = "Todos";
}
