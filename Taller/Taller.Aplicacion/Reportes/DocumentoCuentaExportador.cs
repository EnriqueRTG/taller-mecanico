using System.Globalization;
using System.Text;
using Taller.Aplicacion.DTOs.Cobros;
namespace Taller.Aplicacion.Reportes;
/// <summary>Copias de consulta: separa deuda liquidada y cobro recibido, sin alterar la cuenta.</summary>
public static class DocumentoCuentaExportador
{
    private static string N(decimal n) => n.ToString("N2", CultureInfo.GetCultureInfo("es-AR"));
    private static string Identidad(ComprobanteDto d) => $"Atención {d.IdAtencion:D6} · Dominio: {d.Dominio}\nCliente: {d.Cliente}\nDocumento: {d.Documento}\n";
    public static string Liquidacion(ComprobanteDto d)
    {
        var t = new StringBuilder($"LIQUIDACIÓN DE TRABAJOS REALIZADOS\n{d.Numero} · Emitida {d.Fecha:dd/MM/yyyy HH:mm}\n\n");
        t.AppendLine(Identidad(d)).AppendLine("CONCEPTOS LIQUIDADOS\n");
        foreach (var i in d.Items) t.AppendLine($"{i.Descripcion}\nCantidad realizada: {i.Cantidad.ToString("N3", CultureInfo.GetCultureInfo("es-AR"))} · Precio acordado: {N(i.PrecioUnitario)} {d.Moneda}\nSubtotal: {N(i.Subtotal)} {d.Moneda}\n");
        t.AppendLine($"TOTAL LIQUIDADO: {N(d.Total)} {d.Moneda}\n\nCUENTA AL MOMENTO DE LA CONSULTA\nCobros vigentes: {N(d.Pagado)} {d.Moneda}\nSaldo pendiente: {N(d.Saldo)} {d.Moneda}");
        if (!string.IsNullOrWhiteSpace(d.Observaciones)) t.AppendLine("\nOBSERVACIONES\n" + d.Observaciones);
        return t.AppendLine("\nDetalla lo realizado y el importe a abonar. No acredita un pago: cada cobro tiene su propio recibo.\nDocumento interno sin validez fiscal.").ToString();
    }
    public static string Recibo(ComprobanteDto d, PagoConsultaDto p)
    {
        var t = new StringBuilder($"RECIBO DE COBRO\n{p.Numero} · Recibido {p.Fecha:dd/MM/yyyy HH:mm}\n");
        if (p.Anulado) t.AppendLine("ANULADO · NO COMPUTA COMO COBRO VIGENTE\nMotivo: " + p.MotivoAnulacion);
        t.AppendLine($"Liquidación asociada: {d.Numero}\n").AppendLine(Identidad(d));
        t.AppendLine($"IMPORTE DE ESTE COBRO: {N(p.Importe)} {d.Moneda}\n\nMEDIOS DE PAGO Y REFERENCIAS");
        if (p.DetalleMedios.Count > 0)
            foreach (var m in p.DetalleMedios)
            {
                t.AppendLine($"{m.Nombre}: {N(m.Importe)} {d.Moneda}");
                if (!string.IsNullOrWhiteSpace(m.Referencia)) t.AppendLine("Referencia de la operación: " + m.Referencia);
                t.AppendLine();
            }
        else t.AppendLine(p.Medios);
        if (!string.IsNullOrWhiteSpace(p.Usuario)) t.AppendLine("Registró el cobro: " + p.Usuario);
        t.AppendLine($"\nSALDO AL REGISTRAR ESTE COBRO: {N(p.SaldoPosterior)} {d.Moneda}");
        return t.AppendLine("\nEste saldo es histórico: consulte la liquidación para conocer el saldo actual.\nEl recibo no registra la entrega del vehículo.\nDocumento interno sin validez fiscal.").ToString();
    }
}
