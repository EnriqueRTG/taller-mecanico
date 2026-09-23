namespace Taller.Presentacion.Formularios.Clientes;

/// <summary>
/// Simula la edición de los datos personales y de contacto
/// del cliente seleccionado.
/// </summary>
public partial class FrmEditarCliente : Form
{
    public FrmEditarCliente()
    {
        InitializeComponent();
        ConfigurarFormulario();
    }

    public void PrepararEdicion(
        int id,
        string tipoDocumento,
        string documento,
        string nombreCompleto,
        string telefono,
        string email,
        string direccion,
        bool activo,
        DateTime fechaAlta)
    {
        string[] partes = nombreCompleto.Split(
            ',', 2, StringSplitOptions.TrimEntries);

        txtId.Text = id.ToString();
        txtNombreUsuario.Text = documento;
        txtFechaAlta.Text = tipoDocumento;
        txtEstado.Text = partes.Length > 1 ? partes[1] : nombreCompleto;
        textBox2.Text = partes.Length > 1 ? partes[0] : string.Empty;
        txtApellido.Text = telefono;
        txtNombre.Text = email;
        textBox4.Text = direccion;
        textBox3.Text = activo ? "Habilitado" : "Deshabilitado";
        textBox1.Text = fechaAlta.ToString("dd/MM/yyyy");

        Text = $"Editar cliente - {nombreCompleto}";
    }

    private void ConfigurarFormulario()
    {
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;

        lblDescripcion.Text =
            "Modifique el nombre, apellido y los datos de contacto. " +
            "La identificación del cliente no puede cambiarse.";

        ConfigurarSoloLectura(txtId);
        ConfigurarSoloLectura(txtNombreUsuario);
        ConfigurarSoloLectura(txtFechaAlta);
        ConfigurarSoloLectura(textBox3);
        ConfigurarSoloLectura(textBox1);

        txtEstado.MaxLength = 100;
        textBox2.MaxLength = 100;
        txtApellido.MaxLength = 30;
        txtNombre.MaxLength = 150;
        textBox4.MaxLength = 200;

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
        if (string.IsNullOrWhiteSpace(txtEstado.Text)
            || string.IsNullOrWhiteSpace(textBox2.Text))
        {
            MessageBox.Show(
                "Complete el nombre y el apellido.",
                "Datos incompletos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return false;
        }

        string email = txtNombre.Text.Trim();

        if (!string.IsNullOrWhiteSpace(email)
            && (!email.Contains('@') || email.StartsWith('@') || email.EndsWith('@')))
        {
            MessageBox.Show(
                "Ingrese un correo electrónico válido o deje el campo vacío.",
                "Correo inválido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            txtNombre.Focus();
            return false;
        }

        return true;
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (!ValidarDatosEditables())
            return;

        MessageBox.Show(
            "El nombre, apellido y los datos de contacto fueron " +
            "guardados en la simulación.",
            "Edición de cliente",
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
