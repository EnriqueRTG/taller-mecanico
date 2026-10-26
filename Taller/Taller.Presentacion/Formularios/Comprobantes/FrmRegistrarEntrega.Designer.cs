namespace Taller.Presentacion.Formularios.Comprobantes;
partial class FrmRegistrarEntrega
{
    private Label lblResultado = null!;
    private void InitializeComponent()
    {
        Text = "Registrar entrega"; Font = new Font("Segoe UI", 10); ClientSize = new Size(820, 440); MinimumSize = new Size(650, 440); StartPosition = FormStartPosition.CenterParent;
        var tabla = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(16), ColumnCount = 1, RowCount = 7 };
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        foreach (int alto in new[] { 100, 24, 36, 45, 24 }) tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, alto));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        lblResultado = new Label { Name = "lblResultado", Dock = DockStyle.Fill };
        tabla.Controls.Add(lblResultado, 0, 0);
        tabla.Controls.Add(new Label { Text = "Persona que retira (cliente o representante)", Dock = DockStyle.Fill }, 0, 1);
         tabla.Controls.Add(persona, 0, 2); tabla.Controls.Add(conforme, 0, 3);
        tabla.Controls.Add(new Label { Text = "Observaciones de entrega (opcional)", Dock = DockStyle.Fill }, 0, 4); tabla.Controls.Add(notas, 0, 5);
        var acciones = new FlowLayoutPanel { Dock = DockStyle.Fill };
        var confirmar = new Button { Text = "Confirmar entrega", AutoSize = true, Height = 34 };
        confirmar.Click += Confirmar;
        var cancelar = new Button { Text = "Cancelar", AutoSize = true, Height = 34, DialogResult = DialogResult.Cancel };
        acciones.Controls.AddRange([confirmar, cancelar]); tabla.Controls.Add(acciones, 0, 6);
        Estilos.EstilosInterfaz.AplicarBotonPrimario(confirmar); Estilos.EstilosInterfaz.AplicarBotonSecundario(cancelar);
        Controls.Add(tabla); CancelButton = cancelar;
    }
}
