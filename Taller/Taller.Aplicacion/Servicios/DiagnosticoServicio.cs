using Taller.Aplicacion.Seguridad;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Diagnosticos;
using Taller.Aplicacion.Excepciones;
using Taller.Dominio.Constantes;

namespace Taller.Aplicacion.Servicios;
/// <summary>Consulta informes históricos. EvaluacionServicio coordina su registro junto con la propuesta.</summary>
public sealed class DiagnosticoServicio
{
    private readonly IDiagnosticoRepositorio _diagnosticoRepositorio;
    private readonly IUsuarioRepositorio _usuarioRepositorio;
    private readonly SesionUsuario _sesionUsuario;
    public DiagnosticoServicio(IDiagnosticoRepositorio diagnosticoRepositorio, IUsuarioRepositorio usuarioRepositorio, SesionUsuario sesionUsuario)
    {
        _diagnosticoRepositorio = diagnosticoRepositorio;
        _usuarioRepositorio = usuarioRepositorio;
        _sesionUsuario = sesionUsuario;
    }

    /// <summary>Obtiene un informe definitivo sin habilitar su edición.</summary>
    public async Task<DiagnosticoDetalleDto> ObtenerDetalleAsync(int idDiagnostico)
    {
        if (idDiagnostico <= 0)
        {
            throw new ValidacionException("Debe seleccionar un diagnóstico válido.");
        }

        var usuarioSesion = _sesionUsuario.UsuarioActual;
        if (usuarioSesion is null)
        {
            throw new ValidacionException("Debe iniciar sesión para consultar el diagnóstico.");
        }

        var usuario = await _usuarioRepositorio.ObtenerPorIdAsync(usuarioSesion.Id);
        if (!UsuarioAutorizado.EstaHabilitado(usuario) || (usuario.RolId != RolesSistema.TecnicoId && usuario.RolId != RolesSistema.AdministrativoId))
        {
            throw new ValidacionException("No tiene permisos para consultar el diagnóstico.");
        }

        var detalle = await _diagnosticoRepositorio.ObtenerDetalleAsync(idDiagnostico);
        if (detalle is null)
        {
            throw new ValidacionException("El diagnóstico seleccionado no existe.");
        }

        return detalle;
    }
}
