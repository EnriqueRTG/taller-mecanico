using Taller.Dominio.Enumeraciones;
namespace Taller.Aplicacion.DTOs.Atenciones;
/// <summary>Solicitud individual con identidad persistida al consultar el expediente.</summary>
public sealed class SolicitudIngresoDto
{
    public int IdSolicitud { get; set; }
    public TipoSolicitudIngreso Tipo { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string Etiqueta => $"{(Tipo == TipoSolicitudIngreso.TrabajoSolicitado ? "Trabajo solicitado" : "Problema informado")}: {Descripcion}";
}
