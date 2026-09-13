
namespace Taller.Presentacion.Seguridad;

/// <summary>
/// Representa las funciones protegidas disponibles
/// desde la interfaz del sistema.
/// </summary>
public enum PermisoAplicacion
{
    // Administrador
    GestionarUsuarios,
    ConsultarReportesGenerales,
    GestionarRespaldo,

    // Administrativo
    GestionarClientes,
    GestionarVehiculos,
    GestionarAtenciones,
    ConsultarDiagnosticos,
    GestionarDecisionPresupuesto,
    GestionarComprobantesPagos,
    GestionarEntregas,
    ConsultarReportesOperativos,

    // Técnico
    ConsultarAtencionesAsignadas,
    GestionarDiagnosticos,
    GestionarPresupuestosTecnicos,
    RegistrarEjecucionTrabajo,
    ConsultarHistorialVehiculo,
    ConsultarReportesTecnicos
}
