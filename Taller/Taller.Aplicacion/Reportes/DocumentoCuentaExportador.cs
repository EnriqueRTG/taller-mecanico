using System.Globalization;
using Taller.Aplicacion.DTOs.Cobros;
using Taller.Aplicacion.DTOs.Reportes;
namespace Taller.Aplicacion.Reportes;
/// <summary>Copias de consulta: separa deuda liquidada y cobro recibido, sin alterar la cuenta.</summary>
public static class DocumentoCuentaExportador
{
    private static string N(decimal n) => n.ToString("N2", CultureInfo.GetCultureInfo("es-AR"));
    private static string Identidad(ComprobanteDto d) => $"Atención {d.IdAtencion:D6} · Dominio: {d.Dominio}\nCliente: {d.Cliente}\nDocumento: {d.Documento}\n";
    public static DocumentoComercialDto LiquidacionEstructurada(ComprobanteDto d) => new(
        "LIQUIDACIÓN DE TRABAJOS REALIZADOS", $"{d.Numero} · Emitida {d.Fecha:dd/MM/yyyy HH:mm}", Identidad(d),
        ["Concepto realizado", "Cantidad", "Precio unitario ARS", "Subtotal ARS"],
        d.Items.Select(i => (IReadOnlyList<string>)new[] { i.Descripcion, i.Cantidad.ToString("N3", CultureInfo.GetCultureInfo("es-AR")), N(i.PrecioUnitario), N(i.Subtotal) }).ToList(),
        [new("TOTAL LIQUIDADO · " + d.Moneda, N(d.Total)), new("Cobros vigentes · " + d.Moneda, N(d.Pagado)), new("Saldo pendiente · " + d.Moneda, N(d.Saldo))],
        (string.IsNullOrWhiteSpace(d.Observaciones) ? "" : "Observaciones: " + d.Observaciones + "\n\n") + "Detalla lo realizado y el importe a abonar. No acredita un pago: cada cobro tiene su propio recibo.");

    public static DocumentoComercialDto ReciboEstructurado(ComprobanteDto d, PagoConsultaDto p) => new(
        p.Anulado ? "RECIBO ANULADO · NO COMPUTA COMO COBRO" : "RECIBO DE COBRO",
        $"{p.Numero} · Recibido {p.Fecha:dd/MM/yyyy HH:mm} · Liquidación {d.Numero}", Identidad(d),
        ["Medio de pago / referencia", "Importe ARS"],
        p.DetalleMedios.Count > 0
            ? p.DetalleMedios.Select(m => (IReadOnlyList<string>)new[] { m.Nombre + (string.IsNullOrWhiteSpace(m.Referencia) ? "" : "\nReferencia: " + m.Referencia), N(m.Importe) }).ToList()
            : new List<IReadOnlyList<string>> { new[] { p.Medios, N(p.Importe) } },
        [new("IMPORTE DE ESTE COBRO · " + d.Moneda, N(p.Importe)), new("Saldo al registrar el cobro · " + d.Moneda, N(p.SaldoPosterior))],
        (p.Anulado ? "Motivo de anulación: " + p.MotivoAnulacion + "\n" : "") + "Registró el cobro: " + p.Usuario
        + "\nEl saldo indicado es histórico; consulte la cuenta para conocer el saldo actual. El recibo no registra la entrega del vehículo.");

}
