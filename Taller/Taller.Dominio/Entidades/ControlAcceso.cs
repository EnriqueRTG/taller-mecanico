namespace Taller.Dominio.Entidades;
/// <summary>Ventana de intentos compartida entre estaciones. La clave es SHA-256 del nombre normalizado.</summary>
public sealed class ControlAcceso
{
    public string Clave { get; set; } = "";
    public int Intentos { get; set; }
    public DateTimeOffset VentanaUtc { get; set; }
    public DateTimeOffset? BloqueadoHastaUtc { get; set; }
}

/// <summary>Evidencia de autenticación; no contiene contraseñas, hashes de contraseñas ni cadena de conexión.</summary>
public sealed class EventoAcceso
{
    public long Id { get; set; }
    public string Clave { get; set; } = "";
    public int? IdUsuario { get; set; }
    public bool Correcto { get; set; }
    public DateTimeOffset FechaUtc { get; set; }
}
