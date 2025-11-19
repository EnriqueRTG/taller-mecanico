using System;
using System.Linq;                 // Necesario para .All(...)
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Taller.Domain.Entities;
using Taller.Infrastructure.Persistence;

namespace Taller.App
{
    /// <summary>
    /// Formulario de alta / edición de Cliente.
    /// - Si _clienteId es null: modo ALTA.
    /// - Si _clienteId tiene valor: modo EDICIÓN.
    /// </summary>
    public partial class ClienteEditForm : Form
    {
        private readonly AppDbContext _db;
        private readonly ILogger<ClienteEditForm> _logger;

        /// <summary>
        /// Id del cliente a editar. Si es null, el formulario trabaja en modo ALTA.
        /// </summary>
        private long? _clienteId;

        public ClienteEditForm(AppDbContext db, ILogger<ClienteEditForm> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            InitializeComponent();
        }

        /// <summary>
        /// Configura el formulario para trabajar en modo EDICIÓN.
        /// </summary>
        public void SetClienteId(long clienteId)
        {
            _clienteId = clienteId;
        }

        /// <summary>
        /// Al cargar el form:
        /// - Si hay Id, carga el cliente para edición.
        /// - Si no hay Id, queda en modo alta.
        /// </summary>
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (_clienteId.HasValue)
            {
                Text = "Editar cliente";
                await CargarClienteAsync(_clienteId.Value);
            }
            else
            {
                Text = "Nuevo cliente";
            }
        }

        /// <summary>
        /// Carga los datos del cliente en los controles.
        /// </summary>
        private async Task CargarClienteAsync(long id)
        {
            try
            {
                var cliente = await _db.Clientes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (cliente == null)
                {
                    MessageBox.Show(
                        "El cliente seleccionado ya no existe.",
                        "Información",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }

                txtNombre.Text = cliente.Nombre;
                txtApellido.Text = cliente.Apellido;
                txtDireccion.Text = cliente.Direccion;
                txtEmail.Text = cliente.Email;
                txtTelefono.Text = cliente.Telefono;
                txtDocTipo.Text = cliente.DocTipo;
                txtDocNro.Text = cliente.DocNro;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar datos del cliente Id={Id}.", id);
                MessageBox.Show(
                    "Ocurrió un error al cargar los datos del cliente.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        /// <summary>
        /// Valida los campos del formulario antes de guardar.
        /// </summary>
        private bool Validar()
        {
            // === Nombre obligatorio ===
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return false;
            }

            // === Apellido obligatorio ===
            if (string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("El apellido es obligatorio.", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtApellido.Focus();
                return false;
            }

            // === Email: opcional, pero si está debe ser válido ===
            if (!string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                var email = txtEmail.Text.Trim();
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    if (addr.Address != email)
                        throw new Exception();
                }
                catch
                {
                    MessageBox.Show(
                        "El email ingresado no tiene un formato válido.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return false;
                }
            }

            // === Teléfono: opcional; dígitos, espacios, +, - ===
            if (!string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                var tel = txtTelefono.Text.Trim();

                bool validoTelefono = tel.All(c =>
                    char.IsDigit(c) ||
                    c == ' ' ||
                    c == '+' ||
                    c == '-');

                if (!validoTelefono)
                {
                    MessageBox.Show(
                        "El teléfono solo puede contener números, espacios, '+' o '-'.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtTelefono.Focus();
                    return false;
                }
            }

            // === Documento: consistencia entre tipo y número ===
            bool tieneTipo = !string.IsNullOrWhiteSpace(txtDocTipo.Text);
            bool tieneNro = !string.IsNullOrWhiteSpace(txtDocNro.Text);

            if (tieneTipo && !tieneNro)
            {
                MessageBox.Show(
                    "Ingresaste un tipo de documento, pero falta el número.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtDocNro.Focus();
                return false;
            }

            if (!tieneTipo && tieneNro)
            {
                MessageBox.Show(
                    "Ingresaste un número de documento, pero falta el tipo (DNI, CI, PAS...).",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtDocTipo.Focus();
                return false;
            }

            // === DocTipo: si está, solo letras ===
            if (tieneTipo)
            {
                var tipo = txtDocTipo.Text.Trim();
                if (!tipo.All(char.IsLetter))
                {
                    MessageBox.Show(
                        "El tipo de documento solo debe contener letras (ej: DNI, CI, PAS).",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtDocTipo.Focus();
                    return false;
                }
            }

            // === DocNro: si está, solo números ===
            if (tieneNro)
            {
                var nro = txtDocNro.Text.Trim();
                if (!nro.All(char.IsDigit))
                {
                    MessageBox.Show(
                        "El número de documento debe contener solo números.",
                        "Validación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    txtDocNro.Focus();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Botón Guardar: alta o edición según corresponda.
        /// </summary>
        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Validar())
                return;

            try
            {
                btnGuardar.Enabled = false;

                if (_clienteId.HasValue)
                {
                    // ================== MODO EDICIÓN ==================
                    var cliente = await _db.Clientes
                        .FirstOrDefaultAsync(c => c.Id == _clienteId.Value);

                    if (cliente == null)
                    {
                        MessageBox.Show(
                            "El cliente que se intenta editar ya no existe en la base de datos.",
                            "Información",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        DialogResult = DialogResult.Cancel;
                        Close();
                        return;
                    }

                    cliente.Nombre = txtNombre.Text.Trim();
                    cliente.Apellido = txtApellido.Text.Trim();
                    cliente.Direccion = string.IsNullOrWhiteSpace(txtDireccion.Text)
                                            ? null
                                            : txtDireccion.Text.Trim();
                    cliente.Email = string.IsNullOrWhiteSpace(txtEmail.Text)
                                            ? null
                                            : txtEmail.Text.Trim();
                    cliente.Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text)
                                            ? null
                                            : txtTelefono.Text.Trim();
                    cliente.DocTipo = string.IsNullOrWhiteSpace(txtDocTipo.Text)
                                            ? null
                                            : txtDocTipo.Text.Trim().ToUpper();
                    cliente.DocNro = string.IsNullOrWhiteSpace(txtDocNro.Text)
                                            ? null
                                            : txtDocNro.Text.Trim();

                    await _db.SaveChangesAsync();

                    _logger.LogInformation(
                        "Cliente actualizado correctamente. Id={Id}, Nombre={Nombre}, Apellido={Apellido}",
                        cliente.Id, cliente.Nombre, cliente.Apellido);
                }
                else
                {
                    // ================== MODO ALTA ==================
                    var cliente = new Cliente
                    {
                        Nombre = txtNombre.Text.Trim(),
                        Apellido = txtApellido.Text.Trim(),
                        Direccion = string.IsNullOrWhiteSpace(txtDireccion.Text)
                                        ? null
                                        : txtDireccion.Text.Trim(),
                        Email = string.IsNullOrWhiteSpace(txtEmail.Text)
                                        ? null
                                        : txtEmail.Text.Trim(),
                        Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text)
                                        ? null
                                        : txtTelefono.Text.Trim(),
                        DocTipo = string.IsNullOrWhiteSpace(txtDocTipo.Text)
                                        ? null
                                        : txtDocTipo.Text.Trim().ToUpper(),
                        DocNro = string.IsNullOrWhiteSpace(txtDocNro.Text)
                                        ? null
                                        : txtDocNro.Text.Trim()
                    };

                    _db.Clientes.Add(cliente);
                    await _db.SaveChangesAsync();

                    _logger.LogInformation(
                        "Cliente creado correctamente. Id={Id}, Nombre={Nombre}, Apellido={Apellido}",
                        cliente.Id, cliente.Nombre, cliente.Apellido);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar el cliente.");
                MessageBox.Show(
                    "Ocurrió un error al guardar el cliente.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
