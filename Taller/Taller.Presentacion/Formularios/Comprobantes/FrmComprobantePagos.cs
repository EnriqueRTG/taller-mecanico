using Taller.Aplicacion.DTOs.Cobros;
using Taller.Aplicacion.Servicios;
using Taller.Presentacion.Formularios.Compartidos;
using Taller.Presentacion.Impresion;
using Taller.Dominio.Enumeraciones;

namespace Taller.Presentacion.Formularios.Comprobantes;
/// <summary>Gestiona documentos económicos reales y pagos, conservando toda su historia.</summary>
public partial class FrmComprobantePagos : Form
{
    private readonly int _id;
    private readonly CobroServicio _servicio;
    private readonly Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto _empresa;
    private CuentaAtencionDto? _datos;
    private bool _ocupado, _cargando;
    public FrmComprobantePagos(int id, CobroServicio servicio, Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto empresa)
    {
        InitializeComponent();
        Taller.Presentacion.Estilos.CabeceraOperacion.Aplicar(lblResumen);
        Button[] orden = [btnEmitir, btnPago, btnEntregar, btnImprimir, btnRecibo, btnActualizar, btnAnular, btnCerrar];
        for (int i = 0; i < orden.Length; i++) flpAcciones.Controls.SetChildIndex(orden[i], i);
        btnAnular.Margin = new Padding(20, 3, 3, 3);
        _id = id;
        _servicio = servicio;
        _empresa = empresa;
    }

    private async void Cargar(object? s, EventArgs e) { Taller.Presentacion.Estilos.DisposicionVentanas.AjustarAlAreaDisponible(this); await RecargarAsync(); }
    private async Task RecargarAsync()
    {
        _cargando = true;
        tlpPrincipal.Enabled = false;
        try
        {
            _datos = await _servicio.ObtenerAsync(_id);
            var d = _datos.Comprobante;
            lblResumen.Text = _datos.Resumen.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
            dgvItems.DataSource = d?.Items ?? _datos.ItemsPorLiquidar;
            int? pagoAnterior = PagoSeleccionado?.IdPago;
            dgvPagos.DataSource = d?.Pagos;
            dgvPagos.ClearSelection();
            dgvPagos.CurrentCell = null;
            var filaPago = dgvPagos.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.DataBoundItem is PagoConsultaDto p && p.IdPago == pagoAnterior)
                ?? dgvPagos.Rows.Cast<DataGridViewRow>().FirstOrDefault();
            if (filaPago != null) { dgvPagos.CurrentCell = filaPago.Cells[0]; filaPago.Selected = true; }
            if (d != null)
                txtObservaciones.Text = d.Observaciones;
            txtObservaciones.ReadOnly = d != null;
            Text = d == null ? "Liquidar trabajos realizados" : d.Saldo > 0 ? "Registrar pagos" : _datos.Estado == EstadoAtencion.Cerrada ? "Cuenta e historial de entrega" : "Registrar entrega del vehículo";
            lblSaldo.Text = d == null ? $"Por liquidar: {_datos.TotalPorLiquidar:N2}. Revise los conceptos antes de emitir." : $"{d.Numero} · Total {d.Total:N2} · Pagado {d.Pagado:N2} · Saldo {d.Saldo:N2}";
            var abierta = _datos.Estado is EstadoAtencion.TrabajoFinalizado or EstadoAtencion.PendienteLiquidacion;
            btnEmitir.Visible = abierta && d == null;
            btnPago.Visible = abierta && d is { Saldo: > 0 };
            btnEntregar.Visible = _datos.PuedeRegistrarEntrega;
            btnImprimir.Visible = d != null;
            btnRecibo.Visible = d?.Pagos.Count > 0;
            btnAnular.Visible = abierta && d?.Pagos.Any(p => !p.Anulado) == true;
            btnEmitir.Enabled = abierta && d == null;
            btnPago.Enabled = abierta && d is { Saldo: > 0 };
            btnEntregar.Enabled = _datos.PuedeRegistrarEntrega;
            btnImprimir.Enabled = d != null;
            Seleccion(null, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            Error(ex);
        }
        finally
        {
            _cargando = false;
            tlpPrincipal.Enabled = true;
        }
    }

    private PagoConsultaDto? PagoSeleccionado => dgvPagos.CurrentRow?.DataBoundItem as PagoConsultaDto ?? _datos?.Comprobante?.Pagos.FirstOrDefault();

    private void Seleccion(object? s, EventArgs e)
    {
        btnRecibo.Enabled = PagoSeleccionado != null;
        btnAnular.Visible = _datos?.Comprobante?.Pagos.Any(p => !p.Anulado) == true && (_datos.Estado is EstadoAtencion.TrabajoFinalizado or EstadoAtencion.PendienteLiquidacion);
        btnAnular.Enabled = PagoSeleccionado is { Anulado: false } && (_datos?.Estado is EstadoAtencion.TrabajoFinalizado or EstadoAtencion.PendienteLiquidacion);
    }

    private async Task OperarAsync(Func<Task> accion)
    {
        if (_ocupado)
            return;
        _ocupado = true;
        tlpPrincipal.Enabled = false;
        try
        {
            await accion();
            await RecargarAsync();
        }
        catch (Exception ex)
        {
            Error(ex);
        }
        finally
        {
            _ocupado = false;
            tlpPrincipal.Enabled = true;
        }
    }

    private async void Emitir(object? s, EventArgs e)
    {
        if (_datos == null || _ocupado)
            return;
        if (MessageBox.Show(this, $"¿Emitir liquidación interna por {_datos.TotalPorLiquidar:N2}? Se conservarán los conceptos e importes; no podrá editarlos después de emitir.", "Liquidación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        await OperarAsync(async () =>
        {
            int idDocumento = await _servicio.LiquidarAsync(_id, _datos.VersionAtencion, txtObservaciones.Text);
            MessageBox.Show(this, $"{_datos.Resumen}\nLiquidación LIQ-{idDocumento:D8} emitida. No acredita pagos. Registre los cobros cuando se reciban.", "Liquidación emitida", MessageBoxButtons.OK, MessageBoxIcon.Information);
        });
    }

    private async void RegistrarPago(object? s, EventArgs e)
    {
        if (_datos?.Comprobante is not { } d || _ocupado)
            return;
        using var f = new FrmRegistrarPago(d, _servicio);
        f.ShowDialog(this);
        await RecargarAsync();
    }

    private async void Anular(object? s, EventArgs e)
    {
        if (_datos?.Comprobante is not { } d || PagoSeleccionado is not { } pago || _ocupado)
            return;
        using var f = new FrmSolicitarMotivo("Anular registro de pago", $"Recibo {pago.Numero} por {pago.Importe:N2}. La anulación conserva el original y repone el saldo. Registra una corrección administrativa; no realiza una devolución bancaria.");
        if (f.ShowDialog(this) == DialogResult.OK)
            await OperarAsync(() => _servicio.AnularPagoAsync(d.IdComprobante, pago.IdPago, d.Version, f.Motivo));
    }

    private async void Entregar(object? s, EventArgs e)
    {
        if (_datos == null || _ocupado)
            return;
        _ocupado = true;
        try { await EntregaAtencionPresentador.MostrarAsync(this, _id, _servicio); }
        catch (Exception ex) { Error(ex); }
        finally { _ocupado = false; await RecargarAsync(); }
    }

    private async void Imprimir(object? s, EventArgs e)
    {
        if (_ocupado)
            return;
        await OperarAsync(async () =>
        {
            var cuenta = await _servicio.ObtenerAsync(_id);
            if (cuenta.Comprobante is { } d)
                ImpresionDocumento.Mostrar(this, TextoDocumentos.Liquidacion(d), _empresa, Taller.Aplicacion.Reportes.DocumentoReporteExportador.NombreArchivo("Liquidacion-" + d.Numero, d.Fecha));
        });
    }

    private async void Recibo(object? s, EventArgs e)
    {
        if (PagoSeleccionado is not { } seleccionado || _ocupado)
            return;
        await OperarAsync(async () =>
        {
            var cuenta = await _servicio.ObtenerAsync(_id);
            if (cuenta.Comprobante is { } d && d.Pagos.SingleOrDefault(p => p.IdPago == seleccionado.IdPago)is { } p)
                ImpresionDocumento.Mostrar(this, TextoDocumentos.Recibo(d, p), _empresa, Taller.Aplicacion.Reportes.DocumentoReporteExportador.NombreArchivo("Recibo-" + p.Numero, p.Fecha));
        });
    }

    private async void Actualizar(object? s, EventArgs e)
    {
        if (!_ocupado && !_cargando)
            await RecargarAsync();
    }

    private void Cerrar(object? s, EventArgs e) => Close();
    private void AlCerrar(object? s, FormClosingEventArgs e)
    {
        if (_ocupado || _cargando)
            e.Cancel = true;
    }

    private void Error(Exception ex) => Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Liquidación y cobros");
}
