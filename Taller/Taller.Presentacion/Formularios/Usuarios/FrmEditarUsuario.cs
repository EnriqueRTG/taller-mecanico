using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;

namespace Taller.Presentacion.Formularios.Usuarios;

/// <summary>
/// Permite modificar los datos personales y el rol
/// de un usuario registrado.
/// </summary>
/// <remarks>
/// El identificador, la fecha de alta, el estado y las
/// credenciales del usuario se muestran únicamente como
/// información y no pueden modificarse desde este formulario.
/// </remarks>
public partial class FrmEditarUsuario : Form
{
    private readonly UsuarioServicio _usuarioServicio;
    private readonly SesionUsuario _sesionUsuario;

    private int _usuarioId;

    /// <summary>
    /// Inicializa una nueva instancia del formulario.
    /// </summary>
    /// <param name="usuarioServicio">
    /// Servicio utilizado para consultar y actualizar usuarios.
    /// </param>
    /// <param name="sesionUsuario">
    /// Sesión utilizada para identificar al administrador
    /// que realiza la operación.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Se produce cuando alguna dependencia requerida es nula.
    /// </exception>
    public FrmEditarUsuario(
        UsuarioServicio usuarioServicio,
        SesionUsuario sesionUsuario)
    {
        InitializeComponent();

        _usuarioServicio =
            usuarioServicio
            ?? throw new ArgumentNullException(
                nameof(usuarioServicio));

        _sesionUsuario =
            sesionUsuario
            ?? throw new ArgumentNullException(
                nameof(sesionUsuario));

        btnGuardar.Click += btnGuardar_Click;
        btnCancelar.Click += btnCancelar_Click;
    }

    /// <summary>
    /// Establece el usuario que será editado.
    /// Debe invocarse antes de mostrar el formulario.
    /// </summary>
    /// <param name="usuarioId">
    /// Identificador del usuario seleccionado.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Se produce cuando el identificador no es válido.
    /// </exception>
    public void PrepararEdicion(
        int usuarioId)
    {
        if (usuarioId <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(usuarioId),
                "El identificador del usuario no es válido.");
        }

        _usuarioId = usuarioId;
    }

    /// <summary>
    /// Configura el formulario y carga los datos del usuario.
    /// </summary>
    protected override async void OnLoad(
        EventArgs e)
    {
        base.OnLoad(e);

        try
        {
            ValidarPreparacion();
            ConfigurarFormulario();

            await CargarRolesAsync();
            await CargarUsuarioAsync();
        }
        catch (Exception)
        {
            MessageBox.Show(
                "No fue posible cargar los datos del usuario. " +
                "Inténtelo nuevamente.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            DialogResult = DialogResult.Cancel;
            Close();
        }
    }

    /// <summary>
    /// Establece las restricciones y el comportamiento
    /// inicial de los controles.
    /// </summary>
    private void ConfigurarFormulario()
    {
        txtNombre.MaxLength =
            UsuarioRestricciones.NombreMaximo;

        txtApellido.MaxLength =
            UsuarioRestricciones.ApellidoMaximo;

        cmbRol.DropDownStyle =
            ComboBoxStyle.DropDownList;

        AcceptButton = btnGuardar;
        CancelButton = btnCancelar;

        btnCancelar.DialogResult =
            DialogResult.Cancel;
    }

    /// <summary>
    /// Carga los roles activos y válidos que pueden
    /// asignarse al usuario.
    /// </summary>
    private async Task CargarRolesAsync()
    {
        List<Rol> roles =
            await _usuarioServicio
                .ListarRolesDisponiblesAsync();

        cmbRol.DataSource = roles;
        cmbRol.DisplayMember = nameof(Rol.Nombre);
        cmbRol.ValueMember = nameof(Rol.Id);
        cmbRol.SelectedIndex = -1;
    }

    /// <summary>
    /// Obtiene el usuario seleccionado y muestra sus datos.
    /// </summary>
    private async Task CargarUsuarioAsync()
    {
        Usuario? usuario =
            await _usuarioServicio
                .ObtenerPorIdAsync(_usuarioId);

        if (usuario is null)
        {
            throw new InvalidOperationException(
                "El usuario seleccionado no existe.");
        }

        txtId.Text =
            usuario.Id.ToString();

        txtNombreUsuario.Text =
            usuario.NombreUsuario;

        txtFechaAlta.Text =
            usuario.FechaAlta.ToString("dd/MM/yyyy");

        txtEstado.Text =
            usuario.Activo
                ? "Activo"
                : "Inactivo";

        txtEstado.ForeColor =
            usuario.Activo
                ? Color.FromArgb(22, 101, 52)
                : Color.FromArgb(185, 28, 28);

        txtNombre.Text =
            usuario.Nombre;

        txtApellido.Text =
            usuario.Apellido;

        cmbRol.SelectedValue =
            usuario.RolId;
    }

    /// <summary>
    /// Valida los datos ingresados y guarda los cambios.
    /// </summary>
    private async void btnGuardar_Click(
        object? sender,
        EventArgs e)
    {
        if (cmbRol.SelectedValue is not int rolId)
        {
            MessageBox.Show(
                "Debe seleccionar un rol.",
                "Datos incompletos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            cmbRol.Focus();
            return;
        }

        Usuario? usuarioEjecutor =
            _sesionUsuario.UsuarioActual;

        if (usuarioEjecutor is null)
        {
            MessageBox.Show(
                "La sesión actual no es válida. " +
                "Vuelva a iniciar sesión.",
                "Sesión no disponible",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            return;
        }

        Exception? errorActualizacion = null;

        try
        {
            CambiarEstadoOperacion(
                procesando: true,
                textoBoton: "Guardando...");

            await _usuarioServicio.ActualizarDatosAsync(
                _usuarioId,
                txtNombre.Text,
                txtApellido.Text,
                rolId,
                usuarioEjecutor.Id);
        }
        catch (Exception ex)
        {
            errorActualizacion = ex;
        }
        finally
        {
            if (!IsDisposed && !Disposing)
            {
                CambiarEstadoOperacion(
                    procesando: false,
                    textoBoton: "Guardar cambios");
            }
        }

        if (errorActualizacion is not null)
        {
            MostrarErrorActualizacion(
                errorActualizacion);

            return;
        }

        MessageBox.Show(
            "Los datos del usuario se actualizaron correctamente.",
            "Usuario actualizado",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        DialogResult = DialogResult.OK;
        Close();
    }

    /// <summary>
    /// Muestra un mensaje apropiado según el error producido
    /// durante la actualización.
    /// </summary>
    private void MostrarErrorActualizacion(
        Exception error)
    {
        switch (error)
        {
            case ValidacionException validacion:
                MessageBox.Show(
                    validacion.Message,
                    "Datos inválidos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                break;

            case InvalidOperationException operacion:
                MessageBox.Show(
                    operacion.Message,
                    "No se pudo actualizar el usuario",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                break;

            default:
                MessageBox.Show(
                    "Ocurrió un error inesperado al actualizar " +
                    "el usuario. Inténtelo nuevamente.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                break;
        }
    }

    /// <summary>
    /// Cancela la edición y cierra el formulario.
    /// </summary>
    private void btnCancelar_Click(
        object? sender,
        EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    /// <summary>
    /// Comprueba que se haya indicado qué usuario editar.
    /// </summary>
    private void ValidarPreparacion()
    {
        if (_usuarioId <= 0)
        {
            throw new InvalidOperationException(
                "No se indicó el usuario que será editado.");
        }
    }

    /// <summary>
    /// Habilita o bloquea los controles mientras se ejecuta
    /// una operación asincrónica.
    /// </summary>
    private void CambiarEstadoOperacion(
    bool procesando,
    string textoBoton)
    {
        txtNombre.Enabled = !procesando;
        txtApellido.Enabled = !procesando;
        cmbRol.Enabled = !procesando;
        btnGuardar.Enabled = !procesando;
        btnCancelar.Enabled = !procesando;

        btnGuardar.Text = textoBoton;
    }

}