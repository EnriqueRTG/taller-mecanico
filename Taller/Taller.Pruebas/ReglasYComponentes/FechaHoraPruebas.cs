using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Seguridad;
using Taller.Aplicacion.Seguridad;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;
using Taller.Dominio.Tiempo;
using Taller.Infraestructura.Persistencia;
using Taller.Infraestructura.Persistencia.Repositorios;
using Xunit;

namespace Taller.Pruebas;
public sealed class FechaHoraPruebas
{
    [Fact]
    public void EventoCercanoAMedianocheSeMuestraEnElDiaDelTaller()
    {
        var instante = new DateTimeOffset(2026, 10, 24, 1, 30, 0, TimeSpan.Zero);
        var local = FechaHoraTaller.DesdeUtc(instante);
        Assert.Equal(new DateTime(2026, 10, 23, 22, 30, 0), local.DateTime);
        Assert.Equal(TimeSpan.FromHours(-3), local.Offset);
        Assert.Equal(instante, FechaHoraTaller.AUTC(local.DateTime));
    }

    [Fact]
    public async Task FiltroDeAuditoriaIncluyeElDiaCivilCompleto_SinDependerDeLaPc()
    {
        using var db = new TallerDbContext(new DbContextOptionsBuilder<TallerDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        db.Database.EnsureCreated();
        db.Usuarios.Add(new Usuario { Id = 91, NombreUsuario = "admin.tiempo", Nombre = "Admin", Apellido = "Prueba", RolId = 1, Activo = true });
        await db.SaveChangesAsync();
        var repoUsuarios = new UsuarioRepositorio(db);
        var sesion = new SesionUsuario();
        sesion.Iniciar((await repoUsuarios.ObtenerPorIdAsync(91))!);
        var repo = new AuditoriaCaptura();
        var servicio = new AuditoriaAccesoServicio(repo, new AutorizacionOperativa(repoUsuarios, sesion));
        await servicio.ConsultarAsync(new DateTime(2026, 1, 5), new DateTime(2026, 1, 5), null);
        Assert.Equal(new DateTimeOffset(2026, 1, 5, 3, 0, 0, TimeSpan.Zero), repo.Desde);
        Assert.Equal(new DateTimeOffset(2026, 1, 6, 3, 0, 0, TimeSpan.Zero), repo.Hasta);
    }

    private sealed class AuditoriaCaptura : IAuditoriaAccesoRepositorio
    {
        public DateTimeOffset Desde, Hasta;
        public Task<List<AccesoAuditoriaDto>> ConsultarAsync(DateTimeOffset desde, DateTimeOffset hasta, bool? correcto)
        {
            Desde = desde; Hasta = hasta;
            return Task.FromResult(new List<AccesoAuditoriaDto>());
        }
    }
}
