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
    /// <param name = "contexto">Contexto de Entity Framework Core.</param>
    public VehiculoRepositorio(TallerDbContext contexto)
    {
        _contexto = contexto;
    }

    /// <summary>Una única lectura evita duplicar filas si cambia el estado entre consultas separadas.</summary>
    public Task<List<Vehiculo>> ListarTodosAsync() => _contexto.Vehiculos.AsNoTracking().Include(v => v.Modelo).ThenInclude(m => m.Marca).OrderByDescending(x => x.Activo).ThenByDescending(x => x.FechaAlta).ThenByDescending(x => x.IdVehiculo).ToListAsync();
    /// <summary>
    /// Actualiza un vehículo en la base de datos.
    /// </summary>
    public async Task ActualizarAsync(Vehiculo vehiculo)
    {
        var entrada = _contexto.Entry(vehiculo);
        entrada.State = EntityState.Unchanged;
        entrada.Property(x => x.Version).OriginalValue = vehiculo.Version.ToArray();
        entrada.Property(x => x.Dominio).IsModified = true;
        entrada.Property(x => x.Anio).IsModified = true;
        entrada.Property(x => x.Color).IsModified = true;
        entrada.Property(x => x.IdModelo).IsModified = true;
        entrada.Property(x => x.Activo).IsModified = true;
        try
        {
            await _contexto.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw ErroresPersistencia.Traducir(ex, "IX_Vehiculos_Dominio", "Ya existe un vehículo con ese dominio.");
        }
        finally
        {
            entrada.State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Agrega un nuevo vehículo a la base de datos.
    /// </summary>
    public async Task AgregarAsync(Vehiculo vehiculo)
    {
        var entrada = _contexto.Entry(vehiculo);
        entrada.State = EntityState.Added;
        try
        {
            await _contexto.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw ErroresPersistencia.Traducir(ex, "IX_Vehiculos_Dominio", "Ya existe un vehículo con ese dominio.");
        }
        finally
        {
            entrada.State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Verifica si ya existe un vehículo con el mismo dominio, excluyendo opcionalmente un vehículo por su ID.
    /// </summary>
    /// <param name = "dominio"></param>
    /// <param name = "excluirIdVehiculo"></param>
    /// <returns>Boolean</returns>
    public async Task<bool> ExisteDominioAsync(string dominio, int? excluirIdVehiculo = null)
    {
        return await _contexto.Vehiculos.AnyAsync(v => v.Dominio == dominio && (!excluirIdVehiculo.HasValue || v.IdVehiculo != excluirIdVehiculo.Value));
    }

    /// <summary>
    /// Lista todos los vehículos activos en la base de datos
    /// </summary>
    /// <returns>Lista de vehículos</returns>
    public async Task<List<Vehiculo>> ListarActivosAsync()
    {
        return await _contexto.Vehiculos.AsNoTracking().Include(v => v.Modelo).ThenInclude(m => m.Marca).Where(v => v.Activo).OrderBy(v => v.Dominio).ToListAsync();
    }


    public async Task<Vehiculo?> ObtenerPorDominioAsync(string dominio)
    {
        return await _contexto.Vehiculos.AsNoTracking().Include(v => v.Modelo).ThenInclude(m => m.Marca).FirstOrDefaultAsync(v => v.Dominio == dominio);
    }

    /// <summary>
    /// Obtiene un vehículo por su ID
    /// </summary>
    /// <param name = "idVehiculo"></param>
    /// <returns>Vehículo o null si no se encuentra</returns>
    public async Task<Vehiculo?> ObtenerPorIdAsync(int idVehiculo)
    {
        return await _contexto.Vehiculos.AsNoTracking().Include(v => v.Modelo).ThenInclude(m => m.Marca).FirstOrDefaultAsync(v => v.IdVehiculo == idVehiculo);
    }
}
