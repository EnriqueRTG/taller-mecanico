namespace Taller.Presentacion.Estilos;
/// <summary>
/// Centraliza los colores y estilos visuales utilizados
/// por los formularios del sistema.
/// </summary>
public static class EstilosInterfaz
{
    public static readonly Color FondoAplicacion =
        Color.FromArgb(245, 247, 250);

    public static readonly Color FondoPanel =
        Color.White;

    public static readonly Color TextoPrincipal =
        Color.FromArgb(15, 23, 42);

    public static readonly Color TextoNormal =
        Color.FromArgb(30, 41, 59);

    public static readonly Color TextoSecundario =
        Color.FromArgb(100, 116, 139);

    public static readonly Color TextoEtiqueta =
        Color.FromArgb(51, 65, 85);

    public static readonly Color AzulPrincipal =
        Color.FromArgb(30, 64, 175);

    public static readonly Color AzulHover =
        Color.FromArgb(37, 99, 235);

    public static readonly Color AzulPresionado =
        Color.FromArgb(30, 58, 138);

    public static readonly Color AzulSeleccion =
        Color.FromArgb(219, 234, 254);

    public static readonly Color AzulSeleccionTexto =
        Color.FromArgb(30, 64, 175);

    public static readonly Color Borde =
        Color.FromArgb(226, 232, 240);

    public static readonly Color EncabezadoGrilla =
        Color.FromArgb(241, 245, 249);

    public static readonly Color Peligro =
        Color.FromArgb(185, 28, 28);

    public static void AplicarBotonPrimario(Button boton)
    {
        boton.BackColor = AzulPrincipal;
        boton.ForeColor = Color.White;
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderSize = 0;
        boton.FlatAppearance.MouseOverBackColor = AzulHover;
        boton.FlatAppearance.MouseDownBackColor = AzulPresionado;
        boton.Font = new Font(
            "Segoe UI",
            9.5F,
            FontStyle.Bold);
        boton.Cursor = Cursors.Hand;
        boton.UseVisualStyleBackColor = false;
    }

    public static void AplicarBotonSecundario(Button boton)
    {
        boton.BackColor = FondoPanel;
        boton.ForeColor = AzulPrincipal;
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderSize = 1;
        boton.FlatAppearance.BorderColor = AzulPrincipal;
        boton.FlatAppearance.MouseOverBackColor =
            Color.FromArgb(239, 246, 255);
        boton.FlatAppearance.MouseDownBackColor =
            AzulSeleccion;
        boton.Font = new Font(
            "Segoe UI",
            9F,
            FontStyle.Bold);
        boton.Cursor = Cursors.Hand;
        boton.UseVisualStyleBackColor = false;
    }

    public static void AplicarBotonPeligro(Button boton)
    {
        AplicarBotonSecundario(boton);

        boton.ForeColor = Peligro;
        boton.FlatAppearance.BorderColor = Peligro;
    }

    public static void AplicarGrilla(DataGridView grilla)
    {
        grilla.BackgroundColor = FondoPanel;
        grilla.BorderStyle = BorderStyle.None;
        grilla.CellBorderStyle =
            DataGridViewCellBorderStyle.SingleHorizontal;

        grilla.GridColor = Borde;
        grilla.RowHeadersVisible = false;
        grilla.AllowUserToAddRows = false;
        grilla.AllowUserToDeleteRows = false;
        grilla.AllowUserToResizeRows = false;
        grilla.ReadOnly = true;
        grilla.MultiSelect = false;

        grilla.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;

        grilla.AutoGenerateColumns = false;
        grilla.AutoSizeRowsMode =
            DataGridViewAutoSizeRowsMode.None;

        grilla.RowTemplate.Height = 42;

        grilla.EnableHeadersVisualStyles = false;
        grilla.ColumnHeadersHeight = 40;
        grilla.ColumnHeadersHeightSizeMode =
            DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

        grilla.ColumnHeadersDefaultCellStyle.BackColor =
            EncabezadoGrilla;

        grilla.ColumnHeadersDefaultCellStyle.ForeColor =
            TextoEtiqueta;

        grilla.ColumnHeadersDefaultCellStyle.Font =
            new Font("Segoe UI", 9F, FontStyle.Bold);

        grilla.ColumnHeadersDefaultCellStyle.SelectionBackColor =
            EncabezadoGrilla;

        grilla.ColumnHeadersDefaultCellStyle.SelectionForeColor =
            TextoEtiqueta;

        grilla.DefaultCellStyle.BackColor =
            FondoPanel;

        grilla.DefaultCellStyle.ForeColor =
            TextoNormal;

        grilla.DefaultCellStyle.Font =
            new Font("Segoe UI", 9F);

        grilla.DefaultCellStyle.SelectionBackColor =
            AzulSeleccion;

        grilla.DefaultCellStyle.SelectionForeColor =
            AzulSeleccionTexto;
    }
}
