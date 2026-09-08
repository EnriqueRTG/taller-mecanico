using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Servicios;

/// <summary>
/// Mantiene la información del usuario autenticado
/// durante la ejecución de la aplicación.
/// </summary>
public sealed class SesionUsuario
{
    /// <summary>
    /// Usuario actualmente autenticado.
    /// </summary>
    public Usuario? UsuarioActual { get; private set; }

    /// <summary>
    /// Indica si existe un usuario autenticado.
    /// </summary>
    public bool EstaAutenticado => UsuarioActual is not null;

    /// <summary>
    /// Establece el usuario autenticado.
    /// </summary>
    public void Iniciar(Usuario usuario)
    {
        ArgumentNullException.ThrowIfNull(usuario);

        UsuarioActual = usuario;
    }

    /// <summary>
    /// Finaliza la sesión actual.
    /// </summary>
    public void Cerrar()
    {
        UsuarioActual = null;
    }
}