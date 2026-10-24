using Taller.Aplicacion.DTOs.Cobros;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Enumeraciones;
using Taller.Presentacion.Navegacion;

namespace Taller.Presentacion.Formularios.Comprobantes;
/// <summary>Prioriza saldos pendientes y atenciones listas para liquidar; conserva consultas históricas.</summary>
public partial class FrmComprobantes : Form, IFormularioConOperacion
{
    private readonly CobroServicio _servicio;
    private readonly Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto _empresa;
    private List<AtencionCobroDto> _datos = [];
    private bool _cargando;
    public bool OperacionEnCurso => _cargando;

    public FrmComprobantes(CobroServicio servicio, Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto empresa)
    {
        InitializeComponent();
        _servicio = servicio;
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
            _datos = await _servicio.ListarAsync();
            Filtrar(null, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            _datos = [];
            dgvDatos.DataSource = null;
            Seleccion(null, EventArgs.Empty);
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Cobros");
        }
        finally
        {
            _cargando = false;
            tlpPrincipal.Enabled = true;
        }
    }

    private void Filtrar(object? s, EventArgs e)
    {
        var q = txtBuscar.Text.Trim();
        dgvDatos.DataSource = _datos.Where(d => $"{d.IdAtencion:D6} {d.IdComprobante} {d.Cliente} {d.Dominio}".Contains(q, StringComparison.OrdinalIgnoreCase)).Where(d => cboEstado.SelectedIndex <= 0 || cboEstado.SelectedIndex == 1 && d.IdComprobante == null && d.Estado != EstadoAtencion.Cerrada || cboEstado.SelectedIndex == 2 && d.Saldo > 0 || cboEstado.SelectedIndex == 3 && d.Estado != EstadoAtencion.Cerrada && d.Saldo == 0 || cboEstado.SelectedIndex == 4 && d.Estado == EstadoAtencion.Cerrada).OrderBy(d => d.Estado == EstadoAtencion.Cerrada ? 3 : d.Saldo > 0 ? 0 : d.IdComprobante == null ? 1 : 2).ThenByDescending(d => d.Fecha).ThenByDescending(d => d.IdAtencion).ToList();
        dgvDatos.ClearSelection();
        dgvDatos.CurrentCell = null;
        Seleccion(null, EventArgs.Empty);
    }

    private void Seleccion(object? s, EventArgs e) => btnGestionar.Enabled = dgvDatos.CurrentRow?.DataBoundItem is AtencionCobroDto;
    private async void Gestionar(object? s, EventArgs e)
    {
        if (_cargando || dgvDatos.CurrentRow?.DataBoundItem is not AtencionCobroDto d)
            return;
        using var f = new FrmComprobantePagos(d.IdAtencion, _servicio, _empresa);
        f.ShowDialog(this);
        await RecargarAsync();
    }

    private async void Actualizar(object? s, EventArgs e) => await RecargarAsync();
    private void AlCerrar(object? s, FormClosingEventArgs e)
    {
        if (_cargando)
            e.Cancel = true;
    }
}
