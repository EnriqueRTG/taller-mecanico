using Taller.Dominio.Enumeraciones;

namespace Taller.Dominio.Entidades;

/// <summary>
/// Representa una atención en el sistema.
/// </summary>
public sealed class Atencion
{
    public int IdAtencion { get; set; }
    public int IdVehiculo { get; set; }
    public int IdCliente { get; set; }
    public int IdUsuarioRecepcion { get; set; }
    public DateTime FechaApertura { get; set; } = DateTime.Now;
    public string MotivoConsulta { get; set; } = null!;
    public EstadoAtencion Estado { get; set; } = EstadoAtencion.Abierta;
    public DateTime? FechaCierre { get; set; }

    /// <summary>
    /// Representa el vehiculo asociado a esta atención.
    /// </summary>
    public Vehiculo Vehiculo { get; set; } = null!;

    /// <summary>
    /// Representa el cliente asociado a esta atención.
    /// </summary>
    public Cliente Cliente { get; set; } = null!;

    /// <summary>
    /// Representa al usuario de recepción que registró esta atención.
    /// </summary>
    public Usuario UsuarioRecepcion { get; set; } = null!;


}
