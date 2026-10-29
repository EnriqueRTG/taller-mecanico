using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;

namespace Taller.Presentacion.Formularios.Usuarios;
/// <summary>
/// Permite ingresar los datos necesarios para registrar
/// un nuevo usuario en el sistema.
/// </summary>
/// <remarks>
/// Este formulario realiza validaciones relacionadas con la
/// interacción, como comprobar la confirmación de contraseña
/// y verificar que se haya seleccionado una opción.
///
/// Las reglas indispensables del alta se encuentran en
/// <see cref = "UsuarioServicio"/>.
/// </remarks>
public partial class FrmAltaUsuario : Form
{
    private readonly UsuarioServicio _usuarioServicio;
    /// <summary>
    /// Obtiene el usuario creado correctamente desde
    /// el formulario.
    /// </summary>
    /// <value>
    /// El usuario creado o <see langword="null"/> si la
    /// operación no fue completada.
    /// </value>
    public Usuario? UsuarioCreado { get; private set; }

    /// <summary>
    /// Inicializa una nueva instancia del formulario
    /// de alta de usuarios.
    /// </summary>
    /// <param name = "usuarioServicio">
    /// Servicio de aplicación encargado de consultar roles
    /// disponibles y registrar usuarios.
    /// </param>
    /// <exception cref = "ArgumentNullException">
    /// Se produce cuando el servicio recibido es nulo.
    /// </exception>
    public FrmAltaUsuario(UsuarioServicio usuarioServicio)
    {
        InitializeComponent();
        _usuarioServicio = usuarioServicio ?? throw new ArgumentNullException(nameof(usuarioServicio));
    }

    /// <summary>
    /// Configura los controles y carga los roles disponibles
    /// cuando se abre el formulario.
    /// </summary>
    private async void FrmAltaUsuario_Load(object? sender, EventArgs e)
    {
        ConfigurarFormulario();
        await CargarRolesAsync();
    }

    /// <summary>
    /// Configura las restricciones y el comportamiento inicial
    /// de los controles.
    /// </summary>
    private void ConfigurarFormulario()
    {
        txtNombre.MaxLength = UsuarioRestricciones.NombreMaximo;
        txtApellido.MaxLength = UsuarioRestricciones.ApellidoMaximo;
        txtNombreUsuario.MaxLength = UsuarioRestricciones.NombreUsuarioMaximo;
        txtPassword.MaxLength = UsuarioRestricciones.PasswordMaximo;
        txtPassword.PlaceholderText = $"Entre {UsuarioRestricciones.PasswordMinimo} y {UsuarioRestricciones.PasswordMaximo} caracteres";
        txtConfirmarPassword.MaxLength = UsuarioRestricciones.PasswordMaximo;
        txtPassword.UseSystemPasswordChar = true;
        txtConfirmarPassword.UseSystemPasswordChar = true;
        chkMostrarPassword.Checked = false;
        cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
        AcceptButton = btnGuardar;
        CancelButton = btnCancelar;
        btnCancelar.DialogResult = DialogResult.Cancel;
    }

    /// <summary>
    /// Obtiene mediante la capa de aplicación los roles
    /// disponibles y los carga en el selector.
    /// </summary>
    private async Task CargarRolesAsync()
    {
        try
        {
            CambiarEstadoFormulario(false);
            List<Rol> rolesDisponibles = await _usuarioServicio.ListarRolesDisponiblesAsync();
            cmbRol.DataSource = rolesDisponibles;
            cmbRol.DisplayMember = nameof(Rol.Nombre);
            cmbRol.ValueMember = nameof(Rol.Id);
            cmbRol.SelectedIndex = -1;
            if (rolesDisponibles.Count == 0)
            {
                MessageBox.Show("No existen roles disponibles para " + "registrar nuevos usuarios.", "Roles no disponibles", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CambiarEstadoFormulario(true);
        }
        catch (Exception ex)
        {
            cmbRol.DataSource = null;
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "No se pudo completar la operación de usuarios");
        }
    }

    /// <summary>
    /// Comprueba las validaciones propias de la interfaz.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> cuando la confirmación coincide
    /// y se seleccionó un rol; de lo contrario,
    /// <see langword="false"/>.
    /// </returns>
    private bool ValidarFormulario(out int idRol)
    {
        idRol = 0;
        // Validar Nombre
        if (string.IsNullOrWhiteSpace(txtNombre.Text) || !EsTextoValido(txtNombre.Text))
        {
            MessageBox.Show("El campo Nombre es obligatorio y solo debe contener letras.", "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtNombre.SelectAll();
            txtNombre.Focus();
            return false;
        }

        // Validar Apellido
        if (string.IsNullOrWhiteSpace(txtApellido.Text) || !EsTextoValido(txtApellido.Text))
        {
            MessageBox.Show("El campo Apellido es obligatorio y solo debe contener letras.", "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtApellido.SelectAll();
            txtApellido.Focus();
            return false;
        }

        if (txtPassword.Text != txtConfirmarPassword.Text)
        {
            MessageBox.Show("Las contraseñas no coinciden.", "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtConfirmarPassword.SelectAll();
            txtConfirmarPassword.Focus();
            return false;
        }

        if (cmbRol.SelectedValue is not int rolSeleccionado)
        {
            MessageBox.Show("Debe seleccionar un rol.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            cmbRol.Focus();
            return false;
        }

        idRol = rolSeleccionado;
        return true;
    }

    /// <summary>
    /// Comprueba que una cadena contenga únicamente letras y espacios.
    /// </summary>
    private bool EsTextoValido(string valor)
    {
        foreach (char c in valor)
        {
            if (!char.IsLetter(c) && c != ' ')
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Solicita a la capa de aplicación el registro del
    /// nuevo usuario.
    /// </summary>
    private async void btnGuardar_Click(object? sender, EventArgs e)
    {
        if (!ValidarFormulario(out int idRol))
        {
            return;
        }

        try
        {
            CambiarEstadoGuardado(true);
            UsuarioCreado = await _usuarioServicio.CrearAsync(txtNombreUsuario.Text, txtPassword.Text, txtNombre.Text, txtApellido.Text, idRol);
            MessageBox.Show($"Usuario '{UsuarioCreado.NombreUsuario}' " + "registrado correctamente.", "Nuevo usuario", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (ValidacionException ex)
        {
            CambiarEstadoGuardado(false);
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Datos inválidos");
            if (string.Equals(ex.Campo, "nombreUsuario", StringComparison.OrdinalIgnoreCase))
            {
                txtNombreUsuario.Focus();
                txtNombreUsuario.SelectAll();
            }
        }
        catch (InvalidOperationException ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "No se pudo registrar el usuario");
        }
        catch (Exception ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "No se pudo completar la operación de usuarios");
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
    /// Cancela el alta y cierra el formulario.
    /// </summary>
    private void btnCancelar_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    /// <summary>
    /// Genera una contraseña y completa su confirmación.
    /// </summary>
    private void BtnGenerarPassword_Click(object? sender, EventArgs e)
    {
        txtPassword.Text = Taller.Presentacion.Seguridad.GeneradorContrasenas.Generar();
        txtConfirmarPassword.Text = txtPassword.Text;
        chkMostrarPassword.Checked = true;
        txtPassword.Focus();
        txtPassword.SelectAll();
    }

    private void chkMostrarPassword_CheckedChanged(object? sender, EventArgs e)
    {
        bool ocultar = !chkMostrarPassword.Checked;
        txtPassword.UseSystemPasswordChar = ocultar;
        txtConfirmarPassword.UseSystemPasswordChar = ocultar;
    }

    /// <summary>
    /// Coloca el foco inicial en el nombre de la persona.
    /// </summary>
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        txtNombre.Focus();
    }

    /// <summary>
    /// Habilita o bloquea todo el formulario dependiendo
    /// de si los datos auxiliares están disponibles.
    /// </summary>
    private void CambiarEstadoFormulario(bool habilitado)
    {
        txtNombre.Enabled = habilitado;
        txtApellido.Enabled = habilitado;
        txtNombreUsuario.Enabled = habilitado;
        txtPassword.Enabled = habilitado;
        txtConfirmarPassword.Enabled = habilitado;
        chkMostrarPassword.Enabled = habilitado;
        btnGenerarPassword.Enabled = habilitado;
        cmbRol.Enabled = habilitado;
        btnGuardar.Enabled = habilitado;
        btnCancelar.Enabled = true;
    }

    /// <summary>
    /// Habilita o bloquea los controles mientras se registra
    /// el usuario.
    /// </summary>
    private void CambiarEstadoGuardado(bool guardando)
    {
        txtNombre.Enabled = !guardando;
        txtApellido.Enabled = !guardando;
        txtNombreUsuario.Enabled = !guardando;
        txtPassword.Enabled = !guardando;
        txtConfirmarPassword.Enabled = !guardando;
        chkMostrarPassword.Enabled = !guardando;
        btnGenerarPassword.Enabled = !guardando;
        cmbRol.Enabled = !guardando;
        btnGuardar.Enabled = !guardando;
        btnCancelar.Enabled = !guardando;
        btnGuardar.Text = guardando ? "Guardando..." : "Guardar usuario";
        UseWaitCursor = guardando;
    }

    /// <summary>
    /// Permite únicamente el ingreso de letras, espacios y caracteres acentuados.
    /// </summary>
    private void SoloLetras_KeyPress(object? sender, KeyPressEventArgs e)
    {
        // Permitir tecla de retroceso (Backspace) y teclas de control
        if (char.IsControl(e.KeyChar))
        {
            return;
        }

        // Verificar si es letra, espacio o caracteres acentuados comunes en español
        if (!char.IsLetter(e.KeyChar) && e.KeyChar != ' ' && !char.IsWhiteSpace(e.KeyChar))
        {
            e.Handled = true; // Cancela la tecla presionada
        }
    }
}
