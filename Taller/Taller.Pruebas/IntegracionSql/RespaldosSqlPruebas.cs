using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.DTOs.Respaldos;
using Taller.Aplicacion.Respaldos;
using Taller.Infraestructura.Persistencia;
using Taller.Infraestructura.Respaldos;
using Xunit;

namespace Taller.Pruebas;
/// <summary>Exclusivamente instancia aislada. Crea y elimina bases con GUID; nunca apunta a la base del usuario.</summary>
public sealed class SqlRespaldoFactAttribute : FactAttribute
{
    public SqlRespaldoFactAttribute()
    {
        if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("SGTM_SQL_PRUEBAS")) || Environment.GetEnvironmentVariable("SGTM_PERMITIR_PRUEBAS_SQL") != "1")
            Skip = "Requiere instancia SQL Server aislada y activación explícita; GitHub Actions la proporciona.";
    }
}

public sealed class RespaldosSqlPruebas
{
    private sealed class NubePrueba : Taller.Aplicacion.Abstracciones.Persistencia.IArchivoRespaldoNube
    {
        public bool Fallar { get; set; }

        public void ValidarConfiguracion()
        {
        }

        public Task<CopiaRespaldoDto> SubirAsync(CopiaRespaldoDto copia, CancellationToken ct = default) => Fallar ? throw new IOException("Fallo controlado de transferencia") : Task.FromResult(copia with { EstadoAzure = "Verificada", VerificadaAzureUtc = DateTimeOffset.UtcNow, BlobAzure = "objeto-de-prueba" });
        public Task RecuperarArchivoAsync(CopiaRespaldoDto copia, CancellationToken ct = default) => Task.CompletedTask;
    }

    [SqlRespaldoFact]
    public async Task Azure_FalloDeReplica_ConservaCopiaVerificadaYPermiteReintentar()
    {
        await using var c = await Caso.CrearAsync();
        var nube = new NubePrueba
        {
            Fallar = true
        };
        var motor = new RespaldoSqlServer(c.Opciones, TimeProvider.System, nube);
        var copia = await motor.CrearConDestinoAsync(TipoRespaldo.Completa, c.Carpeta, "Administrador CI", true);
        Assert.Equal("Pendiente de reintento", copia.EstadoAzure);
        Assert.Single((await motor.ConsultarAsync()).Copias, x => x.Id == copia.Id);
        // El ensayo lee y verifica el respaldo SQL real, independientemente de la réplica fallida.
        await motor.ProbarRestauracionAsync(copia.Id, 1);
        nube.Fallar = false;
        await motor.ReintentarAzureAsync(copia.Id, "Administrador CI");
        Assert.Equal("Verificada", (await motor.ConsultarAsync()).Copias.Single(x => x.Id == copia.Id).EstadoAzure);
    }

    private sealed class Caso : IAsyncDisposable
    {
        public string Nombre { get; } = "SGTM_Pruebas_" + Guid.NewGuid().ToString("N");
        public string Conexion { get; private set; } = "";
        public string Master { get; private set; } = "";
        public string Carpeta => Environment.GetEnvironmentVariable("SGTM_SQL_CARPETA_PRUEBAS") ?? "/var/opt/mssql/backup";
        public OpcionesRespaldos Opciones { get; private set; } = null!;
        public RespaldoSqlServer Motor { get; private set; } = null!;
        public AprobadorRestauracionDto Primero => new(1, 1, "Admin uno", PoliticaRespaldos.Huella("hash-admin"));
        public AprobadorRestauracionDto Segundo => new(2, 2, "Recepción dos", PoliticaRespaldos.Huella("hash-recepcion"));

        public static async Task<Caso> CrearAsync()
        {
            var c = new Caso();
            var b = new SqlConnectionStringBuilder(Environment.GetEnvironmentVariable("SGTM_SQL_PRUEBAS"));
            if (b.InitialCatalog != "master" || Environment.GetEnvironmentVariable("SGTM_PERMITIR_PRUEBAS_SQL") != "1")
                throw new InvalidOperationException("No se autorizó una instancia aislada para pruebas.");
            b.Pooling = false;
            b.Enlist = false;
            c.Master = b.ConnectionString;
            b.InitialCatalog = c.Nombre;
            c.Conexion = b.ConnectionString;
            await c.SqlAsync("CREATE DATABASE " + SqlRespaldos.Identificador(c.Nombre));
            await c.SqlAsync($"""
                USE {SqlRespaldos.Identificador(c.Nombre)};
                CREATE TABLE dbo.__EFMigrationsHistory(MigrationId nvarchar(150) PRIMARY KEY,ProductVersion nvarchar(32));
                INSERT dbo.__EFMigrationsHistory VALUES(N'PruebaAislada_1',N'10.0.11');
                CREATE TABLE dbo.Roles(Id int PRIMARY KEY,Nombre nvarchar(50),Activo bit);INSERT dbo.Roles VALUES(1,N'Administrador',1),(2,N'Administrativo',1);
                CREATE TABLE dbo.Usuarios(Id int PRIMARY KEY,RolId int,Activo bit,PasswordHash nvarchar(500));INSERT dbo.Usuarios VALUES(1,1,1,N'hash-admin'),(2,2,1,N'hash-recepcion');
                CREATE TABLE dbo.DatosPrueba(Id int PRIMARY KEY,Valor nvarchar(100));INSERT dbo.DatosPrueba VALUES(1,N'Original');
                """);
            c.Opciones = new OpcionesRespaldos(c.Conexion, c.Conexion, c.Nombre + "_Control", true)
            {
                EsperaMantenimientoMs = 200
            };
            await new RegistroRespaldosSql(c.Opciones).InstalarAsync();
            c.Motor = new RespaldoSqlServer(c.Opciones, TimeProvider.System);
            return c;
        }

        public async Task SqlAsync(string sql, params SqlParameter[] parametros)
        {
            await using var c = new SqlConnection(Master);
            await c.OpenAsync();
            using var cmd = new SqlCommand(sql, c)
            {
                CommandTimeout = 120
            };
            cmd.Parameters.AddRange(parametros);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> ConteoAsync()
        {
            await using var c = new SqlConnection(Conexion);
            await c.OpenAsync();
            return Convert.ToInt32(await new SqlCommand("SELECT COUNT(*) FROM dbo.DatosPrueba", c).ExecuteScalarAsync());
        }

        public Task AgregarAsync(int n) => SqlAsync($"INSERT {SqlRespaldos.Identificador(Nombre)}.dbo.DatosPrueba VALUES({n},N'Posterior')");
        public async Task<PlanRestauracionDto> PlanAsync(Guid id) => (await Motor.ProbarRestauracionAsync(id, 1))with
        {
            Token = Guid.NewGuid(),
            VenceUtc = DateTimeOffset.UtcNow.AddMinutes(10)
        };
        public TallerDbContext Contexto(GeneracionMantenimiento generacion) => new(new DbContextOptionsBuilder<TallerDbContext>().UseSqlServer(Conexion).AddInterceptors(new IntercepcionMantenimiento(Opciones, generacion)).Options);
        public async ValueTask DisposeAsync()
        {
            foreach (var db in new[]
            {
                Nombre,
                Nombre + "_Control"
            }

            )
            {
                if (!db.StartsWith("SGTM_Pruebas_", StringComparison.Ordinal))
                    throw new InvalidOperationException("No se elimina una base ajena a la prueba.");
                await SqlAsync($"IF DB_ID(@db) IS NOT NULL BEGIN IF DATABASEPROPERTYEX(@db,'Status')=N'ONLINE' ALTER DATABASE {SqlRespaldos.Identificador(db)} SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE {SqlRespaldos.Identificador(db)}; END", new SqlParameter("@db", db));
            }
        }
    }

    [SqlRespaldoFact]
    public async Task CompletaMasDiferencial_RestauraPuntoExacto_PreservaAuditoriaYProtectora()
    {
        await using var c = await Caso.CrearAsync();
        var completa = await c.Motor.CrearAsync(TipoRespaldo.Completa, c.Carpeta, "Admin");
        await c.AgregarAsync(2);
        var diferencial = await c.Motor.CrearAsync(TipoRespaldo.Diferencial, c.Carpeta, "Admin");
        Assert.Equal(completa.Id, diferencial.BaseId);
        var plan = await c.PlanAsync(diferencial.Id);
        await c.AgregarAsync(3);
        var r = await c.Motor.RestaurarAsync(plan, c.Primero, c.Segundo, "Recuperación aprobada en instancia de prueba");
        Assert.True(r.Restaurada);
        Assert.False(r.RecuperadaAnterior);
        Assert.Equal(2, await c.ConteoAsync());
        var estado = await c.Motor.ConsultarAsync();
        Assert.False(estado.EnMantenimiento);
        Assert.Equal(2, estado.Generacion);
        Assert.Contains(estado.Copias, x => x.SoloCopia && x.RutaServidor == r.RutaProtectora);
        Assert.Contains(estado.Historial, x => x.Accion == "Restauracion" && x.SegundoAprobador == "Recepción dos");
    }

    [SqlRespaldoFact]
    public async Task DiferencialSinBaseNoSeCreaNiRegistraComoCorrecta()
    {
        await using var c = await Caso.CrearAsync();
        await Assert.ThrowsAsync<InvalidOperationException>(() => c.Motor.CrearAsync(TipoRespaldo.Diferencial, c.Carpeta, "Admin"));
        Assert.Empty((await c.Motor.ConsultarAsync()).Copias);
    }

    [SqlRespaldoFact]
    public async Task CopiaDeEsquemaAnteriorSeRechazaAntesDeCambiarDatos()
    {
        await using var c = await Caso.CrearAsync();
        var copia = await c.Motor.CrearAsync(TipoRespaldo.Completa, c.Carpeta, "Admin");
        await c.SqlAsync($"INSERT {SqlRespaldos.Identificador(c.Nombre)}.dbo.__EFMigrationsHistory VALUES(N'PruebaAislada_2',N'10.0.11')");
        await c.AgregarAsync(2);
        await Assert.ThrowsAsync<InvalidOperationException>(() => c.PlanAsync(copia.Id));
        Assert.Equal(2, await c.ConteoAsync());
    }

    [SqlRespaldoFact]
    public async Task ArchivoReemplazadoSeDetectaPorIdentidadDelConjunto()
    {
        await using var c = await Caso.CrearAsync();
        var copia = await c.Motor.CrearAsync(TipoRespaldo.Completa, c.Carpeta, "Admin");
        await c.SqlAsync($"BACKUP DATABASE {SqlRespaldos.Identificador(c.Nombre)} TO DISK=@ruta WITH INIT,CHECKSUM", new SqlParameter("@ruta", copia.RutaServidor));
        await Assert.ThrowsAsync<InvalidOperationException>(() => c.PlanAsync(copia.Id));
        Assert.False((await c.Motor.ConsultarAsync()).EnMantenimiento);
    }

    [SqlRespaldoFact]
    public async Task NoDesconectaSesionExternaNiPierdeTransacciones()
    {
        await using var c = await Caso.CrearAsync();
        var copia = await c.Motor.CrearAsync(TipoRespaldo.Completa, c.Carpeta, "Admin");
        var plan = await c.PlanAsync(copia.Id);
        await c.AgregarAsync(2);
        await using var externa = new SqlConnection(c.Conexion);
        await externa.OpenAsync();
        await Assert.ThrowsAsync<InvalidOperationException>(() => c.Motor.RestaurarAsync(plan, c.Primero, c.Segundo, "La conexión externa debe impedir el reemplazo"));
        Assert.Equal(System.Data.ConnectionState.Open, externa.State);
        Assert.False((await c.Motor.ConsultarAsync()).EnMantenimiento);
        Assert.Equal(2, await c.ConteoAsync());
    }

    [SqlRespaldoFact]
    public async Task OperacionDeNegocioAbiertaBloqueaReemplazoYSuDisposeLiberaElPermiso()
    {
        await using var c = await Caso.CrearAsync();
        var copia = await c.Motor.CrearAsync(TipoRespaldo.Completa, c.Carpeta, "Admin");
        var plan = await c.PlanAsync(copia.Id);
        var db = c.Contexto(new GeneracionMantenimiento());
        await db.Database.OpenConnectionAsync();
        await Assert.ThrowsAsync<InvalidOperationException>(() => c.Motor.RestaurarAsync(plan, c.Primero, c.Segundo, "La operación debe terminar antes del reemplazo"));
        await db.DisposeAsync();
        var r = await c.Motor.RestaurarAsync(plan, c.Primero, c.Segundo, "Ahora la operación ya liberó la conexión");
        Assert.True(r.Restaurada);
    }

    [SqlRespaldoFact]
    public async Task MantenimientoBloqueaAccesosYGeneracionNuevaInvalidaSesionesAnteriores()
    {
        await using var c = await Caso.CrearAsync();
        var g = new GeneracionMantenimiento();
        await using var anterior = c.Contexto(g);
        await anterior.Database.ExecuteSqlRawAsync("SELECT 1");
        await c.SqlAsync($"UPDATE {SqlRespaldos.Identificador(c.Opciones.BaseControl)}.dbo.EstadoSGTM SET Mantenimiento=1");
        await Assert.ThrowsAsync<InvalidOperationException>(() => anterior.Database.ExecuteSqlRawAsync("SELECT 1"));
        await c.SqlAsync($"UPDATE {SqlRespaldos.Identificador(c.Opciones.BaseControl)}.dbo.EstadoSGTM SET Mantenimiento=0,Generacion=Generacion+1");
        await Assert.ThrowsAsync<InvalidOperationException>(() => anterior.Database.ExecuteSqlRawAsync("SELECT 1"));
        await using var nueva = c.Contexto(new GeneracionMantenimiento());
        await nueva.Database.ExecuteSqlRawAsync("SELECT 1");
    }

    private sealed class FallarTrasCompleta : IProgress<string>
    {
        public void Report(string valor)
        {
            if (valor.StartsWith("Copia completa aplicada", StringComparison.Ordinal))
                throw new InvalidOperationException("Fallo controlado de prueba después de aplicar la completa");
        }
    }

    [SqlRespaldoFact]
    public async Task FalloDespuesDeAplicarBaseRecuperaProtectoraSinPerderEstadoAnterior()
    {
        await using var c = await Caso.CrearAsync();
        var completa = await c.Motor.CrearAsync(TipoRespaldo.Completa, c.Carpeta, "Admin");
        await c.AgregarAsync(2);
        var diff = await c.Motor.CrearAsync(TipoRespaldo.Diferencial, c.Carpeta, "Admin");
        var plan = await c.PlanAsync(diff.Id);
        await c.AgregarAsync(3);
        var r = await c.Motor.RestaurarAsync(plan, c.Primero, c.Segundo, "Probar recuperación protectora", new FallarTrasCompleta());
        Assert.False(r.Restaurada);
        Assert.True(r.RecuperadaAnterior);
        Assert.Equal(3, await c.ConteoAsync());
        Assert.False((await c.Motor.ConsultarAsync()).EnMantenimiento);
    }

    [SqlRespaldoFact]
    public async Task RevocarCuentaDespuesDelEnsayoInvalidaAprobacionSinModificarDestino()
    {
        await using var c = await Caso.CrearAsync();
        var copia = await c.Motor.CrearAsync(TipoRespaldo.Completa, c.Carpeta, "Admin");
        var plan = await c.PlanAsync(copia.Id);
        await c.AgregarAsync(2);
        await c.SqlAsync($"UPDATE {SqlRespaldos.Identificador(c.Nombre)}.dbo.Usuarios SET Activo=0 WHERE Id=2");
        await Assert.ThrowsAsync<InvalidOperationException>(() => c.Motor.RestaurarAsync(plan, c.Primero, c.Segundo, "La cuenta fue revocada"));
        Assert.Equal(2, await c.ConteoAsync());
        Assert.False((await c.Motor.ConsultarAsync()).EnMantenimiento);
    }

    [SqlRespaldoFact]
    public async Task AgenteEjecutaPlanVencidoUnaVezYAvanzaProximaFecha()
    {
        await using var c = await Caso.CrearAsync();
        var p = await c.Motor.ObtenerProgramaAsync();
        await c.Motor.GuardarProgramaAsync(p with { Activo = true, CarpetaServidor = c.Carpeta, ZonaHoraria = "UTC", ProximaUtc = DateTimeOffset.UtcNow.AddDays(-1), IdAdministrador = 1, Autor = "Admin" });
        await c.Motor.EjecutarProgramacionAsync();
        await c.Motor.EjecutarProgramacionAsync();
        Assert.Single((await c.Motor.ConsultarAsync()).Copias);
        var actual = await c.Motor.ObtenerProgramaAsync();
        Assert.True(actual.ProximaUtc > DateTimeOffset.UtcNow);
        Assert.NotNull(actual.UltimaSenalAgenteUtc);
        Assert.Contains("verificada", actual.UltimoResultado);
    }

    [SqlRespaldoFact]
    public async Task ConfirmacionFinalEsAtomicaEIdempotenteAnteRespuestaPerdida()
    {
        await using var c = await Caso.CrearAsync();
        var op = Guid.NewGuid();
        await c.SqlAsync($"UPDATE {SqlRespaldos.Identificador(c.Opciones.BaseControl)}.dbo.EstadoSGTM SET Mantenimiento=1,Operacion=@op", new SqlParameter("@op", op));
        await using var control = new SqlConnection(c.Opciones.Control());
        await control.OpenAsync();
        var registro = new RegistroRespaldosSql(c.Opciones);
        await registro.ConfirmarFinAsync(control, op, "Correcta", "Admin", "Recepción", "Prueba de confirmación", "Completada");
        await registro.ConfirmarFinAsync(control, op, "Correcta", "Admin", "Recepción", "Reintento tras respuesta perdida", "Completada");
        var estado = await c.Motor.ConsultarAsync();
        Assert.Equal(2, estado.Generacion);
        Assert.False(estado.EnMantenimiento);
        Assert.Single(estado.Historial, x => x.Accion == "Restauracion");
    }

    private sealed class FallarSinRecuperar : IProgress<string>
    {
        public void Report(string valor)
        {
            if (valor.StartsWith("Copia completa aplicada", StringComparison.Ordinal) || valor.StartsWith("La recuperación solicitada falló", StringComparison.Ordinal))
                throw new InvalidOperationException("Fallo de prueba para comprobar bloqueo durable");
        }
    }

    [SqlRespaldoFact]
    public async Task RecuperacionIncompletaMantieneBloqueadasLasOperacionesDeNegocio()
    {
        await using var c = await Caso.CrearAsync();
        await c.Motor.CrearAsync(TipoRespaldo.Completa, c.Carpeta, "Admin");
        await c.AgregarAsync(2);
        var diff = await c.Motor.CrearAsync(TipoRespaldo.Diferencial, c.Carpeta, "Admin");
        var plan = await c.PlanAsync(diff.Id);
        await Assert.ThrowsAsync<InvalidOperationException>(() => c.Motor.RestaurarAsync(plan, c.Primero, c.Segundo, "Probar bloqueo seguro ante recuperación incompleta", new FallarSinRecuperar()));
        var estado = await c.Motor.ConsultarAsync();
        Assert.True(estado.EnMantenimiento);
        Assert.Contains(estado.Historial, x => x.Resultado == "Requiere asistencia");
        await using var db = c.Contexto(new GeneracionMantenimiento());
        await Assert.ThrowsAsync<InvalidOperationException>(() => db.Database.ExecuteSqlRawAsync("SELECT 1"));
    }
}
