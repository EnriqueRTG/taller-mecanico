using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.DTOs.Diagnosticos;
using Taller.Aplicacion.DTOs.Reportes;
using Taller.Aplicacion.Reportes;
using Taller.Dominio.Enumeraciones;
using Xunit;

namespace Taller.Pruebas;
public sealed class DocumentosOperativosTests
{
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
        var datos = new DiagnosticoDetalleDto { IdDiagnostico = 3, IdAtencion = 101, TecnicoAutor = "Técnico", FallaDetectada = "Batería agotada", TrabajoRecomendado = "Reemplazar batería", ImpideContinuar = true };
        var html = DocumentoOperativoExportador.Diagnostico(datos, new());
        Assert.Contains("Hallazgo / conclusión", html);
        Assert.Contains("Batería agotada", System.Net.WebUtility.HtmlDecode(html));
        Assert.Contains("Trabajo recomendado", html);
        Assert.Contains("Impide continuar: Sí", html);
        Assert.Contains("no autoriza trabajos adicionales", html);
    }
}
