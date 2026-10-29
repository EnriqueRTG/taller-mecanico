using Microsoft.EntityFrameworkCore;
using Taller.Infraestructura.Persistencia;
using Xunit;

namespace Taller.Pruebas;

/// <summary>Contrasta el modelo SQL configurado con el snapshot sin abrir una conexión.</summary>
public sealed class ModeloPersistenciaPruebas
{
    [Fact]
    public void ModeloFinalCoincideConMigracionesYGeneraSqlSinVinculoRetirado()
    {
        using var db = new TallerDbContext(new DbContextOptionsBuilder<TallerDbContext>()
            .UseSqlServer("Server=localhost;Database=SGTM_SoloModelo;Integrated Security=True")
            .Options);
        Assert.False(db.Database.HasPendingModelChanges());
        var sql = db.Database.GenerateCreateScript();
        Assert.DoesNotContain("IdPresupuestoCambio", sql);
        Assert.Contains("FK_Presupuestos_Diagnosticos_IdDiagnostico_IdAtencion", sql);
        Assert.Contains("CK_Clientes_TelefonoContacto", sql);
        Assert.Equal(4, db.Database.GetMigrations().Count());
    }
}
