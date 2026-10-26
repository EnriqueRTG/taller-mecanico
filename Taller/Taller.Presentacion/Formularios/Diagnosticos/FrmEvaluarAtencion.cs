using System.ComponentModel;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Enumeraciones;

namespace Taller.Presentacion.Formularios.Diagnosticos;
/// <summary>Registra un avance técnico o finaliza el informe y su propuesta. Cerrar no libera la actividad.</summary>
public partial class FrmEvaluarAtencion : Form
{
    private readonly IServiceProvider _proveedor;
    private readonly EvaluacionServicio _servicio;
    private readonly int _id;
    private EvaluacionAtencionDto? _datos;
    private List<ConceptoPropuestaDto> _catalogo = [];
    private BindingList<ItemPropuestaDto> _items = [];
    private BindingList<ServicioEvaluadoDto> _iniciales = [];
    private bool _cargando = true, _ocupado, _sucio;
    public bool Finalizada { get; private set; }

    public FrmEvaluarAtencion(int id, EvaluacionServicio servicio, IServiceProvider proveedor)
    {
        InitializeComponent();
        Taller.Presentacion.Estilos.CabeceraOperacion.Aplicar(txtResumen);
        _proveedor = proveedor;
        _id = id;
        _servicio = servicio;
    }

    private async void Cargar(object? s, EventArgs e)
    {
        Taller.Presentacion.Estilos.DisposicionVentanas.AjustarAlAreaDisponible(this);
        try
        {
            _catalogo = await _servicio.ListarCatalogoAsync();
            CargarDatos(await _servicio.ObtenerAsync(_id));
            FiltrarCatalogo(null, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            MostrarError(ex);
            Close();
        }
    }

    private void CargarDatos(EvaluacionAtencionDto d)
    {
        _cargando = true;
        _datos = d;
        var pedidos = d.Solicitudes.Count == 0 ? d.Motivo : string.Join(" | ", d.Solicitudes.Select(s => s.Etiqueta));
        txtResumen.Text = $"Atención {d.IdAtencion:D6} · {d.Cliente} · {d.Vehiculo}{Environment.NewLine}{pedidos}"
            + (string.IsNullOrWhiteSpace(d.MotivoRevision) ? "" : $"{Environment.NewLine}Revisión solicitada: {d.MotivoRevision}")
            + (string.IsNullOrWhiteSpace(d.ObservacionesRecepcion) ? "" : Environment.NewLine + "Ingreso: " + d.ObservacionesRecepcion);
        cboSolicitud.DisplayMember = nameof(SolicitudIngresoDto.Etiqueta);
        cboSolicitud.DataSource = d.Solicitudes;
        cboSolicitud.Visible = d.Solicitudes.Count > 0;
        chkInforme.Checked = d.IncluyeDiagnostico;
        chkInforme.Visible = d.TipoSolicitud == TipoSolicitudAtencion.ServicioDefinido;
        chkInforme.Enabled = d.TipoSolicitud == TipoSolicitudAtencion.ServicioDefinido && !d.InformeHistorico;
        txtHallazgo.ReadOnly = d.InformeHistorico;
        txtTrabajo.ReadOnly = d.InformeHistorico;
        nudKm.Enabled = !d.InformeHistorico;
        chkKm.Enabled = !d.InformeHistorico;
        chkSinPropuesta.Checked = d.SoloEvaluacion;
        txtHallazgo.Text = d.FallaDetectada;
        txtTrabajo.Text = d.TrabajoRecomendado;
        txtObservaciones.Text = d.Observaciones;
        nudKm.Value = d.Kilometraje ?? 0;
        chkKm.Checked = d.Kilometraje.HasValue;
        _items = new BindingList<ItemPropuestaDto>(d.Items);
        foreach (var item in _items)
        {
            if (!item.IdSolicitudAtencion.HasValue && d.Solicitudes.Count == 1) item.IdSolicitudAtencion = d.Solicitudes[0].IdSolicitud;
            item.SolicitudDescripcion = d.Solicitudes.FirstOrDefault(s => s.IdSolicitud == item.IdSolicitudAtencion)?.Descripcion ?? "";
        }
        dgvItems.DataSource = _items;
        _iniciales = new BindingList<ServicioEvaluadoDto>(d.ServiciosIniciales);
        dgvIniciales.DataSource = _iniciales;
        foreach (DataGridViewRow fila in dgvIniciales.Rows)
            if (fila.DataBoundItem is ServicioEvaluadoDto servicio && servicio.Estado != EstadoEjecucionDetalle.NoIniciado)
                fila.ReadOnly = true;
        ConfigurarRecorrido();
        _cargando = false;
        _sucio = false;
        ActualizarTotal();
    }

    private void ConfigurarRecorrido()
    {
        if (_datos is null) return;
        bool informe = chkInforme.Checked;
        if (informe && !tabs.TabPages.Contains(tabInforme)) tabs.TabPages.Insert(0, tabInforme);
        if (!informe) tabs.TabPages.Remove(tabInforme);
        if (_iniciales.Count == 0) tabs.TabPages.Remove(tabIniciales);
        else if (!tabs.TabPages.Contains(tabIniciales)) tabs.TabPages.Add(tabIniciales);
        Text = informe ? "Diagnóstico y presupuesto" : "Preparar presupuesto";
        btnFinalizar.Text = chkSinPropuesta.Checked ? "Finalizar sin presupuesto" : "Enviar a recepción";
        btnGuardar.Text = "Guardar borrador";
        btnSeguimiento.Visible = informe;
        chkSinPropuesta.Enabled = informe && _items.Count == 0;
        if (!informe) chkSinPropuesta.Checked = false;
        btnAgregar.Enabled = !chkSinPropuesta.Checked;
        btnQuitar.Enabled = !chkSinPropuesta.Checked && _items.Count > 0;
        lblAviso.Text = informe ? "Complete el informe y el presupuesto. Guarde un borrador para continuar después." : "Presupueste lo solicitado. Enviar a recepción no autoriza ejecutar trabajos.";
        // La clasificación de recepción orienta, pero no obliga a inventar un diagnóstico.
        if (_datos.TipoSolicitud == TipoSolicitudAtencion.ServicioDefinido) tabs.SelectedTab = tabPropuesta;
        else tabs.SelectedTab = informe ? tabInforme : tabPropuesta;
    }

    private void Cambiado(object? s, EventArgs e)
    {
        txtHallazgo.Enabled = chkInforme.Checked;
        txtTrabajo.Enabled = chkInforme.Checked;
        if (!_cargando)
        {
            _sucio = true;
            if (ReferenceEquals(s, chkInforme) || ReferenceEquals(s, chkSinPropuesta)) ConfigurarRecorrido();
        }
    }

    private void FiltrarCatalogo(object? s, EventArgs e)
    {
        var q = txtBuscar.Text.Trim();
        cboConcepto.DisplayMember = nameof(ConceptoPropuestaDto.Etiqueta);
        cboConcepto.DataSource = _catalogo.Where(c => ($"{c.Nombre} {c.Categoria} {c.Descripcion}").Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    private void ConceptoCambiado(object? s, EventArgs e)
    {
        if (cboConcepto.SelectedItem is not ConceptoPropuestaDto c)
            return;
        chkAportaCliente.Visible = c.Tipo != TipoConcepto.Servicio;
        if (c.Tipo == TipoConcepto.Servicio) chkAportaCliente.Checked = false;
        nudCantidad.Value = 1;
        txtDescripcion.Text = c.Nombre;
        nudPrecio.Value = Math.Min(nudPrecio.Maximum, c.PrecioReferencia ?? 0);
        lblPrecio.Text = c.PrecioReferencia.HasValue ? "Precio de referencia: revise su vigencia." : "Sin precio de referencia: establezca el importe (0 si es sin cargo).";
    }

    private void Agregar(object? s, EventArgs e)
    {
        ValidateChildren();
        if (cboConcepto.SelectedItem is not ConceptoPropuestaDto c)
            return;
        if (_items.Any(i => i.IdConcepto == c.IdConcepto && i.IdSolicitudAtencion == (cboSolicitud.SelectedItem as SolicitudIngresoDto)?.IdSolicitud))
        {
            MessageBox.Show(this, "El concepto ya está agregado. Quite la fila para reemplazarlo.");
            return;
        }

        if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            return;
        if (chkAportaCliente.Checked && c.Tipo == TipoConcepto.Servicio)
        {
            MessageBox.Show(this, "El cliente puede aportar repuestos o consumibles, no servicios.");
            return;
        }

        _items.Add(new ItemPropuestaDto { SolicitudDescripcion = (cboSolicitud.SelectedItem as SolicitudIngresoDto)?.Descripcion ?? "", IdSolicitudAtencion = (cboSolicitud.SelectedItem as SolicitudIngresoDto)?.IdSolicitud, OrigenMaterial = chkAportaCliente.Checked ? OrigenMaterial.Cliente : OrigenMaterial.Taller, IdConcepto = c.IdConcepto, Descripcion = txtDescripcion.Text.Trim(), Cantidad = nudCantidad.Value, PrecioUnitario = chkAportaCliente.Checked ? 0 : nudPrecio.Value });
        _sucio = true;
        ActualizarTotal();
    }

    private void OrigenCambiado(object? s, EventArgs e)
    {
        nudPrecio.Enabled = !chkAportaCliente.Checked;
        if (chkAportaCliente.Checked)
            nudPrecio.Value = 0;
    }

    private void Quitar(object? s, EventArgs e)
    {
        if (dgvItems.CurrentRow?.DataBoundItem is ItemPropuestaDto i)
        {
            _items.Remove(i);
            _sucio = true;
            ActualizarTotal();
        }
    }

    private void ActualizarTotal()
    {
        lblTotal.Text = $"{_items.Count} ítems · Total propuesto: {_items.Sum(i => i.Subtotal):N2} ARS · Cantidad y precio editables en la lista.";
        chkSinPropuesta.Enabled = chkInforme.Checked && _items.Count == 0;
        btnQuitar.Enabled = _items.Count > 0;
    }

    private void ValidarItem(object? s, DataGridViewCellValidatingEventArgs e)
    {
        if (_cargando || e.RowIndex < 0) return;
        var columna = dgvItems.Columns[e.ColumnIndex].Name;
        if (columna is not ("colCantidad" or "colPrecioUnitario")) return;
        var precio = columna == "colPrecioUnitario";
        if (!decimal.TryParse(Convert.ToString(e.FormattedValue), out var valor)
            || valor < (precio ? 0 : 0.001m) || valor > (precio ? 1000000000m : 1000000m)
            || decimal.Round(valor, precio ? 2 : 3) != valor
            || precio && dgvItems.Rows[e.RowIndex].DataBoundItem is ItemPropuestaDto { OrigenMaterial: OrigenMaterial.Cliente } && valor != 0)
        {
            e.Cancel = true;
            dgvItems.Rows[e.RowIndex].ErrorText = precio ? "Ingrese un precio no negativo con hasta dos decimales. Material del cliente: 0." : "Ingrese una cantidad positiva con hasta tres decimales.";
        }
        else dgvItems.Rows[e.RowIndex].ErrorText = "";
    }

    private void ItemModificado(object? s, DataGridViewCellEventArgs e)
    {
        if (_cargando || e.RowIndex < 0) return;
        _sucio = true;
        dgvItems.InvalidateRow(e.RowIndex);
        ActualizarTotal();
    }

    private void ErrorItem(object? s, DataGridViewDataErrorEventArgs e)
    {
        e.ThrowException = false;
        e.Cancel = true;
        lblAviso.Text = "Revise cantidad y precio: use valores numéricos válidos para la configuración regional de Windows.";
    }
    private void InicialModificado(object? s, DataGridViewCellEventArgs e)
    {
        if (!_cargando && e.RowIndex >= 0)
            _sucio = true;
    }

    private void ErrorGrilla(object? s, DataGridViewDataErrorEventArgs e)
    {
        e.ThrowException = false;
        MessageBox.Show(this, "Seleccione un estado válido de ejecución.");
    }

    private async void Guardar(object? s, EventArgs e) => await PersistirAsync(false);
    private async void Finalizar(object? s, EventArgs e) => await PersistirAsync(true);
    private async Task PersistirAsync(bool finalizar)
    {
        if (_ocupado || _datos is null)
            return;
        if (!ValidateChildren() || !dgvItems.EndEdit()) return;
        if (dgvItems.DataSource is not null) BindingContext?[dgvItems.DataSource]?.EndCurrentEdit();
        dgvIniciales.EndEdit();
        if (dgvIniciales.DataSource is not null)
            BindingContext?[dgvIniciales.DataSource]?.EndCurrentEdit();
        if (finalizar && MessageBox.Show(this, "¿Enviar a recepción? El presupuesto quedará listo para comunicar al cliente. Los trabajos requieren su aceptación previa.", "Finalizar evaluación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        var pestana = tabs.SelectedTab;
        var primeraFila = dgvItems.FirstDisplayedScrollingRowIndex;
        _ocupado = true;
        flpAcciones.Enabled = false;
        tabs.Enabled = false;
        try
        {
            _datos.IncluyeDiagnostico = chkInforme.Checked;
            _datos.SoloEvaluacion = chkSinPropuesta.Checked;
            _datos.FallaDetectada = txtHallazgo.Text;
            _datos.TrabajoRecomendado = txtTrabajo.Text;
            _datos.Observaciones = txtObservaciones.Text;
            _datos.Kilometraje = chkKm.Checked ? (int)nudKm.Value : null;
            _datos.Items = _items.ToList();
            _datos.ServiciosIniciales = _iniciales.ToList();
            await _servicio.GuardarAsync(_datos, finalizar);
            _sucio = false;
            if (finalizar)
            {
                MessageBox.Show(this, $"Atención {_id:D6} · {_datos.Vehiculo}\nPropuesta enviada a recepción. Pendiente de informar al cliente y obtener su decisión.", "Preparación finalizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Finalizada = true;
                DialogResult = DialogResult.OK;
            }
            else
            {
                // Actualiza versiones sin reconstruir la lista enlazada: conserva selección y desplazamiento.
                _datos = await _servicio.ObtenerAsync(_id);
                lblAviso.Text = "Avance guardado. Puede cerrar y continuar luego; la reserva sigue a su nombre.";
            }
        }
        catch (Exception ex)
        {
            MostrarError(ex);
        }
        finally
        {
            _ocupado = false;
            flpAcciones.Enabled = true;
            tabs.Enabled = true;
            if (pestana is not null && tabs.TabPages.Contains(pestana)) tabs.SelectedTab = pestana;
            dgvItems.PerformLayout();
            if (primeraFila >= 0 && primeraFila < dgvItems.RowCount) dgvItems.FirstDisplayedScrollingRowIndex = primeraFila;
            dgvItems.Invalidate();
        }

        if (Finalizada)
            Close();
    }

    private void Cerrar(object? s, EventArgs e) => Close();
    private void AlCerrar(object? s, FormClosingEventArgs e)
    {
        if (_ocupado)
        {
            e.Cancel = true;
            return;
        }

        if (_sucio && MessageBox.Show(this, "Hay cambios sin guardar. ¿Cerrar y descartarlos? Los avances guardados y la reserva se conservan.", "Cerrar evaluación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            e.Cancel = true;
    }

    private async void Seguimiento(object? s, EventArgs e)
    {
        if (_ocupado || _cargando)
            return;
        if (_sucio)
        {
            MessageBox.Show(this, "Guarde primero el avance de la evaluación.");
            return;
        }

        _ocupado = true;
        try
        {
            using var f = new Taller.Presentacion.Formularios.Atenciones.FrmSeguimientoTrabajo(_id, Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<SeguimientoTrabajoServicio>(_proveedor), _servicio, Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<PresupuestoServicio>(_proveedor), Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto>(_proveedor));
            f.PrepararOperacion("tabHallazgos");
            f.ShowDialog(this);
            CargarDatos(await _servicio.ObtenerAsync(_id));
        }
        catch (Exception ex)
        {
            MostrarError(ex);
        }
        finally
        {
            _ocupado = false;
        }
    }

    private void MostrarError(Exception ex) => Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Evaluación técnica");
}
