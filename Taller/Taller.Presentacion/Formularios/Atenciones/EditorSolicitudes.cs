using System.ComponentModel;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Dominio.Enumeraciones;
namespace Taller.Presentacion.Formularios.Atenciones;
/// <summary>Editor de los pedidos del cliente. No selecciona conceptos ni autoriza servicios.</summary>
public partial class EditorSolicitudes : UserControl
{
    private readonly BindingList<SolicitudIngresoDto> _solicitudes = [];
    public event EventHandler? SolicitudesCambiadas;
    public EditorSolicitudes()
    {
        InitializeComponent();
        cboTipo.SelectedIndex = 0;
        dgvSolicitudes.DataSource = _solicitudes;
    }
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<SolicitudIngresoDto> Solicitudes => _solicitudes.Select(s => new SolicitudIngresoDto { IdSolicitud = s.IdSolicitud, Tipo = s.Tipo, Descripcion = s.Descripcion }).ToList();
    [Browsable(false)] public int? Kilometraje => chkKm.Checked ? (int)nudKm.Value : null;
    [Browsable(false)] public string Observaciones => txtObservaciones.Text.Trim();
    private void Agregar(object? sender, EventArgs e)
    {
        var texto = txtSolicitud.Text.Trim();
        if (texto.Length == 0) { txtSolicitud.Focus(); return; }
        if (_solicitudes.Count >= 20 || _solicitudes.Any(s => s.Descripcion.Equals(texto, StringComparison.OrdinalIgnoreCase)))
        { MessageBox.Show(this, "No repita solicitudes y registre como máximo 20.", "Solicitudes"); return; }
        _solicitudes.Add(new SolicitudIngresoDto { Tipo = cboTipo.SelectedIndex == 0 ? TipoSolicitudIngreso.TrabajoSolicitado : TipoSolicitudIngreso.ProblemaInformado, Descripcion = texto });
        txtSolicitud.Clear();
        SolicitudesCambiadas?.Invoke(this, EventArgs.Empty);
        txtSolicitud.Focus();
    }
    private void Quitar(object? sender, EventArgs e)
    {
        if (dgvSolicitudes.CurrentRow?.DataBoundItem is SolicitudIngresoDto s)
        { _solicitudes.Remove(s); SolicitudesCambiadas?.Invoke(this, EventArgs.Empty); }
    }
    private void KilometrajeCambiado(object? sender, EventArgs e) => nudKm.Enabled = chkKm.Checked;
    /// <summary>Evita perder un pedido escrito pero todavía no agregado al listado.</summary>
    public bool ConfirmarPendiente()
    {
        if (!string.IsNullOrWhiteSpace(txtSolicitud.Text)) Agregar(this, EventArgs.Empty);
        return string.IsNullOrWhiteSpace(txtSolicitud.Text) && _solicitudes.Count > 0;
    }
}
