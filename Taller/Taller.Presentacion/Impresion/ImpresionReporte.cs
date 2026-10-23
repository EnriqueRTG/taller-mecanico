using System.Drawing.Printing;
using System.Text;
using Taller.Aplicacion.DTOs.Reportes;
using Taller.Aplicacion.Servicios;
namespace Taller.Presentacion.Impresion;
/// <summary>Documento paginado con gráfico vectorial, indicadores, metodología y todos los registros.</summary>
internal static class ImpresionReporte
{
    public static void Mostrar(Form propietario, ReporteDocumentoDto d, TipoGraficoReporte tipo = TipoGraficoReporte.Barras)
    {
        if (PrinterSettings.InstalledPrinters.Count == 0) { MessageBox.Show(propietario, "No hay impresora instalada. Puede exportar el documento HTML con su gráfico.", "Imprimir reporte"); return; }
        var b = new StringBuilder();
        foreach (var i in d.Indicadores) b.AppendLine($"{i.Nombre}: {i.Valor}");
        b.AppendLine().AppendLine(d.Recomendacion).AppendLine().AppendLine("MÉTODO: " + d.Metodologia).AppendLine().AppendLine($"DETALLE: {d.Registros.Count} registros");
        foreach (var x in d.Registros) b.AppendLine($"{x.Codigo} · A-{x.IdAtencion} · {x.Fecha:dd/MM/yyyy HH:mm} · {ReporteServicio.Legible(x.Estado)}").AppendLine(x.Descripcion).AppendLine($"Responsable: {x.Responsable} · Cantidad: {x.Cantidad?.ToString("N2") ?? "—"} · Importe ARS: {x.Importe?.ToString("N2") ?? "—"} · Autorizado ARS: {x.ImporteAceptado?.ToString("N2") ?? "—"}").AppendLine();
        string texto = b.ToString(); int inicio = 0, pagina = 0;
        using var documento = new PrintDocument { DocumentName = "SGTM - " + d.Titulo };
        documento.DefaultPageSettings.Landscape = true;
        using var fuente = new Font("Segoe UI", 9); using var titulo = new Font("Segoe UI", 14, FontStyle.Bold); using var formato = new StringFormat(StringFormatFlags.LineLimit);
        documento.BeginPrint += (_, _) => { inicio = 0; pagina = 0; };
        documento.PrintPage += (sender, e) =>
        {
            var g = e.Graphics ?? throw new InvalidOperationException("No hay área de impresión."); pagina++;
            var a = e.MarginBounds; g.DrawString(d.Titulo, titulo, Brushes.Black, new RectangleF(a.X, a.Y, a.Width, 28));
            g.DrawString($"{d.Generado:dd/MM/yyyy HH:mm} · {d.EmitidoPor} · {d.Alcance}\n{d.Filtros}", fuente, Brushes.Black, new RectangleF(a.X, a.Y + 30, a.Width, 52));
            int top = a.Y + 86;
            if (pagina == 1) { int alto = Math.Min(250, Math.Max(55, d.Grafico.Count * 26)); GraficoReporte.Dibujar(g, new Rectangle(a.X, top, a.Width, alto), d, tipo); top += alto + 12; }
            var contenido = new RectangleF(a.X, top, a.Width, a.Bottom - top - 28); string restante = texto[inicio..];
            g.MeasureString(restante, fuente, contenido.Size, formato, out int caracteres, out _);
            if (caracteres == 0) throw new InvalidOperationException("El área de impresión es insuficiente. Use papel A4 horizontal y márgenes normales.");
            g.DrawString(restante[..caracteres], fuente, Brushes.Black, contenido, formato); inicio += caracteres;
            g.DrawString($"SGTM · Informe interno sin validez fiscal · Página {pagina}", fuente, Brushes.DimGray, a.X, a.Bottom - 20); e.HasMorePages = inicio < texto.Length;
        };
        using var vista = new PrintPreviewDialog { Document = documento, Width = 1050, Height = 700, StartPosition = FormStartPosition.CenterParent };
        vista.ShowDialog(propietario);
    }
}
