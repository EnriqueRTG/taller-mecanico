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
    [Fact]
    public async Task BandejaYProgresoCompartenAvancePersistidoConRecepcionYSeRetiranAlFinalizar()
    {
        using var c = new Caso();
        await c.AceptarAsync();
        Assert.Single(await c.Ejecucion.ListarAsync());
        c.Entrar(1);
        var datos = await c.Ejecucion.ObtenerAsync(1);
        await c.Ejecucion.TomarAsync(1, datos.Version, "Alcance revisado");
        await c.VersionesAsync();
        datos = await c.Ejecucion.ObtenerAsync(1);
        datos.Items[0].Estado = EstadoEjecucionDetalle.EnCurso;
        datos.Items[0].CantidadEjecutada = 1;
        await c.Ejecucion.GuardarAsync(datos, false);
        await c.VersionesAsync();
        c.Entrar(2);
        var fila = Assert.Single(await c.Ejecucion.ListarAsync());
        Assert.Equal(1, fila.EnCurso);
        var progreso = await c.Seguimiento.ObtenerAsync(1);
        Assert.False(progreso.PuedeRegistrar);
        Assert.Contains(progreso.Progreso, i => i.Estado == EstadoEjecucionDetalle.EnCurso && i.CantidadEjecutada == 1);
        Assert.Contains(await c.Db.Set<HistorialAtencion>().AsNoTracking().ToListAsync(), h => h.Accion == "ItemEjecucionActualizado");
        datos.Items[0].CantidadEjecutada = 2;
        await Assert.ThrowsAsync<ValidacionException>(() => c.Ejecucion.GuardarAsync(datos, false));
        c.Entrar(1);
        await c.EjecutarAsync();
        c.Entrar(2);
        Assert.Empty(await c.Ejecucion.ListarAsync());
        Assert.All((await c.Seguimiento.ObtenerAsync(1)).Progreso, i => Assert.Equal(EstadoEjecucionDetalle.Completado, i.Estado));
    }

    [Fact]
    public async Task EntregaEstructuradaExigeResponsableConformidadYCuentaSaldada()
    {
        using var c = new Caso();
        await c.AceptarAsync(); await c.EjecutarAsync(); var documento = await c.LiquidarAsync();
        var cuenta = await c.Cobros.ObtenerAsync(1);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Cobros.EntregarAsync(new RegistrarEntregaDto { IdAtencion = 1, Version = cuenta.VersionAtencion, PersonaRetira = "", ConformidadRecepcion = true }));
        await Assert.ThrowsAsync<ValidacionException>(() => c.Cobros.EntregarAsync(new RegistrarEntregaDto { IdAtencion = 1, Version = cuenta.VersionAtencion, PersonaRetira = "Ana Pérez" }));
        await Assert.ThrowsAsync<ValidacionException>(() => c.Cobros.EntregarAsync(new RegistrarEntregaDto { IdAtencion = 1, Version = cuenta.VersionAtencion, PersonaRetira = "Ana Pérez", ConformidadRecepcion = true }));
        await c.Cobros.RegistrarPagoAsync(Caso.Pago(documento, documento.Total)); await c.VersionesAsync();
        cuenta = await c.Cobros.ObtenerAsync(1);
        await c.Cobros.EntregarAsync(new RegistrarEntregaDto { IdAtencion = 1, Version = cuenta.VersionAtencion, PersonaRetira = "Ana Pérez", ConformidadRecepcion = true, Observaciones = "Retiro presencial" });
        var cerrada = await c.Cobros.ObtenerAsync(1);
        Assert.Equal(EstadoAtencion.Cerrada, cerrada.Estado);
        Assert.Contains("Ana Pérez", cerrada.Resumen);
        Assert.Contains("Retiro presencial", cerrada.Resumen);
    }

    [Fact]
    public async Task PagoYEntregaPuedenRealizarseEnMomentosDistintos()
    {
        using var c = new Caso();
        await c.AceptarAsync(); await c.EjecutarAsync(); var documento = await c.LiquidarAsync();
        Assert.False((await c.Cobros.ObtenerAsync(1)).PuedeRegistrarEntrega);
        await c.Cobros.RegistrarPagoAsync(Caso.Pago(documento, documento.Total / 2)); await c.VersionesAsync();
        var cuenta = await c.Cobros.ObtenerAsync(1);
        Assert.False(cuenta.PuedeRegistrarEntrega);
        await c.Cobros.RegistrarPagoAsync(Caso.Pago(cuenta.Comprobante!, cuenta.Comprobante!.Saldo)); await c.VersionesAsync();
        c.Entrar(2); // Recepción vuelve a consultar después: el pago no cerró ni entregó.
        cuenta = await c.Cobros.ObtenerAsync(1);
        Assert.True(cuenta.PuedeRegistrarEntrega);
        Assert.NotEqual(EstadoAtencion.Cerrada, cuenta.Estado);
        Assert.True((await c.Cobros.ListarAsync()).Single(a => a.IdAtencion == 1).PuedeRegistrarEntrega);
        Assert.All(cuenta.Comprobante!.Pagos, p => Assert.NotEmpty(p.DetalleMedios));
        await c.Cobros.EntregarAsync(new RegistrarEntregaDto { IdAtencion = 1, Version = cuenta.VersionAtencion,
            PersonaRetira = "Ana Pérez", ConformidadRecepcion = true }); await c.VersionesAsync();
        Assert.False((await c.Cobros.ObtenerAsync(1)).PuedeRegistrarEntrega);
        Assert.False((await c.Cobros.ListarAsync()).Single(a => a.IdAtencion == 1).PuedeRegistrarEntrega);
    }

    [Fact]
    public async Task RecepcionActualPuedeCancelarseSoloAntesDeActividadTecnica()
    {
        using var c = new Caso();
        var anterior = await c.Db.Atenciones.SingleAsync();
        anterior.Estado = EstadoAtencion.Cerrada; anterior.FechaCierre = FechaHoraTaller.Ahora;
        await c.Db.SaveChangesAsync(); c.Db.ChangeTracker.Clear(); c.Entrar(2);
        var a = await c.Recepcion.RegistrarRecepcionAsync(new() { IdCliente = 1, IdVehiculo = 1, IdUsuarioRecepcion = 2,
            Solicitudes = [new() { Tipo = TipoSolicitudIngreso.TrabajoSolicitado, Descripcion = "Solicita reemplazo de discos" }] });
        await c.VersionesAsync();
        a = await c.Db.Atenciones.AsNoTracking().SingleAsync(x => x.IdAtencion == a.IdAtencion);
        await c.Evaluacion.CancelarIngresoAsync(a.IdAtencion, a.Version, "Desiste antes de cualquier intervención");
        Assert.Equal(EstadoAtencion.Cancelada, (await c.Db.Atenciones.AsNoTracking().SingleAsync(x => x.IdAtencion == a.IdAtencion)).Estado);
        Assert.Contains(await c.Db.HistorialAtenciones.AsNoTracking().ToListAsync(), h => h.IdAtencion == a.IdAtencion && h.Accion == "AperturaCancelada" && h.EstadoAnterior == EstadoAtencion.EnEvaluacionTecnica);
    }

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
            var cliente = new Cliente { Telefono = "3794123456",
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
            Db.Add(new Atencion { IdAtencion = 1, IdCliente = 1, Cliente = cliente, IdVehiculo = 1, Vehiculo = new Vehiculo { IdVehiculo = 1, Dominio = "AB123CD", Color = "Gris", IdModelo = 1, Modelo = modelo }, IdUsuarioRecepcion = 2, TipoSolicitud = TipoSolicitudAtencion.Problema, MotivoConsulta = "Ruido al frenar", Estado = EstadoAtencion.EnEvaluacionTecnica, FechaApertura = FechaHoraTaller.Ahora, Version = new byte[8] });
            Db.AddRange(new Usuario { Id = 1, RolId = RolesSistema.TecnicoId, Nombre = "Técnico", Apellido = "Uno", Activo = true }, new Usuario { Id = 2, RolId = RolesSistema.AdministrativoId, Nombre = "Recepción", Apellido = "Uno", Activo = true }, new Usuario { Id = 3, RolId = RolesSistema.TecnicoId, Nombre = "Técnico", Apellido = "Dos", Activo = true });
            var categoria = new Categoria
            {
                IdCategoria = 1,
                Nombre = "Frenos",
                Activo = true
            };
            Db.AddRange(new Concepto { IdConcepto = 1, IdCategoria = 1, Categoria = categoria, Nombre = "Mano de obra", Tipo = TipoConcepto.Servicio, Activo = true }, new Concepto { IdConcepto = 2, IdCategoria = 1, Categoria = categoria, Nombre = "Repuesto", Tipo = TipoConcepto.Repuesto, Activo = true }, new Concepto { IdConcepto = 3, IdCategoria = 1, Categoria = categoria, Nombre = "Inspección inicial", Tipo = TipoConcepto.Servicio, Activo = true });
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
            Recepcion = new AtencionServicio(new AtencionRepositorio(Db), new ClienteRepositorio(Db), new VehiculoRepositorio(Db), usuarios, Sesion);
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
            {
                Entrar(1);
                await Evaluacion.TomarAsync(1);
                await VersionesAsync();
                var seguimiento = await Seguimiento.ObtenerAsync(1);
                var idInspeccion = await Seguimiento.CrearAdicionalAsync(new() { IdAtencion = 1, Version = seguimiento.Version, Motivo = "Desarme adicional para determinar la falla", Items = [new() { IdConcepto = 3, Descripcion = "Inspección autorizada", Cantidad = 1, PrecioUnitario = 30 }] });
                await VersionesAsync();
                Entrar(2);
                var inspeccion = await Presupuestos.ObtenerAsync(idInspeccion);
                await Evaluacion.PresentarAsync(idInspeccion, inspeccion.Version);
                await VersionesAsync();
                inspeccion = await Presupuestos.ObtenerAsync(idInspeccion);
                await Presupuestos.DecidirAsync(new() { IdPresupuesto = idInspeccion, Version = inspeccion.Version, ItemsAceptados = inspeccion.Items.Select(i => i.IdDetalle).ToList() });
            }
            await VersionesAsync();
            Entrar(1);
            if (!servicioInicial) await Evaluacion.TomarAsync(1);
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
            foreach (var i in d.InspeccionesAutorizadas)
                i.Estado = EstadoEjecucionDetalle.Completado;
            await Evaluacion.GuardarAsync(d, true);
            await VersionesAsync();
            Entrar(2);
            var p = await Db.Presupuestos.AsNoTracking().SingleAsync(x => !x.EsAdicional);
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
                Assert.Equal(original.IdPresupuesto, id);
                Assert.Equal(EstadoPresupuesto.Aceptado, (await Presupuestos.ObtenerAsync(id)).Estado);
                Entrar(2);
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

            d.ConformidadTecnica = true;
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

    [Theory]
    [InlineData(0)] // solo trabajos conocidos
    [InlineData(1)] // solo problema desconocido
    [InlineData(2)] // pedidos y síntomas en la misma visita
    public async Task RecepcionIndividual_HastaEntrega_RespetaAutorizacionYRealizado(int caso)
    {
        using var c = new Caso();
        var anterior = await c.Db.Atenciones.SingleAsync();
        anterior.Estado = EstadoAtencion.Cerrada; anterior.FechaCierre = FechaHoraTaller.Ahora;
        await c.Db.SaveChangesAsync(); c.Db.ChangeTracker.Clear();
        var solicitudes = new List<SolicitudIngresoDto>();
        if (caso != 1) solicitudes.Add(new() { Tipo = TipoSolicitudIngreso.TrabajoSolicitado, Descripcion = "Cambiar aceite y filtros" });
        if (caso != 0) solicitudes.Add(new() { Tipo = TipoSolicitudIngreso.ProblemaInformado, Descripcion = "Ruido al frenar en caliente" });
        var a = await c.Recepcion.RegistrarRecepcionAsync(new() { IdCliente = 1, IdVehiculo = 1, IdUsuarioRecepcion = 2,
            Kilometraje = 45000, ObservacionesRecepcion = "Testigo encendido informado al ingresar", Solicitudes = solicitudes });
        int id = a.IdAtencion;
        Assert.Equal(EstadoAtencion.EnEvaluacionTecnica, a.Estado);
        Assert.Equal(solicitudes.Count, a.Solicitudes.Count);
        await c.VersionesAsync();
        c.Entrar(1); await c.Evaluacion.TomarAsync(id); await c.VersionesAsync();
        var preparacion = await c.Evaluacion.ObtenerAsync(id);
        Assert.Equal(45000, preparacion.Kilometraje);
        Assert.Equal(caso != 0, preparacion.IncluyeDiagnostico);
        Assert.Empty(preparacion.InspeccionesAutorizadas);
        preparacion.FallaDetectada = caso == 0 ? "" : "Desgaste comprobado en frenos";
        preparacion.TrabajoRecomendado = caso == 0 ? "" : "Reemplazar las piezas desgastadas";
        preparacion.Items = [new() { IdSolicitudAtencion = preparacion.Solicitudes[0].IdSolicitud, IdConcepto = 1, Descripcion = "Mano de obra", Cantidad = 2, PrecioUnitario = 100 },
            new() { IdSolicitudAtencion = preparacion.Solicitudes[^1].IdSolicitud, IdConcepto = 2, Descripcion = "Repuesto", Cantidad = 1, PrecioUnitario = 50 }];
        if (caso == 2)
        {
            var solicitud = preparacion.Items[1].IdSolicitudAtencion;
            preparacion.Items[1].IdSolicitudAtencion = int.MaxValue;
            await Assert.ThrowsAsync<ValidacionException>(() => c.Evaluacion.GuardarAsync(preparacion, false));
            preparacion.Items[1].IdSolicitudAtencion = null;
            await Assert.ThrowsAsync<ValidacionException>(() => c.Evaluacion.GuardarAsync(preparacion, true));
            preparacion.Items[1].IdSolicitudAtencion = solicitud;
        }
        await c.Evaluacion.GuardarAsync(preparacion, true); await c.VersionesAsync();
        Assert.Equal(caso == 0 ? 0 : 1, await c.Db.Diagnosticos.CountAsync());
        var p = await c.Db.Presupuestos.AsNoTracking().SingleAsync();
        c.Entrar(2); await c.Evaluacion.PresentarAsync(p.IdPresupuesto, p.Version); await c.VersionesAsync();
        var propuesta = await c.Presupuestos.ObtenerAsync(p.IdPresupuesto);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Presupuestos.DecidirAsync(new() { IdPresupuesto = propuesta.IdPresupuesto, Version = propuesta.Version,
            ItemsAceptados = propuesta.Items.Select(i => i.IdDetalle).ToList() }));
        await c.Presupuestos.DecidirAsync(new() { IdPresupuesto = propuesta.IdPresupuesto, Version = propuesta.Version,
            PersonaDecision = "Ana Pérez", MedioDecision = "Presencial", ItemsAceptados = propuesta.Items.Select(i => i.IdDetalle).ToList() });
        await c.VersionesAsync();
        c.Entrar(1); var ejecucion = await c.Ejecucion.ObtenerAsync(id);
        await c.Ejecucion.TomarAsync(id, ejecucion.Version, "Materiales disponibles"); await c.VersionesAsync();
        ejecucion = await c.Ejecucion.ObtenerAsync(id);
        foreach (var item in ejecucion.Items) { item.CantidadEjecutada = item.CantidadAutorizada; item.Estado = EstadoEjecucionDetalle.Completado; }
        await Assert.ThrowsAsync<ValidacionException>(() => c.Ejecucion.GuardarAsync(ejecucion, true));
        ejecucion.ConformidadTecnica = true;
        await c.Ejecucion.GuardarAsync(ejecucion, true); await c.VersionesAsync();
        c.Entrar(2); var cuenta = await c.Cobros.ObtenerAsync(id);
        Assert.Equal(250, cuenta.TotalPorLiquidar);
        await c.Cobros.LiquidarAsync(id, cuenta.VersionAtencion, "Trabajos realizados y comprobados"); await c.VersionesAsync();
        cuenta = await c.Cobros.ObtenerAsync(id);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Cobros.EntregarAsync(id, cuenta.VersionAtencion, "Cliente conforme"));
        await c.Cobros.RegistrarPagoAsync(Caso.Pago(cuenta.Comprobante!, 100)); await c.VersionesAsync();
        cuenta = await c.Cobros.ObtenerAsync(id); Assert.Equal(150, cuenta.Comprobante!.Saldo);
        await c.Cobros.RegistrarPagoAsync(Caso.Pago(cuenta.Comprobante, 50, 100)); await c.VersionesAsync();
        cuenta = await c.Cobros.ObtenerAsync(id); Assert.Equal(0, cuenta.Comprobante!.Saldo);
        await c.Cobros.EntregarAsync(id, cuenta.VersionAtencion, "Ana Pérez retira conforme con los trabajos y documentación");
        Assert.Equal(EstadoAtencion.Cerrada, (await c.Db.Atenciones.AsNoTracking().SingleAsync(x => x.IdAtencion == id)).Estado);
    }

    [Fact]
    public async Task SeleccionParcial_ReformuladaExigeNuevaAceptacion()
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
    public async Task Rechazo_LiquidaSoloInspeccionesAutorizadasRealizados(bool inicial, int total)
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
    public async Task AvanceParcial_SeConservaEnCursoYNoPermiteCerrarNiLiquidar()
    {
        using var c = new Caso();
        await c.AceptarAsync(); c.Entrar(1);
        var e = await c.Ejecucion.ObtenerAsync(1);
        await c.Ejecucion.TomarAsync(1, e.Version, ""); await c.VersionesAsync();
        e = await c.Ejecucion.ObtenerAsync(1);
        e.Items[0].CantidadEjecutada = 1;
        e.Items[0].Estado = EstadoEjecucionDetalle.EnCurso;
        await c.Ejecucion.GuardarAsync(e, false); await c.VersionesAsync();
        e = await c.Ejecucion.ObtenerAsync(1); e.ConformidadTecnica = true;
        await Assert.ThrowsAsync<ValidacionException>(() => c.Ejecucion.GuardarAsync(e, true));
        Assert.Equal(EstadoAtencion.EnEjecucion, (await c.Db.Atenciones.AsNoTracking().SingleAsync()).Estado);
        Assert.Equal(1, (await c.Ejecucion.ObtenerAsync(1)).Items[0].CantidadEjecutada);
        c.Entrar(2);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Cobros.ObtenerAsync(1));
        var version = (await c.Db.Atenciones.AsNoTracking().SingleAsync()).Version;
        await Assert.ThrowsAsync<ValidacionException>(() => c.Cobros.LiquidarAsync(1, version, "Intento antes de terminar"));
    }

    [Theory]
    [InlineData("Precio")]
    [InlineData("Cantidad")]
    [InlineData("Descripcion")]
    [InlineData("Origen")]
    [InlineData("Condiciones")]
    public async Task ModificarSeleccion_NoReutilizaLaAutorizacionDelCliente(string cambio)
    {
        using var c = new Caso();
        var p = await c.PropuestaAsync();
        await c.Presupuestos.DecidirAsync(new() { IdPresupuesto = p.IdPresupuesto, Version = p.Version, ItemsAceptados = [p.Items[1].IdDetalle], Observaciones = "Solo repuesto" });
        await c.VersionesAsync(); c.Entrar(1);
        p = await c.Presupuestos.ObtenerAsync(p.IdPresupuesto);
        var d = p.Items.Single(i => i.Aceptar);
        var item = new ItemPropuestaDto { IdConcepto = d.IdConcepto, Descripcion = d.Descripcion, Cantidad = d.Cantidad, PrecioUnitario = d.PrecioUnitario, OrigenMaterial = d.OrigenMaterial };
        if (cambio == "Precio") item.PrecioUnitario += 1;
        if (cambio == "Cantidad") item.Cantidad += 1;
        if (cambio == "Descripcion") item.Descripcion += " modificado";
        if (cambio == "Origen") { item.OrigenMaterial = OrigenMaterial.Cliente; item.PrecioUnitario = 0; }
        var nuevo = await c.Seguimiento.RevisarSeleccionAsync(p.IdPresupuesto, p.Version, [item], "Se modifica la propuesta y debe consultarse al cliente", condicionesModificadas: cambio == "Condiciones");
        await c.VersionesAsync();
        Assert.NotEqual(p.IdPresupuesto, nuevo);
        Assert.Equal(EstadoPresupuesto.PendientePresentacion, (await c.Presupuestos.ObtenerAsync(nuevo)).Estado);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Ejecucion.ObtenerAsync(1));
    }

    [Fact]
    public async Task SeleccionExactaValidada_HabilitaEjecucionYPreservaDecisionDelCliente()
    {
        using var c = new Caso();
        var p = await c.PropuestaAsync();
        await c.Presupuestos.DecidirAsync(new() { IdPresupuesto = p.IdPresupuesto, Version = p.Version,
            ItemsAceptados = [p.Items[0].IdDetalle], PersonaDecision = "Ana Pérez", MedioDecision = "Teléfono", Observaciones = "Autoriza solo mano de obra si es viable" });
        await c.VersionesAsync();
        var decision = await c.Db.Presupuestos.AsNoTracking().SingleAsync(x => x.IdPresupuesto == p.IdPresupuesto);
        p = await c.Presupuestos.ObtenerAsync(p.IdPresupuesto);
        var seleccion = p.Items.Where(i => i.Aceptar).Select(i => new ItemPropuestaDto { IdSolicitudAtencion = i.IdSolicitudAtencion,
            IdConcepto = i.IdConcepto, Descripcion = i.Descripcion, Cantidad = i.Cantidad, PrecioUnitario = i.PrecioUnitario, OrigenMaterial = i.OrigenMaterial }).ToList();
        await Assert.ThrowsAsync<ValidacionException>(() => c.Seguimiento.RevisarSeleccionAsync(p.IdPresupuesto, p.Version, seleccion, "Viable"));
        c.Entrar(1);
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => c.Seguimiento.RevisarSeleccionAsync(p.IdPresupuesto, new byte[8], seleccion, "Viable"));
        int validado = await c.Seguimiento.RevisarSeleccionAsync(p.IdPresupuesto, p.Version, seleccion, "La tarea puede hacerse sin las excluidas");
        await c.VersionesAsync();
        Assert.Equal(p.IdPresupuesto, validado);
        var guardado = await c.Db.Presupuestos.AsNoTracking().SingleAsync(x => x.IdPresupuesto == validado);
        Assert.Equal(EstadoPresupuesto.Aceptado, guardado.Estado); Assert.True(guardado.AlcanceValidado);
        Assert.Equal(decision.FechaDecision, guardado.FechaDecision); Assert.Equal(decision.IdUsuarioDecision, guardado.IdUsuarioDecision);
        Assert.Equal("Ana Pérez", guardado.PersonaDecision); Assert.Equal("Teléfono", guardado.MedioDecision);
        Assert.Single((await c.Ejecucion.ObtenerAsync(1)).Items);
        Assert.Contains(await c.Db.HistorialAtenciones.AsNoTracking().ToListAsync(), h => h.Accion == "SeleccionClienteValidada" && h.IdUsuario == 1);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Seguimiento.RevisarSeleccionAsync(p.IdPresupuesto, guardado.Version, seleccion, "Reintento"));
        var ejecucion = await c.Ejecucion.ObtenerAsync(1);
        await c.Ejecucion.TomarAsync(1, ejecucion.Version, "");
        await c.VersionesAsync();
        Assert.Equal(1, (await c.Ejecucion.ObtenerAsync(1)).IdTecnico);
        c.Entrar(2);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Evaluacion.PresentarAsync(validado, guardado.Version));
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
        e.Items[0].Estado = EstadoEjecucionDetalle.EnCurso;
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
        Assert.All(await c.Db.Set<HistorialAtencion>().AsNoTracking().ToListAsync(), x => Assert.True((x.Observaciones?.Length ?? 0) <= limiteHistorial));
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
        var evento = await c.Db.Set<HistorialAtencion>().AsNoTracking().SingleAsync(h => h.Accion == "PagoAnulado");
        Assert.EndsWith(motivo, evento.Observaciones);
        var limite = c.Db.Model.FindEntityType(typeof(HistorialAtencion))!.FindProperty(nameof(HistorialAtencion.Observaciones))!.GetMaxLength()!.Value;
        Assert.True(evento.Observaciones!.Length <= limite);
    }

    [Fact]
    public async Task CancelarApertura_ConservaMotivoYCierreSinIngresarAlCircuitoTecnico()
    {
        using var c = new Caso();
        var a = await c.Db.Atenciones.AsNoTracking().SingleAsync();
        await c.Evaluacion.CancelarIngresoAsync(a.IdAtencion, a.Version, "El cliente desiste antes de ingresar");
        var actual = await c.Db.Atenciones.AsNoTracking().SingleAsync();
        Assert.Equal(EstadoAtencion.Cancelada, actual.Estado);
        Assert.NotNull(actual.FechaCierre);
        Assert.Equal("AperturaCancelada", (await c.Db.Set<HistorialAtencion>().AsNoTracking().SingleAsync()).Accion);
    }

    [Fact]
    public async Task CancelarApertura_NoPermiteTecnicoNiVersionObsoletaNiServiciosAutorizados()
    {
        using var c = new Caso();
        var a = await c.Db.Atenciones.AsNoTracking().SingleAsync();
        c.Entrar(1);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Evaluacion.CancelarIngresoAsync(a.IdAtencion, a.Version, "Motivo"));
        c.Entrar(2);
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => c.Evaluacion.CancelarIngresoAsync(a.IdAtencion, Enumerable.Repeat((byte)255, 8).ToArray(), "Motivo"));
        await c.PropuestaAsync(servicioInicial: true);
        a = await c.Db.Atenciones.AsNoTracking().SingleAsync();
        c.Entrar(2);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Evaluacion.CancelarIngresoAsync(a.IdAtencion, a.Version, "Motivo"));
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

        e.ConformidadTecnica = true;
        await c.Ejecucion.GuardarAsync(e, true);
        Assert.Equal(EstadoAtencion.TrabajoFinalizado, (await c.Db.Atenciones.SingleAsync()).Estado);
    }

    [Fact]
    public async Task DetencionAdministrativaBloqueaAvancesYSoloTecnicoResuelve()
    {
        using var c = new Caso();
        await c.AceptarAsync();
        c.Entrar(1);
        var e = await c.Ejecucion.ObtenerAsync(1);
        await c.Ejecucion.TomarAsync(1, e.Version, "");
        await c.VersionesAsync();
        c.Entrar(2);
        var seguimiento = await c.Seguimiento.ObtenerAsync(1);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Seguimiento.PausarAsync(1, seguimiento.Version,
            seguimiento.Items[0].IdDetalle, "El cliente solicita detener"));
        await c.Seguimiento.PausarAsync(1, seguimiento.Version, null, "Cliente solicita detener hasta nueva comunicación");
        await c.VersionesAsync();
        seguimiento = await c.Seguimiento.ObtenerAsync(1);
        Assert.Null(seguimiento.Pausas.Single().IdDetalle);
        await Assert.ThrowsAsync<ValidacionException>(() => c.Seguimiento.ResolverPausaAsync(1, seguimiento.Version,
            seguimiento.Pausas.Single().Id, "Recepción no puede certificar continuidad"));
        c.Entrar(1);
        e = await c.Ejecucion.ObtenerAsync(1);
        e.Items[0].Estado = EstadoEjecucionDetalle.Completado;
        e.Items[0].CantidadEjecutada = e.Items[0].CantidadAutorizada;
        await Assert.ThrowsAsync<ValidacionException>(() => c.Ejecucion.GuardarAsync(e, false));
        seguimiento = await c.Seguimiento.ObtenerAsync(1);
        await c.Seguimiento.ResolverPausaAsync(1, seguimiento.Version, seguimiento.Pausas.Single().Id,
            "Recepción confirmó el pedido de retomar y el técnico comprobó condiciones seguras");
        await c.VersionesAsync();
        e = await c.Ejecucion.ObtenerAsync(1);
        e.Items[0].Estado = EstadoEjecucionDetalle.Completado;
        e.Items[0].CantidadEjecutada = e.Items[0].CantidadAutorizada;
        await c.Ejecucion.GuardarAsync(e, false);
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
        await c.VersionesAsync();
        c.Entrar(1);
        await c.Evaluacion.TomarAsync(1);
        await c.VersionesAsync();
        var seg = await c.Seguimiento.ObtenerAsync(1);
        int id = await c.Seguimiento.CrearAdicionalAsync(new() { IdAtencion = 1, Version = seg.Version, Motivo = "Se necesita desarme adicional para inspeccionar", Items = [new() { IdConcepto = 3, Descripcion = "Desarme autorizado por separado", Cantidad = 1, PrecioUnitario = 30 }] });
        await c.VersionesAsync();
        Assert.Empty((await c.Evaluacion.ObtenerAsync(1)).InspeccionesAutorizadas);
        c.Entrar(2);
        var p = await c.Presupuestos.ObtenerAsync(id);
        await c.Evaluacion.PresentarAsync(id, p.Version);
        await c.VersionesAsync();
        p = await c.Presupuestos.ObtenerAsync(id);
        await c.Presupuestos.DecidirAsync(new() { IdPresupuesto = id, Version = p.Version, ItemsAceptados = p.Items.Select(i => i.IdDetalle).ToList() });
        await c.VersionesAsync();
        c.Entrar(1);
        var d = await c.Evaluacion.ObtenerAsync(1);
        Assert.Single(d.InspeccionesAutorizadas);
        d.InspeccionesAutorizadas[0].Estado = EstadoEjecucionDetalle.Completado;
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
