// MainForm.cs en Taller.App
// Formulario principal. Se inyectan dependencias por constructor (cuando existan servicios).
// Mantener lógica de negocio fuera de la UI (llamar a servicios).

namespace Taller.App
{
    public partial class MainForm : Form
    {
        // Ejemplo: más adelante podés inyectar IClienteService, ILogger<MainForm>, etc.
        // private readonly IClienteService _clientes;
        // private readonly ILogger<MainForm> _logger;

        public MainForm(/*IClienteService clientes, ILogger<MainForm> logger*/)
        {
            InitializeComponent();

            // _clientes = clientes;
            // _logger = logger;
        }

        // Manejar eventos de botones/menú para abrir otras pantallas (Clientes, Vehículos, OT, etc.)
    }
}
