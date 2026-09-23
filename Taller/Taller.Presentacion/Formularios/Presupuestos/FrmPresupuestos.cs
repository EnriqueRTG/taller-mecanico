using System.Globalization;
using Taller.Presentacion.Estilos;

namespace Taller.Presentacion.Formularios.Presupuestos;

/// <summary>
/// Representa el centro de consulta y navegación del módulo
/// de presupuestos.
/// </summary>
/// <remarks>
/// Durante la etapa actual utiliza información demostrativa y
/// no realiza operaciones de persistencia.
/// </remarks>
public partial class FrmPresupuestos : Form
{
    private const string Todos = "Todos";
    private const string PendienteElaboracion = "Pendiente de elaboración";
    private const string PendienteDecision = "Pendiente de decisión";
    private const string Aprobado = "Aprobado";
    private const string Rechazado = "Rechazado";

    private readonly List<PresupuestoDemostrativo> _presupuestos = [];
    private bool _inicializando;

    public FrmPresupuestos()
    {
        InitializeComponent();
    }

    private void FrmPresupuestos_Load(object? sender, EventArgs e)
    {
        _inicializando = true;
        AplicarEstilos();
        ConfigurarFiltros();
        CargarDatosDemostrativos();
        _inicializando = false;
        AplicarFiltros();
    }

    private void ConfigurarFiltros()
    {
        cboEstado.Items.AddRange(
        [
            Todos,
            PendienteElaboracion,
            PendienteDecision,
            Aprobado,
            Rechazado
        ]);

        cboEstado.SelectedIndex = 0;
        dtpDesde.Value = DateTime.Today.AddMonths(-3);
        dtpHasta.Value = DateTime.Today;
        dtpDesde.MaxDate = DateTime.Today;
        dtpHasta.MaxDate = DateTime.Today;
    }

    private void CargarDatosDemostrativos()
    {
        _presupuestos.AddRange(
        [
            new(38, DateTime.Today.AddDays(-1), 61, "María González",
                "Toyota Corolla - AB123CD", 185000m, PendienteDecision),
            new(37, DateTime.Today.AddDays(-4), 59, "Carlos Ramírez",
                "Volkswagen Gol - AC456EF", 92000m, Aprobado),
            new(36, DateTime.Today.AddDays(-8), 57, "Lucía Fernández",
                "Ford Fiesta - AD789GH", 248500m, Rechazado),
            new(35, DateTime.Today.AddDays(-12), 54, "Jorge Pérez",
                "Honda Fit - AE321JK", 135400m, PendienteDecision),
            new(34, DateTime.Today.AddDays(-20), 51, "Ana Benítez",
                "Chevrolet Onix - AF654LM", 0m, PendienteElaboracion),
            new(33, DateTime.Today.AddDays(-36), 47, "Diego Acosta",
                "Renault Sandero - AG987NP", 316800m, Aprobado)
        ]);
    }

    private void AplicarFiltros()
    {
        string texto = txtBuscar.Text.Trim();
        string estado = cboEstado.SelectedItem?.ToString() ?? Todos;
        DateTime desde = dtpDesde.Value.Date;
        DateTime hasta = dtpHasta.Value.Date;

        IEnumerable<PresupuestoDemostrativo> resultado = _presupuestos
            .Where(p => p.Fecha.Date >= desde && p.Fecha.Date <= hasta);

        if (!string.IsNullOrWhiteSpace(texto))
        {
            resultado = resultado.Where(p =>
                p.Numero.ToString().Contains(texto, StringComparison.OrdinalIgnoreCase)
                || p.Atencion.ToString().Contains(texto, StringComparison.OrdinalIgnoreCase)
                || p.Cliente.Contains(texto, StringComparison.OrdinalIgnoreCase)
                || p.Vehiculo.Contains(texto, StringComparison.OrdinalIgnoreCase));
        }

        if (estado != Todos)
        {
            resultado = resultado.Where(p => p.Estado == estado);
        }

        CargarGrilla(resultado
            .OrderByDescending(p => p.Fecha)
            .ThenByDescending(p => p.Numero));
    }

    private void CargarGrilla(IEnumerable<PresupuestoDemostrativo> presupuestos)
    {
        dgvPresupuestos.Rows.Clear();

        foreach (PresupuestoDemostrativo presupuesto in presupuestos)
        {
            int indice = dgvPresupuestos.Rows.Add(
                $"P-{presupuesto.Numero:000000}",
                presupuesto.Fecha.ToString("dd/MM/yyyy"),
                $"A-{presupuesto.Atencion:000000}",
                presupuesto.Cliente,
                presupuesto.Vehiculo,
                presupuesto.Total > 0 ? FormatearImporte(presupuesto.Total) : "—",
                presupuesto.Estado);

            DataGridViewRow fila = dgvPresupuestos.Rows[indice];
            fila.Tag = presupuesto;
            AplicarColorEstado(fila.Cells[colEstado.Index], presupuesto.Estado);
        }

        lblCantidad.Text = dgvPresupuestos.Rows.Count == 1
            ? "1 presupuesto"
            : $"{dgvPresupuestos.Rows.Count} presupuestos";

        int pendientes = _presupuestos.Count(p => p.Estado == PendienteDecision);
        lblResumen.Text = pendientes == 1
            ? "1 pendiente de decisión"
            : $"{pendientes} pendientes de decisión";

        dgvPresupuestos.ClearSelection();
        dgvPresupuestos.CurrentCell = null;
        ActualizarAcciones();
    }

    private static string FormatearImporte(decimal importe)
    {
        return importe.ToString("C0", CultureInfo.GetCultureInfo("es-AR"));
    }

    private static void AplicarColorEstado(DataGridViewCell celda, string estado)
    {
        celda.Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        celda.Style.ForeColor = estado switch
        {
            Aprobado => Color.FromArgb(21, 128, 61),
            Rechazado => Color.FromArgb(185, 28, 28),
            PendienteDecision => Color.FromArgb(180, 83, 9),
            _ => Color.FromArgb(30, 64, 175)
        };
    }

    private PresupuestoDemostrativo? ObtenerSeleccionado()
    {
        return dgvPresupuestos.CurrentRow?.Tag as PresupuestoDemostrativo;
    }

    private void ActualizarAcciones()
    {
        PresupuestoDemostrativo? presupuesto = ObtenerSeleccionado();
        btnVerDetalle.Enabled = presupuesto is not null;
        btnRegistrarDecision.Enabled = presupuesto?.Estado == PendienteDecision;
    }

    private void Filtros_Changed(object? sender, EventArgs e)
    {
        if (_inicializando || dtpDesde.Value.Date > dtpHasta.Value.Date)
        {
            return;
        }

        AplicarFiltros();
    }

    private void BtnLimpiar_Click(object? sender, EventArgs e)
    {
        _inicializando = true;
        txtBuscar.Clear();
        cboEstado.SelectedIndex = 0;
        dtpDesde.Value = DateTime.Today.AddMonths(-3);
        dtpHasta.Value = DateTime.Today;
        _inicializando = false;

        AplicarFiltros();
        txtBuscar.Focus();
    }

    private void BtnGenerar_Click(object? sender, EventArgs e)
    {
        using var formulario = new FrmGenerarPresupuesto();

        if (formulario.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        int siguienteNumero = _presupuestos.Max(p => p.Numero) + 1;
        _presupuestos.Add(new PresupuestoDemostrativo(
            siguienteNumero,
            DateTime.Today,
            62,
            "Cliente seleccionado",
            "Vehículo de la atención",
            formulario.TotalPresupuesto,
            PendienteDecision));

        AplicarFiltros();
    }

    private void BtnVerDetalle_Click(object? sender, EventArgs e)
    {
        MostrarDetalleSeleccionado();
    }

    private void DgvPresupuestos_CellDoubleClick(
        object? sender,
        DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) MostrarDetalleSeleccionado();
    }

    private void MostrarDetalleSeleccionado()
    {
        PresupuestoDemostrativo? presupuesto = ObtenerSeleccionado();
        if (presupuesto is null) return;

        using var formulario = new FrmDetallePresupuesto(
            presupuesto.Numero,
            presupuesto.Atencion,
            presupuesto.Fecha,
            presupuesto.Cliente,
            presupuesto.Vehiculo,
            presupuesto.Total,
            presupuesto.Estado);

        formulario.ShowDialog(this);
    }

    private void BtnRegistrarDecision_Click(object? sender, EventArgs e)
    {
        PresupuestoDemostrativo? presupuesto = ObtenerSeleccionado();
        if (presupuesto?.Estado != PendienteDecision) return;

        using var formulario = new FrmDecisionPresupuesto();

        if (formulario.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        presupuesto.Estado = formulario.Decision == "Aceptado"
            ? Aprobado
            : Rechazado;
        AplicarFiltros();
    }

    private void BtnActualizar_Click(object? sender, EventArgs e)
    {
        AplicarFiltros();
        MessageBox.Show(
            "El listado demostrativo fue actualizado.",
            "Presupuestos",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void DgvPresupuestos_SelectionChanged(object? sender, EventArgs e)
    {
        ActualizarAcciones();
    }

    private void AplicarEstilos()
    {
        BackColor = EstilosInterfaz.FondoAplicacion;
        EstilosInterfaz.AplicarBotonPrimario(btnGenerar);
        EstilosInterfaz.AplicarBotonPrimario(btnRegistrarDecision);
        EstilosInterfaz.AplicarBotonSecundario(btnVerDetalle);
        EstilosInterfaz.AplicarBotonSecundario(btnActualizar);
        EstilosInterfaz.AplicarBotonSecundario(btnLimpiar);
        EstilosInterfaz.AplicarGrilla(dgvPresupuestos);
    }

    private sealed class PresupuestoDemostrativo(
        int numero,
        DateTime fecha,
        int atencion,
        string cliente,
        string vehiculo,
        decimal total,
        string estado)
    {
        public int Numero { get; } = numero;
        public DateTime Fecha { get; } = fecha;
        public int Atencion { get; } = atencion;
        public string Cliente { get; } = cliente;
        public string Vehiculo { get; } = vehiculo;
        public decimal Total { get; } = total;
        public string Estado { get; set; } = estado;
    }
}
