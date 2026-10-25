using System.Text;
namespace Taller.Presentacion.Impresion;
/// <summary>Guarda una copia HTML autónoma con nombre identificable; no abre contenido externo.</summary>
internal static class ExportacionDocumento
{
    public static void GuardarHtml(Form propietario, string html, string nombre)
    {
        using var dialogo = new SaveFileDialog { Filter = "Documento HTML (*.html)|*.html", FileName = nombre + ".html", DefaultExt = "html", AddExtension = true, OverwritePrompt = true };
        if (dialogo.ShowDialog(propietario) != DialogResult.OK) return;
        File.WriteAllText(dialogo.FileName, html, new UTF8Encoding(false));
    }
}
