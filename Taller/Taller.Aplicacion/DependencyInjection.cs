using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Servicios;

namespace Taller.Aplicacion;
/// <summary>
/// Clase de extensión para registrar los servicios de la capa de aplicación en el contenedor de dependencias.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra los servicios de la capa de aplicación en el contenedor de dependencias.
    /// </summary>
    /// <param name = "services"></param>
    /// <returns>El contenedor de dependencias con los servicios registrados.</returns>
    public static IServiceCollection AddAplicacion(this IServiceCollection services)
    {
        services.AddSingleton<SesionUsuario>();
        services.AddScoped<UsuarioServicio>();
        services.AddScoped<AutenticacionServicio>();
        services.AddScoped<ClienteServicio>();
        services.AddScoped<VehiculoServicio>();
        services.AddScoped<AtencionServicio>();
        services.AddScoped<ConceptoServicio>();
        services.AddScoped<DiagnosticoServicio>();
        services.AddScoped<EvaluacionServicio>();
        services.AddScoped<Taller.Aplicacion.Seguridad.AutorizacionOperativa>();
        services.AddScoped<PresupuestoServicio>();
        services.AddScoped<EjecucionServicio>();
        services.AddScoped<SeguimientoTrabajoServicio>();
        services.AddScoped<CobroServicio>();
        services.AddScoped<ReporteServicio>();
        services.AddScoped<InicioServicio>();
        services.AddScoped<AuditoriaAccesoServicio>();
        services.AddSingleton<TimeProvider>(TimeProvider.System);
        services.AddSingleton<Taller.Aplicacion.Respaldos.PlanesRestauracion>();
        services.AddScoped<RespaldoServicio>();
        return services;
    }
}
