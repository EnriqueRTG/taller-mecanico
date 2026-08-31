using Microsoft.AspNetCore.Identity;
using Taller.Aplicacion.Abstracciones.Seguridad;

namespace Taller.Infraestructura.Seguridad;

/// <summary>
/// Implementa el hash y la verificación de contraseñas
/// utilizando el PasswordHasher provisto por .NET.
/// </summary>
public sealed class PasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();

    /// <summary>
    /// Genera un hash seguro a partir de la contraseña recibida.
    /// </summary>
    public string Hash(string password)
    {
        return _hasher.HashPassword(null!, password);
    }

    /// <summary>
    /// Verifica si la contraseña ingresada coincide
    /// con el hash almacenado.
    /// </summary>
    public bool Verificar(string password, string passwordHash)
    {
        var resultado = _hasher.VerifyHashedPassword(
            null!,
            passwordHash,
            password);

        return resultado != PasswordVerificationResult.Failed;
    }
}