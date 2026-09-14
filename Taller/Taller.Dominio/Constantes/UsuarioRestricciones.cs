namespace Taller.Dominio.Constantes;

/// <summary>
/// Define los límites y restricciones estructurales
/// aplicables a los datos de un usuario.
/// </summary>
public static class UsuarioRestricciones
{
    /// <summary>
    /// Longitud máxima del nombre utilizado para iniciar sesión.
    /// </summary>
    public const int NombreUsuarioMaximo = 50;

    /// <summary>
    /// Longitud máxima del nombre real del usuario.
    /// </summary>
    public const int NombreMaximo = 100;

    /// <summary>
    /// Longitud máxima del apellido del usuario.
    /// </summary>
    public const int ApellidoMaximo = 100;

    /// <summary>
    /// Longitud mínima admitida para una contraseña.
    /// </summary>
    public const int PasswordMinimo = 8;

    /// <summary>
    /// Longitud máxima admitida para una contraseña.
    /// </summary>
    public const int PasswordMaximo = 100;

    /// <summary>
    /// Longitud máxima de la representación almacenada
    /// de la contraseña procesada mediante hash.
    /// </summary>
    public const int PasswordHashMaximo = 255;
}