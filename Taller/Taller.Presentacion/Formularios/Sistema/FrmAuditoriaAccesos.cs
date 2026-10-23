using Taller.Aplicacion.Servicios;
using Taller.Presentacion.Navegacion;

namespace Taller.Presentacion.Formularios.Sistema;
/// <summary>Consulta administrativa de autenticaciones reales, sin credenciales.</summary>
public partial class FrmAuditoriaAccesos : Form, IFormularioConOperacion
{
    private readonly AuditoriaAccesoServicio _servicio;
    public bool OperacionEnCurso { get; private set; }

    public FrmAuditoriaAccesos(AuditoriaAccesoServicio servicio)
    {
        InitializeComponent();
        _servicio = servicio;
    }

    private async void Formulario_Load(object? sender, EventArgs e)
    {
        desde.Value = DateTime.Today.AddDays(-7);
        hasta.Value = DateTime.Today;
        resultado.SelectedIndex = 0;
        await CargarAsync();
    }

    private async void Consultar_Click(object? sender, EventArgs e) => await CargarAsync();
    private async Task CargarAsync()
    {
        if (OperacionEnCurso)
            return;
        try
        {
            OperacionEnCurso = true;
            consultar.Enabled = false;
            var datos = await _servicio.ConsultarAsync(desde.Value, hasta.Value, resultado.SelectedIndex == 0 ? null : resultado.SelectedIndex == 1);
            grilla.DataSource = datos;
            estado.Text = $"{datos.Count} accesos. Máximo 500 por consulta; acote el período si alcanza ese límite. Fechas de eventos en UTC. No se registran contraseñas.";
        }
        catch (Exception ex)
        {
            grilla.DataSource = null;
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Auditoría de accesos");
        }
        finally
        {
            OperacionEnCurso = false;
            consultar.Enabled = true;
        }
    }

    private void Formulario_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (OperacionEnCurso)
            e.Cancel = true;
    }
}
