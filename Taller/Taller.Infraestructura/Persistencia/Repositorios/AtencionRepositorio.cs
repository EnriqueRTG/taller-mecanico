using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Excepciones;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;

namespace Taller.Infraestructura.Persistencia.Repositorios;
/// <summary>
/// Implementa las consultas y operaciones de persistencia de atenciones.
/// </summary>
/// <remarks>
/// Las consultas devuelven entidades sin seguimiento.
/// Las actualizaciones utilizan la versión recibida para detectar
/// modificaciones concurrentes.
/// </remarks>
public sealed class AtencionRepositorio : IAtencionRepositorio
{
    private const string IndiceVehiculoActivo = "UX_Atenciones_VehiculoActivo";
    // Contexto de Entity Framework Core utilizado para acceder
    // y realizar operaciones sobre la base de datos.
    private readonly TallerDbContext _contexto;
    /// <summary>
    /// Inicializa una nueva instancia del repositorio de atenciones
    /// utilizando el contexto recibido mediante inyección de dependencias.
    /// </summary>
    /// <param name = "contexto">
    /// Contexto de Entity Framework Core utilizado para acceder a la base de datos.
    /// </param>
    public AtencionRepositorio(TallerDbContext contexto)
    {
        _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
    }

    /// <summary>
    /// Agrega una nueva atención a la base de datos.
    /// </summary>
    /// <param name = "atencion">
    /// Atención que se desea registrar.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica.
    /// </returns>
    public async Task AgregarAsync(Atencion atencion)
    {
        //Es una comprobación del contrato del método, no una validación de negocio.
        ArgumentNullException.ThrowIfNull(atencion);
        // RepeatableRead conserva las precondiciones de habilitación hasta confirmar el alta.
        await using var transaccion = _contexto.Database.IsRelational()
            ? await _contexto.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead)
            : null;
        if (transaccion is not null)
        {
            bool habilitados = await _contexto.Clientes.AsNoTracking().AnyAsync(c => c.IdCliente == atencion.IdCliente && c.Activo)
                && await _contexto.Vehiculos.AsNoTracking().AnyAsync(v => v.IdVehiculo == atencion.IdVehiculo && v.Activo);
            if (!habilitados) throw new ValidacionException("El cliente o el vehículo fueron inhabilitados. Actualice antes de abrir la atención.");
        }
        var entrada = _contexto.Entry(atencion);
        entrada.State = EntityState.Added;
        foreach (var evento in atencion.Historial)
        {
            evento.Atencion = atencion;
            _contexto.HistorialAtenciones.Add(evento);
        }

        foreach (var solicitud in atencion.Solicitudes)
        {
            solicitud.Atencion = atencion;
            _contexto.Set<SolicitudAtencion>().Add(solicitud);
        }
        try
        {
            await GuardarAsync(entrada);
            if (transaccion is not null) await transaccion.CommitAsync();
        }
        finally
        {
            foreach (var solicitud in atencion.Solicitudes) _contexto.Entry(solicitud).State = EntityState.Detached;
            foreach (var evento in atencion.Historial) _contexto.Entry(evento).State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Comprueba si el vehículo tiene una atención
    /// que no está cerrada ni cancelada.
    /// </summary>
    /// <param name = "idVehiculo">Identificador del vehículo.</param>
    /// <returns>
    /// <see langword="true"/> si existe una atención activa;
    /// en caso contrario, <see langword="false"/>.
    /// </returns>
    public async Task<bool> ExisteAtencionActivaParaVehiculoAsync(int idVehiculo)
    {
        return await _contexto.Atenciones.AnyAsync(a => a.IdVehiculo == idVehiculo && a.Estado != EstadoAtencion.Cerrada && a.Estado != EstadoAtencion.Cancelada);
    }

    /// <summary>
    /// Lista las atenciones activas desde la más reciente.
    /// </summary>
    /// <returns>
    /// Lista de atenciones activas, ordenadas desde la más reciente.
    /// </returns>
    public Task<List<Atencion>> ListarActivasAsync()
    {
        return ConsultaConRelaciones().Where(a => a.Estado != EstadoAtencion.Cerrada && a.Estado != EstadoAtencion.Cancelada).OrderByDescending(a => a.FechaApertura).ThenByDescending(a => a.IdAtencion).ToListAsync();
    }

    /// <summary>
    /// Lista las atenciones, incluyendo cerradas y canceladas.
    /// </summary>
    public Task<List<Atencion>> ListarTodasAsync()
    {
        return ConsultaConRelaciones().OrderByDescending(a => a.FechaApertura).ThenByDescending(a => a.IdAtencion).ToListAsync();
    }

    /// <summary>
    /// Lista el historial del cliente, incluyendo
    /// atenciones cerradas y canceladas.
    /// </summary>
    /// <param name = "idCliente">
    /// Identificador del cliente.
    /// </param>
    /// <returns>
    /// Lista de atenciones asociadas al cliente indicado.
    /// </returns>
    public Task<List<Atencion>> ListarPorClienteAsync(int idCliente)
    {
        return ConsultaConRelaciones().Include(a => a.Diagnosticos).Where(a => a.IdCliente == idCliente).OrderByDescending(a => a.FechaApertura).ThenByDescending(a => a.IdAtencion).ToListAsync();
    }

    /// <summary>
    /// Lista el historial del vehículo, incluyendo
    /// atenciones cerradas y canceladas.
    /// </summary>
    /// <param name = "idVehiculo">
    /// Identificador del vehículo.
    /// </param>
    /// <returns>
    /// Lista de atenciones asociadas al vehículo indicado.
    /// </returns>
    public Task<List<Atencion>> ListarPorVehiculoAsync(int idVehiculo)
    {
        return ConsultaConRelaciones().Include(a => a.Diagnosticos).Where(a => a.IdVehiculo == idVehiculo).OrderByDescending(a => a.FechaApertura).ThenByDescending(a => a.IdAtencion).ToListAsync();
    }

    /// <summary>
    /// Obtiene una atención con sus datos relacionados y su versión.
    /// Devuelve null si no existe.
    /// </summary>
    /// <param name = "idAtencion">
    /// Identificador de la atención.
    /// </param>
    /// <returns>
    /// La atención encontrada o null si no existe.
    /// </returns>
    public Task<Atencion?> ObtenerPorIdAsync(int idAtencion)
    {
        return ConsultaConRelaciones().Include(a => a.Historial).Include(a => a.Diagnosticos).FirstOrDefaultAsync(a => a.IdAtencion == idAtencion);
    }

    /// <summary>
    /// Construye la consulta común sin seguimiento
    /// para consultar atenciones y sus relaciones.
    /// </summary>
    private IQueryable<Atencion> ConsultaConRelaciones()
    {
        return _contexto.Atenciones.AsNoTracking().Include(a => a.Solicitudes).Include(a => a.Pausas).AsSplitQuery().Include(a => a.Evaluacion).Include(a => a.Cliente).Include(a => a.Vehiculo).ThenInclude(v => v.Modelo).ThenInclude(m => m.Marca).Include(a => a.UsuarioRecepcion).Include(a => a.TecnicoEnActividad);
    }

    private async Task GuardarAsync(EntityEntry<Atencion> entrada)
    {
        try
        {
            await _contexto.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConflictoConcurrenciaException("La atención fue modificada o eliminada por otra operación. Actualice la información antes de continuar.", ex);
        }
        catch (DbUpdateException ex)when (EsDuplicadoDeVehiculoActivo(ex))
        {
            throw new ValidacionException("El vehículo ya tiene una atención activa. Actualizá el listado antes de continuar.", nameof(Atencion.IdVehiculo));
        }
        finally
        {
            // Evita conservar la atencion o una operación fallida
            // como cambio pendiente para un guardado posterior.
            entrada.State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Identifica exclusivamente la violación del índice
    /// que limita las atenciones activas por vehículo.
    /// </summary>
    private static bool EsDuplicadoDeVehiculoActivo(DbUpdateException excepcion)
    {
        return excepcion.InnerException is SqlException sqlEx && sqlEx.Errors.Cast<SqlError>().Any(error => (error.Number == 2601 || error.Number == 2627) && error.Message.Contains(IndiceVehiculoActivo, StringComparison.OrdinalIgnoreCase));
    }
}
