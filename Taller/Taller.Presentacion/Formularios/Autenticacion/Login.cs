using Taller.Aplicacion.Servicios;

namespace Taller.Presentacion.Formularios.Autenticacion;

/// <summary>
/// Formulario utilizado para autenticar usuarios
/// antes de ingresar al sistema.
/// </summary>
public partial class Login : Form
{
    private readonly AutenticacionServicio _autenticacionServicio;
    private readonly SesionUsuario _sesionUsuario;

    /// <summary>
    /// Inicializa el formulario utilizando el servicio
    /// de autenticación de la aplicación.
    /// </summary>
    public Login(
     AutenticacionServicio autenticacionServicio,
     SesionUsuario sesionUsuario)
    {
        InitializeComponent();

        _autenticacionServicio = autenticacionServicio;
        _sesionUsuario = sesionUsuario;
    }

    /// <summary>
    /// Intenta autenticar al usuario utilizando
    /// las credenciales ingresadas.
    /// </summary>
    private async void iniciar_Click(object? sender, EventArgs e)
    {
        var nombreUsuario = textBox_usuario.Text.Trim();
        var password = textBox_contraseña.Text;

        var usuario = await _autenticacionServicio.AutenticarAsync(nombreUsuario, password);

        if (usuario is null)
        {
            MessageBox.Show(
                "Usuario o contraseña incorrectos.",
                "Inicio de sesión",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        _sesionUsuario.Iniciar(usuario);

        DialogResult = DialogResult.OK;
        Close();
    }
}