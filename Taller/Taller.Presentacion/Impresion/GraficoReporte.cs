using Taller.Aplicacion.DTOs.Reportes;
namespace Taller.Presentacion.Impresion;
/// <summary>Dibuja la misma serie del documento en pantalla e impresión, admitiendo netos negativos.</summary>
internal static class GraficoReporte
{
    public static void Dibujar(Graphics g, Rectangle area, ReporteDocumentoDto d, TipoGraficoReporte tipo = TipoGraficoReporte.Barras)
    {
        using var fuente = new Font("Segoe UI", 9); using var formato = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };
        if (d.Grafico.Count == 0) { g.DrawString("Sin datos para graficar", fuente, Brushes.DimGray, area); return; }
        if (tipo != TipoGraficoReporte.Barras && !(tipo == TipoGraficoReporte.Torta && (d.Grafico.Any(p => p.Valor < 0) || d.Grafico.Sum(p => p.Valor) <= 0)))
        {
            DibujarAlternativo(g, area, d, tipo, fuente, formato);
            return;
        }
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
    private static void DibujarAlternativo(Graphics g, Rectangle area, ReporteDocumentoDto d, TipoGraficoReporte tipo, Font fuente, StringFormat formato)
    {
        Color[] colores = [Color.RoyalBlue, Color.SeaGreen, Color.DarkOrange, Color.MediumPurple, Color.Crimson, Color.SteelBlue];
        float anchoGrafico = area.Width * .48f;
        float diametro = Math.Max(1, Math.Min(anchoGrafico - 12, area.Height - 12));
        decimal max = d.Grafico.Max(p => Math.Abs(p.Valor));
        if (max == 0) max = 1;
        decimal total = d.Grafico.Sum(p => p.Valor);
        bool negativos = d.Grafico.Any(p => p.Valor < 0);
        float angulo = -90;
        for (int i = 0; i < d.Grafico.Count; i++)
        {
            var p = d.Grafico[i];
            using var pincel = new SolidBrush(colores[i % colores.Length]);
            if (tipo == TipoGraficoReporte.Torta && p.Valor > 0)
            {
                float arco = (float)(p.Valor / total) * 360;
                g.FillPie(pincel, area.X, area.Y, diametro, diametro, angulo, arco);
                angulo += arco;
            }
            else if (tipo == TipoGraficoReporte.Columnas)
            {
                float origen = area.Y + (negativos ? area.Height * .48f : area.Height - 28);
                float altura = (float)(Math.Abs(p.Valor) / max) * (negativos ? area.Height * .4f : area.Height - 40);
                float paso = anchoGrafico / d.Grafico.Count;
                g.DrawLine(Pens.DimGray, area.X, origen, area.X + anchoGrafico, origen);
                g.FillRectangle(pincel, area.X + i * paso, p.Valor < 0 ? origen : origen - altura, Math.Max(1, paso * .7f), altura);
                g.DrawString((i + 1).ToString(), fuente, Brushes.Black, area.X + i * paso, area.Bottom - 24);
            }
            float alto = Math.Min(28, (float)area.Height / d.Grafico.Count);
            float x = area.X + anchoGrafico + 12, y = area.Y + i * alto;
            g.FillRectangle(pincel, x, y + 3, 12, 12);
            g.DrawString($"{i + 1}. {p.Etiqueta}: {p.Valor:N2}", fuente, Brushes.Black, new RectangleF(x + 18, y, area.Width - anchoGrafico - 32, alto), formato);
        }
    }

}
