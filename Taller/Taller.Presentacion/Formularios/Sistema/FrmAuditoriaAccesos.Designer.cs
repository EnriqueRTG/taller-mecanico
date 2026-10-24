#nullable enable
namespace Taller.Presentacion.Formularios.Sistema;
partial class FrmAuditoriaAccesos
{
    private System.ComponentModel.IContainer? components;
    private DateTimePicker desde = null!, hasta = null!;
    private ComboBox resultado = null!;
    private Button consultar = null!;
    private Button btnLimpiarFiltros = null!;
    private Label estado = null!;
    private DataGridView grilla = null!;
    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        var tabla = new TableLayoutPanel
        {
            Name = "tabla",
            Dock = DockStyle.Fill,
            RowCount = 3,
            ColumnCount = 1,
            Padding = new Padding(12)
        };
        tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
        var filtros = new FlowLayoutPanel
        {
            Name = "filtros",
            Dock = DockStyle.Fill,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            AutoScroll = false,
            BackColor = Color.White,
            Padding = new Padding(8)
        };
        desde = new DateTimePicker
        {
            Name = "desde",
            Format = DateTimePickerFormat.Short,
            Width = 120
        };
        hasta = new DateTimePicker
        {
            Name = "hasta",
            Format = DateTimePickerFormat.Short,
            Width = 120
        };
        resultado = new ComboBox
        {
            Name = "resultado",
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = 150
        };
        resultado.Items.AddRange(["Todos", "Correctos", "Rechazados"]);
        consultar = new Button
        {
            Name = "consultar",
            Text = "Consultar",
            AutoSize = true
        };
        consultar.Click += Consultar_Click;
        btnLimpiarFiltros = new Button { Name = "btnLimpiarFiltros", Text = "Limpiar filtros" };
        btnLimpiarFiltros.Click += LimpiarFiltros_Click;
        consultar.FlatStyle = FlatStyle.Flat;
        consultar.BackColor = Color.White;
        consultar.ForeColor = Color.FromArgb(30, 64, 175);
        consultar.FlatAppearance.BorderColor = Color.FromArgb(30, 64, 175);
        consultar.FlatAppearance.BorderSize = 1;
        consultar.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
        consultar.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
        consultar.UseVisualStyleBackColor = false;
        consultar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        consultar.Cursor = Cursors.Hand;
        consultar.AutoSize = true;
        consultar.MinimumSize = new Size(110, 32);
        consultar.Margin = new Padding(0, 0, 8, 4);
        btnLimpiarFiltros.FlatStyle = FlatStyle.Flat;
        btnLimpiarFiltros.BackColor = Color.White;
        btnLimpiarFiltros.ForeColor = Color.FromArgb(30, 64, 175);
        btnLimpiarFiltros.FlatAppearance.BorderColor = Color.FromArgb(30, 64, 175);
        btnLimpiarFiltros.FlatAppearance.BorderSize = 1;
        btnLimpiarFiltros.FlatAppearance.MouseOverBackColor = Color.FromArgb(239, 246, 255);
        btnLimpiarFiltros.FlatAppearance.MouseDownBackColor = Color.FromArgb(219, 234, 254);
        btnLimpiarFiltros.UseVisualStyleBackColor = false;
        btnLimpiarFiltros.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnLimpiarFiltros.Cursor = Cursors.Hand;
        btnLimpiarFiltros.AutoSize = true;
        btnLimpiarFiltros.MinimumSize = new Size(110, 32);
        btnLimpiarFiltros.Margin = new Padding(0, 0, 8, 4);

        filtros.Controls.AddRange([new Label { Text = "Desde", AutoSize = true }, desde, new Label { Text = "Hasta", AutoSize = true }, hasta, resultado, btnLimpiarFiltros, consultar]);
        grilla = new DataGridView
        {
            Name = "grilla",
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AutoGenerateColumns = false,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeColumns = true,
            RowHeadersVisible = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        grilla.Columns.AddRange([new DataGridViewTextBoxColumn { Name = "colId", DataPropertyName = "Id", HeaderText = "Evento" }, new DataGridViewTextBoxColumn { Name = "colFecha", DataPropertyName = "FechaLocal", HeaderText = "Fecha y hora — Argentina", DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm:ss" } }, new DataGridViewTextBoxColumn { Name = "colUsuario", DataPropertyName = "Usuario", HeaderText = "Cuenta" }, new DataGridViewCheckBoxColumn { Name = "colCorrecto", DataPropertyName = "Correcto", HeaderText = "Acceso correcto" }]);
        estado = new Label
        {
            Name = "estado",
            Dock = DockStyle.Fill
        };
        tabla.Controls.Add(filtros, 0, 0);
        tabla.Controls.Add(grilla, 0, 1);
        tabla.Controls.Add(estado, 0, 2);
        Controls.Add(tabla);
        Text = "Auditoría de accesos";
        Font = new Font("Segoe UI", 9);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 600);
        Load += Formulario_Load;
        FormClosing += Formulario_FormClosing;
    }
}
