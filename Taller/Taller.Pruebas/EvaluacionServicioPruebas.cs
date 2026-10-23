using Taller.Dominio.Tiempo;
using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
using Taller.Infraestructura.Persistencia;
using Taller.Infraestructura.Persistencia.Repositorios;
using Xunit;

namespace Taller.Pruebas;

/// <summary>Prueba reglas del circuito con repositorios reales y EF InMemory.
/// No sustituye las pruebas de restricciones, transacciones y rowversion en SQL Server.</summary>
public sealed class EvaluacionServicioPruebas
{
    private sealed class Escenario : IDisposable
    {
        public TallerDbContext Db { get; }
        public SesionUsuario Sesion { get; } = new();
        public EvaluacionServicio Servicio { get; }
        public Escenario(TipoSolicitudAtencion tipo = TipoSolicitudAtencion.Problema)
        {
            Db = new TallerDbContext(new DbContextOptionsBuilder<TallerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            Db.Database.EnsureCreated();
            var marca = new Marca { IdMarca = 1, Nombre = "Marca", Activo = true };
            var modelo = new Modelo { IdModelo = 1, IdMarca = 1, Marca = marca, Nombre = "Modelo", Activo = true };
            var cliente = new Cliente { IdCliente = 1, Nombre = "Ana", Apellido = "Pérez", Documento = "123", TipoDocumento = "DNI" };
            var vehiculo = new Vehiculo { IdVehiculo = 1, Dominio = "AB123CD", Color = "Gris", IdModelo = 1, Modelo = modelo };
            Db.AddRange(new Usuario { Id = 1, RolId = RolesSistema.TecnicoId, Activo = true },
                new Usuario { Id = 2, RolId = RolesSistema.AdministrativoId, Activo = true },
                new Usuario { Id = 3, RolId = RolesSistema.TecnicoId, Activo = true });
            Db.Add(new Atencion { IdAtencion = 1, IdCliente = 1, Cliente = cliente, IdVehiculo = 1, Vehiculo = vehiculo,
                IdUsuarioRecepcion = 2, MotivoConsulta = "Solicitud del cliente", TipoSolicitud = tipo,
                FechaApertura = FechaHoraTaller.Ahora, Estado = EstadoAtencion.Abierta, Version = new byte[8] });
            Db.Add(new Concepto { IdConcepto = 1, IdCategoria = 1, Categoria = new Categoria { IdCategoria = 1, Nombre = "Mantenimiento", Activo = true },
                Nombre = "Servicio", Tipo = TipoConcepto.Servicio, Activo = true, DisponibleParaIngreso = true });
            Db.SaveChanges(); Db.ChangeTracker.Clear();
            Servicio = new EvaluacionServicio(new EvaluacionRepositorio(Db), new UsuarioRepositorio(Db), new UnidadDeTrabajo(Db), Sesion);
            Entrar(2);
        }
        public void Entrar(int id) => Sesion.Iniciar(new Usuario { Id = id });
        public async Task<EvaluacionAtencionDto> TomarAsync()
        {
            var atencion = await Db.Atenciones.AsNoTracking().SingleAsync();
            await Servicio.EnviarAEvaluacionAsync(1, atencion.TipoSolicitud, atencion.Version);
            Entrar(1); await Servicio.TomarAsync(1);
            return await Servicio.ObtenerAsync(1);
        }
        public static ItemPropuestaDto Item() => new() { IdConcepto = 1, Descripcion = "Servicio solicitado", Cantidad = 1, PrecioUnitario = 100 };
        public void Dispose() => Db.Dispose();
    }

    [Fact]
    public async Task ServicioDefinido_FinalizaPropuestaSinFabricarDiagnostico()
    {
        using var e = new Escenario(TipoSolicitudAtencion.ServicioDefinido);
        var d = await e.TomarAsync(); d.Items.Add(Escenario.Item());
        Assert.False(d.IncluyeDiagnostico);
        await e.Servicio.GuardarAsync(d, finalizar: true);
        Assert.Empty(await e.Db.Diagnosticos.ToListAsync());
        var presupuesto = await e.Db.Presupuestos.SingleAsync();
        Assert.Equal(1, presupuesto.IdAtencion); Assert.Null(presupuesto.IdDiagnostico);
        Assert.Equal(EstadoPresupuesto.PendientePresentacion, presupuesto.Estado);
        Assert.Null((await e.Db.Atenciones.AsNoTracking().SingleAsync()).IdTecnicoEnActividad);
        Assert.True((await e.Servicio.ListarAsync()).Single().Finalizada);
        Assert.Single(await e.Servicio.ListarPresupuestosAsync());
    }

    [Fact]
    public async Task Problema_ExigeInformeYGuardaPropuestaConSuDiagnostico()
    {
        using var e = new Escenario(); var d = await e.TomarAsync(); d.Items.Add(Escenario.Item());
        await Assert.ThrowsAsync<ValidacionException>(() => e.Servicio.GuardarAsync(d, true));
        Assert.Empty(await e.Db.Presupuestos.ToListAsync());
        d.FallaDetectada = "Desgaste detectado"; d.TrabajoRecomendado = "Sustituir componente";
        await e.Servicio.GuardarAsync(d, true);
        var informe = await e.Db.Diagnosticos.SingleAsync();
        Assert.Equal(informe.IdDiagnostico, (await e.Db.Presupuestos.SingleAsync()).IdDiagnostico);
        Assert.Equal(1, informe.IdTecnicoAutor);
    }

    [Fact]
    public async Task GuardarCerrarLiberarYRetomar_ConservaAvanceYAutorDelInformeFinal()
    {
        using var e = new Escenario(); var d = await e.TomarAsync();
        d.FallaDetectada = "Hallazgo parcial"; d.Items.Add(Escenario.Item());
        await e.Servicio.GuardarAsync(d, false);
        Assert.Equal(1, (await e.Db.Atenciones.AsNoTracking().SingleAsync()).IdTecnicoEnActividad);
        await e.Servicio.LiberarAsync(1); e.Entrar(3); await e.Servicio.TomarAsync(1);
        var retomado = await e.Servicio.ObtenerAsync(1);
        Assert.Equal("Hallazgo parcial", retomado.FallaDetectada); Assert.Single(retomado.Items);
        retomado.TrabajoRecomendado = "Trabajo completado";
        await e.Servicio.GuardarAsync(retomado, true);
        Assert.Equal(3, (await e.Db.Diagnosticos.SingleAsync()).IdTecnicoAutor);
        Assert.Equal(EstadoPresupuesto.PendientePresentacion, (await e.Db.Presupuestos.SingleAsync()).Estado);
    }

    [Fact]
    public async Task SinPropuesta_ExigeMotivoYNoDejaPresupuestoVacio()
    {
        using var e = new Escenario(); var d = await e.TomarAsync(); d.Items.Add(Escenario.Item());
        await e.Servicio.GuardarAsync(d, false); d = await e.Servicio.ObtenerAsync(1);
        d.Items.Clear(); d.SoloEvaluacion = true; d.FallaDetectada = "Sin falla reproducible"; d.TrabajoRecomendado = "Observar evolución";
        await Assert.ThrowsAsync<ValidacionException>(() => e.Servicio.GuardarAsync(d, true));
        d.Observaciones = "Se informa el resultado; no requiere reparación.";
        await e.Servicio.GuardarAsync(d, true);
        Assert.Empty(await e.Db.Presupuestos.ToListAsync());
        Assert.Equal(EstadoAtencion.PendienteLiquidacion, (await e.Db.Atenciones.SingleAsync()).Estado);
    }

    [Fact]
    public async Task OtroTecnico_NoPuedeModificarNiLiberarActividadAjena()
    {
        using var e = new Escenario(); var d = await e.TomarAsync(); e.Entrar(3);
        await Assert.ThrowsAsync<ValidacionException>(() => e.Servicio.GuardarAsync(d, false));
        await Assert.ThrowsAsync<ValidacionException>(() => e.Servicio.LiberarAsync(1));
    }

    [Fact]
    public async Task VersionObsoleta_NoSobrescribeAvances()
    {
        using var e = new Escenario(); var d = await e.TomarAsync(); d.VersionAtencion[0] = 99;
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => e.Servicio.GuardarAsync(d, false));
        Assert.Empty(await e.Db.Set<EvaluacionAtencion>().ToListAsync());
    }

    [Fact]
    public async Task ConceptoInactivoODuplicado_NoConfirmaPropuesta()
    {
        using var e = new Escenario(TipoSolicitudAtencion.ServicioDefinido); var d = await e.TomarAsync();
        d.Items.Add(Escenario.Item()); d.Items.Add(Escenario.Item());
        await Assert.ThrowsAsync<ValidacionException>(() => e.Servicio.GuardarAsync(d, true));
        d.Items.RemoveAt(1); (await e.Db.Conceptos.SingleAsync()).Activo = false;
        await e.Db.SaveChangesAsync(); e.Db.ChangeTracker.Clear();
        await Assert.ThrowsAsync<ValidacionException>(() => e.Servicio.GuardarAsync(d, true));
    }

    [Fact]
    public async Task PresentarSoloRecepcion_NoAceptaNiEjecutaLaPropuesta()
    {
        using var e = new Escenario(TipoSolicitudAtencion.ServicioDefinido); var d = await e.TomarAsync(); d.Items.Add(Escenario.Item());
        await e.Servicio.GuardarAsync(d, true);
        var p = await e.Db.Presupuestos.SingleAsync(); p.Version = new byte[8]; await e.Db.SaveChangesAsync(); e.Db.ChangeTracker.Clear();
        await Assert.ThrowsAsync<ValidacionException>(() => e.Servicio.PresentarAsync(p.IdPresupuesto, p.Version));
        e.Entrar(2); await e.Servicio.PresentarAsync(p.IdPresupuesto, p.Version);
        var presentado = await e.Db.Presupuestos.SingleAsync();
        Assert.Equal(EstadoPresupuesto.PendienteDecision, presentado.Estado);
        Assert.Null(presentado.IdUsuarioDecision);
        Assert.Equal(EstadoAtencion.PendienteDecision, (await e.Db.Atenciones.SingleAsync()).Estado);
        Assert.Equal(EstadoEjecucionDetalle.NoIniciado, (await e.Db.PresupuestoDetalles.SingleAsync()).EstadoEjecucion);
    }
    private static AtencionServicio Recepcion(Escenario e) => new(
        new AtencionRepositorio(e.Db), new ClienteRepositorio(e.Db), new VehiculoRepositorio(e.Db),
        new UsuarioRepositorio(e.Db), new ConceptoRepositorio(e.Db), e.Sesion);

    [Fact]
    public async Task ServiciosIniciales_AutorizacionNoEquivaleAEjecucionNiDiagnostico()
    {
        using var e = new Escenario();
        await Recepcion(e).ConfirmarIngresoAsync(new ConfirmarIngresoDto { IdAtencion = 1, VersionAtencion = new byte[8], ClienteAutoriza = true,
            Servicios = [new ServicioIngresoSeleccionadoDto { IdConcepto = 1, Cantidad = 1, PrecioUnitario = 0 }] });
        e.Db.ChangeTracker.Clear();
        Assert.Equal(EstadoAtencion.EnEvaluacionTecnica, (await e.Db.Atenciones.SingleAsync()).Estado);
        var inicial = await e.Db.PresupuestoDetalles.SingleAsync();
        Assert.Equal(EstadoEjecucionDetalle.NoIniciado, inicial.EstadoEjecucion);
        Assert.Equal(0, inicial.CantidadEjecutada); Assert.Null(inicial.IdTecnicoRegistro);
        Assert.Empty(await e.Db.Diagnosticos.ToListAsync());
        e.Entrar(1); await e.Servicio.TomarAsync(1); var d = await e.Servicio.ObtenerAsync(1);
        d.FallaDetectada = "Hallazgo"; d.TrabajoRecomendado = "Reparación"; d.Items.Add(Escenario.Item());
        await Assert.ThrowsAsync<ValidacionException>(() => e.Servicio.GuardarAsync(d, true));
        d.ServiciosIniciales[0].Estado = EstadoEjecucionDetalle.Completado;
        await e.Servicio.GuardarAsync(d, true);
        var ejecutado = await e.Db.PresupuestoDetalles.SingleAsync(x => x.IdPresupuestoDetalle == inicial.IdPresupuestoDetalle);
        Assert.Equal(1, ejecutado.CantidadEjecutada); Assert.Equal(1, ejecutado.IdTecnicoRegistro);
        Assert.Equal(2, await e.Db.Presupuestos.CountAsync());
    }

    [Fact]
    public async Task ServicioInicialOmitido_ExigeJustificacionYConservaCantidadEjecutadaCero()
    {
        using var e = new Escenario();
        await Recepcion(e).ConfirmarIngresoAsync(new ConfirmarIngresoDto { IdAtencion = 1, VersionAtencion = new byte[8], ClienteAutoriza = true,
            Servicios = [new ServicioIngresoSeleccionadoDto { IdConcepto = 1, PrecioUnitario = 100 }] });
        e.Entrar(1); await e.Servicio.TomarAsync(1); var d = await e.Servicio.ObtenerAsync(1);
        d.ServiciosIniciales[0].Estado = EstadoEjecucionDetalle.Omitido;
        await Assert.ThrowsAsync<ValidacionException>(() => e.Servicio.GuardarAsync(d, false));
        d.ServiciosIniciales[0].Observaciones = "No fue necesario realizarlo.";
        await e.Servicio.GuardarAsync(d, false);
        var detalle = await e.Db.PresupuestoDetalles.SingleAsync();
        Assert.Equal(EstadoEjecucionDetalle.Omitido, detalle.EstadoEjecucion);
        Assert.Equal(0, detalle.CantidadEjecutada);
        Assert.Equal("No fue necesario realizarlo.", detalle.ObservacionesEjecucion);
    }

    [Fact]
    public async Task AbrirAtencion_RegistraTipoYEventoDeApertura()
    {
        using var e = new Escenario();
        e.Db.Add(new Vehiculo { IdVehiculo = 2, IdModelo = 1, Dominio = "CD456EF", Color = "Azul" });
        await e.Db.SaveChangesAsync(); e.Db.ChangeTracker.Clear();
        var nueva = await Recepcion(e).RegistrarAsync(1, 2, 2, "Cambio de aceite solicitado", TipoSolicitudAtencion.ServicioDefinido);
        e.Db.ChangeTracker.Clear();
        var evento = await e.Db.HistorialAtenciones.SingleAsync(h => h.IdAtencion == nueva.IdAtencion);
        Assert.Equal("AtencionAbierta", evento.Accion); Assert.Null(evento.EstadoAnterior);
        Assert.Equal(TipoSolicitudAtencion.ServicioDefinido, (await e.Db.Atenciones.FindAsync(nueva.IdAtencion))!.TipoSolicitud);
    }

    [Fact]
    public async Task Recepcion_NoConfirmaSinAutorizarNiConDetalleObsoleto()
    {
        using var e = new Escenario();
        await Assert.ThrowsAsync<ValidacionException>(() => Recepcion(e).ConfirmarIngresoAsync(new ConfirmarIngresoDto {
            IdAtencion = 1, VersionAtencion = new byte[8], ClienteAutoriza = false,
            Servicios = [new ServicioIngresoSeleccionadoDto { IdConcepto = 1, PrecioUnitario = 100 }] }));
        var obsoleta = new byte[8]; obsoleta[0] = 99;
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => Recepcion(e).ConfirmarIngresoAsync(new ConfirmarIngresoDto {
            IdAtencion = 1, VersionAtencion = obsoleta, ClienteAutoriza = true,
            Servicios = [new ServicioIngresoSeleccionadoDto { IdConcepto = 1, PrecioUnitario = 100 }] }));
        await Assert.ThrowsAsync<ConflictoConcurrenciaException>(() => e.Servicio.EnviarAEvaluacionAsync(1, TipoSolicitudAtencion.Problema, obsoleta));
        Assert.Empty(await e.Db.Presupuestos.ToListAsync());
        Assert.Equal(EstadoAtencion.Abierta, (await e.Db.Atenciones.SingleAsync()).Estado);
    }

}
