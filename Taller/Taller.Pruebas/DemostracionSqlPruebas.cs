using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion;
using Taller.Aplicacion.Abstracciones.Seguridad;
using Taller.Aplicacion.Servicios;
using Taller.Demostracion;
using Taller.Dominio.Enumeraciones;
using Taller.Infraestructura;
using Taller.Infraestructura.Persistencia;
using Taller.Infraestructura.Persistencia.Inicializacion;
using Xunit;

namespace Taller.Pruebas;
public sealed class DemostracionSqlPruebas
{
    [SqlRespaldoFact]
    public async Task LoteCompleto_RespetaServiciosSaldosYEstadosYProtegeAcceso()
    {
        var cadena = new SqlConnectionStringBuilder(Environment.GetEnvironmentVariable("SGTM_SQL_PRUEBAS"));
        if (cadena.InitialCatalog != "master")
            throw new InvalidOperationException("Solo instancia de CI aislada.");
        string master = cadena.ConnectionString;
        string nombre = "SGTM_Pruebas_" + Guid.NewGuid().ToString("N") + "_Demo";
        cadena.InitialCatalog = nombre;
        cadena.Pooling = false;
        const string clave = "PruebaAislada2026!Demo";
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["ConnectionStrings:Default"] = cadena.ConnectionString, ["Respaldos:Habilitado"] = "false", ["Instalacion:ClaveAdministrador"] = clave }).Build();
        // El runner de CI no debe heredar una conexión de negocio real.
        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SGTM_CONN")))
            throw new InvalidOperationException("Retire SGTM_CONN para ejecutar esta prueba aislada.");
        try
        {
            var servicios = new ServiceCollection();
            servicios.AddSingleton<IConfiguration>(config);
            servicios.AddLogging();
            servicios.AddAplicacion().AddInfraestructura(config);
            using var proveedor = servicios.BuildServiceProvider();
            using var scope = proveedor.CreateScope();
            var sp = scope.ServiceProvider;
            var db = sp.GetRequiredService<TallerDbContext>();
            await db.Database.MigrateAsync();
            await sp.GetRequiredService<InicializadorDatos>().InicializarAsync();
            await CargadorDemostracion.CargarAsync(sp, clave);
            Assert.Equal(100, await db.Atenciones.CountAsync());
            Assert.Equal(45, await db.Clientes.CountAsync());
            Assert.Equal(5, await db.Clientes.CountAsync(x => !x.Activo));
            Assert.True(await db.Atenciones.AnyAsync(x => x.Estado == EstadoAtencion.Cerrada));
            Assert.True(await db.Presupuestos.AnyAsync(x => x.Estado == EstadoPresupuesto.PendienteRevisionTecnica));
            Assert.True(await db.Presupuestos.AnyAsync(x => x.EsAdicional));
            Assert.True(await db.Pagos.AnyAsync());
            Assert.False(await db.Comprobantes.AnyAsync(x => x.TotalPagado > x.Total));
            Assert.False(await db.Atenciones.Where(x => x.Estado == EstadoAtencion.Cerrada).AnyAsync(a => db.Comprobantes.Any(c => c.IdAtencion == a.IdAtencion && c.Total != c.TotalPagado)));
            var proteccion = sp.GetRequiredService<IProteccionAcceso>();
            for (int i = 0; i < 5; i++)
                await proteccion.RegistrarAsync("demo.tecnico", null, false);
            Assert.True(await proteccion.EstaBloqueadoAsync("DEMO.TECNICO"));
            Assert.Null(await sp.GetRequiredService<AutenticacionServicio>().AutenticarAsync("demo.tecnico", clave));
            Assert.NotNull(await sp.GetRequiredService<AutenticacionServicio>().AutenticarAsync("demo.recepcion", clave));
        }
        finally
        {
            await using var c = new SqlConnection(master);
            await c.OpenAsync();
            // Nombre construido exclusivamente aquí; no se utiliza una base proporcionada por el usuario.
            using var cmd = new SqlCommand($"IF DB_ID(@nombre) IS NOT NULL BEGIN ALTER DATABASE [{nombre}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{nombre}]; END", c);
            cmd.Parameters.AddWithValue("@nombre", nombre);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
