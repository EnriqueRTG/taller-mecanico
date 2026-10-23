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
            foreach(var tipo in new[]{typeof(FrmPrincipal),typeof(FrmInicio),typeof(FrmAuditoriaAccesos),typeof(FrmRespaldoRestauracion)})
            {
                using var formulario=(Form)scope.ServiceProvider.GetRequiredService(tipo);
                formulario.PerformLayout();
                if(formulario.Controls.Count==0)throw new InvalidOperationException("Formulario sin controles: "+tipo.Name);
                Console.WriteLine($"Construcción y layout: {tipo.Name}, rol {rol}");
            }
        }
        return 0;
    }
}
