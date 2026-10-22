using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.DTOs.Ejecuciones;
/// <summary>Contrato de datos del seguimiento operativo; independiente de los controles de presentación.</summary>
public sealed class MaterialTrabajoDto
{
    public string Etiqueta => IdDetalle == 0 ? Descripcion : $"P{IdPresupuesto} / Ítem {IdDetalle}: {Descripcion}";
    public int IdDetalle { get; set; }
    public int IdPresupuesto { get; set; }
    public string Descripcion { get; set; } = "";
    public OrigenMaterial Origen { get; set; }
    public decimal Cantidad { get; set; }
    public decimal Recibida { get; set; }
    public bool Conforme { get; set; }
    public string? Observaciones { get; set; }
}
