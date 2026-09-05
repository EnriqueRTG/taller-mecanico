
using Taller.Aplicacion.Abstracciones.Persistencia;
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

    /// <summary>
    /// Inicializa el servicio con los repositorios necesarios
    /// para gestionar una atención.
    /// </summary>
    public AtencionServicio(
        IAtencionRepositorio atencionRepositorio,
        IClienteRepositorio clienteRepositorio,
        IVehiculoRepositorio vehiculoRepositorio,
        IUsuarioRepositorio usuarioRepositorio)
    {
        _atencionRepositorio = atencionRepositorio;
        _clienteRepositorio = clienteRepositorio;
        _vehiculoRepositorio = vehiculoRepositorio;
        _usuarioRepositorio = usuarioRepositorio;
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
    public async Task<Atencion?> ObtenerPorIdAsync(int idAtencion)
    {
        ValidarId(idAtencion, nameof(idAtencion));

        return await _atencionRepositorio.ObtenerPorIdAsync(idAtencion);
    }

    /// <summary>
    /// Lista todas las atenciones asociadas a un cliente.
    /// </summary>
    public async Task<List<Atencion>> ListarPorClienteAsync(int idCliente)
    {
        ValidarId(idCliente, nameof(idCliente));

        return await _atencionRepositorio.ListarPorClienteAsync(idCliente);
    }

    /// <summary>
    /// Lista todas las atenciones asociadas a un vehículo.
    /// </summary>
    public async Task<List<Atencion>> ListarPorVehiculoAsync(int idVehiculo)
    {
        ValidarId(idVehiculo, nameof(idVehiculo));

        return await _atencionRepositorio.ListarPorVehiculoAsync(idVehiculo);
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
    public async Task IniciarProcesoAsync(int idAtencion)
    {
        ValidarId(idAtencion, nameof(idAtencion));

        var atencion =
            await ObtenerAtencionRequeridaAsync(idAtencion);

        if (atencion.Estado != EstadoAtencion.Abierta)
        {
            throw new InvalidOperationException(
                "Solo una atención abierta puede pasar a estado EnProceso.");
        }

        atencion.Estado = EstadoAtencion.EnProceso;

        await _atencionRepositorio.ActualizarAsync(atencion);
    }

    /// <summary>
    /// Cancela una atención que aún no se encuentra cerrada.
    /// </summary>
    public async Task CancelarAsync(int idAtencion)
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
    public async Task CerrarAsync(int idAtencion)
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

        if (atencion.Estado != EstadoAtencion.EnProceso)
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
    private async Task ValidarClienteAsync(int idCliente)
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
    private async Task ValidarVehiculoAsync(int idVehiculo)
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
}
