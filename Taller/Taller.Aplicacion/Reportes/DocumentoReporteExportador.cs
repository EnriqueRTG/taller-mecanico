using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Taller.Aplicacion.DTOs.Reportes;
namespace Taller.Aplicacion.Reportes;
/// <summary>Genera documentos portables sin scripts ni dependencias externas; codifica texto del usuario y neutraliza fórmulas CSV.</summary>
public static class DocumentoReporteExportador
{
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("es-AR");
    private static string H(string? texto) => WebUtility.HtmlEncode(texto ?? "");
    public static string Html(ReporteDocumentoDto d, TipoGraficoReporte tipo = TipoGraficoReporte.Barras, EmpresaDocumentoDto? empresa = null)
    {
        empresa ??= new EmpresaDocumentoDto();
        var b = new StringBuilder("<!doctype html><html lang='es'><meta charset='utf-8'><meta name='viewport' content='width=device-width,initial-scale=1'><title>");
        b.Append(H(d.Titulo)).Append("</title><style>body{font:14px Segoe UI,Arial,sans-serif;color:#172033;max-width:1120px;margin:32px auto;padding:16px}h1{font-size:24px}h2{font-size:18px;color:#1e40af}.meta,footer{color:#475569}.indicadores{display:flex;flex-wrap:wrap;gap:12px}.indicador{padding:12px;background:#eff6ff;border:1px solid #cbd5e1;min-width:140px}.indicador strong{display:block;font-size:22px}table{border-collapse:collapse;width:100%;font-size:12px}th,td{border:1px solid #cbd5e1;padding:7px;text-align:left;overflow-wrap:anywhere}th{background:#e2e8f0}svg{width:100%;height:auto}tr,.indicador{break-inside:avoid}thead{display:table-header-group}@media print{body{margin:0;padding:0;max-width:none}svg{max-height:320px}a{color:inherit}h2{break-after:avoid}}@page{size:A4 landscape;margin:14mm}</style><body>")
            .Append(Cabecera(empresa)).Append("<h1>")
            .Append(H(d.Titulo)).Append("</h1><p class='meta'>SGTM · Informe de gestión interno<br>Generado: ").Append(d.Generado.ToString("dd/MM/yyyy HH:mm:ss")).Append(" · Por: ").Append(H(d.EmitidoPor)).Append("<br>").Append(H(d.Alcance)).Append("<br>").Append(H(d.Filtros)).Append("</p><div class='indicadores'>");
        foreach (var i in d.Indicadores) b.Append("<div class='indicador'>").Append(H(i.Nombre)).Append("<strong>").Append(H(i.Valor)).Append("</strong></div>");
        b.Append("</div><h2>Distribución · ").Append(H(d.UnidadGrafico)).Append("</h2>").Append(Svg(d, tipo)).Append("<h2>Lectura para la gestión</h2><p>").Append(H(d.Recomendacion)).Append("</p><h2>Alcance y método</h2><p>").Append(H(d.Metodologia)).Append("</p><h2>Detalle · ").Append(d.Registros.Count).Append(" registros</h2><table><thead><tr><th>Código</th><th>Atención</th><th>Fecha</th><th>Descripción</th><th>Responsable</th><th>Estado</th><th>Cantidad realizada</th><th>Importe ARS</th><th>Autorizado ARS</th></tr></thead><tbody>");
        foreach (var x in d.Registros) b.Append("<tr><td>").Append(H(x.Codigo)).Append("</td><td>").Append(x.IdAtencion).Append("</td><td>").Append(x.Fecha.ToString("dd/MM/yyyy HH:mm")).Append("</td><td>").Append(H(x.Descripcion)).Append("</td><td>").Append(H(x.Responsable)).Append("</td><td>").Append(H(Servicios.ReporteServicio.Legible(x.Estado))).Append("</td><td>").Append(N(x.Cantidad)).Append("</td><td>").Append(N(x.Importe)).Append("</td><td>").Append(N(x.ImporteAceptado)).Append("</td></tr>");
        if (d.Registros.Count == 0) b.Append("<tr><td colspan='9'>Sin registros para el alcance consultado.</td></tr>");
        return b.Append("</tbody></table><footer><p>Documento de gestión. Sin validez fiscal. La situación de los registros puede cambiar después de su generación.</p></footer></body></html>").ToString();
    }
    public static string Svg(ReporteDocumentoDto d, TipoGraficoReporte tipo = TipoGraficoReporte.Barras)
    {
        if (tipo != TipoGraficoReporte.Barras && d.Grafico.Count > 0)
            return SvgAlternativo(d, tipo);
        var b = new StringBuilder($"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 1000 {Math.Max(60, d.Grafico.Count * 42 + 16)}' role='img' aria-label='Distribución del reporte'>");
        decimal max = d.Grafico.Select(x => Math.Abs(x.Valor)).DefaultIfEmpty(1).Max(); if (max == 0) max = 1;
        bool negativos = d.Grafico.Any(x => x.Valor < 0); int origen = negativos ? 610 : 330; int escala = negativos ? 260 : 530;
        for (int i = 0; i < d.Grafico.Count; i++)
        {
            var p = d.Grafico[i]; int ancho = (int)(Math.Abs(p.Valor) / max * escala); int x = p.Valor < 0 ? origen - ancho : origen; int y = 12 + i * 42;
            b.Append($"<text x='0' y='{y + 19}' font-size='14'><title>{H(p.Etiqueta)}</title>{H(p.Etiqueta.Length > 40 ? p.Etiqueta[..37] + "…" : p.Etiqueta)}</text><rect x='{x}' y='{y}' width='{ancho}' height='26' fill='{(p.Valor < 0 ? "#b91c1c" : "#1e40af")}'/><text x='890' y='{y + 19}' font-size='14'>{H(p.Valor.ToString(d.UnidadGrafico == "ARS" ? "N2" : "N0", Cultura))}</text>");
        }
        if (d.Grafico.Count == 0) b.Append("<text x='0' y='26' font-size='16'>Sin datos para graficar</text>");
        return b.Append("</svg>").ToString();
    }
    private static string SvgAlternativo(ReporteDocumentoDto d, TipoGraficoReporte tipo)
    {
        if (tipo == TipoGraficoReporte.Torta && (d.Grafico.Any(p => p.Valor < 0) || d.Grafico.Sum(p => p.Valor) <= 0))
            return Svg(d); // Un neto negativo no puede representarse como parte de un total.
        string[] colores = ["#1e40af", "#047857", "#b45309", "#7c3aed", "#be123c", "#0369a1"];
        static string F(double n) => n.ToString("0.###", CultureInfo.InvariantCulture);
        var b = new StringBuilder("<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 1000 480' role='img' aria-label='Distribución categórica del reporte'>");
        double total = (double)d.Grafico.Sum(p => p.Valor), angulo = -Math.PI / 2;
        double maximo = (double)d.Grafico.Max(p => Math.Abs(p.Valor));
        if (maximo == 0) maximo = 1;
        bool negativos = d.Grafico.Any(p => p.Valor < 0);
        for (int i = 0; i < d.Grafico.Count; i++)
        {
            var punto = d.Grafico[i]; string color = colores[i % colores.Length];
            if (tipo == TipoGraficoReporte.Torta && punto.Valor > 0)
            {
                double amplitud = (double)punto.Valor / total * Math.PI * 2;
                if (amplitud >= Math.PI * 2 - .000001)
                    b.Append($"<circle cx='220' cy='220' r='170' fill='{color}'/>");
                else
                    b.Append($"<path d='M220 220 L{F(220 + 170 * Math.Cos(angulo))} {F(220 + 170 * Math.Sin(angulo))} A170 170 0 {(amplitud > Math.PI ? 1 : 0)} 1 {F(220 + 170 * Math.Cos(angulo + amplitud))} {F(220 + 170 * Math.Sin(angulo + amplitud))} Z' fill='{color}'/>");
                angulo += amplitud;
            }
            else if (tipo == TipoGraficoReporte.Columnas)
            {
                double ancho = 420d / d.Grafico.Count, origen = negativos ? 220 : 390, alto = Math.Abs((double)punto.Valor) / maximo * (negativos ? 160 : 330);
                b.Append($"<line x1='40' y1='{F(origen)}' x2='480' y2='{F(origen)}' stroke='#475569'/><rect x='{F(45 + i * ancho)}' y='{F(punto.Valor < 0 ? origen : origen - alto)}' width='{F(ancho * .7)}' height='{F(alto)}' fill='{color}'/><text x='{F(45 + i * ancho)}' y='425' font-size='13'>{i + 1}</text>");
            }
            b.Append($"<rect x='510' y='{20 + i * 28}' width='14' height='14' fill='{color}'/><text x='535' y='{32 + i * 28}' font-size='13'>{i + 1}. {H(punto.Etiqueta)}: {H(punto.Valor.ToString("N2", Cultura))}</text>");
        }
        return b.Append("</svg>").ToString();
    }

    private static string N(decimal? valor) => valor?.ToString("N2", Cultura) ?? "—";
    private static string Celda(string valor)
    {
        // Excel interpreta fórmulas incluso dentro de comillas: proteger textos que comienzan con operadores o controles.
        if (valor.Length > 0 && ("=+-@".Contains(valor.TrimStart().FirstOrDefault()) || char.IsControl(valor[0]))) valor = "'" + valor;
        return "\"" + valor.Replace("\"", "\"\"") + "\"";
    }
    /// <summary>Nombre ordenable por fecha de elaboración; no crea ni reserva un número fiscal.</summary>
    public static string NombreArchivo(string titulo, DateTime fecha)
    {
        string normalizado = titulo.Normalize(NormalizationForm.FormD);
        string sinAcentos = new(normalizado.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray());
        string nombre = Regex.Replace(sinAcentos.Normalize(NormalizationForm.FormC), "[^a-zA-Z0-9]+", "-").Trim('-');
        if (nombre.Length > 100) nombre = nombre[..100].TrimEnd('-');
        return $"SGTM-{(nombre.Length == 0 ? "Documento" : nombre)}_" + fecha.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture);
    }
    /// <summary>Identidad de empresa compartida por documentos, con texto codificado y logo embebido.</summary>
    public static string Cabecera(EmpresaDocumentoDto empresa)
    {
        // Solo admite PNG embebido: nunca URLs externas ni atributos HTML provenientes de configuración.
        string logo = "";
        if (empresa.LogoPngBase64.Length > 0 && Convert.TryFromBase64String(empresa.LogoPngBase64, new byte[empresa.LogoPngBase64.Length], out int cantidad) && cantidad > 0)
            logo = "<img alt='Logo del taller' style='width:80px;height:80px;object-fit:contain;margin-right:18px' src='data:image/png;base64," + empresa.LogoPngBase64 + "'>";
        return "<header style='display:flex;align-items:center;border-bottom:1px solid #cbd5e1;padding-bottom:12px'>" + logo + "<div><strong>" + H(empresa.Nombre) + "</strong><p>" + H(empresa.Contacto) + "</p></div></header>";
    }
    /// <summary>Detalle tabular con contexto por fila. La directiva sep permite abrirlo en Excel sin depender del separador regional.</summary>
    public static string Csv(ReporteDocumentoDto d, EmpresaDocumentoDto? empresa = null)
    {
        empresa ??= new EmpresaDocumentoDto();
        var b = new StringBuilder("sep=;\r\nCódigo;Atención;Fecha;Descripción;Responsable;Estado;Cantidad realizada;Importe ARS;Autorizado ARS;Reporte;Generado;Emitido por;Alcance;Filtros;Método;Unidad gráfico;Empresa;Dirección;Teléfono;Correo;Identificación fiscal\r\n");
        foreach (var x in d.Registros) b.AppendLine(string.Join(";", Celda(x.Codigo), x.IdAtencion.ToString(Cultura), Celda(x.Fecha.ToString("dd/MM/yyyy HH:mm")), Celda(x.Descripcion), Celda(x.Responsable), Celda(Servicios.ReporteServicio.Legible(x.Estado)), x.Cantidad?.ToString(Cultura) ?? "", x.Importe?.ToString(Cultura) ?? "", x.ImporteAceptado?.ToString(Cultura) ?? "",
            Celda(d.Titulo), Celda(d.Generado.ToString("dd/MM/yyyy HH:mm:ss")), Celda(d.EmitidoPor), Celda(d.Alcance), Celda(d.Filtros), Celda(d.Metodologia), Celda(d.UnidadGrafico),
            Celda(empresa.Nombre), Celda(empresa.Direccion), Celda(empresa.Telefono), Celda(empresa.Correo), Celda(empresa.IdentificacionFiscal)));
        return b.ToString();
    }
}
