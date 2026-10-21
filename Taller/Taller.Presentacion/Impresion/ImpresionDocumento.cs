using System.Drawing.Printing;
namespace Taller.Presentacion.Impresion;
/// <summary>Previsualización e impresión paginada mediante las impresoras instaladas de Windows.</summary>
internal static class ImpresionDocumento
{
    public static void Mostrar(Form propietario, string texto)
    {
        if (!PrinterSettings.InstalledPrinters.Cast<string>().Any())
        {
            MessageBox.Show(propietario, "No hay una impresora instalada. Puede instalar una impresora PDF de Windows para guardar o imprimir el documento.", "Impresión"); return;
        }
        using var documento = new PrintDocument(); using var fuente = new Font("Segoe UI", 10);
        using var formato = new StringFormat(StringFormatFlags.LineLimit); int inicio = 0;
        documento.DocumentName = "SGTM - Documento interno";
        documento.BeginPrint += (_, _) => inicio = 0;
        documento.PrintPage += (sender, e) =>
        {
            var graficos = e.Graphics ?? throw new InvalidOperationException("No se pudo crear el área de impresión.");
            var pendiente = texto[inicio..]; var area = new SizeF(e.MarginBounds.Width, e.MarginBounds.Height);
            graficos.MeasureString(pendiente, fuente, area, formato, out var caracteres, out _);
            if (caracteres <= 0) throw new InvalidOperationException("El área de impresión es demasiado pequeña.");
            graficos.DrawString(pendiente[..caracteres], fuente, Brushes.Black, e.MarginBounds, formato);
            inicio += caracteres; e.HasMorePages = inicio < texto.Length;
        };
        using var vista = new PrintPreviewDialog { Document = documento, Width = 1000, Height = 700, StartPosition = FormStartPosition.CenterParent };
        vista.ShowDialog(propietario);
    }
}
