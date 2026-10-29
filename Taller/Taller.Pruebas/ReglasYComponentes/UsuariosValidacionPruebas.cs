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
    /// <summary>Adaptador de prueba: conserva consultas reales y evita ejecutar SP de SQL Server en InMemory.</summary>
    private sealed class UsuariosEnMemoria(TallerDbContext db) : Taller.Aplicacion.Abstracciones.Persistencia.IUsuarioRepositorio
    {
        private readonly UsuarioRepositorio consultas = new(db);
        public Task<Usuario?> ObtenerPorIdAsync(int id) => consultas.ObtenerPorIdAsync(id);
        public Task<Usuario?> ObtenerPorNombreUsuarioAsync(string nombre) => consultas.ObtenerPorNombreUsuarioAsync(nombre);
        public Task<List<Usuario>> ListarAsync() => consultas.ListarAsync();
        public Task<bool> ExisteNombreUsuarioAsync(string nombre) => consultas.ExisteNombreUsuarioAsync(nombre);
        public async Task AgregarAsync(Usuario usuario) { db.Usuarios.Add(usuario); await db.SaveChangesAsync(); }
        public async Task ActualizarCredencialesAsync(int id, string nombre, string? hash)
        {
            var usuario = await db.Usuarios.SingleAsync(u => u.Id == id);
            usuario.NombreUsuario = nombre;
            if (hash is not null) usuario.PasswordHash = hash;
            await db.SaveChangesAsync();
        }
        public Task ActualizarDatosAsync(int id, string nombre, string apellido, int rol) => consultas.ActualizarDatosAsync(id, nombre, apellido, rol);
        public Task CambiarEstadoAsync(int id, bool activo) => consultas.CambiarEstadoAsync(id, activo);
    }

    [Theory]
    [InlineData(7, false)]
    [InlineData(8, true)]
    [InlineData(100, true)]
    [InlineData(101, false)]
    public async Task AltaYCredencialesAplicanLaMismaPolitica(int longitud, bool valido)
    {
        using var db = new TallerDbContext(new DbContextOptionsBuilder<TallerDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        db.Database.EnsureCreated();
        db.Usuarios.AddRange(
            new Usuario { Id = 91, NombreUsuario = "administrador.prueba", Nombre = "Admin", Apellido = "Prueba", RolId = 1 },
            new Usuario { Id = 92, NombreUsuario = "cuenta.prueba", Nombre = "Cuenta", Apellido = "Prueba", RolId = 2 });
        await db.SaveChangesAsync();
        var repo = new UsuariosEnMemoria(db);
        var sesion = new SesionUsuario();
        sesion.Iniciar((await repo.ObtenerPorIdAsync(91))!);
        var hasher = new PasswordHasher();
        var servicio = new UsuarioServicio(repo, new RolRepositorio(db), hasher, new AutorizacionOperativa(repo, sesion));
        string clave = new('a', longitud);
        if (valido)
        {
            var usuario = await servicio.CrearAsync("nuevo.usuario", clave, "Nueva", "Cuenta", 2);
            await servicio.ActualizarCredencialesAsync(92, "cuenta.prueba", clave, clave, 91);
            Assert.True(hasher.Verificar(clave, usuario.PasswordHash));
            Assert.True(hasher.Verificar(clave, (await repo.ObtenerPorIdAsync(92))!.PasswordHash));
        }
        else
        {
            var alta = await Assert.ThrowsAsync<ValidacionException>(() => servicio.CrearAsync("nuevo.usuario", clave, "Nueva", "Cuenta", 2));
            var cambio = await Assert.ThrowsAsync<ValidacionException>(() => servicio.ActualizarCredencialesAsync(92, "cuenta.prueba", clave, clave, 91));
            Assert.Equal(alta.Message, cambio.Message);
            Assert.Equal(alta.Campo, cambio.Campo);
            Assert.Equal(2, await db.Usuarios.CountAsync());
        }
    }

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
