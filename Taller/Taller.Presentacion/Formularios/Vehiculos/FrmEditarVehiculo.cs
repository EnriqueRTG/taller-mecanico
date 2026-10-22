using Taller.Aplicacion.Servicios;
namespace Taller.Presentacion.Formularios.Vehiculos;

/// <summary>
/// Persiste la edición de los datos del vehículo seleccionado.
/// </summary>
public partial class FrmEditarVehiculo : Form
{
    private readonly VehiculoServicio _servicio;
    private int _id;
    private bool _guardando;
    public FrmEditarVehiculo(VehiculoServicio servicio)
    {
        _servicio = servicio;
        InitializeComponent();
        ConfigurarFormulario();
    }

    public void PrepararEdicion(
        int id,
        string dominio,
        string marca,
        string modelo,
        int anio,
        string color,
        bool activo,
        DateTime fechaAlta)
    {
        _id = id;
        txtId.Text = id.ToString();
        txtMarca.Text = marca;
        txtModelo.Text = modelo;
        txtAnio.Text = anio.ToString();
        txtColor.Text = color;
        txtFechaAltaVehiculo.Text = fechaAlta.ToString("dd/MM/yyyy");
        txtEstadoVehiculo.Text = activo ? "Habilitado" : "Deshabilitado";
        txtDominio.Text = dominio;

        Text = $"Editar vehículo - {dominio}";
    }

    private void ConfigurarFormulario()
    {
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;

        lblDescripcion.Text =
            "Modifique los datos del vehículo que considere necesarios.";

        ConfigurarCamposEditables();
        txtId.ReadOnly = true; txtId.TabStop = false;
        txtMarca.ReadOnly = true; txtMarca.TabStop = false;
        txtModelo.ReadOnly = true; txtModelo.TabStop = false;
        txtEstadoVehiculo.ReadOnly = true; txtEstadoVehiculo.TabStop = false;
        txtFechaAltaVehiculo.ReadOnly = true; txtFechaAltaVehiculo.TabStop = false;


        txtMarca.MaxLength = 80;
        txtModelo.MaxLength = 100;
        txtAnio.MaxLength = 4;
        txtColor.MaxLength = 50;

        // El propietario no forma parte de la edición del vehículo.
        label2.Visible = false;
        textBox2.Visible = false;
        textBox2.TabStop = false;


    }

    private void ConfigurarCamposEditables()
    {
        TextBox[] campos =
        [
            txtId,
            txtDominio,
            txtMarca,
            txtModelo,
            txtAnio,
            txtColor,
            txtEstadoVehiculo,
            txtFechaAltaVehiculo
        ];

        foreach (TextBox campo in campos)
        {
            campo.ReadOnly = false;
            campo.Enabled = true;
            campo.TabStop = true;
            campo.BackColor = SystemColors.Window;
            campo.ForeColor = SystemColors.WindowText;
        }
    }

    private bool ValidarDatosEditables()
    {
        if (string.IsNullOrWhiteSpace(txtDominio.Text)
            || string.IsNullOrWhiteSpace(txtMarca.Text)
            || string.IsNullOrWhiteSpace(txtModelo.Text)
            || string.IsNullOrWhiteSpace(txtAnio.Text)
            || string.IsNullOrWhiteSpace(txtColor.Text))
        {
            MessageBox.Show(
                "Complete el dominio, la marca, el modelo, el año y el color.",
                "Datos incompletos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return false;
        }

        if (!int.TryParse(txtAnio.Text, out int anio)
            || anio < 1900
            || anio > DateTime.Today.Year + 1)
        {
            MessageBox.Show(
                $"Ingrese un año entre 1900 y {DateTime.Today.Year + 1}.",
                "Año inválido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            txtAnio.Focus();
            return false;
        }

        return true;
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_guardando || !ValidarDatosEditables()) return;
        try
        {
            _guardando = true; btnGuardar.Enabled = btnCancelar.Enabled = false;
            var actual = await _servicio.ObtenerPorIdAsync(_id) ?? throw new InvalidOperationException("El vehículo ya no existe.");
            await _servicio.ModificarAsync(_id, txtDominio.Text, int.Parse(txtAnio.Text), txtColor.Text, actual.IdModelo);
            DialogResult = DialogResult.OK; Close();
        }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "Guardar vehículo"); }
        finally { _guardando = false; btnGuardar.Enabled = btnCancelar.Enabled = true; }
    }

    private void Formulario_FormClosing(object? sender, FormClosingEventArgs e) { if (_guardando && DialogResult != DialogResult.OK) e.Cancel = true; }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
