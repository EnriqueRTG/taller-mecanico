using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Taller.Infraestructura.Persistencia;

public sealed class TallerDbContextFactory
    : IDesignTimeDbContextFactory<TallerDbContext>
{
    public TallerDbContext CreateDbContext(string[] args)
    {
        var presentationPath = Path.GetFullPath(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "Taller.Presentacion"));

        var configuration = new ConfigurationBuilder()
            .SetBasePath(presentationPath)
            .AddJsonFile(
                "appsettings.json",
                optional: false)
            .AddJsonFile(
                "appsettings.Local.json",
                optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString =
            Environment.GetEnvironmentVariable("SGTM_CONN")
            ?? configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException(
                "No se encontró una cadena de conexión para SGTM.");

        var optionsBuilder =
            new DbContextOptionsBuilder<TallerDbContext>();

        optionsBuilder.UseSqlServer(connectionString);

        return new TallerDbContext(optionsBuilder.Options);
    }
}