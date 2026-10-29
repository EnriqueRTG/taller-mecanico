using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Repositorios;
/// <summary>
/// Implementa las operaciones de persistencia de clientes
/// utilizando Entity Framework Core.
/// </summary>
public sealed class ClienteRepositorio : IClienteRepositorio
{
    // Contexto de Entity Framework Core utilizado para acceder
    // y realizar operaciones sobre la base de datos.
    private readonly TallerDbContext _contexto;
    /// <summary>
    /// Inicializa el repositorio utilizando el contexto recibido
    /// mediante inyección de dependencias.
    /// </summary>
    public ClienteRepositorio(TallerDbContext contexto)
    {
        _contexto = contexto;
    }

    /// <summary>Una única lectura evita duplicar filas si cambia el estado entre consultas separadas.</summary>
    public Task<List<Cliente>> ListarTodosAsync() => _contexto.Clientes.AsNoTracking().OrderByDescending(x => x.Activo).ThenByDescending(x => x.FechaAlta).ThenByDescending(x => x.IdCliente).ToListAsync();
    /// <summary>
    /// Actualiza los datos de un cliente existente.
    /// </summary>
    public async Task ActualizarAsync(Cliente cliente)
    {
        var entrada = _contexto.Entry(cliente);
        entrada.State = EntityState.Unchanged;
        entrada.Property(x => x.Version).OriginalValue = cliente.Version.ToArray();
        entrada.Property(x => x.TipoDocumento).IsModified = true;
        entrada.Property(x => x.Documento).IsModified = true;
        entrada.Property(x => x.Nombre).IsModified = true;
        entrada.Property(x => x.Apellido).IsModified = true;
        entrada.Property(x => x.Telefono).IsModified = true;
        entrada.Property(x => x.Email).IsModified = true;
        entrada.Property(x => x.Direccion).IsModified = true;
        entrada.Property(x => x.Activo).IsModified = true;
        try
        {
            await _contexto.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw ErroresPersistencia.Traducir(ex, "IX_Clientes_TipoDocumento_Documento", "Ya existe un cliente con ese tipo y número de documento.");
        }
        finally
        {
            entrada.State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Agrega un nuevo cliente a la persistencia.
    /// </summary>
    public async Task AgregarAsync(Cliente cliente)
    {
        var entrada = _contexto.Entry(cliente);
        entrada.State = EntityState.Added;
        try
        {
            await _contexto.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw ErroresPersistencia.Traducir(ex, "IX_Clientes_TipoDocumento_Documento", "Ya existe un cliente con ese tipo y número de documento.");
        }
        finally
        {
            entrada.State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Verifica si ya existe un cliente con el mismo tipo y número de documento, excluyendo opcionalmente un cliente por su ID.
    /// </summary>
    /// <param name = "tipoDocumento"></param>
    /// <param name = "documento"></param>
    /// <param name = "excluirIdCliente"></param>
    /// <returns></returns>
    public async Task<bool> ExisteDocumentoAsync(string tipoDocumento, string documento, int? excluirIdCliente = null)
    {
        return await _contexto.Clientes.AnyAsync(c => c.TipoDocumento == tipoDocumento && c.Documento == documento && (!excluirIdCliente.HasValue || c.IdCliente != excluirIdCliente.Value));
    }

    /// <summary>
    /// Obtiene los clientes que se encuentran activos,
    /// ordenados por apellido y nombre.
    /// </summary>
    public async Task<List<Cliente>> ListarActivosAsync()
    {
        return await _contexto.Clientes.AsNoTracking().Where(c => c.Activo).OrderBy(c => c.Apellido).ThenBy(c => c.Nombre).ToListAsync();
    }


    public async Task<Cliente?> ObtenerPorDocumentoAsync(string tipoDocumento, string documento)
    {
        return await _contexto.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.TipoDocumento == tipoDocumento && c.Documento == documento);
    }

    /// <summary>
    /// Obtiene un cliente mediante su identificador.
    /// </summary>
    public async Task<Cliente?> ObtenerPorIdAsync(int id)
    {
        return await _contexto.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.IdCliente == id);
    }
}
