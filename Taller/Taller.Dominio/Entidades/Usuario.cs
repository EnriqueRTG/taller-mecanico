namespace Taller.Dominio.Entidades;

/// <summary>
/// Representa un usuario en el sistema.
/// </summary>
public sealed class Usuario
{
    public int Id { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public DateTime FechaAlta {  get; set; } = DateTime.Now;
    public int RolId { get; set; }

    /// <summary>
    /// Representa el rol asociado a este usuario.
    /// </summary>
    public Rol Rol { get; set; } = null!;

    /// <summary>
    /// Representa la colección de atenciones asociadas a este usuario.
    /// </summary>
    public ICollection<Atencion> AtencionesRecepcionadas { get; set;} = new List<Atencion>();
}