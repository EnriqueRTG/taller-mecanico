using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Repositorios;

/// <summary>
/// Implementa las operaciones de persistencia de marcas
/// utilizando Entity Framework Core.
/// </summary>
public sealed class MarcaRepositorio : IMarcaRepositorio
{
    // Contexto de Entity Framework Core utilizado para acceder y realizar operaciones sobre la base de datos.
    private readonly TallerDbContext _contexto;

    /// <summary>
    /// Inicializa el repositorio utilizando el contexto recibido
    /// </summary>
    /// <param name="contexto"></param>
    public MarcaRepositorio(TallerDbContext contexto)
    {
        _contexto = contexto;
    }
    
    /// <summary>
    /// Lista todas las marcas activas
    /// </summary>
    /// <returns>Lista de marcas activas</returns>
    public async Task<List<Marca>> ListarActivosAsync()
    {
        return await _contexto.Marcas
        .Where(m => m.Activo)
        .OrderBy(m => m.Nombre)
        .ToListAsync();
    }

    /// <summary>
    /// Obtiene una marca por su identificador
    /// </summary>
    /// <param name="idMarca"></param>
    /// <returns>Marca encontrada o null</returns>
    public async Task<Marca?> ObtenerPorIdAsync(int idMarca)
    {
        return await _contexto.Marcas
            .FirstOrDefaultAsync(m => m.IdMarca == idMarca);
    }
}       
