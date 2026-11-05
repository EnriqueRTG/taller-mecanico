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
    /// <param name = "contexto"></param>
    public ModeloRepositorio(TallerDbContext contexto)
    {
        _contexto = contexto;
    }

    /// <summary>Lista modelos habilitados y conserva el modelo actual aunque haya sido inhabilitado.</summary>
    public Task<List<Modelo>> ListarParaEdicionAsync(int actual) => _contexto.Modelos.AsNoTracking().Include(m => m.Marca).Where(m => m.IdModelo == actual || m.Activo && m.Marca.Activo).OrderBy(m => m.Marca.Nombre).ThenBy(m => m.Nombre).ToListAsync();
    public Task<List<Marca>> ListarMarcasDisponiblesAsync() => _contexto.Marcas.AsNoTracking()
        .Where(m => m.Activo && _contexto.Modelos.Any(modelo => modelo.IdMarca == m.IdMarca && modelo.Activo))
        .OrderBy(m => m.Nombre).ToListAsync();

    public Task<List<Modelo>> ListarDisponiblesPorMarcaAsync(int idMarca) => _contexto.Modelos.AsNoTracking()
        .Include(m => m.Marca).Where(m => m.IdMarca == idMarca && m.Activo && m.Marca.Activo)
        .OrderBy(m => m.Nombre).ToListAsync();

    public async Task<Modelo?> ObtenerPorIdAsync(int idModelo)
    {
        return await _contexto.Modelos.AsNoTracking().Include(m => m.Marca).FirstOrDefaultAsync(m => m.IdModelo == idModelo);
    }
}
