using Taller.Aplicacion.DTOs.Reportes;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Presentacion.Navegacion;

namespace Taller.Presentacion.Formularios.Principal;
/// <summary>Resumen real por rol, con período y alcance explícitos; comparte las definiciones de los reportes.</summary>
public partial class FrmInicio : Form, IFormularioConOperacion
{
    private readonly ReporteServicio _reportes;
    private readonly InicioServicio _inicio;
    private readonly SesionUsuario _sesion;
    public bool OperacionEnCurso { get; private set; }

    public FrmInicio(ReporteServicio reportes, SesionUsuario sesion, InicioServicio inicio)
    {
        InitializeComponent();
        _reportes = reportes;
        _inicio = inicio;
        _sesion = sesion;
    }

    private async void FrmInicio_Load(object? sender, EventArgs e)
    {
        ActualizarReloj();
        reloj.Start();
        await CargarAsync();
    }

    private void Reloj_Tick(object? sender, EventArgs e) => ActualizarReloj();
    private void ActualizarReloj() => lblReloj.Text = DateTime.Now.ToString("dddd dd 'de' MMMM 'de' yyyy · HH:mm:ss", System.Globalization.CultureInfo.GetCultureInfo("es-AR"));
    private async void BtnActualizar_Click(object? sender, EventArgs e) => await CargarAsync();
    private async Task CargarAsync()
    {
        if (OperacionEnCurso)
            return;
        try
        {
            OperacionEnCurso = true;
            btnActualizar.Enabled = false;
            txtIndicadores.Clear();
            dgvActividad.DataSource = null;
            var tipos = await _reportes.ListarTiposAsync();
            var usuario = _sesion.UsuarioActual;
            lblBienvenida.Text = $"Bienvenido, {usuario?.Nombre} {usuario?.Apellido}";
            lblDescripcion.Text = $"Perfil: {usuario?.Rol?.Nombre} · Usuario: {usuario?.NombreUsuario}. Pendientes priorizados según su intervención.";
            var seleccion = tipos.Contains(TipoReporte.MovimientosCobro) ? new[]
            {
                TipoReporte.Atenciones,
                TipoReporte.MovimientosCobro,
                TipoReporte.SaldosPendientes
            }

            : new[]
            {
                TipoReporte.CargaTecnica,
                TipoReporte.ActividadTecnica,
                TipoReporte.Presupuestos
            };
            var documentos = new List<ReporteDocumentoDto>();
            foreach (var tipo in seleccion)
                documentos.Add(await _reportes.GenerarAsync(new SolicitudReporteDto { Tipo = tipo, Desde = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), Hasta = DateTime.Today }));
            txtIndicadores.Text = string.Join(Environment.NewLine + Environment.NewLine, documentos.Select(d => d.Titulo + Environment.NewLine + string.Join("   |   ", d.Indicadores.Select(i => i.Nombre + ": " + i.Valor))));
            dgvActividad.DataSource = await _inicio.ListarPendientesAsync();
            dgvActividad.ClearSelection();
            lblEstado.Text = $"Actualizado {DateTime.Now:dd/MM/yyyy HH:mm:ss} · Indicadores: mes actual; saldos y carga: situación actual. Pendientes sin filtro de fecha. Detalle en Reportes.";
        }
        catch (Exception ex)
        {
            txtIndicadores.Clear();
            dgvActividad.DataSource = null;
            lblEstado.Text = "No se pudieron actualizar los datos. Presione Actualizar para reintentar.";
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Inicio");
        }
        finally
        {
            OperacionEnCurso = false;
            btnActualizar.Enabled = true;
        }
    }

    private void FrmInicio_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (OperacionEnCurso)
            e.Cancel = true;
    }
}
