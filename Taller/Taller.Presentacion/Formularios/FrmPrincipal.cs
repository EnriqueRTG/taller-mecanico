using System;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;


namespace Taller.Presentacion.Formularios
{
    /// <summary>
    /// Formulario principal de la aplicación de escritorio.
    /// Actúa como “shell”/contenedor: muestra el menú lateral y un área central
    /// donde se cargarán las pantallas de Clientes, Vehículos, Órdenes, etc.
    /// 
    /// Este formulario se construye mediante DI (inyectado desde Program.cs),
    /// lo que permite recibir servicios como ILogger, servicios de dominio, etc.
    /// </summary>
    public partial class FrmPrincipal : Form
    {
        private readonly ILogger<FrmPrincipal> _logger;
        private readonly IServiceProvider _serviceProvider;   
        private Form? _formActual;

        /// <summary>
        /// Constructor principal. Recibe dependencias por inyección (DI).
        /// </summary>
        /// <param name="logger">
        /// Instancia de <see cref="ILogger{MainForm}"/> para registro de eventos.
        /// </param>
        public FrmPrincipal(ILogger<FrmPrincipal> logger, IServiceProvider serviceProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            InitializeComponent();   // Inicializa los controles generados por el diseñador.
            ConfigureUi();           // Ajustes de UI que no dependen del diseñador.

            _logger.LogInformation("MainForm inicializado correctamente.");
        }

        /// <summary>
        /// Ajustes de apariencia y comportamiento que complementan al diseñador.
        /// </summary>
        private void ConfigureUi()
        {
            Text = "Sistema de Gestión de Taller Mecánico";

            // Ejemplo: establecer texto inicial del header.
            lblTitulo.Text = "Panel principal";

            // Si más adelante tenés usuario logueado, podés mostrarlo en el header
            // o en el menú lateral, etc.
        }

        /// <summary>
        /// Carga un formulario dentro del panel central (`panelContent`),
        /// utilizando el contenedor de DI para resolverlo.
        /// </summary>
        /// <typeparam name="TForm">Tipo de formulario a mostrar.</typeparam>
        private void MostrarEnPanel<TForm>() where TForm : Form
        {
            // Cerrar y liberar el formulario anterior, si lo hubiera.
            if (_formActual != null)
            {
                _formActual.Close();
                _formActual.Dispose();
                _formActual = null;
            }

            // Resolver el formulario desde DI.
            var form = _serviceProvider.GetRequiredService<TForm>();

            // Configuración para usarlo como "form embebido" dentro del panel.
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;

            panelContent.Controls.Clear();
            panelContent.Controls.Add(form);

            _formActual = form;

            form.Show();
        }


        #region Manejo de navegación (botones menú lateral)

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            _logger.LogInformation("Navegación: Dashboard (inicio).");

            // Cerrar y liberar el formulario actual si existe
            if (_formActual != null)
            {
                _formActual.Close();
                _formActual.Dispose();
                _formActual = null;
            }

            // Limpiar el área central
            panelContent.Controls.Clear();

            // Actualizar título
            lblTitulo.Text = "Panel principal";
       
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            _logger.LogInformation("Navegación: Clientes.");
            lblTitulo.Text = "Gestión de clientes";
            // TODO: abrir formulario/UC de Clientes dentro de panelContent.
            MostrarEnPanel<FrmClientes>();   // <-- ahora realmente abre la pantalla

        }

        private void btnVehiculos_Click(object sender, EventArgs e)
        {
            _logger.LogInformation("Navegación: Vehículos.");
            lblTitulo.Text = "Gestión de vehículos";
            // TODO: abrir pantalla de Vehículos.
        }

        private void btnOrdenes_Click(object sender, EventArgs e)
        {
            _logger.LogInformation("Navegación: Órdenes de trabajo.");
            lblTitulo.Text = "Órdenes de trabajo";
            // TODO: abrir pantalla de Órdenes.
        }

        private void btnServicios_Click(object sender, EventArgs e)
        {
            _logger.LogInformation("Navegación: Servicios.");
            lblTitulo.Text = "Catálogo de servicios";
            // TODO: abrir pantalla de Servicios.
        }

        private void btnRepuestos_Click(object sender, EventArgs e)
        {
            _logger.LogInformation("Navegación: Repuestos.");
            lblTitulo.Text = "Catálogo de repuestos";
            // TODO: abrir pantalla de Repuestos.
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            _logger.LogInformation("Cierre solicitado desde el botón Salir.");

            var result = MessageBox.Show(
                "¿Seguro que desea salir de la aplicación?",
                "Confirmar salida",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Close();
            }
        }

        #endregion
    }
}
