using System.Text;
using Taller.Aplicacion.DTOs.Cobros;
namespace Taller.Presentacion.Impresion;
/// <summary>Compone documentos de consulta sin datos fiscales inventados.</summary>
internal static class TextoDocumentos
{
    public static string Liquidacion(ComprobanteDto d)
    {
        var texto = new StringBuilder($"SGTM · TALLER MECÁNICO\nLIQUIDACIÓN INTERNA · SIN VALIDEZ FISCAL\n{d.Numero} · {d.Fecha:dd/MM/yyyy HH:mm}\nAtención {d.IdAtencion:D6}\nCliente: {d.Cliente}\nDocumento: {d.Documento}\nVehículo: {d.Dominio}\n\n");
        foreach (var i in d.Items) texto.AppendLine($"{i.Descripcion}\n  {i.Cantidad:N3} × {i.PrecioUnitario:N2} = {i.Subtotal:N2}\n");
        texto.AppendLine($"TOTAL LIQUIDADO ({d.Moneda}): {d.Total:N2}\n\nObservaciones: {d.Observaciones}\n\nEstado de cuenta al consultar:\nPagos vigentes: {d.Pagado:N2}\nSaldo pendiente: {d.Saldo:N2}\n");
        texto.AppendLine("Este documento detalla lo liquidado. Cada cobro se acredita mediante su recibo interno.");
        return texto.ToString();
    }
    public static string Recibo(ComprobanteDto d, PagoConsultaDto p) =>
        $"SGTM · TALLER MECÁNICO\nRECIBO INTERNO · SIN VALIDEZ FISCAL\n{p.Numero} · {p.Fecha:dd/MM/yyyy HH:mm}\n" +
        (p.Anulado ? $"ANULADO · Motivo: {p.MotivoAnulacion}\n" : "") +
        $"Liquidación {d.Numero} · Atención {d.IdAtencion:D6}\nCliente: {d.Cliente}\nDocumento: {d.Documento}\nVehículo: {d.Dominio}\n\n" +
        $"Importe recibido ({d.Moneda}): {p.Importe:N2}\nMedios y referencias: {p.Medios}\nRegistró: {p.Usuario}\nSaldo al registrar este cobro: {p.SaldoPosterior:N2}\n\n" +
        "El saldo indicado corresponde al momento de este cobro. Consulte la liquidación para conocer el saldo actual.";
}
