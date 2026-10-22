#nullable enable
namespace Taller.Presentacion.Formularios.Sistema;
partial class FrmAprobarRestauracion
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            LimpiarClaves();
            components?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        tlp = new TableLayoutPanel();
        tlp.Name = "tlp";
        txtResumen = new TextBox();
        txtResumen.Name = "txtResumen";
        lblPrimero = new Label();
        lblPrimero.Name = "lblPrimero";
        txtPrimero = new TextBox();
        txtPrimero.Name = "txtPrimero";
        txtClavePrimero = new TextBox();
        txtClavePrimero.Name = "txtClavePrimero";
        lblSegundo = new Label();
        lblSegundo.Name = "lblSegundo";
        txtSegundo = new TextBox();
        txtSegundo.Name = "txtSegundo";
        txtClaveSegundo = new TextBox();
        txtClaveSegundo.Name = "txtClaveSegundo";
        lblMotivo = new Label();
        lblMotivo.Name = "lblMotivo";
        txtMotivo = new TextBox();
        txtMotivo.Name = "txtMotivo";
        chkConsecuencias = new CheckBox();
        chkConsecuencias.Name = "chkConsecuencias";
        chkVentana = new CheckBox();
        chkVentana.Name = "chkVentana";
        flp = new FlowLayoutPanel();
        flp.Name = "flp";
        btnConfirmar = new Button();
        btnConfirmar.Name = "btnConfirmar";
        btnCancelar = new Button();
        btnCancelar.Name = "btnCancelar";
        SuspendLayout();
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Font = new Font("Segoe UI", 9F);
        ClientSize = new Size(750, 620);
        MinimumSize = new Size(650, 570);
        StartPosition = FormStartPosition.CenterParent;
        Text = "Doble aprobación de restauración";
        ShowInTaskbar = false;
        tlp.Dock = DockStyle.Fill;
        tlp.Padding = new Padding(14);
        tlp.ColumnCount = 2;
        tlp.RowCount = 10;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 135));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        Controls.Add(tlp);
        tlp.Controls.Add(txtResumen, 0, 0);
        tlp.SetColumnSpan(txtResumen, 2);
        txtResumen.Dock = DockStyle.Fill;
        txtResumen.Multiline = true;
        txtResumen.ReadOnly = true;
        txtResumen.ScrollBars = ScrollBars.Vertical;
        txtResumen.BackColor = Color.White;
        lblPrimero.Text = "Administrador solicitante";
        lblSegundo.Text = "Segundo aprobador: administrador o administrativo";
        lblPrimero.Dock = lblSegundo.Dock = DockStyle.Fill;
        tlp.Controls.Add(lblPrimero, 0, 1);
        tlp.SetColumnSpan(lblPrimero, 2);
        tlp.Controls.Add(txtPrimero, 0, 2);
        tlp.Controls.Add(txtClavePrimero, 1, 2);
        tlp.Controls.Add(lblSegundo, 0, 3);
        tlp.SetColumnSpan(lblSegundo, 2);
        tlp.Controls.Add(txtSegundo, 0, 4);
        tlp.Controls.Add(txtClaveSegundo, 1, 4);
        txtPrimero.Dock = txtClavePrimero.Dock = txtSegundo.Dock = txtClaveSegundo.Dock = DockStyle.Fill;
        txtPrimero.ReadOnly = true;
        txtClavePrimero.UseSystemPasswordChar = txtClaveSegundo.UseSystemPasswordChar = true;
        txtClavePrimero.MaxLength = txtClaveSegundo.MaxLength = 100;
        txtSegundo.MaxLength = 50;
        txtSegundo.PlaceholderText = "Nombre de usuario distinto";
        txtClavePrimero.PlaceholderText = txtClaveSegundo.PlaceholderText = "Contraseña (no se almacena)";
        lblMotivo.Text = "Motivo de la recuperación (10 a 1000 caracteres)";
        lblMotivo.Dock = DockStyle.Fill;
        tlp.Controls.Add(lblMotivo, 0, 5);
        tlp.SetColumnSpan(lblMotivo, 2);
        tlp.Controls.Add(txtMotivo, 0, 6);
        tlp.SetColumnSpan(txtMotivo, 2);
        txtMotivo.Dock = DockStyle.Fill;
        txtMotivo.Multiline = true;
        txtMotivo.MaxLength = 1000;
        txtMotivo.ScrollBars = ScrollBars.Vertical;
        chkConsecuencias.Text = "Ambos reconocemos que los datos posteriores al punto de copia serán reemplazados, incluidos cobros, usuarios y contraseñas.";
        chkConsecuencias.Dock = DockStyle.Fill;
        tlp.Controls.Add(chkConsecuencias, 0, 7);
        tlp.SetColumnSpan(chkConsecuencias, 2);
        chkVentana.Text = "Coordinamos la ventana de mantenimiento y el cierre de otros equipos/aplicaciones. Se recomienda acompañamiento técnico.";
        chkVentana.Dock = DockStyle.Fill;
        tlp.Controls.Add(chkVentana, 0, 8);
        tlp.SetColumnSpan(chkVentana, 2);
        flp.Dock = DockStyle.Fill;
        flp.FlowDirection = FlowDirection.RightToLeft;
        btnConfirmar.Text = "Aprobar y ejecutar restauración";
        btnConfirmar.ForeColor = Color.Firebrick;
        btnConfirmar.AutoSize = true;
        btnConfirmar.DialogResult = DialogResult.None;
        btnCancelar.Text = "Cancelar";
        btnCancelar.AutoSize = true;
        btnCancelar.DialogResult = DialogResult.Cancel;
        CancelButton = btnCancelar;
        flp.Controls.AddRange([btnConfirmar, btnCancelar]);
        tlp.Controls.Add(flp, 0, 9);
        tlp.SetColumnSpan(flp, 2);
        btnConfirmar.Click += BtnConfirmar_Click;
        btnCancelar.Click += BtnCancelar_Click;
        ResumeLayout(false);
    }

    private TableLayoutPanel tlp = null!;
    private TextBox txtResumen = null!;
    private Label lblPrimero = null!;
    private TextBox txtPrimero = null!;
    private TextBox txtClavePrimero = null!;
    private Label lblSegundo = null!;
    private TextBox txtSegundo = null!;
    private TextBox txtClaveSegundo = null!;
    private Label lblMotivo = null!;
    private TextBox txtMotivo = null!;
    private CheckBox chkConsecuencias = null!;
    private CheckBox chkVentana = null!;
    private FlowLayoutPanel flp = null!;
    private Button btnConfirmar = null!;
    private Button btnCancelar = null!;
}
