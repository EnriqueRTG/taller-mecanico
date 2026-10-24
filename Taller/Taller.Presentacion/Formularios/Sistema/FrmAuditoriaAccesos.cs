using Taller.Dominio.Tiempo;
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
        desde.Value = FechaHoraTaller.Hoy.AddDays(-7);
        hasta.Value = FechaHoraTaller.Hoy;
        resultado.SelectedIndex = 0;
        await CargarAsync();
    }

    private async void LimpiarFiltros_Click(object? sender, EventArgs e)
    {
        if (OperacionEnCurso) return;
        desde.Value = FechaHoraTaller.Hoy.AddDays(-7);
        hasta.Value = FechaHoraTaller.Hoy;
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
            consultar.Enabled = btnLimpiarFiltros.Enabled = false;
            var datos = await _servicio.ConsultarAsync(desde.Value, hasta.Value, resultado.SelectedIndex == 0 ? null : resultado.SelectedIndex == 1);
            grilla.DataSource = datos.Select(x => new { x.Id, FechaLocal = FechaHoraTaller.DesdeUtc(x.FechaUtc).DateTime, x.Usuario, x.Correcto }).ToList();
            estado.Text = $"{datos.Count} accesos. Máximo 500 por consulta; acote el período si alcanza ese límite. Fechas en hora de Argentina; se almacenan en UTC. No se registran contraseñas.";
        }
        catch (Exception ex)
        {
            grilla.DataSource = null;
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Auditoría de accesos");
        }
        finally
        {
            OperacionEnCurso = false;
            consultar.Enabled = btnLimpiarFiltros.Enabled = true;
        }
    }

    private void Formulario_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (OperacionEnCurso)
            e.Cancel = true;
    }
}
