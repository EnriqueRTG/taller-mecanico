namespace Taller.Presentacion.Formularios.Vehiculos;

/// <summary>
/// Simula la edición de los datos del vehículo seleccionado.
/// </summary>
public partial class editar_vehiculo : Form
{
    public editar_vehiculo()
    {
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
        string propietario,
        bool activo,
        DateTime fechaAlta)
    {
        txtId.Text = id.ToString();
        txtNombreUsuario.Text = marca;
        txtFechaAlta.Text = modelo;
        txtEstado.Text = anio.ToString();
        textBox1.Text = color;
        textBox4.Text = fechaAlta.ToString("dd/MM/yyyy");
        textBox3.Text = activo ? "Habilitado" : "Deshabilitado";
        txtApellido.Text = dominio;
        textBox2.Text = propietario;

        Text = $"Editar vehículo - {dominio}";
    }

    private void ConfigurarFormulario()
    {
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;

        txtId.ReadOnly = true;
        textBox3.ReadOnly = true;
        textBox4.ReadOnly = true;

        btnGuardar.Click += BtnGuardar_Click;
        btnCancelar.Click += BtnCancelar_Click;
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtApellido.Text)
            || string.IsNullOrWhiteSpace(txtNombreUsuario.Text)
            || string.IsNullOrWhiteSpace(txtFechaAlta.Text))
        {
            MessageBox.Show(
                "Complete el dominio, la marca y el modelo.",
                "Datos incompletos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show(
            "Los cambios del vehículo fueron guardados en la simulación.",
            "Edición de vehículo",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
