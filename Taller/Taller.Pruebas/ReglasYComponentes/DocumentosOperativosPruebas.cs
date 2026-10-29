using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.DTOs.Diagnosticos;
using Taller.Aplicacion.DTOs.Reportes;
using Taller.Aplicacion.Reportes;
using Taller.Dominio.Enumeraciones;
using Xunit;

namespace Taller.Pruebas;
public sealed class DocumentosOperativosPruebas
{
    [Fact]
    public void DocumentosTabuladosConservanImportesIdentidadYDistincionEntreDeudaYCobro()
    {
        var cuenta = new Taller.Aplicacion.DTOs.Cobros.ComprobanteDto { IdAtencion = 49, Dominio = "AA000AA", Cliente = "Ana", Total = 25, Pagado = 10,
            Items = [new() { Descripcion = "Filtro", Cantidad = 2, PrecioUnitario = 12.5m, Subtotal = 25 }] };
        var pago = new Taller.Aplicacion.DTOs.Cobros.PagoConsultaDto { Importe = 10, SaldoPosterior = 15, DetalleMedios = [new() { Nombre = "Transferencia", Importe = 10, Referencia = "OP-123" }] };
        var liquidacion = DocumentoCuentaExportador.LiquidacionEstructurada(cuenta);
        Assert.Contains("AA000AA", liquidacion.Identidad);
        Assert.Equal(new[] { "Filtro", "2,000", "12,50", "25,00" }, liquidacion.Filas.Single());
        Assert.Equal("25,00", liquidacion.Totales[0].Value);
        Assert.Equal("15,00", liquidacion.Totales.Last().Value);
        var recibo = DocumentoCuentaExportador.ReciboEstructurado(cuenta, pago);
        Assert.Contains("RECIBO", recibo.Titulo);
        Assert.Contains("OP-123", recibo.Filas[0][0]);
        Assert.Equal("10,00", recibo.Totales[0].Value);
        Assert.Contains("histórico", recibo.Notas);
        pago.Anulado = true;
        Assert.Contains("ANULADO", DocumentoCuentaExportador.ReciboEstructurado(cuenta, pago).Titulo);
        var presupuesto = DocumentoOperativoExportador.PresupuestoEstructurado(new() {
            Items = [new() { Descripcion = "Aceite cliente", Cantidad = 4, PrecioUnitario = 0, OrigenMaterial = OrigenMaterial.Cliente }] });
        Assert.Contains("aportado por el cliente", presupuesto.Filas.Single()[0]);
        Assert.Equal("0,00", presupuesto.Totales.Single().Value);
    }

    [Fact]
    public void PresupuestoParaClienteOmiteCamposInternosYSeccionesVacias()
    {
        var datos = new PresupuestoConsultaDto { IdPresupuesto = 7, IdAtencion = 12, Cliente = "Ana Pérez", Vehiculo = "AA111BB",
            PersonaDecision = "Persona de contacto", MedioDecision = "Teléfono", Estado = EstadoPresupuesto.Aceptado,
            Items = [new() { Descripcion = "Servicio acordado", Cantidad = 1, PrecioUnitario = 60 }] };
        var texto = DocumentoOperativoExportador.TextoPresupuesto(datos);
        var html = DocumentoOperativoExportador.Presupuesto(datos, new());
        foreach (var documento in new[] { texto, html })
        {
            Assert.DoesNotContain("Persona de contacto", documento);
            Assert.DoesNotContain("Medio:", documento);
            Assert.DoesNotContain("OBSERVACIONES", documento);
            Assert.DoesNotContain("Observaciones y condiciones", documento);
            Assert.Contains("60,00", documento);
            Assert.Contains("AA111BB", documento);
        }
        datos.Observaciones = "Incluye montaje";
        Assert.Contains("Incluye montaje", DocumentoOperativoExportador.TextoPresupuesto(datos));
        Assert.Contains("Incluye montaje", DocumentoOperativoExportador.Presupuesto(datos, new()));
        Assert.Equal("Persona de contacto", datos.PersonaDecision); // Se conserva en el registro interno.
    }

    [Fact]
    public void PresupuestoConservaAlcanceImportesYIdentidadSinInterpretarHtml()
    {
        var datos = new PresupuestoConsultaDto { IdPresupuesto = 149, IdAtencion = 101, Cliente = "<script>cliente</script>", Vehiculo = "AA111BB", InformeTecnico = new DiagnosticoDetalleDto { FallaDetectada = "Respaldo del diagnóstico" }, Vigente = true, Estado = EstadoPresupuesto.PendienteRevisionTecnica,
            Items = [new() { Descripcion = "Aceite & filtro", SolicitudDescripcion = "Cambio solicitado", Cantidad = 2, PrecioUnitario = 12.5m }] };
        var empresa = new EmpresaDocumentoDto { Nombre = "Taller <Ombú>", Telefono = "123456", LogoPngBase64 = Convert.ToBase64String([137,80,78,71]) };
        var html = DocumentoOperativoExportador.Presupuesto(datos, empresa);
        Assert.Contains("data:image/png;base64,", html);
        Assert.Contains(System.Net.WebUtility.HtmlEncode("Taller <Ombú>"), html);
        Assert.Contains("123456", html);
        Assert.DoesNotContain("<script>", html);
        Assert.Contains("Cambio solicitado", html);
        Assert.Contains("25,00", html);
        Assert.Contains("Informe técnico de respaldo", html);
        Assert.Contains("Respaldo del diagnóstico", System.Net.WebUtility.HtmlDecode(html));
        Assert.Contains("Revisión técnica pendiente", System.Net.WebUtility.HtmlDecode(html));
        Assert.Contains("no registra autorización ni pago", html);
    }
    [Fact]
    public void AvanceExportadoIdentificaEstadosRealesYNoAutorizaNiAcreditaPagos()
    {
        var html = DocumentoOperativoExportador.Progreso(new() {
            IdAtencion = 101, Resumen = "Cliente · AA111BB", Progreso = [new() {
                Descripcion = "Cambio de filtro", CantidadAutorizada = 2, CantidadEjecutada = 1,
                Estado = EstadoEjecucionDetalle.EnCurso, Observaciones = "<b>Nota técnica</b>" }]
        }, new() { Nombre = "Taller", Telefono = "12345" });
        Assert.Contains("En curso", html);
        Assert.Contains("1,000", html);
        Assert.Contains("12345", html);
        Assert.DoesNotContain("<b>Nota", html);
        Assert.Contains("no registra autorización ni pago", html);
    }
    [Fact]
    public void InformeDistingueHallazgoRecomendacionYAutorizacion()
    {
        var datos = new DiagnosticoDetalleDto { IdDiagnostico = 3, IdAtencion = 101, TecnicoAutor = "Técnico", FallaDetectada = "Batería agotada", TrabajoRecomendado = "Reemplazar batería" };
        var html = DocumentoOperativoExportador.Diagnostico(datos, new());
        Assert.Contains("Hallazgo / conclusión", html);
        Assert.Contains("Batería agotada", System.Net.WebUtility.HtmlDecode(html));
        Assert.Contains("Trabajo recomendado", html);
        Assert.Contains("no autoriza trabajos adicionales", html);
    }
}
