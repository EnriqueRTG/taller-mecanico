using Taller.Presentacion.Estilos;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;

namespace Taller.Presentacion.Formularios.Atenciones;

/// <summary>
/// Representa la edición visual de los datos administrativos
/// permitidos de una atención.
/// </summary>
/// <remarks>
/// Durante la primera entrega utiliza información simulada y no
/// realiza modificaciones sobre la base de datos.
/// </remarks>
public partial class FrmEditarAtencion : Form
{
    #region Constantes y campos

    private const int LongitudMinimaMotivo = 10;
    private const int LongitudMaximaMotivo = 500;

    private bool _formularioInicializado;
    private bool _hayCambios;
    private bool _cambiosConfirmados;

    private readonly int _idAtencion;
    private readonly AtencionServicio _atencionServicio;

    private Atencion? _atencionActual;
    private bool _guardando;

    #endregion

    #region Propiedades públicas

    /// <summary>
    /// Obtiene el motivo de consulta ingresado.
    /// </summary>
    public string Motivo =>
        txtMotivo.Text.Trim();

    #endregion

    #region Inicialización

    /// <summary>
    /// Inicializa una nueva instancia del formulario.
    /// </summary>
    public FrmEditarAtencion(
        int idAtencion, 
        AtencionServicio atencionServicio)
    {
        ArgumentNullException.ThrowIfNull(atencionServicio);

        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(idAtencion);

        InitializeComponent();

        _idAtencion = idAtencion;
        _atencionServicio = atencionServicio;
    }

    /// <summary>
    /// Configura el formulario y carga los datos reales de la atención.
    /// </summary>
    private async void FrmEditarAtencion_Load(
        object? sender,
        EventArgs e)
    {
        ConfigurarFormulario();
        AplicarEstilos();

        _formularioInicializado = false;
        btnGuardar.Enabled = false;

        try
        {
            Cursor = Cursors.WaitCursor;

            var atencion = await _atencionServicio.ObtenerPorIdAsync(
                _idAtencion);

            if (atencion is null)
            {
                throw new InvalidOperationException(
                    "La atención seleccionada ya no existe.");
            }

            if (atencion.Estado != EstadoAtencion.Abierta
                || atencion.FechaCierre is not null
                || atencion.IdTecnicoEnActividad is not null
                || atencion.ActividadTecnica is not null)
            {
                throw new InvalidOperationException(
                    "Solo pueden editarse atenciones abiertas, " +
                    "antes de confirmar su envío a evaluación técnica.");
            }

            _atencionActual = atencion;

            lblNumeroAtencion.Text =
                $"Atención N.º {atencion.IdAtencion:D6}";

            lblClienteValor.Text =
                $"{atencion.Cliente.Apellido}, {atencion.Cliente.Nombre}";

            lblVehiculoValor.Text =
                $"{atencion.Vehiculo.Modelo.Marca.Nombre} " +
                $"{atencion.Vehiculo.Modelo.Nombre}";

            lblDominioValor.Text = atencion.Vehiculo.Dominio;

            lblFechaIngresoValor.Text =
                atencion.FechaApertura.ToString("dd/MM/yyyy HH:mm");

            txtMotivo.Text = atencion.MotivoConsulta;
            cboTipoSolicitud.SelectedIndex = (int)atencion.TipoSolicitud - 1;

            _hayCambios = false;
            _formularioInicializado = true;
            btnGuardar.Enabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                this,
                ex.Message,
                "Editar atención",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            Close();
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }

    /// <summary>
    /// Coloca el foco inicial en el motivo de consulta.
    /// </summary>
    private void FrmEditarAtencion_Shown(
        object? sender,
        EventArgs e)
    {
        txtMotivo.Focus();
        txtMotivo.SelectionStart =
            txtMotivo.TextLength;
    }

    /// <summary>
    /// Configura el comportamiento general de la ventana.
    /// </summary>
    private void ConfigurarFormulario()
    {
        Text = "Editar atención";
        StartPosition = FormStartPosition.CenterParent;
        ShowInTaskbar = false;

        AcceptButton = btnGuardar;
        CancelButton = btnCancelar;

        txtMotivo.MaxLength =
            LongitudMaximaMotivo;
    }

    #endregion

    #region Eventos

    /// <summary>
    /// Marca que uno de los campos editables fue modificado.
    /// </summary>
    private void CampoEditable_Cambio(
        object? sender,
        EventArgs e)
    {
        if (!_formularioInicializado)
        {
            return;
        }

        _hayCambios = true;

        if (sender is Control control)
        {
            control.BackColor = Color.White;
        }
    }

    /// <summary>
    /// Valida y confirma visualmente los cambios ingresados.
    /// </summary>
    private async void BtnGuardar_Click(
        object? sender,
        EventArgs e)
    {
        if (_guardando
            || _atencionActual is null
            || !ValidarFormulario())
        {
            return;
        }

        try
        {
            _guardando = true;
            btnGuardar.Enabled = false;
            txtMotivo.Enabled = false;
            cboTipoSolicitud.Enabled = false;
            Cursor = Cursors.WaitCursor;

            await _atencionServicio.ModificarAsync(
                _idAtencion,
                _atencionActual.IdCliente,
                _atencionActual.IdVehiculo,
                txtMotivo.Text.Trim(),
                (TipoSolicitudAtencion)(cboTipoSolicitud.SelectedIndex + 1),
                _atencionActual.Version);

            _cambiosConfirmados = true;
            _hayCambios = false;

            MessageBox.Show(
                this,
                "Los datos de la atención fueron guardados.",
                "Editar atención",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            // Permite que FormClosing acepte el cierre.
            _guardando = false;
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                this,
                $"No se pudo guardar la modificación.\n\n{ex.Message}",
                "Editar atención",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        finally
        {
            _guardando = false;

            if (!IsDisposed)
            {
                Cursor = Cursors.Default;
                btnGuardar.Enabled = true;
                txtMotivo.Enabled = true;
                cboTipoSolicitud.Enabled = true;
            }
        }
    }

    /// <summary>
    /// Solicita cerrar el formulario sin confirmar modificaciones.
    /// </summary>
    private void BtnCancelar_Click(
        object? sender,
        EventArgs e)
    {
        Close();
    }

    /// <summary>
    /// Advierte al usuario cuando intenta cerrar la ventana
    /// después de modificar datos sin guardarlos.
    /// </summary>
    private void FrmEditarAtencion_FormClosing(
        object? sender,
        FormClosingEventArgs e)
    {
        if (_guardando)
        {
            e.Cancel = true;
            return;
        }

        if (_cambiosConfirmados || !_hayCambios)
        {
            NormalizarResultadoCancelacion();
            return;
        }

        DialogResult respuesta =
            MessageBox.Show(
                this,
                "Hay cambios que no fueron guardados. " +
                "¿Desea cerrar igualmente?",
                "Editar atención",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

        if (respuesta == DialogResult.No)
        {
            e.Cancel = true;
            DialogResult = DialogResult.None;
            return;
        }

        NormalizarResultadoCancelacion();
    }

    #endregion

    #region Validación

    /// <summary>
    /// Comprueba que los campos obligatorios contengan
    /// información válida para la simulación.
    /// </summary>
    private bool ValidarFormulario()
    {
        txtMotivo.BackColor = Color.White;

        string motivo = txtMotivo.Text.Trim();

        if (string.IsNullOrWhiteSpace(motivo))
        {
            MostrarAdvertencia(
                "Ingresá el motivo de consulta.",
                txtMotivo);

            return false;
        }

        if (motivo.Length > LongitudMaximaMotivo)
        {
            MostrarAdvertencia(
                "El motivo no puede superar los 500 caracteres.",
                txtMotivo);

            return false;
        }

        return true;
    }

    /// <summary>
    /// Restablece la apariencia de los campos validados.
    /// </summary>
    private void RestablecerIndicadoresValidacion()
    {
        txtMotivo.BackColor = Color.White;
    }

    /// <summary>
    /// Muestra una advertencia y coloca el foco en el control indicado.
    /// </summary>
    private void MostrarAdvertencia(
        string mensaje,
        Control control)
    {
        control.BackColor =
            Color.FromArgb(254, 242, 242);

        MessageBox.Show(
            this,
            mensaje,
            "Datos incompletos",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);

        control.Focus();
    }

    #endregion

    #region Simulación y cierre
/// <summary>
    /// Devuelve Cancel cuando el formulario se cierra
    /// sin confirmar una modificación.
    /// </summary>
    private void NormalizarResultadoCancelacion()
    {
        if (!_cambiosConfirmados &&
            DialogResult == DialogResult.None)
        {
            DialogResult = DialogResult.Cancel;
        }
    }

    #endregion

    #region Estilos

    /// <summary>
    /// Aplica los estilos visuales compartidos por la aplicación.
    /// </summary>
    private void AplicarEstilos()
    {
        BackColor =
            EstilosInterfaz.FondoAplicacion;

        EstilosInterfaz.AplicarBotonPrimario(
            btnGuardar);

        EstilosInterfaz.AplicarBotonSecundario(
            btnCancelar);
    }

    #endregion
}