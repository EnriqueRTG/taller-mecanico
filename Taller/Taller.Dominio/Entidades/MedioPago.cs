namespace Taller.Dominio.Entidades;
/// <summary>Registro persistente del circuito de liquidación y cobros internos, sin integración fiscal.</summary>
public sealed class MedioPago
{
    public int IdMedioPago { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public bool RequiereReferencia { get; set; }
}
