using System.ComponentModel;
using Taller.Aplicacion.DTOs.Cobros;
using Taller.Aplicacion.Servicios;
namespace Taller.Presentacion.Formularios.Comprobantes;
/// <summary>Registra un cobro parcial o total compuesto por uno o varios medios, en una sola transacción.</summary>
public partial class FrmRegistrarPago : Form
{
    private readonly ComprobanteDto _documento; private readonly CobroServicio _servicio;
    private readonly Guid _operacion = Guid.NewGuid(); private readonly BindingList<PagoMedioDto> _medios = [];
    private bool _ocupado, _cargando = true;
    public FrmRegistrarPago(ComprobanteDto documento, CobroServicio servicio) { InitializeComponent(); _documento = documento; _servicio = servicio; }
    private async void Cargar(object? s, EventArgs e)
    {
        tlpPrincipal.Enabled = false;
        try
        {
            lblResumen.Text = $"{_documento.Numero} · {_documento.Cliente} · {_documento.Dominio}\nSaldo pendiente: {_documento.Saldo:N2}. No se admite sobrepago.";
            cboMedio.DisplayMember = nameof(MedioPagoDto.Nombre); cboMedio.DataSource = await _servicio.ListarMediosAsync(); dgvMedios.DataSource = _medios; ActualizarTotal();
        }
        catch (Exception ex) { MessageBox.Show(this, ex.Message); _cargando = false; Close(); }
        finally { _cargando = false; tlpPrincipal.Enabled = true; }
    }
    private void MedioCambiado(object? s, EventArgs e) => lblReferencia.Text = cboMedio.SelectedItem is MedioPagoDto { RequiereReferencia: true } ? "Referencia de la operación (obligatoria)" : "Referencia u observación (opcional)";
    private void Agregar(object? s, EventArgs e)
    {
        ValidateChildren(); if (cboMedio.SelectedItem is not MedioPagoDto m) return;
        if (nudImporte.Value <= 0 || _medios.Sum(x => x.Importe) + nudImporte.Value > _documento.Saldo) { MessageBox.Show(this, "Ingrese un importe positivo sin superar el saldo."); return; }
        if (m.RequiereReferencia && string.IsNullOrWhiteSpace(txtReferencia.Text)) { MessageBox.Show(this, "Ingrese la referencia de la operación."); return; }
        _medios.Add(new PagoMedioDto { IdMedioPago = m.IdMedioPago, Nombre = m.Nombre, Importe = nudImporte.Value, Referencia = txtReferencia.Text.Trim() });
        nudImporte.Value = 0; txtReferencia.Clear(); ActualizarTotal();
    }
    private void Quitar(object? s, EventArgs e) { if (dgvMedios.CurrentRow?.DataBoundItem is PagoMedioDto m) _medios.Remove(m); ActualizarTotal(); }
    private void ActualizarTotal() { var total = _medios.Sum(x => x.Importe); lblTotal.Text = $"Este cobro: {total:N2} · Saldo posterior: {_documento.Saldo - total:N2}"; btnGuardar.Enabled = total > 0 && total <= _documento.Saldo; }
    private async void Guardar(object? s, EventArgs e)
    {
        if (_ocupado || _medios.Count == 0) return;
        if (MessageBox.Show(this, $"¿Confirma haber recibido {_medios.Sum(x => x.Importe):N2} por los medios listados? Se emitirá un recibo interno.", "Registrar cobro", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        _ocupado = true; tlpPrincipal.Enabled = false;
        try { await _servicio.RegistrarPagoAsync(new RegistrarPagoDto { IdOperacion = _operacion, IdComprobante = _documento.IdComprobante, Version = _documento.Version, Medios = _medios.ToList() }); DialogResult = DialogResult.OK; }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "Cobro", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        finally { _ocupado = false; tlpPrincipal.Enabled = true; }
        if (DialogResult == DialogResult.OK) Close();
    }
    private void Cerrar(object? s, EventArgs e) => Close();
    private void AlCerrar(object? s, FormClosingEventArgs e) { if (_ocupado || _cargando) { e.Cancel = true; return; } if (DialogResult != DialogResult.OK && _medios.Count > 0 && MessageBox.Show(this, "¿Descartar los medios preparados sin registrar el cobro?", "Cerrar", MessageBoxButtons.YesNo) != DialogResult.Yes) e.Cancel = true; }
}
