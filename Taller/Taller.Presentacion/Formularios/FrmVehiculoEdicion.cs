using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Taller.Dominio.Entidades;
using Taller.Infraestructura.Persistencia;

namespace Taller.Presentacion.Formularios
{
    public partial class FrmVehiculoEdicion : Form
    {
        private readonly TallerDbContext _db;
        private readonly ILogger<FrmVehiculoEdicion> _logger;

        private long? _vehiculoId;

        public long? VehiculoIdResultado { get; private set; }

        public FrmVehiculoEdicion(TallerDbContext db, ILogger<FrmVehiculoEdicion> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            InitializeComponent();
        }

        public void SetVehiculoId(long vehiculoId)
        {
            _vehiculoId = vehiculoId;
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            await CargarClientesAsync();

            if (_vehiculoId.HasValue)
            {
                Text = "Editar vehículo";
                await CargarVehiculoAsync(_vehiculoId.Value);
            }
            else
            {
                Text = "Nuevo vehículo";
                //chkActivo.Checked = true; // alta por defecto
                //chkActivo.Enabled = false; // opcional: no permitir tocar en alta
            }
        }

        private async Task CargarClientesAsync()
        {
            var clientes = await _db.Clientes
                .AsNoTracking()
                .OrderBy(c => c.Apellido).ThenBy(c => c.Nombre)
                .Select(c => new
                {
                    c.Id,
                    Texto = c.Apellido + ", " + c.Nombre + " (" + c.DocTipo + " " + c.DocNro + ")"
                })
                .ToListAsync();

            cboCliente.DataSource = clientes;
            cboCliente.DisplayMember = "Texto";
            cboCliente.ValueMember = "Id";
        }

        private async Task CargarVehiculoAsync(long id)
        {
            try
            {
                var v = await _db.Vehiculos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (v == null)
                {
                    MessageBox.Show("El vehículo seleccionado ya no existe.", "Información",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }

                cboCliente.SelectedValue = v.ClienteId;
                txtPatente.Text = v.Patente;
                txtMarca.Text = v.Marca;
                txtModelo.Text = v.Modelo;
                numAnio.Value = (decimal)v.Anio;
                txtColor.Text = v.Color;
                txtVin.Text = v.Vin;
                txtObservaciones.Text = v.Observaciones;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar vehículo Id={Id}.", id);
                MessageBox.Show("Ocurrió un error al cargar los datos del vehículo.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private bool Validar()
        {
            if (cboCliente.SelectedValue == null)
            {
                MessageBox.Show("Debe seleccionar un cliente.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboCliente.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPatente.Text))
            {
                MessageBox.Show("La patente es obligatoria.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPatente.Focus();
                return false;
            }

            // Normalización de patente: sin espacios, mayúsculas
            var patente = txtPatente.Text.Trim().ToUpperInvariant();
            txtPatente.Text = patente;

            // Validación simple (ajustable): letras/números, sin símbolos raros
            if (!patente.All(char.IsLetterOrDigit))
            {
                MessageBox.Show("La patente debe contener solo letras y números (sin espacios ni símbolos).",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPatente.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMarca.Text))
            {
                MessageBox.Show("La marca es obligatoria.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMarca.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtModelo.Text))
            {
                MessageBox.Show("El modelo es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtModelo.Focus();
                return false;
            }

            // Año: numérico ya lo controla NumericUpDown

            return true;
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Validar())
                return;

            try
            {
                btnGuardar.Enabled = false;

                var clienteId = (long)cboCliente.SelectedValue;

                if (_vehiculoId.HasValue)
                {
                    // EDICIÓN
                    var v = await _db.Vehiculos
                        .FirstOrDefaultAsync(x => x.Id == _vehiculoId.Value);

                    if (v == null)
                    {
                        MessageBox.Show("El vehículo que se intenta editar ya no existe.", "Información",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        DialogResult = DialogResult.Cancel;
                        Close();
                        return;
                    }

                    v.ClienteId = clienteId;
                    v.Patente = txtPatente.Text.Trim().ToUpperInvariant();
                    v.Marca = txtMarca.Text.Trim();
                    v.Modelo = txtModelo.Text.Trim();
                    v.Anio = (int)numAnio.Value;
                    v.Color = string.IsNullOrWhiteSpace(txtColor.Text) ? null : txtColor.Text.Trim();
                    v.Vin = string.IsNullOrWhiteSpace(txtVin.Text) ? null : txtVin.Text.Trim();
                    v.Observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim();
                    //v.Activo = chkActivo.Checked;

                    await _db.SaveChangesAsync();

                    VehiculoIdResultado = v.Id;

                    _logger.LogInformation("Vehículo actualizado. Id={Id}, Patente={Patente}", v.Id, v.Patente);
                }
                else
                {
                    // ALTA
                    var v = new Vehiculo
                    {
                        ClienteId = clienteId,
                        Patente = txtPatente.Text.Trim().ToUpperInvariant(),
                        Marca = txtMarca.Text.Trim(),
                        Modelo = txtModelo.Text.Trim(),
                        Anio = (int)numAnio.Value,
                        Color = string.IsNullOrWhiteSpace(txtColor.Text) ? null : txtColor.Text.Trim(),
                        Vin = string.IsNullOrWhiteSpace(txtVin.Text) ? null : txtVin.Text.Trim(),
                        Observaciones = string.IsNullOrWhiteSpace(txtObservaciones.Text) ? null : txtObservaciones.Text.Trim(),
                        Activo = true
                    };

                    _db.Vehiculos.Add(v);
                    await _db.SaveChangesAsync();

                    VehiculoIdResultado = v.Id;

                    _logger.LogInformation("Vehículo creado. Id={Id}, Patente={Patente}", v.Id, v.Patente);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (DbUpdateException dbEx)
            {
                // MUY probable: patente duplicada si tenés índice único
                _logger.LogError(dbEx, "Error de BD al guardar vehículo (posible patente duplicada).");

                MessageBox.Show(
                    "No se pudo guardar el vehículo. Verifique si la patente ya existe.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar vehículo.");
                MessageBox.Show("Ocurrió un error al guardar el vehículo.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGuardar.Enabled = true;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
