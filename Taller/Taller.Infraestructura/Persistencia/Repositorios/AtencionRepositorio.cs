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
    /// <param name="contexto">
    /// Contexto de Entity Framework Core utilizado para acceder a la base de datos.
    /// </param>
    public AtencionRepositorio(TallerDbContext 
        contexto)
    {
        _contexto = contexto
            ?? throw new ArgumentNullException(nameof(contexto));
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
    public async Task AgregarAsync(
        Atencion atencion)
    {
        //Es una comprobación del contrato del método, no una validación de negocio.
        ArgumentNullException.ThrowIfNull(atencion);

        var entrada = _contexto.Entry(atencion);
        entrada.State = EntityState.Added;

        await GuardarAsync(entrada);
    }

    /// <summary>
    /// Guarda el ingreso de una atención junto con los servicios
    /// iniciales autorizados y el evento del historial.
    /// </summary>
    /// <remarks>
    /// La atención, el presupuesto, sus detalles y el historial
    /// se persisten mediante un único guardado.
    /// </remarks>
    public async Task EnviarADiagnosticoAsync(
        Atencion atencion,
        Presupuesto presupuesto,
        HistorialAtencion historial)
    {
        ArgumentNullException.ThrowIfNull(atencion);
        ArgumentNullException.ThrowIfNull(presupuesto);
        ArgumentNullException.ThrowIfNull(historial);

        if (atencion.Estado != EstadoAtencion.PendienteDiagnostico
            || atencion.IdTecnicoEnActividad is not null
            || atencion.ActividadTecnica is not null)
        {
            throw new ArgumentException(
                "La atención debe quedar pendiente de diagnóstico y sin reserva.",
                nameof(atencion));
        }

        if (presupuesto.IdPresupuesto != 0
            || presupuesto.Tipo != TipoPresupuesto.Diagnostico
            || presupuesto.Estado != EstadoPresupuesto.Aceptado
            || presupuesto.IdAtencion != atencion.IdAtencion
            || presupuesto.IdDiagnostico is not null)
        {
            throw new ArgumentException(
                "El presupuesto debe ser nuevo, de diagnóstico, aceptado " +
                "y pertenecer a la atención.",
                nameof(presupuesto));
        }

        var detalles = presupuesto.Detalles.ToList();

        if (detalles.Count == 0)
        {
            throw new ArgumentException(
                "El presupuesto inicial debe contener al menos un servicio.",
                nameof(presupuesto));
        }

        foreach (var detalle in detalles)
        {
            if (detalle.IdPresupuestoDetalle != 0
                || detalle.IdPresupuesto != 0
                || detalle.IdPresupuestoCambio is not null)
            {
                throw new ArgumentException(
                    "Los detalles deben ser nuevos y no tener " +
                    "un presupuesto de cambio.",
                    nameof(presupuesto));
            }
        }

        var entradaPresupuesto = _contexto.Entry(presupuesto);

        var entradasDetalles = detalles
            .Select(detalle => _contexto.Entry(detalle))
            .ToList();

        try
        {
            entradaPresupuesto.State = EntityState.Added;

            foreach (var entradaDetalle in entradasDetalles)
            {
                // EF completará la FK cuando se genere
                // el identificador del presupuesto.
                entradaDetalle.Entity.Presupuesto = presupuesto;
                entradaDetalle.State = EntityState.Added;
            }

            // Este método efectúa un único SaveChangesAsync:
            // atención, historial, presupuesto y todos sus detalles.
            await ActualizarConHistorialAsync(atencion, historial);
        }
        catch (DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx
                && sqlEx.Errors.Cast<SqlError>().Any(error =>
                    (error.Number == 2601 || error.Number == 2627)
                    && error.Message.Contains(
                        "UX_Presupuestos_DiagnosticoPorAtencion",
                        StringComparison.OrdinalIgnoreCase)))
        {
            throw new ValidacionException(
                "La atención ya tiene registrados sus servicios iniciales.",
                nameof(Presupuesto.IdAtencion));
        }
        finally
        {
            // Evita conservar inserciones pendientes después
            // de una operación exitosa o fallida.
            foreach (var entradaDetalle in entradasDetalles)
            {
                entradaDetalle.State = EntityState.Detached;
            }

            entradaPresupuesto.State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Actualiza la atención, incluida su reserva técnica,
    /// y agrega un evento al historial.
    /// </summary>
    /// <remarks>
    /// Un único SaveChangesAsync guarda ambos cambios en una transacción.
    /// Si existe un conflicto de concurrencia, ninguno se confirma.
    /// </remarks>
    public async Task ActualizarConHistorialAsync(
        Atencion atencion,
        HistorialAtencion historial)
    {
        ArgumentNullException.ThrowIfNull(atencion);
        ArgumentNullException.ThrowIfNull(historial);

        if (atencion.IdAtencion <= 0)
        {
            throw new ArgumentException(
                "La atención debe tener un identificador válido.",
                nameof(atencion));
        }

        if (atencion.Version is null || atencion.Version.Length != 8)
        {
            throw new ArgumentException(
                "La atención debe tener una versión válida.",
                nameof(atencion));
        }

        if (historial.IdHistorialAtencion != 0)
        {
            throw new ArgumentException(
                "El evento del historial debe ser nuevo.",
                nameof(historial));
        }

        if (historial.IdAtencion != atencion.IdAtencion)
        {
            throw new ArgumentException(
                "El evento debe corresponder a la atención actualizada.",
                nameof(historial));
        }

        if (historial.EstadoNuevo != atencion.Estado)
        {
            throw new ArgumentException(
                "El estado del evento debe coincidir con el de la atención.",
                nameof(historial));
        }

        var entradaAtencion = _contexto.Entry(atencion);
        entradaAtencion.State = EntityState.Unchanged;

        entradaAtencion.Property(a => a.Version).OriginalValue =
            atencion.Version.ToArray();

        entradaAtencion.Property(a => a.IdCliente).IsModified = true;
        entradaAtencion.Property(a => a.IdVehiculo).IsModified = true;
        entradaAtencion.Property(a => a.MotivoConsulta).IsModified = true;
        entradaAtencion.Property(a => a.Estado).IsModified = true;
        entradaAtencion.Property(a => a.FechaCierre).IsModified = true;

        entradaAtencion.Property(a => a.IdTecnicoEnActividad)
            .IsModified = true;

        entradaAtencion.Property(a => a.ActividadTecnica)
            .IsModified = true;

        var entradaHistorial = _contexto.Entry(historial);
        entradaHistorial.State = EntityState.Added;

        try
        {
            await GuardarAsync(entradaAtencion);
        }
        finally
        {
            // Impide que un evento fallido quede pendiente
            // y se inserte accidentalmente en otra operación.
            entradaHistorial.State = EntityState.Detached;
        }
    }

    /// <summary>
    /// Actualiza los datos editables y el estado de una atención,
    /// conservando la fecha de apertura y el usuario de recepción.
    /// </summary>
    /// <param name="atencion">
    /// Atención cuyos datos se desean actualizar.
    /// </param>
    /// <returns>
    /// Una tarea que representa la operación asincrónica.
    /// </returns>
    /// <remarks>
    /// La propiedad Version debe contener la versión utilizada
    /// para validar y preparar esta operación.
    /// </remarks>
    public async Task ActualizarAsync(
        Atencion atencion)
    {
        // Es una comprobación del contrato del método, no una validación de negocio.
        ArgumentNullException.ThrowIfNull(atencion);

        if (atencion.IdAtencion <= 0)
        {
            throw new ArgumentException(
                "La atención debe tener un identificador válido.",
                nameof(atencion));
        }

        if (atencion.Version is null || atencion.Version.Length != 8)
        {
            throw new ArgumentException(
                "La atención debe tener una versión válida para actualizarse.",
                nameof(atencion));
        }

        // Cambiar el estado de esta entrada no adjunta el grafo
        // de Cliente, Vehiculo y UsuarioRecepcion
        var entrada = _contexto.Entry(atencion);
        entrada.State = EntityState.Unchanged;

        entrada.Property(a => a.Version).OriginalValue = atencion.Version.ToArray();

        entrada.Property(a => a.IdCliente).IsModified = true;
        entrada.Property(a => a.IdVehiculo).IsModified = true;
        entrada.Property(a => a.MotivoConsulta).IsModified = true;
        entrada.Property(a => a.Estado).IsModified = true;
        entrada.Property(a => a.FechaCierre).IsModified = true;

        await GuardarAsync(entrada);
    }

    /// <summary>
    /// Prepara el cambio de reserva y su historial.
    /// El guardado corresponde a la unidad de trabajo.
    /// </summary>
    public void PrepararCambioReservaConHistorial(
        Atencion atencion,
        HistorialAtencion historial)
    {
        ArgumentNullException.ThrowIfNull(atencion);
        ArgumentNullException.ThrowIfNull(historial);

        if (atencion.IdAtencion <= 0)
        {
            throw new ArgumentException(
                "La atención debe tener un identificador válido.",
                nameof(atencion));
        }

        if (atencion.Version is null || atencion.Version.Length != 8)
        {
            throw new ArgumentException(
                "La atención debe tener una versión válida.",
                nameof(atencion));
        }

        if (historial.IdHistorialAtencion != 0
            || historial.IdAtencion != atencion.IdAtencion
            || historial.EstadoNuevo != atencion.Estado)
        {
            throw new ArgumentException(
                "El evento debe ser nuevo y corresponder a la atención y su estado.",
                nameof(historial));
        }

        if (atencion.IdTecnicoEnActividad.HasValue
            != atencion.ActividadTecnica.HasValue)
        {
            throw new ArgumentException(
                "La reserva debe identificar tanto al técnico como a la actividad, " +
                "o dejar ambos valores vacíos.",
                nameof(atencion));
        }

        var entrada = _contexto.Entry(atencion);

        entrada.State = EntityState.Unchanged;

        entrada.Property(a => a.Version).OriginalValue =
            atencion.Version.ToArray();

        entrada.Property(a => a.IdTecnicoEnActividad)
            .IsModified = true;

        entrada.Property(a => a.ActividadTecnica)
            .IsModified = true;

        entrada.Property(a => a.Estado)
            .IsModified = true;

        _contexto.Entry(historial).State = EntityState.Added;
    }

    /// <summary>
    /// Comprueba si el vehículo tiene una atención
    /// que no está cerrada ni cancelada.
    /// </summary>
    /// <param name="idVehiculo">Identificador del vehículo.</param>
    /// <returns>
    /// <see langword="true"/> si existe una atención activa;
    /// en caso contrario, <see langword="false"/>.
    /// </returns>
    public async Task<bool> ExisteAtencionActivaParaVehiculoAsync(
        int idVehiculo)
    {
        return await _contexto.Atenciones.AnyAsync(a =>
            a.IdVehiculo == idVehiculo &&
            a.Estado != EstadoAtencion.Cerrada &&
            a.Estado != EstadoAtencion.Cancelada);
    }

    /// <summary>
    /// Lista las atenciones activas desde la más reciente.
    /// </summary>
    /// <returns>
    /// Lista de atenciones activas, ordenadas desde la más reciente.
    /// </returns>
    public Task<List<Atencion>> ListarActivasAsync()
    {
        return ConsultaConRelaciones()
            .Where(a =>
                a.Estado != EstadoAtencion.Cerrada &&
                a.Estado != EstadoAtencion.Cancelada)
            .OrderByDescending(a => a.FechaApertura)
            .ThenByDescending(a => a.IdAtencion)
            .ToListAsync();
    }

    /// <summary>
    /// Lista las atenciones, incluyendo cerradas y canceladas.
    /// </summary>
    public Task<List<Atencion>> ListarTodasAsync()
    {
        return ConsultaConRelaciones()
            .OrderByDescending(a => a.FechaApertura)
            .ThenByDescending(a => a.IdAtencion)
            .ToListAsync();
    }

    /// <summary>
    /// Lista el historial del cliente, incluyendo
    /// atenciones cerradas y canceladas.
    /// </summary>
    /// <param name="idCliente">
    /// Identificador del cliente.
    /// </param>
    /// <returns>
    /// Lista de atenciones asociadas al cliente indicado.
    /// </returns>
    public Task<List<Atencion>> ListarPorClienteAsync(
        int idCliente)
    {
        return ConsultaConRelaciones()
            .Where(a => a.IdCliente == idCliente)
            .OrderByDescending(a => a.FechaApertura)
            .ThenByDescending(a => a.IdAtencion)
            .ToListAsync();
    }

    /// <summary>
    /// Lista el historial del vehículo, incluyendo
    /// atenciones cerradas y canceladas.
    /// </summary>
    /// <param name="idVehiculo">
    /// Identificador del vehículo.
    /// </param>
    /// <returns>
    /// Lista de atenciones asociadas al vehículo indicado.
    /// </returns>
    public Task<List<Atencion>> ListarPorVehiculoAsync(
        int idVehiculo)
    {
        return ConsultaConRelaciones()
            .Where(a => a.IdVehiculo == idVehiculo)
            .OrderByDescending(a => a.FechaApertura)
            .ThenByDescending(a => a.IdAtencion)
            .ToListAsync();
    }

    /// <summary>
    /// Obtiene una atención con sus datos relacionados y su versión.
    /// Devuelve null si no existe.
    /// </summary>
    /// <param name="idAtencion">
    /// Identificador de la atención.
    /// </param>
    /// <returns>
    /// La atención encontrada o null si no existe.
    /// </returns>
    public Task<Atencion?> ObtenerPorIdAsync(
        int idAtencion)
    {
        return ConsultaConRelaciones()
            .FirstOrDefaultAsync(a => a.IdAtencion == idAtencion);
    }

    /// <summary>
    /// Construye la consulta común sin seguimiento
    /// para consultar atenciones y sus relaciones.
    /// </summary>
    private IQueryable<Atencion> ConsultaConRelaciones()
    {
        return _contexto.Atenciones
            .AsNoTracking()
            .Include(a => a.Cliente)
            .Include(a => a.Vehiculo)
                .ThenInclude(v => v.Modelo)
                    .ThenInclude(m => m.Marca)
            .Include(a => a.UsuarioRecepcion)
            .Include(a => a.TecnicoEnActividad);
    }

    /// <summary>
    /// Persiste los cambios y traduce los conflictos conocidos.
    /// </summary>
    private async Task GuardarAsync(
       EntityEntry<Atencion> entrada)
    {
        try
        {
            await _contexto.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConflictoConcurrenciaException(
                "La atención fue modificada o eliminada por otra operación. Actualice la información antes de continuar.",
                ex);
        }
        catch (DbUpdateException ex)
            when (EsDuplicadoDeVehiculoActivo(ex))
        {
            throw new ValidacionException(
                "El vehículo ya tiene una atención activa. Actualizá el listado antes de continuar.",
                nameof(Atencion.IdVehiculo)
            );
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
    private static bool EsDuplicadoDeVehiculoActivo(
        DbUpdateException excepcion)
    {
       return excepcion.InnerException is SqlException sqlEx &&
            sqlEx.Errors.Cast<SqlError>().Any(error =>
                (error.Number == 2601 || error.Number == 2627) &&
                error.Message.Contains(
                    IndiceVehiculoActivo, StringComparison.OrdinalIgnoreCase));
    }


}
