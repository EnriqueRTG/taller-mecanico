namespace Taller.Dominio.Entidades
{
    /// <summary>
    /// Usuario del sistema. La contraseña se guarda como hash (BCrypt) en otra capa.
    /// </summary>
    public sealed class Usuario
    {
        public long Id { get; init; }
        public string Username { get; set; } = null!;
        public string PassHash { get; set; } = null!;
        public string Rol { get; set; } = "Recepcion"; // Admin/Recepcion/Tecnico
        public bool Activo { get; set; } = true;
        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;
    }
}
