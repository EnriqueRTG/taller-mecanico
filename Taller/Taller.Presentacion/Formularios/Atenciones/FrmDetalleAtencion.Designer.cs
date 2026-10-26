#nullable enable
namespace Taller.Presentacion.Formularios.Atenciones;
partial class FrmDetalleAtencion
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing) { if (disposing) components?.Dispose(); base.Dispose(disposing); }
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        tlpPrincipal = new TableLayoutPanel { Name = "tlpPrincipal", Dock = DockStyle.Fill, Padding = new Padding(12), ColumnCount = 1, RowCount = 3 };
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        lblResumen = new Label { Name = "lblResumen", Dock = DockStyle.Fill, Padding = new Padding(8), BackColor = Color.FromArgb(219, 234, 254), ForeColor = Color.FromArgb(30, 58, 138) };
        txtMotivo = new TextBox { Name = "txtMotivo", Dock = DockStyle.Fill, ReadOnly = true, Multiline = true, ScrollBars = ScrollBars.Vertical };
        txtExpediente = new TextBox { Name = "txtExpediente", Dock = DockStyle.Fill, ReadOnly = true, Multiline = true, ScrollBars = ScrollBars.Vertical };
        var tabs = new TabControl { Dock = DockStyle.Fill };
        var ingreso = new TabPage("Ingreso y solicitudes"); ingreso.Controls.Add(txtMotivo);
        var expediente = new TabPage("Expediente de la atención"); expediente.Controls.Add(txtExpediente);
        tabs.TabPages.AddRange([ingreso, expediente]);
        var acciones = new FlowLayoutPanel { Dock = DockStyle.Fill };
        btnProgreso = new Button { Name = "btnProgreso", Text = "Ver progreso y seguimiento", AutoSize = true, Height = 34, Enabled = false };
        btnCerrar = new Button { Name = "btnCerrar", Text = "Cerrar", AutoSize = true, Height = 34 };
        btnProgreso.Click += VerProgreso; btnCerrar.Click += Cerrar;
        Taller.Presentacion.Estilos.EstilosInterfaz.AplicarBotonSecundario(btnProgreso);
        Taller.Presentacion.Estilos.EstilosInterfaz.AplicarBotonSecundario(btnCerrar);
        acciones.Controls.AddRange([btnProgreso, btnCerrar]);
        tlpPrincipal.Controls.Add(lblResumen, 0, 0); tlpPrincipal.Controls.Add(tabs, 0, 1); tlpPrincipal.Controls.Add(acciones, 0, 2);
        Controls.Add(tlpPrincipal);
        AutoScaleMode = AutoScaleMode.Font; Font = new Font("Segoe UI", 10F);
        ClientSize = new Size(1020, 620); MinimumSize = new Size(820, 520); StartPosition = FormStartPosition.CenterParent;
        Text = "Detalle de atención y solicitudes"; BackColor = Color.FromArgb(248, 250, 252);
        Load += Cargar; FormClosing += AlCerrar; CancelButton = btnCerrar;
    }
    private TableLayoutPanel tlpPrincipal = null!;
    private Label lblResumen = null!;
    private TextBox txtMotivo = null!;
    private TextBox txtExpediente = null!;
    private Button btnProgreso = null!;
    private Button btnCerrar = null!;
}
