using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Servicios;

namespace Taller.Presentacion.Formularios.Vehiculos;

public partial class FrmAltaVehiculo : Form
{
    private readonly VehiculoServicio _vehiculoServicio;
    private readonly IMarcaRepositorio _marcaRepositorio;
    private readonly IModeloRepositorio _modeloRepositorio;

    public FrmAltaVehiculo(
        VehiculoServicio vehiculoServicio,
        IMarcaRepositorio marcaRepositorio,
        IModeloRepositorio modeloRepositorio)
    {
        InitializeComponent();

        _vehiculoServicio = vehiculoServicio;
        _marcaRepositorio = marcaRepositorio;
        _modeloRepositorio = modeloRepositorio;
    }

    private async void FrmAltaVehiculo_Load(
        object sender,
        EventArgs e)
    {
        try
        {
            ConfigurarFormulario();

            await CargarMarcasAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void ConfigurarFormulario()
    {
        cmbMarca.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbModelo.DropDownStyle = ComboBoxStyle.DropDownList;

        nudAnio.Minimum = 1900;
        nudAnio.Maximum = DateTime.Now.Year + 1;
        nudAnio.Value = DateTime.Now.Year;

        txtDominio.MaxLength = 10;
        txtColor.MaxLength = 50;
    }

    private async Task CargarMarcasAsync()
    {
        var marcas = await _marcaRepositorio.ListarActivosAsync();

        cmbMarca.DataSource = marcas;
        cmbMarca.DisplayMember = "Nombre";
        cmbMarca.ValueMember = "IdMarca";

        cmbMarca.SelectedIndex = -1;

        cmbModelo.DataSource = null;
        cmbModelo.Enabled = false;
    }

    private async void cmbMarca_SelectedIndexChanged(
        object sender,
        EventArgs e)
    {
        if (cmbMarca.SelectedValue is not int idMarca)
        {
            cmbModelo.DataSource = null;
            cmbModelo.Enabled = false;
            return;
        }

        try
        {
            var modelos =
                await _modeloRepositorio
                    .ListarActivosPorMarcaAsync(idMarca);

            cmbModelo.DataSource = modelos;
            cmbModelo.DisplayMember = "Nombre";
            cmbModelo.ValueMember = "IdModelo";

            cmbModelo.SelectedIndex = -1;
            cmbModelo.Enabled = modelos.Count > 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async void btnGuardar_Click(
        object sender,
        EventArgs e)
    {
        try
        {
            if (cmbMarca.SelectedValue is not int)
            {
                MessageBox.Show(
                    "Debe seleccionar una marca.",
                    "Datos incompletos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbMarca.Focus();
                return;
            }

            if (cmbModelo.SelectedValue is not int idModelo)
            {
                MessageBox.Show(
                    "Debe seleccionar un modelo.",
                    "Datos incompletos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbModelo.Focus();
                return;
            }

            var vehiculo = await _vehiculoServicio.RegistrarAsync(
                txtDominio.Text,
                (int)nudAnio.Value,
                txtColor.Text,
                idModelo);

            MessageBox.Show(
                $"Vehículo {vehiculo.Dominio} registrado correctamente.",
                "Alta de vehículo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Datos inválidos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(
                ex.Message,
                "No se pudo registrar",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ocurrió un error inesperado.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void btnCancelar_Click(
        object sender,
        EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}