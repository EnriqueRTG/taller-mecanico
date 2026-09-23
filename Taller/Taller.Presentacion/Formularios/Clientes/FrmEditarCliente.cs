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

        txtId.ReadOnly = true;
        textBox3.ReadOnly = true;
        textBox1.ReadOnly = true;

        btnGuardar.Click += BtnGuardar_Click;
        btnCancelar.Click += BtnCancelar_Click;
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombreUsuario.Text)
            || string.IsNullOrWhiteSpace(txtEstado.Text)
            || string.IsNullOrWhiteSpace(textBox2.Text))
        {
            MessageBox.Show(
                "Complete el documento, nombre y apellido.",
                "Datos incompletos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show(
            "Los cambios del cliente fueron guardados en la simulación.",
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
