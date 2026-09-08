using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Repositorios;

/// <summary>
/// Implementa las operaciones de persistencia de modelos
/// utilizando Entity Framework Core.
/// </summary>
public sealed class ModeloRepositorio : IModeloRepositorio
{
    // Contexto de Entity Framework Core utilizado para acceder y realizar operaciones sobre la base de datos.
    private readonly TallerDbContext _contexto;

    /// <summary>
    /// Inicializa el repositorio utilizando el contexto recibido
    /// </summary>
    /// <param name="contexto"></param>
    public ModeloRepositorio(TallerDbContext contexto)
    {
        _contexto = contexto;
    }

    /// <summary>
    /// Lista todos los modelos activos en la base de datos.
    /// </summary>
    /// <returns>Lista de modelos activos</returns>
    public async Task<List<Modelo>> ListarActivosPorMarcaAsync(int idMarca)
    {
        return await _contexto.Modelos
            .Where(m => m.Activo && m.IdMarca == idMarca)
            .OrderBy(m => m.Nombre)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene un modelo por su ID.
    /// </summary>
    /// <param name="idModelo"></param>
    /// <returns>Modelo encontrado o null</returns>
    public async Task<Modelo?> ObtenerPorIdAsync(int idModelo)
    {
        return await _contexto.Modelos
            .Include(m => m.Marca)
            .FirstOrDefaultAsync(m => m.IdModelo == idModelo);
    }
}
