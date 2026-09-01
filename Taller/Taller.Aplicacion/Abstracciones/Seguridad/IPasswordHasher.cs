
namespace Taller.Aplicacion.Abstracciones.Seguridad;

/// <summary>
/// Define las operaciones necesarias para generar y verificar
/// hashes seguros de contraseñas.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Genera una representación segura de una contraseña
    /// para poder almacenarla en la base de datos.
    /// </summary>
    string Hash(string password);

    /// <summary>
    /// Verifica si una contraseña ingresada corresponde
    /// al hash almacenado.
    /// </summary>
    bool Verificar(string password, string passwordHash);
}