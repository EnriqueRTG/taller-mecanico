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
    [Fact]
    public void SesionConservaIdentidadHastaCierreExplicitoYPuedeIniciarseOtra()
    {
        var sesion = new SesionUsuario();
        Assert.False(sesion.EstaAutenticado);
        var primero = new Usuario { Id = 1 };
        sesion.Iniciar(primero);
        Assert.Same(primero, sesion.UsuarioActual);
        Assert.True(sesion.EstaAutenticado);
        sesion.Cerrar();
        sesion.Cerrar();
        Assert.Null(sesion.UsuarioActual);
        var segundo = new Usuario { Id = 2 };
        sesion.Iniciar(segundo);
        Assert.Same(segundo, sesion.UsuarioActual);
    }

    [Fact]
    public async Task SesionAbiertaNoPermiteOperarConUsuarioDeshabilitadoORolRevocado()
    {
        using var db = new TallerDbContext(new DbContextOptionsBuilder<TallerDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        await db.Database.EnsureCreatedAsync();
        var usuario = new Usuario { NombreUsuario = "recepcion-prueba", Nombre = "Ana", Apellido = "Prueba", PasswordHash = "hash", RolId = 2 };
        db.Usuarios.Add(usuario);
        await db.SaveChangesAsync();
        var sesion = new SesionUsuario();
        sesion.Iniciar(new Usuario { Id = usuario.Id, RolId = 2 });
        var acceso = new Taller.Aplicacion.Seguridad.AutorizacionOperativa(new Taller.Infraestructura.Persistencia.Repositorios.UsuarioRepositorio(db), sesion);
        Assert.Equal(usuario.Id, (await acceso.ExigirAsync(2)).Id);
        usuario.Activo = false;
        await db.SaveChangesAsync(); db.ChangeTracker.Clear();
        await Assert.ThrowsAsync<Taller.Aplicacion.Excepciones.ValidacionException>(() => acceso.ExigirAsync(2));
        usuario = (await db.Usuarios.FindAsync(usuario.Id))!;
        usuario.Activo = true; usuario.RolId = 3;
        await db.SaveChangesAsync(); db.ChangeTracker.Clear();
        await Assert.ThrowsAsync<Taller.Aplicacion.Excepciones.ValidacionException>(() => acceso.ExigirAsync(2));
        Assert.True(sesion.EstaAutenticado); // La identidad en memoria no sustituye la autorización persistida.
        sesion.Cerrar();
        await Assert.ThrowsAsync<Taller.Aplicacion.Excepciones.ValidacionException>(() => acceso.ExigirAsync());
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

}
