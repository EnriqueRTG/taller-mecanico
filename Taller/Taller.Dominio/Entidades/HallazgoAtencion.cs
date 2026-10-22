namespace Taller.Dominio.Entidades;
/// <summary>Informe puntual inmutable. Un hallazgo no autoriza trabajos ni genera deuda.</summary>
public sealed class HallazgoAtencion
{
    public int IdHallazgoAtencion { get; set; }
    public int IdAtencion { get; set; }
    public Atencion Atencion { get; set; } = null!;
    public int IdTecnico { get; set; }
    public Usuario Tecnico { get; set; } = null!;
    public DateTime Fecha { get; set; }
    public string Descripcion { get; set; } = "";
    public string Recomendacion { get; set; } = "";
    public bool ImpideContinuar { get; set; }
    public int? IdDetalleAfectado { get; set; }
    public PresupuestoDetalle? DetalleAfectado { get; set; }
}
