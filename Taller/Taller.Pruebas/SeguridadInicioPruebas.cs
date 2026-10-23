using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;
using Taller.Infraestructura.Persistencia;
using Taller.Infraestructura.Persistencia.Inicializacion;
using Taller.Infraestructura.Respaldos;
using Taller.Infraestructura.Seguridad;
using Xunit;

namespace Taller.Pruebas;
public sealed class SeguridadInicioPruebas
{
    private sealed class Reloj : TimeProvider
    {
        public DateTimeOffset Ahora = DateTimeOffset.UtcNow;
        public override DateTimeOffset GetUtcNow() => Ahora;
    }

    [Fact]
    public void SesionInactiva_NoSeReviveConActividad()
    {
        var reloj = new Reloj();
        var sesion = new SesionUsuario(reloj);
        sesion.Iniciar(new Usuario { Id = 1 });
        reloj.Ahora = reloj.Ahora.AddMinutes(30);
        sesion.RegistrarActividad();
        Assert.Null(sesion.UsuarioActual);
    }

    [Fact]
    public void SesionActiva_ExpiraAlLimiteAbsoluto()
    {
        var reloj = new Reloj();
        var sesion = new SesionUsuario(reloj);
        sesion.Iniciar(new Usuario { Id = 1 });
        for (int i = 0; i < 47; i++)
        {
            reloj.Ahora = reloj.Ahora.AddMinutes(10);
            sesion.RegistrarActividad();
            Assert.True(sesion.EstaAutenticado);
        }

        reloj.Ahora = reloj.Ahora.AddMinutes(10);
        Assert.False(sesion.EstaAutenticado);
    }

    [Fact]
    public async Task Instalacion_NoCreaContrasenaConocida()
    {
        using var db = new TallerDbContext(new DbContextOptionsBuilder<TallerDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        var config = new ConfigurationBuilder().Build();
        // Esta prueba no depende del entorno de quien la ejecuta.
        if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SGTM_ADMIN_INICIAL_PASSWORD")))
            return;
        await Assert.ThrowsAsync<InvalidOperationException>(() => new InicializadorDatos(db, new PasswordHasher(), config).InicializarAsync());
        Assert.Empty(await db.Usuarios.ToListAsync());
    }

    [Theory]
    [InlineData("http://cuenta.blob.core.windows.net/copias")]
    [InlineData("https://cuenta.blob.core.windows.net/copias?sig=secreto")]
    [InlineData("https://example.com/copias")]
    [InlineData("https://cuenta.blob.core.windows.net/")]
    public void Azure_NoAceptaHttpClavesOEndpointsArbitrarios(string uri)
    {
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["Respaldos:Azure:ContenedorUri"] = uri }).Build();
        Assert.Throws<InvalidOperationException>(() => new ArchivoRespaldoAzure(config).ValidarConfiguracion());
    }

    [Fact]
    public void Azure_AdmiteContenedorPrivadoSinCredencialEnUri()
    {
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { ["Respaldos:Azure:ContenedorUri"] = "https://cuenta.blob.core.windows.net/copias" }).Build();
        new ArchivoRespaldoAzure(config).ValidarConfiguracion();
    }
}
