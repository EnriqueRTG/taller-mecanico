using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;

namespace Taller.Presentacion.Formularios.Diagnosticos;
/// <summary>Bandeja de trabajo técnico: reserva, continuación, liberación y consulta de informes.</summary>
public partial class FrmDiagnosticos : Form, Taller.Presentacion.Navegacion.IFormularioConOperacion
{
    private readonly IServiceProvider _proveedor;
    private readonly EvaluacionServicio _servicio;
    private readonly DiagnosticoServicio _diagnosticos;
    private readonly SesionUsuario _sesion;
    private List<EvaluacionBandejaDto> _datos = [];
    private int? _abrir;
    private bool _ocupado;
    private bool _cargando;
    public bool OperacionEnCurso => _ocupado || _cargando;

    public FrmDiagnosticos(EvaluacionServicio servicio, DiagnosticoServicio diagnosticos, SesionUsuario sesion, IServiceProvider proveedor)
    {
        InitializeComponent();
        _proveedor = proveedor;
        _servicio = servicio;
        _diagnosticos = diagnosticos;
        _sesion = sesion;
    }

    public void PrepararAperturaDiagnostico(int idAtencion) => _abrir = idAtencion;
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
        if (_abrir is int id)
        {
            _abrir = null;
            await AbrirAsync(id);
        }
    }

    private async Task RecargarAsync()
    {
        if (_cargando)
            return;
        _cargando = true;
        tlpPrincipal.Enabled = false;
        try
        {
            _datos = await _servicio.ListarAsync();
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

    private async void Actualizar(object? s, EventArgs e)
    {
        if (!_ocupado)
            await RecargarAsync();
    }

    private void Filtrar(object? s, EventArgs e)
    {
        var id = _sesion.UsuarioActual?.Id;
        var q = txtBuscar.Text.Trim();
        var filtro = cboEstado.SelectedIndex;
        dgvDatos.DataSource = _datos.Where(d => ($"{d.IdAtencion:D6} {d.Cliente} {d.Vehiculo} {d.Motivo} {d.Tecnico}").Contains(q, StringComparison.OrdinalIgnoreCase)).Where(d => filtro == 0 || filtro == 1 && !d.Finalizada && d.IdTecnico == id || filtro == 2 && !d.Finalizada && d.IdTecnico == null || filtro == 3 && d.Finalizada).OrderBy(d => d.Finalizada ? 3 : d.IdTecnico == id ? 0 : d.IdTecnico == null ? 1 : 2).ThenBy(d => d.Finalizada ? DateTime.MaxValue.Ticks - d.Fecha.Ticks : d.Fecha.Ticks).ThenBy(d => d.IdAtencion).ToList();
        dgvDatos.ClearSelection();
        dgvDatos.CurrentCell = null;
        Seleccion(null, EventArgs.Empty);
    }

    private EvaluacionBandejaDto? Fila => dgvDatos.CurrentRow?.DataBoundItem as EvaluacionBandejaDto;

    private void Seleccion(object? s, EventArgs e)
    {
        var d = Fila;
        var tech = _sesion.UsuarioActual?.RolId == RolesSistema.TecnicoId;
        btnTrabajar.Visible = btnLiberar.Visible = tech;
        btnTrabajar.Enabled = !_ocupado && tech && d is { Finalizada: false } && (d.IdTecnico == null || d.IdTecnico == _sesion.UsuarioActual?.Id);
        btnTrabajar.Text = d?.IdTecnico == _sesion.UsuarioActual?.Id ? "Continuar" : d?.TipoSolicitud == Taller.Dominio.Enumeraciones.TipoSolicitudAtencion.ServicioDefinido ? "Preparar presupuesto" : "Diagnosticar y presupuestar";
        btnLiberar.Enabled = !_ocupado && tech && d is { Finalizada: false } && d.IdTecnico == _sesion.UsuarioActual?.Id;
        btnInforme.Enabled = !_ocupado && d?.IdDiagnostico != null;
        lblEstado.Text = d is null ? "Seleccione una atención. Mis actividades primero; disponibles por antigüedad; finalizadas al final." : d.Finalizada ? "Evaluación finalizada. Consulte la propuesta en Presupuestos." : d.IdTecnico == null ? "Disponible para tomar." : $"Reservada por {d.Tecnico}. Cerrar el formulario no la libera.";
        Taller.Presentacion.Estilos.AccionesContextuales.Actualizar(btnTrabajar, btnLiberar, btnInforme);
    }

    private async void Trabajar(object? s, EventArgs e)
    {
        if (Fila is { } d)
            await AbrirAsync(d.IdAtencion);
    }

    private async Task AbrirAsync(int id)
    {
        if (_ocupado || _sesion.UsuarioActual?.RolId != RolesSistema.TecnicoId)
            return;
        _ocupado = true;
        tlpPrincipal.Enabled = false;
        try
        {
            var d = _datos.FirstOrDefault(d => d.IdAtencion == id);
            if (d == null || d.Finalizada)
                return;
            if (d.IdTecnico == null)
                await _servicio.TomarAsync(id);
            using var f = new FrmEvaluarAtencion(id, _servicio, _proveedor);
            f.ShowDialog(this);
            if (f.Finalizada)
                MessageBox.Show(this, "Evaluación finalizada. La propuesta está disponible en Presupuestos para su presentación por Recepción.");
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

    private async void Liberar(object? s, EventArgs e)
    {
        if (_ocupado || Fila is not { } d)
            return;
        if (MessageBox.Show(this, "¿Liberar la evaluación para otro técnico? Se conservan los avances guardados.", "Liberar", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;
        _ocupado = true;
        tlpPrincipal.Enabled = false;
        try
        {
            await _servicio.LiberarAsync(d.IdAtencion);
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

    private void Informe(object? s, EventArgs e)
    {
        if (Fila?.IdDiagnostico is int id)
        {
            using var f = new FrmDetalleDiagnostico(id, _diagnosticos, Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto>(_proveedor));
            f.ShowDialog(this);
        }
    }

    private void Error(Exception ex) => Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Evaluaciones técnicas");
    private void AlCerrar(object? s, FormClosingEventArgs e)
    {
        if (_ocupado)
            e.Cancel = true;
    }
}
