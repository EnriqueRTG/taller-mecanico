using Microsoft.EntityFrameworkCore;

namespace Taller.Infraestructura.Persistencia;

public sealed class TallerDbContext : DbContext
{
    public TallerDbContext(DbContextOptions<TallerDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(TallerDbContext).Assembly);
    }
    public static Microsoft.Data.SqlClient.SqlConnection ObtenerConexion()
    {
        var factory = new TallerDbContextFactory();
        var context = factory.CreateDbContext(System.Array.Empty<string>());
        return (Microsoft.Data.SqlClient.SqlConnection)context.Database.GetDbConnection();
    }
}