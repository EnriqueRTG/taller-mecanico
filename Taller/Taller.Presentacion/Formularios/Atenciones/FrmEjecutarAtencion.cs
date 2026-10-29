using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.DTOs.Ejecuciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Enumeraciones;

namespace Taller.Presentacion.Formularios.Atenciones;
/// <summary>Registra el trabajo real de los ítems aceptados; conserva avances y reserva técnica.</summary>
public partial class FrmEjecutarAtencion : Form
{
    private bool _falloContinuidadRegistrado;
    private readonly IServiceProvider _proveedor;
    private readonly int _id;
    private readonly EjecucionServicio _servicio;
    private readonly SesionUsuario _sesion;
    private EjecucionAtencionDto? _datos;
    private BindingList<EjecucionItemDto> _items = [];
    private bool _ocupado, _sucio, _cargando, _sincronizando;
    private bool _consultandoPausas, _controlNoDisponible;
    private List<PausaConsultaDto> _pausasActivas = [];
    private string _ultimaAlerta = "";
    public FrmEjecutarAtencion(int id, EjecucionServicio servicio, SesionUsuario sesion, IServiceProvider proveedor)
    {
        InitializeComponent();
        Taller.Presentacion.Estilos.CabeceraOperacion.Aplicar(txtResumen);
        _proveedor = proveedor;
        _id = id;
        _servicio = servicio;
        _sesion = sesion;
    }

    private async void Cargar(object? s, EventArgs e)
    {
        Taller.Presentacion.Estilos.DisposicionVentanas.AjustarAlAreaDisponible(this);
        await RecargarAsync();
        await VigilarPausasAsync();
        if (!IsDisposed) temporizadorPausas.Start();
    }

    private async Task RecargarAsync()
    {
        _cargando = true;
        tlpPrincipal.Enabled = false;
        try
        {
            _datos = await _servicio.ObtenerAsync(_id);
            txtResumen.Text = _datos.Resumen.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
            _items = new BindingList<EjecucionItemDto>(_datos.Items);
            dgvItems.DataSource = _items;
            var propia = _datos.Estado == EstadoAtencion.EnEjecucion && _datos.IdTecnico == _sesion.UsuarioActual?.Id;
            dgvItems.ReadOnly = !propia;
            dgvItems.DefaultCellStyle.BackColor = propia ? Color.White : Color.FromArgb(241, 245, 249);
            Taller.Presentacion.Estilos.EstilosInterfaz.AplicarBotonPrimario(propia ? btnGuardar : btnTomar);
            btnCompletar.Visible = false; // La selección de Completado ya sincroniza la cantidad.
            btnOtrasAcciones.Visible = propia;
            btnTomar.Visible = _datos.Estado == EstadoAtencion.EnEjecucion && _datos.IdTecnico == null;
            btnGuardar.Visible = btnFinalizar.Visible = btnLiberar.Visible = chkConformidad.Visible = propia;
            txtObservaciones.ReadOnly = !propia && _datos.IdTecnico != null;
            btnCompletar.Enabled = propia;
            btnGuardar.Enabled = btnFinalizar.Enabled = btnLiberar.Enabled = propia;
            btnTomar.Enabled = _datos.Estado == EstadoAtencion.EnEjecucion && _datos.IdTecnico == null;
            lblAviso.Text = _datos.TieneExclusiones ? "Se muestran solo los ítems elegidos por el cliente. La selección ya pasó por revisión técnica; inicie la ejecución para registrar avances." : "Revise el alcance antes de tomar. Solo se muestran ítems aceptados. Cerrar conserva su reserva.";
            chkConformidad.Enabled = propia;
            chkConformidad.Checked = false;
            _sucio = false;
            ActualizarConformidad(null, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            Error(ex);
        }
        finally
        {
            _cargando = false;
            tlpPrincipal.Enabled = true;
            AplicarPausas();
        }
    }

    private async void Tomar(object? s, EventArgs e)
    {
        if (_ocupado || _datos == null)
            return;
        if (MessageBox.Show(this, "¿Confirma que puede comenzar los trabajos autorizados y dispone de los materiales necesarios?", "Tomar ejecución", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        await OperarAsync(() => _servicio.TomarAsync(_id, _datos.Version, txtObservaciones.Text));
    }

    private async void Liberar(object? s, EventArgs e)
    {
        if (_ocupado || _datos == null)
            return;
        if (_sucio && MessageBox.Show(this, "Hay cambios sin guardar. ¿Descartarlos y liberar la reserva?", "Liberar", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;
        await OperarAsync(() => _servicio.LiberarAsync(_id, _datos.Version));
    }

    private async Task OperarAsync(Func<Task> accion)
    {
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

    private void CompletarSeleccionada(object? sender, EventArgs e)
    {
        if (_ocupado || _cargando || dgvItems.ReadOnly || dgvItems.CurrentRow?.DataBoundItem is not EjecucionItemDto item) return;
        dgvItems.EndEdit();
        item.CantidadEjecutada = item.CantidadAutorizada;
        item.Estado = EstadoEjecucionDetalle.Completado;
        _items.ResetBindings();
        _sucio = true;
    }

    private void CeldaCambiada(object? s, DataGridViewCellEventArgs e)
    {
        if (_cargando || _sincronizando || e.RowIndex < 0 || dgvItems.Rows[e.RowIndex].DataBoundItem is not EjecucionItemDto item) return;
        _sincronizando = true;
        try
        {
            var columna = dgvItems.Columns[e.ColumnIndex].Name;
            if (columna == "colEstado" && item.Estado == EstadoEjecucionDetalle.Completado)
                item.CantidadEjecutada = item.CantidadAutorizada;
            else if (columna == "colCantidadEjecutada")
            {
                if (item.CantidadEjecutada == item.CantidadAutorizada) item.Estado = EstadoEjecucionDetalle.Completado;
                else if (item.CantidadEjecutada >= 0 && item.CantidadEjecutada < item.CantidadAutorizada)
                    item.Estado = EstadoEjecucionDetalle.EnCurso;
            }
            _items.ResetItem(e.RowIndex);
            ActualizarConformidad(null, EventArgs.Empty);
            _sucio = true;
        }
        finally { _sincronizando = false; }
    }

    private void ActualizarConformidad(object? sender, EventArgs e)
    {
        bool propia = _datos?.Estado == EstadoAtencion.EnEjecucion && _datos.IdTecnico == _sesion.UsuarioActual?.Id;
        bool resueltos = !_controlNoDisponible && _pausasActivas.Count == 0 && _items.Count > 0 && _items.All(i => i.Estado == EstadoEjecucionDetalle.Completado);
        chkConformidad.Visible = propia && resueltos;
        btnFinalizar.Visible = propia && resueltos && chkConformidad.Checked;
        chkConformidad.Enabled = propia && resueltos;
        if (!resueltos) chkConformidad.Checked = false;
        btnFinalizar.Enabled = propia && resueltos && chkConformidad.Checked;
        if (btnFinalizar.Enabled) { Taller.Presentacion.Estilos.EstilosInterfaz.AplicarBotonPrimario(btnFinalizar); Taller.Presentacion.Estilos.EstilosInterfaz.AplicarBotonSecundario(btnGuardar); }
        else { btnFinalizar.BackColor = Color.FromArgb(226, 232, 240); btnFinalizar.ForeColor = Color.DimGray; }
    }

    private async void ActualizarSituacion(object? sender, EventArgs e)
    {
        if (_ocupado || _cargando) return;
        if (_sucio && MessageBox.Show(this, "Actualizar reemplaza las ediciones sin guardar por los últimos datos registrados. ¿Continuar?",
            "Actualizar situación", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        await RecargarAsync();
        await VigilarPausasAsync();
    }

    private async void ComprobarPausas(object? sender, EventArgs e) => await VigilarPausasAsync();

    private async Task VigilarPausasAsync()
    {
        if (_consultandoPausas || _ocupado || _cargando || IsDisposed || Disposing) return;
        _consultandoPausas = true;
        try
        {
            // Consulta con contexto independiente: no descarta ni reemplaza las ediciones locales.
            using var scope = _proveedor.CreateScope();
            var seguimiento = await scope.ServiceProvider.GetRequiredService<SeguimientoTrabajoServicio>().ObtenerAsync(_id);
            if (IsDisposed || Disposing || _ocupado) return;
            _controlNoDisponible = false;
            _pausasActivas = seguimiento.Pausas.Where(p => p.FechaResolucion == null).ToList();
            AplicarPausas();
            var firma = string.Join(",", _pausasActivas.Select(p => p.Id).OrderBy(i => i));
            if (firma.Length > 0 && firma != _ultimaAlerta && Visible)
            {
                _ultimaAlerta = firma;
                MessageBox.Show(this, "Existe una detención activa: " + string.Join("; ", _pausasActivas.Select(p => p.Motivo)) +
                    "\nDetenga de forma segura el alcance indicado y coordine con recepción. No se reemplazaron los datos escritos en esta pantalla. Consulte Otras acciones → Gestionar pausas.",
                    "Detención de trabajos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (firma.Length == 0) _ultimaAlerta = "";
            _falloContinuidadRegistrado = false;
        }
        catch (Exception ex)
        {
            if (!_falloContinuidadRegistrado)
            {
                Serilog.Log.Error(ex, "No se pudo verificar la continuidad de la atención {Atencion}", _id);
                _falloContinuidadRegistrado = true;
            }
            if (!IsDisposed && !Disposing)
            {
                _controlNoDisponible = true;
                AplicarPausas();
            }
        }
        finally { _consultandoPausas = false; }
    }

    private void AplicarPausas()
    {
        bool propia = _datos?.Estado == EstadoAtencion.EnEjecucion && _datos.IdTecnico == _sesion.UsuarioActual?.Id;
        bool general = _controlNoDisponible || _pausasActivas.Any(p => p.IdDetalle == null);
        dgvItems.ReadOnly = !propia || general;
        foreach (DataGridViewRow fila in dgvItems.Rows)
        {
            bool bloqueada = general || fila.DataBoundItem is EjecucionItemDto i && _pausasActivas.Any(p => p.IdDetalle == i.IdDetalle);
            fila.ReadOnly = !propia || bloqueada;
            fila.DefaultCellStyle.BackColor = !propia || bloqueada ? Color.FromArgb(241, 245, 249) : Color.White;
        }
        btnGuardar.Enabled = propia && !general;
        btnTomar.Enabled = _datos?.Estado == EstadoAtencion.EnEjecucion && _datos.IdTecnico == null && !general;
        lblAviso.ForeColor = general || _pausasActivas.Count > 0 ? Color.Firebrick : SystemColors.ControlText;
        lblAviso.Text = _controlNoDisponible ? "No se pudo verificar la continuidad. Se reintentará automáticamente; las ediciones se conservan."
            : _pausasActivas.Count > 0 ? "DETENCIÓN ACTIVA: " + string.Join("; ", _pausasActivas.Select(p => p.Alcance + ": " + p.Motivo))
            : "Solo trabajos autorizados. Guarde los avances; cerrar conserva la reserva. Control de pausas cada 10 segundos.";
        btnGuardar.Visible = btnGuardar.Enabled;
        btnTomar.Visible = btnTomar.Enabled;
        ActualizarConformidad(null, EventArgs.Empty);
    }

    private void ConfirmarEstado(object? s, EventArgs e)
    {
        if (dgvItems.IsCurrentCellDirty && dgvItems.CurrentCell is DataGridViewComboBoxCell)
            dgvItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void TextoCambiado(object? s, EventArgs e)
    {
        if (!_cargando)
            _sucio = true;
    }

    private void ErrorCelda(object? s, DataGridViewDataErrorEventArgs e)
    {
        e.ThrowException = false;
        MessageBox.Show(this, "Ingrese cantidades decimales válidas y un estado de la lista.");
    }

    private async void Guardar(object? s, EventArgs e) => await PersistirAsync(false);
    private async void Finalizar(object? s, EventArgs e) => await PersistirAsync(true);
    private async Task PersistirAsync(bool finalizar)
    {
        if (_ocupado || _datos == null)
            return;
        if (!ValidateChildren() || !dgvItems.EndEdit())
            return;
        if (dgvItems.DataSource != null)
            BindingContext?[dgvItems.DataSource]?.EndCurrentEdit();
        if (finalizar && MessageBox.Show(this, "¿Finalizar el trabajo? Todos los ítems autorizados deben estar completados y comprobados.", "Terminar ejecución", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;
        _datos.Items = _items.ToList();
        if (finalizar && !chkConformidad.Checked)
        {
            MessageBox.Show(this, "Compruebe el resultado y marque la conformidad técnica antes de finalizar.");
            return;
        }
        _datos.ConformidadTecnica = chkConformidad.Checked;
        _datos.Observaciones = txtObservaciones.Text;
        bool guardado = false;
        await OperarAsync(async () => { await _servicio.GuardarAsync(_datos, finalizar); guardado = true; });
        if (guardado && finalizar)
        {
            _sucio = false;
            MessageBox.Show(this, $"Atención {_id:D6}\n{txtResumen.Lines.FirstOrDefault()}\nTrabajo técnico finalizado. Informe a recepción para comunicar al cliente y liquidar lo efectivamente realizado.", "Trabajo registrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
    }

    private string _destinoSeguimiento = "tabHallazgos";
    private void MostrarOpciones(object? s, EventArgs e)
    {
        // La pausa puede consultarse y resolverse aunque bloquee la edición.
        foreach (ToolStripItem item in menuOpciones.Items) item.Visible = item.Enabled = !_controlNoDisponible;
        menuOpciones.Show(btnOtrasAcciones, new Point(0, btnOtrasAcciones.Height));
    }
    private void Hallazgos(object? s, EventArgs e) { _destinoSeguimiento = "tabHallazgos"; Seguimiento(s, e); }
    private void Pausas(object? s, EventArgs e) { _destinoSeguimiento = "tabPausas"; Seguimiento(s, e); }
    private void Materiales(object? s, EventArgs e) { _destinoSeguimiento = "tabMateriales"; Seguimiento(s, e); }
    private async void Seguimiento(object? s, EventArgs e)
    {
        if (_ocupado || _cargando)
            return;
        if (_sucio)
        {
            MessageBox.Show(this, "Guarde el avance o vuelva a abrir la ejecución antes de modificar el seguimiento.");
            return;
        }

        _ocupado = true;
        try
        {
            using var f = new FrmSeguimientoTrabajo(_id, Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<SeguimientoTrabajoServicio>(_proveedor), Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<EvaluacionServicio>(_proveedor), Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<PresupuestoServicio>(_proveedor), Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto>(_proveedor));
            f.PrepararOperacion(_destinoSeguimiento);
            _destinoSeguimiento = "tabHallazgos";
            f.ShowDialog(this);
            await RecargarAsync();
        }
        catch (Exception ex)
        {
            Error(ex);
        }
        finally
        {
            _ocupado = false;
        }
    }

    private void Cerrar(object? s, EventArgs e) => Close();
    private void AlCerrar(object? s, FormClosingEventArgs e)
    {
        if (_ocupado || _cargando)
        {
            e.Cancel = true;
            return;
        }

        if (_sucio && MessageBox.Show(this, "¿Descartar los cambios sin guardar? La reserva y los avances guardados se conservan.", "Cerrar", MessageBoxButtons.YesNo) != DialogResult.Yes)
            e.Cancel = true;
    }

    private void Error(Exception ex) => Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Ejecución");
}
