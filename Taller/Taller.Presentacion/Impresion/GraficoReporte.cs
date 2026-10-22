using Taller.Aplicacion.DTOs.Reportes;
namespace Taller.Presentacion.Impresion;
/// <summary>Dibuja la misma serie del documento en pantalla e impresión, admitiendo netos negativos.</summary>
internal static class GraficoReporte
{
    public static void Dibujar(Graphics g, Rectangle area, ReporteDocumentoDto d)
    {
        using var fuente = new Font("Segoe UI", 9); using var formato = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };
        if (d.Grafico.Count == 0) { g.DrawString("Sin datos para graficar", fuente, Brushes.DimGray, area); return; }
        decimal max = d.Grafico.Max(x => Math.Abs(x.Valor)); if (max == 0) max = 1;
        float alto = Math.Min(32, (float)area.Height / d.Grafico.Count); float etiquetas = area.Width * .35f, ancho = area.Width * .48f;
        bool negativos = d.Grafico.Any(x => x.Valor < 0); float origen = area.X + etiquetas + (negativos ? ancho / 2 : 0); float escala = negativos ? ancho / 2 : ancho;
        for (int i = 0; i < d.Grafico.Count; i++)
        {
            var p = d.Grafico[i]; float y = area.Y + i * alto, barra = (float)(Math.Abs(p.Valor) / max) * escala;
            g.DrawString(p.Etiqueta, fuente, Brushes.Black, new RectangleF(area.X, y, etiquetas - 8, alto), formato);
            g.FillRectangle(p.Valor < 0 ? Brushes.Firebrick : Brushes.RoyalBlue, p.Valor < 0 ? origen - barra : origen, y + 3, barra, Math.Max(3, alto - 9));
            g.DrawString(p.Valor.ToString(d.UnidadGrafico == "ARS" ? "N2" : "N0", System.Globalization.CultureInfo.GetCultureInfo("es-AR")), fuente, Brushes.Black, new RectangleF(area.X + etiquetas + ancho + 8, y, area.Width - etiquetas - ancho - 8, alto), formato);
        }
    }
}
