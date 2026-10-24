namespace Taller.Presentacion.Estilos;
/// <summary>Distribuye las acciones de búsqueda sin modificar los criterios de negocio.</summary>
internal static class DisposicionFiltros
{
    public static void Ajustar(TableLayoutPanel tabla, Control acciones, int dpi)
    {
        bool compacto = tabla.ClientSize.Width < 900 * dpi / 96;
        int fila = compacto ? 1 : 0;
        int columna = compacto ? 0 : 2;
        int extension = compacto ? 3 : 1;
        if (tabla.GetRow(acciones) == fila && tabla.GetColumn(acciones) == columna && tabla.GetColumnSpan(acciones) == extension)
            return;
        tabla.SuspendLayout();
        try
        {
            tabla.SetColumnSpan(acciones, 1);
            tabla.SetCellPosition(acciones, new TableLayoutPanelCellPosition(columna, fila));
            tabla.SetColumnSpan(acciones, extension);
        }
        finally { tabla.ResumeLayout(true); }
    }
}
