using System.Security.Cryptography;
using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;

namespace Taller.Presentacion.Formularios.Usuarios;

/// <summary>
/// Permite al administrador gestionar las credenciales
/// de otro usuario.
/// </summary>
public partial class FrmGestionarCredenciales : Form
{
    private readonly UsuarioServicio _usuarioServicio;
    private readonly SesionUsuario _sesionUsuario;

    private int _usuarioId;
    private bool _guardando;

    public FrmGestionarCredenciales(
        UsuarioServicio usuarioServicio,
        SesionUsuario sesionUsuario)
    {
        InitializeComponent();

        _usuarioServicio = usuarioServicio
            ?? throw new ArgumentNullException(nameof(usuarioServicio));

        _sesionUsuario = sesionUsuario
            ?? throw new ArgumentNullException(nameof(sesionUsuario));

        btnGuardar.Click += btnGuardar_Click;
        btnCancelar.Click += btnCancelar_Click;
        btnGenerarPassword.Click += btnGenerarPassword_Click;
        chkMostrarPassword.CheckedChanged += chkMostrarPassword_CheckedChanged;
    }

    public void PrepararGestion(int usuarioId)
    {
        if (usuarioId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(usuarioId));
        }

        _usuarioId = usuarioId;
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (_usuarioId <= 0)
        {
            MessageBox.Show(
                "No se indicó el usuario cuyas credenciales se gestionarán.",
                "Usuario no seleccionado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            DialogResult = DialogResult.Cancel;
            Close();
            return;
        }

        txtId.ReadOnly = true;
        txtNombreCompleto.ReadOnly = true;
        txtId.TabStop = false;
        txtNombreCompleto.TabStop = false;

        txtNombreUsuario.MaxLength =
            UsuarioRestricciones.NombreUsuarioMaximo;

        txtNuevaPassword.MaxLength =
            UsuarioRestricciones.PasswordMaximo;

        txtConfirmarPassword.MaxLength =
            UsuarioRestricciones.PasswordMaximo;

        txtNuevaPassword.UseSystemPasswordChar = true;
        txtConfirmarPassword.UseSystemPasswordChar = true;

        AcceptButton = btnGuardar;
        CancelButton = btnCancelar;

        try
        {
            Usuario? usuario =
                await _usuarioServicio.ObtenerPorIdAsync(_usuarioId);

            if (usuario is null)
            {
                throw new InvalidOperationException(
                    "El usuario seleccionado ya no existe.");
            }

            txtId.Text = usuario.Id.ToString();
            txtNombreCompleto.Text =
                $"{usuario.Apellido}, {usuario.Nombre}";
            txtNombreUsuario.Text = usuario.NombreUsuario;

            // Las contraseñas nunca se recuperan desde la base.
            txtNuevaPassword.Clear();
            txtConfirmarPassword.Clear();
        }
        catch (Exception)
        {
            MessageBox.Show(
                "No fue posible cargar los datos del usuario.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            DialogResult = DialogResult.Cancel;
            Close();
        }
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);

        if (!IsDisposed)
        {
            txtNombreUsuario.Focus();
        }
    }

    private void chkMostrarPassword_CheckedChanged(
        object? sender,
        EventArgs e)
    {
        bool ocultar = !chkMostrarPassword.Checked;

        txtNuevaPassword.UseSystemPasswordChar = ocultar;
        txtConfirmarPassword.UseSystemPasswordChar = ocultar;
    }

    private void btnGenerarPassword_Click(
        object? sender,
        EventArgs e)
    {
        // Incluye letras, números y símbolos sin caracteres
        // visualmente fáciles de confundir, como O/0 o l/1.
        const string caracteres =
            "ABCDEFGHJKLMNPQRSTUVWXYZ" +
            "abcdefghijkmnopqrstuvwxyz" +
            "23456789" +
            "!@#$%";

        int longitud = Math.Clamp(
            16,
            UsuarioRestricciones.PasswordMinimo,
            UsuarioRestricciones.PasswordMaximo);

        var caracteresGenerados = new char[longitud];

        for (int i = 0; i < caracteresGenerados.Length; i++)
        {
            caracteresGenerados[i] =
                caracteres[
                    RandomNumberGenerator.GetInt32(
                        caracteres.Length)];
        }

        string password = new(caracteresGenerados);

        txtNuevaPassword.Text = password;
        txtConfirmarPassword.Text = password;

        chkMostrarPassword.Checked = true;
        txtNuevaPassword.Focus();
        txtNuevaPassword.SelectAll();
    }

    private async void btnGuardar_Click(
        object? sender,
        EventArgs e)
    {
        if (_guardando)
        {
            return;
        }

        Usuario? ejecutor =
            _sesionUsuario.UsuarioActual;

        if (ejecutor is null)
        {
            MessageBox.Show(
                "La sesión actual no es válida. Vuelva a iniciar sesión.",
                "Sesión no disponible",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        // Si se generó una contraseña, el administrador debe
        // disponer de ella antes de cerrar este formulario.
        if (!string.IsNullOrEmpty(txtNuevaPassword.Text))
        {
            DialogResult confirmacion = MessageBox.Show(
                "Después de guardar no podrá volver a consultar " +
                "esta contraseña. Asegúrese de haberla anotado " +
                "o comunicado por un medio adecuado.\n\n" +
                "¿Desea guardar las credenciales?",
                "Confirmar cambio de contraseña",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion != DialogResult.Yes)
            {
                return;
            }
        }

        try
        {
            CambiarEstadoGuardado(true);

            await _usuarioServicio.ActualizarCredencialesAsync(
                _usuarioId,
                txtNombreUsuario.Text,
                txtNuevaPassword.Text,
                txtConfirmarPassword.Text,
                ejecutor.Id);

            CambiarEstadoGuardado(false);

            MessageBox.Show(
                "Las credenciales se actualizaron correctamente.",
                "Credenciales actualizadas",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (ValidacionException ex)
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
                "No se pudieron actualizar las credenciales",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        catch (Exception)
        {
            MessageBox.Show(
                "Ocurrió un error al actualizar las credenciales. " +
                "Inténtelo nuevamente.",
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

    private void btnCancelar_Click(
        object? sender,
        EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void CambiarEstadoGuardado(bool guardando)
    {
        _guardando = guardando;

        txtNombreUsuario.Enabled = !guardando;
        txtNuevaPassword.Enabled = !guardando;
        txtConfirmarPassword.Enabled = !guardando;
        chkMostrarPassword.Enabled = !guardando;
        btnGenerarPassword.Enabled = !guardando;
        btnGuardar.Enabled = !guardando;
        btnCancelar.Enabled = !guardando;

        btnGuardar.Text =
            guardando
                ? "Guardando..."
                : "Guardar credenciales";
    }
}