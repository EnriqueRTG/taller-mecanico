using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Taller.Dominio.Entidades;
using Taller.Infraestructura.Persistencia;
using Taller.Infraestructura.Persistencia.Repositorios;
using Xunit;

namespace Taller.Pruebas;

/// <summary>Un fallo desconocido conserva su excepción y origen; no se convierte en una regla ficticia.</summary>
public sealed class ErroresPersistenciaPruebas
{
    private sealed class FalloTecnico : SaveChangesInterceptor
    {
        public readonly DbUpdateException Error = new("Fallo técnico de persistencia para la prueba");
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            LanzarDesdePersistencia();
            return ValueTask.FromResult(result);
        }
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public void LanzarDesdePersistencia() => throw Error;
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public async Task AltaYEdicionDeMaestros_ConservanLaPilaOriginalDelFallo(bool vehiculo, bool editar)
    {
        var fallo = new FalloTecnico();
        using var db = new TallerDbContext(new DbContextOptionsBuilder<TallerDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).AddInterceptors(fallo).Options);
        Func<Task> guardar;
        if (vehiculo)
        {
            var repo = new VehiculoRepositorio(db);
            var dato = new Vehiculo { IdVehiculo = editar ? 1 : 0, Dominio = "AB123CD", Color = "Gris", Version = new byte[8] };
            guardar = () => editar ? repo.ActualizarAsync(dato) : repo.AgregarAsync(dato);
        }
        else
        {
            var repo = new ClienteRepositorio(db);
            var dato = new Cliente { IdCliente = editar ? 1 : 0, Nombre = "Ana", Apellido = "Pérez", Telefono = "3794123456", Version = new byte[8] };
            guardar = () => editar ? repo.ActualizarAsync(dato) : repo.AgregarAsync(dato);
        }
        var error = await Assert.ThrowsAsync<DbUpdateException>(guardar);
        Assert.Same(fallo.Error, error);
        Assert.Contains(nameof(FalloTecnico.LanzarDesdePersistencia), error.StackTrace);
        Assert.Empty(db.ChangeTracker.Entries());
    }
}
