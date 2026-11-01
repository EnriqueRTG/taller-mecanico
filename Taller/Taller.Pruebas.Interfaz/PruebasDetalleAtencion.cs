using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
using Taller.Infraestructura;
using Taller.Presentacion;
using Taller.Presentacion.Formularios.Atenciones;

namespace Taller.Pruebas.Interfaz;

internal static class PruebasDetalleAtencion
{
    public static void Verificar(IConfiguration config)
    {
        var ingreso = new TaskCompletionSource<Atencion?>();
        var documentos = new TaskCompletionSource<List<PresupuestoConsultaDto>>();
        int consultas = 0;
        var usuario = new Usuario { Id = 2, RolId = RolesSistema.AdministrativoId, Activo = true,
            Rol = new Rol { Id = RolesSistema.AdministrativoId, Nombre = RolesSistema.ObtenerNombreEsperado(RolesSistema.AdministrativoId)!, Activo = true } };
        var servicios = new ServiceCollection();
        servicios.AddSingleton<IConfiguration>(config); servicios.AddLogging();
        servicios.AddAplicacion().AddInfraestructura(config).AddPresentacion();
        servicios.AddScoped<IUsuarioRepositorio>(_ => Crear<IUsuarioRepositorio>((m, _) =>
            m == "ObtenerPorIdAsync" ? Task.FromResult<Usuario?>(usuario) : throw new InvalidOperationException(m)));
        servicios.AddScoped<IAtencionRepositorio>(_ => Crear<IAtencionRepositorio>((m, _) =>
            m == "ObtenerPorIdAsync" ? ingreso.Task : throw new InvalidOperationException(m)));
        servicios.AddScoped<IEvaluacionRepositorio>(_ => Crear<IEvaluacionRepositorio>((m, args) =>
        {
            if (m != "ListarPresupuestosAsync" || (int?)args![0] != 242) throw new InvalidOperationException(m);
            consultas++; return documentos.Task;
        }));
        using var proveedor = servicios.BuildServiceProvider();
        proveedor.GetRequiredService<SesionUsuario>().Iniciar(usuario);
        using var scope = proveedor.CreateScope();
        using var f = new FrmDetalleAtencion(242, scope.ServiceProvider.GetRequiredService<AtencionServicio>(),
            proveedor.GetRequiredService<SesionUsuario>(), scope.ServiceProvider.GetRequiredService<EvaluacionServicio>(), scope.ServiceProvider);
        f.CreateControl();
        var carga = (Task)typeof(FrmDetalleAtencion).GetMethod("RecargarAsync", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(f, null)!;
        if (carga.IsCompleted || !f.Controls.Find("btnCerrar", true).Single().Enabled)
            throw new InvalidOperationException("Una consulta lenta debe permitir cerrar el detalle.");
        ingreso.SetResult(new Atencion { IdAtencion = 242, Estado = EstadoAtencion.EnEvaluacionTecnica,
            TipoSolicitud = TipoSolicitudAtencion.ServicioDefinido, Cliente = new Cliente(), Vehiculo = new Vehiculo() });
        Esperar(carga);
        if (consultas != 0) throw new InvalidOperationException("El ingreso no debe cargar el expediente oculto.");
        var resumen = f.Controls.Find("lblResumen", true).Single().Text;
        if (!resumen.Contains("En preparación técnica") || !resumen.Contains("Servicio definido"))
            throw new InvalidOperationException("El detalle debe mostrar etiquetas legibles.");
        var tabs = (TabControl)typeof(FrmDetalleAtencion).GetField("tabs", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(f)!;
        tabs.SelectedIndex = 1;
        if (consultas != 1) throw new InvalidOperationException("El expediente debe consultar solo la atención seleccionada.");
        // Cambiar de pestaña mientras carga no debe iniciar consultas simultáneas.
        tabs.SelectedIndex = 0; tabs.SelectedIndex = 1;
        if (consultas != 1) throw new InvalidOperationException("Se duplicó una carga pendiente.");
        f.Close();
        documentos.SetResult([]);
        Application.DoEvents();
        if (!f.IsDisposed) throw new InvalidOperationException("Cerrar no debe esperar la consulta del expediente.");
        Console.WriteLine("Detalle: ingreso independiente del expediente, etiquetas legibles y cierre durante consulta pendiente.");
    }
    private static void Esperar(Task tarea)
    {
        var limite = DateTime.UtcNow.AddSeconds(5);
        while (!tarea.IsCompleted && DateTime.UtcNow < limite) Application.DoEvents();
        if (!tarea.IsCompleted) throw new InvalidOperationException("La consulta simulada no terminó.");
        tarea.GetAwaiter().GetResult();
    }
    private static T Crear<T>(Func<string, object?[]?, object?> ejecutar) where T : class
    {
        var proxy = DispatchProxy.Create<T, ConsultaSimulada>();
        ((ConsultaSimulada)(object)proxy).Ejecutar = ejecutar;
        return proxy;
    }
}
public class ConsultaSimulada : DispatchProxy
{
    public Func<string, object?[]?, object?> Ejecutar { get; set; } = null!;
    protected override object? Invoke(MethodInfo? metodo, object?[]? args) => Ejecutar(metodo!.Name, args);
}
