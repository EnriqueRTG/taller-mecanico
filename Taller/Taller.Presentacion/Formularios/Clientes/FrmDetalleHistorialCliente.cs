namespace Taller.Presentacion.Formularios.Clientes;

/// <summary>
/// Presenta el detalle demostrativo de un vehículo o una atención
/// seleccionada desde el historial del cliente.
/// </summary>
public sealed class FrmDetalleHistorialCliente : Form
{
    private readonly string _titulo;
    private readonly string _cliente;
    private readonly IReadOnlyList<KeyValuePair<string, string>> _datos;

    public FrmDetalleHistorialCliente(
        string titulo,
        string cliente,
        IReadOnlyList<KeyValuePair<string, string>> datos)
    {
        _titulo = titulo;
        _cliente = cliente;
        _datos = datos;

        ConfigurarVentana();
        ConstruirInterfaz();
    }

    private void ConfigurarVentana()
    {
        Text = _titulo;
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        MinimumSize = new Size(620, 480);
        Size = new Size(720, 560);
        BackColor = Color.FromArgb(245, 247, 250);
    }

    private void ConstruirInterfaz()
    {
        var principal = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20),
            ColumnCount = 1,
            RowCount = 3
        };

        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 86));
        principal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));

        principal.Controls.Add(CrearEncabezado(), 0, 0);
        principal.Controls.Add(CrearDatos(), 0, 1);
        principal.Controls.Add(CrearAcciones(), 0, 2);

        Controls.Add(principal);
    }

    private Control CrearEncabezado()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(18)
        };

        panel.Controls.Add(new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 15F, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42),
            Text = _titulo,
            Location = new Point(18, 12)
        });

        panel.Controls.Add(new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(100, 116, 139),
            Text = $"Cliente: {_cliente}",
            Location = new Point(20, 50)
        });

        return panel;
    }

    private Control CrearDatos()
    {
        var contenedor = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = new Padding(0, 8, 0, 8),
            Padding = new Padding(18),
            AutoScroll = true
        };

        var tabla = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 2,
            RowCount = _datos.Count,
            Padding = new Padding(4)
        };

        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        for (int indice = 0; indice < _datos.Count; indice++)
        {
            KeyValuePair<string, string> dato = _datos[indice];

            tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));

            tabla.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                Text = dato.Key
            }, 0, indice);

            tabla.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(30, 41, 59),
                Text = dato.Value,
                AutoEllipsis = true
            }, 1, indice);
        }

        contenedor.Controls.Add(tabla);
        return contenedor;
    }

    private Control CrearAcciones()
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(0, 8, 0, 0)
        };

        var cerrar = new Button
        {
            Text = "Cerrar",
            Width = 105,
            Height = 32,
            Cursor = Cursors.Hand,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 64, 175),
            BackColor = Color.White
        };

        cerrar.Click += (_, _) => Close();
        panel.Controls.Add(cerrar);
        return panel;
    }
}
