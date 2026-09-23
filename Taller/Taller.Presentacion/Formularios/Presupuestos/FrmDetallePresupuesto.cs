using System.Globalization;
using Taller.Presentacion.Estilos;

namespace Taller.Presentacion.Formularios.Presupuestos;

/// <summary>
/// Muestra el detalle demostrativo de un presupuesto emitido.
/// </summary>
public partial class FrmDetallePresupuesto : Form
{
    private readonly int _numero;
    private readonly int _atencion;
    private readonly DateTime _fecha;
    private readonly string _cliente;
    private readonly string _vehiculo;
    private readonly decimal _total;
    private readonly string _estado;

    public FrmDetallePresupuesto(
        int numero,
        int atencion,
        DateTime fecha,
        string cliente,
        string vehiculo,
        decimal total,
        string estado)
    {
        InitializeComponent();
        _numero = numero;
        _atencion = atencion;
        _fecha = fecha;
        _cliente = cliente;
        _vehiculo = vehiculo;
        _total = total;
        _estado = estado;
    }

    private void FrmDetallePresupuesto_Load(object? sender, EventArgs e)
    {
        ShowInTaskbar = false;
        CancelButton = btnCerrar;
        AplicarEstilos();
        CargarDatos();
    }

    private void CargarDatos()
    {
        lblTitulo.Text = $"Presupuesto P-{_numero:000000}";
        lblEstado.Text = _estado.ToUpperInvariant();
        lblAtencion.Text = $"A-{_atencion:000000}";
        lblFecha.Text = _fecha.ToString("dd/MM/yyyy");
        lblCliente.Text = _cliente;
        lblVehiculo.Text = _vehiculo;

        decimal total = _total > 0 ? _total : 185000m;
        decimal diagnostico = Math.Round(total * 0.08m, 2);
        decimal manoObra = Math.Round(total * 0.37m, 2);
        decimal repuestoUnitario = Math.Round((total - diagnostico - manoObra) / 2m, 2);

        dgvConceptos.Rows.Clear();
        AgregarConcepto("Servicio", "Diagnóstico técnico integral", 1m, diagnostico);
        AgregarConcepto("Mano de obra", "Reparación y puesta a punto", 1m, manoObra);
        AgregarConcepto("Repuesto", "Componentes y materiales estimados", 2m, repuestoUnitario);

        lblSubtotal.Text = FormatearImporte(total);
        lblTotal.Text = FormatearImporte(total);
        lblObservaciones.Text =
            "El presupuesto incluye mano de obra y repuestos estimados. " +
            "La validez demostrativa es de 15 días y puede variar si aparecen fallas adicionales.";

        lblEstado.ForeColor = _estado switch
        {
            "Aprobado" => Color.FromArgb(21, 128, 61),
            "Rechazado" => Color.FromArgb(185, 28, 28),
            _ => Color.FromArgb(180, 83, 9)
        };

        dgvConceptos.ClearSelection();
        dgvConceptos.CurrentCell = null;
    }

    private void AgregarConcepto(string tipo, string descripcion, decimal cantidad, decimal precio)
    {
        dgvConceptos.Rows.Add(
            tipo,
            descripcion,
            cantidad.ToString("0.##"),
            FormatearImporte(precio),
            FormatearImporte(cantidad * precio));
    }

    private static string FormatearImporte(decimal importe)
    {
        return importe.ToString("C2", CultureInfo.GetCultureInfo("es-AR"));
    }

    private void BtnImprimir_Click(object? sender, EventArgs e)
    {
        MessageBox.Show(
            this,
            "Se mostraría la vista de impresión del presupuesto seleccionado.",
            "Vista de impresión",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void BtnCerrar_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void AplicarEstilos()
    {
        EstilosInterfaz.AplicarGrilla(dgvConceptos);
        EstilosInterfaz.AplicarBotonPrimario(btnCerrar);
        EstilosInterfaz.AplicarBotonSecundario(btnImprimir);
    }
}
