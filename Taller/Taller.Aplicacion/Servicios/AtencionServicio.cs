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
    private readonly AutorizacionOperativa _acceso;
    /// <summary>
    /// Inicializa el servicio con los repositorios necesarios
    /// para gestionar una atención.
    /// </summary>
    public AtencionServicio(IAtencionRepositorio atencionRepositorio, IClienteRepositorio clienteRepositorio, IVehiculoRepositorio vehiculoRepositorio, IUsuarioRepositorio usuarioRepositorio, AutorizacionOperativa acceso)
    {
        _atencionRepositorio = atencionRepositorio;
        _clienteRepositorio = clienteRepositorio;
        _vehiculoRepositorio = vehiculoRepositorio;
        _usuarioRepositorio = usuarioRepositorio;
        _acceso = acceso;
    }

    /// <summary>
    /// Lista las atenciones que se encuentran abiertas
    /// o en proceso.
    /// </summary>
    public async Task<List<Atencion>> ListarActivasAsync()
    {
        await _acceso.ExigirAsync();
        return await _atencionRepositorio.ListarActivasAsync();
    }

    /// <summary>
    /// Lista las atenciones para consulta operativa e histórica.
    /// </summary>
    public async Task<List<Atencion>> ListarTodasAsync()
    {
        await _acceso.ExigirAsync();
        return await _atencionRepositorio.ListarTodasAsync();
    }

    /// <summary>
    /// Obtiene una atención por su identificador.
    /// </summary>
    /// <param name = "idAtencion">Identificador de la atención.</param>
    /// <returns>La atención encontrada o null si no existe.</returns>
    public async Task<Atencion?> ObtenerPorIdAsync(int idAtencion)
    {
        await _acceso.ExigirAsync();
        ValidarId(idAtencion, nameof(idAtencion));
        return await _atencionRepositorio.ObtenerPorIdAsync(idAtencion);
    }

    /// <summary>
    /// Lista todas las atenciones asociadas a un cliente.
    /// </summary>
    public async Task<List<Atencion>> ListarPorClienteAsync(int idCliente)
    {
        await _acceso.ExigirAsync();
        ValidarId(idCliente, nameof(idCliente));
        return await _atencionRepositorio.ListarPorClienteAsync(idCliente);
    }

    /// <summary>
    /// Lista todas las atenciones asociadas a un vehículo.
    /// </summary>
    public async Task<List<Atencion>> ListarPorVehiculoAsync(int idVehiculo)
    {
        await _acceso.ExigirAsync();
        ValidarId(idVehiculo, nameof(idVehiculo));
        return await _atencionRepositorio.ListarPorVehiculoAsync(idVehiculo);
    }

    /// <summary>
    /// Registra una nueva atención vinculando cliente,
    /// vehículo y usuario de recepción.
    /// </summary>
    /// <exception cref = "InvalidOperationException">
    /// Se produce cuando alguna entidad asociada no existe,
    /// se encuentra inactiva o el vehículo ya posee una atención activa.
    /// </exception>
    public Task<Atencion> RegistrarRecepcionAsync(RegistrarRecepcionDto datos)
    {
        ArgumentNullException.ThrowIfNull(datos);
        var solicitudes = ValidarSolicitudes(datos.Solicitudes);
        if (datos.Kilometraje is < 0) throw new ValidacionException("El kilometraje no puede ser negativo.");
        if (datos.ObservacionesRecepcion?.Trim().Length > 1000) throw new ValidacionException("Las observaciones de recepción admiten hasta 1000 caracteres.");
        var tipo = solicitudes.All(s => s.Tipo == TipoSolicitudIngreso.TrabajoSolicitado) ? TipoSolicitudAtencion.ServicioDefinido
            : solicitudes.All(s => s.Tipo == TipoSolicitudIngreso.ProblemaInformado) ? TipoSolicitudAtencion.Problema : TipoSolicitudAtencion.Mixta;
        var resumen = string.Join("; ", solicitudes.Select(s => s.Descripcion));
        return RegistrarAsync(datos.IdCliente, datos.IdVehiculo, datos.IdUsuarioRecepcion, resumen[..Math.Min(resumen.Length, 500)], tipo,
            solicitudes: solicitudes, kilometraje: datos.Kilometraje, observaciones: datos.ObservacionesRecepcion);
    }

    private static List<SolicitudIngresoDto> ValidarSolicitudes(IReadOnlyList<SolicitudIngresoDto>? solicitudes)
    {
        if (solicitudes is null || solicitudes.Count is < 1 or > 20 || solicitudes.Any(s => s is null))
            throw new ValidacionException("Registre entre 1 y 20 solicitudes del cliente.");
        if (solicitudes.Any(s => !Enum.IsDefined(s.Tipo) || string.IsNullOrWhiteSpace(s.Descripcion) || s.Descripcion.Trim().Length > 500))
            throw new ValidacionException("Cada solicitud requiere un tipo válido y una descripción de hasta 500 caracteres.");
        if (solicitudes.Select(s => s.Descripcion.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).Count() != solicitudes.Count)
            throw new ValidacionException("No repita la misma solicitud.");
        return solicitudes.Select(s => new SolicitudIngresoDto { Tipo = s.Tipo, Descripcion = s.Descripcion.Trim() }).ToList();
    }

    private async Task<Atencion> RegistrarAsync(int idCliente, int idVehiculo, int idUsuarioRecepcion, string motivoConsulta, TipoSolicitudAtencion tipoSolicitud = TipoSolicitudAtencion.Problema, IReadOnlyList<SolicitudIngresoDto>? solicitudes = null, int? kilometraje = null, string? observaciones = null)
    {
        var pedidos = ValidarSolicitudes(solicitudes);
        if (kilometraje is < 0 || observaciones?.Trim().Length > 1000)
            throw new ValidacionException("Revise kilometraje y observaciones de recepción.");
        if (!Enum.IsDefined(tipoSolicitud))
            throw new ValidacionException("Seleccione el tipo de solicitud.");
        if (pedidos.Count > 0)
            tipoSolicitud = pedidos.All(s => s.Tipo == TipoSolicitudIngreso.TrabajoSolicitado) ? TipoSolicitudAtencion.ServicioDefinido
                : pedidos.All(s => s.Tipo == TipoSolicitudIngreso.ProblemaInformado) ? TipoSolicitudAtencion.Problema : TipoSolicitudAtencion.Mixta;
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
            KilometrajeRecepcion = kilometraje,
            ObservacionesRecepcion = string.IsNullOrWhiteSpace(observaciones) ? null : observaciones.Trim(),
            Solicitudes = pedidos.Select((s, i) => new SolicitudAtencion { Orden = i + 1, Tipo = s.Tipo, Descripcion = s.Descripcion }).ToList(),
            FechaApertura = FechaHoraTaller.Ahora,
            MotivoConsulta = motivoConsulta,
            Estado = EstadoAtencion.EnEvaluacionTecnica,
            FechaCierre = null
        };
        atencion.Historial.Add(new HistorialAtencion { IdUsuario = recepcion.Id, Fecha = atencion.FechaApertura, Accion = "RecepcionRegistrada", EstadoAnterior = null, EstadoNuevo = atencion.Estado, Observaciones = "Solicitudes recibidas y enviadas a preparación técnica. No autoriza trabajos cobrables." });
        await _atencionRepositorio.AgregarAsync(atencion);
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
        return await _acceso.ExigirRolesAsync(RolesSistema.AdministrativoId);
    }
}
