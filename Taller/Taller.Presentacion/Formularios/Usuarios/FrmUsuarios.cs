using Microsoft.Extensions.DependencyInjection;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Entidades;

namespace Taller.Presentacion.Formularios.Usuarios;

/// <summary>
/// Permite consultar y administrar los usuarios registrados.
/// </summary>
public partial class FrmUsuarios : Form
{
    private readonly UsuarioServicio _usuarioServicio;
    private readonly IServiceProvider _serviceProvider;

    private List<Usuario> _usuarios = [];

    public FrmUsuarios(
        UsuarioServicio usuarioServicio,
        IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _usuarioServicio = usuarioServicio;
        _serviceProvider = serviceProvider;
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        ConfigurarFormulario();
        await CargarUsuariosAsync();
    }

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

    private async Task CargarUsuariosAsync()
    {
        try
        {
            CambiarEstadoCarga(true);

            _usuarios = await _usuarioServicio.ListarAsync();

            AplicarFiltros();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No fue posible cargar los usuarios.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            CambiarEstadoCarga(false);
        }
    }

    private void AplicarFiltros()
    {
        string busqueda = txtBuscar.Text.Trim();

        IEnumerable<Usuario> consulta = _usuarios;

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
            1 => consulta.Where(usuario => usuario.Activo),
            2 => consulta.Where(usuario => !usuario.Activo),
            _ => consulta
        };

        var filas = consulta
            .Select(usuario => new UsuarioFila
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                NombreCompleto =
                    $"{usuario.Apellido}, {usuario.Nombre}",
                Rol = usuario.Rol.Nombre,
                Estado = usuario.Activo
                    ? "Activo"
                    : "Inactivo",
                FechaAlta = usuario.FechaAlta
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

    private async void btnNuevoUsuario_Click(
        object sender,
        EventArgs e)
    {
        try
        {
            using var formulario =
                _serviceProvider.GetRequiredService<FrmAltaUsuario>();

            var resultado = formulario.ShowDialog(this);

            if (resultado == DialogResult.OK)
            {
                await CargarUsuariosAsync();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No fue posible abrir el formulario.\n\n{ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async void btnActualizar_Click(
        object sender,
        EventArgs e)
    {
        await CargarUsuariosAsync();
    }

    private void txtBuscar_TextChanged(
        object sender,
        EventArgs e)
    {
        AplicarFiltros();
    }

    private void cboEstado_SelectedIndexChanged(
        object sender,
        EventArgs e)
    {
        AplicarFiltros();
    }

    private void CambiarEstadoCarga(bool cargando)
    {
        btnNuevoUsuario.Enabled = !cargando;
        btnActualizar.Enabled = !cargando;
        txtBuscar.Enabled = !cargando;
        cboEstado.Enabled = !cargando;

        Cursor = cargando
            ? Cursors.WaitCursor
            : Cursors.Default;
    }

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

    private void dgvUsuarios_CellFormatting(
    object sender,
    DataGridViewCellFormattingEventArgs e)
    {
        if (dgvUsuarios.Columns[e.ColumnIndex].Name != "colEstado")
        {
            return;
        }

        string? estado = e.Value?.ToString();

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

        e.CellStyle.Font =
            new Font("Segoe UI", 9F, FontStyle.Bold);
    }
}