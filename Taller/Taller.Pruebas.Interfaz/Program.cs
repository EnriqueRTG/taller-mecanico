using System.Reflection;
using Taller.Aplicacion.DTOs.Reportes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Infraestructura;
using Taller.Presentacion;
using Taller.Presentacion.Formularios.Principal;
using Taller.Presentacion.Formularios.Sistema;
using Taller.Presentacion.Formularios.Usuarios;
using Taller.Presentacion.Formularios.Reportes;

namespace Taller.Pruebas.Interfaz;
/// <summary>Prueba de construcción WinForms en STA. No abre conexión SQL ni sustituye aceptación visual.</summary>
internal static class Program
{
    [STAThread]
    private static int Main()
    {
        ApplicationConfiguration.Initialize();
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string,string?> {
            ["ConnectionStrings:Default"]="Server=localhost;Database=SGTM_PruebaConstruccion;Integrated Security=True;Encrypt=True",
            ["Respaldos:Habilitado"]="false"
        }).Build();
        var servicios=new ServiceCollection();
        servicios.AddSingleton<IConfiguration>(config);
        servicios.AddLogging();
        servicios.AddAplicacion().AddInfraestructura(config).AddPresentacion();
        using var proveedor=servicios.BuildServiceProvider();
        foreach(int rol in new[]{1,2,3})
        {
            using var scope=proveedor.CreateScope();
            scope.ServiceProvider.GetRequiredService<SesionUsuario>().Iniciar(new Usuario { Id=rol,Nombre="Prueba",Apellido="Interfaz",RolId=rol,Rol=new Rol { Id=rol,Nombre=RolesSistema.ObtenerNombreEsperado(rol)!,Activo=true } });
            foreach(var tipo in new[]{typeof(FrmPrincipal),typeof(FrmInicio),typeof(FrmAuditoriaAccesos),typeof(FrmRespaldoRestauracion),typeof(FrmAltaUsuario),typeof(FrmReportes),typeof(Taller.Presentacion.Formularios.Comprobantes.FrmComprobantes)})
            {
                using var formulario=(Form)scope.ServiceProvider.GetRequiredService(tipo);
                formulario.PerformLayout();
                if (formulario is FrmInicio)
                    VerificarTarjetasInicio(formulario);
                if(formulario.Controls.Count==0)throw new InvalidOperationException("Formulario sin controles: "+tipo.Name);
                Console.WriteLine($"Construcción y layout: {tipo.Name}, rol {rol}");
            }
        }
        VerificarNavegacionDocumento();
        return 0;
    }
    private static void VerificarNavegacionDocumento()
    {
        using var documento = new System.Drawing.Printing.PrintDocument();
        Type tipo = typeof(FrmReportes).Assembly.GetType("Taller.Presentacion.Impresion.VistaPreviaDocumento")!;
        using var formulario = (Form)Activator.CreateInstance(tipo, documento, (Func<int>)(() => 3))!;
        tipo.GetMethod("ActualizarPaginas", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(formulario, null);
        var pagina = (NumericUpDown)formulario.Controls.Find("nudPagina", true).Single();
        var siguiente = (Button)formulario.Controls.Find("btnSiguiente", true).Single();
        var anterior = (Button)formulario.Controls.Find("btnAnterior", true).Single();
        if (pagina.Maximum != 3 || anterior.Enabled || !siguiente.Enabled)
            throw new InvalidOperationException("No se identifica el total de páginas.");
        pagina.Value = 2;
        if (!anterior.Enabled || !siguiente.Enabled)
            throw new InvalidOperationException("No se puede navegar a la segunda página.");
        pagina.Value = 3;
        if (siguiente.Enabled || !anterior.Enabled)
            throw new InvalidOperationException("La navegación supera la última página.");
        Console.WriteLine("Vista previa: navegación entre tres páginas sin impresora ni conexión SQL.");
    }
    private static void VerificarTarjetasInicio(Form formulario)
    {
        // Reproduce la regresión de la tercera tarjeta recortada, sin abrir una conexión SQL.
        var documentos = Enumerable.Range(1, 3).Select(i => new ReporteDocumentoDto
        {
            Titulo = "Indicador " + i,
            Indicadores = [new IndicadorReporteDto("Registros", "10")]
        }).ToList();
        typeof(FrmInicio).GetMethod("MostrarTarjetas", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(formulario, [documentos]);
        var tarjetas = (TableLayoutPanel)formulario.Controls.Find("pnlTarjetas", true).Single();
        foreach (int ancho in new[] { 1020, 1366, 1690 })
        {
            formulario.ClientSize = new Size(ancho, 650);
            Disponer(formulario);
            if (tarjetas.Controls.Count != 3)
                throw new InvalidOperationException("Deben mostrarse las tres tarjetas del inicio.");
            Rectangle anterior = Rectangle.Empty;
            foreach (Control tarjeta in tarjetas.Controls)
            {
                if (tarjeta.Width <= 0 || tarjeta.Height <= 0 || tarjeta.Bottom > tarjetas.ClientSize.Height || tarjeta.Right > tarjetas.ClientSize.Width)
                    throw new InvalidOperationException("Tarjeta recortada para ancho " + ancho);
                if (!anterior.IsEmpty && tarjeta.Top != anterior.Top)
                    throw new InvalidOperationException("Una tarjeta saltó a otra fila.");
                if (!anterior.IsEmpty && tarjeta.Bounds.IntersectsWith(anterior))
                    throw new InvalidOperationException("Tarjetas superpuestas.");
                anterior = tarjeta.Bounds;
            }
            Console.WriteLine("Tres tarjetas visibles sin superposición: " + ancho);
        }
    }

    private static void Disponer(Control control)
    {
        control.PerformLayout();
        foreach (Control hijo in control.Controls)
            Disponer(hijo);
    }

}
