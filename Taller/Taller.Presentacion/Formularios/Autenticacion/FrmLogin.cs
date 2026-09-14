using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;

namespace Taller.Presentacion.Formularios.Autenticacion;

/// <summary>
/// Representa el formulario de acceso a la aplicación.
/// Recopila las credenciales ingresadas por el usuario,
/// solicita su validación al servicio de autenticación e
/// inicia la sesión cuando el acceso resulta satisfactorio.
/// </summary>
/// <remarks>
/// Este formulario solamente realiza validaciones de interfaz,
/// controla el estado visual de sus componentes y presenta
/// mensajes al usuario.
///
/// Las reglas de autenticación y las validaciones indispensables
/// deben permanecer en la capa de aplicación, dentro de
/// <see cref="AutenticacionServicio"/>.
/// </remarks>
public partial class FrmLogin : Form
{
    private readonly AutenticacionServicio _autenticacionServicio;
    private readonly SesionUsuario _sesionUsuario;

    /// <summary>
    /// Inicializa una nueva instancia del formulario de acceso.
    /// </summary>
    /// <param name="autenticacionServicio">
    /// Servicio de aplicación encargado de validar las credenciales.
    /// </param>
    /// <param name="sesionUsuario">
    /// Servicio encargado de mantener al usuario autenticado
    /// durante la ejecución de la aplicación.
    /// </param>
    public FrmLogin(
        AutenticacionServicio autenticacionServicio,
        SesionUsuario sesionUsuario)
    {
        InitializeComponent();

        _autenticacionServicio = autenticacionServicio;
        _sesionUsuario = sesionUsuario;

        ConfigurarFormulario();
    }

    /// <summary>
    /// Configura las restricciones y el comportamiento inicial
    /// de los controles del formulario.
    /// </summary>
    private void ConfigurarFormulario()
    {
        txtUsuario.MaxLength =
            UsuarioRestricciones.NombreUsuarioMaximo;

        txtContrasena.MaxLength =
            UsuarioRestricciones.PasswordMaximo;

        txtContrasena.UseSystemPasswordChar = true;

        chkMostrarContrasena.Checked = false;

        AcceptButton = btnIngresar;
    }

    /// <summary>
    /// Establece el foco inicial en el campo destinado
    /// al nombre de usuario.
    /// </summary>
    /// <param name="e">
    /// Información asociada al evento de visualización.
    /// </param>
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);

        txtUsuario.Focus();
    }

    /// <summary>
    /// Valida los campos requeridos, solicita la autenticación
    /// y crea la sesión cuando las credenciales son válidas.
    /// </summary>
    /// <param name="sender">
    /// Control que originó el evento.
    /// </param>
    /// <param name="e">
    /// Información asociada al evento.
    /// </param>
    private async void btnIngresar_Click(
        object? sender,
        EventArgs e)
    {
        string nombreUsuario =
            txtUsuario.Text.Trim();

        string password =
            txtContrasena.Text;

        if (!ValidarCamposRequeridos(
            nombreUsuario,
            password))
        {
            return;
        }

        Usuario? usuario = null;
        bool ocurrioErrorTecnico = false;

        try
        {
            CambiarEstadoAutenticacion(true);

            usuario =
                await _autenticacionServicio.AutenticarAsync(
                    nombreUsuario,
                    password);
        }
        catch (Exception)
        {
            ocurrioErrorTecnico = true;
        }
        finally
        {
            CambiarEstadoAutenticacion(false);
        }

        if (ocurrioErrorTecnico)
        {
            MostrarErrorTecnico();
            return;
        }

        if (usuario is null)
        {
            MostrarCredencialesIncorrectas();
            return;
        }

        _sesionUsuario.Iniciar(usuario);

        DialogResult = DialogResult.OK;
        Close();
    }

    /// <summary>
    /// Comprueba que el usuario haya completado los campos
    /// requeridos antes de solicitar la autenticación.
    /// </summary>
    /// <param name="nombreUsuario">
    /// Nombre de usuario normalizado.
    /// </param>
    /// <param name="password">
    /// Contraseña ingresada sin modificaciones.
    /// </param>
    /// <returns>
    /// <see langword="true"/> cuando ambos campos contienen datos;
    /// en caso contrario, <see langword="false"/>.
    /// </returns>
    private bool ValidarCamposRequeridos(
        string nombreUsuario,
        string password)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario))
        {
            MessageBox.Show(
                "Debe ingresar el nombre de usuario.",
                "Datos incompletos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            txtUsuario.SelectAll();
            txtUsuario.Focus();

            return false;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show(
                "Debe ingresar la contraseña.",
                "Datos incompletos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            txtContrasena.SelectAll();
            txtContrasena.Focus();

            return false;
        }

        return true;
    }

    /// <summary>
    /// Informa que las credenciales proporcionadas no permitieron
    /// autenticar al usuario.
    /// </summary>
    /// <remarks>
    /// Se utiliza un mensaje genérico para evitar revelar si el
    /// nombre de usuario existe o cuál credencial es incorrecta.
    /// </remarks>
    private void MostrarCredencialesIncorrectas()
    {
        MessageBox.Show(
            "Usuario o contraseña incorrectos.",
            "Inicio de sesión",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning);

        txtContrasena.Clear();
        txtContrasena.Focus();
    }

    /// <summary>
    /// Informa que ocurrió un problema técnico durante
    /// el proceso de autenticación.
    /// </summary>
    /// <remarks>
    /// El formulario no presenta detalles internos de la excepción
    /// para evitar exponer información técnica al usuario.
    /// </remarks>
    private void MostrarErrorTecnico()
    {
        MessageBox.Show(
            "No fue posible iniciar sesión. " +
            "Verifique la conexión e inténtelo nuevamente.",
            "Error de acceso",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);

        txtContrasena.Clear();
        txtContrasena.Focus();
    }

    /// <summary>
    /// Habilita o bloquea los controles del formulario mientras
    /// se ejecuta la autenticación.
    /// </summary>
    /// <param name="autenticando">
    /// Indica si existe una solicitud de autenticación en curso.
    /// </param>
    private void CambiarEstadoAutenticacion(
        bool autenticando)
    {
        txtUsuario.Enabled = !autenticando;
        txtContrasena.Enabled = !autenticando;
        chkMostrarContrasena.Enabled = !autenticando;
        btnIngresar.Enabled = !autenticando;

        btnIngresar.Text = autenticando
            ? "Ingresando..."
            : "Ingresar";

        UseWaitCursor = autenticando;
    }

    /// <summary>
    /// Alterna entre la visualización y el ocultamiento
    /// de la contraseña ingresada.
    /// </summary>
    /// <param name="sender">
    /// Control que originó el evento.
    /// </param>
    /// <param name="e">
    /// Información asociada al evento.
    /// </param>
    private void chkMostrarContrasena_CheckedChanged(
        object? sender,
        EventArgs e)
    {
        txtContrasena.UseSystemPasswordChar =
            !chkMostrarContrasena.Checked;
    }
}