using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Respaldos;
using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Respaldos;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Infraestructura.Persistencia;
using Taller.Infraestructura.Persistencia.Repositorios;
using Taller.Infraestructura.Respaldos;
using Taller.Infraestructura.Seguridad;
using Xunit;

namespace Taller.Pruebas;
public sealed class RespaldosPruebas
{
    private sealed class Reloj : TimeProvider
    {
        public DateTimeOffset Ahora = new(2026, 10, 22, 12, 0, 0, TimeSpan.Zero);
        public override DateTimeOffset GetUtcNow() => Ahora;
    }

    private sealed class Motor : IRespaldoMotor
    {
        public bool Habilitado => true;

        public int Restauraciones;
        public PlanRestauracionDto? PlanUsado;
        public ProgramaRespaldoDto? Programa;
        public int Copias;
        public Task<EstadoRespaldosDto> ConsultarAsync(CancellationToken c = default) => Task.FromResult(new EstadoRespaldosDto("Taller", false, 1, "", [], []));
        public Task<ProgramaRespaldoDto> ObtenerProgramaAsync(CancellationToken c = default) => Task.FromResult(new ProgramaRespaldoDto());
        public Task GuardarProgramaAsync(ProgramaRespaldoDto p, CancellationToken c = default)
        {
            Programa = p;
            return Task.CompletedTask;
        }

        public Task<CopiaRespaldoDto> CrearAsync(TipoRespaldo t, string carpeta, string autor, IProgress<string>? p = null, CancellationToken c = default)
        {
            Copias++;
            return Task.FromResult(new CopiaRespaldoDto { Tipo = t, Autor = autor });
        }

        public Task<PlanRestauracionDto> ProbarRestauracionAsync(Guid copia, int usuario, IProgress<string>? p = null, CancellationToken c = default) => Task.FromResult(new PlanRestauracionDto { IdSolicitante = usuario, Completa = new CopiaRespaldoDto { Id = copia, RutaServidor = "/sql/copia.bak" } });
        public Task<ResultadoRestauracionDto> RestaurarAsync(PlanRestauracionDto plan, AprobadorRestauracionDto a, AprobadorRestauracionDto b, string motivo, IProgress<string>? p = null)
        {
            Restauraciones++;
            PlanUsado = plan;
            return Task.FromResult(new ResultadoRestauracionDto(true, false, "/sql/protectora.bak", "Correcto"));
        }

        public Task EjecutarProgramacionAsync(CancellationToken c = default) => Task.CompletedTask;
    }

    private sealed class Caso : IDisposable
    {
        public TallerDbContext Db { get; } = new(new DbContextOptionsBuilder<TallerDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        public SesionUsuario Sesion { get; } = new();
        public Motor Motor { get; } = new();
        public Reloj Reloj { get; } = new();
        public RespaldoServicio Servicio { get; }

        public const string Clave = "ClaveSegura123!";
        public Caso()
        {
            Db.Database.EnsureCreated();
            var hasher = new PasswordHasher();
            Db.AddRange(new Usuario { Id = 1, NombreUsuario = "admin1", Nombre = "Admin", Apellido = "Uno", RolId = 1, PasswordHash = hasher.Hash(Clave) }, new Usuario { Id = 2, NombreUsuario = "admin2", Nombre = "Admin", Apellido = "Dos", RolId = 1, PasswordHash = hasher.Hash(Clave) }, new Usuario { Id = 3, NombreUsuario = "recepcion", Nombre = "Recep", Apellido = "Tres", RolId = 2, PasswordHash = hasher.Hash(Clave) }, new Usuario { Id = 4, NombreUsuario = "tecnico", Nombre = "Tec", Apellido = "Cuatro", RolId = 3, PasswordHash = hasher.Hash(Clave) });
            Db.SaveChanges();
            Db.ChangeTracker.Clear();
            Sesion.Iniciar(new Usuario { Id = 1 });
            var usuarios = new UsuarioRepositorio(Db);
            Servicio = new RespaldoServicio(Motor, usuarios, new AutenticacionServicio(usuarios, hasher), Sesion, new PlanesRestauracion(Reloj), Reloj);
        }

        public void Dispose() => Db.Dispose();
        public Task<ResultadoRestauracionDto> Restaurar(Guid token, string segundo = "admin2", string clave = Clave, bool reconoce = true) => Servicio.RestaurarAsync(token, "admin1", Clave, segundo, clave, "Recuperar información validada", reconoce);
    }

    [Theory]
    [InlineData("/var/opt/mssql/backup")]
    [InlineData(@"D:\Respaldos")]
    [InlineData(@"\\servidor\copias")]
    public void AceptaRutasAbsolutasDelServidorSinDependerDelCliente(string ruta) => Assert.Equal(ruta, PoliticaRespaldos.Carpeta(ruta));
    [Theory]
    [InlineData("relativa")]
    [InlineData("/sql/../otro")]
    [InlineData("/sql/*")]
    [InlineData("/sql/\nmal")]
    public void RechazaRutasAmbiguasYControles(string ruta) => Assert.Throws<ValidacionException>(() => PoliticaRespaldos.Carpeta(ruta));
    [Fact]
    public void NombreSqlSeEscapaYSistemaNoSePuedeRecuperar()
    {
        Assert.Equal("[Taller]];--]", SqlRespaldos.Identificador("Taller];--"));
        Assert.Equal("N'a''b'", SqlRespaldos.Literal("a'b"));
        Assert.Throws<InvalidOperationException>(() => SqlRespaldos.Identificador("master"));
    }

    [Fact]
    public void PlanDiarioRespetaZonaHorariaYNoRepiteHoraYaEjecutada()
    {
        var p = new ProgramaRespaldoDto
        {
            CarpetaServidor = "/sql/backup",
            MinutoDelDia = 120,
            ZonaHoraria = "Argentina Standard Time"
        };
        var ahora = new DateTimeOffset(2026, 10, 22, 5, 0, 0, TimeSpan.Zero);
        Assert.Equal(ahora.AddDays(1), PoliticaRespaldos.Proxima(p, ahora));
    }

    [Fact]
    public void ProgramacionDiferencialSinBaseSeConvierteEnCompleta()
    {
        var p = new ProgramaRespaldoDto
        {
            CarpetaServidor = "/sql/backup",
            ZonaHoraria = "UTC",
            DiaCompleta = DayOfWeek.Sunday
        };
        var jueves = new DateTimeOffset(2026, 10, 22, 5, 0, 0, TimeSpan.Zero);
        Assert.Equal(TipoRespaldo.Completa, PoliticaRespaldos.TipoProgramado(p, jueves, false));
        Assert.Equal(TipoRespaldo.Diferencial, PoliticaRespaldos.TipoProgramado(p, jueves, true));
    }

    [Fact]
    public async Task SoloAdministradorActivoPuedeCrearCopias()
    {
        using var c = new Caso();
        c.Sesion.Iniciar(new Usuario { Id = 3, RolId = 1 });
        await Assert.ThrowsAsync<ValidacionException>(() => c.Servicio.CrearAsync(TipoRespaldo.Completa, "/sql/backup"));
        Assert.Equal(0, c.Motor.Copias);
    }

    [Theory]
    [InlineData("admin2")]
    [InlineData("recepcion")]
    public async Task RestauraConDosIdentidadesValidasDistintasYPlanDeUnSoloUso(string segundo)
    {
        using var c = new Caso();
        var p = await c.Servicio.ProbarRestauracionAsync(Guid.NewGuid());
        await c.Restaurar(p.Token, segundo);
        Assert.Equal(1, c.Motor.Restauraciones);
        Assert.Equal(p.Completa.RutaServidor, c.Motor.PlanUsado!.Completa.RutaServidor);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Restaurar(p.Token, segundo));
    }

    [Theory]
    [InlineData("admin1")]
    [InlineData("tecnico")]
    public async Task NoRestauraConElMismoUsuarioNiUnTecnico(string segundo)
    {
        using var c = new Caso();
        var p = await c.Servicio.ProbarRestauracionAsync(Guid.NewGuid());
        await Assert.ThrowsAsync<ValidacionException>(() => c.Restaurar(p.Token, segundo));
        Assert.Equal(0, c.Motor.Restauraciones);
    }

    [Fact]
    public async Task ClaveIncorrectaOCuentaRevocadaNoAutorizaReemplazo()
    {
        using var c = new Caso();
        var p = await c.Servicio.ProbarRestauracionAsync(Guid.NewGuid());
        await Assert.ThrowsAsync<ValidacionException>(() => c.Restaurar(p.Token, "admin2", "Incorrecta123!"));
        var u = await c.Db.Usuarios.FindAsync(2);
        u!.Activo = false;
        await c.Db.SaveChangesAsync();
        c.Db.ChangeTracker.Clear();
        await Assert.ThrowsAsync<ValidacionException>(() => c.Restaurar(p.Token));
        Assert.Equal(0, c.Motor.Restauraciones);
    }

    [Fact]
    public async Task PlanVencidoODeOtroAdministradorNoSePuedeUsar()
    {
        using var c = new Caso();
        var p = await c.Servicio.ProbarRestauracionAsync(Guid.NewGuid());
        c.Sesion.Iniciar(new Usuario { Id = 2 });
        await Assert.ThrowsAsync<ValidacionException>(() => c.Servicio.RestaurarAsync(p.Token, "admin2", Caso.Clave, "admin1", Caso.Clave, "Motivo suficiente", true));
        c.Sesion.Iniciar(new Usuario { Id = 1 });
        c.Reloj.Ahora = c.Reloj.Ahora.AddMinutes(11);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Restaurar(p.Token));
        Assert.Equal(0, c.Motor.Restauraciones);
    }

    [Fact]
    public async Task RequiereReconocimientoYMotivoAntesDeConsumirPlan()
    {
        using var c = new Caso();
        var p = await c.Servicio.ProbarRestauracionAsync(Guid.NewGuid());
        await Assert.ThrowsAsync<ValidacionException>(() => c.Restaurar(p.Token, reconoce: false));
        await Assert.ThrowsAsync<ValidacionException>(() => c.Servicio.RestaurarAsync(p.Token, "admin1", Caso.Clave, "admin2", Caso.Clave, "", true));
        await c.Restaurar(p.Token);
    }

    [Fact]
    public async Task GuardaPlanConAdministradorRealYProximaEjecucionNoConDatosDelFormulario()
    {
        using var c = new Caso();
        await c.Servicio.GuardarProgramaAsync(new ProgramaRespaldoDto { Activo = true, CarpetaServidor = "/sql/backup", IdAdministrador = 999, Autor = "falso", ProximaUtc = DateTimeOffset.MinValue });
        Assert.Equal(1, c.Motor.Programa!.IdAdministrador);
        Assert.True(c.Motor.Programa.ProximaUtc > c.Reloj.Ahora);
    }

    [Fact]
    public void SesionAnteriorQuedaInvalidadaAlCambiarGeneracionDeRecuperacion()
    {
        var g = new GeneracionMantenimiento();
        g.Validar(1);
        g.Validar(1);
        Assert.Throws<InvalidOperationException>(() => g.Validar(2));
        new GeneracionMantenimiento().Validar(2);
    }
}
