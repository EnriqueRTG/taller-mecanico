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
    private readonly AutorizacionOperativa _acceso;
    public DiagnosticoServicio(IDiagnosticoRepositorio diagnosticoRepositorio, AutorizacionOperativa acceso)
    {
        _diagnosticoRepositorio = diagnosticoRepositorio;
        _acceso = acceso;
    }

    /// <summary>Obtiene un informe definitivo sin habilitar su edición.</summary>
    public async Task<DiagnosticoDetalleDto> ObtenerDetalleAsync(int idDiagnostico)
    {
        if (idDiagnostico <= 0)
        {
            throw new ValidacionException("Debe seleccionar un diagnóstico válido.");
        }

        await _acceso.ExigirRolesAsync(RolesSistema.TecnicoId, RolesSistema.AdministrativoId);

        var detalle = await _diagnosticoRepositorio.ObtenerDetalleAsync(idDiagnostico);
        if (detalle is null)
        {
            throw new ValidacionException("El diagnóstico seleccionado no existe.");
        }

        return detalle;
    }
}
