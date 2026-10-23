using Taller.Dominio.Tiempo;
using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.DTOs.Cobros;
using Taller.Aplicacion.DTOs.Presupuestos;
using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Seguridad;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
using Taller.Infraestructura.Persistencia;
using Taller.Infraestructura.Persistencia.Repositorios;
using Xunit;

namespace Taller.Pruebas;
/// <summary>Recorridos de negocio con repositorios reales. InMemory no reproduce restricciones o transacciones de SQL Server.</summary>
public sealed class CircuitoCobrosPruebas
{
    private sealed class Caso : IDisposable
    {
        public TallerDbContext Db { get; }
        public SesionUsuario Sesion { get; } = new();
        public EvaluacionServicio Evaluacion { get; }
        public PresupuestoServicio Presupuestos { get; }
        public EjecucionServicio Ejecucion { get; }
        public SeguimientoTrabajoServicio Seguimiento { get; }
        public CobroServicio Cobros { get; }
        public AtencionServicio Recepcion { get; }

        private long _version;
        public Caso()
        {
            Db = new TallerDbContext(new DbContextOptionsBuilder<TallerDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            Db.Database.EnsureCreated();
            var cliente = new Cliente
            {
                IdCliente = 1,
                Documento = "12345678",
                TipoDocumento = "DNI",
                Nombre = "Ana",
                Apellido = "Pérez"
            };
            var marca = new Marca
            {
                IdMarca = 1,
                Nombre = "Marca",
                Activo = true
            };
            var modelo = new Modelo
            {
                IdModelo = 1,
                IdMarca = 1,
                Marca = marca,
                Nombre = "Modelo",
                Activo = true
            };
            Db.Add(new Atencion { IdAtencion = 1, IdCliente = 1, Cliente = cliente, IdVehiculo = 1, Vehiculo = new Vehiculo { IdVehiculo = 1, Dominio = "AB123CD", Color = "Gris", IdModelo = 1, Modelo = modelo }, IdUsuarioRecepcion = 2, TipoSolicitud = TipoSolicitudAtencion.Problema, MotivoConsulta = "Ruido al frenar", Estado = EstadoAtencion.Abierta, FechaApertura = FechaHoraTaller.Ahora, Version = new byte[8] });
            Db.AddRange(new Usuario { Id = 1, RolId = RolesSistema.TecnicoId, Nombre = "Técnico", Apellido = "Uno", Activo = true }, new Usuario { Id = 2, RolId = RolesSistema.AdministrativoId, Nombre = "Recepción", Apellido = "Uno", Activo = true }, new Usuario { Id = 3, RolId = RolesSistema.TecnicoId, Nombre = "Técnico", Apellido = "Dos", Activo = true });
            var categoria = new Categoria
            {
                IdCategoria = 1,
                Nombre = "Frenos",
                Activo = true
            };
            Db.AddRange(new Concepto { IdConcepto = 1, IdCategoria = 1, Categoria = categoria, Nombre = "Mano de obra", Tipo = TipoConcepto.Servicio, Activo = true }, new Concepto { IdConcepto = 2, IdCategoria = 1, Categoria = categoria, Nombre = "Repuesto", Tipo = TipoConcepto.Repuesto, Activo = true }, new Concepto { IdConcepto = 3, IdCategoria = 1, Categoria = categoria, Nombre = "Inspección inicial", Tipo = TipoConcepto.Servicio, Activo = true, DisponibleParaIngreso = true });
            Db.SaveChanges();
            Db.ChangeTracker.Clear();
            var evaluacion = new EvaluacionRepositorio(Db);
            var circuito = new CircuitoRepositorio(Db);
            var usuarios = new UsuarioRepositorio(Db);
            var unidad = new UnidadDeTrabajo(Db);
            var acceso = new AutorizacionOperativa(usuarios, Sesion);
            Evaluacion = new EvaluacionServicio(evaluacion, usuarios, unidad, Sesion);
            Presupuestos = new PresupuestoServicio(evaluacion, circuito, unidad, acceso);
            Seguimiento = new SeguimientoTrabajoServicio(evaluacion, circuito, unidad, acceso);
            Ejecucion = new EjecucionServicio(evaluacion, circuito, unidad, acceso);
            Cobros = new CobroServicio(circuito, evaluacion, unidad, acceso);
            Recepcion = new AtencionServicio(new AtencionRepositorio(Db), new ClienteRepositorio(Db), new VehiculoRepositorio(Db), usuarios, new ConceptoRepositorio(Db), Sesion);
            Entrar(2);
        }

        public void Entrar(int id) => Sesion.Iniciar(new Usuario { Id = id });
        // Simula tokens renovados entre operaciones. No es una prueba del mecanismo rowversion de SQL Server.
        public async Task VersionesAsync()
        {
            Db.ChangeTracker.Clear();
            foreach (var a in await Db.Atenciones.ToListAsync())
                a.Version = BitConverter.GetBytes(++_version);
            foreach (var p in await Db.Presupuestos.ToListAsync())
                p.Version = BitConverter.GetBytes(++_version);
            foreach (var d in await Db.Set<Comprobante>().ToListAsync())
                d.Version = BitConverter.GetBytes(++_version);
            await Db.SaveChangesAsync();
            Db.ChangeTracker.Clear();
        }

        public async Task<PresupuestoGestionDto> PropuestaAsync(bool servicioInicial = false)
        {
            var a = await Db.Atenciones.AsNoTracking().SingleAsync();
            if (servicioInicial)
                await Recepcion.ConfirmarIngresoAsync(new ConfirmarIngresoDto { IdAtencion = 1, VersionAtencion = a.Version, ClienteAutoriza = true, Servicios = [new ServicioIngresoSeleccionadoDto { IdConcepto = 3, PrecioUnitario = 30 }] });
            else
                await Evaluacion.EnviarAEvaluacionAsync(1, a.TipoSolicitud, a.Version);
            await VersionesAsync();
            Entrar(1);
            await Evaluacion.TomarAsync(1);
            await VersionesAsync();
            var d = await Evaluacion.ObtenerAsync(1);
            d.FallaDetectada = "Desgaste detectado";
            d.TrabajoRecomendado = "Reemplazo y ajuste";
            d.Items = [new ItemPropuestaDto
            {
                IdConcepto = 1,
                Descripcion = "Mano de obra acordada",
                Cantidad = 2,
                PrecioUnitario = 100
            }, new ItemPropuestaDto
            {
                IdConcepto = 2,
                Descripcion = "Repuesto acordado",
                Cantidad = 1,
                PrecioUnitario = 50
            }

            ];
            foreach (var i in d.ServiciosIniciales)
                i.Estado = EstadoEjecucionDetalle.Completado;
            await Evaluacion.GuardarAsync(d, true);
            await VersionesAsync();
            Entrar(2);
            var p = await Db.Presupuestos.AsNoTracking().SingleAsync(x => x.Tipo == TipoPresupuesto.Reparacion);
            await Evaluacion.PresentarAsync(p.IdPresupuesto, p.Version);
            await VersionesAsync();
            return await Presupuestos.ObtenerAsync(p.IdPresupuesto);
        }

        public async Task<PresupuestoGestionDto> AceptarAsync(bool parcial = false, bool inicial = false)
        {
            var d = await PropuestaAsync(inicial);
            await Presupuestos.DecidirAsync(new DecisionPresupuestoDto { IdPresupuesto = d.IdPresupuesto, Version = d.Version, ItemsAceptados = parcial ? [d.Items[0].IdDetalle] : d.Items.Select(x => x.IdDetalle).ToList(), Observaciones = parcial ? "Cliente excluye el repuesto." : "Acepta todo." });
            await VersionesAsync();
            if (parcial)
            {
                Entrar(1);
                var original = await Presupuestos.ObtenerAsync(d.IdPresupuesto);
                var id = await Seguimiento.RevisarSeleccionAsync(original.IdPresupuesto, original.Version, [new ItemPropuestaDto { IdConcepto = original.Items[0].IdConcepto, Descripcion = original.Items[0].Descripcion, Cantidad = original.Items[0].Cantidad, PrecioUnitario = original.Items[0].PrecioUnitario }], "La tarea seleccionada puede ejecutarse de forma independiente.");
                await VersionesAsync();
                Entrar(2);
                var nueva = await Presupuestos.ObtenerAsync(id);
                await Evaluacion.PresentarAsync(id, nueva.Version);
                await VersionesAsync();
                nueva = await Presupuestos.ObtenerAsync(id);
                await Presupuestos.DecidirAsync(new() { IdPresupuesto = id, Version = nueva.Version, ItemsAceptados = nueva.Items.Select(x => x.IdDetalle).ToList(), Observaciones = "Acepta alcance definitivo revisado" });
                await VersionesAsync();
            }

            return d;
        }

        public async Task EjecutarAsync()
        {
            Entrar(1);
            var d = await Ejecucion.ObtenerAsync(1);
            if (d.IdTecnico == null)
                await Ejecucion.TomarAsync(1, d.Version, "Alcance viable revisado.");
            await VersionesAsync();
            d = await Ejecucion.ObtenerAsync(1);
            foreach (var i in d.Items)
            {
                i.CantidadEjecutada = i.CantidadAutorizada;
                i.Estado = EstadoEjecucionDetalle.Completado;
            }

            await Ejecucion.GuardarAsync(d, true);
            await VersionesAsync();
            Entrar(2);
        }

        public async Task<ComprobanteDto> LiquidarAsync()
        {
            Entrar(2);
            var cuenta = await Cobros.ObtenerAsync(1);
            await Cobros.LiquidarAsync(1, cuenta.VersionAtencion, "Liquidación según trabajo registrado.");
            await VersionesAsync();
            return (await Cobros.ObtenerAsync(1)).Comprobante!;
        }

        public static RegistrarPagoDto Pago(ComprobanteDto d, decimal efectivo, decimal transferencia = 0) => new()
        {
            IdComprobante = d.IdComprobante,
            Version = d.Version,
            Medios = transferencia > 0 ? [new PagoMedioDto
            {
                IdMedioPago = 1,
                Importe = efectivo
            }, new PagoMedioDto
            {
                IdMedioPago = 2,
                Importe = transferencia,
                Referencia = "TRX-123"
            }

            ] : [new PagoMedioDto
            {
                IdMedioPago = 1,
                Importe = efectivo
            }

            ]
        };
        public void Dispose() => Db.Dispose();
    }

    [Fact]
    public async Task SeleccionParcial_NoAutorizaHastaRevisionTecnicaYNuevaAceptacion()
    {
        using var c = new Caso();
        var p = await c.PropuestaAsync();
        await c.Presupuestos.DecidirAsync(new() { IdPresupuesto = p.IdPresupuesto, Version = p.Version, ItemsAceptados = [p.Items[0].IdDetalle], Observaciones = "Solo mano de obra" });
        await c.VersionesAsync();
        Assert.Equal(EstadoPresupuesto.PendienteRevisionTecnica, (await c.Presupuestos.ObtenerAsync(p.IdPresupuesto)).Estado);
        c.Entrar(1);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Ejecucion.ObtenerAsync(1));
        var original = await c.Presupuestos.ObtenerAsync(p.IdPresupuesto);
        var nueva = await c.Seguimiento.RevisarSeleccionAsync(p.IdPresupuesto, original.Version, [new() { IdConcepto = 1, Descripcion = "Trabajo viable por separado", Cantidad = 2, PrecioUnitario = 100 }], "Se verificaron dependencias; alcance independiente.");
        await c.VersionesAsync();
        await Assert.ThrowsAsync<ValidacionException>(() => c.Ejecucion.ObtenerAsync(1));
        c.Entrar(2);
        var propuesta = await c.Presupuestos.ObtenerAsync(nueva);
        await c.Evaluacion.PresentarAsync(nueva, propuesta.Version);
        await c.VersionesAsync();
        propuesta = await c.Presupuestos.ObtenerAsync(nueva);
        await c.Presupuestos.DecidirAsync(new() { IdPresupuesto = nueva, Version = propuesta.Version, ItemsAceptados = propuesta.Items.Select(i => i.IdDetalle).ToList() });
        await c.VersionesAsync();
        c.Entrar(1);
        Assert.Single((await c.Ejecucion.ObtenerAsync(1)).Items);
        Assert.Equal(2, await c.Db.PresupuestoDetalles.CountAsync(d => d.IdPresupuesto == p.IdPresupuesto));
        Assert.False((await c.Presupuestos.ObtenerAsync(p.IdPresupuesto)).Vigente);
    }

    [Fact]
    public async Task Decision_NoAdmiteItemsAjenosNiRechazoSinMotivo()
    {
        using var c = new Caso();
        var p = await c.PropuestaAsync();
        await Assert.ThrowsAsync<ValidacionException>(() => c.Presupuestos.DecidirAsync(new DecisionPresupuestoDto { IdPresupuesto = p.IdPresupuesto, Version = p.Version, ItemsAceptados = [999] }));
        await Assert.ThrowsAsync<ValidacionException>(() => c.Presupuestos.DecidirAsync(new DecisionPresupuestoDto { IdPresupuesto = p.IdPresupuesto, Version = p.Version }));
        Assert.Equal(EstadoAtencion.PendienteDecision, (await c.Db.Atenciones.SingleAsync()).Estado);
    }

    [Theory]
    [InlineData(false, 0)]
    [InlineData(true, 30)]
    public async Task Rechazo_LiquidaSoloServiciosInicialesRealizados(bool inicial, int total)
    {
        using var c = new Caso();
        var p = await c.PropuestaAsync(inicial);
        await c.Presupuestos.DecidirAsync(new DecisionPresupuestoDto { IdPresupuesto = p.IdPresupuesto, Version = p.Version, Observaciones = "Cliente rechaza la reparación." });
        await c.VersionesAsync();
        var d = await c.LiquidarAsync();
        Assert.Equal(total, d.Total);
        Assert.Equal(total == 0 ? 0 : 1, d.Items.Count);
        if (total == 0)
        {
            var cuenta = await c.Cobros.ObtenerAsync(1);
            await c.Cobros.EntregarAsync(1, cuenta.VersionAtencion, "Retira sin reparar y sin cargo.");
            Assert.Equal(EstadoAtencion.Cerrada, (await c.Db.Atenciones.SingleAsync()).Estado);
        }
    }

    [Fact]
    public async Task Revision_ConservaDecisionAnteriorYPermiteNuevaPropuestaTecnica()
    {
        using var c = new Caso();
        var p = await c.AceptarAsync();
        p = await c.Presupuestos.ObtenerAsync(p.IdPresupuesto);
        var nuevo = await c.Presupuestos.SolicitarRevisionAsync(p.IdPresupuesto, p.Version, "Solicita otro alcance.");
        await c.VersionesAsync();
        c.Entrar(1);
        await c.Evaluacion.TomarAsync(1);
        await c.VersionesAsync();
        var avance = await c.Evaluacion.ObtenerAsync(1);
        Assert.True(avance.InformeHistorico);
        Assert.Equal("Solicita otro alcance.", avance.MotivoRevision);
        avance.Items.RemoveAt(1);
        await c.Evaluacion.GuardarAsync(avance, true);
        var documentos = await c.Db.Presupuestos.AsNoTracking().ToListAsync();
        var anterior = documentos.Single(x => x.IdPresupuesto == p.IdPresupuesto);
        Assert.False(anterior.Vigente);
        Assert.Equal(EstadoPresupuesto.Aceptado, anterior.Estado);
        var actual = documentos.Single(x => x.IdPresupuesto == nuevo);
        Assert.True(actual.Vigente);
        Assert.Equal(p.IdPresupuesto, actual.IdPresupuestoAnterior);
        Assert.Equal(EstadoPresupuesto.PendientePresentacion, actual.Estado);
    }

    [Fact]
    public async Task NoPermiteEjecutarUnItemExcluido()
    {
        using var c = new Caso();
        var p = await c.AceptarAsync(parcial: true);
        c.Entrar(1);
        var e = await c.Ejecucion.ObtenerAsync(1);
        await c.Ejecucion.TomarAsync(1, e.Version, "Alcance restante viable.");
        await c.VersionesAsync();
        e = await c.Ejecucion.ObtenerAsync(1);
        e.Items[0].IdDetalle = p.Items[1].IdDetalle;
        await Assert.ThrowsAsync<ValidacionException>(() => c.Ejecucion.GuardarAsync(e, false));
        Assert.All(await c.Db.PresupuestoDetalles.ToListAsync(), d => Assert.Equal(0, d.CantidadEjecutada));
    }

    [Fact]
    public async Task Interrupcion_LiquidaCantidadRealYNoCantidadPresupuestada()
    {
        using var c = new Caso();
        await c.AceptarAsync();
        c.Entrar(1);
        var e = await c.Ejecucion.ObtenerAsync(1);
        await c.Ejecucion.TomarAsync(1, e.Version, "");
        await c.VersionesAsync();
        e = await c.Ejecucion.ObtenerAsync(1);
        e.Items[0].CantidadEjecutada = 1;
        e.Items[0].Estado = EstadoEjecucionDetalle.Parcial;
        e.Items[0].Observaciones = "Solo se realizó la mitad.";
        e.Items[1].Estado = EstadoEjecucionDetalle.Omitido;
        e.Items[1].Observaciones = "Cliente desiste antes de instalar.";
        e.Observaciones = "Se interrumpe y se entrega sin reparar por completo.";
        await c.Ejecucion.GuardarAsync(e, true, true);
        await c.VersionesAsync();
        c.Entrar(2);
        var d = await c.LiquidarAsync();
        Assert.Equal(100, d.Total);
        Assert.Single(d.Items);
        Assert.Equal(1, d.Items[0].Cantidad);
    }

    [Fact]
    public async Task Ejecucion_ImpideDisminuirLoRealizadoORevisarUnaPropuestaIniciada()
    {
        using var c = new Caso();
        var p = await c.AceptarAsync();
        c.Entrar(1);
        var e = await c.Ejecucion.ObtenerAsync(1);
        await c.Ejecucion.TomarAsync(1, e.Version, "");
        await c.VersionesAsync();
        e = await c.Ejecucion.ObtenerAsync(1);
        e.Items[0].CantidadEjecutada = 1;
        e.Items[0].Estado = EstadoEjecucionDetalle.Parcial;
        e.Items[0].Observaciones = "Avance";
        await c.Ejecucion.GuardarAsync(e, false);
        await c.VersionesAsync();
        e = await c.Ejecucion.ObtenerAsync(1);
        e.Items[0].CantidadEjecutada = 0;
        e.Items[0].Estado = EstadoEjecucionDetalle.NoIniciado;
        await Assert.ThrowsAsync<ValidacionException>(() => c.Ejecucion.GuardarAsync(e, false));
        await c.Ejecucion.LiberarAsync(1, e.Version);
        await c.VersionesAsync();
        c.Entrar(2);
        p = await c.Presupuestos.ObtenerAsync(p.IdPresupuesto);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Presupuestos.SolicitarRevisionAsync(p.IdPresupuesto, p.Version, "Cambio tardío"));
    }

    [Fact]
    public async Task CobrosParcialesMixtos_EntregaUnicamenteConSaldoCero()
    {
        using var c = new Caso();
        await c.AceptarAsync();
        await c.EjecutarAsync();
        var d = await c.LiquidarAsync();
        Assert.Equal(250, d.Total);
        var intento = Caso.Pago(d, 30, 20);
        await c.Cobros.RegistrarPagoAsync(intento);
        await c.VersionesAsync();
        var cuenta = await c.Cobros.ObtenerAsync(1);
        Assert.Equal(200, cuenta.Comprobante!.Saldo);
        Assert.Single(cuenta.Comprobante.Pagos);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Cobros.EntregarAsync(1, cuenta.VersionAtencion, "Retira."));
        await c.Cobros.RegistrarPagoAsync(Caso.Pago(cuenta.Comprobante, 200));
        await c.VersionesAsync();
        cuenta = await c.Cobros.ObtenerAsync(1);
        Assert.Equal(0, cuenta.Comprobante!.Saldo);
        Assert.Equal(2, cuenta.Comprobante.Pagos.Count);
        Assert.Equal(3, await c.Db.Set<PagoMedio>().CountAsync());
        await c.Cobros.EntregarAsync(1, cuenta.VersionAtencion, "Retira el cliente, reparado.");
        Assert.Equal(EstadoAtencion.Cerrada, (await c.Db.Atenciones.SingleAsync()).Estado);
    }

    [Fact]
    public async Task Sobrepago_ReferenciaAusenteOMedioInactivo_NoRegistraDinero()
    {
        using var c = new Caso();
        await c.AceptarAsync();
        await c.EjecutarAsync();
        var d = await c.LiquidarAsync();
        await Assert.ThrowsAsync<ValidacionException>(() => c.Cobros.RegistrarPagoAsync(Caso.Pago(d, 251)));
        var transferencia = Caso.Pago(d, 10, 10);
        transferencia.Medios[1].Referencia = "";
        await Assert.ThrowsAsync<ValidacionException>(() => c.Cobros.RegistrarPagoAsync(transferencia));
        (await c.Db.Set<MedioPago>().FindAsync(1))!.Activo = false;
        await c.Db.SaveChangesAsync();
        c.Db.ChangeTracker.Clear();
        await Assert.ThrowsAsync<ValidacionException>(() => c.Cobros.RegistrarPagoAsync(Caso.Pago(d, 10)));
        Assert.Empty(await c.Db.Set<Pago>().ToListAsync());
        Assert.Equal(0, (await c.Cobros.ObtenerAsync(1)).Comprobante!.Pagado);
    }

    [Fact]
    public async Task ReintentoDeCobro_EsIdempotenteYNoAdmiteCambiarSuContenido()
    {
        using var c = new Caso();
        await c.AceptarAsync();
        await c.EjecutarAsync();
        var d = await c.LiquidarAsync();
        var intento = Caso.Pago(d, 70);
        var id = await c.Cobros.RegistrarPagoAsync(intento);
        await c.VersionesAsync();
        Assert.Equal(id, await c.Cobros.RegistrarPagoAsync(intento));
        Assert.Single(await c.Db.Set<Pago>().ToListAsync());
        intento.Medios[0].Importe = 80;
        await Assert.ThrowsAsync<ValidacionException>(() => c.Cobros.RegistrarPagoAsync(intento));
        Assert.Equal(70, (await c.Cobros.ObtenerAsync(1)).Comprobante!.Pagado);
    }

    [Fact]
    public async Task Anulacion_ConservaReciboYReponeSaldo_NoSePermiteTrasEntrega()
    {
        using var c = new Caso();
        await c.AceptarAsync();
        await c.EjecutarAsync();
        var d = await c.LiquidarAsync();
        var id = await c.Cobros.RegistrarPagoAsync(Caso.Pago(d, 250));
        await c.VersionesAsync();
        d = (await c.Cobros.ObtenerAsync(1)).Comprobante!;
        await c.Cobros.AnularPagoAsync(d.IdComprobante, id, d.Version, "Medio registrado incorrectamente.");
        await c.VersionesAsync();
        d = (await c.Cobros.ObtenerAsync(1)).Comprobante!;
        Assert.Equal(250, d.Saldo);
        Assert.True(d.Pagos.Single().Anulado);
        var otro = await c.Cobros.RegistrarPagoAsync(Caso.Pago(d, 250));
        await c.VersionesAsync();
        var cuenta = await c.Cobros.ObtenerAsync(1);
        await c.Cobros.EntregarAsync(1, cuenta.VersionAtencion, "Retira conforme.");
        await c.VersionesAsync();
        d = (await c.Cobros.ObtenerAsync(1)).Comprobante!;
        await Assert.ThrowsAsync<ValidacionException>(() => c.Cobros.AnularPagoAsync(d.IdComprobante, otro, d.Version, "Después de entregar"));
        Assert.Equal(2, await c.Db.Set<Pago>().CountAsync());
    }

    [Fact]
    public async Task DocumentoCongelado_NoCambiaAlModificarCatalogoNiDuplicaAlReemitir()
    {
        using var c = new Caso();
        await c.AceptarAsync();
        await c.EjecutarAsync();
        var d = await c.LiquidarAsync();
        var concepto = await c.Db.Conceptos.FindAsync(1);
        concepto!.Nombre = "Nombre nuevo";
        concepto.PrecioReferencia = 9000;
        await c.Db.SaveChangesAsync();
        c.Db.ChangeTracker.Clear();
        var cuenta = await c.Cobros.ObtenerAsync(1);
        Assert.Equal(250, cuenta.Comprobante!.Total);
        Assert.Contains(cuenta.Comprobante.Items, x => x.Descripcion == "Mano de obra acordada" && x.PrecioUnitario == 100);
        Assert.Equal(d.IdComprobante, await c.Cobros.LiquidarAsync(1, cuenta.VersionAtencion, "Otra emisión"));
        Assert.Single(await c.Db.Set<Comprobante>().ToListAsync());
    }

    [Fact]
    public async Task RolesYVersiones_ProtegenDecisionEjecucionYCobros()
    {
        using var c = new Caso();
        var p = await c.PropuestaAsync();
        c.Entrar(1);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Presupuestos.DecidirAsync(new DecisionPresupuestoDto { IdPresupuesto = p.IdPresupuesto, Version = p.Version, ItemsAceptados = p.Items.Select(x => x.IdDetalle).ToList() }));
        await Assert.ThrowsAsync<ValidacionException>(() => c.Cobros.ListarAsync());
        c.Entrar(2);
        var vieja = p.Version.ToArray();
        vieja[0] ^= 0xff;
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => c.Presupuestos.DecidirAsync(new DecisionPresupuestoDto { IdPresupuesto = p.IdPresupuesto, Version = vieja, ItemsAceptados = p.Items.Select(x => x.IdDetalle).ToList() }));
        var a = await c.Db.Atenciones.AsNoTracking().SingleAsync();
        await Assert.ThrowsAsync<ValidacionException>(() => c.Cobros.LiquidarAsync(1, a.Version, "Antes de ejecutar"));
    }

    [Fact]
    public async Task RevisionDescartada_NoCobraNiBloqueaPorLaAceptacionHistorica()
    {
        using var c = new Caso();
        var p = await c.AceptarAsync();
        p = await c.Presupuestos.ObtenerAsync(p.IdPresupuesto);
        await c.Presupuestos.SolicitarRevisionAsync(p.IdPresupuesto, p.Version, "Cliente solicita revisión.");
        await c.VersionesAsync();
        c.Entrar(1);
        await c.Evaluacion.TomarAsync(1);
        await c.VersionesAsync();
        var avance = await c.Evaluacion.ObtenerAsync(1);
        avance.Items.Clear();
        avance.SoloEvaluacion = true;
        avance.Observaciones = "Cliente desiste sin realizar los trabajos propuestos.";
        await c.Evaluacion.GuardarAsync(avance, true);
        await c.VersionesAsync();
        var liquidacion = await c.LiquidarAsync();
        Assert.Equal(0, liquidacion.Total);
        Assert.Empty(liquidacion.Items);
    }

    [Fact]
    public async Task Recibo_ConservaMediosYNombreRegistradosAunqueCambieElCatalogoOUsuario()
    {
        using var c = new Caso();
        await c.AceptarAsync();
        await c.EjecutarAsync();
        var d = await c.LiquidarAsync();
        await c.Cobros.RegistrarPagoAsync(Caso.Pago(d, 10, 20));
        await c.VersionesAsync();
        var antes = (await c.Cobros.ObtenerAsync(1)).Comprobante!.Pagos.Single();
        (await c.Db.MediosPago.FindAsync(2))!.Nombre = "Medio renombrado";
        (await c.Db.Usuarios.FindAsync(2))!.Nombre = "Nombre actualizado";
        await c.Db.SaveChangesAsync();
        c.Db.ChangeTracker.Clear();
        var despues = (await c.Cobros.ObtenerAsync(1)).Comprobante!.Pagos.Single();
        Assert.Equal(antes.Usuario, despues.Usuario);
        Assert.Equal(antes.Medios, despues.Medios);
        Assert.Equal(220, despues.SaldoPosterior);
    }

    [Fact]
    public async Task CobroConVersionObsoleta_NoAgregaOtroPago()
    {
        using var c = new Caso();
        await c.AceptarAsync();
        await c.EjecutarAsync();
        var d = await c.LiquidarAsync();
        await c.Cobros.RegistrarPagoAsync(Caso.Pago(d, 20));
        await c.VersionesAsync();
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => c.Cobros.RegistrarPagoAsync(Caso.Pago(d, 30)));
        Assert.Single(await c.Db.Pagos.ToListAsync());
        Assert.Equal(230, (await c.Cobros.ObtenerAsync(1)).Comprobante!.Saldo);
    }

    [Fact]
    public async Task OtroTecnico_NoPuedeRegistrarLaEjecucionReservada()
    {
        using var c = new Caso();
        await c.AceptarAsync();
        c.Entrar(1);
        var e = await c.Ejecucion.ObtenerAsync(1);
        await c.Ejecucion.TomarAsync(1, e.Version, "");
        await c.VersionesAsync();
        e = await c.Ejecucion.ObtenerAsync(1);
        c.Entrar(3);
        e.Items[0].CantidadEjecutada = e.Items[0].CantidadAutorizada;
        e.Items[0].Estado = EstadoEjecucionDetalle.Completado;
        await Assert.ThrowsAsync<ValidacionException>(() => c.Ejecucion.GuardarAsync(e, false));
        Assert.All(await c.Db.PresupuestoDetalles.ToListAsync(), x => Assert.Equal(0, x.CantidadEjecutada));
    }

    [Fact]
    public async Task MotivoLargoDeRevision_CabeEnLasColumnasConfiguradasSinTruncarse()
    {
        using var c = new Caso();
        var p = await c.AceptarAsync();
        p = await c.Presupuestos.ObtenerAsync(p.IdPresupuesto);
        var motivo = new string ('x', 1000);
        var id = await c.Presupuestos.SolicitarRevisionAsync(p.IdPresupuesto, p.Version, motivo);
        var nuevo = await c.Db.Presupuestos.AsNoTracking().SingleAsync(x => x.IdPresupuesto == id);
        Assert.Equal(motivo, nuevo.MotivoRevision);
        var maximo = c.Db.Model.FindEntityType(typeof(Presupuesto))!.FindProperty(nameof(Presupuesto.Observaciones))!.GetMaxLength()!.Value;
        Assert.True(nuevo.Observaciones!.Length <= maximo);
        var limiteHistorial = c.Db.Model.FindEntityType(typeof(HistorialAtencion))!.FindProperty(nameof(HistorialAtencion.Observaciones))!.GetMaxLength()!.Value;
        Assert.All(await c.Db.HistorialAtenciones.AsNoTracking().ToListAsync(), x => Assert.True((x.Observaciones?.Length ?? 0) <= limiteHistorial));
    }

    [Fact]
    public async Task MotivoLargoDeAnulacion_ConservaTextoCompletoEnPagoEHistorial()
    {
        using var c = new Caso();
        await c.AceptarAsync();
        await c.EjecutarAsync();
        var d = await c.LiquidarAsync();
        var id = await c.Cobros.RegistrarPagoAsync(Caso.Pago(d, 50));
        await c.VersionesAsync();
        d = (await c.Cobros.ObtenerAsync(1)).Comprobante!;
        var motivo = new string ('y', 1000);
        await c.Cobros.AnularPagoAsync(d.IdComprobante, id, d.Version, motivo);
        Assert.Equal(motivo, (await c.Db.Pagos.AsNoTracking().SingleAsync()).MotivoAnulacion);
        var evento = await c.Db.HistorialAtenciones.AsNoTracking().SingleAsync(h => h.Accion == "PagoAnulado");
        Assert.EndsWith(motivo, evento.Observaciones);
        var limite = c.Db.Model.FindEntityType(typeof(HistorialAtencion))!.FindProperty(nameof(HistorialAtencion.Observaciones))!.GetMaxLength()!.Value;
        Assert.True(evento.Observaciones!.Length <= limite);
    }

    [Fact]
    public async Task CancelarApertura_ConservaMotivoYCierreSinIngresarAlCircuitoTecnico()
    {
        using var c = new Caso();
        var a = await c.Db.Atenciones.AsNoTracking().SingleAsync();
        await c.Evaluacion.CancelarAperturaAsync(a.IdAtencion, a.Version, "El cliente desiste antes de ingresar");
        var actual = await c.Db.Atenciones.AsNoTracking().SingleAsync();
        Assert.Equal(EstadoAtencion.Cancelada, actual.Estado);
        Assert.NotNull(actual.FechaCierre);
        Assert.Equal("AperturaCancelada", (await c.Db.HistorialAtenciones.AsNoTracking().SingleAsync()).Accion);
    }

    [Fact]
    public async Task CancelarApertura_NoPermiteTecnicoNiVersionObsoletaNiServiciosAutorizados()
    {
        using var c = new Caso();
        var a = await c.Db.Atenciones.AsNoTracking().SingleAsync();
        c.Entrar(1);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Evaluacion.CancelarAperturaAsync(a.IdAtencion, a.Version, "Motivo"));
        c.Entrar(2);
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => c.Evaluacion.CancelarAperturaAsync(a.IdAtencion, Enumerable.Repeat((byte)255, 8).ToArray(), "Motivo"));
        await c.PropuestaAsync(servicioInicial: true);
        a = await c.Db.Atenciones.AsNoTracking().SingleAsync();
        c.Entrar(2);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Evaluacion.CancelarAperturaAsync(a.IdAtencion, a.Version, "Motivo"));
        Assert.NotEqual(EstadoAtencion.Cancelada, (await c.Db.Atenciones.AsNoTracking().SingleAsync()).Estado);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Adicional_ConservaOriginalYCobraSoloSiAceptado(bool aceptar)
    {
        using var c = new Caso();
        await c.AceptarAsync();
        c.Entrar(1);
        var seguimiento = await c.Seguimiento.ObtenerAsync(1);
        await c.Seguimiento.RegistrarHallazgoAsync(new() { IdAtencion = 1, Version = seguimiento.Version, Descripcion = "Pérdida adicional encontrada", Recomendacion = "Cambiar sello independiente", ImpideContinuar = false });
        await c.VersionesAsync();
        seguimiento = await c.Seguimiento.ObtenerAsync(1);
        int adicional = await c.Seguimiento.CrearAdicionalAsync(new() { IdAtencion = 1, Version = seguimiento.Version, IdHallazgo = seguimiento.Hallazgos.Single().Id, Motivo = "Solución del nuevo hallazgo", Items = [new() { IdConcepto = 1, Descripcion = "Cambiar sello", Cantidad = 1, PrecioUnitario = 25 }] });
        await c.VersionesAsync();
        Assert.Equal(2, (await c.Ejecucion.ObtenerAsync(1)).Items.Count);
        c.Entrar(2);
        var p = await c.Presupuestos.ObtenerAsync(adicional);
        await c.Evaluacion.PresentarAsync(adicional, p.Version);
        await c.VersionesAsync();
        p = await c.Presupuestos.ObtenerAsync(adicional);
        await c.Presupuestos.DecidirAsync(new() { IdPresupuesto = adicional, Version = p.Version, ItemsAceptados = aceptar ? p.Items.Select(i => i.IdDetalle).ToList() : [], Observaciones = aceptar ? "Acepta adicional" : "Rechaza adicional, continúa lo original" });
        await c.VersionesAsync();
        c.Entrar(1);
        Assert.Equal(aceptar ? 3 : 2, (await c.Ejecucion.ObtenerAsync(1)).Items.Count);
        await c.EjecutarAsync();
        var cuenta = await c.LiquidarAsync();
        Assert.Equal(aceptar ? 275 : 250, cuenta.Total);
    }

    [Fact]
    public async Task HallazgoBloqueante_PausaSoloSuTareaYNoPermiteFinalizarSinResolver()
    {
        using var c = new Caso();
        await c.AceptarAsync();
        c.Entrar(1);
        var e = await c.Ejecucion.ObtenerAsync(1);
        await c.Ejecucion.TomarAsync(1, e.Version, "");
        await c.VersionesAsync();
        var d = await c.Seguimiento.ObtenerAsync(1);
        var tarea = d.Items[0].IdDetalle;
        await c.Seguimiento.RegistrarHallazgoAsync(new() { IdAtencion = 1, Version = d.Version, IdDetalle = tarea, Descripcion = "No puede completarse esta tarea", Recomendacion = "Esperar repuesto antes de retomar", ImpideContinuar = true });
        await c.VersionesAsync();
        e = await c.Ejecucion.ObtenerAsync(1);
        var otra = e.Items.Single(i => i.IdDetalle != tarea);
        otra.Estado = EstadoEjecucionDetalle.Completado;
        otra.CantidadEjecutada = otra.CantidadAutorizada;
        await c.Ejecucion.GuardarAsync(e, false);
        await c.VersionesAsync();
        e = await c.Ejecucion.ObtenerAsync(1);
        var bloqueada = e.Items.Single(i => i.IdDetalle == tarea);
        bloqueada.Estado = EstadoEjecucionDetalle.Completado;
        bloqueada.CantidadEjecutada = bloqueada.CantidadAutorizada;
        await Assert.ThrowsAsync<ValidacionException>(() => c.Ejecucion.GuardarAsync(e, false));
        d = await c.Seguimiento.ObtenerAsync(1);
        await c.Seguimiento.ResolverPausaAsync(1, d.Version, d.Pausas.Single().Id, "Repuesto adecuado recibido, se puede continuar");
        await c.VersionesAsync();
        e = await c.Ejecucion.ObtenerAsync(1);
        foreach (var item in e.Items)
        {
            item.Estado = EstadoEjecucionDetalle.Completado;
            item.CantidadEjecutada = item.CantidadAutorizada;
        }

        await c.Ejecucion.GuardarAsync(e, true);
        Assert.Equal(EstadoAtencion.TrabajoFinalizado, (await c.Db.Atenciones.SingleAsync()).Estado);
    }

    [Fact]
    public async Task MaterialCliente_ExigeRecepcionConformeYNoCobraSuVenta()
    {
        using var c = new Caso();
        await c.AceptarAsync();
        c.Entrar(1);
        var d = await c.Seguimiento.ObtenerAsync(1);
        int id = await c.Seguimiento.CrearAdicionalAsync(new() { IdAtencion = 1, Version = d.Version, Motivo = "El cliente consigue un repuesto adicional", Items = [new() { IdConcepto = 2, Descripcion = "Repuesto aportado por cliente", Cantidad = 1, PrecioUnitario = 0, OrigenMaterial = OrigenMaterial.Cliente }] });
        await c.VersionesAsync();
        c.Entrar(2);
        var p = await c.Presupuestos.ObtenerAsync(id);
        await c.Evaluacion.PresentarAsync(id, p.Version);
        await c.VersionesAsync();
        p = await c.Presupuestos.ObtenerAsync(id);
        await c.Presupuestos.DecidirAsync(new() { IdPresupuesto = id, Version = p.Version, ItemsAceptados = p.Items.Select(i => i.IdDetalle).ToList() });
        await c.VersionesAsync();
        c.Entrar(1);
        var e = await c.Ejecucion.ObtenerAsync(1);
        await c.Ejecucion.TomarAsync(1, e.Version, "");
        await c.VersionesAsync();
        e = await c.Ejecucion.ObtenerAsync(1);
        foreach (var item in e.Items)
        {
            item.Estado = EstadoEjecucionDetalle.Completado;
            item.CantidadEjecutada = item.CantidadAutorizada;
        }

        await Assert.ThrowsAsync<ValidacionException>(() => c.Ejecucion.GuardarAsync(e, true));
        d = await c.Seguimiento.ObtenerAsync(1);
        var material = d.Items.Single(i => i.Origen == OrigenMaterial.Cliente);
        await c.Seguimiento.RecibirMaterialAsync(1, d.Version, material.IdDetalle, 1, false, "La pieza recibida aún no se considera apta");
        await c.VersionesAsync();
        e = await c.Ejecucion.ObtenerAsync(1);
        foreach (var item in e.Items)
        {
            item.Estado = EstadoEjecucionDetalle.Completado;
            item.CantidadEjecutada = item.CantidadAutorizada;
        }

        await Assert.ThrowsAsync<ValidacionException>(() => c.Ejecucion.GuardarAsync(e, true));
        d = await c.Seguimiento.ObtenerAsync(1);
        await c.Seguimiento.RecibirMaterialAsync(1, d.Version, material.IdDetalle, 1, true, "Pieza adecuada verificada para el vehículo");
        await c.VersionesAsync();
        await c.EjecutarAsync();
        var cuenta = await c.LiquidarAsync();
        Assert.Equal(250, cuenta.Total);
        Assert.DoesNotContain(cuenta.Items, i => i.Descripcion.Contains("aportado"));
    }

    [Fact]
    public async Task PendientesDeAdicional_NoPermitenFinalizarNiLiquidar()
    {
        using var c = new Caso();
        await c.AceptarAsync();
        c.Entrar(1);
        var d = await c.Seguimiento.ObtenerAsync(1);
        await c.Seguimiento.CrearAdicionalAsync(new() { IdAtencion = 1, Version = d.Version, Motivo = "Propuesta aún no comunicada", Items = [new() { IdConcepto = 1, Descripcion = "Trabajo adicional", Cantidad = 1, PrecioUnitario = 1 }] });
        await c.VersionesAsync();
        var e = await c.Ejecucion.ObtenerAsync(1);
        await c.Ejecucion.TomarAsync(1, e.Version, "");
        await c.VersionesAsync();
        e = await c.Ejecucion.ObtenerAsync(1);
        foreach (var i in e.Items)
        {
            i.CantidadEjecutada = i.CantidadAutorizada;
            i.Estado = EstadoEjecucionDetalle.Completado;
        }

        await c.Ejecucion.GuardarAsync(e, false);
        await c.VersionesAsync();
        e = await c.Ejecucion.ObtenerAsync(1);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Ejecucion.GuardarAsync(e, true));
    }

    [Fact]
    public async Task Seguimiento_RevalidaRolReservaVersionYMaterialConPrecio()
    {
        using var c = new Caso();
        await c.AceptarAsync();
        var d = await c.Seguimiento.ObtenerAsync(1);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Seguimiento.RegistrarHallazgoAsync(new() { IdAtencion = 1, Version = d.Version, Descripcion = "No permitido", Recomendacion = "Sin permiso técnico" }));
        c.Entrar(1);
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => c.Seguimiento.PausarAsync(1, [], null, "Espera"));
        await Assert.ThrowsAsync<ValidacionException>(() => c.Seguimiento.CrearAdicionalAsync(new() { IdAtencion = 1, Version = d.Version, Motivo = "Cliente aporta", Items = [new() { IdConcepto = 2, Descripcion = "Material cliente", Cantidad = 1, PrecioUnitario = 15, OrigenMaterial = OrigenMaterial.Cliente }] }));
        var e = await c.Ejecucion.ObtenerAsync(1);
        await c.Ejecucion.TomarAsync(1, e.Version, "");
        await c.VersionesAsync();
        c.Entrar(3);
        d = await c.Seguimiento.ObtenerAsync(1);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Seguimiento.RegistrarHallazgoAsync(new() { IdAtencion = 1, Version = d.Version, Descripcion = "No es mi reserva", Recomendacion = "Respetar técnico actual" }));
    }

    [Fact]
    public async Task AmpliacionInspeccion_RequiereAutorizacionAntesDeRegistrarEjecucion()
    {
        using var c = new Caso();
        var a = await c.Db.Atenciones.AsNoTracking().SingleAsync();
        await c.Evaluacion.EnviarAEvaluacionAsync(1, a.TipoSolicitud, a.Version);
        await c.VersionesAsync();
        c.Entrar(1);
        await c.Evaluacion.TomarAsync(1);
        await c.VersionesAsync();
        var seg = await c.Seguimiento.ObtenerAsync(1);
        int id = await c.Seguimiento.CrearAdicionalAsync(new() { IdAtencion = 1, Version = seg.Version, Motivo = "Se necesita desarme adicional para inspeccionar", Items = [new() { IdConcepto = 3, Descripcion = "Desarme autorizado por separado", Cantidad = 1, PrecioUnitario = 30 }] });
        await c.VersionesAsync();
        Assert.Empty((await c.Evaluacion.ObtenerAsync(1)).ServiciosIniciales);
        c.Entrar(2);
        var p = await c.Presupuestos.ObtenerAsync(id);
        await c.Evaluacion.PresentarAsync(id, p.Version);
        await c.VersionesAsync();
        p = await c.Presupuestos.ObtenerAsync(id);
        await c.Presupuestos.DecidirAsync(new() { IdPresupuesto = id, Version = p.Version, ItemsAceptados = p.Items.Select(i => i.IdDetalle).ToList() });
        await c.VersionesAsync();
        c.Entrar(1);
        var d = await c.Evaluacion.ObtenerAsync(1);
        Assert.Single(d.ServiciosIniciales);
        d.ServiciosIniciales[0].Estado = EstadoEjecucionDetalle.Completado;
        d.FallaDetectada = "Falla determinada tras desarme";
        d.TrabajoRecomendado = "Reparar según propuesta";
        d.Items = [new()
        {
            IdConcepto = 1,
            Descripcion = "Solución propuesta",
            Cantidad = 1,
            PrecioUnitario = 100
        }

        ];
        await c.Evaluacion.GuardarAsync(d, true);
        await c.VersionesAsync();
        Assert.Equal(1, (await c.Db.PresupuestoDetalles.SingleAsync(x => x.IdPresupuesto == id)).CantidadEjecutada);
        c.Entrar(2);
        var principal = await c.Db.Presupuestos.AsNoTracking().SingleAsync(x => !x.EsAdicional);
        await c.Evaluacion.PresentarAsync(principal.IdPresupuesto, principal.Version);
        await c.VersionesAsync();
        p = await c.Presupuestos.ObtenerAsync(principal.IdPresupuesto);
        await c.Presupuestos.DecidirAsync(new() { IdPresupuesto = p.IdPresupuesto, Version = p.Version, Observaciones = "No acepta reparación, retira" });
        await c.VersionesAsync();
        Assert.Equal(30, (await c.LiquidarAsync()).Total);
    }

    [Fact]
    public async Task RevisionDeAdicional_ConservaOriginalSinAutorizarNuevosTrabajos()
    {
        using var c = new Caso();
        await c.AceptarAsync();
        c.Entrar(1);
        var d = await c.Seguimiento.ObtenerAsync(1);
        int adicional = await c.Seguimiento.CrearAdicionalAsync(new() { IdAtencion = 1, Version = d.Version, Motivo = "Se propone tarea adicional", Items = [new() { IdConcepto = 1, Descripcion = "Nueva tarea", Cantidad = 1, PrecioUnitario = 20 }] });
        await c.VersionesAsync();
        c.Entrar(2);
        var p = await c.Presupuestos.ObtenerAsync(adicional);
        int revision = await c.Presupuestos.SolicitarRevisionAsync(adicional, p.Version, "Solicita otra alternativa y precio");
        await c.VersionesAsync();
        Assert.False((await c.Presupuestos.ObtenerAsync(adicional)).Vigente);
        Assert.Equal(EstadoPresupuesto.PendienteRevisionTecnica, (await c.Presupuestos.ObtenerAsync(revision)).Estado);
        c.Entrar(1);
        Assert.Equal(2, (await c.Ejecucion.ObtenerAsync(1)).Items.Count);
        p = await c.Presupuestos.ObtenerAsync(revision);
        int revisado = await c.Seguimiento.RevisarSeleccionAsync(revision, p.Version, [new() { IdConcepto = 1, Descripcion = "Alternativa revisada", Cantidad = 1, PrecioUnitario = 15 }], "La alternativa cumple el alcance solicitado");
        await c.VersionesAsync();
        Assert.Equal(EstadoPresupuesto.PendientePresentacion, (await c.Presupuestos.ObtenerAsync(revisado)).Estado);
        Assert.Equal(2, (await c.Ejecucion.ObtenerAsync(1)).Items.Count);
    }
}
