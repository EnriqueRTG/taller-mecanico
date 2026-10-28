using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.DTOs.Ejecuciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Presentacion.Formularios.Atenciones;
namespace Taller.Presentacion.Formularios.Ejecuciones;
/// <summary>Bandeja de ejecuciones autorizadas. El técnico registra; recepción consulta el mismo progreso persistido.</summary>
public partial class FrmTrabajosEnCurso : Form, Navegacion.IFormularioConOperacion
{
    private readonly EjecucionServicio _servicio;
    private readonly SesionUsuario _sesion;
    private readonly IServiceProvider _proveedor;
    private List<TrabajoEnCursoDto> _datos = [];
    public bool OperacionEnCurso { get; private set; }
    public FrmTrabajosEnCurso(EjecucionServicio servicio, SesionUsuario sesion, IServiceProvider proveedor)
    {
        InitializeComponent(); _servicio = servicio; _sesion = sesion; _proveedor = proveedor; Seleccion(null, EventArgs.Empty);
    }
    private async void Cargar(object? s, EventArgs e) => await RecargarAsync();
    private async void Actualizar(object? s, EventArgs e) => await RecargarAsync();
    private async Task RecargarAsync()
    {
        if (OperacionEnCurso) return;
        OperacionEnCurso = true; tlpPrincipal.Enabled = false;
        try { _datos = await _servicio.ListarAsync(); Filtrar(null, EventArgs.Empty); }
        catch (Exception ex) { _datos = []; dgvDatos.DataSource = null; Seleccion(null, EventArgs.Empty); Error(ex); }
        finally { OperacionEnCurso = false; tlpPrincipal.Enabled = true; Seleccion(null, EventArgs.Empty); }
    }
    private void Limpiar(object? s, EventArgs e) { txtBuscar.Clear(); cboEstado.SelectedIndex = 0; Filtrar(s, e); }
    private void Filtrar(object? s, EventArgs e)
    {
        var q = txtBuscar.Text.Trim(); var id = _sesion.UsuarioActual?.Id;
        dgvDatos.DataSource = _datos.Where(d => Taller.Aplicacion.Consultas.BusquedaTexto.Coincide($"{d.IdAtencion:D6} {d.Cliente} {d.Vehiculo} {d.Motivo}", q))
            .Where(d => cboEstado.SelectedIndex <= 0 || cboEstado.SelectedIndex == 1 && d.IdTecnico == null || cboEstado.SelectedIndex == 2 && d.IdTecnico != null || cboEstado.SelectedIndex == 3 && d.Pausas > 0)
            .OrderBy(d => d.IdTecnico == id ? 0 : d.IdTecnico == null ? 1 : 2).ThenByDescending(d => d.Pausas > 0).ThenByDescending(d => d.Fecha).ThenBy(d => d.IdAtencion).ToList();
        dgvDatos.ClearSelection(); dgvDatos.CurrentCell = null; Seleccion(s, e);
    }
    private TrabajoEnCursoDto? Fila => dgvDatos.CurrentRow?.DataBoundItem as TrabajoEnCursoDto;
    private void Seleccion(object? s, EventArgs e)
    {
        var tecnico = _sesion.UsuarioActual?.RolId == RolesSistema.TecnicoId;
        btnGestionar.Visible = tecnico;
        btnGestionar.Enabled = !OperacionEnCurso && tecnico && Fila is { } d && (d.IdTecnico == null || d.IdTecnico == _sesion.UsuarioActual?.Id);
        btnProgreso.Enabled = !OperacionEnCurso && Fila != null;
        lblAyuda.Text = Fila is null ? "Mis trabajos primero, luego disponibles. Seleccione una atención; los avances guardados se consultan desde Ver progreso. Cerradas y canceladas no se incluyen."
            : $"{Fila.Progreso}. {(Fila.Pausas > 0 ? "Hay pausas: consulte sus causas antes de continuar." : "Cerrar la ejecución conserva la reserva del técnico.")}";
        Taller.Presentacion.Estilos.AccionesContextuales.Actualizar(btnGestionar, btnProgreso);
    }
    private async void Gestionar(object? s, EventArgs e)
    {
        if (!btnGestionar.Enabled || Fila is not { } d) return;
        OperacionEnCurso = true;
        try { using var f = new FrmEjecutarAtencion(d.IdAtencion, _servicio, _sesion, _proveedor); f.ShowDialog(this); }
        catch (Exception ex) { Error(ex); }
        finally { OperacionEnCurso = false; await RecargarAsync(); }
    }
    private async void Progreso(object? s, EventArgs e)
    {
        if (OperacionEnCurso || Fila is not { } d) return;
        OperacionEnCurso = true;
        try { using var f = new FrmSeguimientoTrabajo(d.IdAtencion, _proveedor.GetRequiredService<SeguimientoTrabajoServicio>(), _proveedor.GetRequiredService<EvaluacionServicio>(), _proveedor.GetRequiredService<PresupuestoServicio>(), _proveedor.GetRequiredService<Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto>()); f.ShowDialog(this); }
        catch (Exception ex) { Error(ex); }
        finally { OperacionEnCurso = false; await RecargarAsync(); }
    }
    private void Error(Exception ex) => Errores.ErroresInterfaz.Mostrar(this, ex, "Trabajos en curso");
}
