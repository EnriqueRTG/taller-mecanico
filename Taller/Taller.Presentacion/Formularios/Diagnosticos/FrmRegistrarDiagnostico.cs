using System.Globalization;
using Taller.Aplicacion.DTOs.Diagnosticos;
using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Enumeraciones;
using Taller.Presentacion.Estilos;

namespace Taller.Presentacion.Formularios.Diagnosticos;

/// <summary>
/// Permite registrar el diagnóstico inicial de una atención
/// reservada por el técnico autenticado.
/// </summary>
public partial class FrmRegistrarDiagnostico : Form
{
    private readonly int _idAtencion;
    private readonly AtencionServicio? _atencionServicio;
    private readonly DiagnosticoServicio? _diagnosticoServicio;
    private readonly SesionUsuario? _sesionUsuario;

    private bool _cargando;
    private bool _guardando;
    private bool _registroConfirmado;
    private bool _cierreSinConfirmacion;
    private bool _disponible;

    // Permite abrir el diseñador de Windows Forms.
    public FrmRegistrarDiagnostico()
    {
        InitializeComponent();
    }

    public FrmRegistrarDiagnostico(
        int idAtencion,
        AtencionServicio atencionServicio,
        DiagnosticoServicio diagnosticoServicio,
        SesionUsuario sesionUsuario)
        : this()
    {
        if (idAtencion <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idAtencion));
        }

        _idAtencion = idAtencion;
        _atencionServicio = atencionServicio
            ?? throw new ArgumentNullException(nameof(atencionServicio));
        _diagnosticoServicio = diagnosticoServicio
            ?? throw new ArgumentNullException(nameof(diagnosticoServicio));
        _sesionUsuario = sesionUsuario
            ?? throw new ArgumentNullException(nameof(sesionUsuario));
    }

    private async void FrmRegistrarDiagnostico_Load(
        object? sender,
        EventArgs e)
    {
        _cargando = true;
        ActualizarDisponibilidad();

        EstilosInterfaz.AplicarBotonPrimario(btnGuardar);
        EstilosInterfaz.AplicarBotonSecundario(btnCancelar);

        try
        {
            if (_atencionServicio is null
                || _diagnosticoServicio is null
                || _sesionUsuario is null
                || _idAtencion <= 0)
            {
                throw new InvalidOperationException(
                    "El formulario debe abrirse desde una atención seleccionada.");
            }

            var usuario = _sesionUsuario.UsuarioActual;

            if (usuario is null
                || usuario.RolId != RolesSistema.TecnicoId)
            {
                throw new InvalidOperationException(
                    "Debe iniciar sesión como técnico.");
            }

            var atencion = await _atencionServicio.ObtenerPorIdAsync(
                _idAtencion);

            if (atencion is null
                || atencion.Estado != EstadoAtencion.EnEvaluacionTecnica
                || atencion.FechaCierre is not null
                || atencion.ActividadTecnica != ActividadTecnica.Diagnostico
                || atencion.IdTecnicoEnActividad != usuario.Id)
            {
                throw new InvalidOperationException(
                    "La atención no tiene un diagnóstico reservado por usted. " +
                    "Actualice el listado.");
            }

            lblNumeroAtencion.Text =
                $"Atención N.º {atencion.IdAtencion:D6}";

            lblClienteValor.Text =
                $"{atencion.Cliente.Apellido}, {atencion.Cliente.Nombre}";

            lblVehiculoValor.Text =
                $"{atencion.Vehiculo.Modelo.Marca.Nombre} " +
                atencion.Vehiculo.Modelo.Nombre;

            lblDominioValor.Text = atencion.Vehiculo.Dominio;
            lblMotivoValor.Text = atencion.MotivoConsulta;

            txtTecnico.Text = $"{usuario.Nombre} {usuario.Apellido}";
            dtpFechaDiagnostico.Value = DateTime.Now;

            _disponible = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                this,
                ex.Message,
                "Registrar diagnóstico",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            _cierreSinConfirmacion = true;
            Close();
        }
        finally
        {
            _cargando = false;

            if (!IsDisposed)
            {
                ActualizarDisponibilidad();
            }
        }
    }

    private void FrmRegistrarDiagnostico_Shown(
        object? sender,
        EventArgs e)
    {
        if (_disponible && !_cargando)
        {
            txtDescripcion.Focus();
        }
    }

    private async void BtnGuardar_Click(
        object? sender,
        EventArgs e)
    {
        if (_cargando || _guardando || !_disponible
            || _diagnosticoServicio is null)
        {
            return;
        }

        if (!ValidarFormulario(out int? kilometraje))
        {
            return;
        }

        var datos = new RegistrarDiagnosticoInicialDto
        {
            IdAtencion = _idAtencion,
            FallaDetectada = txtDescripcion.Text.Trim(),
            TrabajoRecomendado = txtTrabajoRecomendado.Text.Trim(),
            Kilometraje = kilometraje,
            Observaciones = txtObservaciones.Text.Trim()
        };

        bool guardado = false;

        try
        {
            _guardando = true;
            ActualizarDisponibilidad();
            Cursor = Cursors.WaitCursor;

            await _diagnosticoServicio.RegistrarDiagnosticoInicialAsync(
                datos);

            _registroConfirmado = true;
            guardado = true;
        }
        catch (ValidacionException ex)
        {
            MessageBox.Show(
                this,
                ex.Message,
                "No se pudo registrar",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (ConflictoConcurrenciaException ex)
        {
            _disponible = false;

            MessageBox.Show(
                this,
                ex.Message +
                "\n\nCierre este formulario y actualice el listado.",
                "Información modificada",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            // Evita repetir un envío cuyo resultado no pudo confirmarse.
            _disponible = false;

            MessageBox.Show(
                this,
                "No se pudo confirmar el registro. Cierre el formulario " +
                "y consulte los datos antes de volver a intentarlo.\n\n" +
                ex.Message,
                "Registrar diagnóstico",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            _guardando = false;
            Cursor = Cursors.Default;
            ActualizarDisponibilidad();
        }

        if (guardado)
        {
            MessageBox.Show(
                this,
                "El diagnóstico inicial fue registrado correctamente.",
                "Diagnóstico",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
    }

    private bool ValidarFormulario(
        out int? kilometraje)
    {
        kilometraje = null;

        if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
        {
            MostrarAdvertencia(
                "Ingresá la falla detectada o la conclusión de la evaluación.",
                txtDescripcion);
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtTrabajoRecomendado.Text))
        {
            MostrarAdvertencia(
                "Ingresá el trabajo recomendado o indicá que no se requiere reparación.",
                txtTrabajoRecomendado);
            return false;
        }

        if (txtDescripcion.Text.Trim().Length > 1000
            || txtTrabajoRecomendado.Text.Trim().Length > 1000
            || txtObservaciones.Text.Trim().Length > 1000)
        {
            MostrarAdvertencia(
                "Los textos no pueden superar los 1000 caracteres.",
                txtDescripcion);
            return false;
        }

        string textoKilometraje = txtKilometraje.Text.Trim();

        if (textoKilometraje.Length > 0)
        {
            if (!int.TryParse(
                    textoKilometraje,
                    NumberStyles.None,
                    CultureInfo.InvariantCulture,
                    out int valor))
            {
                MostrarAdvertencia(
                    "El kilometraje debe ser un entero no negativo válido, " +
                    "sin puntos ni comas. También puede dejarlo vacío.",
                    txtKilometraje);
                return false;
            }

            kilometraje = valor;
        }

        return true;
    }

    private void ActualizarDisponibilidad()
    {
        grpDatosDiagnostico.Enabled =
            _disponible && !_cargando && !_guardando;

        btnGuardar.Enabled =
            _disponible && !_cargando && !_guardando;

        btnCancelar.Enabled = !_guardando;
    }

    private void BtnCancelar_Click(
        object? sender, 
        EventArgs e)
    {
        Close();
    }

    private void FrmRegistrarDiagnostico_FormClosing(
        object? sender,
        FormClosingEventArgs e)
    {
        if (_guardando)
        {
            e.Cancel = true;
            return;
        }

        if (_registroConfirmado || _cierreSinConfirmacion)
        {
            return;
        }

        if (HayDatosIngresados())
        {
            var respuesta = MessageBox.Show(
                this,
                "Hay datos sin guardar. ¿Desea cerrar igualmente?",
                "Cerrar diagnóstico",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (respuesta != DialogResult.Yes)
            {
                e.Cancel = true;
                DialogResult = DialogResult.None;
                return;
            }
        }

        DialogResult = DialogResult.Cancel;
    }

    private bool HayDatosIngresados()
    {
        return !string.IsNullOrWhiteSpace(txtDescripcion.Text)
            || !string.IsNullOrWhiteSpace(txtTrabajoRecomendado.Text)
            || !string.IsNullOrWhiteSpace(txtObservaciones.Text)
            || !string.IsNullOrWhiteSpace(txtKilometraje.Text);
    }

    private void CampoValidado_Cambio(
        object? sender, 
        EventArgs e)
    {
        if (sender is TextBox campo)
        {
            campo.BackColor = Color.White;
        }
    }

    private void MostrarAdvertencia(
        string mensaje, 
        Control control)
    {
        MessageBox.Show(
            this,
            mensaje,
            "Datos inválidos",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);

        control.Focus();
    }
}