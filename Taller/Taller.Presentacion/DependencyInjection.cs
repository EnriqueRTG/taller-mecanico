using Microsoft.Extensions.DependencyInjection;
using Taller.Presentacion.Formularios.Atenciones;
using Taller.Presentacion.Formularios.Autenticacion;
using Taller.Presentacion.Formularios.Clientes;
using Taller.Presentacion.Formularios.Comprobantes;
using Taller.Presentacion.Formularios.Diagnosticos;
using Taller.Presentacion.Formularios.Presupuestos;
using Taller.Presentacion.Formularios.Principal;
using Taller.Presentacion.Formularios.Reportes;
using Taller.Presentacion.Formularios.Sistema;
using Taller.Presentacion.Formularios.Usuarios;
using Taller.Presentacion.Formularios.Vehiculos;
using Taller.Presentacion.Seguridad;

namespace Taller.Presentacion;
/// <summary>
/// Registra los formularios y servicios pertenecientes
/// a la capa de Presentación.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Agrega al contenedor de dependencias los componentes
    /// utilizados por la interfaz de usuario.
    /// </summary>
    public static IServiceCollection AddPresentacion(this IServiceCollection services)
    {
        services.AddSingleton(proveedor => Impresion.EncabezadoEmpresa.Cargar(proveedor.GetService<Microsoft.Extensions.Configuration.IConfiguration>()));
        services.AddTransient<FrmLogin>();
        services.AddTransient<FrmPrincipal>();
        services.AddTransient<FrmInicio>();
        services.AddTransient<Taller.Presentacion.Formularios.Catalogo.FrmCatalogo>();
        services.AddTransient<FrmUsuarios>();
        services.AddTransient<FrmAltaUsuario>();
        services.AddTransient<FrmEditarUsuario>();
        services.AddTransient<FrmGestionarCredenciales>();
        services.AddTransient<FrmAtenciones>();
        services.AddTransient<Taller.Presentacion.Formularios.Ejecuciones.FrmTrabajosEnCurso>();
        services.AddTransient<FrmNuevaAtencion>();
        services.AddTransient<FrmClientes>();
        services.AddTransient<FrmAltaCliente>();
        services.AddTransient<FrmEditarCliente>();
        services.AddTransient<FrmVehiculos>();
        services.AddTransient<FrmAltaVehiculo>();
        services.AddTransient<FrmEditarVehiculo>();
        services.AddTransient<FrmHistorialVehiculo>();
        services.AddTransient<FrmDiagnosticos>();
        services.AddTransient<FrmPresupuestos>();
        services.AddTransient<FrmComprobantes>();
        services.AddTransient<FrmReportes>();
        services.AddTransient<FrmRespaldoRestauracion>();
        services.AddTransient<FrmAuditoriaAccesos>();
        services.AddSingleton<AutorizacionNavegacion>();
        services.AddTransient<AplicacionContexto>();
        return services;
    }
}
