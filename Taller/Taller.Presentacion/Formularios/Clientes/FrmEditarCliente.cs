namespace Taller.Presentacion.Formularios.Clientes;

/// <summary>
/// Simula la edición de los datos del cliente seleccionado.
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
            "Modifique los datos del cliente que considere necesarios.";

        ConfigurarCamposEditables();

        txtEstado.MaxLength = 100;
        textBox2.MaxLength = 100;
        txtApellido.MaxLength = 30;
        txtNombre.MaxLength = 150;
        textBox4.MaxLength = 200;

        btnGuardar.Click += BtnGuardar_Click;
        btnCancelar.Click += BtnCancelar_Click;
    }

    private void ConfigurarCamposEditables()
    {
        TextBox[] campos =
        [
            txtId,
            txtNombreUsuario,
            txtFechaAlta,
            txtEstado,
            textBox2,
            txtApellido,
            txtNombre,
            textBox4,
            textBox3,
            textBox1
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
            "Los datos del cliente fueron guardados en la simulación.",
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
