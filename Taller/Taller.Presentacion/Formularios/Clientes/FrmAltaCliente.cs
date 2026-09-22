using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;
using Taller.Presentacion.Estilos;

namespace Taller.Presentacion.Formularios.Clientes;

/// <summary>
/// Permite registrar un nuevo cliente mediante sus datos de
/// identificación, contacto y domicilio.
///
/// El formulario delega las validaciones de negocio y la persistencia
/// del cliente en la capa de aplicación.
/// </summary>
public partial class FrmAltaCliente : Form
{
    #region Campos y propiedades
    private readonly ClienteServicio _clienteServicio;

    /// <summary>
    /// Cliente creado correctamente desde el formulario.
    /// Permite reutilizar el formulario desde otros procesos,
    /// como el alta de una nueva atención.
    /// </summary>
    public Cliente? ClienteCreado { get; private set; }
    #endregion

    #region Inicialización
    /// <summary>
    /// Inicializa el formulario de alta de clientes.
    /// </summary>
    /// <param name="clienteServicio">
    /// Servicio utilizado para registrar clientes.
    /// </param>
    public FrmAltaCliente(
        ClienteServicio clienteServicio)
    {
        InitializeComponent();

        _clienteServicio =
            clienteServicio
            ?? throw new ArgumentNullException(
                nameof(clienteServicio));

        AplicarEstilos();
    }

    /// <summary>
    /// Prepara el formulario y establece el foco inicial.
    /// </summary>
    private void FrmAltaCliente_Load(
        object? sender,
        EventArgs e)
    {
        ConfigurarFormulario();
        CargarTiposDocumento();
        ActualizarEstadoBotonGuardar();

        cboTipoDocumento.Focus();
    }

    /// <summary>
    /// Aplica al formulario los colores y estilos compartidos
    /// por la interfaz de la aplicación.
    /// </summary>
    private void AplicarEstilos()
    {
        BackColor = EstilosInterfaz.FondoAplicacion;

        pnlEncabezado.BackColor =
            EstilosInterfaz.FondoPanel;

        pnlIdentificacion.BackColor =
            EstilosInterfaz.FondoPanel;

        pnlContacto.BackColor =
            EstilosInterfaz.FondoPanel;

        pnlAcciones.BackColor =
            EstilosInterfaz.FondoPanel;

        EstilosInterfaz.AplicarBotonSecundario(
            btnCancelar);

        EstilosInterfaz.AplicarBotonPrimario(
            btnGuardar);
    }

    /// <summary>
    /// Configura las restricciones y el estado inicial de los
    /// controles utilizados para registrar el cliente.
    /// </summary>
    private void ConfigurarFormulario()
    {
        cboTipoDocumento.DropDownStyle =
            ComboBoxStyle.DropDownList;

        txtDocumento.MaxLength = 20;
        txtNombre.MaxLength = 80;
        txtApellido.MaxLength = 80;
        txtTelefono.MaxLength = 30;
        txtEmail.MaxLength = 100;
        txtCalle.MaxLength = 100;
        txtAltura.MaxLength = 10;
    }

    /// <summary>
    /// Carga los tipos de documento disponibles.
    /// </summary>
    private void CargarTiposDocumento()
    {
        cboTipoDocumento.Items.Clear();

        cboTipoDocumento.Items.Add("DNI");
        cboTipoDocumento.Items.Add("CUIT");
        cboTipoDocumento.Items.Add("CUIL");
        cboTipoDocumento.Items.Add("Pasaporte");

        cboTipoDocumento.SelectedIndex = 0;
    }
    #endregion

    #region Estado del formulario

    /// <summary>
    /// Habilita el registro cuando se completaron los datos obligatorios
    /// de identificación del cliente.
    /// </summary>
    private void ActualizarEstadoBotonGuardar()
    {
        bool tipoDocumentoValido =
            cboTipoDocumento.SelectedItem is not null;

        bool documentoValido =
            !string.IsNullOrWhiteSpace(txtDocumento.Text);

        bool nombreValido =
            !string.IsNullOrWhiteSpace(txtNombre.Text);

        bool apellidoValido =
            !string.IsNullOrWhiteSpace(txtApellido.Text);

        btnGuardar.Enabled =
            tipoDocumentoValido &&
            documentoValido &&
            nombreValido &&
            apellidoValido;
    }

    /// <summary>
    /// Actualiza el estado del botón principal cuando cambia alguno
    /// de los datos obligatorios.
    /// </summary>
    private void CamposObligatorios_TextChanged(
        object? sender,
        EventArgs e)
    {
        ActualizarEstadoBotonGuardar();
    }

    #endregion

    #region Dirección
    /// <summary>
    /// Verifica que la calle y la altura se hayan ingresado conjuntamente
    /// y que la dirección completa respete la longitud permitida.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> cuando el domicilio es válido o está vacío;
    /// de lo contrario, <see langword="false"/>.
    /// </returns>
    private bool ValidarDireccion()
    {
        string calle =
            txtCalle.Text.Trim();

        string altura =
            txtAltura.Text.Trim();

        bool tieneCalle =
            !string.IsNullOrWhiteSpace(calle);

        bool tieneAltura =
            !string.IsNullOrWhiteSpace(altura);

        if (!tieneCalle && !tieneAltura)
            return true;

        if (!tieneCalle)
        {
            MessageBox.Show(
                "Debe ingresar la calle correspondiente a la altura indicada.",
                "Datos del cliente",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            txtCalle.Focus();
            return false;
        }

        if (!tieneAltura)
        {
            MessageBox.Show(
                "Debe ingresar la altura correspondiente a la calle indicada.",
                "Datos del cliente",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            txtAltura.Focus();
            return false;
        }

        string direccionCompleta =
            $"{calle} {altura}".Trim();

        if (direccionCompleta.Length > 120)
        {
            MessageBox.Show(
                "La dirección completa no puede superar los 120 caracteres.",
                "Datos del cliente",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            txtCalle.Focus();
            return false;
        }

        return true;
    }

    /// <summary>
    /// Construye el domicilio completo a partir de la calle y la altura
    /// ingresadas en el formulario.
    /// </summary>
    /// <returns>
    /// Dirección concatenada o <see langword="null"/> si no se
    /// ingresaron datos de domicilio.
    /// </returns>
    private string? ObtenerDireccionCompleta()
    {
        string calle =
            txtCalle.Text.Trim();

        string altura =
            txtAltura.Text.Trim();

        if (string.IsNullOrWhiteSpace(calle) &&
            string.IsNullOrWhiteSpace(altura))
        {
            return null;
        }

        return $"{calle} {altura}".Trim();
    }
    #endregion

    #region Confirmación y cierre
    /// <summary>
    /// Valida los datos ingresados y solicita a la capa de aplicación
    /// el registro del nuevo cliente.
    /// </summary>
    private async void btnGuardar_Click(
        object? sender,
        EventArgs e)
    {
        if (cboTipoDocumento.SelectedItem is null)
        {
            MessageBox.Show(
                "Debe seleccionar un tipo de documento.",
                "Datos incompletos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            cboTipoDocumento.Focus();
            return;
        }

        if (!ValidarDireccion())
            return;

        string tipoDocumento =
            cboTipoDocumento.SelectedItem.ToString()!;

        string? direccion =
            ObtenerDireccionCompleta();

        try
        {
            btnGuardar.Enabled = false;
            btnCancelar.Enabled = false;
            btnGuardar.Text = "Registrando...";

            ClienteCreado =
                await _clienteServicio.RegistrarAsync(
                    tipoDocumento,
                    txtDocumento.Text.Trim(),
                    txtNombre.Text.Trim(),
                    txtApellido.Text.Trim(),
                    txtTelefono.Text.Trim(),
                    txtEmail.Text.Trim(),
                    direccion);

            MessageBox.Show(
                $"Cliente {ClienteCreado.Nombre} " +
                $"{ClienteCreado.Apellido} registrado correctamente.",
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
        finally
        {
            if (!IsDisposed)
            {
                btnGuardar.Text = "Registrar cliente";
                btnCancelar.Enabled = true;
                ActualizarEstadoBotonGuardar();
            }
        }
    }

    /// <summary>
    /// Cancela el alta y cierra el formulario.
    /// </summary>
    private void btnCancelar_Click(
        object? sender,
        EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
    #endregion
}