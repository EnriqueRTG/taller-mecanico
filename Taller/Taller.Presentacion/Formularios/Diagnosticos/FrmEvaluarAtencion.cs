using System.ComponentModel;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Enumeraciones;
namespace Taller.Presentacion.Formularios.Diagnosticos;
/// <summary>Registra un avance técnico o finaliza el informe y su propuesta. Cerrar no libera la actividad.</summary>
public partial class FrmEvaluarAtencion : Form
{
    private readonly EvaluacionServicio _servicio;
    private readonly int _id;
    private EvaluacionAtencionDto? _datos;
    private List<ConceptoPropuestaDto> _catalogo = [];
    private BindingList<ItemPropuestaDto> _items = [];
    private BindingList<ServicioEvaluadoDto> _iniciales = [];
    private bool _cargando = true, _ocupado, _sucio;
    public bool Finalizada { get; private set; }
    public FrmEvaluarAtencion(int id, EvaluacionServicio servicio)
    { InitializeComponent(); _id = id; _servicio = servicio; }
    private async void Cargar(object? s, EventArgs e)
    {
        try {
            _catalogo = await _servicio.ListarCatalogoAsync();
            CargarDatos(await _servicio.ObtenerAsync(_id));
            FiltrarCatalogo(null, EventArgs.Empty);
        } catch (Exception ex) { MostrarError(ex); Close(); }
    }
    private void CargarDatos(EvaluacionAtencionDto d)
    {
        _cargando = true; _datos = d;
        lblResumen.Text = $"Atención {d.IdAtencion:D6} · {d.Cliente} · {d.Vehiculo}\nSolicitud: {d.TipoSolicitud} · {d.Motivo}";
        chkInforme.Checked = d.IncluyeDiagnostico;
        chkInforme.Enabled = d.TipoSolicitud == TipoSolicitudAtencion.ServicioDefinido && !d.InformeHistorico;
        txtHallazgo.ReadOnly = d.InformeHistorico; txtTrabajo.ReadOnly = d.InformeHistorico;
        nudKm.Enabled = !d.InformeHistorico; chkKm.Enabled = !d.InformeHistorico;
        chkSinPropuesta.Checked = d.SoloEvaluacion;
        txtHallazgo.Text = d.FallaDetectada; txtTrabajo.Text = d.TrabajoRecomendado;
        txtObservaciones.Text = d.Observaciones; nudKm.Value = d.Kilometraje ?? 0; chkKm.Checked = d.Kilometraje.HasValue;
        _items = new BindingList<ItemPropuestaDto>(d.Items); dgvItems.DataSource = _items;
        _iniciales = new BindingList<ServicioEvaluadoDto>(d.ServiciosIniciales); dgvIniciales.DataSource = _iniciales;
        foreach (DataGridViewRow fila in dgvIniciales.Rows)
            if (fila.DataBoundItem is ServicioEvaluadoDto servicio && servicio.Estado != EstadoEjecucionDetalle.NoIniciado)
                fila.ReadOnly = true;
        _cargando = false; _sucio = false; ActualizarTotal();
    }
    private void Cambiado(object? s, EventArgs e)
    {
        txtHallazgo.Enabled = chkInforme.Checked;
        txtTrabajo.Enabled = chkInforme.Checked;
        if (!_cargando) _sucio = true;
    }
    private void FiltrarCatalogo(object? s, EventArgs e)
    {
        var q = txtBuscar.Text.Trim();
        cboConcepto.DisplayMember = nameof(ConceptoPropuestaDto.Etiqueta);
        cboConcepto.DataSource = _catalogo.Where(c => ($"{c.Nombre} {c.Categoria} {c.Descripcion}").Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
    }
    private void ConceptoCambiado(object? s, EventArgs e)
    {
        if (cboConcepto.SelectedItem is not ConceptoPropuestaDto c) return;
        txtDescripcion.Text = c.Nombre; nudPrecio.Value = Math.Min(nudPrecio.Maximum, c.PrecioReferencia ?? 0);
        lblPrecio.Text = c.PrecioReferencia.HasValue ? "Precio de referencia: revise su vigencia." : "Sin precio de referencia: establezca el importe (0 si es sin cargo).";
    }
    private void Agregar(object? s, EventArgs e)
    {
        ValidateChildren();
        if (cboConcepto.SelectedItem is not ConceptoPropuestaDto c) return;
        if (_items.Any(i => i.IdConcepto == c.IdConcepto)) { MessageBox.Show(this, "El concepto ya está agregado. Quite la fila para reemplazarlo."); return; }
        if (string.IsNullOrWhiteSpace(txtDescripcion.Text)) return;
        _items.Add(new ItemPropuestaDto { IdConcepto = c.IdConcepto, Descripcion = txtDescripcion.Text.Trim(), Cantidad = nudCantidad.Value, PrecioUnitario = nudPrecio.Value });
        _sucio = true; ActualizarTotal();
    }
    private void Quitar(object? s, EventArgs e)
    {
        if (dgvItems.CurrentRow?.DataBoundItem is ItemPropuestaDto i) { _items.Remove(i); _sucio = true; ActualizarTotal(); }
    }
    private void ActualizarTotal() => lblTotal.Text = $"Propuesta: {_items.Sum(i => i.Subtotal):N2}. Los servicios iniciales se muestran por separado y no se duplican aquí.";
    private void InicialModificado(object? s, DataGridViewCellEventArgs e) { if (!_cargando && e.RowIndex >= 0) _sucio = true; }
    private void ErrorGrilla(object? s, DataGridViewDataErrorEventArgs e) { e.ThrowException = false; MessageBox.Show(this, "Seleccione un estado válido de ejecución."); }
    private async void Guardar(object? s, EventArgs e) => await PersistirAsync(false);
    private async void Finalizar(object? s, EventArgs e) => await PersistirAsync(true);
    private async Task PersistirAsync(bool finalizar)
    {
        if (_ocupado || _datos is null) return;
        ValidateChildren(); dgvIniciales.EndEdit();
        if (dgvIniciales.DataSource is not null) BindingContext?[dgvIniciales.DataSource]?.EndCurrentEdit();
        if (finalizar && MessageBox.Show(this, "¿Finalizar la evaluación? El informe quedará como histórico y la propuesta lista para que Recepción la comunique. Esto no autoriza reparar.", "Finalizar evaluación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        _ocupado = true; tlpPrincipal.Enabled = false;
        try {
            _datos.IncluyeDiagnostico = chkInforme.Checked; _datos.SoloEvaluacion = chkSinPropuesta.Checked;
            _datos.FallaDetectada = txtHallazgo.Text; _datos.TrabajoRecomendado = txtTrabajo.Text;
            _datos.Observaciones = txtObservaciones.Text; _datos.Kilometraje = chkKm.Checked ? (int)nudKm.Value : null;
            _datos.Items = _items.ToList(); _datos.ServiciosIniciales = _iniciales.ToList();
            await _servicio.GuardarAsync(_datos, finalizar);
            _sucio = false;
            if (finalizar) { Finalizada = true; DialogResult = DialogResult.OK; }
            else { CargarDatos(await _servicio.ObtenerAsync(_id)); lblAviso.Text = "Avance guardado. Puede cerrar y continuar luego; la reserva sigue a su nombre."; }
        } catch (Exception ex) { MostrarError(ex); }
        finally { _ocupado = false; tlpPrincipal.Enabled = true; }
        if (Finalizada) Close();
    }
    private void Cerrar(object? s, EventArgs e) => Close();
    private void AlCerrar(object? s, FormClosingEventArgs e)
    {
        if (_ocupado) { e.Cancel = true; return; }
        if (_sucio && MessageBox.Show(this, "Hay cambios sin guardar. ¿Cerrar y descartarlos? Los avances guardados y la reserva se conservan.", "Cerrar evaluación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) e.Cancel = true;
    }
    private void MostrarError(Exception ex) => MessageBox.Show(this, ex.Message, "Evaluación técnica", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
