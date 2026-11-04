using Taller.Dominio.Tiempo;

namespace Taller.Presentacion.Estilos;

/// <summary>Distribución común de búsquedas: campos etiquetados y acciones en una fila independiente.</summary>
internal static class FiltrosBandeja
{
    public static void Configurar(TableLayoutPanel tabla, Control acciones,
        params (string Etiqueta, Control Editor)[] campos)
    {
        tabla.Name = "tlpFiltros";
        tabla.Dock = DockStyle.Fill;
        tabla.AutoSize = true;
        tabla.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        tabla.BackColor = Color.White;
        tabla.Padding = new Padding(12, 8, 12, 8);
        foreach (var (etiqueta, editor) in campos)
        {
            var grupo = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink, ColumnCount = 1, RowCount = 2,
                MinimumSize = new Size(0, 62), Margin = new Padding(0, 0, 12, 4) };
            grupo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            grupo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grupo.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            grupo.Controls.Add(new Label { Text = etiqueta, AutoSize = true, Dock = DockStyle.Fill,
                ForeColor = EstilosInterfaz.TextoEtiqueta, Margin = new Padding(0, 0, 0, 5) }, 0, 0);
            editor.Dock = DockStyle.Fill;
            editor.Margin = Padding.Empty;
            editor.AccessibleName = etiqueta;
            grupo.Controls.Add(editor, 0, 1);
            tabla.Controls.Add(grupo);
        }
        acciones.Anchor = AnchorStyles.Right | AnchorStyles.Top;
        acciones.Margin = Padding.Empty;
        if (acciones is FlowLayoutPanel flujo)
        {
            flujo.AutoSize = true;
            flujo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flujo.WrapContents = false;
            flujo.AutoScroll = false;
        }
        tabla.Controls.Add(acciones);
        bool ajustando = false;
        int columnasActuales = 0;
        void Ajustar()
        {
            if (ajustando) return;
            int columnas = tabla.ClientSize.Width < 1050 * tabla.DeviceDpi / 96 ? 2 : campos.Length;
            if (columnas == columnasActuales) return;
            columnasActuales = columnas;
            ajustando = true;
            tabla.SuspendLayout();
            try
            {
                // En notebook se usan dos filas de campos; en pantallas amplias, una.
                int filas = (campos.Length + columnas - 1) / columnas;
                tabla.ColumnCount = columnas;
                tabla.RowCount = filas + 1;
                tabla.ColumnStyles.Clear();
                tabla.RowStyles.Clear();
                for (int i = 0; i < columnas; i++)
                    tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,
                        columnas == 2 ? (i == 0 ? 60 : 40) : (i == 0 ? 40 : 60f / (columnas - 1))));
                for (int i = 0; i <= filas; i++) tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tabla.SetColumnSpan(acciones, 1);
                for (int i = 0; i < campos.Length; i++)
                    tabla.SetCellPosition(campos[i].Editor.Parent!, new TableLayoutPanelCellPosition(i % columnas, i / columnas));
                tabla.SetCellPosition(acciones, new TableLayoutPanelCellPosition(0, filas));
                tabla.SetColumnSpan(acciones, columnas);
            }
            finally { tabla.ResumeLayout(true); ajustando = false; }
        }
        tabla.SizeChanged += (_, _) => Ajustar();
        Ajustar();
    }

    /// <summary>Los extremos se activan individualmente: iniciar o limpiar no oculta pendientes antiguos.</summary>
    public static DateTimePicker CrearFecha(string nombre) => new()
    {
        Name = nombre, Format = DateTimePickerFormat.Short, ShowCheckBox = true,
        Value = FechaHoraTaller.Hoy, Checked = false
    };

    public static bool RangoValido(DateTimePicker desde, DateTimePicker hasta) =>
        !desde.Checked || !hasta.Checked || desde.Value.Date <= hasta.Value.Date;

    /// <summary>Incluye el día completo de ambos extremos, sin abrir conexiones ni alterar la hora del registro.</summary>
    public static bool Incluye(DateTime fecha, DateTimePicker desde, DateTimePicker hasta) =>
        RangoValido(desde, hasta) && (!desde.Checked || fecha.Date >= desde.Value.Date)
            && (!hasta.Checked || fecha.Date <= hasta.Value.Date);
}
