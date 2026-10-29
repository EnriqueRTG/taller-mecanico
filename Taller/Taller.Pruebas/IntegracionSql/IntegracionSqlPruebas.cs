using Microsoft.Extensions.Configuration;
using Taller.Dominio.Tiempo;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.DTOs.Cobros;
using Taller.Aplicacion.DTOs.Presupuestos;
using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Seguridad;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
using Taller.Infraestructura.Persistencia;
using Taller.Infraestructura.Persistencia.Repositorios;
using Taller.Infraestructura.Respaldos;
using Xunit;

namespace Taller.Pruebas;
/// <summary>Pruebas con migraciones completas y rowversion nativo. Solo crea bases propias con GUID en una instancia desechable autorizada.</summary>
public sealed class IntegracionSqlPruebas
{
    private sealed class BasePrueba : IAsyncDisposable
    {
        public string Nombre = "SGTM_Pruebas_Integracion_" + Guid.NewGuid().ToString("N");
        public string Master = "", Conexion = "";
        public static async Task<BasePrueba> CrearAsync()
        {
            var b = new SqlConnectionStringBuilder(Environment.GetEnvironmentVariable("SGTM_SQL_PRUEBAS"));
            if (b.InitialCatalog != "master" || Environment.GetEnvironmentVariable("SGTM_PERMITIR_PRUEBAS_SQL") != "1")
                throw new InvalidOperationException("Solo se admiten instancias aisladas de prueba.");
            b.Pooling = false;
            var t = new BasePrueba
            {
                Master = b.ConnectionString
            };
            b.InitialCatalog = t.Nombre;
            t.Conexion = b.ConnectionString;
            await using var c = new SqlConnection(t.Master);
            await c.OpenAsync();
            await new SqlCommand("CREATE DATABASE " + SqlRespaldos.Identificador(t.Nombre), c)
            {
                CommandTimeout = 120
            }.ExecuteNonQueryAsync();
            try
            {
                await using var db = t.Contexto();
                db.Database.SetCommandTimeout(120);
                await db.Database.MigrateAsync();
                return t;
            }
            catch
            {
                await t.DisposeAsync();
                throw;
            }
        }

        public TallerDbContext Contexto() => new(new DbContextOptionsBuilder<TallerDbContext>().UseSqlServer(Conexion).Options);
        public async ValueTask DisposeAsync()
        {
            if (!Nombre.StartsWith("SGTM_Pruebas_Integracion_", StringComparison.Ordinal))
                throw new InvalidOperationException("No se elimina una base ajena a esta prueba.");
            await using var c = new SqlConnection(Master);
            await c.OpenAsync();
            await new SqlCommand($"IF DB_ID(@db) IS NOT NULL BEGIN ALTER DATABASE {SqlRespaldos.Identificador(Nombre)} SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE {SqlRespaldos.Identificador(Nombre)}; END", c)
            {
                Parameters =
                {
                    new SqlParameter("@db", Nombre)
                },
                CommandTimeout = 120
            }.ExecuteNonQueryAsync();
        }

        public async Task<(int Recepcion, int Tecnico, int Modelo, int Concepto)> DatosAsync()
        {
            await using var db = Contexto();
            var a = new Usuario
            {
                NombreUsuario = "recepcion-sql",
                Nombre = "Recepción",
                Apellido = "Prueba",
                PasswordHash = "hash-prueba",
                RolId = 2
            };
            var t = new Usuario
            {
                NombreUsuario = "tecnico-sql",
                Nombre = "Técnico",
                Apellido = "Prueba",
                PasswordHash = "hash-prueba",
                RolId = 3
            };
            var modelo = new Modelo
            {
                Nombre = "Modelo SQL",
                Marca = new Marca
                {
                    Nombre = "Marca SQL"
                }
            };
            var concepto = new Concepto
            {
                Nombre = "Trabajo SQL",
                Categoria = new Categoria
                {
                    Nombre = "Categoría SQL"
                },
                Tipo = TipoConcepto.Servicio,
                PrecioReferencia = 100
            };
            db.AddRange(a, t, modelo, concepto);
            await db.SaveChangesAsync();
            return (a.Id, t.Id, modelo.IdModelo, concepto.IdConcepto);
        }
    }

    private static AutorizacionOperativa Acceso(TallerDbContext db, int usuario, out SesionUsuario sesion)
    {
        sesion = new SesionUsuario();
        sesion.Iniciar(new Usuario { Id = usuario });
        return new(new UsuarioRepositorio(db), sesion);
    }

    [SqlRespaldoFact]
    public async Task IndiceUnicoResuelveDosAltasSimultaneasSinExponerSqlAlOperador()
    {
        await using var b = await BasePrueba.CrearAsync();
        await using var uno = b.Contexto();
        await using var dos = b.Contexto();
        async Task<Exception?> Insertar(TallerDbContext db)
        {
            try
            {
                await new ClienteRepositorio(db).AgregarAsync(new Cliente { Telefono = "3794123456", TipoDocumento = "DNI", Documento = "45678901", Nombre = "Alta concurrente", Apellido = "Prueba" });
                return null;
            }
            catch (Exception e)
            {
                return e;
            }
        }

        var resultados = await Task.WhenAll(Insertar(uno), Insertar(dos));
        Assert.Single(resultados, x => x is null);
        Assert.IsType<ValidacionException>(Assert.Single(resultados, x => x is not null));
        Assert.Single(await uno.Clientes.AsNoTracking().ToListAsync());
    }

    [SqlRespaldoFact]
    public async Task MigracionTresEstados_RechazaOmisionesAnterioresSinAlterarlas()
    {
        await using var b = await BasePrueba.CrearAsync();
        var ids = await b.DatosAsync();
        await using var db = b.Contexto();
        var migrador = db.GetService<IMigrator>();
        await migrador.MigrateAsync("20261026155115_EsquemaInicialCircuitoActual");
        var p = new Presupuesto { IdUsuarioCreacion = ids.Tecnico, FechaCreacion = FechaHoraTaller.Ahora,
            Atencion = new Atencion { IdUsuarioRecepcion = ids.Recepcion, MotivoConsulta = "Comprobar conservación de dato anterior",
                Cliente = new Cliente { Telefono = "3794123456", TipoDocumento = "DNI", Documento = "33999111", Nombre = "Demo", Apellido = "Migración" },
                Vehiculo = new Vehiculo { IdModelo = ids.Modelo, Dominio = "ZZ987XY", Color = "Gris" } },
            Detalles = [new PresupuestoDetalle { IdConcepto = ids.Concepto, Descripcion = "Caso anterior", Cantidad = 1, PrecioUnitario = 100 }] };
        db.Add(p); await db.SaveChangesAsync(); db.ChangeTracker.Clear();
        await db.Database.ExecuteSqlInterpolatedAsync($"UPDATE dbo.PresupuestoDetalles SET EstadoEjecucion=N'Omitido', ObservacionesEjecucion=N'Caso anterior' WHERE IdPresupuesto={p.IdPresupuesto}");
        var error = await Assert.ThrowsAsync<SqlException>(() => migrador.MigrateAsync());
        Assert.Equal(51140, error.Number);
        Assert.Single(await db.Database.GetAppliedMigrationsAsync());
        var estados = await db.Database.SqlQueryRaw<string>("SELECT EstadoEjecucion AS [Value] FROM dbo.PresupuestoDetalles").ToListAsync();
        Assert.Equal("Omitido", Assert.Single(estados));
    }

    [SqlRespaldoFact]
    public async Task Limpieza_RechazaVinculosNoNulosSinDescartarlos()
    {
        await using var b = await BasePrueba.CrearAsync();
        var ids = await b.DatosAsync();
        await using var db = b.Contexto();
        var migrador = db.GetService<IMigrator>();
        await migrador.MigrateAsync("20261028164124_SimplificarEjecucionTresEstados");
        var atencion = new Atencion
        {
            IdUsuarioRecepcion = ids.Recepcion, MotivoConsulta = "Preservar vínculo antiguo",
            Cliente = new Cliente { Telefono = "3794123456", TipoDocumento = "DNI", Documento = "33999112", Nombre = "Prueba", Apellido = "Limpieza" },
            Vehiculo = new Vehiculo { IdModelo = ids.Modelo, Dominio = "ZZ988XY", Color = "Gris" }
        };
        var primero = new Presupuesto { Atencion = atencion, IdUsuarioCreacion = ids.Tecnico,
            Detalles = [new PresupuestoDetalle { IdConcepto = ids.Concepto, Descripcion = "Ítem histórico", Cantidad = 1, PrecioUnitario = 100 }] };
        var segundo = new Presupuesto { Atencion = atencion, IdUsuarioCreacion = ids.Tecnico };
        db.AddRange(primero, segundo);
        await db.SaveChangesAsync();
        db.ChangeTracker.Clear();
        await db.Database.ExecuteSqlInterpolatedAsync($"UPDATE dbo.PresupuestoDetalles SET IdPresupuestoCambio={segundo.IdPresupuesto} WHERE IdPresupuesto={primero.IdPresupuesto}");
        var error = await Assert.ThrowsAsync<SqlException>(() => migrador.MigrateAsync());
        Assert.Equal(51141, error.Number);
        Assert.Equal(2, (await db.Database.GetAppliedMigrationsAsync()).Count());
        var vinculo = await db.Database.SqlQueryRaw<int>("SELECT IdPresupuestoCambio AS [Value] FROM dbo.PresupuestoDetalles").SingleAsync();
        Assert.Equal(segundo.IdPresupuesto, vinculo);
        await db.Database.ExecuteSqlRawAsync("UPDATE dbo.PresupuestoDetalles SET IdPresupuestoCambio=NULL");
        await migrador.MigrateAsync();
        Assert.Equal(6, (await db.Database.GetAppliedMigrationsAsync()).Count());
        Assert.Single(await db.PresupuestoDetalles.AsNoTracking().ToListAsync());
    }

    [SqlRespaldoFact]
    public async Task Usuarios_SPRechazanVersionObsoletaEnDatosCredencialesYEstado()
    {
        await using var b = await BasePrueba.CrearAsync();
        var ids = await b.DatosAsync();
        await using var uno = b.Contexto();
        await using var dos = b.Contexto();
        var r1 = new UsuarioRepositorio(uno);
        var r2 = new UsuarioRepositorio(dos);
        var original = (await r1.ObtenerPorIdAsync(ids.Tecnico))!;
        Assert.Equal(8, original.Version.Length);
        await r1.ActualizarDatosAsync(original.Id, "Cambio", "Prueba", 3, original.Version);
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => r2.ActualizarDatosAsync(original.Id, "Obsoleto", "Prueba", 3, original.Version));
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => r2.ActualizarCredencialesAsync(original.Id, original.NombreUsuario, "hash rechazado", original.Version));
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => r2.CambiarEstadoAsync(original.Id, false, original.Version));
        var actual = (await r2.ObtenerPorIdAsync(original.Id))!;
        Assert.Equal("Cambio", actual.Nombre);
        Assert.True(actual.Activo);
        Assert.Equal(original.PasswordHash, actual.PasswordHash);
        await r2.ActualizarCredencialesAsync(actual.Id, "tecnico.actualizado", null, actual.Version);
        actual = (await r1.ObtenerPorIdAsync(actual.Id))!;
        Assert.Equal("tecnico.actualizado", actual.NombreUsuario);
        await r1.CambiarEstadoAsync(actual.Id, false, actual.Version);
        Assert.False((await r2.ObtenerPorIdAsync(actual.Id))!.Activo);
    }

    [SqlRespaldoFact]
    public async Task AltaAtencion_RevalidaMaestrosAntesDeGuardarYPermiteReintentarSinResiduos()
    {
        await using var b = await BasePrueba.CrearAsync();
        var ids = await b.DatosAsync();
        await using var db = b.Contexto();
        var acceso = Acceso(db, ids.Recepcion, out _);
        var cliente = await new ClienteServicio(new ClienteRepositorio(db), acceso).RegistrarAsync("DNI", "45678903", "Ana", "Prueba", "3794123456", null, null);
        var vehiculo = await new VehiculoServicio(new VehiculoRepositorio(db), new ModeloRepositorio(db), acceso).RegistrarAsync("AB987CD", 2020, "Gris", ids.Modelo);
        await using var otro = b.Contexto();
        await otro.Database.ExecuteSqlInterpolatedAsync($"UPDATE dbo.Clientes SET Activo=0 WHERE IdCliente={cliente.IdCliente}");
        Atencion Nueva() => new() { IdCliente = cliente.IdCliente, IdVehiculo = vehiculo.IdVehiculo, IdUsuarioRecepcion = ids.Recepcion,
            MotivoConsulta = "Solicitud de servicio", Solicitudes = [new() { Orden = 1, Tipo = TipoSolicitudIngreso.TrabajoSolicitado, Descripcion = "Servicio solicitado" }],
            Historial = [new() { IdUsuario = ids.Recepcion, Accion = "RecepcionRegistrada", EstadoNuevo = EstadoAtencion.EnEvaluacionTecnica }] };
        var repo = new AtencionRepositorio(db);
        await Assert.ThrowsAsync<ValidacionException>(() => repo.AgregarAsync(Nueva()));
        Assert.Empty(db.ChangeTracker.Entries());
        await otro.Database.ExecuteSqlInterpolatedAsync($"UPDATE dbo.Clientes SET Activo=1 WHERE IdCliente={cliente.IdCliente}");
        await repo.AgregarAsync(Nueva());
        Assert.Single(await db.Atenciones.ToListAsync());
        Assert.Single(await db.Set<SolicitudAtencion>().ToListAsync());
        Assert.Single(await db.HistorialAtenciones.ToListAsync());
    }

    [SqlRespaldoFact]
    public async Task TelefonoObligatorio_NoInventaDatosYProtegeEscriturasDirectas()
    {
        await using var b = await BasePrueba.CrearAsync();
        await using var db = b.Contexto();
        var migrador = db.GetService<IMigrator>();
        await migrador.MigrateAsync("20261029090000_RetirarVinculoCambioSinUso");
        await db.Database.ExecuteSqlRawAsync("INSERT INTO dbo.Clientes (TipoDocumento, Documento, Nombre, Apellido, Telefono, Activo, FechaAlta) VALUES (N'DNI', N'45678902', N'Ana', N'Prueba', NULL, 1, GETDATE())");
        var error = await Assert.ThrowsAsync<SqlException>(() => migrador.MigrateAsync());
        Assert.Equal(51142, error.Number);
        Assert.Equal(3, (await db.Database.GetAppliedMigrationsAsync()).Count());
        Assert.Equal(1, await db.Database.SqlQueryRaw<int>("SELECT COUNT(*) AS [Value] FROM dbo.Clientes WHERE Telefono IS NULL").SingleAsync());
        await db.Database.ExecuteSqlRawAsync("UPDATE dbo.Clientes SET Telefono=N'+54 (379) 412-3456'");
        await migrador.MigrateAsync();
        Assert.Equal(6, (await db.Database.GetAppliedMigrationsAsync()).Count());
        var invalido = await Assert.ThrowsAsync<SqlException>(() => db.Database.ExecuteSqlRawAsync("UPDATE dbo.Clientes SET Telefono=N'abc'"));
        Assert.Equal(547, invalido.Number);
        Assert.Equal("+54 (379) 412-3456", await db.Clientes.AsNoTracking().Select(c => c.Telefono).SingleAsync());
    }

    [SqlRespaldoFact]
    public async Task BaseNuevaSoloAdmiteEstadosVigentesYNoContieneClasificadoresObsoletos()
    {
        await using var b = await BasePrueba.CrearAsync();
        await using var db = b.Contexto();
        Assert.Equal(6, db.Database.GetMigrations().Count());
        await using var sql = new SqlConnection(b.Conexion);
        await sql.OpenAsync();
        var comando = new SqlCommand("SELECT COUNT(*) FROM sys.columns WHERE (object_id = OBJECT_ID(N'dbo.Presupuestos') OR object_id = OBJECT_ID(N'dbo.Diagnosticos')) AND name = N'Tipo' OR object_id = OBJECT_ID(N'dbo.Conceptos') AND name = N'DisponibleParaIngreso'", sql);
        Assert.Equal(0, (int)(await comando.ExecuteScalarAsync())!);
        var columnaRetirada = new SqlCommand("SELECT COUNT(*) FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.PresupuestoDetalles') AND name=N'IdPresupuestoCambio'", sql);
        Assert.Equal(0, (int)(await columnaRetirada.ExecuteScalarAsync())!);
        var restricciones = (await db.Database.SqlQueryRaw<string>("SELECT definition AS [Value] FROM sys.check_constraints WHERE name IN (N'CK_Atenciones_Estado', N'CK_HistorialAtenciones_EstadoNuevo', N'CK_HistorialAtenciones_EstadoAnterior')").ToListAsync());
        Assert.NotEmpty(restricciones);
        Assert.All(restricciones, texto => { Assert.DoesNotContain("Abierta", texto); Assert.DoesNotContain("PendienteDiagnostico", texto); Assert.DoesNotContain("Pausada", texto); });
    }

    [SqlRespaldoFact]
    public async Task MigracionCompleta_ConservaTablasDelCircuitoYProtegeEdicionesObsoletas()
    {
        await using var b = await BasePrueba.CrearAsync();
        var ids = await b.DatosAsync();
        await using var uno = b.Contexto();
        await using var dos = b.Contexto();
        var s1 = new ClienteServicio(new ClienteRepositorio(uno), Acceso(uno, ids.Recepcion, out _));
        var s2 = new ClienteServicio(new ClienteRepositorio(dos), Acceso(dos, ids.Recepcion, out _));
        var c = await s1.RegistrarAsync("DNI", "12345678", "Ana", "Pérez", "3794123456", null, null);
        var inicial = await s2.ObtenerPorIdAsync(c.IdCliente);
        var version = c.Version.ToArray();
        Assert.Equal(8, version.Length);
        await s1.ModificarAsync(c.IdCliente, "DNI", "12345678", "Cambio uno", "Pérez", "3794123456", null, null, version);
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => s2.ModificarAsync(c.IdCliente, "DNI", "12345678", "Cambio dos", "Pérez", "3794123456", null, null, inicial!.Version));
        Assert.Equal("Cambio uno", (await s2.ObtenerPorIdAsync(c.IdCliente))!.Nombre);
        var v1 = new VehiculoServicio(new VehiculoRepositorio(uno), new ModeloRepositorio(uno), Acceso(uno, ids.Recepcion, out _));
        var v2 = new VehiculoServicio(new VehiculoRepositorio(dos), new ModeloRepositorio(dos), Acceso(dos, ids.Recepcion, out _));
        var v = await v1.RegistrarAsync("AA123BB", 2020, "Azul", ids.Modelo);
        var anterior = await v2.ObtenerPorIdAsync(v.IdVehiculo);
        await v1.ModificarAsync(v.IdVehiculo, v.Dominio, 2020, "Gris", ids.Modelo, v.Version);
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => v2.DarDeBajaAsync(v.IdVehiculo, anterior!.Version));
        Assert.Equal("Gris", (await v2.ObtenerPorIdAsync(v.IdVehiculo))!.Color);
        await Assert.ThrowsAsync<ValidacionException>(() => s2.RegistrarAsync("DNI", "12345678", "Otro", "Cliente", "3794123456", null, null));
        Assert.Empty(await uno.Diagnosticos.ToListAsync());
        Assert.Empty(await uno.Presupuestos.ToListAsync());
        Assert.False(uno.Database.HasPendingModelChanges());
    }

    [SqlRespaldoFact]
    public async Task CircuitoConDatosReales_DesdeAltaHastaPagosMixtosYEntregaSinSaldo()
    {
        await using var b = await BasePrueba.CrearAsync();
        var ids = await b.DatosAsync();
        await using var db = b.Contexto();
        var acceso = Acceso(db, ids.Recepcion, out var sesion);
        var usuarios = new UsuarioRepositorio(db);
        var er = new EvaluacionRepositorio(db);
        var cr = new CircuitoRepositorio(db);
        var unidad = new UnidadDeTrabajo(db);
        var clientes = new ClienteServicio(new ClienteRepositorio(db), acceso);
        var vehiculos = new VehiculoServicio(new VehiculoRepositorio(db), new ModeloRepositorio(db), acceso);
        var cliente = await clientes.RegistrarAsync("DNI", "23456789", "Lucía", "Gómez", "3794123456", null, null);
        var vehiculo = await vehiculos.RegistrarAsync("AC456DE", 2021, "Blanco", ids.Modelo);
        var recepcion = new AtencionServicio(new AtencionRepositorio(db), new ClienteRepositorio(db), new VehiculoRepositorio(db), new Taller.Aplicacion.Seguridad.AutorizacionOperativa(usuarios, sesion));
        var a = await recepcion.RegistrarRecepcionAsync(new RegistrarRecepcionDto {
            IdCliente = cliente.IdCliente, IdVehiculo = vehiculo.IdVehiculo, IdUsuarioRecepcion = ids.Recepcion,
            Kilometraje = 80000, Solicitudes = [new() { Tipo = TipoSolicitudIngreso.ProblemaInformado, Descripcion = "Ruido persistente al frenar" }]
        });
        Assert.Single(a.Solicitudes);
        var evaluacion = new EvaluacionServicio(er, usuarios, unidad, sesion);
        // RegistrarRecepcionAsync ya deja la atención en preparación, sin una segunda transición.
        sesion.Iniciar(new Usuario { Id = ids.Tecnico });
        await evaluacion.TomarAsync(a.IdAtencion);
        var d = await evaluacion.ObtenerAsync(a.IdAtencion);
        d.IncluyeDiagnostico = true;
        d.FallaDetectada = "Se detectó desgaste de componentes";
        d.TrabajoRecomendado = "Reemplazar componentes y verificar";
        d.Items = [new ItemPropuestaDto
        {
            IdConcepto = ids.Concepto,
            Descripcion = "Trabajo acordado",
            Cantidad = 2,
            PrecioUnitario = 100,
                IdSolicitudAtencion = 1
            }

        ];
        foreach (var item in d.Items) item.IdSolicitudAtencion = d.Solicitudes.FirstOrDefault()?.IdSolicitud;
        await evaluacion.GuardarAsync(d, true);
        sesion.Iniciar(new Usuario { Id = ids.Recepcion });
        var p = await db.Presupuestos.AsNoTracking().SingleAsync(x => !x.EsAdicional);
        db.ChangeTracker.Clear();
        await evaluacion.PresentarAsync(p.IdPresupuesto, p.Version);
        var presupuestos = new PresupuestoServicio(er, cr, unidad, acceso);
        var propuesta = await presupuestos.ObtenerAsync(p.IdPresupuesto);
        await presupuestos.DecidirAsync(new DecisionPresupuestoDto { PersonaDecision = "Lucía Gómez", MedioDecision = "Presencial", IdPresupuesto = p.IdPresupuesto, Version = propuesta.Version, ItemsAceptados = propuesta.Items.Select(x => x.IdDetalle).ToList() });
        sesion.Iniciar(new Usuario { Id = ids.Tecnico });
        var ejecucion = new EjecucionServicio(er, cr, unidad, acceso);
        var e = await ejecucion.ObtenerAsync(a.IdAtencion);
        await ejecucion.TomarAsync(a.IdAtencion, e.Version, "");
        e = await ejecucion.ObtenerAsync(a.IdAtencion);
        e.Items[0].Estado = EstadoEjecucionDetalle.EnCurso;
        await ejecucion.GuardarAsync(e, false);
        sesion.Iniciar(new Usuario { Id = ids.Recepcion });
        Assert.Equal(1, Assert.Single(await ejecucion.ListarAsync()).EnCurso);
        var seguimiento = new SeguimientoTrabajoServicio(er, cr, unidad, acceso);
        var consulta = await seguimiento.ObtenerAsync(a.IdAtencion);
        Assert.False(consulta.PuedeRegistrar);
        Assert.Contains(consulta.Progreso, i => i.Estado == EstadoEjecucionDetalle.EnCurso);
        sesion.Iniciar(new Usuario { Id = ids.Tecnico });
        e = await ejecucion.ObtenerAsync(a.IdAtencion);
        foreach (var item in e.Items)
        {
            item.CantidadEjecutada = item.CantidadAutorizada;
            item.Estado = EstadoEjecucionDetalle.Completado;
        }

        e.ConformidadTecnica = true;
        await ejecucion.GuardarAsync(e, true);
        sesion.Iniciar(new Usuario { Id = ids.Recepcion });
        var cobro = new CobroServicio(cr, er, unidad, acceso);
        var cuenta = await cobro.ObtenerAsync(a.IdAtencion);
        await cobro.LiquidarAsync(a.IdAtencion, cuenta.VersionAtencion, "Liquidación real de prueba");
        cuenta = await cobro.ObtenerAsync(a.IdAtencion);
        Assert.Equal(200, cuenta.Comprobante!.Total);
        var medios = await cobro.ListarMediosAsync();
        var efectivo = medios.First(m => !m.RequiereReferencia);
        var transferencia = medios.First(m => m.RequiereReferencia);
        var pago = new RegistrarPagoDto
        {
            IdComprobante = cuenta.Comprobante.IdComprobante,
            Version = cuenta.Comprobante.Version,
            Medios = [new PagoMedioDto
            {
                IdMedioPago = efectivo.IdMedioPago,
                Importe = 40
            }, new PagoMedioDto
            {
                IdMedioPago = transferencia.IdMedioPago,
                Importe = 60,
                Referencia = "TRANSFERENCIA-PRUEBA"
            }

            ]
        };
        await cobro.RegistrarPagoAsync(pago);
        await cobro.RegistrarPagoAsync(pago);
        cuenta = await cobro.ObtenerAsync(a.IdAtencion);
        Assert.Single(cuenta.Comprobante!.Pagos);
        Assert.Equal(100, cuenta.Comprobante.Saldo);
        await Assert.ThrowsAsync<ValidacionException>(() => cobro.EntregarAsync(new Taller.Aplicacion.DTOs.Cobros.RegistrarEntregaDto { IdAtencion = a.IdAtencion, Version = cuenta.VersionAtencion, PersonaRetira = "Cliente de prueba", ConformidadRecepcion = true, Observaciones = "Entrega prematura" }));
        await cobro.RegistrarPagoAsync(new RegistrarPagoDto { IdComprobante = cuenta.Comprobante.IdComprobante, Version = cuenta.Comprobante.Version, Medios = [new PagoMedioDto { IdMedioPago = efectivo.IdMedioPago, Importe = 100 }] });
        cuenta = await cobro.ObtenerAsync(a.IdAtencion);
        await cobro.EntregarAsync(new Taller.Aplicacion.DTOs.Cobros.RegistrarEntregaDto { IdAtencion = a.IdAtencion, Version = cuenta.VersionAtencion, PersonaRetira = "Cliente de prueba", ConformidadRecepcion = true, Observaciones = "Retira el cliente con trabajos verificados" });
        Assert.Equal(EstadoAtencion.Cerrada, (await recepcion.ObtenerPorIdAsync(a.IdAtencion))!.Estado);
        Assert.Single(await db.Diagnosticos.ToListAsync());
        Assert.Equal(2, await db.Set<Pago>().CountAsync());
    }

    [SqlRespaldoFact]
    public async Task SeleccionParcialHallazgoAdicionalYMaterialCliente_ConRestriccionesSQLYVersionReal()
    {
        await using var b = await BasePrueba.CrearAsync();
        var ids = await b.DatosAsync();
        await using var db = b.Contexto();
        var acceso = Acceso(db, ids.Recepcion, out var sesion);
        var usuarios = new UsuarioRepositorio(db);
        var er = new EvaluacionRepositorio(db);
        var cr = new CircuitoRepositorio(db);
        var unidad = new UnidadDeTrabajo(db);
        var cliente = await new ClienteServicio(new ClienteRepositorio(db), acceso).RegistrarAsync("DNI", "34567890", "Ana", "Prueba", "3794123456", null, null);
        var vehiculo = await new VehiculoServicio(new VehiculoRepositorio(db), new ModeloRepositorio(db), acceso).RegistrarAsync("AD789FG", 2020, "Gris", ids.Modelo);
        var concepto = await db.Conceptos.SingleAsync(c => c.IdConcepto == ids.Concepto);
        var repuesto = new Concepto
        {
            Nombre = "Repuesto SQL",
            Tipo = TipoConcepto.Repuesto,
            IdCategoria = concepto.IdCategoria,
            Activo = true
        };
        db.Add(repuesto);
        await db.SaveChangesAsync();
        db.ChangeTracker.Clear();
        var a = new Atencion
        {
            IdCliente = cliente.IdCliente,
            IdVehiculo = vehiculo.IdVehiculo,
            IdUsuarioRecepcion = ids.Recepcion,
            Estado = EstadoAtencion.PendienteDecision,
            MotivoConsulta = "Servicio definido solicitado",
            TipoSolicitud = TipoSolicitudAtencion.ServicioDefinido,
            Solicitudes = [new() { Orden = 1, Tipo = TipoSolicitudIngreso.TrabajoSolicitado, Descripcion = "Trabajo principal solicitado" }]
        };
        db.Add(a);
        await db.SaveChangesAsync();
        var p = new Presupuesto
        {
            IdAtencion = a.IdAtencion,

            Estado = EstadoPresupuesto.PendienteDecision,
            IdUsuarioCreacion = ids.Tecnico,
            FechaCreacion = FechaHoraTaller.Ahora,
            FechaPresentacion = FechaHoraTaller.Ahora,
            Detalles = [new()
            {
                IdConcepto = ids.Concepto,
                Descripcion = "Trabajo principal",
                Cantidad = 1,
                PrecioUnitario = 100,
                IdSolicitudAtencion = a.Solicitudes.Single().IdSolicitudAtencion
            }, new()
            {
                IdConcepto = repuesto.IdConcepto,
                Descripcion = "Repuesto propuesto",
                Cantidad = 1,
                PrecioUnitario = 20,
                IdSolicitudAtencion = a.Solicitudes.Single().IdSolicitudAtencion
            }

            ]
        };
        db.Add(p);
        await db.SaveChangesAsync();
        db.ChangeTracker.Clear();
        var ps = new PresupuestoServicio(er, cr, unidad, acceso);
        var ev = new EvaluacionServicio(er, usuarios, unidad, sesion);
        var seg = new SeguimientoTrabajoServicio(er, cr, unidad, acceso);
        var ejec = new EjecucionServicio(er, cr, unidad, acceso);
        var cobro = new CobroServicio(cr, er, unidad, acceso);
        var original = await ps.ObtenerAsync(p.IdPresupuesto);
        await ps.DecidirAsync(new() { IdPresupuesto = p.IdPresupuesto, Version = original.Version, ItemsAceptados = [original.Items.First(i => i.IdConcepto == ids.Concepto).IdDetalle], Observaciones = "Cliente solicita solo la tarea independiente" , PersonaDecision = "Cliente de prueba" , MedioDecision = "Presencial" });
        sesion.Iniciar(new Usuario { Id = ids.Tecnico });
        await Assert.ThrowsAsync<ValidacionException>(() => ejec.ObtenerAsync(a.IdAtencion));
        original = await ps.ObtenerAsync(p.IdPresupuesto);
        var elegido = original.Items.Single(i => i.Aceptar);
        int revisado = await seg.RevisarSeleccionAsync(p.IdPresupuesto, original.Version,
            [new() { IdSolicitudAtencion = elegido.IdSolicitudAtencion, IdConcepto = elegido.IdConcepto, Descripcion = elegido.Descripcion,
                Cantidad = elegido.Cantidad, PrecioUnitario = elegido.PrecioUnitario, OrigenMaterial = elegido.OrigenMaterial }], "Es técnicamente viable ejecutar por separado");
        Assert.Equal(p.IdPresupuesto, revisado);
        var rev = await ps.ObtenerAsync(revisado);
        Assert.Equal(EstadoPresupuesto.Aceptado, rev.Estado);
        await Assert.ThrowsAsync<SqlException>(() => db.Database.ExecuteSqlInterpolatedAsync($"UPDATE dbo.PresupuestoDetalles SET EstadoEjecucion=N'Omitido', ObservacionesEjecucion=N'No permitido' WHERE IdPresupuestoDetalle={elegido.IdDetalle}"));
        sesion.Iniciar(new Usuario { Id = ids.Tecnico });
        var e = await ejec.ObtenerAsync(a.IdAtencion);
        await ejec.TomarAsync(a.IdAtencion, e.Version, "");
        var seguimiento = await seg.ObtenerAsync(a.IdAtencion);
        int tarea = seguimiento.Items.Single().IdDetalle;
        await seg.RegistrarHallazgoAsync(new() { IdAtencion = a.IdAtencion, Version = seguimiento.Version, IdDetalle = tarea, Descripcion = "Hallazgo durante el desarme", Recomendacion = "Requiere material adicional", ImpideContinuar = true });
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => seg.PausarAsync(a.IdAtencion, seguimiento.Version, null, "Token anterior al hallazgo"));
        seguimiento = await seg.ObtenerAsync(a.IdAtencion);
        int adicional = await seg.CrearAdicionalAsync(new() { IdAtencion = a.IdAtencion, Version = seguimiento.Version, IdHallazgo = seguimiento.Hallazgos.Single().Id, Motivo = "El cliente aportará la pieza, el taller realiza el trabajo", Items = [new() { IdConcepto = repuesto.IdConcepto, Descripcion = "Pieza aportada", Cantidad = 1, OrigenMaterial = OrigenMaterial.Cliente }, new() { IdConcepto = ids.Concepto, Descripcion = "Trabajo adicional", Cantidad = 1, PrecioUnitario = 50 , IdSolicitudAtencion = a.Solicitudes.Single().IdSolicitudAtencion }] });
        sesion.Iniciar(new Usuario { Id = ids.Recepcion });
        var ad = await ps.ObtenerAsync(adicional);
        await ev.PresentarAsync(adicional, ad.Version);
        ad = await ps.ObtenerAsync(adicional);
        await ps.DecidirAsync(new() { IdPresupuesto = adicional, Version = ad.Version, ItemsAceptados = ad.Items.Select(i => i.IdDetalle).ToList() , PersonaDecision = "Cliente de prueba" , MedioDecision = "Presencial" });
        sesion.Iniciar(new Usuario { Id = ids.Tecnico });
        seguimiento = await seg.ObtenerAsync(a.IdAtencion);
        var material = seguimiento.Items.Single(i => i.Origen == OrigenMaterial.Cliente);
        await seg.RecibirMaterialAsync(a.IdAtencion, seguimiento.Version, material.IdDetalle, 1, true, "Pieza recibida y verificada apta");
        seguimiento = await seg.ObtenerAsync(a.IdAtencion);
        await seg.ResolverPausaAsync(a.IdAtencion, seguimiento.Version, seguimiento.Pausas.Single().Id, "Material disponible; puede continuar");
        e = await ejec.ObtenerAsync(a.IdAtencion);
        Assert.Equal(3, e.Items.Count);
        foreach (var item in e.Items)
        {
            item.CantidadEjecutada = item.CantidadAutorizada;
            item.Estado = EstadoEjecucionDetalle.Completado;
        }

        e.ConformidadTecnica = true;
        await ejec.GuardarAsync(e, true);
        sesion.Iniciar(new Usuario { Id = ids.Recepcion });
        var cuenta = await cobro.ObtenerAsync(a.IdAtencion);
        await cobro.LiquidarAsync(a.IdAtencion, cuenta.VersionAtencion, "Liquidar trabajos realizados");
        cuenta = await cobro.ObtenerAsync(a.IdAtencion);
        Assert.Equal(150, cuenta.Comprobante!.Total);
        Assert.Equal(2, cuenta.Comprobante.Items.Count);
        Assert.Equal(p.IdPresupuesto, revisado);
        Assert.True((await ps.ObtenerAsync(p.IdPresupuesto)).Vigente);
        Assert.True((await ps.ObtenerAsync(revisado)).Vigente);
        Assert.True((await ps.ObtenerAsync(adicional)).Vigente);
        Assert.Single(await db.Set<HallazgoAtencion>().ToListAsync());
        Assert.NotNull((await db.Set<PausaTrabajo>().SingleAsync()).FechaResolucion);
    }
    [SqlRespaldoFact]
    public async Task InicializacionConcurrente_CreaUnaSolaCuentaYCatalogoSinDuplicados()
    {
        await using var b = await BasePrueba.CrearAsync();
        await using var uno = b.Contexto();
        await using var dos = b.Contexto();
        var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder().AddInMemoryCollection(
            new Dictionary<string, string?> { ["Instalacion:ClaveAdministrador"] = "A1!" + Guid.NewGuid().ToString("N") }).Build();
        var hash = new Taller.Infraestructura.Seguridad.PasswordHasher();
        await Task.WhenAll(
            new Taller.Infraestructura.Persistencia.Inicializacion.InicializadorDatos(uno, hash, config).InicializarAsync(),
            new Taller.Infraestructura.Persistencia.Inicializacion.InicializadorDatos(dos, hash, config).InicializarAsync());
        Assert.Single(await uno.Usuarios.AsNoTracking().ToListAsync());
        Assert.Equal(10, await uno.Marcas.CountAsync());
        Assert.Equal(20, await uno.Modelos.CountAsync());
        Assert.Empty(uno.ChangeTracker.Entries());
        Assert.Empty(dos.ChangeTracker.Entries());
    }

    [SqlRespaldoFact]
    public async Task DecisionPersistida_ExigeContactoYLaMigracionNoInventaDatos()
    {
        await using var b = await BasePrueba.CrearAsync();
        var ids = await b.DatosAsync();
        await using var db = b.Contexto();
        var p = new Presupuesto { IdUsuarioCreacion = ids.Tecnico,
            Atencion = new Atencion { IdUsuarioRecepcion = ids.Recepcion, MotivoConsulta = "Contacto de decisión",
                Cliente = new Cliente { Telefono = "3794123456", Documento = "34111222", TipoDocumento = "DNI", Nombre = "Ana", Apellido = "Prueba" },
                Vehiculo = new Vehiculo { IdModelo = ids.Modelo, Dominio = "AE111AA", Color = "Gris" } } };
        db.Add(p); await db.SaveChangesAsync(); db.ChangeTracker.Clear();
        var rechazo = await Assert.ThrowsAsync<SqlException>(() => db.Database.ExecuteSqlInterpolatedAsync($"UPDATE dbo.Presupuestos SET Estado=N'Rechazado', FechaPresentacion=FechaCreacion, FechaDecision=FechaCreacion, IdUsuarioDecision={ids.Recepcion}, PersonaDecision=NULL, MedioDecision=N'Presencial' WHERE IdPresupuesto={p.IdPresupuesto}"));
        Assert.Equal(547, rechazo.Number);
        var migrador = db.GetService<IMigrator>();
        await migrador.MigrateAsync("20261029210000_AgregarConcurrenciaUsuarios");
        await db.Database.ExecuteSqlInterpolatedAsync($"UPDATE dbo.Presupuestos SET Estado=N'Rechazado', FechaPresentacion=FechaCreacion, FechaDecision=FechaCreacion, IdUsuarioDecision={ids.Recepcion}, PersonaDecision=NULL, MedioDecision=N'Presencial' WHERE IdPresupuesto={p.IdPresupuesto}");
        var error = await Assert.ThrowsAsync<SqlException>(() => migrador.MigrateAsync());
        Assert.Equal(51144, error.Number);
        Assert.Equal(5, (await db.Database.GetAppliedMigrationsAsync()).Count());
        Assert.Null((await db.Presupuestos.AsNoTracking().SingleAsync()).PersonaDecision);
        await db.Database.ExecuteSqlInterpolatedAsync($"UPDATE dbo.Presupuestos SET PersonaDecision=N'Ana Prueba' WHERE IdPresupuesto={p.IdPresupuesto}");
        await migrador.MigrateAsync();
        Assert.Equal(6, (await db.Database.GetAppliedMigrationsAsync()).Count());
        var medioInvalido = await Assert.ThrowsAsync<SqlException>(() => db.Database.ExecuteSqlInterpolatedAsync($"UPDATE dbo.Presupuestos SET MedioDecision=N'Desconocido' WHERE IdPresupuesto={p.IdPresupuesto}"));
        Assert.Equal(547, medioInvalido.Number);
    }

    [SqlRespaldoFact]
    public async Task DiagnosticoDuplicado_TraduceElIndiceActualYRevierteElGuardado()
    {
        await using var b = await BasePrueba.CrearAsync();
        var ids = await b.DatosAsync();
        await using var db = b.Contexto();
        var a = new Atencion { IdUsuarioRecepcion = ids.Recepcion, MotivoConsulta = "Informe duplicado",
            Cliente = new Cliente { Telefono = "3794123456", Documento = "34111223", TipoDocumento = "DNI", Nombre = "Ana", Apellido = "Prueba" },
            Vehiculo = new Vehiculo { IdModelo = ids.Modelo, Dominio = "AE112AA", Color = "Gris" } };
        db.Add(a); await db.SaveChangesAsync(); db.ChangeTracker.Clear();
        db.AddRange(Enumerable.Range(0, 2).Select(_ => new Diagnostico { IdAtencion = a.IdAtencion,
            IdTecnicoAutor = ids.Tecnico, FallaDetectada = "Falla", TrabajoRecomendado = "Trabajo" }));
        var error = await Assert.ThrowsAsync<ValidacionException>(() => new UnidadDeTrabajo(db).GuardarCambiosAsync());
        Assert.Contains("informe técnico definitivo", error.Message);
        Assert.Empty(db.ChangeTracker.Entries());
        Assert.Empty(await db.Diagnosticos.ToListAsync());
    }

    [SqlRespaldoFact]
    public async Task LecturaConsistente_NoMezclaUnCambioConcurrenteYLiberaBloqueosAlFinal()
    {
        await using var b = await BasePrueba.CrearAsync();
        await using var uno = b.Contexto();
        await using var dos = b.Contexto();
        var c = new Cliente { Documento = "34111224", TipoDocumento = "DNI", Nombre = "Antes", Apellido = "Prueba", Telefono = "3794123456" };
        uno.Add(c); await uno.SaveChangesAsync(); uno.ChangeTracker.Clear();
        var unidad = new UnidadDeTrabajo(uno);
        var valor = await unidad.LeerConsistenteAsync(async () =>
        {
            var primero = await uno.Clientes.AsNoTracking().SingleAsync(x => x.IdCliente == c.IdCliente);
            var error = await Assert.ThrowsAsync<SqlException>(() => dos.Database.ExecuteSqlInterpolatedAsync($"SET LOCK_TIMEOUT 250; UPDATE dbo.Clientes SET Nombre=N'Después' WHERE IdCliente={c.IdCliente}"));
            Assert.Equal(1222, error.Number);
            var segundo = await uno.Clientes.AsNoTracking().SingleAsync(x => x.IdCliente == c.IdCliente);
            Assert.Equal(primero.Nombre, segundo.Nombre);
            return segundo.Nombre;
        });
        Assert.Equal("Antes", valor);
        await dos.Database.ExecuteSqlInterpolatedAsync($"UPDATE dbo.Clientes SET Nombre=N'Después' WHERE IdCliente={c.IdCliente}");
        Assert.Equal("Después", (await uno.Clientes.AsNoTracking().SingleAsync()).Nombre);
    }

}
