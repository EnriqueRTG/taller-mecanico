using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Diagnosticos;
using Taller.Aplicacion.Excepciones;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;

namespace Taller.Aplicacion.Servicios;

/// <summary>
/// Coordina el registro de informes técnicos.
/// </summary>
public sealed class DiagnosticoServicio
{
    private readonly IDiagnosticoRepositorio _diagnosticoRepositorio;
    private readonly IAtencionRepositorio _atencionRepositorio;
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly SesionUsuario _sesionUsuario;

    public DiagnosticoServicio(
        IDiagnosticoRepositorio diagnosticoRepositorio,
        IAtencionRepositorio atencionRepositorio,
        IUsuarioRepositorio usuarioRepositorio,
        IUnidadDeTrabajo unidadDeTrabajo,
        SesionUsuario sesionUsuario)
    {
        _diagnosticoRepositorio = diagnosticoRepositorio;
        _atencionRepositorio = atencionRepositorio;
        _usuarioRepositorio = usuarioRepositorio;
        _unidadDeTrabajo = unidadDeTrabajo;
        _sesionUsuario = sesionUsuario;
    }

    /// <summary>
    /// Registra el informe inicial y libera la actividad
    /// para permitir la preparación del presupuesto.
    /// </summary>
    /// <returns>Identificador del diagnóstico registrado.</returns>
    public async Task<int> RegistrarDiagnosticoInicialAsync(
        RegistrarDiagnosticoInicialDto datos)
    {
        ArgumentNullException.ThrowIfNull(datos);

        if (datos.IdAtencion <= 0)
        {
            throw new ValidacionException(
                "Debe seleccionar una atención válida.",
                nameof(datos.IdAtencion));
        }

        var tecnico = await ObtenerTecnicoActualAsync();

        string falla = NormalizarTextoObligatorio(
            datos.FallaDetectada,
            "La falla detectada",
            nameof(datos.FallaDetectada));

        string trabajo = NormalizarTextoObligatorio(
            datos.TrabajoRecomendado,
            "El trabajo recomendado",
            nameof(datos.TrabajoRecomendado));

        string? observaciones = NormalizarObservaciones(
            datos.Observaciones);

        if (datos.Kilometraje is < 0)
        {
            throw new ValidacionException(
                "El kilometraje no puede ser negativo.",
                nameof(datos.Kilometraje));
        }

        var atencion = await _atencionRepositorio.ObtenerPorIdAsync(
            datos.IdAtencion);

        if (atencion is null)
        {
            throw new ValidacionException(
                "La atención seleccionada no existe.");
        }

        if (atencion.Estado != EstadoAtencion.EnEvaluacionTecnica
            || atencion.FechaCierre is not null
            || atencion.ActividadTecnica != ActividadTecnica.Diagnostico)
        {
            throw new ValidacionException(
                "La atención no tiene un diagnóstico inicial en actividad.");
        }

        if (atencion.IdTecnicoEnActividad != tecnico.Id)
        {
            throw new ValidacionException(
                "Solo el técnico que tomó la actividad puede registrar el diagnóstico.");
        }

        if (await _diagnosticoRepositorio.ExisteInicialAsync(
                atencion.IdAtencion))
        {
            throw new ValidacionException(
                "La atención ya tiene registrado un diagnóstico inicial.");
        }

        var fecha = DateTime.Now;

        var diagnostico = new Diagnostico
        {
            IdAtencion = atencion.IdAtencion,
            IdTecnicoAutor = tecnico.Id,
            FechaDiagnostico = fecha,
            FallaDetectada = falla,
            TrabajoRecomendado = trabajo,
            Kilometraje = datos.Kilometraje,
            Observaciones = observaciones,
            Tipo = TipoDiagnostico.Inicial,
            ImpideContinuar = false
        };

        var historial = new HistorialAtencion
        {
            IdAtencion = atencion.IdAtencion,
            IdUsuario = tecnico.Id,
            Fecha = fecha,
            Accion = "DiagnosticoInicialRegistrado",
            EstadoAnterior = atencion.Estado,
            EstadoNuevo = atencion.Estado,
            Diagnostico = diagnostico,
            Observaciones =
                "Se registró el informe inicial y se liberó la reserva de diagnóstico."
        };

        // Termina la actividad, pero conserva la autoría del informe.
        atencion.IdTecnicoEnActividad = null;
        atencion.ActividadTecnica = null;

        // Permanece en evaluación técnica.
        // Registrar el informe no autoriza una reparación.
        try
        {
            _diagnosticoRepositorio.Agregar(diagnostico);

            _atencionRepositorio.PrepararCambioReservaConHistorial(
                atencion,
                historial);

            await _unidadDeTrabajo.GuardarCambiosAsync();
        }
        catch
        {
            // También limpia si la preparación falla antes del guardado.
            _unidadDeTrabajo.DescartarCambios();
            throw;
        }

        return diagnostico.IdDiagnostico;
    }

    /// <summary>
    /// Consulta un informe técnico para un usuario operativo autorizado.
    /// </summary>
    public async Task<DiagnosticoDetalleDto> ObtenerDetalleAsync(
        int idDiagnostico)
    {
        if (idDiagnostico <= 0)
        {
            throw new ValidacionException(
                "Debe seleccionar un diagnóstico válido.");
        }

        var usuarioSesion = _sesionUsuario.UsuarioActual;

        if (usuarioSesion is null)
        {
            throw new ValidacionException(
                "Debe iniciar sesión para consultar el diagnóstico.");
        }

        var usuario = await _usuarioRepositorio.ObtenerPorIdAsync(
            usuarioSesion.Id);

        if (usuario is null
            || !usuario.Activo
            || (usuario.RolId != RolesSistema.TecnicoId
                && usuario.RolId != RolesSistema.AdministrativoId))
        {
            throw new ValidacionException(
                "No tiene permisos para consultar el diagnóstico.");
        }

        var detalle = await _diagnosticoRepositorio.ObtenerDetalleAsync(
            idDiagnostico);

        if (detalle is null)
        {
            throw new ValidacionException(
                "El diagnóstico seleccionado no existe.");
        }

        return detalle;
    }

    private async Task<Usuario> ObtenerTecnicoActualAsync()
    {
        var usuarioSesion = _sesionUsuario.UsuarioActual;

        if (usuarioSesion is null)
        {
            throw new ValidacionException(
                "Debe iniciar sesión para registrar un diagnóstico.");
        }

        var usuario = await _usuarioRepositorio.ObtenerPorIdAsync(
            usuarioSesion.Id);

        if (usuario is null
            || !usuario.Activo
            || usuario.RolId != RolesSistema.TecnicoId)
        {
            throw new ValidacionException(
                "La operación requiere un usuario técnico activo.");
        }

        return usuario;
    }

    private static string NormalizarTextoObligatorio(
        string? texto,
        string nombreVisible,
        string nombrePropiedad)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            throw new ValidacionException(
                $"{nombreVisible} es obligatorio.",
                nombrePropiedad);
        }

        texto = texto.Trim();

        if (texto.Length > 1000)
        {
            throw new ValidacionException(
                $"{nombreVisible} no puede superar los 1000 caracteres.",
                nombrePropiedad);
        }

        return texto;
    }

    private static string? NormalizarObservaciones(
        string? texto)
    {
        if (string.IsNullOrWhiteSpace(texto))
        {
            return null;
        }

        texto = texto.Trim();

        if (texto.Length > 1000)
        {
            throw new ValidacionException(
                "Las observaciones no pueden superar los 1000 caracteres.",
                nameof(RegistrarDiagnosticoInicialDto.Observaciones));
        }

        return texto;
    }

    /// <summary>
    /// Obtiene la bandeja de diagnósticos para los perfiles
    /// autorizados a consultar el seguimiento operativo.
    /// </summary>
    public async Task<List<DiagnosticoBandejaDto>> ListarBandejaAsync()
    {
        var usuarioSesion = _sesionUsuario.UsuarioActual;

        if (usuarioSesion is null)
        {
            throw new ValidacionException(
                "Debe iniciar sesión para consultar los diagnósticos.");
        }

        var usuario = await _usuarioRepositorio.ObtenerPorIdAsync(
            usuarioSesion.Id);

        if (usuario is null || !usuario.Activo)
        {
            throw new ValidacionException(
                "El usuario de la sesión no existe o está inactivo.");
        }

        if (usuario.RolId != RolesSistema.TecnicoId
            && usuario.RolId != RolesSistema.AdministrativoId)
        {
            throw new ValidacionException(
                "No tiene permisos para consultar esta bandeja.");
        }

        return await _diagnosticoRepositorio.ListarBandejaAsync();
    }
}