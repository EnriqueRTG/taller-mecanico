using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;

namespace Taller.Presentacion.Formularios.Clientes;

public partial class FrmAltaCliente : Form
{
    private readonly ClienteServicio _clienteServicio;

    /// <summary>
    /// Cliente creado correctamente desde el formulario.
    /// Permite reutilizar el formulario desde otros procesos,
    /// como el alta de una nueva atención.
    /// </summary>
    public Cliente? ClienteCreado { get; private set; }

    /// <summary>
    /// Inicializa el formulario de alta de clientes.
    /// </summary>
    /// <param name="clienteServicio">
    /// Servicio utilizado para registrar clientes.
    /// </param>
    public FrmAltaCliente(ClienteServicio clienteServicio)
    {
        InitializeComponent();

        _clienteServicio = clienteServicio;
    }

    /// <summary>
    /// Configura los controles iniciales del formulario.
    /// </summary>
    private void FrmAltaCliente_Load(object sender, EventArgs e)
    {
        ConfigurarFormulario();
        CargarTiposDocumento();
    }

    /// <summary>
    /// Establece las propiedades iniciales de los controles.
    /// </summary>
    private void ConfigurarFormulario()
    {
        cmbTipoDocumento.DropDownStyle = ComboBoxStyle.DropDownList;

        txtDocumento.MaxLength = 20;
        txtNombre.MaxLength = 80;
        txtApellido.MaxLength = 80;
        txtTelefono.MaxLength = 30;
        txtEmail.MaxLength = 120;
        txtDireccion.MaxLength = 200;

        txtDocumento.CharacterCasing = CharacterCasing.Upper;
    }

    /// <summary>
    /// Carga los tipos de documento disponibles.
    /// </summary>
    private void CargarTiposDocumento()
    {
        cmbTipoDocumento.Items.Clear();

        cmbTipoDocumento.Items.Add("DNI");
        cmbTipoDocumento.Items.Add("CUIT");
        cmbTipoDocumento.Items.Add("CUIL");
        cmbTipoDocumento.Items.Add("Pasaporte");

        cmbTipoDocumento.SelectedIndex = 0;
    }

    /// <summary>
    /// Registra el cliente utilizando los datos ingresados.
    /// </summary>
    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            if (cmbTipoDocumento.SelectedItem is null)
            {
                MessageBox.Show(
                    "Debe seleccionar un tipo de documento.",
                    "Datos incompletos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbTipoDocumento.Focus();
                return;
            }

            var tipoDocumento = cmbTipoDocumento.SelectedItem.ToString()!;

            ClienteCreado = await _clienteServicio.RegistrarAsync(
                tipoDocumento,
                txtDocumento.Text,
                txtNombre.Text,
                txtApellido.Text,
                txtTelefono.Text,
                txtEmail.Text,
                txtDireccion.Text);

            MessageBox.Show(
                $"Cliente {ClienteCreado.Nombre} {ClienteCreado.Apellido} registrado correctamente.",
                "Alta de cliente",
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

    /// <summary>
    /// Cancela el alta y cierra el formulario.
    /// </summary>
    private void btnCancelar_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}