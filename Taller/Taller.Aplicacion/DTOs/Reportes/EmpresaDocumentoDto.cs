namespace Taller.Aplicacion.DTOs.Reportes;
/// <summary>Identidad del emisor para documentos internos. No representa una habilitación fiscal.</summary>
public sealed class EmpresaDocumentoDto
{
    public string Nombre { get; init; } = "Taller mecánico";
    public string Direccion { get; init; } = "";
    public string Telefono { get; init; } = "";
    public string Correo { get; init; } = "";
    public string IdentificacionFiscal { get; init; } = "";
    public string LogoPngBase64 { get; init; } = "";
    public string Contacto => string.Join(" · ", new[] { Direccion, Telefono, Correo, IdentificacionFiscal }.Where(x => !string.IsNullOrWhiteSpace(x)));
}
