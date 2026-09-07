using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;
using Taller.Presentacion.Formularios.Clientes;
using Taller.Presentacion.Formularios.Vehiculos;

namespace Taller.Presentacion.Formularios.Atenciones;

/// <summary>
/// Formulario utilizado para registrar una nueva atención,
/// seleccionando o registrando previamente un cliente y un vehículo.
/// </summary>
public partial class FrmNuevaAtencion : Form
{
    private readonly ClienteServicio _clienteServicio;
    private readonly VehiculoServicio _vehiculoServicio;
    private readonly AtencionServicio _atencionServicio;
    private readonly SesionUsuario _sesionUsuario;
    private readonly IServiceProvider _serviceProvider;

    private Cliente? _clienteSeleccionado;
    private Vehiculo? _vehiculoSeleccionado;

    /// <summary>
    /// Inicializa el formulario con los servicios necesarios.
    /// </summary>
    public FrmNuevaAtencion(
        ClienteServicio clienteServicio,
        VehiculoServicio vehiculoServicio,
        AtencionServicio atencionServicio,
        SesionUsuario sesionUsuario,
        IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _clienteServicio = clienteServicio;
        _vehiculoServicio = vehiculoServicio;
        _atencionServicio = atencionServicio;
        _sesionUsuario = sesionUsuario;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Configura el estado inicial del formulario.
    /// </summary>
    private void FrmNuevaAtencion_Load(object sender, EventArgs e)
    {
        CargarTiposDocumento();

        txtDocumentoCliente.MaxLength = 20;
        txtDominio.MaxLength = 10;
        txtMotivoConsulta.MaxLength = 500;

        txtDominio.CharacterCasing = CharacterCasing.Upper;

        btnNuevoCliente.Enabled = false;
        btnNuevoVehiculo.Enabled = false;

        lblClienteSeleccionado.Text =
            "Ningún cliente seleccionado.";

        lblVehiculoSeleccionado.Text =
            "Ningún vehículo seleccionado.";

        MostrarUsuarioRecepcion();
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
    /// Muestra el usuario actualmente autenticado.
    /// </summary>
    private void MostrarUsuarioRecepcion()
    {
        var usuario = _sesionUsuario.UsuarioActual;

        if (usuario is null)
        {
            lblUsuarioRecepcion.Text =
                "Usuario no autenticado.";

            return;
        }

        lblUsuarioRecepcion.Text =
            $"{usuario.Nombre} {usuario.Apellido} - {usuario.Rol.Nombre}";
    }

    /// <summary>
    /// Busca un cliente por tipo y número de documento.
    /// </summary>
    private async void btnBuscarCliente_Click(
        object sender,
        EventArgs e)
    {
        try
        {
            if (cmbTipoDocumento.SelectedItem is null)
            {
                MessageBox.Show(
                    "Debe seleccionar un tipo de documento.",
                    "Nueva atención",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbTipoDocumento.Focus();
                return;
            }

            var tipoDocumento =
                cmbTipoDocumento.SelectedItem.ToString()!;

            var documento =
                txtDocumentoCliente.Text.Trim();

            var cliente =
                await _clienteServicio.ObtenerPorDocumentoAsync(
                    tipoDocumento,
                    documento);

            if (cliente is null)
            {
                _clienteSeleccionado = null;

                lblClienteSeleccionado.Text =
                    "Cliente no encontrado.";

                btnNuevoCliente.Enabled = true;

                return;
            }

            if (!cliente.Activo)
            {
                _clienteSeleccionado = null;

                lblClienteSeleccionado.Text =
                    "El cliente se encuentra inactivo.";

                btnNuevoCliente.Enabled = false;

                return;
            }

            _clienteSeleccionado = cliente;

            lblClienteSeleccionado.Text =
                $"{cliente.Apellido}, {cliente.Nombre} - " +
                $"{cliente.TipoDocumento} {cliente.Documento}";

            btnNuevoCliente.Enabled = false;
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Datos inválidos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ocurrió un error al buscar el cliente.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Abre el formulario de alta de cliente.
    /// </summary>
    private void btnNuevoCliente_Click(
        object sender,
        EventArgs e)
    {
        using var formulario =
            _serviceProvider.GetRequiredService<FrmAltaCliente>();

        if (formulario.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        if (formulario.ClienteCreado is null)
        {
            return;
        }

        _clienteSeleccionado =
            formulario.ClienteCreado;

        cmbTipoDocumento.SelectedItem =
            _clienteSeleccionado.TipoDocumento;

        txtDocumentoCliente.Text =
            _clienteSeleccionado.Documento;

        lblClienteSeleccionado.Text =
            $"{_clienteSeleccionado.Apellido}, " +
            $"{_clienteSeleccionado.Nombre} - " +
            $"{_clienteSeleccionado.TipoDocumento} " +
            $"{_clienteSeleccionado.Documento}";

        btnNuevoCliente.Enabled = false;
    }

    /// <summary>
    /// Busca un vehículo por dominio.
    /// </summary>
    private async void btnBuscarVehiculo_Click(
        object sender,
        EventArgs e)
    {
        try
        {
            var dominio =
                txtDominio.Text.Trim();

            var vehiculo =
                await _vehiculoServicio
                    .ObtenerPorDominioAsync(dominio);

            if (vehiculo is null)
            {
                _vehiculoSeleccionado = null;

                lblVehiculoSeleccionado.Text =
                    "Vehículo no encontrado.";

                btnNuevoVehiculo.Enabled = true;

                return;
            }

            if (!vehiculo.Activo)
            {
                _vehiculoSeleccionado = null;

                lblVehiculoSeleccionado.Text =
                    "El vehículo se encuentra inactivo.";

                btnNuevoVehiculo.Enabled = false;

                return;
            }

            _vehiculoSeleccionado = vehiculo;

            lblVehiculoSeleccionado.Text =
                $"{vehiculo.Modelo.Marca.Nombre} " +
                $"{vehiculo.Modelo.Nombre} - " +
                $"{vehiculo.Dominio}";

            btnNuevoVehiculo.Enabled = false;
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(
                ex.Message,
                "Datos inválidos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ocurrió un error al buscar el vehículo.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Abre el formulario de alta de vehículo.
    /// </summary>
    private async void btnNuevoVehiculo_Click(
        object sender,
        EventArgs e)
    {
        using var formulario =
            _serviceProvider.GetRequiredService<FrmAltaVehiculo>();

        if (formulario.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        if (formulario.VehiculoCreado is null)
        {
            return;
        }

        _vehiculoSeleccionado =
            await _vehiculoServicio.ObtenerPorIdAsync(
                formulario.VehiculoCreado.IdVehiculo);

        if (_vehiculoSeleccionado is null)
        {
            MessageBox.Show(
                "El vehículo fue registrado, pero no pudo recuperarse.",
                "Nueva atención",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        txtDominio.Text =
            _vehiculoSeleccionado.Dominio;

        lblVehiculoSeleccionado.Text =
            $"{_vehiculoSeleccionado.Modelo.Marca.Nombre} " +
            $"{_vehiculoSeleccionado.Modelo.Nombre} - " +
            $"{_vehiculoSeleccionado.Dominio}";

        btnNuevoVehiculo.Enabled = false;
    }

    /// <summary>
    /// Registra la nueva atención utilizando el cliente,
    /// vehículo y usuario de recepción seleccionados.
    /// </summary>
    private async void btnRegistrar_Click(
        object sender,
        EventArgs e)
    {
        try
        {
            if (_clienteSeleccionado is null)
            {
                MessageBox.Show(
                    "Debe seleccionar un cliente.",
                    "Nueva atención",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (_vehiculoSeleccionado is null)
            {
                MessageBox.Show(
                    "Debe seleccionar un vehículo.",
                    "Nueva atención",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            var usuario =
                _sesionUsuario.UsuarioActual;

            if (usuario is null)
            {
                MessageBox.Show(
                    "No existe un usuario autenticado.",
                    "Sesión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            var atencion =
                await _atencionServicio.RegistrarAsync(
                    _clienteSeleccionado.IdCliente,
                    _vehiculoSeleccionado.IdVehiculo,
                    usuario.Id,
                    txtMotivoConsulta.Text);

            MessageBox.Show(
                $"Atención N.º {atencion.IdAtencion} registrada correctamente.",
                "Nueva atención",
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
                "No se pudo registrar la atención",
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
    /// Cancela el registro y cierra el formulario.
    /// </summary>
    private void btnCancelar_Click(
        object sender,
        EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}