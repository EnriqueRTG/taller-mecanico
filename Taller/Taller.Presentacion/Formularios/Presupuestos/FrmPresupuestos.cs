using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Enumeraciones;
namespace Taller.Presentacion.Formularios.Presupuestos;
/// <summary>Consulta documentos reales; recepción registra la comunicación al cliente sin decidir por él.</summary>
public partial class FrmPresupuestos : Form, Taller.Presentacion.Navegacion.IFormularioConOperacion
{
    private readonly EvaluacionServicio _servicio;
    private readonly SesionUsuario _sesion;
    private readonly PresupuestoServicio _presupuestos;
    private List<PresupuestoConsultaDto> _datos = [];
    private bool _ocupado;
    private bool _cargando;
    public bool OperacionEnCurso => _ocupado || _cargando;
    public FrmPresupuestos(EvaluacionServicio servicio, SesionUsuario sesion, PresupuestoServicio presupuestos)
    { InitializeComponent(); _servicio = servicio; _sesion = sesion; _presupuestos = presupuestos; }
    private async void Cargar(object? s, EventArgs e) { cboEstado.SelectedIndex = 0; await RecargarAsync(); }
    private async Task RecargarAsync()
    {
        if (_cargando) return;
        _cargando = true; tlpPrincipal.Enabled = false;
        try { _datos = await _servicio.ListarPresupuestosAsync(); Filtrar(null, EventArgs.Empty); }
        catch (Exception ex) { Error(ex); }
        finally { _cargando = false; if (!IsDisposed) tlpPrincipal.Enabled = true; }
    }
    private void Filtrar(object? s, EventArgs e)
    {
        var q = txtBuscar.Text.Trim();
        dgvDatos.DataSource = _datos.Where(d => ($"{d.IdPresupuesto} {d.IdAtencion} {d.Cliente} {d.Vehiculo}").Contains(q, StringComparison.OrdinalIgnoreCase))
            .Where(d => cboEstado.SelectedIndex <= 0 || cboEstado.SelectedIndex == 1 && d.Estado == EstadoPresupuesto.PendientePresentacion || cboEstado.SelectedIndex == 2 && d.Estado == EstadoPresupuesto.Borrador || cboEstado.SelectedIndex == 3 && d.Estado == EstadoPresupuesto.PendienteDecision)
            .OrderBy(d => !d.Vigente ? 5 : d.Tipo == TipoPresupuesto.Reparacion && d.Estado == EstadoPresupuesto.PendientePresentacion ? 0 : d.Estado == EstadoPresupuesto.Borrador ? 1 : d.Estado == EstadoPresupuesto.PendienteDecision ? 2 : 3)
            .ThenByDescending(d => d.Fecha).ThenByDescending(d => d.IdPresupuesto).ToList();
        dgvDatos.ClearSelection(); dgvDatos.CurrentCell = null; Seleccion(null, EventArgs.Empty);
    }
    private PresupuestoConsultaDto? Fila => dgvDatos.CurrentRow?.DataBoundItem as PresupuestoConsultaDto;
    private void Seleccion(object? s, EventArgs e)
    {
        var recepcion = _sesion.UsuarioActual?.RolId == RolesSistema.AdministrativoId;
        btnDecidir.Enabled = !_ocupado && recepcion && Fila is { Vigente: true, Tipo: TipoPresupuesto.Reparacion, Estado: EstadoPresupuesto.PendienteDecision };
        btnRevision.Enabled = !_ocupado && recepcion && Fila is { Vigente: true, Tipo: TipoPresupuesto.Reparacion } && Fila.Estado != EstadoPresupuesto.Borrador;
        btnVer.Enabled = !_ocupado && Fila != null;
        btnPresentar.Enabled = !_ocupado && _sesion.UsuarioActual?.RolId == RolesSistema.AdministrativoId && Fila is { Vigente: true, Tipo: TipoPresupuesto.Reparacion, Estado: EstadoPresupuesto.PendientePresentacion };
    }
    private void Ver(object? s, EventArgs e)
    { if (Fila is { } d) { using var f = new FrmConsultarPropuesta(d); f.ShowDialog(this); } }
    private async void Actualizar(object? s, EventArgs e) { if (!_ocupado) await RecargarAsync(); }
    private async void Presentar(object? s, EventArgs e)
    {
        if (_ocupado || Fila is not { } d) return;
        if (MessageBox.Show(this, "¿Confirma que comunicó al cliente el alcance y los importes de esta propuesta? No registra aceptación ni autoriza trabajos.", "Registrar presentación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        _ocupado = true; tlpPrincipal.Enabled = false;
        try { await _servicio.PresentarAsync(d.IdPresupuesto, d.Version); }
        catch (Exception ex) { Error(ex); }
        finally { _ocupado = false; tlpPrincipal.Enabled = true; await RecargarAsync(); }
    }
    private async void Decidir(object? s, EventArgs e)
    {
        if (_ocupado || Fila is not { } d) return;
        using var f = new FrmDecisionPresupuesto(d.IdPresupuesto, _presupuestos); f.ShowDialog(this); await RecargarAsync();
    }
    private async void Revision(object? s, EventArgs e)
    {
        if (_ocupado || Fila is not { } d) return;
        using var f = new Taller.Presentacion.Formularios.Compartidos.FrmSolicitarMotivo("Solicitar revisión de presupuesto",
            "Explique los cambios solicitados. La propuesta anterior se conserva; el técnico prepara una nueva versión que deberá presentarse y aceptarse. Solo se revisa antes de ejecutar trabajos.");
        if (f.ShowDialog(this) != DialogResult.OK) return;
        _ocupado = true; tlpPrincipal.Enabled = false;
        try { var id = await _presupuestos.SolicitarRevisionAsync(d.IdPresupuesto, d.Version, f.Motivo); MessageBox.Show(this, $"Revisión {id:D6} preparada como borrador. El técnico puede continuar desde Evaluaciones técnicas."); }
        catch (Exception ex) { Error(ex); }
        finally { _ocupado = false; tlpPrincipal.Enabled = true; await RecargarAsync(); }
    }
    private void Error(Exception ex) => MessageBox.Show(this, ex.Message, "Presupuestos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    private void AlCerrar(object? s, FormClosingEventArgs e) { if (_ocupado) e.Cancel = true; }
}
