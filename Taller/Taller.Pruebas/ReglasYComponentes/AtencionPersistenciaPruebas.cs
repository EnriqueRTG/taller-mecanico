using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
using Taller.Infraestructura.Persistencia;
using Taller.Infraestructura.Persistencia.Repositorios;
using Xunit;

namespace Taller.Pruebas;
public sealed class AtencionPersistenciaPruebas
{
    private sealed class FalloGuardado : SaveChangesInterceptor
    {
        public bool Activo;
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (Activo) throw new InvalidOperationException("Fallo técnico de prueba");
            return ValueTask.FromResult(result);
        }
    }

    [Fact]
    public async Task AltaFallidaDescartaTodoElGrafoAntesDeLaSiguienteOperacion()
    {
        var fallo = new FalloGuardado();
        using var db = new TallerDbContext(new DbContextOptionsBuilder<TallerDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).AddInterceptors(fallo).Options);
        db.Database.EnsureCreated();
        Atencion Nueva() => new() { MotivoConsulta = "Servicio solicitado", Solicitudes = [new() { Orden = 1, Tipo = TipoSolicitudIngreso.TrabajoSolicitado, Descripcion = "Revisión solicitada" }],
            Historial = [new() { Accion = "RecepcionRegistrada", EstadoNuevo = EstadoAtencion.EnEvaluacionTecnica }] };
        var repo = new AtencionRepositorio(db);
        fallo.Activo = true;
        await Assert.ThrowsAsync<InvalidOperationException>(() => repo.AgregarAsync(Nueva()));
        Assert.Empty(db.ChangeTracker.Entries());
        fallo.Activo = false;
        await repo.AgregarAsync(Nueva());
        Assert.Single(await db.Atenciones.ToListAsync());
        Assert.Single(await db.Set<SolicitudAtencion>().ToListAsync());
        Assert.Single(await db.HistorialAtenciones.ToListAsync());
    }
}
