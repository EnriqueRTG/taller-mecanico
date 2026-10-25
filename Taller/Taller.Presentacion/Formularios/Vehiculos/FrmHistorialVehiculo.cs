using Taller.Aplicacion.Servicios;
using Taller.Presentacion.Estilos;

namespace Taller.Presentacion.Formularios.Vehiculos;
/// <summary>
/// Representa la consulta visual del historial de atenciones
/// asociadas a un vehículo.
/// </summary>
public partial class FrmHistorialVehiculo : Form
{
    private bool _cargando;
    private string _dominio = "";
    private readonly AtencionServicio _atenciones;
    private readonly VehiculoServicio _vehiculos;
    public FrmHistorialVehiculo(AtencionServicio atenciones, VehiculoServicio vehiculos)
    {
        _atenciones = atenciones;
        _vehiculos = vehiculos;
        InitializeComponent();
    }

    /// <summary>
    /// Recibe los datos de la fila seleccionada antes de abrir el historial.
    /// </summary>
    public void PrepararConsulta(string dominio)
    {
        if (string.IsNullOrWhiteSpace(dominio) || dominio.Trim().Length > 10 || dominio.Any(char.IsControl))
            throw new ArgumentException("Seleccione un vehículo válido antes de consultar su historial.", nameof(dominio));
        _dominio = dominio.Trim().ToUpperInvariant();
    }

    private async void FrmHistorialVehiculo_Load(object? sender, EventArgs e)
    {
        ConfigurarFormulario();
        AplicarEstilos();
        await CargarAsync();
    }

    private void ConfigurarFormulario()
    {
        Text = $"Historial del vehículo - {_dominio}";
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        CancelButton = btnCerrar;
    }

    private async Task CargarAsync()
    {
        try
        {
            _cargando = true;
            btnCerrar.Enabled = false;
            lblDominioEncabezado.Text = lblDominioValor.Text = _dominio;
            var vehiculo = await _vehiculos.ObtenerPorDominioAsync(_dominio) ?? throw new InvalidOperationException("El vehículo ya no existe.");
            lblModeloValor.Text = $"{vehiculo.Modelo.Marca.Nombre} {vehiculo.Modelo.Nombre}";
            lblAnioValor.Text = vehiculo.Anio.ToString();
            var datos = await _atenciones.ListarPorVehiculoAsync(vehiculo.IdVehiculo);
            var ultima = datos.OrderByDescending(a => a.FechaApertura).FirstOrDefault();
            lblClienteValor.Text = ultima is null ? "Sin atenciones" : ultima.Cliente.Apellido + ", " + ultima.Cliente.Nombre;
            dgvHistorial.Rows.Clear();
            foreach (var a in datos.OrderByDescending(a => a.FechaApertura))
                dgvHistorial.Rows.Add(a.IdAtencion.ToString("D6"), a.FechaApertura.ToString("dd/MM/yyyy"), a.MotivoConsulta, string.Join(" · ", a.Diagnosticos.Select(d => d.FallaDetectada)), string.Join(" · ", a.Diagnosticos.Select(d => d.TrabajoRecomendado)), ReporteServicio.Legible(a.Estado.ToString()));
            lblCantidad.Text = $"{datos.Count} atenciones registradas";
            dgvHistorial.ClearSelection();
        }
        catch (Exception ex)
        {
            dgvHistorial.Rows.Clear();
            lblCantidad.Text = "No se pudo cargar el historial";
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Historial del vehículo");
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

    private void BtnCerrar_Click(object? sender, EventArgs e)
    {
        Close();
    }

    private void AplicarEstilos()
    {
        BackColor = EstilosInterfaz.FondoAplicacion;
        EstilosInterfaz.AplicarBotonSecundario(btnCerrar);
        EstilosInterfaz.AplicarGrilla(dgvHistorial);
    }
}
