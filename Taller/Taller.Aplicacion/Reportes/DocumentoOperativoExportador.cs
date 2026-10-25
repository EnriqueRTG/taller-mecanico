using System.Globalization;
using System.Net;
using System.Text;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.DTOs.Diagnosticos;
using Taller.Aplicacion.DTOs.Reportes;

namespace Taller.Aplicacion.Reportes;
/// <summary>Documentos de presupuesto e informe técnico basados en datos consultados. No emiten comprobantes ni cambian estados.</summary>
public static class DocumentoOperativoExportador
{
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("es-AR");
    private static string H(string? valor) => WebUtility.HtmlEncode(valor ?? "");
    private static string N(decimal valor) => valor.ToString("N2", Cultura);
    private static string Inicio(string titulo, EmpresaDocumentoDto empresa) =>
        "<!doctype html><html lang='es'><head><meta charset='utf-8'><meta name='viewport' content='width=device-width,initial-scale=1'><title>" + H(titulo)
        + "</title><style>body{font:14px Segoe UI,Arial,sans-serif;color:#172033;max-width:1000px;margin:24px auto;padding:16px}h1{font-size:24px}h2{font-size:18px;color:#1e40af}table{border-collapse:collapse;width:100%;font-size:13px}td,th{padding:9px;border:1px solid #cbd5e1;text-align:left;overflow-wrap:anywhere}th{background:#eff6ff}.numero{text-align:right;white-space:nowrap}.texto{white-space:pre-wrap;overflow-wrap:anywhere}.aviso{background:#f1f5f9;padding:12px}tr{break-inside:avoid}thead{display:table-header-group}h2{break-after:avoid}footer{color:#475569}@page{size:A4;margin:16mm}@media print{body{margin:0;padding:0}}</style></head><body>"
        + DocumentoReporteExportador.Cabecera(empresa) + "<h1>" + H(titulo) + "</h1>";
    private const string Fin = "<footer><p>SGTM · Documento interno sin validez fiscal. La emisión no registra autorización ni pago.</p></footer></body></html>";

    public static string Presupuesto(PresupuestoConsultaDto d, EmpresaDocumentoDto empresa)
    {
        var b = new StringBuilder(Inicio($"Presupuesto {d.IdPresupuesto:D6}", empresa));
        b.Append($"<p>Atención {d.IdAtencion:D6} · Último movimiento: {d.Fecha:dd/MM/yyyy HH:mm}<br>Cliente: {H(d.Cliente)}<br>Vehículo: {H(d.Vehiculo)}<br>Tipo: {H(d.TipoDescripcion)} · Estado: {H(d.EstadoDescripcion)} · {(d.Vigente ? "Vigente" : "Histórico")}</p>");
        if (d.IdPresupuestoAnterior.HasValue) b.Append($"<p>Reemplaza al presupuesto {d.IdPresupuestoAnterior.Value:D6}. Se conserva la versión anterior.</p>");
        b.Append("<h2>Alcance e importes propuestos</h2><table><thead><tr><th>Solicitud / concepto</th><th>Cantidad</th><th>Aporta</th><th>Precio ARS</th><th>Subtotal ARS</th></tr></thead><tbody>");
        foreach (var i in d.Items)
            b.Append($"<tr><td>{(string.IsNullOrWhiteSpace(i.SolicitudDescripcion) ? "" : "<small>" + H(i.SolicitudDescripcion) + "</small><br>")}{H(i.Descripcion)}</td><td class='numero'>{i.Cantidad.ToString("N3", Cultura)}</td><td>{H(i.OrigenMaterial.ToString())}</td><td class='numero'>{N(i.PrecioUnitario)}</td><td class='numero'>{N(i.Subtotal)}</td></tr>");
        b.Append($"</tbody></table><h2>Total propuesto: ARS {N(d.Total)}</h2><p class='aviso'>El presupuesto requiere aceptación del cliente antes de ejecutar. Se liquida lo autorizado y efectivamente realizado. Los materiales aportados por el cliente no se cobran como venta del taller.</p><h2>Observaciones y condiciones</h2><p class='texto'>{H(d.Observaciones)}</p>");
        if (!string.IsNullOrWhiteSpace(d.PersonaDecision)) b.Append($"<h2>Decisión registrada</h2><p>Comunicada por: {H(d.PersonaDecision)} · Medio: {H(d.MedioDecision)}<br>Estado: {H(d.EstadoDescripcion)}</p>");
        if (d.InformeTecnico is { } informe) b.Append("<section style='break-before:page'><h2>Informe técnico de respaldo</h2>").Append(CuerpoDiagnostico(informe)).Append("</section>");
        return b.Append(Fin).ToString();
    }

    public static string Diagnostico(DiagnosticoDetalleDto d, EmpresaDocumentoDto empresa) =>
        Inicio($"Informe técnico {d.IdDiagnostico:D6}", empresa) + CuerpoDiagnostico(d) + Fin;
    private static string CuerpoDiagnostico(DiagnosticoDetalleDto d) => $"<p>Atención {d.IdAtencion:D6} · Fecha: {d.FechaDiagnostico:dd/MM/yyyy HH:mm}<br>Técnico responsable: {H(d.TecnicoAutor)}<br>Tipo: {H(d.Tipo.ToString())}<br>Kilometraje: {H(d.Kilometraje?.ToString(Cultura) ?? "No registrado")}</p>"
        + $"<h2>Hallazgo / conclusión</h2><p class='texto'>{H(d.FallaDetectada)}</p><h2>Trabajo recomendado</h2><p class='texto'>{H(d.TrabajoRecomendado)}</p><h2>Observaciones</h2><p class='texto'>{H(d.Observaciones ?? "Sin observaciones")}</p><p class='aviso'>Impide continuar: {(d.ImpideContinuar ? "Sí" : "No")}. Este informe fundamenta la propuesta; no autoriza trabajos adicionales.</p>";

    public static string TextoPresupuesto(PresupuestoConsultaDto d)
    {
        var b = new StringBuilder($"PRESUPUESTO {d.IdPresupuesto:D6}\nAtención {d.IdAtencion:D6} · {d.Fecha:dd/MM/yyyy HH:mm}\nCliente: {d.Cliente}\nVehículo: {d.Vehiculo}\nEstado: {d.EstadoDescripcion} · {(d.Vigente ? "Vigente" : "Histórico")}\n\nALCANCE PROPUESTO\n\n");
        foreach (var i in d.Items) b.AppendLine($"{i.Descripcion}\n  Solicitud: {i.SolicitudDescripcion}\n  {i.Cantidad.ToString("N3", Cultura)} × {N(i.PrecioUnitario)} ARS = {N(i.Subtotal)} ARS · Aporta: {i.OrigenMaterial}\n");
        b.AppendLine($"TOTAL PROPUESTO: ARS {N(d.Total)}\n\nOBSERVACIONES\n{d.Observaciones}\n\nDecisión comunicada por: {d.PersonaDecision} · Medio: {d.MedioDecision}\n\nRequiere aceptación previa. Se liquida lo autorizado y ejecutado. No acredita pagos.");
        if (d.InformeTecnico is { } informe) b.AppendLine("\nINFORME TÉCNICO DE RESPALDO\n").AppendLine(TextoDiagnostico(informe));
        return b.ToString();
    }
    public static string TextoDiagnostico(DiagnosticoDetalleDto d) => $"INFORME TÉCNICO {d.IdDiagnostico:D6}\nAtención {d.IdAtencion:D6} · {d.FechaDiagnostico:dd/MM/yyyy HH:mm}\nTécnico: {d.TecnicoAutor} · Tipo: {d.Tipo}\nKilometraje: {d.Kilometraje?.ToString(Cultura) ?? "No registrado"}\n\nHALLAZGO / CONCLUSIÓN\n{d.FallaDetectada}\n\nTRABAJO RECOMENDADO\n{d.TrabajoRecomendado}\n\nOBSERVACIONES\n{d.Observaciones}\nImpide continuar: {(d.ImpideContinuar ? "Sí" : "No")}\n\nEl informe no autoriza trabajos ni acredita pagos.";
}
