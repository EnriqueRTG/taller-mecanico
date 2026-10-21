using Taller.Aplicacion.DTOs.Cobros;
using Taller.Aplicacion.Servicios;
using Taller.Presentacion.Formularios.Compartidos;
using Taller.Presentacion.Impresion;
using Taller.Dominio.Enumeraciones;
namespace Taller.Presentacion.Formularios.Comprobantes;
/// <summary>Gestiona documentos económicos reales y pagos, conservando toda su historia.</summary>
public partial class FrmComprobantePagos : Form
{
    private readonly int _id; private readonly CobroServicio _servicio; private CuentaAtencionDto? _datos;
    private bool _ocupado, _cargando;
    public FrmComprobantePagos(int id, CobroServicio servicio) { InitializeComponent(); _id = id; _servicio = servicio; }
    private async void Cargar(object? s, EventArgs e) => await RecargarAsync();
    private async Task RecargarAsync()
    {
        _cargando = true; tlpPrincipal.Enabled = false;
        try
        {
            _datos = await _servicio.ObtenerAsync(_id); var d = _datos.Comprobante; lblResumen.Text = _datos.Resumen;
            dgvItems.DataSource = d?.Items ?? _datos.ItemsPorLiquidar; dgvPagos.DataSource = d?.Pagos;
            if (d != null) txtObservaciones.Text = d.Observaciones; txtObservaciones.ReadOnly = d != null;
            lblSaldo.Text = d == null ? $"Por liquidar: {_datos.TotalPorLiquidar:N2}. Revise los conceptos antes de emitir." : $"{d.Numero} · Total {d.Total:N2} · Pagado {d.Pagado:N2} · Saldo {d.Saldo:N2}";
            var abierta = _datos.Estado is EstadoAtencion.TrabajoFinalizado or EstadoAtencion.PendienteLiquidacion;
            btnEmitir.Enabled = abierta && d == null; btnPago.Enabled = abierta && d is { Saldo: > 0 }; btnEntregar.Enabled = abierta && d is { Saldo: 0 };
            btnImprimir.Enabled = d != null; Seleccion(null, EventArgs.Empty);
        }
        catch (Exception ex) { Error(ex); }
        finally { _cargando = false; tlpPrincipal.Enabled = true; }
    }
    private PagoConsultaDto? PagoSeleccionado => dgvPagos.CurrentRow?.DataBoundItem as PagoConsultaDto;
    private void Seleccion(object? s, EventArgs e)
    {
        btnRecibo.Enabled = PagoSeleccionado != null;
        btnAnular.Enabled = PagoSeleccionado is { Anulado: false } && _datos?.Estado is EstadoAtencion.TrabajoFinalizado or EstadoAtencion.PendienteLiquidacion;
    }
    private async Task OperarAsync(Func<Task> accion)
    {
        if (_ocupado) return; _ocupado = true; tlpPrincipal.Enabled = false;
        try { await accion(); await RecargarAsync(); }
        catch (Exception ex) { Error(ex); }
        finally { _ocupado = false; tlpPrincipal.Enabled = true; }
    }
    private async void Emitir(object? s, EventArgs e)
    {
        if (_datos == null || _ocupado) return;
        if (MessageBox.Show(this, $"¿Emitir liquidación interna por {_datos.TotalPorLiquidar:N2}? Se conservarán los conceptos e importes; no podrá editarlos después de emitir.", "Liquidación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        await OperarAsync(async () => { await _servicio.LiquidarAsync(_id, _datos.VersionAtencion, txtObservaciones.Text); });
    }
    private async void RegistrarPago(object? s, EventArgs e)
    {
        if (_datos?.Comprobante is not { } d || _ocupado) return;
        using var f = new FrmRegistrarPago(d, _servicio); f.ShowDialog(this); await RecargarAsync();
    }
    private async void Anular(object? s, EventArgs e)
    {
        if (_datos?.Comprobante is not { } d || PagoSeleccionado is not { } pago || _ocupado) return;
        using var f = new FrmSolicitarMotivo("Anular registro de pago", $"Recibo {pago.Numero} por {pago.Importe:N2}. La anulación conserva el original y repone el saldo. Registra una corrección administrativa; no realiza una devolución bancaria.");
        if (f.ShowDialog(this) == DialogResult.OK) await OperarAsync(() => _servicio.AnularPagoAsync(d.IdComprobante, pago.IdPago, d.Version, f.Motivo));
    }
    private async void Entregar(object? s, EventArgs e)
    {
        if (_datos == null || _ocupado) return;
        using var f = new FrmSolicitarMotivo("Registrar entrega del vehículo", "Indique la condición de entrega, el resultado del trabajo y quién retira. Solo se permite entregar con saldo exactamente cero.");
        if (f.ShowDialog(this) == DialogResult.OK) await OperarAsync(() => _servicio.EntregarAsync(_id, _datos.VersionAtencion, f.Motivo));
    }
    private async void Imprimir(object? s, EventArgs e)
    {
        if (_ocupado) return;
        await OperarAsync(async () => { var cuenta = await _servicio.ObtenerAsync(_id); if (cuenta.Comprobante is { } d) ImpresionDocumento.Mostrar(this, TextoDocumentos.Liquidacion(d)); });
    }
    private async void Recibo(object? s, EventArgs e)
    {
        if (PagoSeleccionado is not { } seleccionado || _ocupado) return;
        await OperarAsync(async () => { var cuenta = await _servicio.ObtenerAsync(_id); if (cuenta.Comprobante is { } d && d.Pagos.SingleOrDefault(p => p.IdPago == seleccionado.IdPago) is { } p) ImpresionDocumento.Mostrar(this, TextoDocumentos.Recibo(d, p)); });
    }
    private async void Actualizar(object? s,EventArgs e) { if(!_ocupado&&!_cargando) await RecargarAsync(); }
    private void Cerrar(object? s, EventArgs e) => Close();
    private void AlCerrar(object? s, FormClosingEventArgs e) { if (_ocupado || _cargando) e.Cancel = true; }
    private void Error(Exception ex) => MessageBox.Show(this, ex.Message, "Liquidación y cobros", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
