namespace Taller.Aplicacion.DTOs.Reportes;
/// <summary>Documento de consulta estructurado, independiente de impresoras y controles de interfaz.</summary>
public sealed record DocumentoComercialDto(string Titulo, string Referencia, string Identidad,
    IReadOnlyList<string> Columnas, IReadOnlyList<IReadOnlyList<string>> Filas,
    IReadOnlyList<KeyValuePair<string, string>> Totales, string Notas);
