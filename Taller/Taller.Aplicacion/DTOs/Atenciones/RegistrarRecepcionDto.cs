using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Atenciones;
/// <summary>Datos de recepción: no registra cargos, diagnóstico ni autorización de reparación.</summary>
public sealed class RegistrarRecepcionDto
{
    public int IdCliente { get; set; }
    public int IdVehiculo { get; set; }
    public int IdUsuarioRecepcion { get; set; }
    public int? Kilometraje { get; set; }
    public string? ObservacionesRecepcion { get; set; }
    public List<SolicitudIngresoDto> Solicitudes { get; set; } = [];
}
