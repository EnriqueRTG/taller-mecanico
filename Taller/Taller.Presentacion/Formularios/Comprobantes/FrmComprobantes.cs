using System.Globalization;
using Taller.Presentacion.Estilos;
using Taller.Presentacion.Formularios.Atenciones;

namespace Taller.Presentacion.Formularios.Comprobantes;

public partial class FrmComprobantes : Form
{
    private const string Todos = "Todos";
    private const string NoEmitido = "No emitido";
    private const string Emitido = "Emitido";
    private const string Pagado = "Pagado";
    private readonly List<ComprobanteDemostrativo> _datos = [];
    private bool _inicializando;

    public FrmComprobantes() { InitializeComponent(); }

    private void FrmComprobantes_Load(object? sender, EventArgs e)
    {
        _inicializando = true;
        AplicarEstilos();
        cboEstado.Items.AddRange([Todos, NoEmitido, Emitido, Pagado]);
        cboSaldo.Items.AddRange([Todos, "Con saldo pendiente", "Sin saldo pendiente"]);
        cboEstado.SelectedIndex = 0;
        cboSaldo.SelectedIndex = 0;
        CargarDatos();
        _inicializando = false;
        AplicarFiltros();
    }

    private void CargarDatos()
    {
        _datos.AddRange(
        [
            new(0, 61, "María González", "Toyota Corolla · AB123CD", 185000m, 0m, NoEmitido),
            new(1028, 59, "Carlos Ramírez", "Volkswagen Gol · AC456EF", 92000m, 45000m, Emitido),
            new(1027, 57, "Lucía Fernández", "Ford Fiesta · AD789GH", 248500m, 248500m, Pagado),
            new(1026, 54, "Jorge Pérez", "Honda Fit · AE321JK", 135400m, 85000m, Emitido),
            new(1025, 47, "Diego Acosta", "Renault Sandero · AG987NP", 316800m, 316800m, Pagado)
        ]);
    }

    private void AplicarFiltros()
    {
        string texto = txtBuscar.Text.Trim();
        string estado = cboEstado.SelectedItem?.ToString() ?? Todos;
        string saldo = cboSaldo.SelectedItem?.ToString() ?? Todos;
        IEnumerable<ComprobanteDemostrativo> resultado = _datos;

        if (!string.IsNullOrWhiteSpace(texto))
            resultado = resultado.Where(c => c.Numero.ToString().Contains(texto, StringComparison.OrdinalIgnoreCase)
                || c.Atencion.ToString().Contains(texto, StringComparison.OrdinalIgnoreCase)
                || c.Cliente.Contains(texto, StringComparison.OrdinalIgnoreCase)
                || c.Vehiculo.Contains(texto, StringComparison.OrdinalIgnoreCase));
        if (estado != Todos) resultado = resultado.Where(c => c.Estado == estado);
        if (saldo == "Con saldo pendiente") resultado = resultado.Where(c => c.Saldo > 0);
        if (saldo == "Sin saldo pendiente") resultado = resultado.Where(c => c.Saldo <= 0);

        dgvComprobantes.Rows.Clear();
        foreach (ComprobanteDemostrativo c in resultado.OrderByDescending(c => c.Atencion))
        {
            int i = dgvComprobantes.Rows.Add(c.Numero == 0 ? "Sin emitir" : $"C-{c.Numero:000000}",
                $"A-{c.Atencion:000000}", c.Cliente, c.Vehiculo, Formatear(c.Total), Formatear(c.Abonado), Formatear(c.Saldo), c.Estado);
            dgvComprobantes.Rows[i].Tag = c;
            DataGridViewCell celda = dgvComprobantes.Rows[i].Cells[colEstado.Index];
            celda.Style.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            celda.Style.ForeColor = c.Estado == Pagado ? Color.FromArgb(21, 128, 61) : Color.FromArgb(180, 83, 9);
            if (c.Saldo > 0) dgvComprobantes.Rows[i].Cells[colSaldo.Index].Style.ForeColor = Color.FromArgb(185, 28, 28);
        }

        lblResumen.Text = $"Saldo pendiente: {Formatear(_datos.Sum(c => c.Saldo))}";
        lblCantidad.Text = $"{dgvComprobantes.Rows.Count} comprobantes";
        dgvComprobantes.ClearSelection();
        dgvComprobantes.CurrentCell = null;
        ActualizarAcciones();
    }

    private static string Formatear(decimal valor) => valor.ToString("C0", CultureInfo.GetCultureInfo("es-AR"));
    private ComprobanteDemostrativo? Seleccionado() => dgvComprobantes.CurrentRow?.Tag as ComprobanteDemostrativo;

    private void ActualizarAcciones()
    {
        ComprobanteDemostrativo? c = Seleccionado();
        btnGestionar.Enabled = c is not null;
        btnVerPagos.Enabled = c is not null;
        btnRegistrarEntrega.Enabled = c?.Estado == Pagado;
    }

    private void BtnGestionar_Click(object? sender, EventArgs e)
    {
        ComprobanteDemostrativo? c = Seleccionado();
        if (c is null)
        {
            MessageBox.Show(this, "Seleccione una atención finalizada.", "Comprobantes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        using var formulario = new FrmComprobantePagos();
        formulario.ShowDialog(this);
        if (c.Estado == NoEmitido) { c.Numero = _datos.Max(x => x.Numero) + 1; c.Estado = Emitido; }
        AplicarFiltros();
    }

    private void BtnRegistrarEntrega_Click(object? sender, EventArgs e)
    {
        if (Seleccionado()?.Estado != Pagado) return;
        using var formulario = new FrmRegistrarEntrega();
        if (formulario.ShowDialog(this) == DialogResult.OK)
            MessageBox.Show(this, "La atención quedó marcada visualmente como entregada.", "Entrega registrada", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void DgvComprobantes_CellDoubleClick(object? sender, DataGridViewCellEventArgs e) { if (e.RowIndex >= 0) BtnGestionar_Click(sender, EventArgs.Empty); }
    private void Filtros_Changed(object? sender, EventArgs e) { if (!_inicializando) AplicarFiltros(); }
    private void BtnLimpiar_Click(object? sender, EventArgs e) { _inicializando = true; txtBuscar.Clear(); cboEstado.SelectedIndex = 0; cboSaldo.SelectedIndex = 0; _inicializando = false; AplicarFiltros(); }
    private void BtnActualizar_Click(object? sender, EventArgs e) => AplicarFiltros();
    private void DgvComprobantes_SelectionChanged(object? sender, EventArgs e) => ActualizarAcciones();

    private void AplicarEstilos()
    {
        EstilosInterfaz.AplicarBotonPrimario(btnGestionar);
        EstilosInterfaz.AplicarBotonSecundario(btnVerPagos);
        EstilosInterfaz.AplicarBotonPrimario(btnRegistrarEntrega);
        EstilosInterfaz.AplicarBotonSecundario(btnActualizar);
        EstilosInterfaz.AplicarBotonSecundario(btnLimpiar);
        EstilosInterfaz.AplicarGrilla(dgvComprobantes);
    }

    private sealed class ComprobanteDemostrativo(int numero, int atencion, string cliente, string vehiculo, decimal total, decimal abonado, string estado)
    {
        public int Numero { get; set; } = numero;
        public int Atencion { get; } = atencion;
        public string Cliente { get; } = cliente;
        public string Vehiculo { get; } = vehiculo;
        public decimal Total { get; } = total;
        public decimal Abonado { get; } = abonado;
        public decimal Saldo => Total - Abonado;
        public string Estado { get; set; } = estado;
    }
}
