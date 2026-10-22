using Taller.Aplicacion.Servicios;

namespace Taller.Presentacion.Formularios.Clientes;
/// <summary>Atenciones reales vinculadas al cliente, incluyendo vehículos atendidos e informes disponibles.</summary>
public partial class FrmHistorialCliente : Form
{
    private bool _cargando;
    private readonly int _id;
    private readonly AtencionServicio _servicio;
    public FrmHistorialCliente(int id, string nombre, string documento, AtencionServicio servicio)
    {
        InitializeComponent();
        _id = id;
        _servicio = servicio;
        lblCliente.Text = nombre + " · " + documento;
    }

    private async void Formulario_Load(object? sender, EventArgs e)
    {
        try
        {
            _cargando = true;
            btnCerrar.Enabled = false;
            var datos = await _servicio.ListarPorClienteAsync(_id);
            dgvDatos.DataSource = datos.Select(a => new { Numero = a.IdAtencion, Fecha = a.FechaApertura, Dominio = a.Vehiculo.Dominio, Motivo = a.MotivoConsulta, Estado = ReporteServicio.Legible(a.Estado.ToString()), Informe = string.Join(" · ", a.Diagnosticos.Select(d => d.FallaDetectada)) }).ToList();
            dgvDatos.ClearSelection();
            lblResumen.Text = $"{datos.Count} atenciones · {datos.Select(a => a.IdVehiculo).Distinct().Count()} vehículos atendidos para este cliente";
        }
        catch (Exception ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Historial del cliente");
        }
        finally
        {
            _cargando = false;
            btnCerrar.Enabled = true;
        }
    }

    private void Formulario_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_cargando)
            e.Cancel = true;
    }

    private void BtnCerrar_Click(object? sender, EventArgs e) => Close();
}
