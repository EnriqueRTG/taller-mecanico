using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;

namespace Taller.Infraestructura.Persistencia.Repositorios;

/// <summary>
/// Implementa las operaciones de persistencia necesarias
/// para trabajar con atenciones mediante Entity Framework Core.
/// </summary>
public sealed class AtencionRepositorio : IAtencionRepositorio
{
    // Contexto de Entity Framework Core utilizado para acceder
    // y realizar operaciones sobre la base de datos.
    private readonly TallerDbContext _contexto;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio de atenciones
    /// utilizando el contexto recibido mediante inyección de dependencias.
    /// </summary>
    /// <param name="contexto">
    /// Contexto de Entity Framework Core utilizado para acceder a la base de datos.
    /// </param>
    public AtencionRepositorio(TallerDbContext contexto)
    {
        _contexto = contexto;
    }

    /// <summary>
    /// Actualiza los datos de una atención existente.
    /// </summary>
    /// <param name="atencion">
    /// Atención cuyos datos se desean actualizar.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica.
    /// </returns>
    public async Task ActualizarAsync(Atencion atencion)
    {
        _contexto.Atenciones.Update(atencion);
        await _contexto.SaveChangesAsync();
    }

    /// <summary>
    /// Agrega una nueva atención a la base de datos.
    /// </summary>
    /// <param name="atencion">
    /// Atención que se desea registrar.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica.
    /// </returns>
    public async Task AgregarAsync(Atencion atencion)
    {
        await _contexto.Atenciones.AddAsync(atencion);
        await _contexto.SaveChangesAsync();
    }

    /// <summary>
    /// Verifica si existe una atención activa asociada
    /// al vehículo indicado.
    /// </summary>
    /// <param name="idVehiculo">
    /// Identificador del vehículo.
    /// </param>
    /// <returns>
    /// true si existe una atención abierta o en proceso para el vehículo;
    /// en caso contrario, false.
    /// </returns>
    public async Task<bool> ExisteAtencionActivaParaVehiculoAsync(
        int idVehiculo)
    {
        return await _contexto.Atenciones.AnyAsync(a =>
            a.IdVehiculo == idVehiculo &&
            (a.Estado == EstadoAtencion.Abierta ||
            a.Estado == EstadoAtencion.EnProceso));
    }

    /// <summary>
    /// Lista todas las atenciones que se encuentran activas,
    /// ordenadas desde la más reciente a la más antigua.
    /// </summary>
    /// <returns>
    /// Lista de atenciones activas.
    /// </returns>
    public async Task<List<Atencion>> ListarActivasAsync()
    {
        return await _contexto.Atenciones
            .Include(a => a.Cliente)
            .Include(a => a.Vehiculo)
                .ThenInclude(v => v.Modelo)
                    .ThenInclude(m => m.Marca)
            .Include(a => a.UsuarioRecepcion)
            .Where(a =>
                a.Estado == EstadoAtencion.Abierta ||
                a.Estado == EstadoAtencion.EnProceso)
            .OrderByDescending(a => a.FechaApertura)
            .ToListAsync();
    }

    /// <summary>
    /// Lista todas las atenciones asociadas a un cliente,
    /// ordenadas desde la más reciente a la más antigua.
    /// </summary>
    /// <param name="idCliente">
    /// Identificador del cliente.
    /// </param>
    /// <returns>
    /// Lista de atenciones asociadas al cliente indicado.
    /// </returns>
    public async Task<List<Atencion>> ListarPorClienteAsync(int idCliente)
    {
        return await _contexto.Atenciones
            .Include(a => a.Cliente)
            .Include(a => a.Vehiculo)
                .ThenInclude(v => v.Modelo)
                    .ThenInclude(m => m.Marca)
            .Include(a => a.UsuarioRecepcion)
            .Where(a => a.IdCliente == idCliente)
            .OrderByDescending(a => a.FechaApertura)
            .ToListAsync();
    }

    /// <summary>
    /// Lista todas las atenciones asociadas a un vehículo,
    /// ordenadas desde la más reciente a la más antigua.
    /// </summary>
    /// <param name="idVehiculo">
    /// Identificador del vehículo.
    /// </param>
    /// <returns>
    /// Lista de atenciones asociadas al vehículo indicado.
    /// </returns>
    public async Task<List<Atencion>> ListarPorVehiculoAsync(int idVehiculo)
    {
        return await _contexto.Atenciones
            .Include(a => a.Cliente)
            .Include(a => a.Vehiculo)
                .ThenInclude(v => v.Modelo)
                    .ThenInclude(m => m.Marca)
            .Include(a => a.UsuarioRecepcion)
            .Where(a => a.IdVehiculo == idVehiculo)
            .OrderByDescending(a => a.FechaApertura)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene una atención mediante su identificador,
    /// incluyendo los datos relacionados del cliente,
    /// vehículo, modelo, marca y usuario de recepción.
    /// </summary>
    /// <param name="idAtencion">
    /// Identificador de la atención.
    /// </param>
    /// <returns>
    /// La atención encontrada o null si no existe.
    /// </returns>
    public async Task<Atencion?> ObtenerPorIdAsync(int idAtencion)
    {
        return await _contexto.Atenciones
            .Include(a => a.Cliente)
            .Include(a => a.Vehiculo)
                .ThenInclude(v => v.Modelo)
                    .ThenInclude(m => m.Marca)
            .Include(a => a.UsuarioRecepcion)
            .FirstOrDefaultAsync(a => a.IdAtencion == idAtencion);
    }
}
