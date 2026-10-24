using System.Drawing.Printing;
namespace Taller.Presentacion.Impresion;
/// <summary>Previsualización e impresión paginada mediante las impresoras instaladas de Windows.</summary>
internal static class ImpresionDocumento
{
    public static void Mostrar(Form propietario, string texto, Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto empresa, string nombreArchivo)
    {
        if (!PrinterSettings.InstalledPrinters.Cast<string>().Any())
        {
            MessageBox.Show(propietario, "No hay una impresora instalada. Puede instalar una impresora PDF de Windows para guardar o imprimir el documento.", "Impresión"); return;
        }
        using var documento = new PrintDocument(); using var fuente = new Font("Segoe UI", 10);
        using var formato = new StringFormat(StringFormatFlags.LineLimit); int inicio = 0, pagina = 0;
        documento.DocumentName = nombreArchivo;
        documento.BeginPrint += (_, _) => { inicio = 0; pagina = 0; };
        documento.PrintPage += (sender, e) =>
        {
            var graficos = e.Graphics ?? throw new InvalidOperationException("No se pudo crear el área de impresión.");
            pagina++;
            int encabezado = EncabezadoEmpresa.Dibujar(graficos, e.MarginBounds, empresa);
            var contenido = new Rectangle(e.MarginBounds.X, e.MarginBounds.Y + encabezado, e.MarginBounds.Width, e.MarginBounds.Height - encabezado - 30);
            var pendiente = texto[inicio..]; var area = contenido.Size;
            graficos.MeasureString(pendiente, fuente, area, formato, out var caracteres, out _);
            if (caracteres <= 0) throw new InvalidOperationException("El área de impresión es demasiado pequeña.");
            graficos.DrawString(pendiente[..caracteres], fuente, Brushes.Black, contenido, formato);
            graficos.DrawString($"SGTM · Documento interno sin validez fiscal · Página {pagina}", fuente, Brushes.DimGray, e.MarginBounds.X, e.MarginBounds.Bottom - 22);
            inicio += caracteres; e.HasMorePages = inicio < texto.Length;
        };
        using var vista = new VistaPreviaDocumento(documento, () => pagina);
        vista.ShowDialog(propietario);
    }
}
