namespace Taller.Dominio.Entidades;


/// <summary>
/// Representa un cliente en el sistema.
/// </summary>
public sealed class Cliente
{
    public int IdCliente { get; set; }
    public string Documento { get; set; } = null!;
    public string TipoDocumento { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaAlta { get; set; } = DateTime.Now;
}
