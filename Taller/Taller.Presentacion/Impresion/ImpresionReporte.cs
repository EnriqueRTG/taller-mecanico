using Taller.Aplicacion.DTOs.Reportes;
using Taller.Aplicacion.Reportes;

namespace Taller.Presentacion.Impresion;

/// <summary>Reporte A4 horizontal: resumen y gráfico inicial, detalle tabular con
/// cabeceras repetidas, cantidades precisas y metodología completa al final.</summary>
internal static class ImpresionReporte
{
    public static void Mostrar(Form propietario, ReporteDocumentoDto d, TipoGraficoReporte tipo = TipoGraficoReporte.Barras, EmpresaDocumentoDto? empresa = null)
    {
        var tabla = DocumentoReporteExportador.Tabular(d);
        var pesos = tabla.Columnas.Select((_, i) => i switch { 0 => 1.6f, 1 => 3.4f, 2 => 1.8f, 3 => 1.9f, _ => 1.3f }).ToArray();
        var numericas = Enumerable.Range(4, Math.Max(0, tabla.Columnas.Count - 4)).ToHashSet();
        var resumen = string.Join("   |   ", d.Indicadores.Select(i => i.Nombre + ": " + i.Valor));
        int alto = Math.Min(280, Math.Max(130, d.Grafico.Count * 34));
        ImpresionComercial.Mostrar(propietario, tabla, empresa ?? new(), DocumentoReporteExportador.NombreArchivo(d.Titulo, d.Generado),
            horizontal: true, pesosColumnas: pesos, columnasNumericas: numericas, introduccion: resumen,
            graficoPrimeraPagina: (g, a) => GraficoReporte.Dibujar(g, a, d, tipo), altoGrafico: alto);
    }
}
