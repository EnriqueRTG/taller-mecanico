using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Taller.Infrastructure.Persistence;

namespace Taller.App
{
    /// <summary>
    /// Pantalla de gestión de clientes.
    /// 
    /// Por ahora se centra en:
    /// - Mostrar un listado de clientes en una grilla.
    /// - Permitir filtrar por texto (nombre, email, teléfono).
    /// - Dejar preparados los botones de Nuevo / Editar / Eliminar / Actualizar.
    /// 
    /// La lógica de alta/edición/baja se implementará más adelante.
    /// </summary>
    public partial class ClientesForm : Form
    {
        private readonly AppDbContext _db;
        private readonly ILogger<ClientesForm> _logger;
        private readonly IServiceProvider _serviceProvider;
        private int _paginaActual = 1;
        private int _tamPagina = 10;   // podés ajustar a gusto
        private int _totalPaginas = 1;
        private int _totalRegistros = 0;


        /// <summary>
        /// Constructor con inyección de dependencias.
        /// </summary>
        public ClientesForm(AppDbContext db, ILogger<ClientesForm> logger, IServiceProvider serviceProvider)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            InitializeComponent();
        }

        /// <summary>
        /// Evento de carga del formulario.
        /// Carga la lista inicial de clientes.
        /// </summary>
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await CargarClientesAsync();
        }

        /// <summary>
        /// Carga los clientes desde base de datos aplicando el filtro (si lo hubiera)
        /// y los vuelca en la grilla.
        /// </summary>
        private async Task CargarClientesAsync()
        {
            try
            {
                btnBuscar.Enabled = btnActualizar.Enabled = false;

                var filtro = txtBuscar.Text?.Trim();
                var query = _db.Clientes.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    filtro = filtro.ToUpperInvariant();

                    query = query.Where(c =>
                        ((c.Nombre + " " + c.Apellido).ToUpper().Contains(filtro)) ||
                        (c.Email != null && c.Email.ToUpper().Contains(filtro)) ||
                        (c.Telefono != null && c.Telefono.ToUpper().Contains(filtro)));
                }

                var lista = await query
                    .OrderBy(c => c.Apellido).ThenBy(c => c.Nombre)
                    .Select(c => new
                    {
                        c.Id,
                        Nombre = c.Apellido + ", " + c.Nombre, // nombre completo para la grilla
                        c.Email,
                        c.Telefono,
                        c.DocTipo,
                        c.DocNro,
                        c.Direccion
                    })
                    .ToListAsync();

                gridClientes.DataSource = lista;
                gridClientes.AutoResizeColumns();

                _logger.LogInformation("Clientes cargados correctamente. Cantidad: {Cantidad}", lista.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar clientes.");
                MessageBox.Show(
                    "Ocurrió un error al cargar los clientes.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnBuscar.Enabled = btnActualizar.Enabled = true;
            }
        }



        #region Eventos de UI

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            _paginaActual = 1;
            await CargarClientesAsync();
        }

        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            _paginaActual = 1;
            await CargarClientesAsync();
        }

        private async void btnNuevo_Click(object sender, EventArgs e)
        {
            _logger.LogInformation("Acción: Alta de cliente (abrir formulario).");

            using var form = _serviceProvider.GetRequiredService<ClienteEditForm>();

            form.StartPosition = FormStartPosition.CenterParent;

            var result = form.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                // Si se guardó un cliente nuevo, refrescamos la grilla.
                await CargarClientesAsync();
            }

        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (gridClientes.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un cliente.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var id = (long)gridClientes.CurrentRow.Cells["Id"].Value;

            _logger.LogInformation("Acción: Edición de cliente Id={Id}.", id);

            // Resolver el formulario de edición desde DI
            using var form = _serviceProvider.GetRequiredService<ClienteEditForm>();

            //Indicarle que va a trabajar en modo edición para este Id
            form.SetClienteId(id);

            form.StartPosition = FormStartPosition.CenterParent;

            var result = form.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                // Si el usuario guardo cambios, refresca la lista
                await CargarClientesAsync();
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (gridClientes.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un cliente.", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var id = (long)gridClientes.CurrentRow.Cells["Id"].Value;

            var confirmar = MessageBox.Show(
                "¿Seguro que desea dar de baja este cliente?",
                "Confirmar baja",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmar != DialogResult.Yes)
                return;

            try
            {
                var cliente = await _db.Clientes
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (cliente == null)
                {
                    MessageBox.Show(
                        "El cliente ya no existe en la base de datos.",
                        "Información",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                cliente.Activo = false;
                await _db.SaveChangesAsync();

                _logger.LogInformation("Cliente dado de baja lógicamente. Id={Id}, Nombre={Nombre}",
                    cliente.Id, cliente.Nombre);

                await CargarClientesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al dar de baja el cliente Id={Id}.", id);
                MessageBox.Show(
                    "Ocurrió un error al dar de baja el cliente.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void btnPaginaAnterior_Click(object sender, EventArgs e)
        {
            if (_paginaActual > 1)
            {
                _paginaActual--;
                await CargarClientesAsync();
            }
        }

        private async void btnPaginaSiguiente_Click(object sender, EventArgs e)
        {
            if (_paginaActual < _totalPaginas)
            {
                _paginaActual++;
                await CargarClientesAsync();
            }
        }



        #endregion
    }
}
