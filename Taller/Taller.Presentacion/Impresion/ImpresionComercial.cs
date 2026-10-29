using System.Drawing.Printing;
using Taller.Aplicacion.DTOs.Reportes;

namespace Taller.Presentacion.Impresion;
/// <summary>Imprime documentos comerciales en tablas. Repite identidad y columnas; divide filas extensas sin perder texto.</summary>
internal static class ImpresionComercial
{
    public static void Mostrar(Form propietario, DocumentoComercialDto datos, EmpresaDocumentoDto empresa, string nombreArchivo,
        bool horizontal = false, float[]? pesosColumnas = null, ISet<int>? columnasNumericas = null,
        string introduccion = "", Action<Graphics, Rectangle>? graficoPrimeraPagina = null, int altoGrafico = 0)
    {
        if (PrinterSettings.InstalledPrinters.Count == 0)
        {
            MessageBox.Show(propietario, "Instale una impresora o Microsoft Print to PDF para imprimir el documento.", "Impresión");
            return;
        }
        using var documento = new PrintDocument { DocumentName = nombreArchivo };
        documento.DefaultPageSettings.Landscape = horizontal;
        documento.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
        documento.DefaultPageSettings.Margins = new Margins(50, 50, 45, 45);
        using var fuente = new Font("Segoe UI", 9);
        using var titulo = new Font("Segoe UI", 13, FontStyle.Bold);
        using var izquierda = new StringFormat(StringFormatFlags.NoWrap);
        using var derecha = new StringFormat(StringFormatFlags.NoWrap) { Alignment = StringAlignment.Far };
        int fila = 0, linea = 0, pagina = 0;
        List<Bloque>? bloques = null;
        documento.BeginPrint += (_, _) => { fila = linea = pagina = 0; bloques = null; };
        documento.PrintPage += (_, e) =>
        {
            var g = e.Graphics ?? throw new InvalidOperationException("No se pudo crear el área de impresión.");
            var area = e.MarginBounds;
            if (area.Width < 300 || area.Height < 400) throw new InvalidOperationException("Seleccione un papel con mayor área imprimible.");
            pagina++;
            float y = area.Top + EncabezadoEmpresa.Dibujar(g, area, empresa);
            float altoLinea = fuente.GetHeight(g) + 3;
            void Parrafo(string texto, Font font)
            {
                var alto = g.MeasureString(texto, font, area.Width).Height + 5;
                g.DrawString(texto, font, Brushes.Black, new RectangleF(area.Left, y, area.Width, alto));
                y += alto;
            }
            Parrafo(datos.Titulo, titulo);
            Parrafo(datos.Referencia, fuente);
            Parrafo(datos.Identidad, fuente);
            y += 5;
            if (pagina == 1)
            {
                if (!string.IsNullOrWhiteSpace(introduccion)) Parrafo(introduccion, fuente);
                if (graficoPrimeraPagina is not null && altoGrafico > 0)
                {
                    graficoPrimeraPagina(g, new Rectangle(area.Left, (int)y, area.Width, altoGrafico));
                    y += altoGrafico + 12;
                }
            }
            float[] pesos = pesosColumnas ?? (datos.Columnas.Count == 2 ? [0.76f, 0.24f] : [0.49f, 0.13f, 0.19f, 0.19f]);
            if (pesos.Length != datos.Columnas.Count || pesos.Any(p => p <= 0))
                throw new InvalidOperationException("La distribución de columnas no corresponde al documento.");
            float[] anchos = pesos.Select(p => p / pesos.Sum() * area.Width).ToArray();
            bool Numerica(int columna) => columnasNumericas?.Contains(columna) ?? columna > 0;
            var cabeceras = datos.Columnas.Select((t, i) => Envolver(g, t, fuente, anchos[i] - 12)).ToArray();
            float altoCabecera = cabeceras.Max(c => c.Count) * altoLinea + 12;
            g.FillRectangle(Brushes.AliceBlue, area.Left, y, area.Width, altoCabecera);
            float x = area.Left;
            for (int c = 0; c < cabeceras.Length; c++)
            {
                for (int l = 0; l < cabeceras[c].Count; l++)
                    g.DrawString(cabeceras[c][l], fuente, Brushes.Black, new RectangleF(x + 6, y + 6 + l * altoLinea, anchos[c] - 12, altoLinea), Numerica(c) ? derecha : izquierda);
                x += anchos[c];
            }
            y += altoCabecera;
            float inicioContenido = y, limite = area.Bottom - 30;
            bloques ??= Construir(g, datos, fuente, area.Width, anchos);
            int filaInicial = fila, lineaInicial = linea;
            while (fila < bloques.Count)
            {
                var b = bloques[fila];
                int totalLineas = b.Celdas.Max(c => c.Count);
                int caben = (int)((limite - y - 12) / altoLinea);
                if (caben <= 0) break;
                // Una fila normal pasa completa a la siguiente página; una extraordinariamente extensa se continúa.
                if (linea == 0 && totalLineas > caben && totalLineas * altoLinea + 12 <= limite - inicioContenido) break;
                int tomar = Math.Min(caben, totalLineas - linea);
                float alto = tomar * altoLinea + 12;
                if (b.Total) g.FillRectangle(Brushes.AliceBlue, area.Left, y, area.Width, alto);
                x = area.Left;
                for (int c = 0; c < b.Celdas.Length; c++)
                {
                    for (int l = 0; l < tomar && linea + l < b.Celdas[c].Count; l++)
                        g.DrawString(b.Celdas[c][linea + l], fuente, Brushes.Black,
                            new RectangleF(x + 6, y + 6 + l * altoLinea, b.Anchos[c] - 12, altoLinea), (b.Total ? c > 0 : Numerica(c)) ? derecha : izquierda);
                    x += b.Anchos[c];
                }
                y += alto;
                g.DrawLine(Pens.LightGray, area.Left, y, area.Right, y);
                linea += tomar;
                if (linea >= totalLineas) { fila++; linea = 0; }
            }
            g.DrawString($"SGTM · Documento interno sin validez fiscal · Página {pagina}", fuente, Brushes.DimGray, area.Left, area.Bottom - 20);
            if (pagina > 1 && fila == filaInicial && linea == lineaInicial && fila < bloques.Count)
                throw new InvalidOperationException("El encabezado no deja espacio para el detalle. Use A4 y márgenes normales.");
            e.HasMorePages = fila < bloques.Count;
        };
        using var vista = new VistaPreviaDocumento(documento, () => pagina);
        vista.ShowDialog(propietario);
    }

    private sealed record Bloque(List<string>[] Celdas, float[] Anchos, bool Total = false);
    private static List<Bloque> Construir(Graphics g, DocumentoComercialDto d, Font fuente, float ancho, float[] columnas)
    {
        var resultado = d.Filas.Select(f => new Bloque(f.Select((t, i) => Envolver(g, t, fuente, columnas[i] - 12)).ToArray(), columnas)).ToList();
        float[] totales = [ancho * .76f, ancho * .24f];
        foreach (var t in d.Totales)
            resultado.Add(new Bloque([Envolver(g, t.Key, fuente, totales[0] - 12), Envolver(g, t.Value, fuente, totales[1] - 12)], totales, true));
        if (!string.IsNullOrWhiteSpace(d.Notas)) resultado.Add(new Bloque([Envolver(g, d.Notas, fuente, ancho - 12)], [ancho]));
        return resultado;
    }

    private static List<string> Envolver(Graphics g, string texto, Font fuente, float ancho)
    {
        var lineas = new List<string>();
        foreach (var parrafo in texto.Replace("\r", "").Split('\n'))
        {
            string resto = parrafo;
            if (resto.Length == 0) { lineas.Add(""); continue; }
            while (resto.Length > 0)
            {
                int minimo = 1, maximo = resto.Length, largo = 1;
                while (minimo <= maximo)
                {
                    int medio = (minimo + maximo) / 2;
                    if (g.MeasureString(resto[..medio], fuente).Width <= ancho)
                    { largo = medio; minimo = medio + 1; }
                    else maximo = medio - 1;
                }
                if (largo > 1 && largo < resto.Length && char.IsHighSurrogate(resto[largo - 1])) largo--;
                if (largo < resto.Length && resto.LastIndexOf(' ', largo - 1, largo) is var espacio && espacio > 0) largo = espacio;
                lineas.Add(resto[..largo]);
                resto = resto[largo..].TrimStart();
            }
        }
        return lineas;
    }
}
