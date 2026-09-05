namespace Taller.Dominio.Entidades;

/// <summary>
/// Representa un rol en el sistema.
/// </summary>
public sealed class Rol
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; } = true;

    /// <summary>
    /// Representa la colección de usuarios asociados a este rol.
    /// </summary>
    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}