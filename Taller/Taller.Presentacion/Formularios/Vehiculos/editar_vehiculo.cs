namespace Taller.Presentacion.Formularios.Vehiculos;

/// <summary>
/// Simula la edición de los datos descriptivos del vehículo seleccionado.
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

        Text = $"Editar vehículo - {dominio}";
    }

    private void ConfigurarFormulario()
    {
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;

        lblDescripcion.Text =
            "Modifique la marca, el modelo, el año y el color. " +
            "El dominio no puede cambiarse.";

        ConfigurarSoloLectura(txtId);
        ConfigurarSoloLectura(txtApellido);
        ConfigurarSoloLectura(textBox3);
        ConfigurarSoloLectura(textBox4);

        txtNombreUsuario.MaxLength = 80;
        txtFechaAlta.MaxLength = 100;
        txtEstado.MaxLength = 4;
        textBox1.MaxLength = 50;

        // El propietario no forma parte de la edición del vehículo.
        label2.Visible = false;
        textBox2.Visible = false;
        textBox2.TabStop = false;

        btnGuardar.Click += BtnGuardar_Click;
        btnCancelar.Click += BtnCancelar_Click;
    }

    private static void ConfigurarSoloLectura(TextBox control)
    {
        control.ReadOnly = true;
        control.TabStop = false;
        control.BackColor = Color.FromArgb(241, 245, 249);
        control.ForeColor = Color.FromArgb(71, 85, 105);
    }

    private bool ValidarDatosEditables()
    {
        if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text)
            || string.IsNullOrWhiteSpace(txtFechaAlta.Text)
            || string.IsNullOrWhiteSpace(txtEstado.Text)
            || string.IsNullOrWhiteSpace(textBox1.Text))
        {
            MessageBox.Show(
                "Complete la marca, el modelo, el año y el color.",
                "Datos incompletos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return false;
        }

        if (!int.TryParse(txtEstado.Text, out int anio)
            || anio < 1900
            || anio > DateTime.Today.Year + 1)
        {
            MessageBox.Show(
                $"Ingrese un año entre 1900 y {DateTime.Today.Year + 1}.",
                "Año inválido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            txtEstado.Focus();
            return false;
        }

        return true;
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (!ValidarDatosEditables())
            return;

        MessageBox.Show(
            "La marca, el modelo, el año y el color fueron " +
            "guardados en la simulación.",
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
