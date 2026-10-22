#nullable enable
namespace Taller.Presentacion.Formularios.Clientes;
partial class FrmHistorialCliente
{
    private System.ComponentModel.IContainer? components;
    protected override void Dispose(bool disposing) { if(disposing)components?.Dispose();base.Dispose(disposing); }
    private void InitializeComponent()
    {
        components=new System.ComponentModel.Container(); tlp=new TableLayoutPanel();lblCliente=new Label();lblResumen=new Label();dgvDatos=new DataGridView();btnCerrar=new Button();
        SuspendLayout(); AutoScaleDimensions=new SizeF(7F,15F);AutoScaleMode=AutoScaleMode.Font;Font=new Font("Segoe UI",9F);ClientSize=new Size(980,550);MinimumSize=new Size(760,450);Text="Historial del cliente";StartPosition=FormStartPosition.CenterParent;
        tlp.Dock=DockStyle.Fill;tlp.Padding=new Padding(14);tlp.ColumnCount=1;tlp.RowCount=4;tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,100));tlp.RowStyles.Add(new RowStyle(SizeType.Absolute,40));tlp.RowStyles.Add(new RowStyle(SizeType.Absolute,32));tlp.RowStyles.Add(new RowStyle(SizeType.Percent,100));tlp.RowStyles.Add(new RowStyle(SizeType.Absolute,42));
        Controls.Add(tlp);tlp.Controls.Add(lblCliente,0,0);tlp.Controls.Add(lblResumen,0,1);tlp.Controls.Add(dgvDatos,0,2);tlp.Controls.Add(btnCerrar,0,3);lblCliente.Dock=lblResumen.Dock=DockStyle.Fill;
        dgvDatos.Dock=DockStyle.Fill;dgvDatos.ReadOnly=true;dgvDatos.AutoGenerateColumns=false;dgvDatos.AllowUserToAddRows=false;dgvDatos.AllowUserToDeleteRows=false;dgvDatos.AllowUserToResizeColumns=true;dgvDatos.RowHeadersVisible=false;dgvDatos.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;
        colNumero=new DataGridViewTextBoxColumn { Name="colNumero",DataPropertyName="Numero",HeaderText="Atención",MinimumWidth=70,FillWeight=70 };dgvDatos.Columns.Add(colNumero);
        colFecha=new DataGridViewTextBoxColumn { Name="colFecha",DataPropertyName="Fecha",HeaderText="Fecha",MinimumWidth=70,FillWeight=100 };dgvDatos.Columns.Add(colFecha);
        colFecha.DefaultCellStyle.Format="dd/MM/yyyy HH:mm";
        colDominio=new DataGridViewTextBoxColumn { Name="colDominio",DataPropertyName="Dominio",HeaderText="Dominio",MinimumWidth=70,FillWeight=90 };dgvDatos.Columns.Add(colDominio);
        colMotivo=new DataGridViewTextBoxColumn { Name="colMotivo",DataPropertyName="Motivo",HeaderText="Motivo",MinimumWidth=70,FillWeight=220 };dgvDatos.Columns.Add(colMotivo);
        colEstado=new DataGridViewTextBoxColumn { Name="colEstado",DataPropertyName="Estado",HeaderText="Estado",MinimumWidth=70,FillWeight=100 };dgvDatos.Columns.Add(colEstado);
        colInforme=new DataGridViewTextBoxColumn { Name="colInforme",DataPropertyName="Informe",HeaderText="Informe técnico",MinimumWidth=70,FillWeight=220 };dgvDatos.Columns.Add(colInforme);
        btnCerrar.Text="Cerrar";btnCerrar.DialogResult=DialogResult.Cancel;btnCerrar.Anchor=AnchorStyles.Right;CancelButton=btnCerrar;btnCerrar.Click+=BtnCerrar_Click;Load+=Formulario_Load;ResumeLayout(false);
    }
    private TableLayoutPanel tlp=null!;
    private Label lblCliente=null!,lblResumen=null!;
    private DataGridView dgvDatos=null!;
    private Button btnCerrar=null!;
    private DataGridViewTextBoxColumn colNumero=null!;
    private DataGridViewTextBoxColumn colFecha=null!;
    private DataGridViewTextBoxColumn colDominio=null!;
    private DataGridViewTextBoxColumn colMotivo=null!;
    private DataGridViewTextBoxColumn colEstado=null!;
    private DataGridViewTextBoxColumn colInforme=null!;
}
