
using Taller.Dominio.Constantes;

namespace Taller.Presentacion.Seguridad;

/// <summary>
/// Determina las funciones de navegación permitidas
/// para cada rol del sistema.
/// </summary>
public sealed class AutorizacionNavegacion
{
    /// <summary>
    /// Indica si el rol especificado posee un permiso.
    /// </summary>
    public bool TienePermiso(int rolId, PermisoAplicacion permiso)
    {
        return rolId switch
        {
            RolesSistema.AdministradorId => PermisoAdministrador.Contains(permiso),

            RolesSistema.AdministrativoId => PermisoAdministrativo.Contains(permiso),

            RolesSistema.TecnicoId => PermisoTecnico.Contains(permiso),

            _ => false
        };
    }

    private static readonly HashSet<PermisoAplicacion> PermisoAdministrador = 
        [
            PermisoAplicacion.GestionarUsuarios,
            PermisoAplicacion.ConsultarReportesGenerales,
            PermisoAplicacion.GestionarRespaldo
        ];

    private static readonly HashSet<PermisoAplicacion> PermisoAdministrativo =
        [
            PermisoAplicacion.GestionarClientes,
            PermisoAplicacion.GestionarVehiculos,
            PermisoAplicacion.GestionarAtenciones,
            PermisoAplicacion.ConsultarDiagnosticos,
            PermisoAplicacion.GestionarDecisionPresupuesto,
            PermisoAplicacion.GestionarComprobantesPagos,
            PermisoAplicacion.GestionarEntregas,
            PermisoAplicacion.ConsultarReportesOperativos
        ];

    private static readonly HashSet<PermisoAplicacion> PermisoTecnico =
        [
            PermisoAplicacion.ConsultarAtencionesAsignadas,
            PermisoAplicacion.GestionarDiagnosticos,
            PermisoAplicacion.GestionarPresupuestosTecnicos,
            PermisoAplicacion.RegistrarEjecucionTrabajo,
            PermisoAplicacion.ConsultarHistorialVehiculo,
            PermisoAplicacion.ConsultarReportesTecnicos
        ];
}
