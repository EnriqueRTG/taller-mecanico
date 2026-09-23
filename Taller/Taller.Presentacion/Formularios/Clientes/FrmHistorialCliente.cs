namespace Taller.Presentacion.Formularios.Clientes;

/// <summary>
/// Presenta una vista demostrativa e integrada del historial de un cliente.
/// </summary>
public sealed class FrmHistorialCliente : Form
{
    private readonly string _nombreCliente;
    private readonly string _documento;
    private readonly DataGridView _dgvVehiculos = new();
    private readonly DataGridView _dgvAtenciones = new();
    private readonly Label _lblResumen = new();

    public FrmHistorialCliente(string nombreCliente, string documento)
    {
        _nombreCliente = nombreCliente;
        _documento = documento;

        ConfigurarVentana();
        ConstruirInterfaz();
        CargarDatosSimulados();
    }

    private void ConfigurarVentana()
    {
        Text = "Historial del cliente";
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        MinimumSize = new Size(820, 560);
        Size = new Size(940, 640);
        BackColor = Color.FromArgb(245, 247, 250);
    }

    private void ConstruirInterfaz()
    {
        var principal = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20),
            ColumnCount = 1,
            RowCount = 4
        };
        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 82));
        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
        principal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));

        principal.Controls.Add(CrearEncabezado(), 0, 0);
        principal.Controls.Add(CrearResumen(), 0, 1);
        principal.Controls.Add(CrearPestanas(), 0, 2);
        principal.Controls.Add(CrearAcciones(), 0, 3);

        Controls.Add(principal);
    }

    private Control CrearEncabezado()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(18, 10, 18, 8)
        };

        var titulo = new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42),
            Text = "Historial del cliente",
            Location = new Point(18, 10)
        };

        var descripcion = new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(100, 116, 139),
            Text = $"{_nombreCliente} · Documento: {_documento}",
            Location = new Point(20, 48)
        };

        panel.Controls.Add(titulo);
        panel.Controls.Add(descripcion);
        return panel;
    }

    private Control CrearResumen()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = new Padding(0, 8, 0, 8),
            Padding = new Padding(18)
        };

        _lblResumen.AutoSize = true;
        _lblResumen.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        _lblResumen.ForeColor = Color.FromArgb(30, 64, 175);
        _lblResumen.Location = new Point(18, 22);
        panel.Controls.Add(_lblResumen);

        return panel;
    }

    private Control CrearPestanas()
    {
        var pestañas = new TabControl { Dock = DockStyle.Fill };

        var tabVehiculos = new TabPage("Vehículos asociados");
        var tabAtenciones = new TabPage("Atenciones e historial");

        ConfigurarGrilla(_dgvVehiculos);
        ConfigurarGrilla(_dgvAtenciones);

        _dgvVehiculos.Columns.Add("Dominio", "Dominio");
        _dgvVehiculos.Columns.Add("Modelo", "Marca / modelo");
        _dgvVehiculos.Columns.Add("Anio", "Año");
        _dgvVehiculos.Columns.Add("Estado", "Estado");

        _dgvAtenciones.Columns.Add("Numero", "Atención");
        _dgvAtenciones.Columns.Add("Fecha", "Fecha");
        _dgvAtenciones.Columns.Add("Vehiculo", "Vehículo");
        _dgvAtenciones.Columns.Add("Motivo", "Motivo");
        _dgvAtenciones.Columns.Add("Estado", "Estado");

        tabVehiculos.Controls.Add(_dgvVehiculos);
        tabAtenciones.Controls.Add(_dgvAtenciones);
        pestañas.TabPages.Add(tabVehiculos);
        pestañas.TabPages.Add(tabAtenciones);

        return pestañas;
    }

    private static void ConfigurarGrilla(DataGridView grilla)
    {
        grilla.Dock = DockStyle.Fill;
        grilla.AllowUserToAddRows = false;
        grilla.AllowUserToDeleteRows = false;
        grilla.ReadOnly = true;
        grilla.MultiSelect = false;
        grilla.RowHeadersVisible = false;
        grilla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grilla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grilla.BackgroundColor = Color.White;
        grilla.BorderStyle = BorderStyle.None;
    }

    private Control CrearAcciones()
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(0, 8, 0, 0)
        };

        var cerrar = CrearBoton("Cerrar", 100);
        cerrar.Click += (_, _) => Close();

        var detalle = CrearBoton("Ver detalle seleccionado", 185);
        detalle.Click += (_, _) => MessageBox.Show(
            "Aquí se abrirá el formulario de detalle correspondiente al registro seleccionado.",
            "Navegación simulada",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        panel.Controls.Add(cerrar);
        panel.Controls.Add(detalle);
        return panel;
    }

    private static Button CrearBoton(string texto, int ancho)
    {
        return new Button
        {
            Text = texto,
            Width = ancho,
            Height = 32,
            Cursor = Cursors.Hand,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 64, 175),
            BackColor = Color.White
        };
    }

    private void CargarDatosSimulados()
    {
        _dgvVehiculos.Rows.Add("AB345CD", "Honda Fit", "2020", "Habilitado");
        _dgvVehiculos.Rows.Add("AC902LM", "Toyota Corolla", "2018", "Habilitado");

        _dgvAtenciones.Rows.Add(
            "000027", "10/09/2026", "AB345CD",
            "Ruido en tren delantero", "Finalizada");
        _dgvAtenciones.Rows.Add(
            "000041", "18/09/2026", "AC902LM",
            "Mantenimiento general", "En reparación");

        _lblResumen.Text =
            $"{_dgvVehiculos.Rows.Count} vehículos · " +
            $"{_dgvAtenciones.Rows.Count} atenciones registradas";

        _dgvVehiculos.ClearSelection();
        _dgvAtenciones.ClearSelection();
    }
}
