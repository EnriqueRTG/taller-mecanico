using Taller.Aplicacion.Servicios;

namespace Taller.Presentacion.Formularios.Principal;

/// <summary>
/// Representa la pantalla inicial mostrada dentro
/// del formulario principal.
/// </summary>
public partial class FrmInicio : Form
{
    private readonly SesionUsuario _sesionUsuario;

    public FrmInicio(SesionUsuario sesionUsuario)
    {
        InitializeComponent();

        _sesionUsuario = sesionUsuario;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        MostrarBienvenida();
    }

    private void MostrarBienvenida()
    {
        var usuario = _sesionUsuario.UsuarioActual;

        if (usuario is null)
        {
            lblBienvenida.Text = "Bienvenido";
            lblRolActual.Text = string.Empty;
            return;
        }

        lblBienvenida.Text =
            $"Bienvenido, {usuario.Nombre}";

        lblRolActual.Text =
            $"Rol: {usuario.Rol.Nombre}";
    }
}