using Taller.Aplicacion.Servicios;
namespace Taller.Presentacion.Formularios.Clientes;

/// <summary>
/// Persiste la edición de los datos del cliente seleccionado.
/// </summary>
public partial class FrmEditarCliente : Form
{
    private readonly ClienteServicio _servicio;
    private int _id;
    private bool _guardando;
    public FrmEditarCliente(ClienteServicio servicio)
    {
        _servicio = servicio;
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

        _id = id;
        txtId.Text = id.ToString();
        txtDocumento.Text = documento;
        txtTipoDocumento.Text = tipoDocumento;
        txtNombreCliente.Text = partes.Length > 1 ? partes[1] : nombreCompleto;
        txtApellidoCliente.Text = partes.Length > 1 ? partes[0] : string.Empty;
        txtTelefono.Text = telefono;
        txtEmail.Text = email;
        txtDireccion.Text = direccion;
        txtEstadoCliente.Text = activo ? "Habilitado" : "Deshabilitado";
        txtFechaAltaCliente.Text = fechaAlta.ToString("dd/MM/yyyy");

        Text = $"Editar cliente - {nombreCompleto}";
    }

    private void ConfigurarFormulario()
    {
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;

        lblDescripcion.Text =
            "Modifique los datos del cliente que considere necesarios.";

        ConfigurarCamposEditables();
        txtId.ReadOnly = true; txtId.TabStop = false;
        txtEstadoCliente.ReadOnly = true; txtEstadoCliente.TabStop = false;
        txtFechaAltaCliente.ReadOnly = true; txtFechaAltaCliente.TabStop = false;


        txtNombreCliente.MaxLength = 100;
        txtApellidoCliente.MaxLength = 100;
        txtTelefono.MaxLength = 30;
        txtEmail.MaxLength = 150;
        txtDireccion.MaxLength = 200;


    }

    private void ConfigurarCamposEditables()
    {
        TextBox[] campos =
        [
            txtId,
            txtDocumento,
            txtTipoDocumento,
            txtNombreCliente,
            txtApellidoCliente,
            txtTelefono,
            txtEmail,
            txtDireccion,
            txtEstadoCliente,
            txtFechaAltaCliente
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
        if (string.IsNullOrWhiteSpace(txtNombreCliente.Text)
            || string.IsNullOrWhiteSpace(txtApellidoCliente.Text))
        {
            MessageBox.Show(
                "Complete el nombre y el apellido.",
                "Datos incompletos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return false;
        }

        string email = txtEmail.Text.Trim();

        if (!string.IsNullOrWhiteSpace(email)
            && (!email.Contains('@') || email.StartsWith('@') || email.EndsWith('@')))
        {
            MessageBox.Show(
                "Ingrese un correo electrónico válido o deje el campo vacío.",
                "Correo inválido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            txtEmail.Focus();
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
            await _servicio.ModificarAsync(_id, txtTipoDocumento.Text, txtDocumento.Text, txtNombreCliente.Text, txtApellidoCliente.Text, txtTelefono.Text, txtEmail.Text, txtDireccion.Text);
            DialogResult = DialogResult.OK; Close();
        }
        catch (Exception ex) { MessageBox.Show(this, ex.Message, "Guardar cliente"); }
        finally { _guardando = false; btnGuardar.Enabled = btnCancelar.Enabled = true; }
    }

    private void Formulario_FormClosing(object? sender, FormClosingEventArgs e) { if (_guardando && DialogResult != DialogResult.OK) e.Cancel = true; }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
