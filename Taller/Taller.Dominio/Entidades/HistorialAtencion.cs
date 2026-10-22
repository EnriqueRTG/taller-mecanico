using Taller.Dominio.Enumeraciones;

namespace Taller.Dominio.Entidades;

/// <summary>
/// Conserva un evento del ciclo de vida de una atención.
/// </summary>
/// <remarks>
/// Los servicios generan estos registros junto con la operación
/// correspondiente. No se editan ni eliminan ordinariamente.
/// </remarks>
public sealed class HistorialAtencion
{
    public int IdHistorialAtencion { get; set; }
    public int IdAtencion { get; set; }

    /// <summary>
    /// Usuario que realizó la acción registrada.
    /// </summary>
    public int IdUsuario { get; set; }
    public int? IdDiagnostico { get; set; }
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Identifica la operación realizada sobre la atención.
    /// Ej: AtencionAbierta, ActividadTomada, ActividadLiberada
    /// </summary>
    public string Accion { get; set; } = null!;

    /// <summary>
    /// Estado previo. Es null únicamente en la apertura.
    /// </summary>
    public EstadoAtencion? EstadoAnterior { get; set; }

    public EstadoAtencion EstadoNuevo { get; set; }

    /// <summary>
    /// Motivo o información complementaria de la operación.
    /// </summary>
    public string? Observaciones { get; set; }

    public Atencion Atencion { get; set; } = null!;
    public Usuario Usuario { get; set; } = null!;
    public Diagnostico? Diagnostico { get; set; }
}
