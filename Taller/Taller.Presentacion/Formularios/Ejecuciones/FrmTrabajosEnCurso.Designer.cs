#nullable enable
namespace Taller.Presentacion.Formularios.Ejecuciones;
partial class FrmTrabajosEnCurso
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }
    private TableLayoutPanel tlpPrincipal = null!;
    private TextBox txtBuscar = null!;
    private ComboBox cboEstado = null!;
    private DataGridView dgvDatos = null!;
    private Button btnGestionar = null!, btnProgreso = null!;
    private Label lblAyuda = null!;
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        tlpPrincipal = new TableLayoutPanel { Name = "tlpPrincipal", Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5, Padding = new Padding(12) };
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 76));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        tlpPrincipal.Controls.Add(new Label { Text = "Trabajos en curso · Ejecución autorizada y progreso", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 12, FontStyle.Bold) }, 0, 0);
        var filtros = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 2 };
        filtros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65)); filtros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35)); filtros.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260));
        filtros.RowStyles.Add(new RowStyle(SizeType.Absolute, 24)); filtros.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        txtBuscar = new TextBox { Name = "txtBuscar", Dock = DockStyle.Fill, PlaceholderText = "Atención, cliente, dominio o motivo" };
        txtBuscar.TextChanged += Filtrar;
        cboEstado = new ComboBox { Name = "cboEstado", Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
        cboEstado.Items.AddRange(["Todos", "Disponibles", "Asignados", "Con pausas"]); cboEstado.SelectedIndex = 0; cboEstado.SelectedIndexChanged += Filtrar;
        filtros.Controls.Add(new Label { Text = "Buscar", Dock = DockStyle.Fill }, 0, 0); filtros.Controls.Add(txtBuscar, 0, 1);
        filtros.Controls.Add(new Label { Text = "Situación", Dock = DockStyle.Fill }, 1, 0); filtros.Controls.Add(cboEstado, 1, 1);
        var botonesFiltros = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, AutoScroll = true };
        var limpiar = new Button { Name = "btnLimpiarFiltros", Text = "Limpiar filtros", AutoSize = true, Height = 32 }; limpiar.Click += Limpiar;
        var actualizar = new Button { Name = "btnActualizar", Text = "Actualizar", AutoSize = true, Height = 32 }; actualizar.Click += Actualizar;
        botonesFiltros.Controls.AddRange([limpiar, actualizar]); filtros.Controls.Add(botonesFiltros, 2, 1);
        tlpPrincipal.Controls.Add(filtros, 0, 1);
        dgvDatos = new DataGridView { Name = "dgvDatos", Dock = DockStyle.Fill, AutoGenerateColumns = false, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
            MultiSelect = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect, RowHeadersVisible = false, BackgroundColor = Color.White, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "IdAtencion", HeaderText = "Atención", MinimumWidth = 80, FillWeight = 65 });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Cliente", HeaderText = "Cliente", MinimumWidth = 150 });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Vehiculo", HeaderText = "Dominio", MinimumWidth = 95, FillWeight = 70 });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Tecnico", HeaderText = "Técnico / reserva", MinimumWidth = 140 });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Progreso", HeaderText = "Avance guardado", MinimumWidth = 260, FillWeight = 180 });
        dgvDatos.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Fecha", HeaderText = "Último movimiento", MinimumWidth = 140, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" } });
        dgvDatos.SelectionChanged += Seleccion;
        tlpPrincipal.Controls.Add(dgvDatos, 0, 2);
        lblAyuda = new Label { Dock = DockStyle.Fill }; tlpPrincipal.Controls.Add(lblAyuda, 0, 3);
        var acciones = new FlowLayoutPanel { Dock = DockStyle.Fill };
        btnGestionar = new Button { Name = "btnGestionar", Text = "Gestionar ejecución", AutoSize = true, Height = 34, Enabled = false }; btnGestionar.Click += Gestionar;
        btnProgreso = new Button { Name = "btnProgreso", Text = "Ver progreso y seguimiento", AutoSize = true, Height = 34, Enabled = false }; btnProgreso.Click += Progreso;
        acciones.Controls.AddRange([btnGestionar, btnProgreso]); tlpPrincipal.Controls.Add(acciones, 0, 4);
        foreach (var boton in new[] { limpiar, actualizar, btnProgreso }) Estilos.EstilosInterfaz.AplicarBotonSecundario(boton);
        Estilos.EstilosInterfaz.AplicarBotonPrimario(btnGestionar);
        AutoScaleMode = AutoScaleMode.Font; Font = new Font("Segoe UI", 10); BackColor = Color.FromArgb(248, 250, 252); ClientSize = new Size(1140, 680);
        Controls.Add(tlpPrincipal); Text = "Trabajos en curso"; Load += Cargar;
    }
}
