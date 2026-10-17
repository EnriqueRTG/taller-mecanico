using System.Diagnostics;
using System.Windows.Forms;
namespace Taller.Presentacion.Formularios.Catalogo
{
    partial class FrmCatalogo : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            tlpPrincipal = new TableLayoutPanel();
            pnlGrilla = new Panel();
            dgvConceptos = new DataGridView();
            idConcepto = new DataGridViewTextBoxColumn();
            idCategoria = new DataGridViewTextBoxColumn();
            tipo_concepto = new DataGridViewTextBoxColumn();
            colNombre = new DataGridViewTextBoxColumn();
            descripcion = new DataGridViewTextBoxColumn();
            precioReferencia = new DataGridViewTextBoxColumn();
            colEstado = new DataGridViewTextBoxColumn();
            pnlCabecera = new Panel();
            lblDescripcion = new Label();
            lblTitulo = new Label();
            pnlFiltros = new Panel();
            lblTipoConcepto = new Label();
            cmbTiposConcepto = new ComboBox();
            cmbCategorias = new ComboBox();
            lblCategoria = new Label();
            lblCantidad = new Label();
            pnlAccionesUsuario = new Panel();
            flowLayoutPanel1 = new FlowLayoutPanel();
            lblBuscar = new Label();
            txtBuscar = new TextBox();
            tlpPrincipal.SuspendLayout();
            pnlGrilla.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvConceptos).BeginInit();
            pnlCabecera.SuspendLayout();
            pnlFiltros.SuspendLayout();
            pnlAccionesUsuario.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tlpPrincipal
            // 
            tlpPrincipal.ColumnCount = 1;
            tlpPrincipal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPrincipal.Controls.Add(pnlGrilla, 0, 3);
            tlpPrincipal.Controls.Add(pnlCabecera, 0, 0);
            tlpPrincipal.Controls.Add(pnlFiltros, 0, 1);
            tlpPrincipal.Controls.Add(lblCantidad, 0, 4);
            tlpPrincipal.Controls.Add(pnlAccionesUsuario, 0, 2);
            tlpPrincipal.Dock = DockStyle.Fill;
            tlpPrincipal.Location = new Point(0, 0);
            tlpPrincipal.Margin = new Padding(0);
            tlpPrincipal.Name = "tlpPrincipal";
            tlpPrincipal.Padding = new Padding(27);
            tlpPrincipal.RowCount = 5;
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 81F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPrincipal.RowStyles.Add(new RowStyle(SizeType.Absolute, 37F));
            tlpPrincipal.Size = new Size(800, 451);
            tlpPrincipal.TabIndex = 2;
            // 
            // pnlGrilla
            // 
            pnlGrilla.BackColor = Color.White;
            pnlGrilla.Controls.Add(dgvConceptos);
            pnlGrilla.Dock = DockStyle.Fill;
            pnlGrilla.Location = new Point(27, 283);
            pnlGrilla.Margin = new Padding(0);
            pnlGrilla.Name = "pnlGrilla";
            pnlGrilla.Padding = new Padding(1);
            pnlGrilla.Size = new Size(746, 104);
            pnlGrilla.TabIndex = 5;
            // 
            // dgvConceptos
            // 
            dgvConceptos.AllowUserToAddRows = false;
            dgvConceptos.AllowUserToDeleteRows = false;
            dgvConceptos.AllowUserToResizeColumns = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(248, 250, 252);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(30, 64, 175);
            dgvConceptos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvConceptos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvConceptos.BackgroundColor = Color.White;
            dgvConceptos.BorderStyle = BorderStyle.None;
            dgvConceptos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(241, 245, 249);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(51, 65, 85);
            dataGridViewCellStyle2.Padding = new Padding(6, 0, 6, 0);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(241, 245, 249);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(51, 65, 85);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvConceptos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvConceptos.ColumnHeadersHeight = 42;
            dgvConceptos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvConceptos.Columns.AddRange(new DataGridViewColumn[] { idConcepto, idCategoria, tipo_concepto, colNombre, descripcion, precioReferencia, colEstado });
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.White;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(30, 41, 59);
            dataGridViewCellStyle4.Padding = new Padding(6, 0, 6, 0);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(219, 234, 254);
            dataGridViewCellStyle4.SelectionForeColor = Color.FromArgb(30, 64, 175);
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dgvConceptos.DefaultCellStyle = dataGridViewCellStyle4;
            dgvConceptos.Dock = DockStyle.Fill;
            dgvConceptos.EnableHeadersVisualStyles = false;
            dgvConceptos.GridColor = Color.FromArgb(226, 232, 240);
            dgvConceptos.Location = new Point(1, 1);
            dgvConceptos.MultiSelect = false;
            dgvConceptos.Name = "dgvConceptos";
            dgvConceptos.ReadOnly = true;
            dgvConceptos.RowHeadersVisible = false;
            dgvConceptos.RowHeadersWidth = 51;
            dgvConceptos.RowTemplate.Height = 38;
            dgvConceptos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvConceptos.Size = new Size(744, 102);
            dgvConceptos.TabIndex = 5;
            // 
            // idConcepto
            // 
            idConcepto.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            idConcepto.DataPropertyName = "Id_concepto";
            idConcepto.HeaderText = "ID Concepto";
            idConcepto.MinimumWidth = 6;
            idConcepto.Name = "idConcepto";
            idConcepto.ReadOnly = true;
            idConcepto.Width = 60;
            // 
            // idCategoria
            // 
            idCategoria.DataPropertyName = "id_categoria";
            idCategoria.FillWeight = 110F;
            idCategoria.HeaderText = "ID Categoria";
            idCategoria.MinimumWidth = 6;
            idCategoria.Name = "idCategoria";
            idCategoria.ReadOnly = true;
            // 
            // tipo_concepto
            // 
            tipo_concepto.DataPropertyName = "tipo_concepto";
            tipo_concepto.FillWeight = 110F;
            tipo_concepto.HeaderText = "Tipo Concepto";
            tipo_concepto.MinimumWidth = 6;
            tipo_concepto.Name = "tipo_concepto";
            tipo_concepto.ReadOnly = true;
            // 
            // colNombre
            // 
            colNombre.DataPropertyName = "Nombre";
            colNombre.FillWeight = 170F;
            colNombre.HeaderText = "Nombre ";
            colNombre.MinimumWidth = 6;
            colNombre.Name = "colNombre";
            colNombre.ReadOnly = true;
            // 
            // descripcion
            // 
            descripcion.HeaderText = "Descripción";
            descripcion.MinimumWidth = 6;
            descripcion.Name = "descripcion";
            descripcion.ReadOnly = true;
            // 
            // precioReferencia
            // 
            precioReferencia.HeaderText = "Precio Referencia";
            precioReferencia.MinimumWidth = 6;
            precioReferencia.Name = "precioReferencia";
            precioReferencia.ReadOnly = true;
            // 
            // colEstado
            // 
            colEstado.DataPropertyName = "Estado";
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            colEstado.DefaultCellStyle = dataGridViewCellStyle3;
            colEstado.FillWeight = 75F;
            colEstado.HeaderText = "Estado";
            colEstado.MinimumWidth = 6;
            colEstado.Name = "colEstado";
            colEstado.ReadOnly = true;
            // 
            // pnlCabecera
            // 
            pnlCabecera.BackColor = Color.White;
            pnlCabecera.Controls.Add(lblDescripcion);
            pnlCabecera.Controls.Add(lblTitulo);
            pnlCabecera.Dock = DockStyle.Fill;
            pnlCabecera.Location = new Point(27, 27);
            pnlCabecera.Margin = new Padding(0, 0, 0, 11);
            pnlCabecera.Name = "pnlCabecera";
            pnlCabecera.Padding = new Padding(21, 12, 21, 11);
            pnlCabecera.Size = new Size(746, 79);
            pnlCabecera.TabIndex = 0;
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Font = new Font("Segoe UI", 9.5F);
            lblDescripcion.ForeColor = Color.FromArgb(100, 116, 139);
            lblDescripcion.Location = new Point(18, 49);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(173, 21);
            lblDescripcion.TabIndex = 1;
            lblDescripcion.Text = "Conceptos y Categorías";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.FromArgb(15, 23, 42);
            lblTitulo.Location = new Point(13, 12);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(285, 37);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Gestión de Catálogo ";
            // 
            // pnlFiltros
            // 
            pnlFiltros.BackColor = Color.White;
            pnlFiltros.Controls.Add(lblTipoConcepto);
            pnlFiltros.Controls.Add(cmbTiposConcepto);
            pnlFiltros.Controls.Add(cmbCategorias);
            pnlFiltros.Controls.Add(lblCategoria);
            pnlFiltros.Dock = DockStyle.Fill;
            pnlFiltros.Location = new Point(27, 117);
            pnlFiltros.Margin = new Padding(0, 0, 0, 11);
            pnlFiltros.Name = "pnlFiltros";
            pnlFiltros.Padding = new Padding(18, 11, 18, 11);
            pnlFiltros.Size = new Size(746, 74);
            pnlFiltros.TabIndex = 1;
            // 
            // lblTipoConcepto
            // 
            lblTipoConcepto.AutoSize = true;
            lblTipoConcepto.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTipoConcepto.ForeColor = Color.FromArgb(51, 65, 85);
            lblTipoConcepto.Location = new Point(363, 11);
            lblTipoConcepto.Name = "lblTipoConcepto";
            lblTipoConcepto.Size = new Size(131, 20);
            lblTipoConcepto.TabIndex = 4;
            lblTipoConcepto.Text = "Tipo de Concepto";
            // 
            // cmbTiposConcepto
            // 
            cmbTiposConcepto.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTiposConcepto.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbTiposConcepto.FormattingEnabled = true;
            cmbTiposConcepto.Location = new Point(363, 34);
            cmbTiposConcepto.Name = "cmbTiposConcepto";
            cmbTiposConcepto.Size = new Size(294, 29);
            cmbTiposConcepto.TabIndex = 3;
            // 
            // cmbCategorias
            // 
            cmbCategorias.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategorias.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbCategorias.FormattingEnabled = true;
            cmbCategorias.Location = new Point(18, 34);
            cmbCategorias.Name = "cmbCategorias";
            cmbCategorias.Size = new Size(294, 29);
            cmbCategorias.TabIndex = 2;
            // 
            // lblCategoria
            // 
            lblCategoria.AutoSize = true;
            lblCategoria.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCategoria.ForeColor = Color.FromArgb(51, 65, 85);
            lblCategoria.Location = new Point(18, 11);
            lblCategoria.Name = "lblCategoria";
            lblCategoria.Size = new Size(76, 20);
            lblCategoria.TabIndex = 2;
            lblCategoria.Text = "Categoría";
            // 
            // lblCantidad
            // 
            lblCantidad.AutoSize = true;
            lblCantidad.Dock = DockStyle.Fill;
            lblCantidad.ForeColor = Color.FromArgb(100, 116, 139);
            lblCantidad.Location = new Point(32, 391);
            lblCantidad.Margin = new Padding(5, 4, 0, 0);
            lblCantidad.Name = "lblCantidad";
            lblCantidad.Size = new Size(741, 33);
            lblCantidad.TabIndex = 4;
            lblCantidad.Text = "0 usuarios encontrados";
            lblCantidad.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlAccionesUsuario
            // 
            pnlAccionesUsuario.BackColor = Color.White;
            pnlAccionesUsuario.Controls.Add(flowLayoutPanel1);
            pnlAccionesUsuario.Dock = DockStyle.Fill;
            pnlAccionesUsuario.Location = new Point(27, 202);
            pnlAccionesUsuario.Margin = new Padding(0, 0, 0, 11);
            pnlAccionesUsuario.Name = "pnlAccionesUsuario";
            pnlAccionesUsuario.Padding = new Padding(18, 7, 18, 7);
            pnlAccionesUsuario.Size = new Size(746, 70);
            pnlAccionesUsuario.TabIndex = 6;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.White;
            flowLayoutPanel1.Controls.Add(lblBuscar);
            flowLayoutPanel1.Controls.Add(txtBuscar);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(18, 7);
            flowLayoutPanel1.Margin = new Padding(0, 0, 0, 11);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(18, 11, 18, 11);
            flowLayoutPanel1.Size = new Size(710, 56);
            flowLayoutPanel1.TabIndex = 0;
            flowLayoutPanel1.WrapContents = false;
            // 
            // lblBuscar
            // 
            lblBuscar.AutoSize = true;
            lblBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBuscar.ForeColor = Color.FromArgb(51, 65, 85);
            lblBuscar.Location = new Point(21, 11);
            lblBuscar.Name = "lblBuscar";
            lblBuscar.Size = new Size(56, 20);
            lblBuscar.TabIndex = 0;
            lblBuscar.Text = "Buscar";
            // 
            // txtBuscar
            // 
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            txtBuscar.Dock = DockStyle.Fill;
            txtBuscar.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtBuscar.Location = new Point(83, 14);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.PlaceholderText = "Escriba para buscar...";
            txtBuscar.Size = new Size(588, 29);
            txtBuscar.TabIndex = 1;
            // 
            // FrmCatalogo
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 451);
            Controls.Add(tlpPrincipal);
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(800, 451);
            Name = "FrmCatalogo";
            Text = "FrmCategoriasConceptos";
            Load += FormCatalogo_Load;
            tlpPrincipal.ResumeLayout(false);
            tlpPrincipal.PerformLayout();
            pnlGrilla.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvConceptos).EndInit();
            pnlCabecera.ResumeLayout(false);
            pnlCabecera.PerformLayout();
            pnlFiltros.ResumeLayout(false);
            pnlFiltros.PerformLayout();
            pnlAccionesUsuario.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpPrincipal;
        private Panel pnlGrilla;
        private DataGridView dgvConceptos;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn dni;
        private DataGridViewTextBoxColumn tipo_documento;
        private DataGridViewTextBoxColumn colNombreCompleto;
        private Panel pnlCabecera;
        private Label lblDescripcion;
        private Label lblTitulo;
        private Panel pnlFiltros;
        private ComboBox cmbCategorias;
        private Label lblCategoria;
        private TextBox txtBuscar;
        private Label lblBuscar;
        private Label lblCantidad;
        private Panel pnlAccionesUsuario;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label lblTipoConcepto;
        private ComboBox cmbTiposConcepto;
        private DataGridViewTextBoxColumn idConcepto;
        private DataGridViewTextBoxColumn idCategoria;
        private DataGridViewTextBoxColumn tipo_concepto;
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn descripcion;
        private DataGridViewTextBoxColumn precioReferencia;
        private DataGridViewTextBoxColumn colEstado;

        private string GetDebuggerDisplay()
        {
            return ToString();
        }
    }
}