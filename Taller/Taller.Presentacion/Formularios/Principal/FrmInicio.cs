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
    private void ActualizarReloj() => lblReloj.Text = DateTime.Now.ToString("dddd dd 'de' MMMM 'de' yyyy\nHH:mm:ss", System.Globalization.CultureInfo.GetCultureInfo("es-AR"));
    private async void BtnActualizar_Click(object? sender, EventArgs e) => await CargarAsync();
    private async Task CargarAsync()
    {
        if (OperacionEnCurso)
            return;
        try
        {
            OperacionEnCurso = true;
            btnActualizar.Enabled = false;
            LimpiarTarjetas();
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
            MostrarTarjetas(documentos);
            var pendientes = await _inicio.ListarPendientesAsync();
            dgvActividad.DataSource = pendientes;
            dgvActividad.Visible = pendientes.Count > 0;
            lblSinPendientes.Visible = pendientes.Count == 0;
            lblSinPendientes.Text = usuario?.RolId == RolesSistema.AdministradorId
                ? "Sin accesos rechazados en los últimos 7 días. Los accesos correctos pueden consultarse en Auditoría de accesos."
                : "No hay tareas pendientes para su perfil. Actualice el resumen después de registrar operaciones.";
            dgvActividad.ClearSelection();
            lblEstado.Text = $"Actualizado {DateTime.Now:dd/MM/yyyy HH:mm:ss} · Indicadores: mes actual; saldos y carga: situación actual. Pendientes sin filtro de fecha. Detalle en Reportes.";
        }
        catch (Exception ex)
        {
            LimpiarTarjetas();
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

    private void LimpiarTarjetas()
    {
        while (pnlTarjetas.Controls.Count > 0)
            pnlTarjetas.Controls[0].Dispose();
    }

    private void MostrarTarjetas(IEnumerable<ReporteDocumentoDto> documentos)
    {
        Color[] colores = [Color.FromArgb(30, 64, 175), Color.FromArgb(4, 120, 87), Color.FromArgb(146, 64, 14)];
        int indice = 0;
        foreach (var documento in documentos)
        {
            var tarjeta = new Panel { Width = 300, Height = 225, BackColor = Color.White, Margin = new Padding(0, 0, 12, 12), Padding = new Padding(14) };
            var titulo = new Label { Dock = DockStyle.Top, Height = 48, Text = documento.Titulo, Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = colores[indice++ % colores.Length] };
            var contenido = new Label { Dock = DockStyle.Fill, AutoEllipsis = true, Font = new Font("Segoe UI", 11F), Text = string.Join(Environment.NewLine, documento.Indicadores.Select(i => $"{i.Nombre}: {i.Valor}")) };
            tarjeta.Controls.Add(contenido);
            tarjeta.Controls.Add(titulo);
            pnlTarjetas.Controls.Add(tarjeta);
        }
        AjustarTarjetas();
    }

    private void PnlTarjetas_SizeChanged(object? sender, EventArgs e) => AjustarTarjetas();
    private void AjustarTarjetas()
    {
        int ancho = Math.Max(220, (pnlTarjetas.ClientSize.Width - 42) / 3);
        foreach (Control tarjeta in pnlTarjetas.Controls)
            tarjeta.Width = ancho;
    }

    private void FrmInicio_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (OperacionEnCurso)
            e.Cancel = true;
    }
}
