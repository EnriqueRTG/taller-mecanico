using Taller.Aplicacion.Servicios;

namespace Taller.Presentacion.Formularios.Autenticacion;

/// <summary>
/// Formulario utilizado para autenticar usuarios
/// antes de ingresar al sistema.
/// </summary>
public partial class Login : Form
{
    private readonly AutenticacionServicio _autenticacionServicio;

    /// <summary>
    /// Inicializa el formulario utilizando el servicio
    /// de autenticación de la aplicación.
    /// </summary>
    public Login(AutenticacionServicio autenticacionServicio)
    {
        InitializeComponent();

        _autenticacionServicio = autenticacionServicio;
    }

    /// <summary>
    /// Intenta autenticar al usuario utilizando
    /// las credenciales ingresadas.
    /// </summary>
    private async void iniciar_Click(
        object? sender,
        EventArgs e)
    {
        var nombreUsuario = textBox_usuario.Text.Trim();
        var password = textBox_contraseña.Text;

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

            return;
        }

        MessageBox.Show(
            $"Bienvenido {usuario.Nombre} {usuario.Apellido}.\n" +
            $"Rol: {usuario.Rol.Nombre}",
            "Inicio de sesión",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}