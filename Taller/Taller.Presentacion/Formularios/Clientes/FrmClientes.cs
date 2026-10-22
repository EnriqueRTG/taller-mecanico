using Taller.Aplicacion.Servicios;
using Taller.Presentacion.Navegacion;
using Microsoft.Extensions.DependencyInjection;

namespace Taller.Presentacion.Formularios.Clientes;
/// <summary>
/// Consulta y administra datos persistidos de clientes y concentra
/// la navegación hacia sus formularios relacionados.
/// </summary>
public partial class FrmClientes : Form, IFormularioConOperacion
{
    private readonly IServiceProvider _proveedorServicios;
    private readonly List<ClienteFila> _clientes = [];
    private bool _configurando;
    public bool OperacionEnCurso { get; private set; }
    private ClienteServicio Servicio => _proveedorServicios.GetRequiredService<ClienteServicio>();

    public FrmClientes(IServiceProvider proveedorServicios)
    {
        InitializeComponent();
        _proveedorServicios = proveedorServicios ?? throw new ArgumentNullException(nameof(proveedorServicios));
        ConfigurarFormulario();
    }

    private async void Formulario_Load(object? sender, EventArgs e) => await CargarAsync();
    private async Task CargarAsync()
    {
        if (OperacionEnCurso)
            return;
        try
        {
            OperacionEnCurso = true;
            Enabled = false;
            var registros = await Servicio.ListarAsync();
            _clientes.Clear();
            _clientes.AddRange(registros.Select(c => new ClienteFila(c.IdCliente, c.TipoDocumento, c.Documento, c.Apellido + ", " + c.Nombre, c.Telefono ?? "", c.Email ?? "", c.Direccion ?? "", c.Activo, c.FechaAlta, c.Version.ToArray())));
            AplicarFiltros();
        }
        catch (Exception ex)
        {
            _clientes.Clear();
            AplicarFiltros();
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Cargar clientes");
        }
        finally
        {
            OperacionEnCurso = false;
            Enabled = true;
        }
    }

    private void ConfigurarFormulario()
    {
        _configurando = true;
        dgvUsuarios.AutoGenerateColumns = false;
        dgvUsuarios.MultiSelect = false;
        dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        colId.DataPropertyName = nameof(ClienteFila.Id);
        tipo_documento.DataPropertyName = nameof(ClienteFila.TipoDocumento);
        dni.DataPropertyName = nameof(ClienteFila.Documento);
        colNombreCompleto.DataPropertyName = nameof(ClienteFila.NombreCompleto);
        telefono.DataPropertyName = nameof(ClienteFila.Telefono);
        mail.DataPropertyName = nameof(ClienteFila.Email);
        direccion.DataPropertyName = nameof(ClienteFila.Direccion);
        colEstado.DataPropertyName = nameof(ClienteFila.Estado);
        colFechaAlta.DataPropertyName = nameof(ClienteFila.FechaAlta);
        txtBuscar.PlaceholderText = "Nombre, documento o correo";
        cboEstado.Items.Clear();
        cboEstado.Items.AddRange(["Todos", "Habilitados", "Deshabilitados"]);
        cboEstado.SelectedIndex = 0;
        btnEditarUsuario.Text = "Editar cliente";
        btnCambiarEstado.Text = "Deshabilitar cliente";
        _configurando = false;
    }

    private void AplicarFiltros()
    {
        if (_configurando)
            return;
        string busqueda = txtBuscar.Text.Trim();
        IEnumerable<ClienteFila> consulta = _clientes;
        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            consulta = consulta.Where(cliente => Taller.Aplicacion.Consultas.BusquedaTexto.Coincide(cliente.NombreCompleto, busqueda) || Taller.Aplicacion.Consultas.BusquedaTexto.Coincide(cliente.Documento, busqueda) || Taller.Aplicacion.Consultas.BusquedaTexto.Coincide(cliente.Email, busqueda) || Taller.Aplicacion.Consultas.BusquedaTexto.Coincide(cliente.Telefono, busqueda) || Taller.Aplicacion.Consultas.BusquedaTexto.Coincide(cliente.Direccion, busqueda) || cliente.Id.ToString().Contains(busqueda, StringComparison.OrdinalIgnoreCase));
        }

        consulta = cboEstado.SelectedIndex switch
        {
            1 => consulta.Where(cliente => cliente.Activo),
            2 => consulta.Where(cliente => !cliente.Activo),
            _ => consulta
        };
        List<ClienteFila> resultado = consulta.OrderByDescending(x => x.Activo).ThenByDescending(x => x.FechaAlta).ThenByDescending(x => x.Id).ToList();
        dgvUsuarios.DataSource = null;
        dgvUsuarios.DataSource = resultado;
        dgvUsuarios.ClearSelection();
        dgvUsuarios.CurrentCell = null;
        lblCantidad.Text = resultado.Count == 1 ? "1 cliente encontrado" : $"{resultado.Count} clientes encontrados";
        ActualizarAcciones();
    }

    private ClienteFila? ObtenerSeleccionado()
    {
        return dgvUsuarios.SelectedRows.Count == 1 ? dgvUsuarios.SelectedRows[0].DataBoundItem as ClienteFila : null;
    }

    private void ActualizarAcciones()
    {
        ClienteFila? cliente = ObtenerSeleccionado();
        bool haySeleccion = cliente is not null;
        btnEditarUsuario.Enabled = haySeleccion;
        _btnVerHistorial.Enabled = haySeleccion;
        btnCambiarEstado.Enabled = haySeleccion;
        bool activo = cliente?.Activo == true;
        btnCambiarEstado.Text = activo ? "Deshabilitar cliente" : "Habilitar cliente";
        btnCambiarEstado.ForeColor = activo ? Color.FromArgb(185, 28, 28) : Color.FromArgb(30, 64, 175);
        btnCambiarEstado.FlatAppearance.BorderColor = btnCambiarEstado.ForeColor;
    }

    private async void BtnNuevoCliente_Click(object? sender, EventArgs e)
    {
        using FrmAltaCliente formulario = _proveedorServicios.GetRequiredService<FrmAltaCliente>();
        if (formulario.ShowDialog(this) == DialogResult.OK)
            await CargarAsync();
    }

    private async void BtnEditarCliente_Click(object? sender, EventArgs e)
    {
        ClienteFila? cliente = ObtenerSeleccionado();
        if (cliente is null || OperacionEnCurso)
            return;
        using FrmEditarCliente formulario = _proveedorServicios.GetRequiredService<FrmEditarCliente>();
        bool guardado=false;
        try
        {
            OperacionEnCurso=true;Enabled=false;
            var datos = await Servicio.ObtenerEdicionAsync(cliente.Id);
            formulario.PrepararEdicion(datos);
            guardado=formulario.ShowDialog(this)==DialogResult.OK;
        }
        catch (Exception ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Abrir edición");
        }
        finally {OperacionEnCurso=false;Enabled=true;}
        if(guardado)await CargarAsync();
    }

    private void BtnVerHistorial_Click(object? sender, EventArgs e)
    {
        ClienteFila? cliente = ObtenerSeleccionado();
        if (cliente is null)
            return;
        using FrmHistorialCliente formulario = new(cliente.Id, cliente.NombreCompleto, cliente.Documento, _proveedorServicios.GetRequiredService<AtencionServicio>());
        formulario.ShowDialog(this);
    }

    private async void BtnCambiarEstado_Click(object? sender, EventArgs e)
    {
        var seleccionado = ObtenerSeleccionado();
        if (seleccionado is null || OperacionEnCurso)
            return;
        if (MessageBox.Show(this, "¿Confirma cambiar el estado del registro seleccionado?", "Cambiar estado", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        try
        {
            OperacionEnCurso = true;
            Enabled = false;
            if (seleccionado.Activo)
                await Servicio.DarDeBajaAsync(seleccionado.Id, seleccionado.Version);
            else
                await Servicio.ReactivarAsync(seleccionado.Id, seleccionado.Version);
        }
        catch (Exception ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Cambiar estado");
        }
        finally
        {
            OperacionEnCurso = false;
            Enabled = true;
        }

        await CargarAsync();
    }

    private async void BtnActualizar_Click(object? sender, EventArgs e)
    {
        await CargarAsync();
    }

    private void Filtro_Cambio(object? sender, EventArgs e) => AplicarFiltros();
    private void DgvClientes_SelectionChanged(object? sender, EventArgs e) => ActualizarAcciones();
    private void DgvClientes_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            BtnVerHistorial_Click(sender, EventArgs.Empty);
    }

    private void DgvClientes_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || dgvUsuarios.Columns[e.ColumnIndex].Name != "colEstado")
            return;
        bool activo = string.Equals(e.Value?.ToString(), "Habilitado", StringComparison.OrdinalIgnoreCase);
        e.CellStyle.ForeColor = activo ? Color.FromArgb(22, 101, 52) : Color.FromArgb(185, 28, 28);
        e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
    }

    private sealed class ClienteFila
    {
        public ClienteFila(int id, string tipoDocumento, string documento, string nombreCompleto, string telefono, string email, string direccion, bool activo, DateTime fechaAlta, byte[] version)
        {
            Id = id;
            TipoDocumento = tipoDocumento;
            Documento = documento;
            NombreCompleto = nombreCompleto;
            Telefono = telefono;
            Email = email;
            Direccion = direccion;
            Activo = activo;
            FechaAlta = fechaAlta;
            Version = version.ToArray();
        }

        public byte[] Version { get; }
        public int Id { get; }
        public string TipoDocumento { get; }
        public string Documento { get; }
        public string NombreCompleto { get; }
        public string Telefono { get; }
        public string Email { get; }
        public string Direccion { get; }
        public bool Activo { get; set; }
        public string Estado => Activo ? "Habilitado" : "Deshabilitado";
        public DateTime FechaAlta { get; }
    }
}
