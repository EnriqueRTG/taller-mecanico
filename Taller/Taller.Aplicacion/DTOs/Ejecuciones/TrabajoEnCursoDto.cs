namespace Taller.Aplicacion.DTOs.Ejecuciones;
/// <summary>Resumen operativo de una atención con trabajos autorizados pendientes de cierre técnico.</summary>
public sealed class TrabajoEnCursoDto
{
    public int IdAtencion { get; init; }
    public string Cliente { get; init; } = "";
    public string Vehiculo { get; init; } = "";
    public string Motivo { get; init; } = "";
    public int? IdTecnico { get; init; }
    public string Tecnico { get; init; } = "Disponible";
    public DateTime Fecha { get; init; }
    public int Total { get; init; }
    public int Completados { get; init; }
    public int EnCurso { get; init; }
    public int Pausas { get; init; }
    public string Progreso => $"{Completados} completados · {EnCurso} en curso · {Total} autorizados";
}
