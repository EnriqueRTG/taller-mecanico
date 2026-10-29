using Taller.Dominio.Tiempo;
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
        string clave = "A1!" + Guid.NewGuid().ToString("N");
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
            Assert.Equal(48, await db.Atenciones.CountAsync());
            foreach (var estado in Enum.GetValues<EstadoAtencion>())
                Assert.True(await db.Atenciones.AnyAsync(a => a.Estado == estado), "Falta etapa de atención: " + estado);
            await db.Database.MigrateAsync(); // Repetir preparación no reintroduce conceptos del catálogo mínimo.
            Assert.Equal(CatalogoDemostracion.Entradas.Count + 1, await db.Conceptos.CountAsync());
            Assert.True(await db.Conceptos.AnyAsync(c => !c.Activo && !c.Categoria.Activo));
            Assert.False(await db.Conceptos.AnyAsync(c => c.Activo && (c.PrecioReferencia == null || c.PrecioReferencia <= 0)));
            Assert.True(await db.Conceptos.AnyAsync(c => c.Nombre == "Disco de freno trasero"));
            Assert.True(await db.Conceptos.AnyAsync(c => c.Nombre == "Service completo de filtros y lubricación"));
            Assert.True(await db.Presupuestos.AnyAsync(p => p.EsInspeccion && p.Estado == EstadoPresupuesto.Aceptado && p.Detalles.Any(d => d.CantidadEjecutada > 0)));
            foreach (var estado in Enum.GetValues<EstadoEjecucionDetalle>())
                Assert.True(await db.PresupuestoDetalles.AnyAsync(d => d.EstadoEjecucion == estado), "Falta estado de ejecución: " + estado);
            await ConciliarReportesAsync(sp, db);
            var receptor = await db.Usuarios.Include(u => u.Rol).AsNoTracking().FirstAsync(u => u.RolId == 2 && u.Activo);
            sp.GetRequiredService<SesionUsuario>().Iniciar(receptor);
            Assert.Contains(await sp.GetRequiredService<CobroServicio>().ListarAsync(), c => c.PuedeRegistrarEntrega);

            Assert.Equal(19, await db.Clientes.CountAsync());
            Assert.Equal(3, await db.Clientes.CountAsync(x => !x.Activo));
            Assert.Equal(2, await db.Vehiculos.CountAsync(x => !x.Activo));
            Assert.False(await db.Atenciones.AnyAsync(a=>a.FechaApertura<a.Cliente.FechaAlta||a.FechaApertura<a.Vehiculo.FechaAlta));
            Assert.True(await db.Atenciones.AnyAsync(x => x.Estado == EstadoAtencion.Cerrada));
            Assert.True(await db.Presupuestos.AnyAsync(x => x.Estado == EstadoPresupuesto.PendienteRevisionTecnica));
            Assert.True(await db.Presupuestos.AnyAsync(x => x.EsAdicional));
            Assert.True(await db.Pagos.AnyAsync(p => p.Anulado));
            Assert.True(await db.Presupuestos.AnyAsync(p => p.IdPresupuestoAnterior != null));
            Assert.True(await db.PresupuestoDetalles.AnyAsync(p => p.OrigenMaterial == OrigenMaterial.Cliente && p.MaterialClienteConforme));
            Assert.Equal(5, await db.PagoMedios.Select(p => p.IdMedioPago).Distinct().CountAsync());
            Assert.True(await db.Pagos.AnyAsync());
            Assert.False(await db.Comprobantes.AnyAsync(x => x.TotalPagado > x.Total));
            Assert.False(await db.Atenciones.Where(x => x.Estado == EstadoAtencion.Cerrada).AnyAsync(a => db.Comprobantes.Any(c => c.IdAtencion == a.IdAtencion && c.Total != c.TotalPagado)));
            // La bandeja nueva debe ejecutar consultas SQL reales, incluir pendientes antiguos y respetar permisos.
            foreach (int rol in new[] { 1, 2, 3 })
            {
                var usuario = await db.Usuarios.Include(u => u.Rol).AsNoTracking().FirstAsync(u => u.RolId == rol && u.Activo);
                sp.GetRequiredService<SesionUsuario>().Iniciar(usuario);
                var pendientes = await sp.GetRequiredService<InicioServicio>().ListarPendientesAsync();
                Assert.True(pendientes.Count <= 30);
                if (rol != 1)
                {
                    Assert.NotEmpty(pendientes);
                    Assert.Contains(pendientes, x => x.Fecha < FechaHoraTaller.Hoy.AddDays(-30));
                    await Assert.ThrowsAsync<Taller.Aplicacion.Excepciones.ValidacionException>(() => sp.GetRequiredService<AuditoriaAccesoServicio>().ConsultarAsync(FechaHoraTaller.Hoy.AddDays(-7), FechaHoraTaller.Hoy, null));
                }
            }
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
    private static async Task ConciliarReportesAsync(IServiceProvider sp, TallerDbContext db)
    {
        var admin = await db.Usuarios.AsNoTracking().SingleAsync(u => u.NombreUsuario == "admin");
        sp.GetRequiredService<SesionUsuario>().Iniciar(admin);
        var servicio = sp.GetRequiredService<ReporteServicio>();
        var desde = FechaHoraTaller.Hoy.AddYears(-1);
        var hasta = FechaHoraTaller.Hoy.AddDays(1);
        var acciones = Taller.Aplicacion.Consultas.ActividadTecnicaReporte.Acciones;
        var conteos = new Dictionary<Taller.Aplicacion.DTOs.Reportes.TipoReporte, int>
        {
            [Taller.Aplicacion.DTOs.Reportes.TipoReporte.Atenciones] = await db.Atenciones.CountAsync(a => a.FechaApertura >= desde && a.FechaApertura < hasta),
            [Taller.Aplicacion.DTOs.Reportes.TipoReporte.Presupuestos] = await db.Presupuestos.CountAsync(p => p.Vigente && p.FechaCreacion >= desde && p.FechaCreacion < hasta),
            [Taller.Aplicacion.DTOs.Reportes.TipoReporte.CargaTecnica] = await db.Atenciones.CountAsync(a => a.Estado == EstadoAtencion.EnEvaluacionTecnica || a.Estado == EstadoAtencion.EnEjecucion),
            [Taller.Aplicacion.DTOs.Reportes.TipoReporte.SaldosPendientes] = await db.Comprobantes.CountAsync(c => c.Total > c.TotalPagado),
            [Taller.Aplicacion.DTOs.Reportes.TipoReporte.ActividadTecnica] = await db.HistorialAtenciones.CountAsync(h => h.Fecha >= desde && h.Fecha < hasta && acciones.Contains(h.Accion)),
            [Taller.Aplicacion.DTOs.Reportes.TipoReporte.ConceptosRealizados] = await db.PresupuestoDetalles.CountAsync(d => d.CantidadEjecutada > 0 && d.FechaRegistroEjecucion >= desde && d.FechaRegistroEjecucion < hasta),
            [Taller.Aplicacion.DTOs.Reportes.TipoReporte.MovimientosCobro] = await db.PagoMedios.CountAsync(m => m.Pago.Fecha >= desde && m.Pago.Fecha < hasta) + await db.PagoMedios.CountAsync(m => m.Pago.Anulado && m.Pago.FechaAnulacion >= desde && m.Pago.FechaAnulacion < hasta)
        };
        foreach (var tipo in Enum.GetValues<Taller.Aplicacion.DTOs.Reportes.TipoReporte>())
        {
            var reporte = await servicio.GenerarAsync(new() { Tipo = tipo, Desde = desde, Hasta = hasta.AddDays(-1), Estado = "Todos" });
            Assert.Equal(conteos[tipo], reporte.Registros.Count);
            bool monetario = tipo is Taller.Aplicacion.DTOs.Reportes.TipoReporte.MovimientosCobro or Taller.Aplicacion.DTOs.Reportes.TipoReporte.SaldosPendientes;
            Assert.Equal(monetario ? reporte.Registros.Sum(x => x.Importe ?? 0) : reporte.Registros.Count, reporte.Grafico.Sum(x => x.Valor));
            if (tipo == Taller.Aplicacion.DTOs.Reportes.TipoReporte.SaldosPendientes)
                Assert.Equal(await db.Database.SqlQueryRaw<decimal>("SELECT COALESCE(SUM(Total-TotalPagado),0) AS [Value] FROM dbo.Comprobantes WHERE Total>TotalPagado").SingleAsync(), reporte.Registros.Sum(x => x.Importe ?? 0));
            if (tipo == Taller.Aplicacion.DTOs.Reportes.TipoReporte.Presupuestos)
                Assert.Equal(await db.Database.SqlQueryRaw<decimal>("SELECT COALESCE(SUM(ROUND(d.Cantidad*d.PrecioUnitario,2)),0) AS [Value] FROM dbo.PresupuestoDetalles d JOIN dbo.Presupuestos p ON p.IdPresupuesto=d.IdPresupuesto WHERE p.Vigente=1 AND p.Estado=N'Aceptado' AND d.AceptadoPorCliente=1").SingleAsync(), reporte.Registros.Sum(x => x.ImporteAceptado ?? 0));
        }
    }

}
