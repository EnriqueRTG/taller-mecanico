using Taller.Dominio.Tiempo;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Taller.Aplicacion.DTOs.Reportes;
using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Reportes;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
using Taller.Infraestructura.Persistencia;
using Taller.Infraestructura.Persistencia.Repositorios;
using Xunit;
namespace Taller.Pruebas;
/// <summary>Verifica alcance, fechas, conciliación, ausencia de datos ficticios y exportación; SQL se traduce sin abrir una conexión.</summary>
public sealed class ReportesPruebas
{
    private sealed class Caso : IDisposable
    {
        public TallerDbContext Db { get; } = new(new DbContextOptionsBuilder<TallerDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        public SesionUsuario Sesion { get; } = new();
        public ReporteServicio Servicio { get; }
        public ReporteRepositorio Repo { get; }
        public DateTime Hoy => FechaHoraTaller.Hoy;
        public Caso()
        {
            Db.Database.EnsureCreated();
            var admin = new Usuario { Id = 1, RolId = RolesSistema.AdministradorId, Nombre = "Admin", Apellido = "Uno" };
            var recep = new Usuario { Id = 2, RolId = RolesSistema.AdministrativoId, Nombre = "Recep", Apellido = "Dos" };
            var tec = new Usuario { Id = 3, RolId = RolesSistema.TecnicoId, Nombre = "Tec", Apellido = "Tres" };
            var otro = new Usuario { Id = 4, RolId = RolesSistema.TecnicoId, Nombre = "Tec", Apellido = "Cuatro" };
            var cliente = new Cliente { Telefono = "3794123456", IdCliente = 1, Documento = "1", TipoDocumento = "DNI", Nombre = "Ana", Apellido = "Pérez" };
            var marca = new Marca { IdMarca = 1, Nombre = "Marca" }; var modelo = new Modelo { IdModelo = 1, Marca = marca, Nombre = "Modelo" };
            var vehiculo = new Vehiculo { IdVehiculo = 1, Dominio = "AB123CD", Color = "Gris", Modelo = modelo };
            Db.AddRange(admin, recep, tec, otro);
            Db.AddRange(new Atencion { IdAtencion = 1, Cliente = cliente, Vehiculo = vehiculo, UsuarioRecepcion = recep, FechaApertura = Hoy.AddDays(-90), MotivoConsulta = "Ruido antiguo", Estado = EstadoAtencion.EnEvaluacionTecnica, IdTecnicoEnActividad = 3 },
                new Atencion { IdAtencion = 2, Cliente = cliente, Vehiculo = vehiculo, UsuarioRecepcion = recep, FechaApertura = Hoy.AddHours(23), MotivoConsulta = "Consulta reciente", Estado = EstadoAtencion.Cerrada },
                new Atencion { IdAtencion = 3, Cliente = cliente, Vehiculo = vehiculo, UsuarioRecepcion = recep, FechaApertura = Hoy.AddDays(-10), MotivoConsulta = "Reserva ajena", Estado = EstadoAtencion.EnEjecucion, IdTecnicoEnActividad = 4 },
                new Atencion { IdAtencion = 4, Cliente = cliente, Vehiculo = vehiculo, UsuarioRecepcion = recep, FechaApertura = Hoy.AddDays(-80), MotivoConsulta = "Disponible", Estado = EstadoAtencion.EnEjecucion });
            Db.AddRange(new Comprobante { IdComprobante = 1, IdAtencion = 1, IdUsuarioEmision = 2, FechaEmision = Hoy.AddDays(-90), Cliente = "Ana", DocumentoCliente = "1", Dominio = "AB123CD", Total = 100, TotalPagado = 40 },
                new Comprobante { IdComprobante = 2, IdAtencion = 2, IdUsuarioEmision = 2, FechaEmision = Hoy, Cliente = "Otra", DocumentoCliente = "2", Dominio = "AB123CD", Total = 50, TotalPagado = 50 });
            Db.AddRange(new Pago { IdPago = 1, IdOperacion = Guid.NewGuid(), IdComprobante = 1, IdUsuarioRegistro = 2, Fecha = Hoy.AddDays(-5), Importe = 30, UsuarioRegistroNombre = "Recep", Anulado = true, FechaAnulacion = Hoy, IdUsuarioAnulacion = 2, MotivoAnulacion = "Duplicado", Medios = [new PagoMedio { IdMedioPago = 1, NombreMedio = "Efectivo histórico", Importe = 10, Orden = 0 }, new PagoMedio { IdMedioPago = 2, NombreMedio = "Transferencia", Importe = 20, Orden = 1 }] },
                new Pago { IdPago = 2, IdOperacion = Guid.NewGuid(), IdComprobante = 1, IdUsuarioRegistro = 2, Fecha = Hoy.AddHours(23), Importe = 40, UsuarioRegistroNombre = "Recep", Medios = [new PagoMedio { IdMedioPago = 1, NombreMedio = "Efectivo histórico", Importe = 40 }] });
            Db.AddRange(new HistorialAtencion { IdAtencion = 1, IdUsuario = 3, Fecha = Hoy, Accion = "EvaluacionTomada", EstadoNuevo = EstadoAtencion.EnEvaluacionTecnica },
                new HistorialAtencion { IdAtencion = 3, IdUsuario = 4, Fecha = Hoy, Accion = "EjecucionTomada", EstadoNuevo = EstadoAtencion.EnEjecucion });
            var categoria = new Categoria { IdCategoria = 1, Nombre = "Mecánica" }; var concepto = new Concepto { IdConcepto = 1, Categoria = categoria, Nombre = "Revisión", Tipo = TipoConcepto.Servicio };
            Db.AddRange(new Presupuesto { IdPresupuesto = 1, IdAtencion = 1, IdUsuarioCreacion = 3, Estado = EstadoPresupuesto.Aceptado, FechaCreacion = Hoy, Detalles = [new PresupuestoDetalle { Concepto = concepto, Descripcion = "Ítem aceptado", Cantidad = 1, PrecioUnitario = 80, AceptadoPorCliente = true, CantidadEjecutada = 1, EstadoEjecucion = EstadoEjecucionDetalle.Completado, IdTecnicoRegistro = 3, FechaRegistroEjecucion = Hoy }, new PresupuestoDetalle { Concepto = concepto, Descripcion = "Ítem excluido", Cantidad = 1, PrecioUnitario = 20, AceptadoPorCliente = false }] },
                new Presupuesto { IdPresupuesto = 2, IdAtencion = 3, IdUsuarioCreacion = 4, Estado = EstadoPresupuesto.Rechazado, FechaCreacion = Hoy },
                new Presupuesto { IdPresupuesto = 3, IdAtencion = 1, IdUsuarioCreacion = 3, Estado = EstadoPresupuesto.Aceptado, FechaCreacion = Hoy, Vigente = false });
            Db.SaveChanges(); Db.ChangeTracker.Clear(); Repo = new ReporteRepositorio(Db); Servicio = new ReporteServicio(Repo, new UsuarioRepositorio(Db), Sesion); Entrar(1);
        }
        public void Entrar(int id) => Sesion.Iniciar(new Usuario { Id = id });
        public Task<ReporteDocumentoDto> Generar(TipoReporte tipo, string busqueda = "", string estado = "Todos") => Servicio.GenerarAsync(new SolicitudReporteDto { Tipo = tipo, Desde = Hoy, Hasta = Hoy, Busqueda = busqueda, Estado = estado });
        public void Dispose() => Db.Dispose();
    }
    [Fact]
    public void FiltroDeActividadSoloOfreceAccionesDelCircuitoActual()
    {
        var acciones = Taller.Aplicacion.Consultas.ActividadTecnicaReporte.Acciones;
        Assert.DoesNotContain("ActividadTomada", acciones);
        Assert.DoesNotContain("ActividadLiberada", acciones);
        Assert.DoesNotContain("DiagnosticoRegistrado", acciones);
        Assert.Contains("EvaluacionFinalizada", acciones);
        Assert.Contains("TrabajoFinalizado", acciones);
        Assert.Equal(acciones.Count, acciones.Distinct().Count());
    }

    [Fact]
    public async Task CargaTecnica_SimplificaFiltroSinOcultarAtencionesConPausas()
    {
        using var c = new Caso();
        c.Db.Add(new PausaTrabajo { IdAtencion = 1, IdUsuario = 3, Fecha = c.Hoy, Motivo = "Requiere revisión antes de continuar" });
        await c.Db.SaveChangesAsync(); c.Db.ChangeTracker.Clear();
        var completo = await c.Generar(TipoReporte.CargaTecnica);
        Assert.Equal(new[] { "Disponible para tomar", "Reservada" }, completo.EstadosDisponibles);
        Assert.Contains(completo.Registros, r => r.IdAtencion == 1 && r.Estado == "Con pausas pendientes");
        var disponibles = await c.Generar(TipoReporte.CargaTecnica, estado: "Disponible para tomar");
        Assert.Single(disponibles.Registros);
        Assert.Equal(4, disponibles.Registros[0].IdAtencion);
        var reservadas = await c.Generar(TipoReporte.CargaTecnica, estado: "Reservada");
        Assert.Single(reservadas.Registros);
        Assert.Equal(3, reservadas.Registros[0].IdAtencion);
    }

    [Fact]
    public async Task OpcionesDeEstadoExistenSinFilasYNoDependenDelPeriodo()
    {
        using var c = new Caso();
        foreach (var tipo in Enum.GetValues<TipoReporte>())
        {
            var opciones = ReporteServicio.EstadosPara(tipo);
            Assert.NotEmpty(opciones);
            var vacio = await c.Generar(tipo, "SIN-COINCIDENCIAS");
            Assert.Equal(opciones, vacio.EstadosDisponibles);
            Assert.Empty(vacio.Registros);
        }
        Assert.Contains("En preparación técnica", ReporteServicio.EstadosPara(TipoReporte.Atenciones));
        Assert.Contains("Cancelada", ReporteServicio.EstadosPara(TipoReporte.Atenciones));
        Assert.Contains("Completado", ReporteServicio.EstadosPara(TipoReporte.ConceptosRealizados));
        Assert.DoesNotContain("No Iniciado", ReporteServicio.EstadosPara(TipoReporte.ConceptosRealizados));
    }

    [Fact]
    public async Task GraficoConResto_IdentificaCadaCategoriaYConservaTodosLosRegistros()
    {
        using var c = new Caso();
        string[] acciones = ["EvaluacionTomada", "EvaluacionLiberada", "EvaluacionAvanceGuardado", "EvaluacionFinalizada",
            "EjecucionLiberada", "EjecucionAvanceGuardado", "TrabajoFinalizado", "HallazgoRegistrado", "PausaResuelta", "SeleccionClienteValidada"];
        c.Db.AddRange(acciones.Select(a => new HistorialAtencion { IdAtencion = 1, IdUsuario = 3, Fecha = c.Hoy, Accion = a, EstadoNuevo = EstadoAtencion.EnEjecucion }));
        await c.Db.SaveChangesAsync(); c.Db.ChangeTracker.Clear();
        var d = await c.Generar(TipoReporte.ActividadTecnica);
        Assert.Equal(12, d.Registros.Count);
        Assert.Equal(7, d.EstadosDisponibles.Count);
        Assert.DoesNotContain("Avance de evaluación guardado", d.EstadosDisponibles);
        Assert.Contains(d.Registros, r => r.Estado == "EvaluacionAvanceGuardado");
        var filtrado = await c.Generar(TipoReporte.ActividadTecnica, estado: "Selección del cliente validada");
        Assert.Single(filtrado.Registros);
        Assert.Equal("SeleccionClienteValidada", filtrado.Registros[0].Estado);
        Assert.Equal(8, d.Grafico.Count);
        Assert.StartsWith("Resto (", d.Grafico.Last().Etiqueta);
        foreach (var accion in acciones) Assert.True(d.Grafico.Any(g => g.Etiqueta == ReporteServicio.Legible(accion)) || d.Metodologia.Contains(ReporteServicio.Legible(accion)));
        Assert.Equal(d.Registros.Count, d.Grafico.Sum(p => p.Valor));
    }

    [Fact]
    public void ExportacionesIdentificanEmpresaYConsulta_SinInyectarHtmlNiFormulas()
    {
        var d = new ReporteDocumentoDto {
            Titulo = "Atenciones recibidas y situación actual", Generado = new DateTime(2026, 10, 24, 10, 50, 39),
            EmitidoPor = "Laura Gómez", Alcance = "Taller completo", Filtros = "Estado: Todos",
            Registros = [new ReporteRegistroDto { Codigo = "A-1", Descripcion = "Ruido; frenos, revisión", Responsable = "Pérez, Ana", Importe = 12.5m }]
        };
        var empresa = new EmpresaDocumentoDto { Nombre = "<Taller>", Correo = "=SUM(1)", Telefono = "123" };
        string html = DocumentoReporteExportador.Html(d, empresa: empresa);
        Assert.Contains("&lt;Taller&gt;", html);
        Assert.DoesNotContain("<Taller>", html);
        string csv = DocumentoReporteExportador.Csv(d, empresa);
        Assert.StartsWith("sep=;\r\n", csv);
        Assert.Contains("Reporte;Generado;Emitido por;Alcance;Filtros", csv);
        Assert.Contains("\"Ruido; frenos, revisión\"", csv);
        Assert.Contains("\"Pérez, Ana\"", csv);
        Assert.Contains("'=SUM(1)", csv);
        Assert.Contains("Laura Gómez", csv);
        Assert.Contains("24/10/2026 10:50:39", csv);
        Assert.Equal("SGTM-Atenciones-recibidas-y-situacion-actual_20261024_105039",
            DocumentoReporteExportador.NombreArchivo(d.Titulo, d.Generado));
        Assert.DoesNotContain("/", DocumentoReporteExportador.NombreArchivo("Informe: / prueba?", d.Generado));
    }
    [Fact]
    public async Task EstadosDisponiblesNoSeReducenPorBusquedaEstadoNiResultadoVacio()
    {
        using var c = new Caso();
        var completo = await c.Generar(TipoReporte.Presupuestos);
        var filtrado = await c.Generar(TipoReporte.Presupuestos, estado: "Aceptado parcialmente");
        var vacio = await c.Generar(TipoReporte.Presupuestos, busqueda: "ZZZ-SIN-RESULTADOS");
        Assert.Equal(ReporteServicio.EstadosPara(TipoReporte.Presupuestos), completo.EstadosDisponibles);
        Assert.Contains("Borrador", completo.EstadosDisponibles);
        Assert.Equal(completo.EstadosDisponibles.ToArray(), filtrado.EstadosDisponibles.ToArray());
        Assert.Equal(completo.EstadosDisponibles.ToArray(), vacio.EstadosDisponibles.ToArray());
        Assert.Empty(vacio.Registros);
    }
    [Fact] public async Task PeriodoIncluyeTodoElUltimoDia_YNoAgregaEjemplos() { using var c = new Caso(); var d = await c.Generar(TipoReporte.Atenciones); Assert.Single(d.Registros); Assert.Equal(2, d.Registros[0].IdAtencion); Assert.Equal(1, d.Grafico.Sum(x => x.Valor)); }
    [Fact] public async Task SaldosIncluyenLiquidacionesAntiguasSinContarCancelacionesDosVeces() { using var c = new Caso(); var d = await c.Generar(TipoReporte.SaldosPendientes); Assert.Equal(60, d.Registros.Single().Importe); Assert.Equal(60, d.Grafico.Sum(x => x.Valor)); }
    [Fact] public async Task CobroMixtoYAnulacionDeOtroPeriodo_ConcilianNetoPorFechaYMedio() { using var c = new Caso(); var d = await c.Generar(TipoReporte.MovimientosCobro); Assert.Equal(3, d.Registros.Count); Assert.Equal(10, d.Registros.Sum(x => x.Importe)); Assert.Equal(10, d.Grafico.Sum(x => x.Valor)); Assert.Contains(d.Grafico, x => x.Etiqueta == "Transferencia" && x.Valor == -20); }
    [Fact] public async Task TecnicoNoPuedeConsultarCaja_NiFalsearRolEnLaSesion() { using var c = new Caso(); c.Entrar(3); c.Sesion.UsuarioActual!.RolId = 1; await Assert.ThrowsAsync<ValidacionException>(() => c.Generar(TipoReporte.MovimientosCobro)); }
    [Fact] public async Task ReportesTecnicosSoloDevuelvenAutoriaPropia() { using var c = new Caso(); c.Entrar(3); var d = await c.Generar(TipoReporte.ActividadTecnica); Assert.Single(d.Registros); Assert.Equal(1, d.Registros[0].IdAtencion); var p = await c.Generar(TipoReporte.Presupuestos); Assert.Single(p.Registros); Assert.Equal(80, p.Registros[0].ImporteAceptado); }
    [Fact] public async Task CargaTecnicaIncluyePendientesAntiguosPropiosYDisponibles_NoReservasAjenasNiCerradas() { using var c = new Caso(); c.Entrar(3); var d = await c.Generar(TipoReporte.CargaTecnica); Assert.Equal(new[] { 1, 4 }, d.Registros.Select(x => x.IdAtencion).Order().ToArray()); }
    [Fact] public async Task PresupuestosNoDuplicanVersionesReemplazadas() { using var c = new Caso(); var d = await c.Generar(TipoReporte.Presupuestos); Assert.Equal(2, d.Registros.Count); Assert.Contains(d.Indicadores, x => x.Nombre == "Aceptación entre decididos" && x.Valor.StartsWith("50")); Assert.Equal(80, d.Registros.Sum(x => x.ImporteAceptado)); }
    [Fact] public async Task FiltrosAfectanDetalleIndicadoresYGrafico() { using var c = new Caso(); var d = await c.Generar(TipoReporte.MovimientosCobro, "Duplicado", "Anulación"); Assert.Equal(2, d.Registros.Count); Assert.Equal(-30, d.Grafico.Sum(x => x.Valor)); Assert.Contains(d.Indicadores, x => x.Nombre == "Registros" && x.Valor == "2"); }
    [Fact] public async Task SesionRevocadaBloqueaGeneracion() { using var c = new Caso(); c.Entrar(3); var u = await c.Db.Usuarios.FindAsync(3); u!.Activo = false; await c.Db.SaveChangesAsync(); c.Db.ChangeTracker.Clear(); await Assert.ThrowsAsync<ValidacionException>(() => c.Servicio.ListarTiposAsync()); }
    [Fact] public async Task ConsultaVaciaTieneDocumentoSinDatosInventados() { using var c = new Caso(); var d = await c.Generar(TipoReporte.Atenciones, "inexistente"); Assert.Empty(d.Registros); Assert.Empty(d.Grafico); Assert.Contains("No hay registros", d.Recomendacion); Assert.Contains("Sin registros", DocumentoReporteExportador.Html(d)); }
    [Fact] public async Task FechasInvertidasSonRechazadas() { using var c = new Caso(); await Assert.ThrowsAsync<ValidacionException>(() => c.Servicio.GenerarAsync(new SolicitudReporteDto { Tipo = TipoReporte.Atenciones, Desde = c.Hoy, Hasta = c.Hoy.AddDays(-1) })); }
    [Fact] public async Task HtmlCodificaTextoYGraficoEsPortable_CsvNeutralizaFormulas() { using var c = new Caso(); var original = await c.Generar(TipoReporte.Atenciones); var d = new ReporteDocumentoDto { Titulo = "<script>alert(1)</script>", Grafico = [new PuntoReporteDto("<img onerror=x>", -30)], Registros = [new ReporteRegistroDto { Descripcion = "=HYPERLINK(1)", Responsable = "@SUM(1)", Codigo = "R-1" }] }; var h = DocumentoReporteExportador.Html(d); Assert.DoesNotContain("<script>", h); Assert.Contains("&lt;script&gt;", h); Assert.Contains("<svg", h); Assert.Contains("#b91c1c", h); var csv = DocumentoReporteExportador.Csv(d); Assert.Contains("'=HYPERLINK", csv); Assert.Contains("'@SUM", csv); }
    [Fact]
    public async Task PresupuestosRedondeanCadaItemIgualQueElDocumentoOriginal()
    {
        using var c = new Caso(); var detalles = await c.Db.PresupuestoDetalles.Where(x => x.IdPresupuesto == 1).ToListAsync();
        foreach (var d in detalles) { d.Cantidad = .125m; d.PrecioUnitario = .04m; }
        await c.Db.SaveChangesAsync(); c.Db.ChangeTracker.Clear();
        var r = await c.Generar(TipoReporte.Presupuestos); var fila = r.Registros.Single(x => x.Codigo == "P-1"); Assert.Equal(.02m, fila.Importe); Assert.Equal(.01m, fila.ImporteAceptado);
    }
    [Fact]
    public async Task HistorialTecnicoNoDesapareceSiElUsuarioCambiaDeRol()
    {
        using var c = new Caso(); var u = await c.Db.Usuarios.FindAsync(3); u!.RolId = RolesSistema.AdministrativoId; await c.Db.SaveChangesAsync(); c.Db.ChangeTracker.Clear();
        var d = await c.Generar(TipoReporte.ActividadTecnica); Assert.Contains(d.Registros, x => x.IdAtencion == 1);
    }
    [Fact]
    public async Task ConceptosRealizadosSoloIncluyenCantidadesPositivasYPropias()
    {
        using var c = new Caso(); c.Entrar(3); var d = await c.Generar(TipoReporte.ConceptosRealizados); Assert.Single(d.Registros); Assert.Equal(1, d.Registros[0].Cantidad); Assert.Equal(80, d.Registros[0].Importe);
    }
    [Fact]
    public async Task LecturaCatalogosNoConservaEntidadesObsoletas_NiEdicionModificaModeloVehiculo()
    {
        using var c = new Caso(); var clientes = new ClienteRepositorio(c.Db); var vehiculos = new VehiculoRepositorio(c.Db);
        var cliente = (await clientes.ListarActivosAsync()).Single(); var vehiculo = (await vehiculos.ListarActivosAsync()).Single(); Assert.Empty(c.Db.ChangeTracker.Entries());
        cliente.Nombre = "Nombre actualizado"; await clientes.ActualizarAsync(cliente);
        vehiculo.Color = "Rojo"; vehiculo.Modelo.Nombre = "Este cambio al grafo no debe persistirse"; await vehiculos.ActualizarAsync(vehiculo);
        Assert.Empty(c.Db.ChangeTracker.Entries()); Assert.Equal("Nombre actualizado", (await clientes.ObtenerPorIdAsync(1))!.Nombre); Assert.Equal("Modelo", (await vehiculos.ObtenerPorIdAsync(1))!.Modelo.Nombre); Assert.Equal("Rojo", (await vehiculos.ObtenerPorIdAsync(1))!.Color);
    }
    private sealed class DetenerConexion : DbConnectionInterceptor
    {
        public override ValueTask<InterceptionResult> ConnectionOpeningAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result, CancellationToken cancellationToken = default) => throw new InvalidOperationException("SQL traducido; conexión evitada");
    }
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public async Task ConsultasSeTraducenASqlServerSinAbrirBase(int tipo)
    {
        using var db = new TallerDbContext(new DbContextOptionsBuilder<TallerDbContext>().UseSqlServer("Server=localhost;Database=NoSeConecta;Trusted_Connection=True;TrustServerCertificate=True").AddInterceptors(new DetenerConexion()).Options);
        var repo = new ReporteRepositorio(db); var fecha = FechaHoraTaller.Hoy;
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => tipo switch { 0 => repo.ListarAtencionesAsync(fecha, fecha.AddDays(1)), 1 => repo.ListarPresupuestosAsync(fecha, fecha.AddDays(1), 3), 2 => repo.ListarMovimientosAsync(fecha, fecha.AddDays(1)), 3 => repo.ListarSaldosAsync(), 4 => repo.ListarActividadAsync(fecha, fecha.AddDays(1), 3), 5 => repo.ListarConceptosAsync(fecha, fecha.AddDays(1), 3), _ => repo.ListarCargaAsync(3) });
        Assert.Equal("SQL traducido; conexión evitada", ex.Message);
    }
    [Fact]
    public void TortaConUnaCategoriaEsCirculo_YCodificaEtiquetas()
    {
        var d = new ReporteDocumentoDto { Grafico = [new PuntoReporteDto("<script>", 10)] };
        string svg = DocumentoReporteExportador.Svg(d, TipoGraficoReporte.Torta);
        Assert.Contains("<circle", svg);
        Assert.Contains("&lt;script&gt;", svg);
        Assert.DoesNotContain("<script>", svg);
    }

    [Fact]
    public void TortaNoOcultaAnulacionesNegativas_YColumnasLasConservan()
    {
        var d = new ReporteDocumentoDto { UnidadGrafico = "ARS", Grafico = [new PuntoReporteDto("Cobros", 100), new PuntoReporteDto("Anulaciones", -20)] };
        Assert.Equal(DocumentoReporteExportador.Svg(d), DocumentoReporteExportador.Svg(d, TipoGraficoReporte.Torta));
        string columnas = DocumentoReporteExportador.Html(d, TipoGraficoReporte.Columnas);
        Assert.Contains("Anulaciones", columnas);
        Assert.Contains("-20,00", columnas);
        Assert.DoesNotContain("NaN", columnas);
    }

    [Theory]
    [InlineData("Estado inexistente")]
    [InlineData("Cobro")]
    [InlineData("")]
    [InlineData(null)]
    public async Task EstadoInvalidoNoSeConfundeConReporteVacio(string? estado)
    {
        using var c = new Caso();
        await Assert.ThrowsAsync<ValidacionException>(() => c.Servicio.GenerarAsync(new SolicitudReporteDto
        {
            Tipo = TipoReporte.Atenciones, Desde = c.Hoy, Hasta = c.Hoy, Estado = estado!
        }));
    }

    [Fact]
    public void DetalleTabularOmiteMagnitudesInexistentesYPreservaPrecision()
    {
        var soloEstados = DocumentoReporteExportador.Tabular(new() { Registros = [new() { Codigo = "A-1", Descripcion = "Ingreso", Estado = "EnEvaluacionTecnica" }] });
        Assert.Equal(4, soloEstados.Columnas.Count);
        Assert.DoesNotContain("Importe ARS", soloEstados.Columnas);
        var d = new ReporteDocumentoDto { Registros = [new() { Codigo = "ITEM-1", Cantidad = 1.235m, Importe = 123.45m, Descripcion = "Filtro <especial>" }] };
        var detalle = DocumentoReporteExportador.Tabular(d);
        Assert.Equal(6, detalle.Columnas.Count);
        Assert.Equal("1,235", detalle.Filas.Single()[4]);
        Assert.Equal("123,45", detalle.Filas.Single()[5]);
        var html = DocumentoReporteExportador.Html(d);
        Assert.Contains("Filtro &lt;especial&gt;", html);
        Assert.Contains("class='numero'", html);
        Assert.DoesNotContain("<th>Autorizado ARS</th>", html);
    }

    [Fact]
    public void ReporteExtensoConservaTodoElDetalleYLaMetodologia()
    {
        var d = new ReporteDocumentoDto { Metodologia = "Método verificable de prueba", Registros = Enumerable.Range(1, 300)
            .Select(i => new ReporteRegistroDto { Codigo = "COD-" + i, IdAtencion = i,
                Descripcion = new string('A', 500) + " FIN-" + i, Responsable = "Responsable de prueba" }).ToList() };
        var tabla = DocumentoReporteExportador.Tabular(d);
        Assert.Equal(300, tabla.Filas.Count);
        Assert.Contains("FIN-300", tabla.Filas.Last()[1]);
        Assert.Contains(d.Metodologia, tabla.Notas);
        var html = DocumentoReporteExportador.Html(d);
        Assert.Contains("FIN-300", html);
        Assert.Contains("thead{display:table-header-group}", html);
    }

}
