using Taller.Aplicacion.Servicios;

namespace Taller.Presentacion.Formularios.Autenticacion;

/// <summary>
/// Formulario utilizado para autenticar usuarios
/// antes de ingresar al sistema.
/// </summary>
public partial class FrmLogin : Form
{
    private readonly AutenticacionServicio _autenticacionServicio;
    private readonly SesionUsuario _sesionUsuario;

    /// <summary>
    /// Inicializa el formulario utilizando el servicio
    /// de autenticación de la aplicación.
    /// </summary>
    public FrmLogin(
     AutenticacionServicio autenticacionServicio,
     SesionUsuario sesionUsuario)
    {
        InitializeComponent();

        _autenticacionServicio = autenticacionServicio;
        _sesionUsuario = sesionUsuario;
    }

    /// <summary>
    /// Coloca el foco en el campo de usuario
    /// cuando se muestra el formulario.
    /// </summary>
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);

        txtUsuario.Focus();
    }

    /// <summary>
    /// Intenta autenticar al usuario utilizando
    /// las credenciales ingresadas.
    /// </summary>
    private async void btnIngresar_Click(
        object? sender,
        EventArgs e)
    {
        var nombreUsuario = txtUsuario.Text.Trim();
        var password = txtContrasena.Text;

        if (string.IsNullOrWhiteSpace(nombreUsuario) ||
            string.IsNullOrWhiteSpace(password))
        {
            MessageBox.Show(
                "Ingrese el nombre de usuario y la contraseña.",
                "Datos incompletos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                txtUsuario.Focus();
            }
            else
            {
                txtContrasena.Focus();
            }

            return;
        }

        try
        {
            CambiarEstadoAutenticacion(autenticando: true);

            var usuario =
                await _autenticacionServicio.AutenticarAsync(
                    nombreUsuario,
                    password);

            if (usuario is null)
            {
                MessageBox.Show(
                    "Usuario o contraseña incorrectos.",
                    "Inicio de sesión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtContrasena.Clear();
                txtContrasena.Focus();

                return;
            }

            _sesionUsuario.Iniciar(usuario);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception)
        {
            MessageBox.Show(
                "No fue posible iniciar sesión. Verifique la conexión e inténtelo nuevamente.",
                "Error de acceso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            CambiarEstadoAutenticacion(autenticando: false);
        }
    }

    private void CambiarEstadoAutenticacion(
        bool autenticando)
    {
        txtUsuario.Enabled = !autenticando;
        txtContrasena.Enabled = !autenticando;
        chkMostrarContrasena.Enabled = !autenticando;
        btnIngresar.Enabled = !autenticando;

        btnIngresar.Text =
            autenticando
                ? "Ingresando..."
                : "Ingresar";

        Cursor =
            autenticando
                ? Cursors.WaitCursor
                : Cursors.Default;
    }

    private void chkMostrarContrasena_CheckedChanged(
        object? sender,
        EventArgs e)
    {
        txtContrasena.UseSystemPasswordChar =
            !chkMostrarContrasena.Checked;
    }
}