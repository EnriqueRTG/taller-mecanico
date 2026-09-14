using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;

namespace Taller.Presentacion.Formularios.Usuarios;

/// <summary>
/// Permite consultar, filtrar y administrar los usuarios
/// registrados en el sistema.
/// </summary>
/// <remarks>
/// El formulario controla exclusivamente aspectos de presentación:
/// carga visual, filtros, apertura de formularios auxiliares y
/// comunicación de resultados.
///
/// La obtención y las reglas relacionadas con usuarios se delegan
/// en <see cref="UsuarioServicio"/>.
/// </remarks>
public partial class FrmUsuarios : Form
{
    private readonly UsuarioServicio _usuarioServicio;
    private readonly IServiceProvider _proveedorServicios;

    private List<Usuario> _usuarios = [];

    /// <summary>
    /// Inicializa una nueva instancia del formulario de usuarios.
    /// </summary>
    /// <param name="usuarioServicio">
    /// Servicio de aplicación utilizado para consultar usuarios.
    /// </param>
    /// <param name="proveedorServicios">
    /// Proveedor utilizado para resolver formularios auxiliares.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Se produce cuando alguna dependencia requerida es nula.
    /// </exception>
    public FrmUsuarios(
        UsuarioServicio usuarioServicio,
        IServiceProvider proveedorServicios)
    {
        InitializeComponent();

        _usuarioServicio =
            usuarioServicio
            ?? throw new ArgumentNullException(
                nameof(usuarioServicio));

        _proveedorServicios =
            proveedorServicios
            ?? throw new ArgumentNullException(
                nameof(proveedorServicios));
    }

    /// <summary>
    /// Configura los controles y carga los usuarios cuando
    /// se abre el formulario.
    /// </summary>
    protected override async void OnLoad(
        EventArgs e)
    {
        base.OnLoad(e);

        ConfigurarFormulario();

        await CargarUsuariosAsync();
    }

    /// <summary>
    /// Establece el comportamiento inicial de los controles.
    /// </summary>
    private void ConfigurarFormulario()
    {
        dgvUsuarios.AutoGenerateColumns = false;

        cboEstado.Items.Clear();
        cboEstado.Items.Add("Todos");
        cboEstado.Items.Add("Activos");
        cboEstado.Items.Add("Inactivos");
        cboEstado.SelectedIndex = 0;

        txtBuscar.Clear();
    }

    /// <summary>
    /// Obtiene los usuarios mediante la capa de aplicación
    /// y actualiza la grilla.
    /// </summary>
    private async Task CargarUsuariosAsync()
    {
        try
        {
            CambiarEstadoCarga(true);

            _usuarios =
                await _usuarioServicio.ListarAsync();

            AplicarFiltros();
        }
        catch (Exception)
        {
            _usuarios = [];
            AplicarFiltros();

            MessageBox.Show(
                "No fue posible cargar los usuarios. " +
                "Verifique la conexión e inténtelo nuevamente.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            CambiarEstadoCarga(false);
        }
    }

    /// <summary>
    /// Aplica los criterios de búsqueda y estado sobre
    /// los usuarios cargados actualmente.
    /// </summary>
    private void AplicarFiltros()
    {
        string busqueda =
            txtBuscar.Text.Trim();

        IEnumerable<Usuario> consulta =
            _usuarios;

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            consulta = consulta.Where(usuario =>
                usuario.NombreUsuario.Contains(
                    busqueda,
                    StringComparison.OrdinalIgnoreCase)
                ||
                usuario.Nombre.Contains(
                    busqueda,
                    StringComparison.OrdinalIgnoreCase)
                ||
                usuario.Apellido.Contains(
                    busqueda,
                    StringComparison.OrdinalIgnoreCase));
        }

        consulta = cboEstado.SelectedIndex switch
        {
            1 => consulta.Where(
                usuario => usuario.Activo),

            2 => consulta.Where(
                usuario => !usuario.Activo),

            _ => consulta
        };

        List<UsuarioFila> filas =
            consulta
                .Select(usuario => new UsuarioFila
                {
                    Id = usuario.Id,

                    NombreUsuario =
                        usuario.NombreUsuario,

                    NombreCompleto =
                        $"{usuario.Apellido}, {usuario.Nombre}",

                    Rol =
                        usuario.Rol?.Nombre
                        ?? "Rol no disponible",

                    Estado =
                        usuario.Activo
                            ? "Activo"
                            : "Inactivo",

                    FechaAlta =
                        usuario.FechaAlta
                })
                .ToList();

        dgvUsuarios.DataSource = null;
        dgvUsuarios.DataSource = filas;
        dgvUsuarios.ClearSelection();

        lblCantidad.Text =
            filas.Count == 1
                ? "1 usuario encontrado"
                : $"{filas.Count} usuarios encontrados";
    }

    /// <summary>
    /// Abre el formulario de alta y actualiza el listado
    /// cuando se registra correctamente un usuario.
    /// </summary>
    private async void btnNuevoUsuario_Click(
        object? sender,
        EventArgs e)
    {
        try
        {
            using FrmAltaUsuario formulario =
                _proveedorServicios
                    .GetRequiredService<FrmAltaUsuario>();

            DialogResult resultado =
                formulario.ShowDialog(this);

            if (resultado == DialogResult.OK)
            {
                await CargarUsuariosAsync();
            }
        }
        catch (Exception)
        {
            MessageBox.Show(
                "No fue posible abrir el formulario " +
                "de alta de usuarios.",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Actualiza manualmente el listado de usuarios.
    /// </summary>
    private async void btnActualizar_Click(
        object? sender,
        EventArgs e)
    {
        await CargarUsuariosAsync();
    }

    /// <summary>
    /// Actualiza la grilla cuando cambia el texto de búsqueda.
    /// </summary>
    private void txtBuscar_TextChanged(
        object? sender,
        EventArgs e)
    {
        AplicarFiltros();
    }

    /// <summary>
    /// Actualiza la grilla cuando cambia el filtro de estado.
    /// </summary>
    private void cboEstado_SelectedIndexChanged(
        object? sender,
        EventArgs e)
    {
        AplicarFiltros();
    }

    /// <summary>
    /// Habilita o bloquea los controles mientras se cargan datos.
    /// </summary>
    /// <param name="cargando">
    /// Indica si existe una carga en curso.
    /// </param>
    private void CambiarEstadoCarga(
        bool cargando)
    {
        btnNuevoUsuario.Enabled = !cargando;
        btnActualizar.Enabled = !cargando;
        txtBuscar.Enabled = !cargando;
        cboEstado.Enabled = !cargando;

        UseWaitCursor = cargando;
    }

    /// <summary>
    /// Aplica colores diferenciados según el estado
    /// mostrado en cada fila.
    /// </summary>
    private void dgvUsuarios_CellFormatting(
        object? sender,
        DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0
            || e.ColumnIndex < 0
            || dgvUsuarios.Columns[e.ColumnIndex].Name
                != "colEstado")
        {
            return;
        }

        string? estado =
            e.Value?.ToString();

        if (estado == "Activo")
        {
            e.CellStyle.ForeColor =
                Color.FromArgb(22, 101, 52);

            e.CellStyle.SelectionForeColor =
                Color.FromArgb(22, 101, 52);
        }
        else if (estado == "Inactivo")
        {
            e.CellStyle.ForeColor =
                Color.FromArgb(185, 28, 28);

            e.CellStyle.SelectionForeColor =
                Color.FromArgb(185, 28, 28);
        }
    }

    /// <summary>
    /// Representa una fila adaptada específicamente
    /// para la grilla de usuarios.
    /// </summary>
    private sealed class UsuarioFila
    {
        public int Id { get; init; }

        public string NombreUsuario { get; init; }
            = string.Empty;

        public string NombreCompleto { get; init; }
            = string.Empty;

        public string Rol { get; init; }
            = string.Empty;

        public string Estado { get; init; }
            = string.Empty;

        public DateTime FechaAlta { get; init; }
    }
}