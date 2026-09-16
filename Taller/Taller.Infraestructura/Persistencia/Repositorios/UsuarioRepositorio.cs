using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Repositorios;

/// <summary>
/// Implementa las operaciones de persistencia relacionadas
/// con los usuarios mediante Entity Framework Core.
/// </summary>
/// <remarks>
/// Este repositorio solamente se ocupa del acceso y almacenamiento
/// de datos. Las validaciones y reglas de negocio deben permanecer
/// en los servicios de la capa de aplicación.
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
    /// Agrega un nuevo usuario y confirma la operación
    /// en la base de datos.
    /// </summary>
    /// <param name="usuario">
    /// Usuario que debe registrarse.
    /// </param>
    public async Task AgregarAsync(
        Usuario usuario)
    {
        await _contexto.Usuarios.AddAsync(usuario);
        await _contexto.SaveChangesAsync();
    }

    /// <summary>
    /// Actualiza los datos modificables de un usuario existente.
    ///
    /// La actualización se ejecuta directamente en la base de datos
    /// y no depende del seguimiento de entidades del contexto.
    /// </summary>
    /// <param name="usuario">
    /// Usuario que contiene los valores que deben persistirse.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Se produce cuando el usuario recibido es nulo.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Se produce cuando el usuario ya no existe.
    /// </exception>
    public async Task ActualizarAsync(
        Usuario usuario)
    {
        ArgumentNullException.ThrowIfNull(usuario);

        int filasAfectadas =
            await _contexto.Usuarios
                .Where(registrado =>
                    registrado.Id == usuario.Id)
                .ExecuteUpdateAsync(actualizacion =>
                    actualizacion
                        .SetProperty(
                            registrado => registrado.NombreUsuario,
                            usuario.NombreUsuario)
                        .SetProperty(
                            registrado => registrado.PasswordHash,
                            usuario.PasswordHash)
                        .SetProperty(
                            registrado => registrado.Nombre,
                            usuario.Nombre)
                        .SetProperty(
                            registrado => registrado.Apellido,
                            usuario.Apellido)
                        .SetProperty(
                            registrado => registrado.Activo,
                            usuario.Activo)
                        .SetProperty(
                            registrado => registrado.RolId,
                            usuario.RolId));

        if (filasAfectadas == 0)
        {
            throw new InvalidOperationException(
                "El usuario que desea actualizar ya no existe.");
        }
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
}