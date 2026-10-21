#nullable enable
namespace Taller.Presentacion.Formularios.Compartidos;
partial class FrmSolicitarMotivo
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }
    private void InitializeComponent()
    {
        tlpPrincipal = new TableLayoutPanel();
        tlpPrincipal.Name = "tlpPrincipal";
        lblAyuda = new Label();
        lblAyuda.Name = "lblAyuda";
        txtMotivo = new TextBox();
        txtMotivo.Name = "txtMotivo";
        flpAcciones = new FlowLayoutPanel();
        flpAcciones.Name = "flpAcciones";
        btnConfirmar = new Button();
        btnConfirmar.Name = "btnConfirmar";
        btnCancelar = new Button();
        btnCancelar.Name = "btnCancelar";
        components = new System.ComponentModel.Container();
        SuspendLayout();
        AutoScaleMode = AutoScaleMode.Font;
        Font = new Font("Segoe UI", 10F);
        ClientSize = new Size(1020, 620);
        MinimumSize = new Size(820, 520);
        StartPosition = FormStartPosition.CenterParent;
        tlpPrincipal.Dock = DockStyle.Fill;
        tlpPrincipal.Padding = new Padding(12);
        tlpPrincipal.ColumnCount = 1;
        tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        tlpPrincipal.RowCount = 3;
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 68));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        Controls.Add(tlpPrincipal); ClientSize=new Size(680,320); MinimumSize=new Size(520,300);
        tlpPrincipal.Controls.Add(lblAyuda, 0, 0);
        lblAyuda.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(txtMotivo, 0, 1);
        txtMotivo.Dock = DockStyle.Fill;
        tlpPrincipal.Controls.Add(flpAcciones, 0, 2);
        flpAcciones.Dock = DockStyle.Fill;
        txtMotivo.Multiline=true;txtMotivo.MaxLength=1000;txtMotivo.ScrollBars=ScrollBars.Vertical;
        btnConfirmar.Text = "Confirmar";
        btnConfirmar.AutoSize = true;
        btnConfirmar.Height = 34;
        btnConfirmar.Click += Confirmar;
        flpAcciones.Controls.Add(btnConfirmar);
        btnCancelar.Text = "Cancelar";
        btnCancelar.AutoSize = true;
        btnCancelar.Height = 34;
        btnCancelar.Click += Cancelar;
        flpAcciones.Controls.Add(btnCancelar);



        BackColor=Color.FromArgb(248,250,252);
        btnConfirmar.BackColor=Color.FromArgb(30,64,175);btnConfirmar.ForeColor=Color.White;btnConfirmar.FlatStyle=FlatStyle.Flat;
        CancelButton=btnCancelar;btnCancelar.DialogResult=DialogResult.None;
        ResumeLayout(false);
    }
    private TableLayoutPanel tlpPrincipal = null!;
    private Label lblAyuda = null!;
    private TextBox txtMotivo = null!;
    private FlowLayoutPanel flpAcciones = null!;
    private Button btnConfirmar = null!;
    private Button btnCancelar = null!;
}
