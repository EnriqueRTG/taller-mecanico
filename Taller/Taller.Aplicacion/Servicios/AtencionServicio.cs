
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.Servicios;

/// <summary>
/// Proporciona las operaciones de negocio relacionadas
/// con la gestión y ciclo de vida de las atenciones.
/// </summary>
public sealed class AtencionServicio
{
    private readonly IAtencionRepositorio _atencionRepositorio;
    private readonly IClienteRepositorio _clienteRepositorio;
    private readonly IVehiculoRepositorio _vehiculoRepositorio;
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly IConceptoRepositorio _conceptoRepositorio;
    private readonly SesionUsuario _sesionUsuario; //
    

    /// <summary>
    /// Inicializa el servicio con los repositorios necesarios
    /// para gestionar una atención.
    /// </summary>
    public AtencionServicio(
        IAtencionRepositorio atencionRepositorio,
        IClienteRepositorio clienteRepositorio,
        IVehiculoRepositorio vehiculoRepositorio,
        IUsuarioRepositorio usuarioRepositorio,
        IConceptoRepositorio conceptoRepositorio,
        SesionUsuario sesionUsuario)
    {
        _atencionRepositorio = atencionRepositorio;
        _clienteRepositorio = clienteRepositorio;
        _vehiculoRepositorio = vehiculoRepositorio;
        _usuarioRepositorio = usuarioRepositorio;
        _conceptoRepositorio = conceptoRepositorio;
        _sesionUsuario = sesionUsuario; //
    }

    /// <summary>
    /// Lista las atenciones que se encuentran abiertas
    /// o en proceso.
    /// </summary>
    public async Task<List<Atencion>> ListarActivasAsync()
    {
        return await _atencionRepositorio.ListarActivasAsync();
    }

    /// <summary>
    /// Obtiene una atención por su identificador.
    /// </summary>
    /// <param name="idAtencion">Identificador de la atención.</param>
    /// <returns>La atención encontrada o null si no existe.</returns>
    public async Task<Atencion?> ObtenerPorIdAsync(
        int idAtencion)
    {
        ValidarId(idAtencion, nameof(idAtencion));

        return await _atencionRepositorio.ObtenerPorIdAsync(idAtencion);
    }

    /// <summary>
    /// Lista todas las atenciones asociadas a un cliente.
    /// </summary>
    public async Task<List<Atencion>> ListarPorClienteAsync(
        int idCliente)
    {
        ValidarId(idCliente, nameof(idCliente));

        return await _atencionRepositorio.ListarPorClienteAsync(idCliente);
    }

    /// <summary>
    /// Lista todas las atenciones asociadas a un vehículo.
    /// </summary>
    public async Task<List<Atencion>> ListarPorVehiculoAsync(
        int idVehiculo)
    {
        ValidarId(idVehiculo, nameof(idVehiculo));

        return await _atencionRepositorio.ListarPorVehiculoAsync(idVehiculo);
    }

    /// <summary>
    /// Obtiene los servicios disponibles para que Recepción
    /// seleccione el concepto del cargo inicial.
    /// </summary>
    public async Task<List<Concepto>> ListarServiciosParaIngresoAsync()
    {
        await ObtenerAdministrativoActualAsync();

        return await _conceptoRepositorio.ListarServiciosActivosAsync();
    }

    /// <summary>
    /// Registra una nueva atención vinculando cliente,
    /// vehículo y usuario de recepción.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Se produce cuando alguna entidad asociada no existe,
    /// se encuentra inactiva o el vehículo ya posee una atención activa.
    /// </exception>
    public async Task<Atencion> RegistrarAsync(
        int idCliente,
        int idVehiculo,
        int idUsuarioRecepcion,
        string motivoConsulta)
    {
        ValidarId(idCliente, nameof(idCliente));
        ValidarId(idVehiculo, nameof(idVehiculo));
        ValidarId(idUsuarioRecepcion, nameof(idUsuarioRecepcion));

        motivoConsulta = NormalizarMotivoConsulta(motivoConsulta);

        await ValidarClienteAsync(idCliente);
        await ValidarVehiculoAsync(idVehiculo);
        await ValidarUsuarioRecepcionAsync(idUsuarioRecepcion);

        if (await _atencionRepositorio
            .ExisteAtencionActivaParaVehiculoAsync(idVehiculo))
        {
            throw new InvalidOperationException(
                "El vehículo ya posee una atención activa.");
        }

        var atencion = new Atencion
        {
            IdCliente = idCliente,
            IdVehiculo = idVehiculo,
            IdUsuarioRecepcion = idUsuarioRecepcion,
            FechaApertura = DateTime.Now,
            MotivoConsulta = motivoConsulta,
            Estado = EstadoAtencion.Abierta,
            FechaCierre = null
        };

        await _atencionRepositorio.AgregarAsync(atencion);

        return atencion;
    }

    /// <summary>
    /// Modifica los datos básicos de una atención existente.
    /// No permite modificar directamente su estado ni sus fechas.
    /// </summary>
    public async Task ModificarAsync(
        int idAtencion,
        int idCliente,
        int idVehiculo,
        string motivoConsulta)
    {
        ValidarId(idAtencion, nameof(idAtencion));
        ValidarId(idCliente, nameof(idCliente));
        ValidarId(idVehiculo, nameof(idVehiculo));

        var atencion =
            await ObtenerAtencionRequeridaAsync(idAtencion);

        if (atencion.Estado == EstadoAtencion.Cerrada)
        {
            throw new InvalidOperationException(
                "No se puede modificar una atención cerrada.");
        }

        if (atencion.Estado == EstadoAtencion.Cancelada)
        {
            throw new InvalidOperationException(
                "No se puede modificar una atención cancelada.");
        }

        motivoConsulta = NormalizarMotivoConsulta(motivoConsulta);

        await ValidarClienteAsync(idCliente);
        await ValidarVehiculoAsync(idVehiculo);

        // Si se cambia el vehículo, se verifica que el nuevo
        // no tenga otra atención activa.
        if (atencion.IdVehiculo != idVehiculo &&
            await _atencionRepositorio
                .ExisteAtencionActivaParaVehiculoAsync(idVehiculo))
        {
            throw new InvalidOperationException(
                "El vehículo seleccionado ya posee una atención activa.");
        }

        atencion.IdCliente = idCliente;
        atencion.IdVehiculo = idVehiculo;
        atencion.MotivoConsulta = motivoConsulta;

        await _atencionRepositorio.ActualizarAsync(atencion);
    }

    /// <summary>
    /// Cambia una atención abierta al estado EnProceso.
    /// </summary>
    public async Task IniciarProcesoAsync(
        int idAtencion)
    {
        ValidarId(idAtencion, nameof(idAtencion));

        var atencion =
            await ObtenerAtencionRequeridaAsync(idAtencion);

        if (atencion.Estado != EstadoAtencion.Abierta)
        {
            throw new InvalidOperationException(
                "Solo una atención abierta puede pasar a estado EnProceso.");
        }

        atencion.Estado = EstadoAtencion.EnEjecucion;

        await _atencionRepositorio.ActualizarAsync(atencion);
    }

    /// <summary>
    /// Confirma el ingreso al circuito técnico y registra
    /// el cargo inicial previamente autorizado por el cliente.
    /// </summary>
    /// <remarks>
    /// El importe corresponde al servicio mínimo de ingreso
    /// e inspección acordado, exigible desde este momento.
    /// No significa que el informe técnico esté terminado.
    /// </remarks>
    public async Task EnviarADiagnosticoAsync(
        int idAtencion,
        int idConcepto,
        decimal importe,
        bool clienteAutoriza)
    {
        ValidarId(idAtencion, nameof(idAtencion));
        ValidarId(idConcepto, nameof(idConcepto));

        var administrativo = await ObtenerAdministrativoActualAsync();

        if (!clienteAutoriza)
        {
            throw new InvalidOperationException(
                "Debe registrar la autorización del cliente para el servicio inicial.");
        }

        if (importe <= 0 ||
            importe > 9999999999999999.99m ||
            decimal.Round(importe, 2) != importe)
        {
            throw new ArgumentException(
                "El importe debe ser positivo, tener como máximo dos decimales y estar dentro del rango permitido.",
                nameof(importe));
        }

        var atencion = await ObtenerAtencionRequeridaAsync(
            idAtencion);

        if (atencion.Estado != EstadoAtencion.Abierta ||
            atencion.FechaCierre is not null)
        {
            throw new InvalidOperationException(
                "Solo una atención abierta puede enviarse a diagnóstico.");
        }

        if (atencion.IdTecnicoEnActividad is not null ||
            atencion.ActividadTecnica is not null)
        {
            throw new InvalidOperationException(
                "La atención tiene una actividad reservada.");
        }

        var concepto = await _conceptoRepositorio.ObtenerPorIdAsync(
            idConcepto);

        if (concepto is null ||
            !concepto.Activo ||
            !concepto.Categoria.Activo)
        {
            throw new InvalidOperationException(
                "El concepto seleccionado no existe o su concepto o categoría están inactivos.");
        }

        if (concepto.Tipo != TipoConcepto.Servicio)
        {
            throw new InvalidOperationException(
                "El cargo inicial debe corresponder a un servicio.");
        }

        var descripcion = concepto.Nombre.Trim();

        if (string.IsNullOrWhiteSpace(descripcion) ||
            descripcion.Length > 500)
        {
            throw new InvalidOperationException(
                "El concepto no tiene una descripción válida para presupuestarlo.");
        }

        await ValidarClienteAsync(atencion.IdCliente);
        await ValidarVehiculoAsync(atencion.IdVehiculo);

        var fecha = DateTime.Now;
        var estadoAnterior = atencion.Estado;

        var presupuesto = new Presupuesto
        {
            Tipo = TipoPresupuesto.Diagnostico,
            IdAtencion = atencion.IdAtencion,
            IdDiagnostico = null,
            IdUsuarioCreacion = administrativo.Id,
            FechaCreacion = fecha,
            Estado = EstadoPresupuesto.Aceptado,
            FechaPresentacion = fecha,
            IdUsuarioDecision = administrativo.Id,
            FechaDecision = fecha,
            Observaciones =
                "El cliente autorizó el cargo inicial al confirmar el ingreso al circuito técnico."
        };

        presupuesto.Detalles.Add(new PresupuestoDetalle
        {
            IdConcepto = concepto.IdConcepto,
            Descripcion = descripcion,
            Cantidad = 1m,
            PrecioUnitario = importe,

            // Cargo inicial devengado al confirmar el ingreso.
            // No representa la finalización del informe técnico.
            EstadoEjecucion = EstadoEjecucionDetalle.Completado,
            CantidadEjecutada = 1m,
            ObservacionesEjecucion =
                "Cargo inicial generado al confirmar el ingreso, conforme a la autorización del cliente."
        });

        atencion.Estado = EstadoAtencion.PendienteDiagnostico;

        var historial = new HistorialAtencion
        {
            IdAtencion = atencion.IdAtencion,
            IdUsuario = administrativo.Id,
            Fecha = fecha,
            Accion = "AtencionEnviadaADiagnostico",
            EstadoAnterior = estadoAnterior,
            EstadoNuevo = atencion.Estado,
            Observaciones =
                "Recepción confirmó el ingreso y registró la autorización del cargo inicial."
        };

        await _atencionRepositorio.EnviarADiagnosticoAsync(
            atencion,
            presupuesto,
            historial);
    }

    /// <summary>
    /// Reserva el diagnóstico inicial para el técnico autenticado
    /// y cambia la atención a evaluación técnica.
    /// </summary>
    /// <remarks>
    /// La reserva y el evento del historial se guardan juntos.
    /// La versión de la atención permite detectar si otra operación
    /// la modificó después de consultar su disponibilidad.
    /// </remarks>
    public async Task TomarDiagnosticoAsync(
        int idAtencion)
    {
        ValidarId(idAtencion, nameof(idAtencion));

        var tecnico = await ObtenerTecnicoActualAsync();

        var atencion = await ObtenerAtencionRequeridaAsync(
            idAtencion);

        if (atencion.Estado != EstadoAtencion.PendienteDiagnostico)
        {
            throw new InvalidOperationException(
                "Solo puede iniciar el diagnóstico de una atención pendiente de diagnóstico.");
        }

        if (atencion.IdTecnicoEnActividad is not null ||
            atencion.ActividadTecnica is not null)
        {
            throw new InvalidOperationException(
                "La atención ya tiene una actividad reservada. Actualice la información.");
        }

        var estadoAnterior = atencion.Estado;

        atencion.IdTecnicoEnActividad = tecnico.Id;
        atencion.ActividadTecnica = ActividadTecnica.Diagnostico;
        atencion.Estado = EstadoAtencion.EnEvaluacionTecnica;

        var historial = new HistorialAtencion
        {
            IdAtencion = atencion.IdAtencion,
            IdUsuario = tecnico.Id,
            Fecha = DateTime.Now,
            Accion = "ActividadTomada",
            EstadoAnterior = estadoAnterior,
            EstadoNuevo = atencion.Estado,
            Observaciones = "El técnico tomó el diagnóstico inicial."
        };

        await _atencionRepositorio.ActualizarConHistorialAsync(
            atencion,
            historial);
    }

    /// <summary>
    /// Libera la reserva del diagnóstico inicial sin completarlo
    /// y devuelve la atención a la espera de un técnico.
    /// </summary>
    /// <remarks>
    /// Solo puede liberar la actividad el técnico que la tomó.
    /// No elimina informes ni cancela el cargo inicial autorizado.
    /// </remarks>
    public async Task LiberarDiagnosticoAsync(
        int idAtencion)
    {
        ValidarId(idAtencion, nameof(idAtencion));

        var tecnico = await ObtenerTecnicoActualAsync();

        var atencion = await ObtenerAtencionRequeridaAsync(
            idAtencion);

        if (atencion.Estado != EstadoAtencion.EnEvaluacionTecnica ||
            atencion.ActividadTecnica != ActividadTecnica.Diagnostico)
        {
            throw new InvalidOperationException(
                "La atención no tiene un diagnóstico inicial en actividad.");
        }

        if (atencion.IdTecnicoEnActividad != tecnico.Id)
        {
            throw new InvalidOperationException(
                "Solo el técnico que tomó el diagnóstico puede liberar esta actividad.");
        }

        var estadoAnterior = atencion.Estado;

        atencion.IdTecnicoEnActividad = null;
        atencion.ActividadTecnica = null;
        atencion.Estado = EstadoAtencion.PendienteDiagnostico;

        var historial = new HistorialAtencion
        {
            IdAtencion = atencion.IdAtencion,
            IdUsuario = tecnico.Id,
            Fecha = DateTime.Now,
            Accion = "ActividadLiberada",
            EstadoAnterior = estadoAnterior,
            EstadoNuevo = atencion.Estado,
            Observaciones =
                "El técnico liberó el diagnóstico inicial sin completar el informe."
        };

        await _atencionRepositorio.ActualizarConHistorialAsync(
            atencion,
            historial);
    }

    /// <summary>
    /// Cancela una atención que aún no se encuentra cerrada.
    /// </summary>
    public async Task CancelarAsync(
        int idAtencion)
    {
        ValidarId(idAtencion, nameof(idAtencion));

        var atencion =
            await ObtenerAtencionRequeridaAsync(idAtencion);

        if (atencion.Estado == EstadoAtencion.Cerrada)
        {
            throw new InvalidOperationException(
                "Una atención cerrada no puede ser cancelada.");
        }

        if (atencion.Estado == EstadoAtencion.Cancelada)
        {
            throw new InvalidOperationException(
                "La atención ya se encuentra cancelada.");
        }

        atencion.Estado = EstadoAtencion.Cancelada;

        await _atencionRepositorio.ActualizarAsync(atencion);
    }

    /// <summary>
    /// Cierra una atención y registra la fecha de cierre.
    /// El cierre representa también la entrega del vehículo.
    /// </summary>
    public async Task CerrarAsync(
        int idAtencion)
    {
        ValidarId(idAtencion, nameof(idAtencion));

        var atencion =
            await ObtenerAtencionRequeridaAsync(idAtencion);

        if (atencion.Estado == EstadoAtencion.Cerrada)
        {
            throw new InvalidOperationException(
                "La atención ya se encuentra cerrada.");
        }

        if (atencion.Estado == EstadoAtencion.Cancelada)
        {
            throw new InvalidOperationException(
                "Una atención cancelada no puede ser cerrada.");
        }

        if (atencion.Estado != EstadoAtencion.EnEjecucion)
        {
            throw new InvalidOperationException(
                "Solo una atención en proceso puede ser cerrada.");
        }

        /*
         * MÁS ADELANTE:
         *
         * Antes de cerrar deberán verificarse reglas como:
         *
         * - Proceso técnico terminado.
         * - Comprobante generado.
         * - Saldo pendiente igual a cero.
         *
         * Cuando esas entidades estén implementadas,
         * estas validaciones se incorporarán aquí.
         */

        atencion.Estado = EstadoAtencion.Cerrada;
        atencion.FechaCierre = DateTime.Now;

        await _atencionRepositorio.ActualizarAsync(atencion);
    }

    // -----------------------------------------------------------------
    // VALIDACIONES PRIVADAS
    // -----------------------------------------------------------------

    /// <summary>
    /// Obtiene una atención y genera una excepción si no existe.
    /// </summary>
    private async Task<Atencion> ObtenerAtencionRequeridaAsync(
        int idAtencion)
    {
        var atencion =
            await _atencionRepositorio.ObtenerPorIdAsync(idAtencion);

        if (atencion is null)
        {
            throw new InvalidOperationException(
                "La atención seleccionada no existe.");
        }

        return atencion;
    }

    /// <summary>
    /// Verifica que el cliente exista y se encuentre activo.
    /// </summary>
    private async Task ValidarClienteAsync(
        int idCliente)
    {
        var cliente =
            await _clienteRepositorio.ObtenerPorIdAsync(idCliente);

        if (cliente is null)
        {
            throw new InvalidOperationException(
                "El cliente seleccionado no existe.");
        }

        if (!cliente.Activo)
        {
            throw new InvalidOperationException(
                "El cliente seleccionado se encuentra inactivo.");
        }
    }

    /// <summary>
    /// Verifica que el vehículo exista y se encuentre activo.
    /// </summary>
    private async Task ValidarVehiculoAsync(
        int idVehiculo)
    {
        var vehiculo =
            await _vehiculoRepositorio.ObtenerPorIdAsync(idVehiculo);

        if (vehiculo is null)
        {
            throw new InvalidOperationException(
                "El vehículo seleccionado no existe.");
        }

        if (!vehiculo.Activo)
        {
            throw new InvalidOperationException(
                "El vehículo seleccionado se encuentra inactivo.");
        }
    }

    /// <summary>
    /// Verifica que el usuario de recepción exista y
    /// se encuentre activo.
    /// </summary>
    private async Task ValidarUsuarioRecepcionAsync(
        int idUsuarioRecepcion)
    {
        var usuario =
            await _usuarioRepositorio.ObtenerPorIdAsync(
                idUsuarioRecepcion);

        if (usuario is null)
        {
            throw new InvalidOperationException(
                "El usuario de recepción no existe.");
        }

        if (!usuario.Activo)
        {
            throw new InvalidOperationException(
                "El usuario de recepción se encuentra inactivo.");
        }

        /*
         * Si más adelante se quiere restringir estrictamente
         * quién puede abrir atenciones, acá podemos validar:
         *
         * usuario.Rol.Nombre == "Administrativo"
         *
         * o los roles que finalmente correspondan.
         */
    }

    /// <summary>
    /// Valida que un identificador sea mayor a cero.
    /// </summary>
    private static void ValidarId(
        int id,
        string nombreParametro)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "El identificador proporcionado no es válido.",
                nombreParametro);
        }
    }

    /// <summary>
    /// Normaliza y valida el motivo informado al abrir
    /// una atención.
    /// </summary>
    private static string NormalizarMotivoConsulta(
        string? motivoConsulta)
    {
        if (string.IsNullOrWhiteSpace(motivoConsulta))
        {
            throw new ArgumentException(
                "El motivo de consulta es obligatorio.",
                nameof(motivoConsulta));
        }

        motivoConsulta = motivoConsulta.Trim();

        if (motivoConsulta.Length > 500)
        {
            throw new ArgumentException(
                "El motivo de consulta no puede superar los 500 caracteres.",
                nameof(motivoConsulta));
        }

        return motivoConsulta;
    }

    /// <summary>
    /// Obtiene al técnico autenticado y verifica que continúe
    /// activo y autorizado para realizar operaciones técnicas.
    /// </summary>
    private async Task<Usuario> ObtenerTecnicoActualAsync()
    {
        var usuarioSesion = _sesionUsuario.UsuarioActual;

        if (usuarioSesion is null)
        {
            throw new InvalidOperationException(
                "Debe iniciar sesión para realizar esta operación.");
        }

        var usuario = await _usuarioRepositorio.ObtenerPorIdAsync(
            usuarioSesion.Id);

        if (usuario is null || !usuario.Activo)
        {
            throw new InvalidOperationException(
                "El usuario de la sesión no existe o está inactivo.");
        }

        if (usuario.RolId != RolesSistema.TecnicoId)
        {
            throw new InvalidOperationException(
                "Solo un técnico puede tomar esta actividad.");
        }

        return usuario;
    }

    /// <summary>
    /// Obtiene al usuario autenticado y verifica que sea
    /// un administrativo activo.
    /// </summary>
    private async Task<Usuario> ObtenerAdministrativoActualAsync()
    {
        var usuarioSesion = _sesionUsuario.UsuarioActual;

        if (usuarioSesion is null)
        {
            throw new InvalidOperationException(
                "Debe iniciar sesión para realizar esta operación.");
        }

        var usuario = await _usuarioRepositorio.ObtenerPorIdAsync(
            usuarioSesion.Id);

        if (usuario is null || !usuario.Activo)
        {
            throw new InvalidOperationException(
                "El usuario de la sesión no existe o está inactivo.");
        }

        if (usuario.RolId != RolesSistema.AdministrativoId)
        {
            throw new InvalidOperationException(
                "Solo Recepción puede enviar el vehículo a diagnóstico.");
        }

        return usuario;
    }
}
