using Taller.Dominio.Tiempo;
using Taller.Aplicacion.Seguridad;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Excepciones;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
using Taller.Aplicacion.DTOs.Atenciones;

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
    private readonly SesionUsuario _sesionUsuario;
    /// <summary>
    /// Inicializa el servicio con los repositorios necesarios
    /// para gestionar una atención.
    /// </summary>
    public AtencionServicio(IAtencionRepositorio atencionRepositorio, IClienteRepositorio clienteRepositorio, IVehiculoRepositorio vehiculoRepositorio, IUsuarioRepositorio usuarioRepositorio, IConceptoRepositorio conceptoRepositorio, SesionUsuario sesionUsuario)
    {
        _atencionRepositorio = atencionRepositorio;
        _clienteRepositorio = clienteRepositorio;
        _vehiculoRepositorio = vehiculoRepositorio;
        _usuarioRepositorio = usuarioRepositorio;
        _conceptoRepositorio = conceptoRepositorio;
        _sesionUsuario = sesionUsuario;
    }

    /// <summary>
    /// Lista las atenciones que se encuentran abiertas
    /// o en proceso.
    /// </summary>
    public async Task<List<Atencion>> ListarActivasAsync()
    {
        await new AutorizacionOperativa(_usuarioRepositorio, _sesionUsuario).ExigirAsync();
        return await _atencionRepositorio.ListarActivasAsync();
    }

    /// <summary>
    /// Lista las atenciones para consulta operativa e histórica.
    /// </summary>
    public async Task<List<Atencion>> ListarTodasAsync()
    {
        await new AutorizacionOperativa(_usuarioRepositorio, _sesionUsuario).ExigirAsync();
        return await _atencionRepositorio.ListarTodasAsync();
    }

    /// <summary>
    /// Obtiene una atención por su identificador.
    /// </summary>
    /// <param name = "idAtencion">Identificador de la atención.</param>
    /// <returns>La atención encontrada o null si no existe.</returns>
    public async Task<Atencion?> ObtenerPorIdAsync(int idAtencion)
    {
        await new AutorizacionOperativa(_usuarioRepositorio, _sesionUsuario).ExigirAsync();
        ValidarId(idAtencion, nameof(idAtencion));
        return await _atencionRepositorio.ObtenerPorIdAsync(idAtencion);
    }

    /// <summary>
    /// Lista todas las atenciones asociadas a un cliente.
    /// </summary>
    public async Task<List<Atencion>> ListarPorClienteAsync(int idCliente)
    {
        await new AutorizacionOperativa(_usuarioRepositorio, _sesionUsuario).ExigirAsync();
        ValidarId(idCliente, nameof(idCliente));
        return await _atencionRepositorio.ListarPorClienteAsync(idCliente);
    }

    /// <summary>
    /// Lista todas las atenciones asociadas a un vehículo.
    /// </summary>
    public async Task<List<Atencion>> ListarPorVehiculoAsync(int idVehiculo)
    {
        await new AutorizacionOperativa(_usuarioRepositorio, _sesionUsuario).ExigirAsync();
        ValidarId(idVehiculo, nameof(idVehiculo));
        return await _atencionRepositorio.ListarPorVehiculoAsync(idVehiculo);
    }

    /// <summary>
    /// Lista los servicios disponibles para preparar
    /// el ingreso autorizado por el cliente.
    /// </summary>
    public async Task<List<ServicioIngresoDto>> ListarServiciosDisponiblesParaIngresoAsync()
    {
        await ObtenerAdministrativoActualAsync();
        var conceptos = await _conceptoRepositorio.ListarServiciosDisponiblesParaIngresoAsync();
        return conceptos.Select(concepto => new ServicioIngresoDto { IdConcepto = concepto.IdConcepto, Nombre = concepto.Nombre, Descripcion = concepto.Descripcion, Categoria = concepto.Categoria.Nombre, PrecioReferencia = concepto.PrecioReferencia }).ToList();
    }

    /// <summary>
    /// Registra una nueva atención vinculando cliente,
    /// vehículo y usuario de recepción.
    /// </summary>
    /// <exception cref = "InvalidOperationException">
    /// Se produce cuando alguna entidad asociada no existe,
    /// se encuentra inactiva o el vehículo ya posee una atención activa.
    /// </exception>
    public async Task<Atencion> RegistrarAsync(int idCliente, int idVehiculo, int idUsuarioRecepcion, string motivoConsulta, TipoSolicitudAtencion tipoSolicitud = TipoSolicitudAtencion.Problema, bool enviarATaller = false)
    {
        if (!Enum.IsDefined(tipoSolicitud))
            throw new ValidacionException("Seleccione el tipo de solicitud.");
        var recepcion = await ObtenerAdministrativoActualAsync();
        if (recepcion.Id != idUsuarioRecepcion)
            throw new ValidacionException("Usuario de recepción inválido.");
        ValidarId(idCliente, nameof(idCliente));
        ValidarId(idVehiculo, nameof(idVehiculo));
        ValidarId(idUsuarioRecepcion, nameof(idUsuarioRecepcion));
        motivoConsulta = NormalizarMotivoConsulta(motivoConsulta);
        await ValidarClienteAsync(idCliente);
        await ValidarVehiculoAsync(idVehiculo);
        await ValidarUsuarioRecepcionAsync(idUsuarioRecepcion);
        if (await _atencionRepositorio.ExisteAtencionActivaParaVehiculoAsync(idVehiculo))
        {
            throw new ValidacionException("El vehículo ya posee una atención activa.");
        }

        var atencion = new Atencion
        {
            IdCliente = idCliente,
            IdVehiculo = idVehiculo,
            IdUsuarioRecepcion = idUsuarioRecepcion,
            TipoSolicitud = tipoSolicitud,
            FechaApertura = FechaHoraTaller.Ahora,
            MotivoConsulta = motivoConsulta,
            Estado = enviarATaller ? EstadoAtencion.EnEvaluacionTecnica : EstadoAtencion.Abierta,
            FechaCierre = null
        };
        atencion.Historial.Add(new HistorialAtencion { IdUsuario = recepcion.Id, Fecha = atencion.FechaApertura, Accion = "AtencionAbierta", EstadoAnterior = null, EstadoNuevo = EstadoAtencion.Abierta, Observaciones = $"Solicitud: {tipoSolicitud}. Motivo registrado por Recepción." });
        // Ambos eventos y la atención se insertan en el mismo SaveChanges del repositorio.
        if (enviarATaller)
            atencion.Historial.Add(new HistorialAtencion { IdUsuario = recepcion.Id, Fecha = atencion.FechaApertura, Accion = "EvaluacionSolicitada", EstadoAnterior = EstadoAtencion.Abierta, EstadoNuevo = atencion.Estado, Observaciones = tipoSolicitud == TipoSolicitudAtencion.ServicioDefinido ? "Solicitud enviada para preparar presupuesto. No autoriza ejecución ni cargos." : "Solicitud enviada para diagnóstico y presupuesto. No autoriza tareas cobrables." });
        await _atencionRepositorio.AgregarAsync(atencion);
        return atencion;
    }

    /// <summary>
    /// Modifica los datos de recepción antes de confirmar
    /// el ingreso al circuito técnico.
    /// </summary>
    public async Task ModificarAsync(int idAtencion, int idCliente, int idVehiculo, string motivoConsulta, TipoSolicitudAtencion? tipoSolicitud = null, byte[]? version = null)
    {
        if (tipoSolicitud.HasValue && !Enum.IsDefined(tipoSolicitud.Value))
            throw new ValidacionException("Tipo de solicitud inválido.");
        ValidarId(idAtencion, nameof(idAtencion));
        ValidarId(idCliente, nameof(idCliente));
        ValidarId(idVehiculo, nameof(idVehiculo));
        var administrativo = await ObtenerAdministrativoActualAsync();
        var atencion = await ObtenerAtencionRequeridaAsync(idAtencion);
        AutorizacionOperativa.Version(version, atencion.Version);
        if (atencion.Estado != EstadoAtencion.Abierta || atencion.FechaCierre is not null)
        {
            throw new ValidacionException("Solo pueden modificarse los datos de una atención abierta, " + "antes de confirmar su ingreso a diagnóstico.");
        }

        if (atencion.IdTecnicoEnActividad is not null || atencion.ActividadTecnica is not null)
        {
            throw new ValidacionException("No puede modificarse una atención con una actividad reservada.");
        }

        motivoConsulta = NormalizarMotivoConsulta(motivoConsulta);
        await ValidarClienteAsync(idCliente);
        await ValidarVehiculoAsync(idVehiculo);
        if (atencion.IdVehiculo != idVehiculo && await _atencionRepositorio.ExisteAtencionActivaParaVehiculoAsync(idVehiculo))
        {
            throw new ValidacionException("El vehículo seleccionado ya posee una atención activa.");
        }

        // No genera una actualización ni un evento si nada cambió.
        if (atencion.IdCliente == idCliente && atencion.IdVehiculo == idVehiculo && atencion.MotivoConsulta == motivoConsulta && (!tipoSolicitud.HasValue || atencion.TipoSolicitud == tipoSolicitud.Value))
        {
            return;
        }

        var observaciones = $"Cliente: {atencion.IdCliente} → {idCliente}. " + $"Vehículo: {atencion.IdVehiculo} → {idVehiculo}. " + $"Motivo de consulta modificado: " + $"{(atencion.MotivoConsulta != motivoConsulta ? "Sí" : "No")}.";
        atencion.IdCliente = idCliente;
        atencion.IdVehiculo = idVehiculo;
        atencion.MotivoConsulta = motivoConsulta;
        atencion.TipoSolicitud = tipoSolicitud ?? atencion.TipoSolicitud;
        var historial = new HistorialAtencion
        {
            IdAtencion = atencion.IdAtencion,
            IdUsuario = administrativo.Id,
            Fecha = FechaHoraTaller.Ahora,
            Accion = "DatosRecepcionModificados",
            EstadoAnterior = atencion.Estado,
            EstadoNuevo = atencion.Estado,
            Observaciones = observaciones + $" Tipo de solicitud: {atencion.TipoSolicitud}."};
        await _atencionRepositorio.ActualizarConHistorialAsync(atencion, historial);
    }

    /// <summary>Autoriza los servicios iniciales seleccionados, sin registrarlos como ejecutados ni cobrados.</summary>
    public async Task ConfirmarIngresoAsync(ConfirmarIngresoDto datos)
    {
        ArgumentNullException.ThrowIfNull(datos);
        var administrativo = await ObtenerAdministrativoActualAsync();
        if (datos.IdAtencion <= 0)
        {
            throw new ValidacionException("Debe seleccionar una atención válida.", nameof(datos.IdAtencion));
        }

        if (!datos.ClienteAutoriza)
        {
            throw new ValidacionException("Debe registrar la autorización del cliente " + "para los servicios e importes seleccionados.", nameof(datos.ClienteAutoriza));
        }

        if (datos.Servicios is null || datos.Servicios.Count == 0)
        {
            throw new ValidacionException("Debe agregar al menos un servicio de ingreso.", nameof(datos.Servicios));
        }

        // Conserva el conjunto recibido antes de consultar la base.
        var servicios = datos.Servicios.ToArray();
        if (servicios.Any(servicio => servicio is null))
        {
            throw new ValidacionException("La selección contiene un servicio inválido.", nameof(datos.Servicios));
        }

        if (servicios.Any(servicio => servicio.IdConcepto <= 0))
        {
            throw new ValidacionException("Todos los servicios deben tener un identificador válido.", nameof(datos.Servicios));
        }

        if (servicios.GroupBy(servicio => servicio.IdConcepto).Any(grupo => grupo.Count() > 1))
        {
            throw new ValidacionException("No puede agregar el mismo servicio más de una vez. " + "Modifique su cantidad si corresponde.", nameof(datos.Servicios));
        }

        var atencion = await ObtenerAtencionRequeridaAsync(datos.IdAtencion);
        if (datos.VersionAtencion is null || datos.VersionAtencion.Length != 8 || !datos.VersionAtencion.SequenceEqual(atencion.Version))
            throw new ConflictoConcurrenciaException("La atención cambió desde que abrió el detalle. Vuelva a abrirlo antes de confirmar.");
        if (atencion.Estado != EstadoAtencion.Abierta || atencion.FechaCierre is not null)
        {
            throw new ValidacionException("Solo una atención abierta puede confirmar su ingreso.");
        }

        if (atencion.IdTecnicoEnActividad is not null || atencion.ActividadTecnica is not null)
        {
            throw new ValidacionException("La atención tiene una actividad técnica reservada.");
        }

        await ValidarClienteAsync(atencion.IdCliente);
        await ValidarVehiculoAsync(atencion.IdVehiculo);
        // Consulta nuevamente la disponibilidad.
        // No confía únicamente en lo mostrado por el formulario.
        var conceptosDisponibles = await _conceptoRepositorio.ListarServiciosDisponiblesParaIngresoAsync();
        var conceptosPorId = conceptosDisponibles.ToDictionary(concepto => concepto.IdConcepto);
        const decimal cantidadMaxima = 999999999999999.999m;
        const decimal importeMaximo = 9999999999999999.99m;
        var detalles = new List<PresupuestoDetalle>();
        decimal total = 0m;
        foreach (var servicio in servicios)
        {
            if (!conceptosPorId.TryGetValue(servicio.IdConcepto, out var concepto))
            {
                throw new ValidacionException("Uno de los servicios seleccionados ya no está " + "disponible para ingreso. Actualice el catálogo.", nameof(datos.Servicios));
            }

            if (servicio.Cantidad <= 0m || servicio.Cantidad > cantidadMaxima || decimal.Round(servicio.Cantidad, 3) != servicio.Cantidad)
            {
                throw new ValidacionException($"La cantidad de «{concepto.Nombre}» debe ser positiva, " + "tener como máximo tres decimales y estar " + "dentro del rango permitido.", nameof(datos.Servicios));
            }

            if (servicio.PrecioUnitario < 0m || servicio.PrecioUnitario > importeMaximo || decimal.Round(servicio.PrecioUnitario, 2) != servicio.PrecioUnitario)
            {
                throw new ValidacionException($"El precio de «{concepto.Nombre}» no puede ser negativo " + "y debe tener como máximo dos decimales.", nameof(datos.Servicios));
            }

            // Comprueba el límite antes de multiplicar.
            if (servicio.PrecioUnitario > importeMaximo / servicio.Cantidad)
            {
                throw new ValidacionException($"El subtotal de «{concepto.Nombre}» supera " + "el importe permitido.", nameof(datos.Servicios));
            }

            decimal subtotal = decimal.Round(servicio.Cantidad * servicio.PrecioUnitario, 2, MidpointRounding.AwayFromZero);
            if (subtotal > importeMaximo - total)
            {
                throw new ValidacionException("El total de los servicios supera el importe permitido.", nameof(datos.Servicios));
            }

            total += subtotal;
            string descripcion = concepto.Nombre.Trim();
            if (string.IsNullOrWhiteSpace(descripcion) || descripcion.Length > 500)
            {
                throw new ValidacionException("Uno de los servicios no tiene una descripción " + "válida para registrarse.", nameof(datos.Servicios));
            }

            detalles.Add(new PresupuestoDetalle { IdConcepto = concepto.IdConcepto, AceptadoPorCliente = true, Descripcion = descripcion, Cantidad = servicio.Cantidad, PrecioUnitario = servicio.PrecioUnitario, EstadoEjecucion = EstadoEjecucionDetalle.NoIniciado, CantidadEjecutada = 0m });
        }

        var fecha = FechaHoraTaller.Ahora;
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
            Observaciones = "Recepción registró la autorización del cliente para " + "los servicios iniciales y sus condiciones económicas.",
            Detalles = detalles
        };
        var estadoAnterior = atencion.Estado;
        atencion.Estado = EstadoAtencion.EnEvaluacionTecnica;
        var historial = new HistorialAtencion
        {
            IdAtencion = atencion.IdAtencion,
            IdUsuario = administrativo.Id,
            Fecha = fecha,
            Accion = "ServiciosInicialesAutorizados",
            EstadoAnterior = estadoAnterior,
            EstadoNuevo = atencion.Estado,
            Observaciones = $"Se confirmó el ingreso con {detalles.Count} " + "servicio(s) autorizado(s), pendientes de ejecución."
        };
        await _atencionRepositorio.ConfirmarIngresoAsync(atencion, presupuesto, historial);
    }

    // -----------------------------------------------------------------
    // VALIDACIONES PRIVADAS
    // -----------------------------------------------------------------
    /// <summary>
    /// Obtiene una atención y genera una excepción si no existe.
    /// </summary>
    private async Task<Atencion> ObtenerAtencionRequeridaAsync(int idAtencion)
    {
        var atencion = await _atencionRepositorio.ObtenerPorIdAsync(idAtencion);
        if (atencion is null)
        {
            throw new ValidacionException("La atención seleccionada no existe.");
        }

        return atencion;
    }

    /// <summary>
    /// Verifica que el cliente exista y se encuentre activo.
    /// </summary>
    private async Task ValidarClienteAsync(int idCliente)
    {
        var cliente = await _clienteRepositorio.ObtenerPorIdAsync(idCliente);
        if (cliente is null)
        {
            throw new ValidacionException("El cliente seleccionado no existe.");
        }

        if (!cliente.Activo)
        {
            throw new ValidacionException("El cliente seleccionado se encuentra inactivo.");
        }
    }

    /// <summary>
    /// Verifica que el vehículo exista y se encuentre activo.
    /// </summary>
    private async Task ValidarVehiculoAsync(int idVehiculo)
    {
        var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(idVehiculo);
        if (vehiculo is null)
        {
            throw new ValidacionException("El vehículo seleccionado no existe.");
        }

        if (!vehiculo.Activo)
        {
            throw new ValidacionException("El vehículo seleccionado se encuentra inactivo.");
        }
    }

    /// <summary>
    /// Verifica que el usuario de recepción exista y
    /// se encuentre activo.
    /// </summary>
    private async Task ValidarUsuarioRecepcionAsync(int idUsuarioRecepcion)
    {
        var usuario = await _usuarioRepositorio.ObtenerPorIdAsync(idUsuarioRecepcion);
        if (usuario is null)
        {
            throw new ValidacionException("El usuario de recepción no existe.");
        }

        if (!UsuarioAutorizado.EstaHabilitado(usuario) || usuario.RolId != RolesSistema.AdministrativoId)
        {
            throw new ValidacionException("El usuario de recepción se encuentra inactivo.");
        }
    }

    /// <summary>
    /// Valida que un identificador sea mayor a cero.
    /// </summary>
    private static void ValidarId(int id, string nombreParametro)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El identificador proporcionado no es válido.", nombreParametro);
        }
    }

    /// <summary>
    /// Normaliza y valida el motivo informado al abrir
    /// una atención.
    /// </summary>
    private static string NormalizarMotivoConsulta(string? motivoConsulta)
    {
        if (string.IsNullOrWhiteSpace(motivoConsulta))
        {
            throw new ArgumentException("El motivo de consulta es obligatorio.", nameof(motivoConsulta));
        }

        motivoConsulta = motivoConsulta.Trim();
        if (motivoConsulta.Length > 500)
        {
            throw new ArgumentException("El motivo de consulta no puede superar los 500 caracteres.", nameof(motivoConsulta));
        }

        return motivoConsulta;
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
            throw new ValidacionException("Debe iniciar sesión para realizar esta operación.");
        }

        var usuario = await _usuarioRepositorio.ObtenerPorIdAsync(usuarioSesion.Id);
        if (!UsuarioAutorizado.EstaHabilitado(usuario))
        {
            throw new ValidacionException("El usuario de la sesión no existe o está inactivo.");
        }

        if (usuario.RolId != RolesSistema.AdministrativoId)
        {
            throw new ValidacionException("Solo Recepción puede enviar el vehículo a diagnóstico.");
        }

        return usuario;
    }
/// <summary>
/// Obtiene el servicio configurado para el ingreso
/// y comprueba que esté disponible para utilizarse.
/// </summary>
}
