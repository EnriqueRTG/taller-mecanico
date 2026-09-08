using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Abstracciones.Seguridad;
using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Servicios;

public sealed class UsuarioServicio
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly IRolRepositorio _rolRepositorio;
    private readonly IPasswordHasher _passwordHasher;

    public UsuarioServicio(IUsuarioRepositorio usuarioRepositorio, IRolRepositorio rolRepositorio, IPasswordHasher passwordHasher)
    {
        _usuarioRepositorio = usuarioRepositorio;
        _rolRepositorio = rolRepositorio;
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

    public async Task<Usuario> CrearAsync(
    string nombreUsuario,
    string password,
    string nombre,
    string apellido,
    int rolId)
    {
        nombreUsuario = NormalizarObligatorio(
            nombreUsuario,
            nameof(nombreUsuario),
            "El nombre de usuario es obligatorio.");

        password = NormalizarObligatorio(
            password,
            nameof(password),
            "La contraseña es obligatoria.");

        nombre = NormalizarObligatorio(
            nombre,
            nameof(nombre),
            "El nombre es obligatorio.");

        apellido = NormalizarObligatorio(
            apellido,
            nameof(apellido),
            "El apellido es obligatorio.");

        if (rolId <= 0)
        {
            throw new ArgumentException(
                "Debe seleccionar un rol válido.",
                nameof(rolId));
        }

        var rol = await _rolRepositorio.ObtenerPorIdAsync(rolId);

        if (rol is null)
        {
            throw new InvalidOperationException(
                "El rol seleccionado no existe.");
        }

        if (!rol.Activo)
        {
            throw new InvalidOperationException(
                "El rol seleccionado se encuentra inactivo.");
        }

        if (await _usuarioRepositorio.ExisteNombreUsuarioAsync(nombreUsuario))
        {
            throw new InvalidOperationException(
                "El nombre de usuario ya se encuentra registrado.");
        }

        var usuario = new Usuario
        {
            NombreUsuario = nombreUsuario,
            PasswordHash = _passwordHasher.Hash(password),
            Nombre = nombre,
            Apellido = apellido,
            RolId = rolId,
            Activo = true,
            FechaAlta = DateTime.Now
        };

        await _usuarioRepositorio.AgregarAsync(usuario);

        return usuario;
    }

    private static string NormalizarObligatorio(
    string? valor,
    string nombreParametro,
    string mensaje)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ArgumentException(
                mensaje,
                nombreParametro);
        }

        return valor.Trim();
    }

}


