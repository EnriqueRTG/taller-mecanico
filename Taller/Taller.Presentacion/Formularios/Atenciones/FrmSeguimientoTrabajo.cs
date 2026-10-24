using Taller.Aplicacion.DTOs.Ejecuciones;
using Taller.Aplicacion.Servicios;
using Taller.Presentacion.Errores;
using Taller.Presentacion.Formularios.Compartidos;
using Taller.Presentacion.Formularios.Presupuestos;
using Taller.Dominio.Enumeraciones;

namespace Taller.Presentacion.Formularios.Atenciones;
/// <summary>Expediente real compartido: hallazgos, pausas y materiales. Los servicios vuelven a comprobar permisos y versiones.</summary>
public partial class FrmSeguimientoTrabajo : Form
{
    private readonly int _id;
    private readonly Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto _empresa;
    private readonly SeguimientoTrabajoServicio _servicio;
    private readonly EvaluacionServicio _evaluacion;
    private readonly PresupuestoServicio _presupuestos;
    private SeguimientoTrabajoDto? _datos;
    private bool _ocupado;
    public FrmSeguimientoTrabajo(int id, SeguimientoTrabajoServicio servicio, EvaluacionServicio evaluacion, PresupuestoServicio presupuestos, Taller.Aplicacion.DTOs.Reportes.EmpresaDocumentoDto empresa)
    {
        InitializeComponent();
        _id = id;
        _empresa = empresa;
        _servicio = servicio;
        _evaluacion = evaluacion;
        _presupuestos = presupuestos;
    }

    private async void Cargar(object? s, EventArgs e)
    {
        Taller.Presentacion.Estilos.DisposicionVentanas.AjustarAlAreaDisponible(this);
        await OperarAsync(() => Task.CompletedTask);
    }

    private async Task RecargarAsync()
    {
        _datos = null;
        dgvHallazgos.DataSource = null;
        dgvPausas.DataSource = null;
        dgvMateriales.DataSource = null;
        btnHallazgo.Enabled = btnAdicional.Enabled = btnResolver.Enabled = btnRecibir.Enabled = btnPausar.Enabled = false;
        _datos = await _servicio.ObtenerAsync(_id);
        lblResumen.Text = _datos.Resumen + " · Seleccione una tarea o Toda la intervención. Un hallazgo no autoriza cargos.";
        dgvHallazgos.DataSource = _datos.Hallazgos;
        dgvPausas.DataSource = _datos.Pausas;
        dgvMateriales.DataSource = _datos.Items.Where(i => i.Origen == OrigenMaterial.Cliente).ToList();
        cboTarea.DisplayMember = "Etiqueta";
        cboTarea.ValueMember = "IdDetalle";
        cboTarea.DataSource = new List<MaterialTrabajoDto>
        {
            new()
            {
                IdDetalle = 0,
                Descripcion = "Toda la intervención"
            }
        }.Concat(_datos.Items).ToList();
        btnPausar.Enabled = _datos.PuedePausar;
        btnHallazgo.Enabled = btnAdicional.Enabled = btnResolver.Enabled = btnRecibir.Enabled = _datos.PuedeRegistrar;
        dgvHallazgos.ClearSelection();
        dgvHallazgos.CurrentCell = null;
        dgvPausas.ClearSelection();
        dgvPausas.CurrentCell = null;
        dgvMateriales.ClearSelection();
        dgvMateriales.CurrentCell = null;
    }

    private int? Tarea => cboTarea.SelectedItem is MaterialTrabajoDto { IdDetalle: > 0 } i ? i.IdDetalle : null;

    private async Task OperarAsync(Func<Task> accion)
    {
        if (_ocupado)
            return;
        _ocupado = true;
        tlpPrincipal.Enabled = false;
        try
        {
            await accion();
            await RecargarAsync();
        }
        catch (Exception ex)
        {
            ErroresInterfaz.Mostrar(this, ex, "Seguimiento técnico");
        }
        finally
        {
            _ocupado = false;
            tlpPrincipal.Enabled = true;
        }
    }

    private async void Actualizar(object? s, EventArgs e) => await OperarAsync(() => Task.CompletedTask);
    private async void Hallazgo(object? s, EventArgs e)
    {
        if (_datos == null || _ocupado)
            return;
        await OperarAsync(async () =>
        {
            await _servicio.RegistrarHallazgoAsync(new() { IdAtencion = _id, Version = _datos.Version, IdDetalle = Tarea, Descripcion = txtDescripcion.Text, Recomendacion = txtRecomendacion.Text, ImpideContinuar = chkImpide.Checked });
            txtDescripcion.Clear();
            txtRecomendacion.Clear();
            chkImpide.Checked = false;
        });
    }

    private async void Pausar(object? s, EventArgs e)
    {
        if (_datos == null || _ocupado)
            return;
        using var f = new FrmSolicitarMotivo("Pausar trabajo", "Indique la causa: espera de repuesto, solicitud de detener, autorización pendiente u otra. La pausa no borra lo ejecutado ni cancela lo autorizado.");
        if (f.ShowDialog(this) == DialogResult.OK)
            await OperarAsync(() => _servicio.PausarAsync(_id, _datos.Version, Tarea, f.Motivo));
    }

    private async void Resolver(object? s, EventArgs e)
    {
        if (_datos == null || _ocupado || dgvPausas.CurrentRow?.DataBoundItem is not PausaConsultaDto p || p.FechaResolucion != null)
            return;
        using var f = new FrmSolicitarMotivo("Resolver pausa bajo responsabilidad técnica", "Explique si ya puede retomar, continuar solo otras tareas o terminar con interrupción justificada. Resolver la pausa no ejecuta ni autoriza trabajos adicionales.");
        if (f.ShowDialog(this) == DialogResult.OK)
            await OperarAsync(() => _servicio.ResolverPausaAsync(_id, _datos.Version, p.Id, f.Motivo));
    }

    private void MaterialSeleccionado(object? s, EventArgs e)
    {
        if (dgvMateriales.CurrentRow?.DataBoundItem is MaterialTrabajoDto d)
        {
            nudRecibido.Value = 0;
            nudRecibido.Maximum = d.Cantidad;
            nudRecibido.Value = d.Recibida;
            chkConforme.Checked = d.Conforme;
            txtMaterial.Text = d.Observaciones;
        }
    }

    private async void Recibir(object? s, EventArgs e)
    {
        if (_datos == null || _ocupado || dgvMateriales.CurrentRow?.DataBoundItem is not MaterialTrabajoDto d)
            return;
        await OperarAsync(() => _servicio.RecibirMaterialAsync(_id, _datos.Version, d.IdDetalle, nudRecibido.Value, chkConforme.Checked, txtMaterial.Text));
    }

    private async void Adicional(object? s, EventArgs e)
    {
        if (_datos == null || _ocupado)
            return;
        int? hallazgo = dgvHallazgos.CurrentRow?.DataBoundItem is HallazgoConsultaDto h ? h.Id : null;
        _ocupado = true;
        try
        {
            using var f = new FrmPrepararPropuesta(_id, _servicio, _evaluacion, _presupuestos, hallazgo: hallazgo);
            f.ShowDialog(this);
            await RecargarAsync();
        }
        catch (Exception ex)
        {
            ErroresInterfaz.Mostrar(this, ex, "Preparar adicional");
        }
        finally
        {
            _ocupado = false;
        }
    }

    private void Imprimir(object? s, EventArgs e)
    {
        if (_datos == null || _ocupado)
            return;
        try
        {
            Taller.Presentacion.Impresion.ImpresionDocumento.Mostrar(this, Taller.Presentacion.Impresion.TextoIntervencion.Generar(_datos), _empresa, Taller.Aplicacion.Reportes.DocumentoReporteExportador.NombreArchivo("Intervencion-A-" + _id, Taller.Dominio.Tiempo.FechaHoraTaller.Ahora));
        }
        catch (Exception ex)
        {
            ErroresInterfaz.Mostrar(this, ex, "Informe de intervención");
        }
    }

    private void Cerrar(object? s, EventArgs e) => Close();
    private void AlCerrar(object? s, FormClosingEventArgs e)
    {
        if (_ocupado)
            e.Cancel = true;
        else if (txtDescripcion.TextLength > 0 && MessageBox.Show(this, "¿Cerrar sin registrar el hallazgo escrito?", "Cerrar", MessageBoxButtons.YesNo) != DialogResult.Yes)
            e.Cancel = true;
    }
}
