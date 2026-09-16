using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Abstracciones.Seguridad;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Servicios;

/// <summary>
/// Proporciona las operaciones necesarias para autenticar
/// usuarios dentro del sistema.
/// </summary>
/// <remarks>
/// Este servicio pertenece a la capa de aplicación y concentra
/// las validaciones indispensables del proceso de autenticación.
///
/// No muestra mensajes ni depende de componentes de WinForms.
/// Cuando las credenciales o el estado de la cuenta no permiten
/// el acceso, devuelve <see langword="null"/> sin revelar cuál
/// condición particular produjo el rechazo.
/// </remarks>
public sealed class AutenticacionServicio
{
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de autenticación.
    /// </summary>
    /// <param name="usuarioRepositorio">
    /// Repositorio utilizado para obtener los usuarios registrados.
    /// </param>
    /// <param name="passwordHasher">
    /// Componente encargado de verificar las contraseñas
    /// almacenadas mediante hash.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Se produce cuando alguna dependencia requerida es nula.
    /// </exception>
    public AutenticacionServicio(
        IUsuarioRepositorio usuarioRepositorio,
        IPasswordHasher passwordHasher)
    {
        _usuarioRepositorio =
            usuarioRepositorio
            ?? throw new ArgumentNullException(
                nameof(usuarioRepositorio));

        _passwordHasher =
            passwordHasher
            ?? throw new ArgumentNullException(
                nameof(passwordHasher));
    }

    /// <summary>
    /// Intenta autenticar un usuario mediante su nombre
    /// de acceso y contraseña.
    /// </summary>
    /// <param name="nombreUsuario">
    /// Nombre utilizado por el usuario para acceder al sistema.
    /// </param>
    /// <param name="password">
    /// Contraseña ingresada en texto sin transformar.
    /// </param>
    /// <returns>
    /// El usuario autenticado cuando las credenciales, la cuenta
    /// y su rol son válidos; en caso contrario,
    /// <see langword="null"/>.
    /// </returns>
    /// <remarks>
    /// El nombre de usuario se normaliza eliminando espacios
    /// exteriores. La contraseña se conserva exactamente como
    /// fue ingresada porque sus espacios podrían formar parte
    /// de la credencial.
    ///
    /// Los errores técnicos producidos por el repositorio o por
    /// el verificador de contraseñas no se capturan aquí. Deben
    /// propagarse hasta la capa de presentación, que mostrará
    /// un mensaje apropiado sin exponer detalles internos.
    /// </remarks>
    public async Task<Usuario?> AutenticarAsync(
        string? nombreUsuario,
        string? password)
    {
        if (!SonCredencialesAdmisibles(
            nombreUsuario,
            password))
        {
            return null;
        }

        string nombreUsuarioNormalizado =
            nombreUsuario!.Trim();

        Usuario? usuario =
            await _usuarioRepositorio
                .ObtenerPorNombreUsuarioAsync(
                    nombreUsuarioNormalizado);

        if (!EsUsuarioHabilitado(usuario)
            || !string.Equals(
            usuario!.NombreUsuario,
             nombreUsuarioNormalizado,
            StringComparison.Ordinal))
        {
            return null;
        }

        if (!EsRolHabilitado(usuario!))
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(
            usuario!.PasswordHash))
        {
            return null;
        }

        bool passwordValido =
            _passwordHasher.Verificar(
                password!,
                usuario.PasswordHash);

        return passwordValido
            ? usuario
            : null;
    }

    /// <summary>
    /// Comprueba que las credenciales posean contenido y respeten
    /// las restricciones admitidas por el sistema.
    /// </summary>
    /// <param name="nombreUsuario">
    /// Nombre de usuario recibido.
    /// </param>
    /// <param name="password">
    /// Contraseña recibida.
    /// </param>
    /// <returns>
    /// <see langword="true"/> cuando las credenciales pueden
    /// procesarse; de lo contrario, <see langword="false"/>.
    /// </returns>
    private static bool SonCredencialesAdmisibles(
        string? nombreUsuario,
        string? password)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario)
            || string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        string nombreUsuarioNormalizado =
            nombreUsuario.Trim();

        if (nombreUsuarioNormalizado.Length >
            UsuarioRestricciones.NombreUsuarioMaximo)
        {
            return false;
        }

        if (password.Length <
                UsuarioRestricciones.PasswordMinimo
            || password.Length >
                UsuarioRestricciones.PasswordMaximo)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Comprueba que el usuario exista y que su cuenta
    /// se encuentre activa.
    /// </summary>
    /// <param name="usuario">
    /// Usuario recuperado desde la persistencia.
    /// </param>
    /// <returns>
    /// <see langword="true"/> cuando el usuario existe y está
    /// habilitado; de lo contrario, <see langword="false"/>.
    /// </returns>
    private static bool EsUsuarioHabilitado(
        Usuario? usuario)
    {
        return usuario is not null
            && usuario.Activo;
    }

    /// <summary>
    /// Comprueba que el usuario tenga asignado uno de los roles
    /// admitidos por la aplicación y que su configuración sea válida.
    /// </summary>
    /// <param name="usuario">
    /// Usuario cuyo rol debe comprobarse.
    /// </param>
    /// <returns>
    /// <see langword="true"/> cuando el rol existe, está activo
    /// y coincide con la configuración esperada; de lo contrario,
    /// <see langword="false"/>.
    /// </returns>
    private static bool EsRolHabilitado(
        Usuario usuario)
    {
        if (!RolesSistema.EsRolValido(usuario.RolId))
        {
            return false;
        }

        if (usuario.Rol is null
            || !usuario.Rol.Activo)
        {
            return false;
        }

        string? nombreRolEsperado =
            RolesSistema.ObtenerNombreEsperado(
                usuario.RolId);

        if (nombreRolEsperado is null
            || string.IsNullOrWhiteSpace(
                usuario.Rol.Nombre))
        {
            return false;
        }

        return string.Equals(
            usuario.Rol.Nombre,
            nombreRolEsperado,
            StringComparison.Ordinal);
    }
}