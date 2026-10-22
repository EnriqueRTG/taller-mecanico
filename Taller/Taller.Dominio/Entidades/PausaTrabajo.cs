namespace Taller.Dominio.Entidades;
/// <summary>Pausa trazable de una tarea o de toda la atención. No altera la autorización ni lo realizado.</summary>
public sealed class PausaTrabajo
{
    public int IdPausaTrabajo { get; set; }
    public int IdAtencion { get; set; }
    public Atencion Atencion { get; set; } = null!;
    public int? IdDetalle { get; set; }
    public PresupuestoDetalle? Detalle { get; set; }
    public int IdUsuario { get; set; }
    public Usuario Usuario { get; set; } = null!;
    public DateTime Fecha { get; set; }
    public string Motivo { get; set; } = "";
    public DateTime? FechaResolucion { get; set; }
    public int? IdTecnicoResolucion { get; set; }
    public Usuario? TecnicoResolucion { get; set; }
    public string? Resolucion { get; set; }
}
