using Taller.Aplicacion.DTOs.Cobros;
using Taller.Aplicacion.Servicios;
using Taller.Aplicacion.Excepciones;
using Taller.Dominio.Enumeraciones;
namespace Taller.Presentacion.Formularios.Comprobantes;
/// <summary>Único recorrido visual de entrega, invocable desde Atenciones o desde la cuenta.</summary>
internal static class EntregaAtencionPresentador
{
    public static async Task<bool> MostrarAsync(IWin32Window propietario, int id, CobroServicio servicio)
    {
        var cuenta = await servicio.ObtenerAsync(id);
        if (!cuenta.PuedeRegistrarEntrega) throw new ValidacionException("La entrega requiere cierre técnico y liquidación saldada. Actualice la atención.");
        using var f = new FrmRegistrarEntrega(cuenta.Comprobante!.Cliente,
            cuenta.Estado == EstadoAtencion.TrabajoFinalizado ? "Trabajos finalizados con registro técnico." : "Cierre técnico con liquidación de lo realizado.",
            cuenta.Resumen);
        if (f.ShowDialog(propietario) != DialogResult.OK) return false;
        await servicio.EntregarAsync(new RegistrarEntregaDto { IdAtencion = id, Version = cuenta.VersionAtencion,
            PersonaRetira = f.PersonaRetira, ConformidadRecepcion = f.Conformidad, Observaciones = f.Observaciones });
        MessageBox.Show(propietario, $"Atención {id:D6} · Dominio {cuenta.Comprobante.Dominio}\nVehículo entregado. Atención cerrada. Los documentos y el historial permanecen disponibles para consulta.",
            "Entrega registrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return true;
    }
}
