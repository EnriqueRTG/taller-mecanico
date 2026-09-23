using Microsoft.Extensions.DependencyInjection;

namespace Taller.Presentacion.Formularios.Clientes;

/// <summary>
/// Simula la consulta y administración de clientes y concentra
/// la navegación hacia sus formularios relacionados.
/// </summary>
public partial class FrmClientes : Form
{
    private readonly IServiceProvider _proveedorServicios;
    private readonly List<ClienteFila> _clientes =
    [
        new(1, "DNI", "28456321", "Pérez, Juan", "3794-552211",
            "juan.perez@email.com", "Av. Independencia 1240", true,
            new DateTime(2025, 3, 12)),
        new(2, "DNI", "33654789", "Gómez, Laura", "3794-610287",
            "laura.gomez@email.com", "San Martín 845", true,
            new DateTime(2025, 8, 7)),
        new(3, "CUIT", "20-24875136-4", "Acosta, Miguel", "3794-448912",
            "miguel.acosta@email.com", "Belgrano 230", false,
            new DateTime(2024, 11, 19)),
        new(4, "DNI", "40125874", "Fernández, Sofía", "3794-702144",
            "sofia.fernandez@email.com", "Junín 1578", true,
            new DateTime(2026, 2, 3))
    ];

    private readonly Button _btnVerHistorial = new();
    private bool _configurando;

    public FrmClientes(IServiceProvider proveedorServicios)
    {
        InitializeComponent();
        _proveedorServicios = proveedorServicios
            ?? throw new ArgumentNullException(nameof(proveedorServicios));

        ConfigurarFormulario();
        ConectarEventos();
        AplicarFiltros();
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

        ConfigurarBotonHistorial();
        _configurando = false;
    }

    private void ConfigurarBotonHistorial()
    {
        _btnVerHistorial.Name = "btnVerHistorial";
        _btnVerHistorial.Text = "Ver historial";
        _btnVerHistorial.Size = new Size(112, 26);
        _btnVerHistorial.Margin = new Padding(0, 0, 7, 0);
        _btnVerHistorial.Cursor = Cursors.Hand;
        _btnVerHistorial.Enabled = false;
        _btnVerHistorial.FlatStyle = FlatStyle.Flat;
        _btnVerHistorial.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        _btnVerHistorial.ForeColor = Color.FromArgb(30, 64, 175);
        _btnVerHistorial.FlatAppearance.BorderColor = Color.FromArgb(30, 64, 175);

        flowLayoutPanel1.Controls.Add(_btnVerHistorial);
        flowLayoutPanel1.Controls.SetChildIndex(_btnVerHistorial, 1);
    }

    private void ConectarEventos()
    {
        nuevo.Click += BtnNuevoCliente_Click;
        btnEditarUsuario.Click += BtnEditarCliente_Click;
        _btnVerHistorial.Click += BtnVerHistorial_Click;
        btnCambiarEstado.Click += BtnCambiarEstado_Click;
        btnActualizar.Click += BtnActualizar_Click;
        txtBuscar.TextChanged += Filtro_Cambio;
        cboEstado.SelectedIndexChanged += Filtro_Cambio;
        dgvUsuarios.SelectionChanged += DgvClientes_SelectionChanged;
        dgvUsuarios.CellDoubleClick += DgvClientes_CellDoubleClick;
        dgvUsuarios.CellFormatting += DgvClientes_CellFormatting;
    }

    private void AplicarFiltros()
    {
        if (_configurando)
            return;

        string busqueda = txtBuscar.Text.Trim();

        IEnumerable<ClienteFila> consulta = _clientes;

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            consulta = consulta.Where(cliente =>
                cliente.NombreCompleto.Contains(busqueda, StringComparison.OrdinalIgnoreCase)
                || cliente.Documento.Contains(busqueda, StringComparison.OrdinalIgnoreCase)
                || cliente.Email.Contains(busqueda, StringComparison.OrdinalIgnoreCase));
        }

        consulta = cboEstado.SelectedIndex switch
        {
            1 => consulta.Where(cliente => cliente.Activo),
            2 => consulta.Where(cliente => !cliente.Activo),
            _ => consulta
        };

        List<ClienteFila> resultado = consulta.ToList();

        dgvUsuarios.DataSource = null;
        dgvUsuarios.DataSource = resultado;
        dgvUsuarios.ClearSelection();
        dgvUsuarios.CurrentCell = null;

        lblCantidad.Text = resultado.Count == 1
            ? "1 cliente encontrado"
            : $"{resultado.Count} clientes encontrados";

        ActualizarAcciones();
    }

    private ClienteFila? ObtenerSeleccionado()
    {
        return dgvUsuarios.SelectedRows.Count == 1
            ? dgvUsuarios.SelectedRows[0].DataBoundItem as ClienteFila
            : null;
    }

    private void ActualizarAcciones()
    {
        ClienteFila? cliente = ObtenerSeleccionado();
        bool haySeleccion = cliente is not null;

        btnEditarUsuario.Enabled = haySeleccion;
        _btnVerHistorial.Enabled = haySeleccion;
        btnCambiarEstado.Enabled = haySeleccion;

        bool activo = cliente?.Activo == true;
        btnCambiarEstado.Text = activo
            ? "Deshabilitar cliente"
            : "Habilitar cliente";
        btnCambiarEstado.ForeColor = activo
            ? Color.FromArgb(185, 28, 28)
            : Color.FromArgb(30, 64, 175);
        btnCambiarEstado.FlatAppearance.BorderColor = btnCambiarEstado.ForeColor;
    }

    private void BtnNuevoCliente_Click(object? sender, EventArgs e)
    {
        using FrmAltaCliente formulario =
            _proveedorServicios.GetRequiredService<FrmAltaCliente>();

        formulario.ShowDialog(this);
    }

    private void BtnEditarCliente_Click(object? sender, EventArgs e)
    {
        ClienteFila? cliente = ObtenerSeleccionado();
        if (cliente is null)
            return;

        using FrmEditarCliente formulario =
            _proveedorServicios.GetRequiredService<FrmEditarCliente>();

        formulario.Text = $"Editar cliente - {cliente.NombreCompleto}";
        formulario.ShowDialog(this);
    }

    private void BtnVerHistorial_Click(object? sender, EventArgs e)
    {
        ClienteFila? cliente = ObtenerSeleccionado();
        if (cliente is null)
            return;

        using FrmHistorialCliente formulario =
            new(cliente.NombreCompleto, cliente.Documento);

        formulario.ShowDialog(this);
    }

    private void BtnCambiarEstado_Click(object? sender, EventArgs e)
    {
        ClienteFila? cliente = ObtenerSeleccionado();
        if (cliente is null)
            return;

        string accion = cliente.Activo ? "deshabilitar" : "habilitar";

        DialogResult respuesta = MessageBox.Show(
            $"¿Está seguro de que desea {accion} al cliente " +
            $"\"{cliente.NombreCompleto}\"?\n\n" +
            "Esta operación es una simulación y no modificará la base de datos.",
            "Confirmar cambio de estado",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2);

        if (respuesta != DialogResult.Yes)
            return;

        cliente.Activo = !cliente.Activo;
        AplicarFiltros();

        MessageBox.Show(
            $"El cliente fue {(cliente.Activo ? "habilitado" : "deshabilitado")} " +
            "en la simulación.",
            "Estado actualizado",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void BtnActualizar_Click(object? sender, EventArgs e)
    {
        txtBuscar.Clear();
        cboEstado.SelectedIndex = 0;
        AplicarFiltros();
    }

    private void Filtro_Cambio(object? sender, EventArgs e) => AplicarFiltros();

    private void DgvClientes_SelectionChanged(object? sender, EventArgs e) =>
        ActualizarAcciones();

    private void DgvClientes_CellDoubleClick(
        object? sender,
        DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
            BtnVerHistorial_Click(sender, EventArgs.Empty);
    }

    private void DgvClientes_CellFormatting(
        object? sender,
        DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0
            || dgvUsuarios.Columns[e.ColumnIndex].Name != "colEstado")
            return;

        bool activo = string.Equals(e.Value?.ToString(), "Habilitado",
            StringComparison.OrdinalIgnoreCase);

        e.CellStyle.ForeColor = activo
            ? Color.FromArgb(22, 101, 52)
            : Color.FromArgb(185, 28, 28);
        e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
    }

    private sealed class ClienteFila
    {
        public ClienteFila(
            int id,
            string tipoDocumento,
            string documento,
            string nombreCompleto,
            string telefono,
            string email,
            string direccion,
            bool activo,
            DateTime fechaAlta)
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
        }

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
