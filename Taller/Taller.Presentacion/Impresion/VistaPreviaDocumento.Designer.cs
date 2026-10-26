namespace Taller.Presentacion.Impresion;
internal partial class VistaPreviaDocumento
{
    private VistaPreviaNavegable vista = null!;
    private Button btnAnterior = null!, btnSiguiente = null!;
    private NumericUpDown nudPagina = null!;
    private Label lblTotal = null!;
    private ComboBox cboZoom = null!;
    private void InitializeComponent()
    {
        vista = new VistaPreviaNavegable { Dock = DockStyle.Fill, AutoZoom = true, UseAntiAlias = true, Name = "vista" };
        btnAnterior = new Button { Text = "Anterior", AutoSize = true, Name = "btnAnterior", Enabled = false };
        btnSiguiente = new Button { Text = "Siguiente", AutoSize = true, Name = "btnSiguiente", Enabled = false };
        nudPagina = new NumericUpDown { Minimum = 1, Maximum = 1, Value = 1, Width = 65, Name = "nudPagina" };
        lblTotal = new Label { Text = "de …", AutoSize = true, Padding = new Padding(0, 6, 6, 0) };
        cboZoom = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 100 };
        cboZoom.Items.AddRange(new object[] { "Ajustar", "50 %", "75 %", "100 %", "150 %" });
        var btnImprimir = new Button { Text = "Imprimir / PDF", AutoSize = true };
        var btnCerrar = new Button { Text = "Cerrar", AutoSize = true };
        var acciones = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, WrapContents = true, Padding = new Padding(8) };
        acciones.Controls.AddRange(new Control[] { btnAnterior, btnSiguiente, new Label { Text = "Página", AutoSize = true, Padding = new Padding(5, 6, 0, 0) }, nudPagina, lblTotal, cboZoom, btnImprimir, btnCerrar });
        Controls.Add(vista);
        Controls.Add(acciones);
        Text = "Vista previa del documento";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(1050, 700);
        MinimumSize = new Size(650, 450);
        AutoScaleMode = AutoScaleMode.Dpi;
        Load += Preparar;
        btnAnterior.Click += Anterior;
        btnSiguiente.Click += Siguiente;
        nudPagina.ValueChanged += PaginaCambio;
        vista.StartPageChanged += InicioPaginaCambio;
        vista.SolicitarZoom += CambiarZoomConTeclado;
        cboZoom.SelectedIndexChanged += ZoomCambio;
        btnImprimir.Click += Imprimir;
        btnCerrar.Click += Cerrar;
    }
}
