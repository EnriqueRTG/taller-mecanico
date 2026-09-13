using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;

namespace Taller.Presentacion.Formularios.Usuarios;

/// <summary>
/// Formulario utilizado para registrar nuevos usuarios en el sistema.
/// </summary>
public partial class FrmAltaUsuario : Form
{
    private readonly UsuarioServicio _usuarioServicio;
    private readonly IRolRepositorio _rolRepositorio;

    /// <summary>
    /// Usuario creado correctamente desde el formulario.
    /// Puede utilizarse por el formulario que haya abierto esta ventana.
    /// </summary>
    public Usuario? UsuarioCreado { get; private set; }

    /// <summary>
    /// Inicializa una nueva instancia del formulario de alta de usuarios.
    /// </summary>
    /// <param name="usuarioServicio">
    /// Servicio de aplicación encargado de gestionar usuarios.
    /// </param>
    /// <param name="rolRepositorio">
    /// Repositorio utilizado para obtener los roles disponibles.
    /// </param>
    public FrmAltaUsuario(
        UsuarioServicio usuarioServicio,
        IRolRepositorio rolRepositorio)
    {
        InitializeComponent();

        _usuarioServicio = usuarioServicio;
        _rolRepositorio = rolRepositorio;
    }

    /// <summary>
    /// Configura los controles del formulario y carga
    /// los roles disponibles al abrir la ventana.
    /// </summary>
    private async void FrmAltaUsuario_Load(
        object sender,
        EventArgs e)
    {
        try
        {
            ConfigurarFormulario();

            await CargarRolesAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No fue posible inicializar el formulario.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Configura las propiedades iniciales de los controles.
    /// </summary>
    private void ConfigurarFormulario()
    {
        txtNombre.MaxLength = 100;
        txtApellido.MaxLength = 100;
        txtNombreUsuario.MaxLength = 50;
        txtPassword.MaxLength = 100;
        txtConfirmarPassword.MaxLength = 100;

        txtPassword.UseSystemPasswordChar = true;
        txtConfirmarPassword.UseSystemPasswordChar = true;

        chkMostrarPassword.Checked = false;

        cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;

        AcceptButton = btnGuardar;
        CancelButton = btnCancelar;

        btnCancelar.DialogResult = DialogResult.Cancel;
    }

    /// <summary>
    /// Obtiene y carga en el ComboBox los roles activos
    /// disponibles para asignar al nuevo usuario.
    /// </summary>
    private async Task CargarRolesAsync()
    {
        var roles = await _rolRepositorio.ListarActivosAsync();

        cmbRol.DataSource = roles;
        cmbRol.DisplayMember = "Nombre";
        cmbRol.ValueMember = "Id";

        cmbRol.SelectedIndex = -1;
    }

    /// <summary>
    /// Valida los datos ingresados y registra un nuevo usuario.
    /// </summary>
    private async void btnGuardar_Click(
        object sender,
        EventArgs e)
    {
        if (txtPassword.Text != txtConfirmarPassword.Text)
        {
            MessageBox.Show(
                "Las contraseñas no coinciden.",
                "Datos inválidos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            txtConfirmarPassword.SelectAll();
            txtConfirmarPassword.Focus();
            return;
        }

        if (cmbRol.SelectedValue is not int idRol)
        {
            MessageBox.Show(
                "Debe seleccionar un rol.",
                "Datos incompletos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            cmbRol.Focus();
            return;
        }

        try
        {
            CambiarEstadoGuardado(true);

            UsuarioCreado = await _usuarioServicio.CrearAsync(
                txtNombreUsuario.Text,
                txtPassword.Text,
                txtNombre.Text,
                txtApellido.Text,
                idRol);

            MessageBox.Show(
                $"Usuario '{UsuarioCreado.NombreUsuario}' registrado correctamente.",
                "Nuevo usuario",
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
                "No se pudo registrar el usuario",
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
            if (!IsDisposed && !Disposing)
            {
                CambiarEstadoGuardado(false);
            }
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

    /// <summary>
    /// Permite mostrar u ocultar el contenido de los campos
    /// de contraseña y confirmación.
    /// </summary>
    private void chkMostrarPassword_CheckedChanged(
        object sender,
        EventArgs e)
    {
        bool ocultar = !chkMostrarPassword.Checked;

        txtPassword.UseSystemPasswordChar = ocultar;
        txtConfirmarPassword.UseSystemPasswordChar = ocultar;
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        txtNombre.Focus();
    }

    private void CambiarEstadoGuardado(bool guardando)
    {
        txtNombre.Enabled = !guardando;
        txtApellido.Enabled = !guardando;
        txtNombreUsuario.Enabled = !guardando;
        txtPassword.Enabled = !guardando;
        txtConfirmarPassword.Enabled = !guardando;
        chkMostrarPassword.Enabled = !guardando;
        cmbRol.Enabled = !guardando;
        btnGuardar.Enabled = !guardando;
        btnCancelar.Enabled = !guardando;

        btnGuardar.Text = guardando
            ? "Guardando..."
            : "Guardar usuario";

        UseWaitCursor = guardando;
    }
}