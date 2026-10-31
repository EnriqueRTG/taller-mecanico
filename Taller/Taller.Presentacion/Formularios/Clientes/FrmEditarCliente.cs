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

    private void Formulario_Shown(object? sender, EventArgs e) =>
        Taller.Presentacion.Estilos.DisposicionVentanas.AjustarAlAreaDisponible(this);

    private byte[] _version = [];
    public void PrepararEdicion(Taller.Aplicacion.DTOs.Clientes.ClienteEdicionDto c)
    {
        _id = c.Id;
        _version = c.Version.ToArray();
        txtId.Text = c.Id.ToString();
        txtDocumento.Text = c.Documento;
        txtTipoDocumento.Text = c.TipoDocumento;
        txtNombreCliente.Text = c.Nombre;
        txtApellidoCliente.Text = c.Apellido;
        txtTelefono.Text = c.Telefono;
        txtEmail.Text = c.Email;
        txtDireccion.Text = c.Direccion;
        txtEstadoCliente.Text = c.Activo ? "Habilitado" : "Deshabilitado";
        txtFechaAltaCliente.Text = c.FechaAlta.ToString("dd/MM/yyyy");
        Text = $"Editar cliente - {c.Apellido}, {c.Nombre}";
    }

    private void ConfigurarFormulario()
    {
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;
        lblDescripcion.Text = "Modifique los datos del cliente que considere necesarios.";
        ConfigurarCamposEditables();
        txtId.ReadOnly = true;
        txtId.TabStop = false;
        txtEstadoCliente.ReadOnly = true;
        txtEstadoCliente.TabStop = false;
        txtFechaAltaCliente.ReadOnly = true;
        txtFechaAltaCliente.TabStop = false;
        txtNombreCliente.MaxLength = 80;
        txtApellidoCliente.MaxLength = 80;
        txtTipoDocumento.MaxLength = 20;
        txtDocumento.MaxLength = 20;
        txtTelefono.MaxLength = 30;
        txtTelefono.PlaceholderText = "Obligatorio: ej. +54 379 4123456";
        txtEmail.MaxLength = 120;
        txtDireccion.MaxLength = 200;
    }

    private void ConfigurarCamposEditables()
    {
        TextBox[] campos = [txtId, txtDocumento, txtTipoDocumento, txtNombreCliente, txtApellidoCliente, txtTelefono, txtEmail, txtDireccion, txtEstadoCliente, txtFechaAltaCliente];
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
        if (string.IsNullOrWhiteSpace(txtNombreCliente.Text) || string.IsNullOrWhiteSpace(txtApellidoCliente.Text))
        {
            MessageBox.Show("Complete el nombre y el apellido.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        string email = txtEmail.Text.Trim();
        if (!string.IsNullOrWhiteSpace(email) && (!email.Contains('@') || email.StartsWith('@') || email.EndsWith('@')))
        {
            MessageBox.Show("Ingrese un correo electrónico válido o deje el campo vacío.", "Correo inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtEmail.Focus();
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
            await _servicio.ModificarAsync(_id, txtTipoDocumento.Text, txtDocumento.Text, txtNombreCliente.Text, txtApellidoCliente.Text, txtTelefono.Text, txtEmail.Text, txtDireccion.Text, _version);
            MessageBox.Show(this, $"Datos de {txtNombreCliente.Text.Trim()} {txtApellidoCliente.Text.Trim()} actualizados correctamente.", "Edición de cliente", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Guardar cliente");
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
