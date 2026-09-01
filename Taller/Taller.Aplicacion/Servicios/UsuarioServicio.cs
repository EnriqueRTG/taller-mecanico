using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Abstracciones.Seguridad;
using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Servicios;

public sealed class UsuarioServicio
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly IPasswordHasher _passwordHasher;

    public UsuarioServicio(IUsuarioRepositorio usuarioRepositorio, IPasswordHasher passwordHasher)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _passwordHasher = passwordHasher;
    }

    public async Task<List<Usuario>> ListarAsync()
    {
        return await _usuarioRepositorio.ListarAsync();
    }

    public async Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        return await _usuarioRepositorio.ObtenerPorIdAsync(id);
    }

    public async Task CrearAsync(string nombreUsuario, string password, string nombre, string apellido, int rolId)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario))
            throw new ArgumentException("El nombre de usuario es obligatorio.");

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("La contraseña es obligatoria.");

        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(apellido))
            throw new ArgumentException("El apellido es obligatorio.");

        var existeUsuario = await _usuarioRepositorio.ExisteNombreUsuarioAsync(nombreUsuario);

        if (existeUsuario)
            throw new InvalidOperationException("El nombre de usuario ya se encuentra registrado.");

        var usuario = new Usuario()
        {
            NombreUsuario = nombreUsuario,
            PasswordHash = _passwordHasher.Hash(password),
            Nombre = nombre.Trim(),
            Apellido = apellido.Trim(),
            RolId = rolId,
            Activo = true
        };

        await _usuarioRepositorio.AgregarAsync(usuario);
    }

}


