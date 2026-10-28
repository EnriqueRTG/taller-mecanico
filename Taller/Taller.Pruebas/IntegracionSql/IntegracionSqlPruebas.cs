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
                await new ClienteRepositorio(db).AgregarAsync(new Cliente { TipoDocumento = "DNI", Documento = "45678901", Nombre = "Alta concurrente", Apellido = "Prueba" });
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
                Cliente = new Cliente { TipoDocumento = "DNI", Documento = "33999111", Nombre = "Demo", Apellido = "Migración" },
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
    public async Task BaseNuevaSoloAdmiteEstadosVigentesYNoContieneClasificadoresObsoletos()
    {
        await using var b = await BasePrueba.CrearAsync();
        await using var db = b.Contexto();
        Assert.Equal(2, db.Database.GetMigrations().Count());
        await using var sql = new SqlConnection(b.Conexion);
        await sql.OpenAsync();
        var comando = new SqlCommand("SELECT COUNT(*) FROM sys.columns WHERE (object_id = OBJECT_ID(N'dbo.Presupuestos') OR object_id = OBJECT_ID(N'dbo.Diagnosticos')) AND name = N'Tipo' OR object_id = OBJECT_ID(N'dbo.Conceptos') AND name = N'DisponibleParaIngreso'", sql);
        Assert.Equal(0, (int)(await comando.ExecuteScalarAsync())!);
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
        var c = await s1.RegistrarAsync("DNI", "12345678", "Ana", "Pérez", null, null, null);
        var inicial = await s2.ObtenerPorIdAsync(c.IdCliente);
        var version = c.Version.ToArray();
        Assert.Equal(8, version.Length);
        await s1.ModificarAsync(c.IdCliente, "DNI", "12345678", "Cambio uno", "Pérez", null, null, null, version);
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => s2.ModificarAsync(c.IdCliente, "DNI", "12345678", "Cambio dos", "Pérez", null, null, null, inicial!.Version));
        Assert.Equal("Cambio uno", (await s2.ObtenerPorIdAsync(c.IdCliente))!.Nombre);
        var v1 = new VehiculoServicio(new VehiculoRepositorio(uno), new ModeloRepositorio(uno), Acceso(uno, ids.Recepcion, out _));
        var v2 = new VehiculoServicio(new VehiculoRepositorio(dos), new ModeloRepositorio(dos), Acceso(dos, ids.Recepcion, out _));
        var v = await v1.RegistrarAsync("AA123BB", 2020, "Azul", ids.Modelo);
        var anterior = await v2.ObtenerPorIdAsync(v.IdVehiculo);
        await v1.ModificarAsync(v.IdVehiculo, v.Dominio, 2020, "Gris", ids.Modelo, v.Version);
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => v2.DarDeBajaAsync(v.IdVehiculo, anterior!.Version));
        Assert.Equal("Gris", (await v2.ObtenerPorIdAsync(v.IdVehiculo))!.Color);
        await Assert.ThrowsAsync<ValidacionException>(() => s2.RegistrarAsync("DNI", "12345678", "Otro", "Cliente", null, null, null));
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
        var cliente = await clientes.RegistrarAsync("DNI", "23456789", "Lucía", "Gómez", null, null, null);
        var vehiculo = await vehiculos.RegistrarAsync("AC456DE", 2021, "Blanco", ids.Modelo);
        var recepcion = new AtencionServicio(new AtencionRepositorio(db), new ClienteRepositorio(db), new VehiculoRepositorio(db), usuarios, sesion);
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
            PrecioUnitario = 100
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
        await Assert.ThrowsAsync<ValidacionException>(() => cobro.EntregarAsync(a.IdAtencion, cuenta.VersionAtencion, "Entrega prematura"));
        await cobro.RegistrarPagoAsync(new RegistrarPagoDto { IdComprobante = cuenta.Comprobante.IdComprobante, Version = cuenta.Comprobante.Version, Medios = [new PagoMedioDto { IdMedioPago = efectivo.IdMedioPago, Importe = 100 }] });
        cuenta = await cobro.ObtenerAsync(a.IdAtencion);
        await cobro.EntregarAsync(a.IdAtencion, cuenta.VersionAtencion, "Retira el cliente con trabajos verificados");
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
        var cliente = await new ClienteServicio(new ClienteRepositorio(db), acceso).RegistrarAsync("DNI", "34567890", "Ana", "Prueba", null, null, null);
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
            TipoSolicitud = TipoSolicitudAtencion.ServicioDefinido
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
                PrecioUnitario = 100
            }, new()
            {
                IdConcepto = repuesto.IdConcepto,
                Descripcion = "Repuesto propuesto",
                Cantidad = 1,
                PrecioUnitario = 20
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
        await ps.DecidirAsync(new() { IdPresupuesto = p.IdPresupuesto, Version = original.Version, ItemsAceptados = [original.Items.First(i => i.IdConcepto == ids.Concepto).IdDetalle], Observaciones = "Cliente solicita solo la tarea independiente" });
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
        int adicional = await seg.CrearAdicionalAsync(new() { IdAtencion = a.IdAtencion, Version = seguimiento.Version, IdHallazgo = seguimiento.Hallazgos.Single().Id, Motivo = "El cliente aportará la pieza, el taller realiza el trabajo", Items = [new() { IdConcepto = repuesto.IdConcepto, Descripcion = "Pieza aportada", Cantidad = 1, OrigenMaterial = OrigenMaterial.Cliente }, new() { IdConcepto = ids.Concepto, Descripcion = "Trabajo adicional", Cantidad = 1, PrecioUnitario = 50 }] });
        sesion.Iniciar(new Usuario { Id = ids.Recepcion });
        var ad = await ps.ObtenerAsync(adicional);
        await ev.PresentarAsync(adicional, ad.Version);
        ad = await ps.ObtenerAsync(adicional);
        await ps.DecidirAsync(new() { IdPresupuesto = adicional, Version = ad.Version, ItemsAceptados = ad.Items.Select(i => i.IdDetalle).ToList() });
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
}
