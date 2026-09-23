using Taller.Dominio.Constantes;
using Taller.Presentacion.Estilos;
using Taller.Presentacion.Formularios.Atenciones;

namespace Taller.Presentacion.Formularios.Diagnosticos;

/// <summary>
/// Lista las atenciones disponibles para consulta o diagnóstico técnico.
/// </summary>
public partial class FrmDiagnosticos : Form
{
    private const string Todos = "Todos";
    private const string Pendiente = "Pendiente";
    private const string Registrado = "Registrado";
    private readonly List<DiagnosticoDemostrativo> _datos = [];
    private bool _inicializando;

    public FrmDiagnosticos()
    {
        InitializeComponent();
    }

    private void FrmDiagnosticos_Load(object? sender, EventArgs e)
    {
        _inicializando = true;
        AplicarEstilos();
        cboEstado.Items.AddRange([Todos, Pendiente, Registrado]);
        cboTecnico.Items.AddRange([Todos, "Técnico actual", "Laura Medina", "Martín López"]);
        cboEstado.SelectedIndex = 0;
        cboTecnico.SelectedIndex = 0;
        CargarDatos();
        _inicializando = false;
        AplicarFiltros();
    }

    private void CargarDatos()
    {
        _datos.AddRange(
        [
            new(61, DateTime.Today.AddDays(-1), "María González", "Toyota Corolla · AB123CD", "Ruido al frenar", "Técnico actual", Pendiente),
            new(60, DateTime.Today.AddDays(-2), "Pedro Gómez", "Fiat Cronos · AC234DE", "Pérdida de potencia", "Técnico actual", Pendiente),
            new(59, DateTime.Today.AddDays(-4), "Carlos Ramírez", "Volkswagen Gol · AC456EF", "No enciende en frío", "Laura Medina", Registrado),
            new(57, DateTime.Today.AddDays(-8), "Lucía Fernández", "Ford Fiesta · AD789GH", "Temperatura elevada", "Martín López", Registrado),
            new(54, DateTime.Today.AddDays(-12), "Jorge Pérez", "Honda Fit · AE321JK", "Mantenimiento general", "Técnico actual", Registrado)
        ]);
    }

    private void AplicarFiltros()
    {
        string texto = txtBuscar.Text.Trim();
        string estado = cboEstado.SelectedItem?.ToString() ?? Todos;
        string tecnico = cboTecnico.SelectedItem?.ToString() ?? Todos;

        IEnumerable<DiagnosticoDemostrativo> resultado = _datos;
        if (!string.IsNullOrWhiteSpace(texto))
        {
            resultado = resultado.Where(d =>
                d.Atencion.ToString().Contains(texto, StringComparison.OrdinalIgnoreCase)
                || d.Cliente.Contains(texto, StringComparison.OrdinalIgnoreCase)
                || d.Vehiculo.Contains(texto, StringComparison.OrdinalIgnoreCase)
                || d.Motivo.Contains(texto, StringComparison.OrdinalIgnoreCase));
        }
        if (estado != Todos) resultado = resultado.Where(d => d.Estado == estado);
        if (tecnico != Todos) resultado = resultado.Where(d => d.Tecnico == tecnico);

        dgvDiagnosticos.Rows.Clear();
        foreach (DiagnosticoDemostrativo d in resultado.OrderByDescending(d => d.Fecha))
        {
            int i = dgvDiagnosticos.Rows.Add($"A-{d.Atencion:000000}", d.Fecha.ToString("dd/MM/yyyy"), d.Cliente, d.Vehiculo, d.Motivo, d.Tecnico, d.Estado);
            dgvDiagnosticos.Rows[i].Tag = d;
            dgvDiagnosticos.Rows[i].Cells[colEstado.Index].Style.ForeColor = d.Estado == Registrado
                ? Color.FromArgb(21, 128, 61)
                : Color.FromArgb(180, 83, 9);
            dgvDiagnosticos.Rows[i].Cells[colEstado.Index].Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        int pendientes = _datos.Count(d => d.Estado == Pendiente);
        lblResumen.Text = pendientes == 1 ? "1 pendiente" : $"{pendientes} pendientes";
        lblCantidad.Text = $"{dgvDiagnosticos.Rows.Count} diagnósticos";
        dgvDiagnosticos.ClearSelection();
        dgvDiagnosticos.CurrentCell = null;
        ActualizarAcciones();
    }

    private DiagnosticoDemostrativo? Seleccionado() =>
        dgvDiagnosticos.CurrentRow?.Tag as DiagnosticoDemostrativo;

    private void ActualizarAcciones()
    {
        DiagnosticoDemostrativo? d = Seleccionado();
        btnVerAtencion.Enabled = d is not null;
        btnRegistrarSeleccionado.Enabled = d?.Estado == Pendiente;
        btnRegistrar.Enabled = d?.Estado == Pendiente;
    }

    private void BtnRegistrar_Click(object? sender, EventArgs e)
    {
        DiagnosticoDemostrativo? d = Seleccionado();
        if (d?.Estado != Pendiente)
        {
            MessageBox.Show(this, "Seleccione una atención pendiente de diagnóstico.", "Diagnósticos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var formulario = new FrmRegistrarDiagnostico();
        if (formulario.ShowDialog(this) == DialogResult.OK)
        {
            d.Estado = Registrado;
            AplicarFiltros();
        }
    }

    private void BtnVerAtencion_Click(object? sender, EventArgs e)
    {
        if (Seleccionado() is null) return;
        using var formulario = new FrmDetalleAtencion(RolesSistema.TecnicoId);
        formulario.ShowDialog(this);
    }

    private void DgvDiagnosticos_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) BtnVerAtencion_Click(sender, EventArgs.Empty);
    }

    private void Filtros_Changed(object? sender, EventArgs e)
    {
        if (!_inicializando) AplicarFiltros();
    }

    private void BtnLimpiar_Click(object? sender, EventArgs e)
    {
        _inicializando = true;
        txtBuscar.Clear();
        cboEstado.SelectedIndex = 0;
        cboTecnico.SelectedIndex = 0;
        _inicializando = false;
        AplicarFiltros();
    }

    private void BtnActualizar_Click(object? sender, EventArgs e) => AplicarFiltros();
    private void DgvDiagnosticos_SelectionChanged(object? sender, EventArgs e) => ActualizarAcciones();

    private void AplicarEstilos()
    {
        EstilosInterfaz.AplicarBotonPrimario(btnRegistrar);
        EstilosInterfaz.AplicarBotonPrimario(btnRegistrarSeleccionado);
        EstilosInterfaz.AplicarBotonSecundario(btnVerAtencion);
        EstilosInterfaz.AplicarBotonSecundario(btnActualizar);
        EstilosInterfaz.AplicarBotonSecundario(btnLimpiar);
        EstilosInterfaz.AplicarGrilla(dgvDiagnosticos);
    }

    private sealed class DiagnosticoDemostrativo(int atencion, DateTime fecha, string cliente, string vehiculo, string motivo, string tecnico, string estado)
    {
        public int Atencion { get; } = atencion;
        public DateTime Fecha { get; } = fecha;
        public string Cliente { get; } = cliente;
        public string Vehiculo { get; } = vehiculo;
        public string Motivo { get; } = motivo;
        public string Tecnico { get; } = tecnico;
        public string Estado { get; set; } = estado;
    }
}
