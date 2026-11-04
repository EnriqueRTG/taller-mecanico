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
        Taller.Presentacion.Estilos.ValoresGrilla.Aplicar(dgvDatos);
        _servicio = servicio;
        _empresa = empresa;
    }

    private void LimpiarFiltros_Click(object? sender, EventArgs e)
    {
        if (OperacionEnCurso) return;
        txtBuscar.Clear();
        cboEstado.SelectedIndex = 0;
        dtpDesde.Checked = dtpHasta.Checked = false;
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
        if (dtpDesde is null || dtpHasta is null) return;
        var q = txtBuscar.Text.Trim();
        dgvDatos.DataSource = _datos.Where(d => Taller.Presentacion.Estilos.FiltrosBandeja.Incluye(d.Fecha, dtpDesde, dtpHasta)).Where(d => $"{d.IdAtencion:D6} {d.IdComprobante} {d.Cliente} {d.Dominio}".Contains(q, StringComparison.OrdinalIgnoreCase)).Where(d => cboEstado.SelectedIndex <= 0 || cboEstado.SelectedIndex == 1 && d.IdComprobante == null && d.Estado != EstadoAtencion.Cerrada || cboEstado.SelectedIndex == 2 && d.Saldo > 0 || cboEstado.SelectedIndex == 3 && d.Estado != EstadoAtencion.Cerrada && d.Saldo == 0 || cboEstado.SelectedIndex == 4 && d.Estado == EstadoAtencion.Cerrada).OrderBy(d => d.Estado == EstadoAtencion.Cerrada ? 3 : d.Saldo > 0 ? 0 : d.IdComprobante == null ? 1 : 2).ThenByDescending(d => d.Fecha).ThenByDescending(d => d.IdAtencion).ToList();
        dgvDatos.ClearSelection();
        dgvDatos.CurrentCell = null;
        Seleccion(null, EventArgs.Empty);
        if (!Taller.Presentacion.Estilos.FiltrosBandeja.RangoValido(dtpDesde, dtpHasta))
            lblAyuda.Text = "El rango no es válido: Desde debe ser anterior o igual a Hasta.";
    }

    private void Seleccion(object? s, EventArgs e) => btnGestionar.Visible = btnGestionar.Enabled = dgvDatos.CurrentRow?.DataBoundItem is AtencionCobroDto;
    private void AbrirConDobleClick(object? s, DataGridViewCellEventArgs e) { if (e.RowIndex >= 0) Gestionar(s, EventArgs.Empty); }
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
