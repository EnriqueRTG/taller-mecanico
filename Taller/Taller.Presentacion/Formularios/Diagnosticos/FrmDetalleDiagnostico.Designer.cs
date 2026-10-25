namespace Taller.Presentacion.Formularios.Diagnosticos
{
    partial class FrmDetalleDiagnostico
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
            tlpDetalle = new TableLayoutPanel();
            lblAtencion = new Label();
            lblFecha = new Label();
            lblTecnicoAutor = new Label();
            lblTipo = new Label();
            lblKilometraje = new Label();
            lblFallaDetectada = new Label();
            lblTrabajoRecomendado = new Label();
            lblObservaciones = new Label();
            txtAtencion = new TextBox();
            txtFecha = new TextBox();
            txtTecnicoAutor = new TextBox();
            txtTipo = new TextBox();
            txtKilometraje = new TextBox();
            txtFallaDetectada = new TextBox();
            txtTrabajoRecomendado = new TextBox();
            txtObservaciones = new TextBox();
            btnCerrar = new Button();
            tlpDetalle.SuspendLayout();
            SuspendLayout();
            // 
            // tlpDetalle
            // 
            tlpDetalle.ColumnCount = 2;
            tlpDetalle.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190F));
            tlpDetalle.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDetalle.Controls.Add(lblAtencion, 0, 0);
            tlpDetalle.Controls.Add(lblFecha, 0, 1);
            tlpDetalle.Controls.Add(lblTecnicoAutor, 0, 2);
            tlpDetalle.Controls.Add(lblTipo, 0, 3);
            tlpDetalle.Controls.Add(lblKilometraje, 0, 4);
            tlpDetalle.Controls.Add(lblFallaDetectada, 0, 5);
            tlpDetalle.Controls.Add(lblTrabajoRecomendado, 0, 6);
            tlpDetalle.Controls.Add(lblObservaciones, 0, 7);
            tlpDetalle.Controls.Add(txtAtencion, 1, 0);
            tlpDetalle.Controls.Add(txtFecha, 1, 1);
            tlpDetalle.Controls.Add(txtTecnicoAutor, 1, 2);
            tlpDetalle.Controls.Add(txtTipo, 1, 3);
            tlpDetalle.Controls.Add(txtKilometraje, 1, 4);
            tlpDetalle.Controls.Add(txtFallaDetectada, 1, 5);
            tlpDetalle.Controls.Add(txtTrabajoRecomendado, 1, 6);
            tlpDetalle.Controls.Add(txtObservaciones, 1, 7);
            var acciones = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, AutoScroll = true };
            btnExportar = new Button { Name = "btnExportar", Text = "Exportar HTML", AutoSize = true, Height = 34, Enabled = false };
            btnExportar.Click += Exportar;
            btnImprimir = new Button { Name = "btnImprimir", Text = "Imprimir / PDF", AutoSize = true, Height = 34, Enabled = false };
            btnImprimir.Click += Imprimir;
            acciones.Controls.AddRange(new Control[] { btnExportar, btnImprimir, btnCerrar });
            tlpDetalle.Controls.Add(acciones, 1, 8);
            tlpDetalle.Dock = DockStyle.Fill;
            tlpDetalle.Location = new Point(16, 16);
            tlpDetalle.Name = "tlpDetalle";
            tlpDetalle.RowCount = 9;
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.Percent, 33.34F));
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));
            tlpDetalle.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpDetalle.Size = new Size(752, 579);
            tlpDetalle.TabIndex = 0;
            // 
            // lblAtencion
            // 
            lblAtencion.AutoSize = true;
            lblAtencion.Location = new Point(3, 0);
            lblAtencion.Name = "lblAtencion";
            lblAtencion.Size = new Size(55, 15);
            lblAtencion.TabIndex = 0;
            lblAtencion.Text = "Atención";
            // 
            // lblFecha
            // 
            lblFecha.AutoSize = true;
            lblFecha.Location = new Point(3, 36);
            lblFecha.Name = "lblFecha";
            lblFecha.Size = new Size(97, 15);
            lblFecha.TabIndex = 1;
            lblFecha.Text = "Fecha de registro";
            // 
            // lblTecnicoAutor
            // 
            lblTecnicoAutor.AutoSize = true;
            lblTecnicoAutor.Location = new Point(3, 72);
            lblTecnicoAutor.Name = "lblTecnicoAutor";
            lblTecnicoAutor.Size = new Size(79, 15);
            lblTecnicoAutor.TabIndex = 2;
            lblTecnicoAutor.Text = "Técnico autor";
            // 
            // lblTipo
            // 
            lblTipo.AutoSize = true;
            lblTipo.Location = new Point(3, 108);
            lblTipo.Name = "lblTipo";
            lblTipo.Size = new Size(92, 15);
            lblTipo.TabIndex = 3;
            lblTipo.Text = "Tipo de informe";
            // 
            // lblKilometraje
            // 
            lblKilometraje.AutoSize = true;
            lblKilometraje.Location = new Point(3, 144);
            lblKilometraje.Name = "lblKilometraje";
            lblKilometraje.Size = new Size(67, 15);
            lblKilometraje.TabIndex = 4;
            lblKilometraje.Text = "Kilometraje";
            // 
            // lblFallaDetectada
            // 
            lblFallaDetectada.AutoSize = true;
            lblFallaDetectada.Location = new Point(3, 180);
            lblFallaDetectada.Name = "lblFallaDetectada";
            lblFallaDetectada.Size = new Size(86, 15);
            lblFallaDetectada.TabIndex = 5;
            lblFallaDetectada.Text = "Falla detectada";
            // 
            // lblTrabajoRecomendado
            // 
            lblTrabajoRecomendado.AutoSize = true;
            lblTrabajoRecomendado.Location = new Point(3, 298);
            lblTrabajoRecomendado.Name = "lblTrabajoRecomendado";
            lblTrabajoRecomendado.Size = new Size(123, 15);
            lblTrabajoRecomendado.TabIndex = 6;
            lblTrabajoRecomendado.Text = "Trabajo recomendado";
            // 
            // lblObservaciones
            // 
            lblObservaciones.AutoSize = true;
            lblObservaciones.Location = new Point(3, 416);
            lblObservaciones.Name = "lblObservaciones";
            lblObservaciones.Size = new Size(84, 15);
            lblObservaciones.TabIndex = 7;
            lblObservaciones.Text = "Observaciones";
            // 
            // txtAtencion
            // 
            txtAtencion.Dock = DockStyle.Fill;
            txtAtencion.Location = new Point(194, 4);
            txtAtencion.Margin = new Padding(4);
            txtAtencion.Name = "txtAtencion";
            txtAtencion.ReadOnly = true;
            txtAtencion.Size = new Size(554, 23);
            txtAtencion.TabIndex = 8;
            // 
            // txtFecha
            // 
            txtFecha.Dock = DockStyle.Fill;
            txtFecha.Location = new Point(194, 40);
            txtFecha.Margin = new Padding(4);
            txtFecha.Name = "txtFecha";
            txtFecha.ReadOnly = true;
            txtFecha.Size = new Size(554, 23);
            txtFecha.TabIndex = 9;
            // 
            // txtTecnicoAutor
            // 
            txtTecnicoAutor.Dock = DockStyle.Fill;
            txtTecnicoAutor.Location = new Point(194, 76);
            txtTecnicoAutor.Margin = new Padding(4);
            txtTecnicoAutor.Name = "txtTecnicoAutor";
            txtTecnicoAutor.ReadOnly = true;
            txtTecnicoAutor.Size = new Size(554, 23);
            txtTecnicoAutor.TabIndex = 10;
            // 
            // txtTipo
            // 
            txtTipo.Dock = DockStyle.Fill;
            txtTipo.Location = new Point(194, 112);
            txtTipo.Margin = new Padding(4);
            txtTipo.Name = "txtTipo";
            txtTipo.ReadOnly = true;
            txtTipo.Size = new Size(554, 23);
            txtTipo.TabIndex = 11;
            // 
            // txtKilometraje
            // 
            txtKilometraje.Dock = DockStyle.Fill;
            txtKilometraje.Location = new Point(194, 148);
            txtKilometraje.Margin = new Padding(4);
            txtKilometraje.Name = "txtKilometraje";
            txtKilometraje.ReadOnly = true;
            txtKilometraje.Size = new Size(554, 23);
            txtKilometraje.TabIndex = 12;
            // 
            // txtFallaDetectada
            // 
            txtFallaDetectada.Dock = DockStyle.Fill;
            txtFallaDetectada.Location = new Point(194, 184);
            txtFallaDetectada.Margin = new Padding(4);
            txtFallaDetectada.Multiline = true;
            txtFallaDetectada.Name = "txtFallaDetectada";
            txtFallaDetectada.ReadOnly = true;
            txtFallaDetectada.ScrollBars = ScrollBars.Vertical;
            txtFallaDetectada.Size = new Size(554, 110);
            txtFallaDetectada.TabIndex = 13;
            // 
            // txtTrabajoRecomendado
            // 
            txtTrabajoRecomendado.Dock = DockStyle.Fill;
            txtTrabajoRecomendado.Location = new Point(194, 302);
            txtTrabajoRecomendado.Margin = new Padding(4);
            txtTrabajoRecomendado.Multiline = true;
            txtTrabajoRecomendado.Name = "txtTrabajoRecomendado";
            txtTrabajoRecomendado.ReadOnly = true;
            txtTrabajoRecomendado.ScrollBars = ScrollBars.Vertical;
            txtTrabajoRecomendado.Size = new Size(554, 110);
            txtTrabajoRecomendado.TabIndex = 14;
            // 
            // txtObservaciones
            // 
            txtObservaciones.Dock = DockStyle.Fill;
            txtObservaciones.Location = new Point(194, 420);
            txtObservaciones.Margin = new Padding(4);
            txtObservaciones.Multiline = true;
            txtObservaciones.Name = "txtObservaciones";
            txtObservaciones.ReadOnly = true;
            txtObservaciones.ScrollBars = ScrollBars.Vertical;
            txtObservaciones.Size = new Size(554, 110);
            txtObservaciones.TabIndex = 15;
            // 
            // btnCerrar
            // 
            btnCerrar.DialogResult = DialogResult.Cancel;
            btnCerrar.Dock = DockStyle.Right;
            btnCerrar.Location = new Point(639, 537);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(110, 39);
            btnCerrar.TabIndex = 16;
            btnCerrar.Text = "Cerrar";
            btnCerrar.UseVisualStyleBackColor = true;
            // 
            // FrmDetalleDiagnostico
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCerrar;
            ClientSize = new Size(784, 611);
            Controls.Add(tlpDetalle);
            MinimizeBox = false;
            Name = "FrmDetalleDiagnostico";
            Padding = new Padding(16);
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Detalle del diagnóstico";
            Load += FrmDetalleDiagnostico_Load;
            tlpDetalle.ResumeLayout(false);
            tlpDetalle.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpDetalle;
        private Label lblAtencion;
        private Label lblFecha;
        private Label lblTecnicoAutor;
        private Label lblTipo;
        private Label lblKilometraje;
        private Label lblFallaDetectada;
        private Label lblTrabajoRecomendado;
        private Label lblObservaciones;
        private TextBox txtAtencion;
        private TextBox txtFecha;
        private TextBox txtTecnicoAutor;
        private TextBox txtTipo;
        private TextBox txtKilometraje;
        private TextBox txtFallaDetectada;
        private TextBox txtTrabajoRecomendado;
        private TextBox txtObservaciones;
        private Button btnExportar;
        private Button btnImprimir;
        private Button btnCerrar;
    }
}