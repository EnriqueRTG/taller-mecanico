using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;
using Taller.Presentacion.Estilos;

namespace Taller.Presentacion.Formularios.Vehiculos;
public partial class FrmAltaVehiculo : Form
{
    private bool _guardando;
    private void Guardado_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if ((_guardando || _cargandoDatos) && DialogResult != DialogResult.OK)
            e.Cancel = true;
    }

#region Campos y propiedades
    private readonly VehiculoServicio _vehiculoServicio;
    /*
     * Impide que los eventos de selección se procesen mientras
     * se están enlazando marcas o modelos a los ComboBox.
     */
    private bool _cargandoDatos;
    /// <summary>
    /// Vehículo creado correctamente desde el formulario.
    /// Permite recuperar el resultado desde otros procesos,
    /// como el alta de una nueva atención.
    /// </summary>
    public Vehiculo? VehiculoCreado { get; private set; }

#endregion
#region Inicialización
    /// <summary>
    /// Inicializa el formulario de alta de vehículos.
    /// </summary>
    /// <param name = "vehiculoServicio">
    /// Servicio utilizado para registrar el vehículo.
    /// </param>
    public FrmAltaVehiculo(VehiculoServicio vehiculoServicio)
    {
        InitializeComponent();
        _vehiculoServicio = vehiculoServicio ?? throw new ArgumentNullException(nameof(vehiculoServicio));
        AplicarEstilos();
    }

    /// <summary>
    /// Prepara el formulario, carga las marcas disponibles y
    /// establece el foco en el primer campo.
    /// </summary>
    private async void FrmAltaVehiculo_Load(object? sender, EventArgs e)
    {
        try
        {
            ConfigurarFormulario();
            await CargarMarcasAsync();
            txtDominio.Focus();
        }
        catch (Exception ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Alta de vehículo");
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }

    /// <summary>
    /// Aplica al formulario los colores y estilos compartidos
    /// por la interfaz de la aplicación.
    /// </summary>
    private void AplicarEstilos()
    {
        BackColor = EstilosInterfaz.FondoAplicacion;
        pnlEncabezado.BackColor = EstilosInterfaz.FondoPanel;
        pnlIdentificacion.BackColor = EstilosInterfaz.FondoPanel;
        pnlClasificacion.BackColor = EstilosInterfaz.FondoPanel;
        pnlAcciones.BackColor = EstilosInterfaz.FondoPanel;
        EstilosInterfaz.AplicarBotonSecundario(btnCancelar);
        EstilosInterfaz.AplicarBotonPrimario(btnGuardar);
    }

    /// <summary>
    /// Configura las restricciones, formatos y estado inicial
    /// de los controles del formulario.
    /// </summary>
    private void ConfigurarFormulario()
    {
        txtDominio.MaxLength = 10;
        txtDominio.CharacterCasing = CharacterCasing.Upper;
        txtColor.MaxLength = 50;
        nudAnio.Minimum = 1900;
        nudAnio.Maximum = DateTime.Today.Year + 1;
        nudAnio.Value = DateTime.Today.Year;
        cboMarca.DropDownStyle = ComboBoxStyle.DropDownList;
        cboModelo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboModelo.DataSource = null;
        cboModelo.Enabled = false;
        VehiculoCreado = null;
        ActualizarEstadoBotonGuardar();
    }

#endregion
#region Carga de marcas y modelos
    /// <summary>
    /// Obtiene las marcas activas y las muestra en el selector
    /// correspondiente.
    /// </summary>
    private async Task CargarMarcasAsync()
    {
        _cargandoDatos = true;
        try
        {
            var marcas = await _vehiculoServicio.ListarMarcasAsync();
            cboMarca.DataSource = null;
            cboMarca.DisplayMember = nameof(Marca.Nombre);
            cboMarca.ValueMember = nameof(Marca.IdMarca);
            cboMarca.DataSource = marcas;
            cboMarca.SelectedIndex = -1;
            LimpiarModelos();
        }
        finally
        {
            _cargandoDatos = false;
            ActualizarEstadoBotonGuardar();
        }
    }

    /// <summary>
    /// Carga los modelos activos correspondientes a la marca
    /// seleccionada.
    /// </summary>
    /// <param name = "idMarca">
    /// Identificador de la marca seleccionada.
    /// </param>
    private async Task CargarModelosAsync(int idMarca)
    {
        _cargandoDatos = true;
        try
        {
            LimpiarModelos();
            var modelos = await _vehiculoServicio.ListarModelosPorMarcaAsync(idMarca);
            cboModelo.DisplayMember = nameof(Modelo.Nombre);
            cboModelo.ValueMember = nameof(Modelo.IdModelo);
            cboModelo.DataSource = modelos;
            cboModelo.SelectedIndex = -1;
            cboModelo.Enabled = modelos.Count > 0;
        }
        finally
        {
            _cargandoDatos = false;
            ActualizarEstadoBotonGuardar();
        }
    }

    /// <summary>
    /// Elimina la selección y los datos del selector de modelos.
    /// </summary>
    private void LimpiarModelos()
    {
        cboModelo.DataSource = null;
        cboModelo.Items.Clear();
        cboModelo.SelectedIndex = -1;
        cboModelo.Enabled = false;
        ActualizarEstadoBotonGuardar();
    }

    /// <summary>
    /// Actualiza los modelos cuando el usuario selecciona otra marca.
    /// </summary>
    private async void cboMarca_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_cargandoDatos)
            return;
        if (cboMarca.SelectedValue is not int idMarca)
        {
            LimpiarModelos();
            return;
        }

        try
        {
            await CargarModelosAsync(idMarca);
        }
        catch (Exception ex)
        {
            LimpiarModelos();
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Alta de vehículo");
        }
    }

#endregion
#region Estado del formulario
    /// <summary>
    /// Determina si se ingresaron los datos mínimos necesarios
    /// para registrar el vehículo.
    /// </summary>
    private bool FormularioValido()
    {
        return !string.IsNullOrWhiteSpace(txtDominio.Text) && !string.IsNullOrWhiteSpace(txtColor.Text) && cboMarca.SelectedValue is int && cboModelo.SelectedValue is int && !_cargandoDatos;
    }

    /// <summary>
    /// Actualiza la disponibilidad del botón de confirmación
    /// según el estado de los campos obligatorios.
    /// </summary>
    private void ActualizarEstadoBotonGuardar()
    {
        btnGuardar.Enabled = FormularioValido();
    }

    /// <summary>
    /// Responde a los cambios realizados en los campos obligatorios.
    /// </summary>
    private void CampoObligatorio_Cambio(object? sender, EventArgs e)
    {
        ActualizarEstadoBotonGuardar();
    }

    /// <summary>
    /// Habilita o deshabilita los controles mientras se realiza
    /// el registro del vehículo.
    /// </summary>
    private void EstablecerEstadoRegistro(bool registrando)
    {
        txtDominio.Enabled = !registrando;
        txtColor.Enabled = !registrando;
        nudAnio.Enabled = !registrando;
        cboMarca.Enabled = !registrando;
        cboModelo.Enabled = !registrando && cboModelo.DataSource is not null;
        btnCancelar.Enabled = !registrando;
        btnGuardar.Text = registrando ? "Registrando..." : "Registrar vehículo";
        btnGuardar.Enabled = !registrando && FormularioValido();
        UseWaitCursor = registrando;
    }

#endregion
#region Confirmación y cierre
    /// <summary>
    /// Valida los datos ingresados y registra el vehículo.
    /// </summary>
    private async void btnGuardar_Click(object? sender, EventArgs e)
    {
        if (_guardando)
            return;
        if (!FormularioValido())
        {
            MessageBox.Show("Complete todos los campos obligatorios.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (cboMarca.SelectedValue is not int)
        {
            MessageBox.Show("Debe seleccionar una marca.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cboMarca.Focus();
            return;
        }

        if (cboModelo.SelectedValue is not int idModelo)
        {
            MessageBox.Show("Debe seleccionar un modelo.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cboModelo.Focus();
            return;
        }

        try
        {
            _guardando = true;
            EstablecerEstadoRegistro(true);
            VehiculoCreado = await _vehiculoServicio.RegistrarAsync(txtDominio.Text.Trim(), decimal.ToInt32(nudAnio.Value), txtColor.Text.Trim(), idModelo);
            MessageBox.Show($"Vehículo {VehiculoCreado.Dominio} " + "registrado correctamente.", "Alta de vehículo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (ArgumentException ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Datos inválidos");
        }
        catch (InvalidOperationException ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "No se pudo registrar");
        }
        catch (Exception ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Error");
        }
        finally
        {
            _guardando = false;
            if (!IsDisposed)
                EstablecerEstadoRegistro(false);
        }
    }

    /// <summary>
    /// Cancela el registro del vehículo y cierra el formulario.
    /// </summary>
    private void btnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
#endregion
}
