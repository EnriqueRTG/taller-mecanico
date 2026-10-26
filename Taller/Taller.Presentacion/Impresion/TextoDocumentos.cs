using Taller.Aplicacion.DTOs.Cobros;
using Taller.Aplicacion.Reportes;
namespace Taller.Presentacion.Impresion;
/// <summary>Conecta los documentos de Aplicación con el motor de impresión WinForms.</summary>
internal static class TextoDocumentos
{
    public static string Liquidacion(ComprobanteDto d) => DocumentoCuentaExportador.Liquidacion(d);
    public static string Recibo(ComprobanteDto d, PagoConsultaDto p) => DocumentoCuentaExportador.Recibo(d, p);
}
