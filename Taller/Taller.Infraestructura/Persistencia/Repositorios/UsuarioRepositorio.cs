using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Excepciones;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Repositorios;

// <summary>
/// Implementa las operaciones de persistencia relacionadas con los
/// usuarios mediante Entity Framework Core y procedimientos almacenados
/// de SQL Server.
/// </summary>
/// <remarks>
/// Las operaciones de consulta se realizan mediante Entity Framework Core.
/// Las operaciones de creación y modificación se delegan en procedimientos
/// almacenados.
///
/// Este repositorio es responsable del acceso a datos, de construir los
/// parámetros SQL y de traducir errores conocidos de SQL Server a
/// excepciones comprensibles para la capa de aplicación.
///
/// Las validaciones de entrada y las reglas de negocio deben realizarse
/// principalmente en los servicios de aplicación. Las restricciones
/// críticas también pueden estar protegidas en la base de datos para
/// preservar su integridad.
/// </remarks>
public sealed class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly TallerDbContext _contexto;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio.
    /// </summary>
    /// <param name="contexto">
    /// Contexto de Entity Framework Core utilizado para acceder
    /// a la base de datos.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Se produce cuando el contexto recibido es nulo.
    /// </exception>
    public UsuarioRepositorio(
        TallerDbContext contexto)
    {
        _contexto =
            contexto
            ?? throw new ArgumentNullException(
                nameof(contexto));
    }

    /// <summary>
    /// Obtiene un usuario mediante su identificador,
    /// incluyendo la información de su rol.
    /// </summary>
    /// <param name="id">
    /// Identificador del usuario buscado.
    /// </param>
    /// <returns>
    /// El usuario encontrado o <see langword="null"/>
    /// cuando no existe.
    /// </returns>
    public async Task<Usuario?> ObtenerPorIdAsync(
        int id)
    {
        return await _contexto.Usuarios
            .AsNoTracking()
            .Include(usuario => usuario.Rol)
            .SingleOrDefaultAsync(
                usuario => usuario.Id == id);
    }

    /// <summary>
    /// Obtiene un usuario mediante su nombre de acceso,
    /// incluyendo la información necesaria de su rol.
    /// </summary>
    /// <param name="nombreUsuario">
    /// Nombre de usuario que debe buscarse.
    /// </param>
    /// <returns>
    /// El usuario encontrado o <see langword="null"/>
    /// cuando no existe.
    /// </returns>
    /// <remarks>
    /// Esta operación se utiliza durante la autenticación.
    /// La inclusión explícita del rol permite verificar que
    /// se encuentre activo y correctamente configurado.
    /// </remarks>
    public async Task<Usuario?> ObtenerPorNombreUsuarioAsync(
        string nombreUsuario)
    {
        return await _contexto.Usuarios
            .AsNoTracking()
            .Include(usuario => usuario.Rol)
            .SingleOrDefaultAsync(
                usuario =>
                    usuario.NombreUsuario == nombreUsuario);
    }

    /// <summary>
    /// Obtiene todos los usuarios junto con sus roles,
    /// ordenados por apellido y nombre.
    /// </summary>
    /// <returns>
    /// Lista de usuarios registrados.
    /// </returns>
    public async Task<List<Usuario>> ListarAsync()
    {
        return await _contexto.Usuarios
            .AsNoTracking()
            .Include(usuario => usuario.Rol)
            .OrderBy(usuario => usuario.Apellido)
            .ThenBy(usuario => usuario.Nombre)
            .ToListAsync();
    }

    /// <summary>
    /// Determina si existe un usuario registrado con el nombre
    /// de acceso especificado.
    /// </summary>
    /// <param name="nombreUsuario">
    /// Nombre de usuario que debe buscarse.
    /// </param>
    /// <returns>
    /// <see langword="true"/> cuando existe una coincidencia;
    /// de lo contrario, <see langword="false"/>.
    /// </returns>
    public async Task<bool> ExisteNombreUsuarioAsync(
        string nombreUsuario)
    {
        return await _contexto.Usuarios
            .AsNoTracking()
            .AnyAsync(
                usuario =>
                    usuario.NombreUsuario == nombreUsuario);
    }

    /// <summary>
    /// Registra un nuevo usuario mediante el procedimiento almacenado
    /// <c>dbo.Usuario_Crear</c>.
    /// </summary>
    /// <param name="usuario">
    /// Usuario que contiene el nombre de acceso, el hash de la contraseña,
    /// los datos personales, el rol y la fecha de alta que deben almacenarse.
    /// </param>
    /// <remarks>
    /// El procedimiento crea al usuario en estado activo y devuelve el
    /// identificador generado mediante el parámetro de salida <c>@Id</c>.
    /// Una vez finalizada la operación, dicho identificador se asigna a
    /// <see cref="Usuario.Id"/>.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Se produce cuando <paramref name="usuario"/> es
    /// <see langword="null"/>.
    /// </exception>
    /// <exception cref="ValidacionException">
    /// Se produce cuando ya existe un usuario con el mismo nombre de acceso.
    /// </exception>
    public async Task AgregarAsync(
        Usuario usuario)
    {
        ArgumentNullException.ThrowIfNull(usuario);

        var nombreUsuario = new SqlParameter(
                "@NombreUsuario", 
                SqlDbType.NVarChar,
                UsuarioRestricciones.NombreUsuarioMaximo)
        { Value = usuario.NombreUsuario };

        var passwordHash = new SqlParameter(
                "@PasswordHash", 
                SqlDbType.NVarChar,
                UsuarioRestricciones.PasswordHashMaximo)
        { Value = usuario.PasswordHash };

        var nombre = new SqlParameter(
                "@Nombre", 
                SqlDbType.NVarChar,
                UsuarioRestricciones.NombreMaximo)
        { Value = usuario.Nombre};

        var apellido = new SqlParameter(
                "@Apellido", 
                SqlDbType.NVarChar,
                UsuarioRestricciones.ApellidoMaximo)
        { Value = usuario.Apellido };

        var rolId = new SqlParameter(
                "@RolId", 
                SqlDbType.Int)
        { Value = usuario.RolId};

        var fechaAlta = new SqlParameter(
                "@FechaAlta", 
                SqlDbType.DateTime2)
        { Value = usuario.FechaAlta};

        var id = new SqlParameter(
                "@Id", 
                SqlDbType.Int)
        { Direction = ParameterDirection.Output };

        try
        {
            await _contexto.Database.ExecuteSqlRawAsync(
                    """
                    EXEC dbo.Usuario_Crear
                        @NombreUsuario = @NombreUsuario,
                        @PasswordHash = @PasswordHash,
                        @Nombre = @Nombre, 
                        @Apellido = @Apellido,
                        @RolId = @RolId,
                        @FechaAlta = @FechaAlta,
                        @Id = @Id OUTPUT
                    """,
                    nombreUsuario,
                    passwordHash,
                    nombre,
                    apellido,
                    rolId,
                    fechaAlta,
                    id
                );

            usuario.Id = (int)id.Value;
        }
        catch (SqlException excepcion)
            when (excepcion.Number == 51001)
        {
            throw new ValidacionException(
                "El nombre de usuario ya está registrado.");
        }
    }

    /// <summary>
    /// Actualiza los datos personales y el rol de un usuario mediante el
    /// procedimiento almacenado <c>dbo.Usuario_ActualizarDatos</c>.
    /// </summary>
    /// <param name="id">
    /// Identificador del usuario que debe actualizarse.
    /// </param>
    /// <param name="nombre">
    /// Nuevo nombre del usuario.
    /// </param>
    /// <param name="apellido">
    /// Nuevo apellido del usuario.
    /// </param>
    /// <param name="rolId">
    /// Identificador del nuevo rol asignado al usuario.
    /// </param>
    /// <remarks>
    /// Esta operación modifica únicamente el nombre, el apellido y el rol.
    /// No modifica el nombre de acceso, la contraseña, el estado ni la fecha
    /// de alta del usuario.
    ///
    /// El procedimiento también protege la existencia de al menos un
    /// administrador activo en el sistema.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Se produce cuando el usuario indicado ya no existe en la base de datos.
    /// </exception>
    /// <exception cref="ValidacionException">
    /// Se produce cuando la modificación dejaría al sistema sin ningún
    /// administrador activo.
    /// </exception>
    public async Task ActualizarDatosAsync(
        int id,
        string nombre,
        string apellido,
        int rolId)
    {
        var parametroId = new SqlParameter(
            "@Id", 
            SqlDbType.Int)
        { Value = id };

        var parametroNombre = new SqlParameter(
            "@Nombre", 
            SqlDbType.NVarChar,
            UsuarioRestricciones.NombreMaximo)
        { Value = nombre };

        var parametroApellido = new SqlParameter(
            "@Apellido", 
            SqlDbType.NVarChar,
            UsuarioRestricciones.ApellidoMaximo)
        { Value = apellido };

        var parametroRolId = new SqlParameter(
            "@RolId", 
            SqlDbType.Int)
        { Value = rolId };

        try
        {
            await _contexto.Database.ExecuteSqlRawAsync(
                """
                EXEC dbo.Usuario_ActualizarDatos
                    @Id = @Id,
                    @Nombre = @Nombre,
                    @Apellido = @Apellido,
                    @RolId = @RolId
                """,
                parametroId,
                parametroNombre,
                parametroApellido,
                parametroRolId
                );
        }
        catch (SqlException excepcion)
            when (excepcion.Number == 51002)
        {
            throw new InvalidOperationException(
                "El usuario que desea actualizar ya no existe.",
                excepcion);
        }
        catch (SqlException excepcion)
            when (excepcion.Number == 51003)
        {
            throw new ValidacionException(
                 "No se puede cambiar el rol del último administrador activo.");
        }

    }

    /// <summary>
    /// Actualiza las credenciales de acceso de un usuario mediante el
    /// procedimiento almacenado <c>dbo.Usuario_ActualizarCredenciales</c>.
    /// </summary>
    /// <param name="id">
    /// Identificador del usuario cuyas credenciales deben actualizarse.
    /// </param>
    /// <param name="nombreUsuario">
    /// Nuevo nombre utilizado para iniciar sesión.
    /// </param>
    /// <param name="passwordHash">
    /// Nuevo hash de contraseña. Cuando es <see langword="null"/>,
    /// se conserva el hash almacenado actualmente.
    /// </param>
    /// <remarks>
    /// Este método recibe únicamente el hash de la contraseña.
    /// La contraseña en texto plano debe ser validada y procesada
    /// previamente por el servicio de aplicación.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Se produce cuando el usuario indicado ya no existe.
    /// </exception>
    /// <exception cref="ValidacionException">
    /// Se produce cuando el nombre de usuario ya está registrado.
    /// </exception>
    public async Task ActualizarCredencialesAsync(
        int id, 
        string nombreUsuario, 
        string? passwordHash)
    {
        var parametroId = new SqlParameter(
            "@Id",
            SqlDbType.Int) 
        { Value = id };

        var parametroNombreUsuario = new SqlParameter(
            "@NombreUsuario",
            SqlDbType.NVarChar,
            UsuarioRestricciones.NombreUsuarioMaximo)
        { Value = nombreUsuario };

        var parametroPasswordHash = new SqlParameter(
            "@PasswordHash",
            SqlDbType.NVarChar,
            UsuarioRestricciones.PasswordHashMaximo)
        { Value =  (object?)passwordHash ?? DBNull.Value };

        try 
        {
            await _contexto.Database.ExecuteSqlRawAsync(
                """
                EXEC dbo.Usuario_ActualizarCredenciales
                    @Id = @Id,
                    @NombreUsuario = @NombreUsuario,
                    @PasswordHash = @PasswordHash
                """,
                parametroId,
                parametroNombreUsuario,
                parametroPasswordHash
                );
                
        }
        catch (SqlException excepcion)
            when (excepcion.Number == 51001)
        {
            throw new ValidacionException(
                 "El nombre de usuario ya está registrado.");
        }
        catch (SqlException excepcion)
            when (excepcion.Number == 51002)
        {
            throw new InvalidOperationException(
                "El usuario cuyas credenciales desea actualizar ya no existe.",
                excepcion);
        }
    }

    /// <summary>
    /// Activa o desactiva un usuario mediante el procedimiento almacenado
    /// <c>dbo.Usuario_CambiarEstado</c>.
    /// </summary>
    /// <param name="id">
    /// Identificador del usuario cuyo estado debe modificarse.
    /// </param>
    /// <param name="activo">
    /// <see langword="true"/> para activar al usuario;
    /// <see langword="false"/> para desactivarlo.
    /// </param>
    /// <remarks>
    /// Esta operación modifica únicamente el estado del usuario. El
    /// procedimiento protege la existencia de al menos un administrador
    /// activo en el sistema.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// Se produce cuando el usuario indicado ya no existe en la base de datos.
    /// </exception>
    /// <exception cref="ValidacionException">
    /// Se produce cuando se intenta desactivar al último administrador activo.
    /// </exception>
    public async Task CambiarEstadoAsync(int id, bool activo)
    {
        var parametroId = new SqlParameter(
            "@Id",
            SqlDbType.Int)
        { Value = id };

        var parametroActivo = new SqlParameter(
            "@Activo",
            SqlDbType.Bit)
        { Value = activo };

        try
        {
            await _contexto.Database.ExecuteSqlRawAsync(
                """
                EXEC dbo.Usuario_CambiarEstado
                    @Id = @Id,
                    @Activo = @Activo
                """,
                parametroId,
                parametroActivo
                );
        }
        catch (SqlException excepcion)
            when (excepcion.Number == 51002)
        {
            throw new InvalidOperationException(
                "El usuario cuyo estado desea cambiar ya no existe.",
                excepcion);
        }
        catch (SqlException excepcion)
            when (excepcion.Number == 51004)
        {
            throw new ValidacionException(
                "No se puede desactivar al último administrador activo.");
        }
    }
}