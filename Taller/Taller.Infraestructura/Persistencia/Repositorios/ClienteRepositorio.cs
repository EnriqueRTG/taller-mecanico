
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

    /// <summary>
    /// Actualiza los datos de un cliente existente.
    /// </summary>
    public async Task ActualizarAsync(Cliente cliente)
    {
        _contexto.Clientes.Update(cliente);
        await _contexto.SaveChangesAsync();
    }

    /// <summary>
    /// Agrega un nuevo cliente a la persistencia.
    /// </summary>
    public async Task AgregarAsync(Cliente cliente)
    {
        await _contexto.Clientes.AddAsync(cliente);
        await _contexto.SaveChangesAsync();
    }

    /// <summary>
    /// Verifica si ya existe un cliente con el mismo tipo y número de documento, excluyendo opcionalmente un cliente por su ID.
    /// </summary>
    /// <param name="tipoDocumento"></param>
    /// <param name="documento"></param>
    /// <param name="excluirIdCliente"></param>
    /// <returns></returns>
    public async Task<bool> ExisteDocumentoAsync(
    string tipoDocumento,
    string documento,
    int? excluirIdCliente = null)
    {
        return await _contexto.Clientes.AnyAsync(c =>
            c.TipoDocumento == tipoDocumento &&
            c.Documento == documento &&
            (!excluirIdCliente.HasValue ||
             c.IdCliente != excluirIdCliente.Value));
    }

    /// <summary>
    /// Obtiene los clientes que se encuentran activos,
    /// ordenados por apellido y nombre.
    /// </summary>
    public async Task<List<Cliente>> ListarActivosAsync()
    {
        return await _contexto.Clientes
            .Where(c => c.Activo)
            .OrderBy(c => c.Apellido)
            .ThenBy(c => c.Nombre)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene los clientes dados de baja lógica.
    /// </summary>
    public async Task<List<Cliente>> ListarInactivosAsync()
    {
        return await _contexto.Clientes
            .Where(c => !c.Activo)
            .OrderBy(c => c.Apellido)
            .ThenBy(c => c.Nombre)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene un cliente mediante su tipo y número de documento.
    /// </summary>
    public async Task<Cliente?> ObtenerPorDocumentoAsync(string tipoDocumento, string documento)
    {
        return await _contexto.Clientes
            .FirstOrDefaultAsync(c => c.TipoDocumento == tipoDocumento && c.Documento == documento);
    }

    /// <summary>
    /// Obtiene un cliente mediante su identificador.
    /// </summary>
    public async Task<Cliente?> ObtenerPorIdAsync(int id)
    {
        return await _contexto.Clientes
            .FirstOrDefaultAsync(c => c.IdCliente == id);
    }
}
