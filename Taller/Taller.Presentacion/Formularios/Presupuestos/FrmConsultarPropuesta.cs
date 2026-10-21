using Taller.Aplicacion.DTOs.Atenciones;
namespace Taller.Presentacion.Formularios.Presupuestos;
/// <summary>Consulta inmutable de un presupuesto persistido; los importes no equivalen a un cobro.</summary>
public partial class FrmConsultarPropuesta : Form
{
    public FrmConsultarPropuesta(PresupuestoConsultaDto datos)
    {
        InitializeComponent();
        lblResumen.Text = $"Presupuesto {datos.IdPresupuesto:D6} · Atención {datos.IdAtencion:D6} · {datos.Cliente} · {datos.Vehiculo}\nTipo: {datos.TipoDescripcion} · Estado: {datos.EstadoDescripcion}";
        dgvItems.DataSource = datos.Items;
        txtObservaciones.Text = datos.Observaciones;
        lblTotal.Text = $"Total del documento: {datos.Total:N2}. No es un comprobante de pago. Los cargos iniciales pertenecen a su propio documento.";
    }
    private void Cerrar(object? s, EventArgs e) => Close();
}
