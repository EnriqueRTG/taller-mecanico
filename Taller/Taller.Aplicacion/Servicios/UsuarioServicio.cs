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
    /// <param name="usuarioRepositorio">
    /// Repositorio utilizado para consultar y almacenar usuarios.
    /// </param>
    /// <param name="rolRepositorio">
    /// Repositorio utilizado para consultar los roles registrados.
    /// </param>
    /// <param name="passwordHasher">
    /// Componente utilizado para generar hashes seguros
    /// de las contraseñas.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Se produce cuando alguna dependencia requerida es nula.
    /// </exception>
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
    /// <returns>
    /// Lista de roles cuya identificación, estado y nombre
    /// coinciden con la configuración de la aplicación.
    /// </returns>
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
    /// <param name="rol">
    /// Rol que debe comprobarse.
    /// </param>
    /// <returns>
    /// <see langword="true"/> cuando el rol está activo y su
    /// identificador y nombre son válidos; de lo contrario,
    /// <see langword="false"/>.
    /// </returns>
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
    /// <param name="usuarioId">
    /// Identificador del usuario que será modificado.
    /// </param>
    /// <param name="nombre">
    /// Nuevo nombre del usuario.
    /// </param>
    /// <param name="apellido">
    /// Nuevo apellido del usuario.
    /// </param>
    /// <param name="rolId">
    /// Identificador del nuevo rol.
    /// </param>
    /// <param name="usuarioEjecutorId">
    /// Identificador del administrador que realiza la operación.
    /// </param>
    /// <remarks>
    /// Esta operación no modifica las credenciales, el estado,
    /// la fecha de alta ni el identificador del usuario.
    ///
    /// Solamente un administrador activo puede realizarla.
    /// Un administrador no puede quitarse a sí mismo el rol
    /// de Administrador.
    /// </remarks>
    /// <exception cref="ValidacionException">
    /// Se produce cuando alguno de los datos recibidos no es válido.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Se produce cuando el usuario o el rol no existen, están
    /// inactivos o no cumplen las reglas de la aplicación.
    /// </exception>
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
            $"El nombre no puede superar " +
            $"{UsuarioRestricciones.NombreMaximo} caracteres.");

        apellido = NormalizarObligatorio(
            apellido,
            UsuarioRestricciones.ApellidoMaximo,
            nameof(apellido),
            "El apellido es obligatorio.",
            $"El apellido no puede superar " +
            $"{UsuarioRestricciones.ApellidoMaximo} caracteres.");

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

        usuario.Nombre = nombre;
        usuario.Apellido = apellido;
        usuario.RolId = rolId;

        await _usuarioRepositorio.ActualizarAsync(usuario);
    }

    /// <summary>
    /// Registra un nuevo usuario después de validar sus datos,
    /// el rol seleccionado y la disponibilidad del nombre de acceso.
    /// </summary>
    /// <param name="nombreUsuario">
    /// Nombre que utilizará el usuario para iniciar sesión.
    /// </param>
    /// <param name="password">
    /// Contraseña original que será validada y convertida
    /// en un hash seguro.
    /// </param>
    /// <param name="nombre">
    /// Nombre de la persona asociada a la cuenta.
    /// </param>
    /// <param name="apellido">
    /// Apellido de la persona asociada a la cuenta.
    /// </param>
    /// <param name="rolId">
    /// Identificador del rol que se asignará al usuario.
    /// </param>
    /// <returns>
    /// El usuario creado y almacenado correctamente.
    /// </returns>
    /// <exception cref="ValidacionException">
    /// Se produce cuando algún dato obligatorio está vacío,
    /// excede la longitud admitida, la contraseña no cumple
    /// las restricciones o el identificador del rol no es válido.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Se produce cuando el rol no existe, se encuentra inactivo,
    /// está incorrectamente configurado o el nombre de usuario
    /// ya se encuentra registrado.
    /// </exception>
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
                $"El nombre no puede superar " +
                $"{UsuarioRestricciones.NombreMaximo} caracteres.");

        string apellidoNormalizado =
            NormalizarObligatorio(
                apellido,
                UsuarioRestricciones.ApellidoMaximo,
                nameof(apellido),
                "El apellido es obligatorio.",
                $"El apellido no puede superar " +
                $"{UsuarioRestricciones.ApellidoMaximo} caracteres.");

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
}


