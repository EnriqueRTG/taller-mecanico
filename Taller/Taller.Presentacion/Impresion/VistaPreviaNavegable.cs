namespace Taller.Presentacion.Impresion;
/// <summary>Interacción de lectura sobre el control WinForms: no modifica el documento ni la impresión.</summary>
internal sealed class VistaPreviaNavegable : PrintPreviewControl
{
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public int TotalPaginas { get; set; } = 1;
    public event Action<int>? SolicitarZoom;
    public VistaPreviaNavegable()
    {
        SetStyle(ControlStyles.Selectable, true);
        TabStop = true;
    }
    protected override void OnMouseDown(MouseEventArgs e) { Focus(); base.OnMouseDown(e); }
    protected override bool IsInputKey(Keys keyData) => (keyData & Keys.KeyCode) is Keys.Up or Keys.Down or Keys.Left or Keys.Right or Keys.PageUp or Keys.PageDown or Keys.Home or Keys.End || base.IsInputKey(keyData);
    protected override void OnKeyDown(KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case Keys.PageDown: Pagina(1); break;
            case Keys.PageUp: Pagina(-1); break;
            case Keys.Home: StartPage = 0; break;
            case Keys.End: StartPage = Math.Max(0, TotalPaginas - 1); break;
            case Keys.Up: if (AutoZoom) Pagina(-1); else Desplazar(false, 0); break;
            case Keys.Down: if (AutoZoom) Pagina(1); else Desplazar(false, 1); break;
            case Keys.Left: if (AutoZoom) Pagina(-1); else Desplazar(true, 0); break;
            case Keys.Right: if (AutoZoom) Pagina(1); else Desplazar(true, 1); break;
            case Keys.Add: case Keys.Oemplus: SolicitarZoom?.Invoke(1); break;
            case Keys.Subtract: case Keys.OemMinus: SolicitarZoom?.Invoke(-1); break;
            default: base.OnKeyDown(e); return;
        }
        e.Handled = e.SuppressKeyPress = true;
    }
    protected override void OnMouseWheel(MouseEventArgs e)
    {
        if ((ModifierKeys & Keys.Control) != 0) { SolicitarZoom?.Invoke(Math.Sign(e.Delta)); return; }
        if (e.Delta == 0) return;
        if (AutoZoom) Pagina(e.Delta < 0 ? 1 : -1);
        else for (int paso = 0; paso < 3; paso++) Desplazar((ModifierKeys & Keys.Shift) != 0, e.Delta < 0 ? 1 : 0);
    }
    private void Pagina(int cambio) => StartPage = Math.Clamp(StartPage + cambio, 0, Math.Max(0, TotalPaginas - 1));
    private void Desplazar(bool horizontal, int direccion)
    {
        // Mensajes estándar de desplazamiento del propio control; no requiere acceso a procesos externos.
        var mensaje = Message.Create(Handle, horizontal ? 0x0114 : 0x0115, (IntPtr)direccion, IntPtr.Zero);
        WndProc(ref mensaje);
    }
}
