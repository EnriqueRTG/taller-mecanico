using Taller.Dominio.Enumeraciones;

namespace Taller.Dominio.Entidades;
/// <summary>
/// Conserva las condiciones económicas de un ítem
/// y el resultado de su ejecución.
/// </summary>
public sealed class PresupuestoDetalle
{
    public OrigenMaterial OrigenMaterial { get; set; }
    public decimal CantidadRecibidaCliente { get; set; }
    public bool MaterialClienteConforme { get; set; }
    public string? ObservacionesMaterial { get; set; }
    public int? IdTecnicoMaterial { get; set; }
    public Usuario? TecnicoMaterial { get; set; }
    public DateTime? FechaRecepcionMaterial { get; set; }
    public int IdPresupuestoDetalle { get; set; }
    public int IdPresupuesto { get; set; }
    public int? IdSolicitudAtencion { get; set; }
    public int IdConcepto { get; set; }
    /// <summary>Null sin decisión; false excluye el ítem sin borrarlo del presupuesto original.</summary>
    public bool? AceptadoPorCliente { get; set; }
    /// <summary>
    /// Descripción del concepto al presupuestarlo.
    /// Se conserva aunque posteriormente cambie el catálogo.
    /// </summary>
    public string Descripcion { get; set; } = null!;
    /// <summary>
    /// Cantidad propuesta y, tras la aceptación, autorizada.
    /// </summary>
    public decimal Cantidad { get; set; }
    /// <summary>
    /// Precio unitario acordado en este presupuesto.
    /// No depende de posteriores cambios del precio de referencia.
    /// </summary>
    public decimal PrecioUnitario { get; set; }
    public EstadoEjecucionDetalle EstadoEjecucion { get; set; } = EstadoEjecucionDetalle.NoIniciado;
    /// <summary>
    /// Cantidad efectivamente ejecutada, instalada o consumida.
    /// No puede superar la cantidad autorizada.
    /// </summary>
    public decimal CantidadEjecutada { get; set; }
    /// <summary>
    /// Técnico que registró la última actualización de ejecución.
    /// No representa a todos los posibles colaboradores.
    /// </summary>
    public int? IdTecnicoRegistro { get; set; }
    public DateTime? FechaRegistroEjecucion { get; set; }
    /// <summary>
    /// Explica una ejecución parcial, una omisión
    /// u otra circunstancia relevante del trabajo.
    /// </summary>
    public string? ObservacionesEjecucion { get; set; }
    /// <summary>
    /// Propuesta posterior que plantea modificar o suprimir
    /// la parte pendiente de este ítem.
    /// El cambio requiere la aceptación de esa propuesta.
    /// </summary>
    public int? IdPresupuestoCambio { get; set; }
    public Presupuesto Presupuesto { get; set; } = null!;
    public Concepto Concepto { get; set; } = null!;
    public Usuario? TecnicoRegistro { get; set; }
    public Presupuesto? PresupuestoCambio { get; set; }
}
