using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Enumeraciones;

namespace Taller.Presentacion.Formularios.Presupuestos;
/// <summary>Consulta documentos reales; recepción registra la comunicación al cliente sin decidir por él.</summary>
public partial class FrmPresupuestos : Form, Taller.Presentacion.Navegacion.IFormularioConOperacion
{
    private readonly SeguimientoTrabajoServicio _seguimiento;
    private readonly Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto _empresa;
    private readonly EvaluacionServicio _servicio;
    private readonly SesionUsuario _sesion;
    private readonly PresupuestoServicio _presupuestos;
    private List<PresupuestoConsultaDto> _datos = [];
    private bool _ocupado;
    private bool _cargando;
    public bool OperacionEnCurso => _ocupado || _cargando;

    public FrmPresupuestos(EvaluacionServicio servicio, SesionUsuario sesion, PresupuestoServicio presupuestos, SeguimientoTrabajoServicio seguimiento, Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto empresa)
    {
        InitializeComponent();
        _servicio = servicio;
        _sesion = sesion;
        _presupuestos = presupuestos;
        _seguimiento = seguimiento;
        _empresa = empresa;
    }

    private void Filtros_SizeChanged(object? sender, EventArgs e) =>
        Taller.Presentacion.Estilos.DisposicionFiltros.Ajustar(tlpFiltros, flpFiltrosAcciones, DeviceDpi);

    private void LimpiarFiltros_Click(object? sender, EventArgs e)
    {
        if (OperacionEnCurso) return;
        txtBuscar.Clear();
        cboEstado.SelectedIndex = 0;
        Filtrar(null, EventArgs.Empty);
        txtBuscar.Focus();
    }

    private async void Cargar(object? s, EventArgs e)
    {
        cboEstado.SelectedIndex = 0;
        await RecargarAsync();
    }

    private async Task RecargarAsync()
    {
        if (_cargando)
            return;
        _cargando = true;
        tlpPrincipal.Enabled = false;
        try
        {
            _datos = await _servicio.ListarPresupuestosAsync();
            Filtrar(null, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            _datos = [];
            dgvDatos.DataSource = null;
            Seleccion(null, EventArgs.Empty);
            Error(ex);
        }
        finally
        {
            _cargando = false;
            if (!IsDisposed)
                tlpPrincipal.Enabled = true;
        }
    }

    private void Filtrar(object? s, EventArgs e)
    {
        var q = txtBuscar.Text.Trim();
        dgvDatos.DataSource = _datos.Where(d => ($"{d.IdPresupuesto} {d.IdAtencion} {d.Cliente} {d.Vehiculo}").Contains(q, StringComparison.OrdinalIgnoreCase)).Where(d => cboEstado.SelectedIndex <= 0 || cboEstado.SelectedIndex == 1 && d.Estado == EstadoPresupuesto.PendientePresentacion || cboEstado.SelectedIndex == 2 && d.Estado == EstadoPresupuesto.Borrador || cboEstado.SelectedIndex == 3 && d.Estado == EstadoPresupuesto.PendienteDecision || cboEstado.SelectedIndex == 4 && d.Estado == EstadoPresupuesto.PendienteRevisionTecnica).OrderBy(d => !d.Vigente ? 5 : d.Estado == EstadoPresupuesto.PendienteRevisionTecnica ? 0 : d.Tipo == TipoPresupuesto.Reparacion && d.Estado == EstadoPresupuesto.PendientePresentacion ? 0 : d.Estado == EstadoPresupuesto.Borrador ? 1 : d.Estado == EstadoPresupuesto.PendienteDecision ? 2 : 3).ThenByDescending(d => d.Fecha).ThenByDescending(d => d.IdPresupuesto).ToList();
        dgvDatos.ClearSelection();
        dgvDatos.CurrentCell = null;
        Seleccion(null, EventArgs.Empty);
    }

    private PresupuestoConsultaDto? Fila => dgvDatos.CurrentRow?.DataBoundItem as PresupuestoConsultaDto;

    private void Seleccion(object? s, EventArgs e)
    {
        var recepcion = _sesion.UsuarioActual?.RolId == RolesSistema.AdministrativoId;
        var tecnico = _sesion.UsuarioActual?.RolId == RolesSistema.TecnicoId;
        btnDecidir.Visible = btnRevision.Visible = btnPresentar.Visible = recepcion;
        btnValidarSeleccion.Visible = tecnico;
        flpAcciones.Controls.SetChildIndex(btnPresentar, 0);
        flpAcciones.Controls.SetChildIndex(btnDecidir, 1);
        flpAcciones.Controls.SetChildIndex(btnVer, 2);
        flpAcciones.Controls.SetChildIndex(btnRevision, flpAcciones.Controls.Count - 1);
        btnSeguimiento.Text = recepcion ? "Consultar seguimiento" : "Seguimiento y hallazgos";
        lblAyuda.Text = Fila is null ? "Seleccione un presupuesto. Los borradores se continúan en Preparación técnica. Las acciones dependen del estado y del rol."
            : Fila.Estado == EstadoPresupuesto.PendienteRevisionTecnica ? "Selección parcial: el técnico debe comprobar su viabilidad. Luego recepción presenta la nueva versión y registra otra decisión. Aún no autoriza trabajos."
            : Fila.Estado == EstadoPresupuesto.Borrador ? "Borrador: el técnico debe continuarlo y enviarlo desde Preparación técnica antes de informar al cliente."
            : Fila.Estado == EstadoPresupuesto.PendientePresentacion ? "Recepción informa el alcance y los importes al cliente. Solicitar revisión devuelve los cambios al técnico sin autorizar trabajos."
            : "Solicitar revisión conserva esta versión y pide otra al técnico; solo es posible sin ejecución ni liquidación. La ejecución requiere aceptación del cliente.";
        btnDecidir.Enabled = !_ocupado && recepcion && Fila is { Vigente: true, Tipo: TipoPresupuesto.Reparacion, Estado: EstadoPresupuesto.PendienteDecision };
        btnDecidir.Visible = recepcion && Fila is { Estado: EstadoPresupuesto.PendienteDecision, Vigente: true };
        btnPresentar.Visible = recepcion && Fila is { Estado: EstadoPresupuesto.PendientePresentacion, Vigente: true };
        btnRevision.Margin = new Padding(24, 0, 0, 4);
        btnRevision.Text = Fila?.Estado == EstadoPresupuesto.PendientePresentacion ? "Pedir corrección técnica" : Fila?.Estado == EstadoPresupuesto.Aceptado ? "Revisar acuerdo (excepción)" : "Solicitar cambios del cliente";
        btnRevision.Enabled = !_ocupado && recepcion && Fila is { PuedeSolicitarRevision: true };
        btnVer.Enabled = btnSeguimiento.Enabled = !_ocupado && Fila != null;
        btnValidarSeleccion.Enabled = !_ocupado && tecnico && Fila is { Vigente: true, Estado: EstadoPresupuesto.PendienteRevisionTecnica };
        btnPresentar.Enabled = !_ocupado && _sesion.UsuarioActual?.RolId == RolesSistema.AdministrativoId && Fila is { Vigente: true, Tipo: TipoPresupuesto.Reparacion, Estado: EstadoPresupuesto.PendientePresentacion };
    }

    private void Ver(object? s, EventArgs e)
    {
        if (Fila is { } d)
        {
            using var f = new FrmConsultarPropuesta(d, _empresa);
            f.ShowDialog(this);
        }
    }

    private async void Actualizar(object? s, EventArgs e)
    {
        if (!_ocupado)
            await RecargarAsync();
    }

    private async void Presentar(object? s, EventArgs e)
    {
        if (_ocupado || Fila is not { } d)
            return;
        if (MessageBox.Show(this, "¿Confirma que comunicó al cliente el alcance y los importes de esta propuesta? No registra aceptación ni autoriza trabajos.", "Registrar presentación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        _ocupado = true;
        tlpPrincipal.Enabled = false;
        try
        {
            await _servicio.PresentarAsync(d.IdPresupuesto, d.Version);
            if (MessageBox.Show(this, "Presupuesto marcado como informado. ¿El cliente ya respondió y desea registrar su decisión ahora? Puede hacerlo después desde Registrar decisión.", "Presentación registrada", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                using var decision = new FrmDecisionPresupuesto(d.IdPresupuesto, _presupuestos);
                decision.ShowDialog(this);
            }
        }
        catch (Exception ex)
        {
            Error(ex);
        }
        finally
        {
            _ocupado = false;
            tlpPrincipal.Enabled = true;
            await RecargarAsync();
        }
    }

    private async void Decidir(object? s, EventArgs e)
    {
        if (_ocupado || Fila is not { } d)
            return;
        using var f = new FrmDecisionPresupuesto(d.IdPresupuesto, _presupuestos);
        f.ShowDialog(this);
        await RecargarAsync();
    }

    private async void Revision(object? s, EventArgs e)
    {
        if (_ocupado || Fila is not { } d)
            return;
        if (d.Estado == EstadoPresupuesto.Aceptado && MessageBox.Show(this, "Esta excepción sustituye el acuerdo vigente por una nueva propuesta que el técnico debe preparar y el cliente volver a aceptar. Solo puede usarse antes de ejecutar, sin reserva ni liquidación. ¿Continuar?", "Revisar acuerdo", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes) return;
        using var f = new Taller.Presentacion.Formularios.Compartidos.FrmSolicitarMotivo("Solicitar revisión de presupuesto", "Explique los cambios solicitados. La propuesta anterior se conserva; el técnico prepara una nueva versión que deberá presentarse y aceptarse. Solo se revisa antes de ejecutar trabajos.");
        if (f.ShowDialog(this) != DialogResult.OK)
            return;
        _ocupado = true;
        tlpPrincipal.Enabled = false;
        try
        {
            var id = await _presupuestos.SolicitarRevisionAsync(d.IdPresupuesto, d.Version, f.Motivo);
            MessageBox.Show(this, d.EsAdicional ? $"Revisión {id:D6} pendiente de validación técnica desde Presupuestos." : $"Revisión {id:D6} preparada como borrador. El técnico puede continuar desde Evaluaciones técnicas.");
        }
        catch (Exception ex)
        {
            Error(ex);
        }
        finally
        {
            _ocupado = false;
            tlpPrincipal.Enabled = true;
            await RecargarAsync();
        }
    }

    private async void ValidarSeleccion(object? s, EventArgs e)
    {
        if (_ocupado || Fila is not { } d)
            return;
        _ocupado = true;
        try
        {
            using var f = new FrmPrepararPropuesta(d.IdAtencion, _seguimiento, _servicio, _presupuestos, presupuesto: d.IdPresupuesto);
            f.ShowDialog(this);
        }
        catch (Exception ex)
        {
            Error(ex);
        }
        finally
        {
            _ocupado = false;
            await RecargarAsync();
        }
    }

    private async void Seguimiento(object? s, EventArgs e)
    {
        if (_ocupado || Fila is not { } d)
            return;
        _ocupado = true;
        try
        {
            using var f = new Taller.Presentacion.Formularios.Atenciones.FrmSeguimientoTrabajo(d.IdAtencion, _seguimiento, _servicio, _presupuestos, _empresa);
            f.ShowDialog(this);
        }
        catch (Exception ex)
        {
            Error(ex);
        }
        finally
        {
            _ocupado = false;
            await RecargarAsync();
        }
    }

    private void Error(Exception ex) => Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Presupuestos");
    private void AlCerrar(object? s, FormClosingEventArgs e)
    {
        if (_ocupado)
            e.Cancel = true;
    }
}
