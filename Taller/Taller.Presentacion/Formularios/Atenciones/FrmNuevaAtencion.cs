using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;
using Taller.Presentacion.Estilos;
using Taller.Presentacion.Formularios.Clientes;
using Taller.Presentacion.Formularios.Vehiculos;

namespace Taller.Presentacion.Formularios.Atenciones;

/// <summary>
/// Permite registrar el ingreso de un vehículo al taller.
///
/// El formulario coordina la búsqueda o creación del cliente y del
/// vehículo, registra el motivo informado y delega la creación de la
/// atención en la capa de aplicación.
/// </summary>
public partial class FrmNuevaAtencion : Form
{
    #region Campos y dependencias
    private readonly ClienteServicio _clienteServicio;
    private readonly VehiculoServicio _vehiculoServicio;
    private readonly AtencionServicio _atencionServicio;
    private readonly SesionUsuario _sesionUsuario;
    private readonly IServiceProvider _serviceProvider;

    private Cliente? _clienteSeleccionado;
    private Vehiculo? _vehiculoSeleccionado;
    #endregion

    #region Inicialización
    /// <summary>
    /// Inicializa una nueva instancia del formulario con los servicios
    /// necesarios para consultar clientes, vehículos y registrar atenciones.
    /// </summary>
    /// <param name="clienteServicio">
    /// Servicio utilizado para consultar clientes.
    /// </param>
    /// <param name="vehiculoServicio">
    /// Servicio utilizado para consultar vehículos.
    /// </param>
    /// <param name="atencionServicio">
    /// Servicio responsable de registrar la atención.
    /// </param>
    /// <param name="sesionUsuario">
    /// Sesión que proporciona el usuario de recepción autenticado.
    /// </param>
    /// <param name="serviceProvider">
    /// Proveedor utilizado para resolver los formularios de altas asociadas.
    /// </param>
    public FrmNuevaAtencion(
        ClienteServicio clienteServicio,
        VehiculoServicio vehiculoServicio,
        AtencionServicio atencionServicio,
        SesionUsuario sesionUsuario,
        IServiceProvider serviceProvider)
    {
        _clienteServicio =
        clienteServicio
        ?? throw new ArgumentNullException(nameof(clienteServicio));

        _vehiculoServicio =
            vehiculoServicio
            ?? throw new ArgumentNullException(nameof(vehiculoServicio));

        _atencionServicio =
            atencionServicio
            ?? throw new ArgumentNullException(nameof(atencionServicio));

        _sesionUsuario =
            sesionUsuario
            ?? throw new ArgumentNullException(nameof(sesionUsuario));

        _serviceProvider =
            serviceProvider
            ?? throw new ArgumentNullException(nameof(serviceProvider));

        InitializeComponent();
        AplicarEstilos();
    }

    /// <summary>
    /// Configura el estado inicial del formulario.
    /// </summary>
    private void FrmNuevaAtencion_Load(
        object? sender,
        EventArgs e)
    {
        ConfigurarFormulario();
        CargarTiposDocumento();
        MostrarUsuarioRecepcion();
        ActualizarEstadoBotonRegistrar();

        cboTipoDocumento.Focus();
    }

    /// <summary>
    /// Inicializa las restricciones, textos informativos y estados habilitados
    /// de los controles utilizados para registrar una atención.
    /// </summary>
    private void ConfigurarFormulario()
    {
        txtDocumento.MaxLength = 20;

        txtDominio.MaxLength = 10;
        txtDominio.CharacterCasing = CharacterCasing.Upper;

        txtMotivo.MaxLength = 500;

        _clienteSeleccionado = null;
        _vehiculoSeleccionado = null;

        btnNuevoCliente.Enabled = true;

        txtDominio.Enabled = false;
        btnBuscarVehiculo.Enabled = false;
        btnNuevoVehiculo.Enabled = false;

        lblClienteSeleccionado.Text =
            "Todavía no seleccionó un cliente.";

        lblClienteSeleccionado.ForeColor =
            Color.FromArgb(100, 116, 139);

        lblVehiculoSeleccionado.Text =
            "Primero seleccione un cliente.";

        lblVehiculoSeleccionado.ForeColor =
            Color.FromArgb(100, 116, 139);

        lblContadorMotivo.Text = "0 / 500";

        lblFechaIngreso.Text =
            $"Fecha de ingreso: {DateTime.Now:dd/MM/yyyy HH:mm}";
    }

    /// <summary>
    /// Aplica la configuración visual compartida por la aplicación.
    /// </summary>
    private void AplicarEstilos()
    {
        BackColor = EstilosInterfaz.FondoAplicacion;

        pnlEncabezado.BackColor = EstilosInterfaz.FondoPanel;
        pnlRecepcion.BackColor = EstilosInterfaz.FondoPanel;
        pnlCliente.BackColor = EstilosInterfaz.FondoPanel;
        pnlVehiculo.BackColor = EstilosInterfaz.FondoPanel;
        pnlMotivo.BackColor = EstilosInterfaz.FondoPanel;
        pnlAcciones.BackColor = EstilosInterfaz.FondoPanel;

        EstilosInterfaz.AplicarBotonSecundario(btnBuscarCliente);
        EstilosInterfaz.AplicarBotonSecundario(btnNuevoCliente);
        EstilosInterfaz.AplicarBotonSecundario(btnBuscarVehiculo);
        EstilosInterfaz.AplicarBotonSecundario(btnNuevoVehiculo);
        EstilosInterfaz.AplicarBotonSecundario(btnCancelar);
        EstilosInterfaz.AplicarBotonPrimario(btnRegistrar);
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

    /// <summary>
    /// Muestra el usuario actualmente autenticado.
    /// </summary>
    private void MostrarUsuarioRecepcion()
    {
        var usuario = _sesionUsuario.UsuarioActual;

        if (usuario is null)
        {
            lblUsuarioRecepcion.Text =
                "Usuario de recepción: no autenticado";

            lblUsuarioRecepcion.ForeColor =
                Color.FromArgb(185, 28, 28);

            return;
        }

        string nombreRol =
            usuario.Rol?.Nombre ?? "Rol no disponible";

        lblUsuarioRecepcion.Text =
            $"Usuario de recepción: {usuario.Nombre} " +
            $"{usuario.Apellido} · {nombreRol}";

        lblUsuarioRecepcion.ForeColor =
            Color.FromArgb(15, 23, 42);
    }
    #endregion

    #region Estado del formulario
    /// <summary>
    /// Habilita el registro únicamente cuando la atención contiene
    /// todos los datos obligatorios y existe un usuario autenticado.
    /// </summary>
    private void ActualizarEstadoBotonRegistrar()
    {
        bool usuarioValido =
            _sesionUsuario.UsuarioActual is not null;

        bool clienteValido =
            _clienteSeleccionado is not null;

        bool vehiculoValido =
            _vehiculoSeleccionado is not null;

        bool motivoValido =
            !string.IsNullOrWhiteSpace(txtMotivo.Text);

        btnRegistrar.Enabled =
            usuarioValido &&
            clienteValido &&
            vehiculoValido &&
            motivoValido;
    }

    /// <summary>
    /// Establece el cliente de la atención y reinicia la selección del
    /// vehículo para impedir que permanezca asociado al cliente anterior.
    /// </summary>
    /// <param name="cliente">Cliente seleccionado.</param>
    private void SeleccionarCliente(
        Cliente cliente)
    {
        _clienteSeleccionado = cliente;

        lblClienteSeleccionado.Text =
            $"{cliente.Apellido}, {cliente.Nombre} · " +
            $"{cliente.TipoDocumento} {cliente.Documento}";

        lblClienteSeleccionado.ForeColor =
            Color.FromArgb(15, 23, 42);

        btnNuevoCliente.Enabled = true;

        // Al cambiar de cliente no debe conservarse
        // un vehículo seleccionado anteriormente.
        LimpiarVehiculoSeleccionado();

        txtDominio.Enabled = true;
        btnBuscarVehiculo.Enabled = true;
        btnNuevoVehiculo.Enabled = true;

        ActualizarEstadoBotonRegistrar();

        txtDominio.Focus();
    }

    /// <summary>
    /// Elimina la selección actual del cliente y, en consecuencia,
    /// cualquier vehículo asociado al flujo en curso.
    /// </summary>
    private void LimpiarClienteSeleccionado()
    {
        _clienteSeleccionado = null;

        lblClienteSeleccionado.Text =
            "Debe buscar nuevamente el cliente.";

        lblClienteSeleccionado.ForeColor =
            Color.FromArgb(100, 116, 139);

        txtDominio.Enabled = false;
        btnBuscarVehiculo.Enabled = false;
        btnNuevoVehiculo.Enabled = false;

        LimpiarVehiculoSeleccionado();
        ActualizarEstadoBotonRegistrar();
    }

    /// <summary>
    /// Establece el vehículo seleccionado y actualiza su representación
    /// visual en el formulario.
    /// </summary>
    /// <param name="vehiculo">Vehículo seleccionado.</param>
    private void SeleccionarVehiculo(
        Vehiculo vehiculo)
    {
        _vehiculoSeleccionado = vehiculo;

        txtDominio.Text = vehiculo.Dominio;

        lblVehiculoSeleccionado.Text =
            $"{vehiculo.Dominio} · " +
            $"{vehiculo.Modelo.Marca.Nombre} " +
            $"{vehiculo.Modelo.Nombre} · " +
            $"Modelo {vehiculo.Anio}";

        lblVehiculoSeleccionado.ForeColor =
            Color.FromArgb(15, 23, 42);

        btnNuevoVehiculo.Enabled = true;

        ActualizarEstadoBotonRegistrar();

        txtMotivo.Focus();
    }

    /// <summary>
    /// Elimina el vehículo seleccionado y restablece la sección según exista
    /// o no un cliente previamente seleccionado.
    /// </summary>
    private void LimpiarVehiculoSeleccionado()
    {
        _vehiculoSeleccionado = null;

        txtDominio.Clear();

        lblVehiculoSeleccionado.Text =
            _clienteSeleccionado is null
                ? "Primero seleccione un cliente."
                : "Todavía no seleccionó un vehículo.";

        lblVehiculoSeleccionado.ForeColor =
            Color.FromArgb(100, 116, 139);

        ActualizarEstadoBotonRegistrar();
    }
    #endregion

    #region Eventos de cliente
    /// <summary>
    /// Invalida la selección cuando cambia el tipo de documento utilizado
    /// para identificar al cliente.
    /// </summary>
    private void cboTipoDocumento_SelectedIndexChanged(
        object? sender,
        EventArgs e)
    {
        if (_clienteSeleccionado is null)
            return;

        string tipoSeleccionado =
            cboTipoDocumento.SelectedItem?.ToString() ?? string.Empty;

        bool coincideTipo =
            string.Equals(
                tipoSeleccionado,
                _clienteSeleccionado.TipoDocumento,
                StringComparison.OrdinalIgnoreCase);

        if (coincideTipo)
            return;

        LimpiarClienteSeleccionado();
    }

    /// <summary>
    /// Invalida la selección cuando el documento ingresado deja de
    /// coincidir con el cliente previamente seleccionado.
    /// </summary>
    private void txtDocumento_TextChanged(
        object? sender,
        EventArgs e)
    {
        if (_clienteSeleccionado is null)
            return;

        bool coincideDocumento =
            string.Equals(
                txtDocumento.Text.Trim(),
                _clienteSeleccionado.Documento,
                StringComparison.OrdinalIgnoreCase);

        if (coincideDocumento)
            return;

        LimpiarClienteSeleccionado();
    }

    /// <summary>
    /// Busca un cliente activo por tipo y número de documento y lo establece
    /// como cliente de la atención cuando la consulta resulta exitosa.
    /// </summary>
    private async void btnBuscarCliente_Click(
        object? sender,
        EventArgs e)
    {
        try
        {
            if (cboTipoDocumento.SelectedItem is null)
            {
                MessageBox.Show(
                    "Debe seleccionar un tipo de documento.",
                    "Nueva atención",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboTipoDocumento.Focus();
                return;
            }

            string tipoDocumento =
                cboTipoDocumento.SelectedItem.ToString()!;

            string documento =
                txtDocumento.Text.Trim();

            if (string.IsNullOrWhiteSpace(documento))
            {
                MessageBox.Show(
                    "Debe ingresar el número de documento.",
                    "Nueva atención",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtDocumento.Focus();
                return;
            }

            btnBuscarCliente.Enabled = false;

            Cliente? cliente =
                await _clienteServicio.ObtenerPorDocumentoAsync(
                    tipoDocumento,
                    documento);

            if (cliente is null)
            {
                _clienteSeleccionado = null;

                lblClienteSeleccionado.Text =
                    "Cliente no encontrado. Puede registrarlo desde esta pantalla.";

                lblClienteSeleccionado.ForeColor =
                    Color.FromArgb(185, 28, 28);

                txtDominio.Enabled = false;
                btnBuscarVehiculo.Enabled = false;
                btnNuevoVehiculo.Enabled = false;

                LimpiarVehiculoSeleccionado();

                btnNuevoCliente.Enabled = true;
                return;
            }

            if (!cliente.Activo)
            {
                _clienteSeleccionado = null;

                lblClienteSeleccionado.Text =
                    "El cliente se encuentra inactivo.";

                lblClienteSeleccionado.ForeColor =
                    Color.FromArgb(185, 28, 28);

                txtDominio.Enabled = false;
                btnBuscarVehiculo.Enabled = false;
                btnNuevoVehiculo.Enabled = false;

                LimpiarVehiculoSeleccionado();

                btnNuevoCliente.Enabled = true;
                return;
            }

            SeleccionarCliente(cliente);
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
        finally
        {
            btnBuscarCliente.Enabled = true;
        }
    }

    /// <summary>
    /// Abre el formulario de alta de clientes y selecciona el cliente
    /// creado cuando la operación finaliza correctamente.
    /// </summary>
    private void btnNuevoCliente_Click(
        object? sender,
        EventArgs e)
    {
        try
        {
            using var formulario =
                _serviceProvider.GetRequiredService<FrmAltaCliente>();

            if (formulario.ShowDialog(this) != DialogResult.OK)
                return;

            if (formulario.ClienteCreado is null)
                return;

            Cliente cliente = formulario.ClienteCreado;

            cboTipoDocumento.SelectedItem =
                cliente.TipoDocumento;

            txtDocumento.Text =
                cliente.Documento;

            SeleccionarCliente(cliente);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Ocurrió un error al abrir o recuperar el cliente.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
    #endregion

    #region Eventos de vehículo
    /// <summary>
    /// Invalida la selección cuando el dominio escrito deja de coincidir
    /// con el vehículo previamente seleccionado.
    /// </summary>
    private void txtDominio_TextChanged(
        object? sender,
        EventArgs e)
    {
        if (_vehiculoSeleccionado is null)
            return;

        bool coincideDominio =
            string.Equals(
                txtDominio.Text.Trim(),
                _vehiculoSeleccionado.Dominio,
                StringComparison.OrdinalIgnoreCase);

        if (coincideDominio)
            return;

        _vehiculoSeleccionado = null;

        lblVehiculoSeleccionado.Text =
            "Debe buscar nuevamente el vehículo.";

        lblVehiculoSeleccionado.ForeColor =
            Color.FromArgb(100, 116, 139);

        ActualizarEstadoBotonRegistrar();
    }

    /// <summary>
    /// Busca un vehículo activo por dominio y lo establece como vehículo
    /// de la atención cuando la consulta resulta exitosa.
    /// </summary>
    private async void btnBuscarVehiculo_Click(
        object? sender,
        EventArgs e)
    {
        try
        {
            if (_clienteSeleccionado is null)
            {
                MessageBox.Show(
                    "Primero debe seleccionar un cliente.",
                    "Nueva atención",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtDocumento.Focus();
                return;
            }

            string dominio = txtDominio.Text.Trim();

            if (string.IsNullOrWhiteSpace(dominio))
            {
                MessageBox.Show(
                    "Debe ingresar el dominio del vehículo.",
                    "Nueva atención",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtDominio.Focus();
                return;
            }

            btnBuscarVehiculo.Enabled = false;

            Vehiculo? vehiculo =
                await _vehiculoServicio.ObtenerPorDominioAsync(dominio);

            if (vehiculo is null)
            {
                _vehiculoSeleccionado = null;

                lblVehiculoSeleccionado.Text =
                    "Vehículo no encontrado. Puede registrarlo desde esta pantalla.";

                lblVehiculoSeleccionado.ForeColor =
                    Color.FromArgb(185, 28, 28);

                btnNuevoVehiculo.Enabled = true;

                ActualizarEstadoBotonRegistrar();
                return;
            }

            if (!vehiculo.Activo)
            {
                _vehiculoSeleccionado = null;

                lblVehiculoSeleccionado.Text =
                    "El vehículo se encuentra inactivo.";

                lblVehiculoSeleccionado.ForeColor =
                    Color.FromArgb(185, 28, 28);

                btnNuevoVehiculo.Enabled = false;

                ActualizarEstadoBotonRegistrar();
                return;
            }

            SeleccionarVehiculo(vehiculo);
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
        finally
        {
            btnBuscarVehiculo.Enabled =
                _clienteSeleccionado is not null;
        }
    }

    /// <summary>
    /// Abre el formulario de alta de vehículos y selecciona el vehículo
    /// creado cuando la operación finaliza correctamente.
    /// </summary>
    private async void btnNuevoVehiculo_Click(
        object? sender,
        EventArgs e)
    {
        if (_clienteSeleccionado is null)
        {
            MessageBox.Show(
                "Primero debe seleccionar un cliente.",
                "Nueva atención",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            txtDocumento.Focus();
            return;
        }

        try
        {
            using var formulario =
                _serviceProvider.GetRequiredService<FrmAltaVehiculo>();

            if (formulario.ShowDialog(this) != DialogResult.OK)
                return;

            if (formulario.VehiculoCreado is null)
                return;

            Vehiculo? vehiculo =
                await _vehiculoServicio.ObtenerPorIdAsync(
                    formulario.VehiculoCreado.IdVehiculo);

            if (vehiculo is null)
            {
                MessageBox.Show(
                    "El vehículo fue registrado, pero no pudo recuperarse.",
                    "Nueva atención",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            SeleccionarVehiculo(vehiculo);
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
                $"Ocurrió un error al recuperar el vehículo.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
    #endregion

    #region Motivo de consulta
    /// <summary>
    /// Actualiza el contador de caracteres del motivo y vuelve a evaluar
    /// si la atención contiene todos los datos necesarios para registrarse.
    /// </summary>
    private void txtMotivo_TextChanged(
        object? sender, 
        EventArgs e)
    {
        lblContadorMotivo.Text = $"{txtMotivo.TextLength} / 500";
        ActualizarEstadoBotonRegistrar();
    }
    #endregion

    #region Confirmación y cierre
    /// <summary>
    /// Registra la nueva atención utilizando el cliente,
    /// vehículo y usuario de recepción seleccionados.
    /// </summary>
    private async void btnRegistrar_Click(
        object? sender,
        EventArgs e)
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

        if (string.IsNullOrWhiteSpace(txtMotivo.Text))
        {
            MessageBox.Show(
                "Debe ingresar el motivo de la consulta.",
                "Nueva atención",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            txtMotivo.Focus();
            return;
        }

        var usuario = _sesionUsuario.UsuarioActual;

        if (usuario is null)
        {
            MessageBox.Show(
                "No existe un usuario autenticado.",
                "Sesión",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            return;
        }

        try
        {
            btnRegistrar.Enabled = false;
            btnCancelar.Enabled = false;
            btnRegistrar.Text = "Registrando...";

            var atencion =
                await _atencionServicio.RegistrarAsync(
                    _clienteSeleccionado.IdCliente,
                    _vehiculoSeleccionado.IdVehiculo,
                    usuario.Id,
                    txtMotivo.Text.Trim());

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
        finally
        {
            if (!IsDisposed)
            {
                btnRegistrar.Text = "Registrar atención";
                btnCancelar.Enabled = true;
                ActualizarEstadoBotonRegistrar();
            }
        }
    }

    /// <summary>
    /// Cancela el registro y cierra el formulario sin guardar cambios.
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