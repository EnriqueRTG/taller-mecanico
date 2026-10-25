using Taller.Dominio.Enumeraciones;
namespace Taller.Dominio.Entidades;
/// <summary>Pedido individual conservado en el expediente de una visita al taller.</summary>
public sealed class SolicitudAtencion
{
    public int IdSolicitudAtencion { get; set; }
    public int IdAtencion { get; set; }
    public int Orden { get; set; }
    public TipoSolicitudIngreso Tipo { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public Atencion Atencion { get; set; } = null!;
}
