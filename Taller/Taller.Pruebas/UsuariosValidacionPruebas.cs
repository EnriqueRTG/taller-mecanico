using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Seguridad;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;
using Taller.Infraestructura.Persistencia;
using Taller.Infraestructura.Persistencia.Repositorios;
using Taller.Infraestructura.Seguridad;
using Xunit;

namespace Taller.Pruebas;
public sealed class UsuariosValidacionPruebas
{
    [Theory]
    [InlineData(true, false)]
    [InlineData(false, false)]
    [InlineData(true, true)]
    [InlineData(false, true)]
    public async Task DuplicadoEsValidacionCorregible_TambienEnCuentasInactivas(bool activo, bool credenciales)
    {
        using var db = new TallerDbContext(new DbContextOptionsBuilder<TallerDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        db.Database.EnsureCreated();
        db.Usuarios.AddRange(
            new Usuario { Id = 91, NombreUsuario = "administrador.prueba", Nombre = "Admin", Apellido = "Prueba", RolId = 1, Activo = true },
            new Usuario { Id = 92, NombreUsuario = "nombre.ocupado", Nombre = "Cuenta", Apellido = "Existente", RolId = 2, Activo = activo },
            new Usuario { Id = 93, NombreUsuario = "otra.cuenta", Nombre = "Otra", Apellido = "Cuenta", RolId = 2, Activo = true });
        await db.SaveChangesAsync();
        var repo = new UsuarioRepositorio(db);
        var sesion = new SesionUsuario();
        sesion.Iniciar((await repo.ObtenerPorIdAsync(91))!);
        var servicio = new UsuarioServicio(repo, new RolRepositorio(db), new PasswordHasher(), new AutorizacionOperativa(repo, sesion));
        string clave = "A1!" + Guid.NewGuid().ToString("N");
        var error = await Assert.ThrowsAsync<ValidacionException>(() => credenciales
            ? servicio.ActualizarCredencialesAsync(93, "nombre.ocupado", null, null, 91)
            : servicio.CrearAsync("nombre.ocupado", clave, "Nueva", "Cuenta", 2));
        Assert.Equal("nombreUsuario", error.Campo);
        Assert.Contains("ya está registrado", error.Message);
        Assert.Equal(3, await db.Usuarios.CountAsync());
        Assert.Equal("otra.cuenta", (await repo.ObtenerPorIdAsync(93))!.NombreUsuario);
    }
}
