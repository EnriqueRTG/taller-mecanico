using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Abstracciones.Seguridad;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Aplicacion.Excepciones;

namespace Taller.Aplicacion.Servicios;

/// <summary>
/// Proporciona las operaciones de negocio relacionadas
/// con la gestión de usuarios.
/// </summary>
public sealed class UsuarioServicio
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly IRolRepositorio _rolRepositorio;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de usuarios.
    /// </summary>
    public UsuarioServicio(
        IUsuarioRepositorio usuarioRepositorio,
        IRolRepositorio rolRepositorio,
        IPasswordHasher passwordHasher)
    {
        _usuarioRepositorio =
            usuarioRepositorio
            ?? throw new ArgumentNullException(
                nameof(usuarioRepositorio));

        _rolRepositorio =
            rolRepositorio
            ?? throw new ArgumentNullException(
                nameof(rolRepositorio));

        _passwordHasher =
            passwordHasher
            ?? throw new ArgumentNullException(
                nameof(passwordHasher));
    }

    /// <summary>
    /// Obtiene todos los usuarios registrados.
    /// </summary>
    public async Task<List<Usuario>> ListarAsync()
    {
        return await _usuarioRepositorio.ListarAsync();
    }

    /// <summary>
    /// Obtiene los roles activos que pueden asignarse
    /// a los usuarios del sistema.
    /// </summary>
    public async Task<List<Rol>> ListarRolesDisponiblesAsync()
    {
        List<Rol> rolesRegistrados =
            await _rolRepositorio.ListarActivosAsync();

        return rolesRegistrados
            .Where(EsRolDisponible)
            .OrderBy(rol => rol.Id)
            .ToList();
    }

    /// <summary>
    /// Comprueba que un rol coincida con uno de los roles
    /// habilitados por la aplicación.
    /// </summary>
    private static bool EsRolDisponible(
        Rol rol)
    {
        if (!rol.Activo
            || !RolesSistema.EsRolValido(rol.Id))
        {
            return false;
        }

        string? nombreEsperado =
            RolesSistema.ObtenerNombreEsperado(rol.Id);

        return nombreEsperado is not null
            && !string.IsNullOrWhiteSpace(rol.Nombre)
            && string.Equals(
                rol.Nombre,
                nombreEsperado,
                StringComparison.Ordinal);
    }

    /// <summary>
    /// Obtiene un usuario mediante su identificador.
    /// </summary>
    public async Task<Usuario?> ObtenerPorIdAsync(
        int id)
    {
        ValidarId(id);

        return await _usuarioRepositorio.ObtenerPorIdAsync(id);
    }

    /// <summary>
    /// Actualiza los datos personales y el rol de un usuario existente.
    /// </summary>
    public async Task ActualizarDatosAsync(
        int usuarioId,
        string? nombre,
        string? apellido,
        int rolId,
        int usuarioEjecutorId)
    {
        ValidarId(usuarioId);
        ValidarId(usuarioEjecutorId);

        nombre = NormalizarObligatorio(
            nombre,
            UsuarioRestricciones.NombreMaximo,
            nameof(nombre),
            "El nombre es obligatorio.",
            $"El nombre no puede superar {UsuarioRestricciones.NombreMaximo} caracteres.");

        ValidarTextoSoloLetras(nombre, nameof(nombre), "El nombre solo debe contener letras.");

        apellido = NormalizarObligatorio(
            apellido,
            UsuarioRestricciones.ApellidoMaximo,
            nameof(apellido),
            "El apellido es obligatorio.",
            $"El apellido no puede superar {UsuarioRestricciones.ApellidoMaximo} caracteres.");

        ValidarTextoSoloLetras(apellido, nameof(apellido), "El apellido solo debe contener letras.");

        if (!RolesSistema.EsRolValido(rolId))
        {
            throw new ValidacionException(
                "Debe seleccionar un rol válido.",
                nameof(rolId));
        }

        Usuario? usuarioEjecutor =
            await _usuarioRepositorio.ObtenerPorIdAsync(
                usuarioEjecutorId);

        if (usuarioEjecutor is null)
        {
            throw new InvalidOperationException(
                "El usuario que intenta realizar la operación no existe.");
        }

        if (!usuarioEjecutor.Activo)
        {
            throw new InvalidOperationException(
                "El usuario que intenta realizar la operación está inactivo.");
        }

        if (usuarioEjecutor.RolId != RolesSistema.AdministradorId)
        {
            throw new InvalidOperationException(
                "Solamente un administrador puede modificar usuarios.");
        }

        Usuario? usuario =
            await _usuarioRepositorio.ObtenerPorIdAsync(
                usuarioId);

        if (usuario is null)
        {
            throw new InvalidOperationException(
                "El usuario que desea modificar no existe.");
        }

        if (usuario.Id == usuarioEjecutor.Id
            && rolId != RolesSistema.AdministradorId)
        {
            throw new InvalidOperationException(
                "El administrador no puede quitarse a sí mismo " +
                "el rol de Administrador.");
        }

        Rol? rol =
            await _rolRepositorio.ObtenerPorIdAsync(
                rolId);

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

        string nombreRolEsperado =
            RolesSistema.ObtenerNombreEsperado(rolId)
            ?? throw new InvalidOperationException(
                "La configuración del rol no es válida.");

        if (string.IsNullOrWhiteSpace(rol.Nombre)
            || !string.Equals(
                rol.Nombre,
                nombreRolEsperado,
                StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "La configuración del rol seleccionado no es válida.");
        }

        bool modificaDatos =
            !string.Equals(
                usuario.Nombre,
                nombre,
                StringComparison.Ordinal)
            || !string.Equals(
                usuario.Apellido,
                apellido,
                StringComparison.Ordinal)
            || usuario.RolId != rolId;

        if (!modificaDatos)
        {
            throw new ValidacionException(
                "No se realizaron cambios en los datos del usuario.");
        }

        await _usuarioRepositorio.ActualizarDatosAsync(
            usuarioId,
            nombre,
            apellido,
            rolId);
    }

    /// <summary>
    /// Actualiza las credenciales de acceso de un usuario.
    /// </summary>
    public async Task ActualizarCredencialesAsync(
        int usuarioId,
        string? nombreUsuario,
        string? nuevaPassword,
        string? confirmarPassword,
        int usuarioEjecutorId)
    {
        ValidarId(usuarioId);
        ValidarId(usuarioEjecutorId);

        string nombreUsuarioNormalizado =
            NormalizarObligatorio(
                nombreUsuario,
                UsuarioRestricciones.NombreUsuarioMaximo,
                nameof(nombreUsuario),
                "El nombre de usuario es obligatorio.",
                $"El nombre de usuario no puede superar " +
                $"{UsuarioRestricciones.NombreUsuarioMaximo} caracteres.");

        if (string.IsNullOrEmpty(nuevaPassword)
            && !string.IsNullOrEmpty(confirmarPassword))
        {
            throw new ValidacionException(
                "Ingrese la nueva contraseña o deje ambos campos vacíos.",
                nameof(nuevaPassword));
        }

        Usuario? usuarioEjecutor =
            await _usuarioRepositorio.ObtenerPorIdAsync(
                usuarioEjecutorId);

        if (usuarioEjecutor is null)
        {
            throw new InvalidOperationException(
                "El usuario que intenta realizar la operación no existe.");
        }

        if (!usuarioEjecutor.Activo)
        {
            throw new InvalidOperationException(
                "El usuario que intenta realizar la operación está inactivo.");
        }

        if (usuarioEjecutor.RolId != RolesSistema.AdministradorId)
        {
            throw new InvalidOperationException(
                "Solamente un administrador puede gestionar credenciales.");
        }

        Usuario? usuario =
            await _usuarioRepositorio.ObtenerPorIdAsync(usuarioId);

        if (usuario is null)
        {
            throw new InvalidOperationException(
                "El usuario cuyas credenciales desea modificar no existe.");
        }

        if (usuario.Id == usuarioEjecutor.Id)
        {
            throw new InvalidOperationException(
                "El administrador no puede modificar sus propias " +
                "credenciales desde la gestión de usuarios.");
        }

        bool cambiaNombreUsuario =
            !string.Equals(
                usuario.NombreUsuario,
                nombreUsuarioNormalizado,
                StringComparison.Ordinal);

        bool cambiaPassword =
            !string.IsNullOrEmpty(nuevaPassword);

        if (!cambiaNombreUsuario && !cambiaPassword)
        {
            throw new ValidacionException(
                "No se realizaron cambios en las credenciales.");
        }

        if (cambiaNombreUsuario)
        {
            bool solamenteCambiaMayusculas =
                string.Equals(
                    usuario.NombreUsuario,
                    nombreUsuarioNormalizado,
                    StringComparison.OrdinalIgnoreCase);

            if (!solamenteCambiaMayusculas)
            {
                bool nombreUsuarioExistente =
                    await _usuarioRepositorio
                        .ExisteNombreUsuarioAsync(
                            nombreUsuarioNormalizado);

                if (nombreUsuarioExistente)
                {
                    throw new InvalidOperationException(
                        "El nombre de usuario ya se encuentra registrado.");
                }
            }
        }

        string? passwordHash = null;

        if (cambiaPassword)
        {
            string passwordValidado =
                ValidarPassword(nuevaPassword);

            if (!string.Equals(
                passwordValidado,
                confirmarPassword,
                StringComparison.Ordinal))
            {
                throw new ValidacionException(
                    "Las contraseñas ingresadas no coinciden.",
                    nameof(confirmarPassword));
            }

            passwordHash =
                _passwordHasher.Hash(passwordValidado);

            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new InvalidOperationException(
                    "No fue posible proteger la nueva contraseña.");
            }
        }

        await _usuarioRepositorio.ActualizarCredencialesAsync(
            usuarioId,
            nombreUsuarioNormalizado,
            passwordHash);
    }

    /// <summary>
    /// Cambia el estado de acceso de un usuario registrado.
    /// </summary>
    public async Task CambiarEstadoAsync(
        int usuarioId,
        bool habilitar,
        int usuarioEjecutorId)
    {
        ValidarId(usuarioId);
        ValidarId(usuarioEjecutorId);

        Usuario? ejecutor =
            await _usuarioRepositorio.ObtenerPorIdAsync(
                usuarioEjecutorId);

        if (ejecutor is null || !ejecutor.Activo)
        {
            throw new InvalidOperationException(
                "La cuenta del administrador que realiza " +
                "la operación no está disponible.");
        }

        if (ejecutor.RolId != RolesSistema.AdministradorId)
        {
            throw new InvalidOperationException(
                "Solamente un administrador puede cambiar " +
                "el estado de los usuarios.");
        }

        Usuario? usuario =
            await _usuarioRepositorio.ObtenerPorIdAsync(
                usuarioId);

        if (usuario is null)
        {
            throw new InvalidOperationException(
                "El usuario seleccionado ya no existe.");
        }

        if (usuario.Id == ejecutor.Id && !habilitar)
        {
            throw new InvalidOperationException(
                "El administrador no puede deshabilitar " +
                "su propia cuenta.");
        }

        if (usuario.Activo == habilitar)
        {
            throw new InvalidOperationException(
                habilitar
                    ? "El usuario ya se encuentra habilitado."
                    : "El usuario ya se encuentra deshabilitado.");
        }

        await _usuarioRepositorio.CambiarEstadoAsync(
            usuarioId,
            habilitar);
    }

    /// <summary>
    /// Registra un nuevo usuario después de validar sus datos,
    /// el rol seleccionado y la disponibilidad del nombre de acceso.
    /// </summary>
    public async Task<Usuario> CrearAsync(
        string? nombreUsuario,
        string? password,
        string? nombre,
        string? apellido,
        int rolId)
    {
        string nombreUsuarioNormalizado =
            NormalizarObligatorio(
                nombreUsuario,
                UsuarioRestricciones.NombreUsuarioMaximo,
                nameof(nombreUsuario),
                "El nombre de usuario es obligatorio.",
                $"El nombre de usuario no puede superar " +
                $"{UsuarioRestricciones.NombreUsuarioMaximo} caracteres.");

        string passwordValidado =
            ValidarPassword(password);

        string nombreNormalizado =
            NormalizarObligatorio(
                nombre,
                UsuarioRestricciones.NombreMaximo,
                nameof(nombre),
                "El nombre es obligatorio.",
                $"El nombre no puede superar {UsuarioRestricciones.NombreMaximo} caracteres.");

        ValidarTextoSoloLetras(nombreNormalizado, nameof(nombre), "El nombre solo debe contener letras.");

        string apellidoNormalizado =
            NormalizarObligatorio(
                apellido,
                UsuarioRestricciones.ApellidoMaximo,
                nameof(apellido),
                "El apellido es obligatorio.",
                $"El apellido no puede superar {UsuarioRestricciones.ApellidoMaximo} caracteres.");

        ValidarTextoSoloLetras(apellidoNormalizado, nameof(apellido), "El apellido solo debe contener letras.");

        if (!RolesSistema.EsRolValido(rolId))
        {
            throw new ValidacionException(
                "Debe seleccionar un rol válido.",
                nameof(rolId));
        }

        Rol? rol =
            await _rolRepositorio.ObtenerPorIdAsync(rolId);

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

        if (!EsRolDisponible(rol))
        {
            throw new InvalidOperationException(
                "La configuración del rol seleccionado no es válida.");
        }

        bool nombreUsuarioExistente =
            await _usuarioRepositorio
                .ExisteNombreUsuarioAsync(
                    nombreUsuarioNormalizado);

        if (nombreUsuarioExistente)
        {
            throw new InvalidOperationException(
                "El nombre de usuario ya se encuentra registrado.");
        }

        string passwordHash =
            _passwordHasher.Hash(passwordValidado);

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new InvalidOperationException(
                "No fue posible proteger la contraseña del usuario.");
        }

        var usuario = new Usuario
        {
            NombreUsuario = nombreUsuarioNormalizado,
            PasswordHash = passwordHash,
            Nombre = nombreNormalizado,
            Apellido = apellidoNormalizado,
            RolId = rolId,
            Activo = true,
            FechaAlta = DateTime.Now
        };

        await _usuarioRepositorio.AgregarAsync(usuario);

        return usuario;
    }

    /// <summary>
    /// Valida una contraseña sin modificar su contenido.
    /// </summary>
    private static string ValidarPassword(
        string? password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ValidacionException(
                "La contraseña es obligatoria.",
                nameof(password));
        }

        if (password.Length <
            UsuarioRestricciones.PasswordMinimo)
        {
            throw new ValidacionException(
                $"La contraseña debe contener al menos " +
                $"{UsuarioRestricciones.PasswordMinimo} caracteres.",
                nameof(password));
        }

        if (password.Length >
            UsuarioRestricciones.PasswordMaximo)
        {
            throw new ValidacionException(
                $"La contraseña no puede superar " +
                $"{UsuarioRestricciones.PasswordMaximo} caracteres.",
                nameof(password));
        }

        return password;
    }

    /// <summary>
    /// Valida que un identificador de usuario sea mayor que cero.
    /// </summary>
    private static void ValidarId(
        int id)
    {
        if (id <= 0)
        {
            throw new ValidacionException(
                "El identificador del usuario no es válido.",
                nameof(id));
        }
    }

    /// <summary>
    /// Normaliza y valida un texto obligatorio.
    /// </summary>
    private static string NormalizarObligatorio(
        string? valor,
        int longitudMaxima,
        string nombreParametro,
        string mensajeObligatorio,
        string mensajeLongitud)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new ValidacionException(
                mensajeObligatorio,
                nombreParametro);
        }

        string valorNormalizado = valor.Trim();

        if (valorNormalizado.Length > longitudMaxima)
        {
            throw new ValidacionException(
                mensajeLongitud,
                nombreParametro);
        }

        return valorNormalizado;
    }

    /// <summary>
    /// Comprueba que una cadena contenga únicamente letras y espacios.
    /// </summary>
    private static void ValidarTextoSoloLetras(
        string valor,
        string nombreParametro,
        string mensajeError)
    {
        foreach (char c in valor)
        {
            if (!char.IsLetter(c) && c != ' ')
            {
                throw new ValidacionException(mensajeError, nombreParametro);
            }
        }
    }
}