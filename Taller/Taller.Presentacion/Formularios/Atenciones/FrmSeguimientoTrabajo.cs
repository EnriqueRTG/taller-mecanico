using Taller.Aplicacion.DTOs.Ejecuciones;
using Taller.Aplicacion.Servicios;
using Taller.Presentacion.Errores;
using Taller.Presentacion.Formularios.Compartidos;
using Taller.Presentacion.Formularios.Presupuestos;
using Taller.Dominio.Enumeraciones;

namespace Taller.Presentacion.Formularios.Atenciones;
/// <summary>Expediente real compartido: hallazgos, pausas y materiales. Los servicios vuelven a comprobar permisos y versiones.</summary>
public partial class FrmSeguimientoTrabajo : Form
{
    private readonly int _id;
    private readonly Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto _empresa;
    private readonly SeguimientoTrabajoServicio _servicio;
    private readonly EvaluacionServicio _evaluacion;
    private readonly PresupuestoServicio _presupuestos;
    private SeguimientoTrabajoDto? _datos;
    private bool _ocupado;
    private string? _pestanaInicial;
    /// <summary>Abre el alcance de la operación solicitada, sin confundirlo con un filtro.</summary>
    public void PrepararOperacion(string pestana)
    {
        if (pestana is not ("tabHallazgos" or "tabPausas" or "tabMateriales")) throw new ArgumentException("Operación desconocida.", nameof(pestana));
        _pestanaInicial = pestana;
    }
    public FrmSeguimientoTrabajo(int id, SeguimientoTrabajoServicio servicio, EvaluacionServicio evaluacion, PresupuestoServicio presupuestos, Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto empresa)
    {
        InitializeComponent();
        txtDescripcion.ReadOnly = txtRecomendacion.ReadOnly = txtMaterial.ReadOnly = true;
        txtDescripcion.Visible = txtRecomendacion.Visible = chkImpide.Visible = btnHallazgo.Visible = btnAdicional.Visible = false;
        btnResolver.Visible = btnRecibir.Visible = false;
        _id = id;
        _empresa = empresa;
        _servicio = servicio;
        _evaluacion = evaluacion;
        _presupuestos = presupuestos;
    }

    private async void Cargar(object? s, EventArgs e)
    {
        Taller.Presentacion.Estilos.DisposicionVentanas.AjustarAlAreaDisponible(this);
        await OperarAsync(() => Task.CompletedTask);
        if (_pestanaInicial is not null && tabs.TabPages.ContainsKey(_pestanaInicial))
        {
            tabs.SelectedTab = tabs.TabPages[_pestanaInicial];
            if (_pestanaInicial == "tabHallazgos" && !txtDescripcion.ReadOnly) txtDescripcion.Focus();
        }
    }

    private async Task RecargarAsync()
    {
        _datos = null;
        dgvHallazgos.DataSource = null;
        dgvPausas.DataSource = null;
        dgvMateriales.DataSource = null;
        btnHallazgo.Enabled = btnAdicional.Enabled = btnResolver.Enabled = btnRecibir.Enabled = btnPausar.Enabled = false;
        _datos = await _servicio.ObtenerAsync(_id);
        Text = "Progreso y seguimiento de la atención";
        lblResumen.Text = _datos.Resumen + $" · {_datos.Progreso.Count(i => i.Estado == EstadoEjecucionDetalle.Completado)} completados / {_datos.Progreso.Count} autorizados. Avances guardados; use Actualizar para consultar cambios. Los hallazgos son incidencias separadas.";
        dgvProgreso.DataSource = _datos.Progreso;

        btnExportarAvance.Enabled = _datos.Progreso.Count > 0;
        btnImprimirInforme.Enabled = _datos.Progreso.Count > 0 || !string.IsNullOrWhiteSpace(_datos.InformeInicial) || _datos.Hallazgos.Count > 0 || _datos.Pausas.Count > 0;
        if (_datos.Progreso.Count == 0) lblResumen.Text += " Aún no hay trabajos autorizados para mostrar en Avance.";
        if (txtDescripcion.Parent is TableLayoutPanel hallazgos)
            for (int fila = 1; fila < hallazgos.RowCount; fila++) hallazgos.RowStyles[fila].Height = _datos.PuedeRegistrar ? (fila == 3 ? 48 : 65) : 0;
        if (txtMaterial.Parent is TableLayoutPanel materiales)
            for (int fila = 1; fila < materiales.RowCount; fila++) materiales.RowStyles[fila].Height = _datos.PuedeRegistrar ? (fila == 1 ? 65 : 50) : 0;
        txtDescripcion.ReadOnly = txtRecomendacion.ReadOnly = txtMaterial.ReadOnly = !_datos.PuedeRegistrar;
        dgvHallazgos.DataSource = _datos.Hallazgos;
        dgvPausas.DataSource = _datos.Pausas;
        dgvMateriales.DataSource = _datos.Items.Where(i => i.Origen == OrigenMaterial.Cliente).ToList();
        cboTarea.DisplayMember = "Etiqueta";
        cboTarea.ValueMember = "IdDetalle";
        cboTarea.DataSource = new List<MaterialTrabajoDto>
        {
            new()
            {
                IdDetalle = 0,
                Descripcion = "Toda la intervención"
            }
        }.Concat(_datos.Items).ToList();
        // Las consultas no muestran editores de operaciones técnicas que este usuario no puede realizar.
        txtDescripcion.Visible = txtRecomendacion.Visible = chkImpide.Visible = btnHallazgo.Visible = btnAdicional.Visible = _datos.PuedeRegistrar;
        btnResolver.Visible = btnRecibir.Visible = _datos.PuedeRegistrar;
        txtMaterial.Visible = _datos.PuedeRegistrar;
        if (nudRecibido.Parent is { } editorMaterial) editorMaterial.Visible = _datos.PuedeRegistrar;
        btnPausar.Visible = _datos.PuedePausar;
        btnPausar.Enabled = _datos.PuedePausar;
        btnHallazgo.Enabled = btnAdicional.Enabled = btnResolver.Enabled = btnRecibir.Enabled = _datos.PuedeRegistrar;
        dgvHallazgos.ClearSelection();
        dgvHallazgos.CurrentCell = null;
        dgvPausas.ClearSelection();
        dgvPausas.CurrentCell = null;
        dgvMateriales.ClearSelection();
        dgvMateriales.CurrentCell = null;
        ActualizarContexto(null, EventArgs.Empty);
    }

    private void ActualizarContexto(object? sender, EventArgs e)
    {
        if (_datos == null) return;
        var pestana = tabs.SelectedTab?.Name;
        bool alcance = _datos.PuedeRegistrar && (pestana is "tabHallazgos" or "tabPausas");
        cboTarea.Visible = lblAlcance.Visible = alcance;
        panelContexto.RowStyles[1].Height = alcance ? 34 : 0;
        tlpPrincipal.RowStyles[1].Height = alcance ? 68 : 34;
        lblAlcance.Text = pestana == "tabHallazgos" ? "El hallazgo afecta a:" : "Detener este alcance:";
        lblContexto.Text = pestana switch
        {
            "tabHallazgos" => "Registre problemas nuevos; un informe no autoriza trabajos ni genera cargos. Seleccione el informe para presupuestar un adicional.",
            "tabPausas" => "Una pausa bloquea el alcance indicado. El técnico documenta su resolución antes de continuar.",
            "tabMateriales" => "Seleccione un material acordado a cargo del cliente y registre el total recibido y su aptitud para utilizarlo.",
            _ => "Avances guardados de los trabajos autorizados. La edición se realiza desde Gestionar ejecución."
        };
        btnPausar.Text = _datos.PuedeRegistrar ? "Pausar alcance seleccionado" : "Registrar detención solicitada por el cliente";
        btnAdicional.Enabled = _datos.PuedeRegistrar && (_datos.Estado is EstadoAtencion.EnEvaluacionTecnica or EstadoAtencion.EnEjecucion)
            && dgvHallazgos.CurrentRow?.DataBoundItem is HallazgoConsultaDto;
        btnResolver.Enabled = _datos.PuedeRegistrar && dgvPausas.CurrentRow?.DataBoundItem is PausaConsultaDto { FechaResolucion: null };
        btnRecibir.Enabled = _datos.PuedeRegistrar && dgvMateriales.CurrentRow?.DataBoundItem is MaterialTrabajoDto;
    }

    private int? Tarea => cboTarea.SelectedItem is MaterialTrabajoDto { IdDetalle: > 0 } i ? i.IdDetalle : null;

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
            ErroresInterfaz.Mostrar(this, ex, "Seguimiento técnico");
        }
        finally
        {
            _ocupado = false;
            tlpPrincipal.Enabled = true;
        }
    }

    private async void Actualizar(object? s, EventArgs e) => await OperarAsync(() => Task.CompletedTask);
    private async void Hallazgo(object? s, EventArgs e)
    {
        if (_datos == null || _ocupado)
            return;
        await OperarAsync(async () =>
        {
            await _servicio.RegistrarHallazgoAsync(new() { IdAtencion = _id, Version = _datos.Version, IdDetalle = Tarea, Descripcion = txtDescripcion.Text, Recomendacion = txtRecomendacion.Text, ImpideContinuar = chkImpide.Checked });
            txtDescripcion.Clear();
            txtRecomendacion.Clear();
            chkImpide.Checked = false;
        });
    }

    private async void Pausar(object? s, EventArgs e)
    {
        if (_datos == null || _ocupado)
            return;
        using var f = new FrmSolicitarMotivo("Pausar trabajo", _datos.PuedeRegistrar ? "Indique la causa. Se bloquea el alcance elegido sin borrar lo ejecutado ni cancelar lo autorizado." : "Registre quién pidió detener y el motivo. Se detendrá toda la intervención. Avise también directamente al técnico: el sistema controla el registro, no detiene físicamente el trabajo.");
        if (f.ShowDialog(this) == DialogResult.OK)
            await OperarAsync(() => _servicio.PausarAsync(_id, _datos.Version, _datos.PuedeRegistrar ? Tarea : null, f.Motivo));
    }

    private async void Resolver(object? s, EventArgs e)
    {
        if (_datos == null || _ocupado || dgvPausas.CurrentRow?.DataBoundItem is not PausaConsultaDto p || p.FechaResolucion != null)
            return;
        using var f = new FrmSolicitarMotivo("Resolver pausa bajo responsabilidad técnica", "Explique si ya puede retomar, continuar solo otras tareas o terminar con interrupción justificada. Resolver la pausa no ejecuta ni autoriza trabajos adicionales.");
        if (f.ShowDialog(this) == DialogResult.OK)
            await OperarAsync(() => _servicio.ResolverPausaAsync(_id, _datos.Version, p.Id, f.Motivo));
    }

    private void MaterialSeleccionado(object? s, EventArgs e)
    {
        ActualizarContexto(s, e);
        if (dgvMateriales.CurrentRow?.DataBoundItem is MaterialTrabajoDto d)
        {
            nudRecibido.Value = 0;
            nudRecibido.Maximum = d.Cantidad;
            nudRecibido.Value = d.Recibida;
            chkConforme.Checked = d.Conforme;
            txtMaterial.Text = d.Observaciones;
        }
    }

    private async void Recibir(object? s, EventArgs e)
    {
        if (_datos == null || _ocupado || dgvMateriales.CurrentRow?.DataBoundItem is not MaterialTrabajoDto d)
            return;
        await OperarAsync(() => _servicio.RecibirMaterialAsync(_id, _datos.Version, d.IdDetalle, nudRecibido.Value, chkConforme.Checked, txtMaterial.Text));
    }

    private async void Adicional(object? s, EventArgs e)
    {
        if (_datos == null || _ocupado)
            return;
        if (dgvHallazgos.CurrentRow?.DataBoundItem is not HallazgoConsultaDto h) return;
        int? hallazgo = h.Id;
        _ocupado = true;
        try
        {
            using var f = new FrmPrepararPropuesta(_id, _servicio, _evaluacion, _presupuestos, hallazgo: hallazgo);
            f.ShowDialog(this);
            await RecargarAsync();
        }
        catch (Exception ex)
        {
            ErroresInterfaz.Mostrar(this, ex, "Preparar adicional");
        }
        finally
        {
            _ocupado = false;
        }
    }

    private void Exportar(object? s, EventArgs e)
    {
        if (_datos == null || _ocupado) return;
        try { Taller.Presentacion.Impresion.ExportacionDocumento.GuardarHtml(this,
            Taller.Aplicacion.Reportes.DocumentoOperativoExportador.Progreso(_datos, _empresa),
            Taller.Aplicacion.Reportes.DocumentoReporteExportador.NombreArchivo("Avance-Atencion-" + _id, Taller.Dominio.Tiempo.FechaHoraTaller.Ahora)); }
        catch (Exception ex) { ErroresInterfaz.Mostrar(this, ex, "Exportar avance"); }
    }

    private void Imprimir(object? s, EventArgs e)
    {
        if (_datos == null || _ocupado)
            return;
        try
        {
            Taller.Presentacion.Impresion.ImpresionDocumento.Mostrar(this, Taller.Presentacion.Impresion.TextoIntervencion.Generar(_datos), _empresa, Taller.Aplicacion.Reportes.DocumentoReporteExportador.NombreArchivo("Intervencion-A-" + _id, Taller.Dominio.Tiempo.FechaHoraTaller.Ahora));
        }
        catch (Exception ex)
        {
            ErroresInterfaz.Mostrar(this, ex, "Informe de intervención");
        }
    }

    private void Cerrar(object? s, EventArgs e) => Close();
    private void AlCerrar(object? s, FormClosingEventArgs e)
    {
        if (_ocupado)
            e.Cancel = true;
        else if (txtDescripcion.TextLength > 0 && MessageBox.Show(this, "¿Cerrar sin registrar el hallazgo escrito?", "Cerrar", MessageBoxButtons.YesNo) != DialogResult.Yes)
            e.Cancel = true;
    }
}
