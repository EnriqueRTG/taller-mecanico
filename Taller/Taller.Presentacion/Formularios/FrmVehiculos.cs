using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Taller.Infraestructura.Persistencia;

namespace Taller.Presentacion.Formularios
{

    /// <summary>
    /// Pantalla de gestión de vehículos.
    /// - lista de vehículos en grilla.
    /// - permite filtrar por texto (patente, marca, modelo, cliente).
    /// - permite alta/edición/baja de vehículos (baja logica).
    /// </summary>
    public partial class FrmVehiculos : Form
    {

        private readonly TallerDbContext _db;
        private readonly ILogger<FrmVehiculos> _logger;
        private readonly IServiceProvider _serviceProvider;

        private int _paginaActual = 1;
        private int _tamPagina = 10;
        private int _totalPaginas = 1;
        private int _totalRegistros = 0;

        public FrmVehiculos(TallerDbContext db, ILogger<FrmVehiculos> logger, IServiceProvider serviceProvider)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            InitializeComponent();
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Si agregás combo de cliente, cargalo acá:
            // await CargarClientesFiltroAsync();

            await CargarVehiculosAsync();
        }

        private Task CargarVehiculosAsync()
        {
            return RecargarVehiculosSeleccionandoAsync(null);
        }

        private async Task RecargarVehiculosSeleccionandoAsync(long? idSeleccionar)
        {
            try
            {
                btnBuscar.Enabled = btnActualizar.Enabled = false;

                var filtro = txtBuscar.Text?.Trim();
                var query = _db.Vehiculos
                    .AsNoTracking()
                    .Include(v => v.Cliente); // para mostrar el nombre del cliente en la grilla

                if (!string.IsNullOrWhiteSpace(filtro))
                {
                    filtro = filtro.ToUpperInvariant();

                    query = query.Where(v =>
                        (v.Patente != null && v.Patente.ToUpper().Contains(filtro)) ||
                        (v.Marca != null && v.Marca.ToUpper().Contains(filtro)) ||
                        (v.Modelo != null && v.Modelo.ToUpper().Contains(filtro)) ||
                        ((v.Cliente.Apellido + " " + v.Cliente.Nombre).ToUpper().Contains(filtro))
                    );
                }

                // traer solo activos como default
                // query = query.Where(v => v.Activo);

                var lista = await query
                    .OrderBy(v => v.Patente)
                    .Select(v => new
                    {
                        v.Id,
                        v.Patente,
                        v.Marca,
                        v.Modelo,
                        anio = v.Anio,
                        Cliente = v.Cliente.Apellido + " " + v.Cliente.Nombre,
                        v.Activo
                    })
                    .ToListAsync();

                gridVehiculos.DataSource = lista;
                gridVehiculos.AutoResizeColumns();

                // Seleccionar fila del vehículo afectado (si corresponde)
                if (idSeleccionar.HasValue)
                {
                    foreach (DataGridViewRow row in gridVehiculos.Rows)
                    {
                        if (row.Cells["Id"].Value is long idFila && idFila == idSeleccionar.Value)
                        {
                            row.Selected = true;
                            gridVehiculos.CurrentCell = row.Cells[0]; // para que se muestre como seleccionada aunque no se haga click
                            gridVehiculos.FirstDisplayedScrollingRowIndex = row.Index;
                            break;
                        }
                    }
                }

                _logger.LogInformation("Se cargaron {Cantidad} vehículos (filtro: '{Filtro}')", lista.Count, filtro);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar vehículos");
                MessageBox.Show("Ocurrió un error al cargar los vehículos. Por favor, intente nuevamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnBuscar.Enabled = btnActualizar.Enabled = true;
            }
        }

        #region Eventos UI

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            _paginaActual = 1;
            await CargarVehiculosAsync();
        }

        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            _paginaActual = 1;
            await CargarVehiculosAsync();
        }

        private async void btnNuevo_Click(object sender, EventArgs e)
        {
            _logger.LogInformation("Accion: Alta de vehiculo (abrir formulario).");

            using var form = _serviceProvider.GetRequiredService<FrmVehiculoEdicion>();
            form.StartPosition = FormStartPosition.CenterParent;

            var resultado = form.ShowDialog(this);

            if (resultado == DialogResult.OK && form.VehiculoResultado.HasValue)
            {
                await RecargarVehiculosSeleccionandoAsync(form.VehiculoResultado.Value);
            }
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (gridVehiculos.CurrentRow == null)
            {
                MessageBox.Show("Por favor, seleccione un vehículo para editar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var id = (long)gridVehiculos.CurrentRow.Cells["Id"].Value;

            _logger.LogInformation("Accion: Edición de vehículo Id={Id}.", id);

            using var form = _serviceProvider.GetRequiredService<FrmVehiculoEdicion>();
            form.SetVehiculoId(id);
            form.StartPosition = FormStartPosition.CenterParent;

            var resultado = form.ShowDialog(this);

            if (resultado == DialogResult.OK && form.VehiculoResultado.HasValue)
            {
                await RecargarVehiculosSeleccionandoAsync(form.VehiculoResultado.Value);
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            if (gridVehiculos.CurrentRow == null)
            {
                MessageBox.Show("Por favor, seleccione un vehículo para eliminar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var id = (long)gridVehiculos.CurrentRow.Cells["Id"].Value;

            var confirmar = MessageBox.Show(
                "¿Confirma que desea eliminar este vehículo? Esta acción no se puede deshacer.",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmar == DialogResult.Yes)
                return;

            try
            {
                var vehiculo = await _db.Vehiculos.FirstOrDefaultAsync(v =>
                v.Id == id);

                if (vehiculo == null)
                {
                    MessageBox.Show("El vehículo ya no existe en la base de datos.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Baja lógica: marcar como inactivo
                vehiculo.Activo = false;
                await _db.SaveChangesAsync();

                _logger.LogInformation("Vehículo dado de baja lógicamente. Id={Id}, Patente={Patente}",
                    vehiculo.Id, vehiculo.Patente);

                await CargarVehiculosAsync(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar vehículo Id={Id}", id);
                MessageBox.Show("Ocurrió un error al eliminar el vehículo. Por favor, intente nuevamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

    }
}
