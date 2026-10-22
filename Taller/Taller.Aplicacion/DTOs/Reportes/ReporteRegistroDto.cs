namespace Taller.Aplicacion.DTOs.Reportes;
/// <summary>Proyección de lectura; nunca contiene credenciales ni entidades rastreadas.</summary>
public sealed class ReporteRegistroDto
{
    public string Codigo { get; init; } = "";
    public int IdAtencion { get; init; }
    public DateTime Fecha { get; init; }
    public string Descripcion { get; init; } = "";
    public string Responsable { get; init; } = "";
    public string Estado { get; init; } = "";
    public string Grupo { get; init; } = "";
    public decimal? Importe { get; init; }
    public decimal? ImporteAceptado { get; init; }
    public decimal? Cantidad { get; init; }
}
