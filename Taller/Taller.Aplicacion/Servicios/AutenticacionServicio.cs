using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Abstracciones.Seguridad;
using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Servicios;

/// <summary>
/// Contiene la lógica necesaria para autenticar usuarios
/// dentro del sistema.
/// </summary>
public sealed class AutenticacionServicio
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Inicializa el servicio de autenticación con las dependencias
    /// necesarias para consultar usuarios y verificar contraseñas.
    /// </summary>
    public AutenticacionServicio(IUsuarioRepositorio usuarioRepositorio, IPasswordHasher passwordHasher)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Intenta autenticar un usuario utilizando su nombre de usuario
    /// y contraseña.
    /// </summary>
    public async Task<Usuario?> AutenticarAsync(string nombreUsuario, string password)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(password))
        {
            return null;
        }

        var usuario = await _usuarioRepositorio.ObtenerPorNombreUsuarioAsync(nombreUsuario.Trim());

        if (usuario is null)
            return null;

        if (!usuario.Activo)
            return null;

        var passwordValido = _passwordHasher.Verificar(password, usuario.PasswordHash);

        if (!passwordValido)
            return null;

        return usuario;
    }

}
