namespace Taller.Aplicacion.DTOs.Reportes;
/// <summary>Valor de una categoría del gráfico; admite movimientos negativos por anulación.</summary>
public sealed record PuntoReporteDto(string Etiqueta, decimal Valor);
