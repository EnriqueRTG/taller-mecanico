using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;

namespace Taller.Infraestructura.Persistencia.Repositorios;

/// <summary>
/// Implementa las operaciones de persistencia necesarias
/// para trabajar con vehículos mediante Entity Framework Core.
/// </summary>
public sealed class VehiculoRepositorio : IVehiculoRepositorio
{
    // Contexto de Entity Framework Core utilizado para acceder a la base de datos.
    private readonly TallerDbContext _contexto;

    /// <summary>
    /// Inicializa el repositorio utilizando el contexto recibido.
    /// </summary>
    /// <param name="contexto">Contexto de Entity Framework Core.</param>
    public VehiculoRepositorio(TallerDbContext contexto)
    {
        _contexto = contexto;
    }

    /// <summary>
    /// Actualiza un vehículo en la base de datos.
    /// </summary>
    public async Task ActualizarAsync(Vehiculo vehiculo)
    {
        _contexto.Vehiculos.Update(vehiculo);
        await _contexto.SaveChangesAsync();
    }

    /// <summary>
    /// Agrega un nuevo vehículo a la base de datos.
    /// </summary>
    public async Task AgregarAsync(Vehiculo vehiculo)
    {
        await _contexto.Vehiculos.AddAsync(vehiculo);
        await _contexto.SaveChangesAsync();
    }

    /// <summary>
    /// Verifica si ya existe un vehículo con el mismo dominio, excluyendo opcionalmente un vehículo por su ID.
    /// </summary>
    /// <param name="dominio"></param>
    /// <param name="excluirIdVehiculo"></param>
    /// <returns>Boolean</returns>
    public async Task<bool> ExisteDominioAsync(string dominio, int? excluirIdVehiculo = null)
    {
        return await _contexto.Vehiculos.AnyAsync(v =>
            v.Dominio == dominio &&
            (!excluirIdVehiculo.HasValue || v.IdVehiculo != excluirIdVehiculo.Value));
    }

    /// <summary>
    /// Lista todos los vehículos activos en la base de datos
    /// </summary>
    /// <returns>Lista de vehículos</returns>
    public async Task<List<Vehiculo>> ListarActivosAsync()
    {
        return await _contexto.Vehiculos
            .Include(v => v.Modelo)
            .ThenInclude(m => m.Marca)
            .Where(v => v.Activo)
            .OrderBy(v => v.Dominio)
            .ToListAsync();
    }

    /// <summary>
    /// Lista todos los vehículos inactivos en la base de datos
    /// </summary>
    /// <returns>Lista de vehículos</returns>
    public async Task<List<Vehiculo>> ListarInactivosAsync()
    {
        return await _contexto.Vehiculos
            .Include(v => v.Modelo)
            .ThenInclude(m => m.Marca)
            .Where(v => !v.Activo)
            .OrderBy(v => v.Dominio)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene un vehículo por su dominio
    /// </summary>
    /// <param name="dominio"></param>
    /// <returns>Vehículo o null si no se encuentra</returns>
    public async Task<Vehiculo?> ObtenerPorDominioAsync(string dominio)
    {
        return await _contexto.Vehiculos
            .Include(v => v.Modelo)
            .ThenInclude(m => m.Marca)
            .FirstOrDefaultAsync(v => v.Dominio == dominio);
    }

    /// <summary>
    /// Obtiene un vehículo por su ID
    /// </summary>
    /// <param name="idVehiculo"></param>
    /// <returns>Vehículo o null si no se encuentra</returns>
    public async Task<Vehiculo?> ObtenerPorIdAsync(int idVehiculo)
    {
        return await _contexto.Vehiculos
            .Include(v => v.Modelo)
            .ThenInclude(m => m.Marca)
            .FirstOrDefaultAsync(v => v.IdVehiculo == idVehiculo);
    }
}
