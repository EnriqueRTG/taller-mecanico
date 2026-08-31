using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Repositorios;

/// <summary>
/// Implementa las operaciones de persistencia de usuarios
/// utilizando Entity Framework Core.
/// </summary>
public sealed class UsuarioRepositorio : IUsuarioRepositorio
{

    // Contexto de Entity Framework Core utilizado para acceder
    // y realizar operaciones sobre la base de datos.
    private readonly TallerDbContext _contexto;

    /// <summary>
    /// Inicializa el repositorio utilizando el contexto recibido
    /// mediante inyección de dependencias.
    /// </summary>
    public UsuarioRepositorio (TallerDbContext contexto)
    {
        _contexto = contexto;
    }

    /// <summary>
    /// Actualiza los datos de un usuario existente y
    /// guarda los cambios en la base de datos.
    /// </summary>
    public async Task ActualizarAsync(Usuario usuario)
    {
        _contexto.Usuarios.Update(usuario);
        await _contexto.SaveChangesAsync();
    }

    /// <summary>
    /// Agrega un nuevo usuario y guarda los cambios
    /// en la base de datos.
    /// </summary>
    public async Task AgregarAsync(Usuario usuario)
    {
        await _contexto.Usuarios.AddAsync(usuario);
        await _contexto.SaveChangesAsync();
    }

    /// <summary>
    /// Determina si ya existe un usuario registrado
    /// con el nombre de usuario indicado.
    /// </summary>
    public async Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario)
    {
        return await _contexto.Usuarios
            .AnyAsync(u => u.NombreUsuario == nombreUsuario);
    }

    /// <summary>
    /// Obtiene todos los usuarios registrados junto con sus roles,
    /// ordenados por apellido y nombre.
    /// </summary>
    public async Task<List<Usuario>> ListarAsync()
    {
        return await _contexto.Usuarios
            .Include(u => u.Rol)
            .OrderBy(u => u.Apellido)
            .ThenBy(u => u.Nombre)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene un usuario mediante su identificador.
    /// Incluye también la información correspondiente a su rol.
    /// </summary>
    public async Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        return await _contexto.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u =>  u.Id == id);
    }

    /// <summary>
    /// Obtiene un usuario mediante su nombre de usuario.
    /// Este método será utilizado, entre otras cosas,
    /// durante el proceso de autenticación.
    /// </summary>
    public async Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombreUsuario)
    {
        return await _contexto.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
    }
}