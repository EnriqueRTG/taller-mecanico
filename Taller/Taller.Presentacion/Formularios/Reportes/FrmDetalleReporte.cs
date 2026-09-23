namespace Taller.Presentacion.Formularios.Reportes;

/// <summary>
/// Muestra el detalle demostrativo de un resultado de reporte.
/// </summary>
public sealed class FrmDetalleReporte : Form
{
    private readonly string _tipoReporte;
    private readonly string _rol;
    private readonly DateTime _desde;
    private readonly DateTime _hasta;
    private readonly string _codigo;
    private readonly string _descripcion;
    private readonly string _responsable;
    private readonly string _estado;
    private readonly string _valor;

    public FrmDetalleReporte(
        string tipoReporte,
        string rol,
        DateTime desde,
        DateTime hasta,
        string codigo,
        string descripcion,
        string responsable,
        string estado,
        string valor)
    {
        _tipoReporte = tipoReporte;
        _rol = rol;
        _desde = desde;
        _hasta = hasta;
        _codigo = codigo;
        _descripcion = descripcion;
        _responsable = responsable;
        _estado = estado;
        _valor = valor;

        ConfigurarVentana();
        ConstruirInterfaz();
    }

    private void ConfigurarVentana()
    {
        Text = $"Detalle de reporte - {_codigo}";
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        MinimumSize = new Size(780, 570);
        Size = new Size(900, 640);
        BackColor = Color.FromArgb(245, 247, 250);
    }

    private void ConstruirInterfaz()
    {
        var principal = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(20),
            ColumnCount = 1,
            RowCount = 4
        };

        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 88));
        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 155));
        principal.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        principal.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

        principal.Controls.Add(CrearEncabezado(), 0, 0);
        principal.Controls.Add(CrearResumen(), 0, 1);
        principal.Controls.Add(CrearDetalle(), 0, 2);
        principal.Controls.Add(CrearAcciones(), 0, 3);

        Controls.Add(principal);
    }

    private Control CrearEncabezado()
    {
        var panel = CrearPanel();

        var titulo = new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42),
            Text = _tipoReporte,
            Location = new Point(18, 12)
        };

        var descripcion = new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(100, 116, 139),
            Text = $"Período: {_desde:dd/MM/yyyy} al {_hasta:dd/MM/yyyy} · Perfil: {_rol}",
            Location = new Point(20, 51)
        };

        panel.Controls.Add(titulo);
        panel.Controls.Add(descripcion);
        return panel;
    }

    private Control CrearResumen()
    {
        var tabla = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Margin = new Padding(0, 8, 0, 8),
            Padding = new Padding(18),
            ColumnCount = 3,
            RowCount = 2
        };

        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        tabla.Controls.Add(CrearDato("Código", _codigo), 0, 0);
        tabla.Controls.Add(CrearDato("Estado", _estado), 1, 0);
        tabla.Controls.Add(CrearDato("Importe / cantidad", _valor), 2, 0);
        tabla.Controls.Add(CrearDato("Responsable", _responsable), 0, 1);
        tabla.SetColumnSpan(tabla.GetControlFromPosition(0, 1)!, 2);
        tabla.Controls.Add(CrearDato("Generado", DateTime.Now.ToString("dd/MM/yyyy HH:mm")), 2, 1);

        return tabla;
    }

    private static Control CrearDato(string etiqueta, string valor)
    {
        var panel = new Panel { Dock = DockStyle.Fill };

        panel.Controls.Add(new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(100, 116, 139),
            Text = etiqueta.ToUpperInvariant(),
            Location = new Point(4, 4)
        });

        panel.Controls.Add(new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 64, 175),
            Text = valor,
            Location = new Point(4, 27)
        });

        return panel;
    }

    private Control CrearDetalle()
    {
        var panel = CrearPanel();
        panel.Padding = new Padding(18);

        var titulo = new Label
        {
            AutoSize = true,
            Font = new Font("Segoe UI", 11F, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42),
            Text = "Detalle del resultado",
            Location = new Point(18, 16)
        };

        var descripcion = new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.White,
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.FromArgb(30, 41, 59),
            Text = _descripcion + Environment.NewLine + Environment.NewLine
                + "Este contenido representa la información que se obtendrá "
                + "cuando el reporte consulte los datos definitivos del sistema.",
            Location = new Point(18, 52),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom
                | AnchorStyles.Left | AnchorStyles.Right,
            Size = new Size(800, 180)
        };

        panel.Resize += (_, _) =>
        {
            descripcion.Width = Math.Max(300, panel.ClientSize.Width - 36);
            descripcion.Height = Math.Max(100, panel.ClientSize.Height - 70);
        };

        panel.Controls.Add(titulo);
        panel.Controls.Add(descripcion);
        return panel;
    }

    private Control CrearAcciones()
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(0, 9, 0, 0)
        };

        var cerrar = CrearBoton("Cerrar", 100);
        var imprimir = CrearBoton("Imprimir", 110);
        var exportar = CrearBoton("Exportar", 110);

        cerrar.Click += (_, _) => Close();
        imprimir.Click += (_, _) => MostrarSimulacion(
            "Se abrirá la vista de impresión del detalle.");
        exportar.Click += (_, _) => MostrarSimulacion(
            "El detalle podrá exportarse a PDF o Excel.");

        panel.Controls.Add(cerrar);
        panel.Controls.Add(imprimir);
        panel.Controls.Add(exportar);

        return panel;
    }

    private static Panel CrearPanel()
    {
        return new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White
        };
    }

    private static Button CrearBoton(string texto, int ancho)
    {
        return new Button
        {
            Text = texto,
            Width = ancho,
            Height = 32,
            Cursor = Cursors.Hand,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 64, 175),
            BackColor = Color.White
        };
    }

    private void MostrarSimulacion(string mensaje)
    {
        MessageBox.Show(
            mensaje + Environment.NewLine
            + "La operación no genera archivos en esta versión demostrativa.",
            "Función simulada",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}
