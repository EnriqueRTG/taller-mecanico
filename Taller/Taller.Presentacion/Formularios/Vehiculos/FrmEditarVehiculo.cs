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

    private byte[] _version = [];
    private int _idModelo;
    public void PrepararEdicion(Taller.Aplicacion.DTOs.Vehiculos.VehiculoEdicionDto v)
    {
        _id = v.Id;
        _version = v.Version.ToArray();
        _idModelo = v.IdModelo;
        txtId.Text = v.Id.ToString();
        txtMarca.Text = v.Marca;
        txtAnio.Text = v.Anio.ToString();
        txtColor.Text = v.Color;
        txtDominio.Text = v.Dominio;
        txtEstadoVehiculo.Text = v.Activo ? "Habilitado" : "Deshabilitado";
        txtFechaAltaVehiculo.Text = v.FechaAlta.ToString("dd/MM/yyyy");
        Text = $"Editar vehículo - {v.Dominio}";
    }

    public void CargarModelos(List<Taller.Aplicacion.DTOs.Vehiculos.ModeloOpcionDto> modelos)
    {
        var original = _idModelo;
        cboModeloEdicion.DisplayMember = "Nombre";
        cboModeloEdicion.ValueMember = "Id";
        cboModeloEdicion.DataSource = modelos;
        cboModeloEdicion.SelectedValue = original;
    }

    private void Modelo_Cambio(object? sender, EventArgs e)
    {
        if (cboModeloEdicion.SelectedItem is Taller.Aplicacion.DTOs.Vehiculos.ModeloOpcionDto m)
        {
            _idModelo = m.Id;
            txtMarca.Text = m.Marca;
        }
    }

    private void ConfigurarFormulario()
    {
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        lblDescripcion.Text = "Modifique los datos del vehículo que considere necesarios.";
        ConfigurarCamposEditables();
        txtId.ReadOnly = true;
        txtId.TabStop = false;
        txtMarca.ReadOnly = true;
        txtMarca.TabStop = false;
        txtEstadoVehiculo.ReadOnly = true;
        txtEstadoVehiculo.TabStop = false;
        txtFechaAltaVehiculo.ReadOnly = true;
        txtFechaAltaVehiculo.TabStop = false;
        txtDominio.MaxLength = 10;
        txtMarca.MaxLength = 80;
        txtAnio.MaxLength = 4;
        txtColor.MaxLength = 50;
        // El propietario no forma parte de la edición del vehículo.
        label2.Visible = false;
        textBox2.Visible = false;
        textBox2.TabStop = false;
    }

    private void ConfigurarCamposEditables()
    {
        TextBox[] campos = [txtId, txtDominio, txtMarca, txtAnio, txtColor, txtEstadoVehiculo, txtFechaAltaVehiculo];
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
        if (string.IsNullOrWhiteSpace(txtDominio.Text) || string.IsNullOrWhiteSpace(txtMarca.Text) || cboModeloEdicion.SelectedItem is null || string.IsNullOrWhiteSpace(txtAnio.Text) || string.IsNullOrWhiteSpace(txtColor.Text))
        {
            MessageBox.Show("Complete el dominio, la marca, el modelo, el año y el color.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        if (!int.TryParse(txtAnio.Text, out int anio) || anio < 1900 || anio > DateTime.Today.Year + 1)
        {
            MessageBox.Show($"Ingrese un año entre 1900 y {DateTime.Today.Year + 1}.", "Año inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtAnio.Focus();
            return false;
        }

        return true;
    }

    private async void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (_guardando || !ValidarDatosEditables())
            return;
        try
        {
            _guardando = true;
            btnGuardar.Enabled = btnCancelar.Enabled = false;
            await _servicio.ModificarAsync(_id, txtDominio.Text, int.Parse(txtAnio.Text), txtColor.Text, _idModelo, _version);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Guardar vehículo");
        }
        finally
        {
            _guardando = false;
            btnGuardar.Enabled = btnCancelar.Enabled = true;
        }
    }

    private void Formulario_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_guardando && DialogResult != DialogResult.OK)
            e.Cancel = true;
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
