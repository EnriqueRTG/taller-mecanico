using System.ComponentModel;
using Taller.Aplicacion.DTOs.Ejecuciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Enumeraciones;

namespace Taller.Presentacion.Formularios.Atenciones;
/// <summary>Registra el trabajo real de los ítems aceptados; conserva avances y reserva técnica.</summary>
public partial class FrmEjecutarAtencion : Form
{
    private readonly IServiceProvider _proveedor;
    private readonly int _id;
    private readonly EjecucionServicio _servicio;
    private readonly SesionUsuario _sesion;
    private EjecucionAtencionDto? _datos;
    private BindingList<EjecucionItemDto> _items = [];
    private bool _ocupado, _sucio, _cargando;
    public FrmEjecutarAtencion(int id, EjecucionServicio servicio, SesionUsuario sesion, IServiceProvider proveedor)
    {
        InitializeComponent();
        _proveedor = proveedor;
        _id = id;
        _servicio = servicio;
        _sesion = sesion;
    }

    private async void Cargar(object? s, EventArgs e)
    {
        Taller.Presentacion.Estilos.DisposicionVentanas.AjustarAlAreaDisponible(this);
        await RecargarAsync();
    }

    private async Task RecargarAsync()
    {
        _cargando = true;
        tlpPrincipal.Enabled = false;
        try
        {
            _datos = await _servicio.ObtenerAsync(_id);
            txtResumen.Text = _datos.Resumen;
            _items = new BindingList<EjecucionItemDto>(_datos.Items);
            dgvItems.DataSource = _items;
            var propia = _datos.Estado == EstadoAtencion.EnEjecucion && _datos.IdTecnico == _sesion.UsuarioActual?.Id;
            dgvItems.ReadOnly = !propia;
            btnGuardar.Enabled = btnFinalizar.Enabled = btnInterrumpir.Enabled = btnLiberar.Enabled = propia;
            btnTomar.Enabled = _datos.Estado == EstadoAtencion.EnEjecucion && _datos.IdTecnico == null;
            lblAviso.Text = _datos.TieneExclusiones ? "El cliente excluyó ítems. Antes de tomar confirme que el alcance restante es viable y explique su revisión abajo." : "Revise el alcance antes de tomar. Solo se muestran ítems aceptados. Cerrar conserva su reserva.";
            _sucio = false;
        }
        catch (Exception ex)
        {
            Error(ex);
        }
        finally
        {
            _cargando = false;
            tlpPrincipal.Enabled = true;
        }
    }

    private async void Tomar(object? s, EventArgs e)
    {
        if (_ocupado || _datos == null)
            return;
        if (MessageBox.Show(this, "¿Validó que puede ejecutar de forma adecuada el alcance aceptado, incluidas las exclusiones del cliente?", "Tomar ejecución", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        await OperarAsync(() => _servicio.TomarAsync(_id, _datos.Version, txtObservaciones.Text));
    }

    private async void Liberar(object? s, EventArgs e)
    {
        if (_ocupado || _datos == null)
            return;
        if (_sucio && MessageBox.Show(this, "Hay cambios sin guardar. ¿Descartarlos y liberar la reserva?", "Liberar", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;
        await OperarAsync(() => _servicio.LiberarAsync(_id, _datos.Version));
    }

    private async Task OperarAsync(Func<Task> accion)
    {
        _ocupado = true;
        tlpPrincipal.Enabled = false;
        try
        {
            await accion();
            await RecargarAsync();
        }
        catch (Exception ex)
        {
            Error(ex);
        }
        finally
        {
            _ocupado = false;
            tlpPrincipal.Enabled = true;
        }
    }

    private void CeldaCambiada(object? s, DataGridViewCellEventArgs e)
    {
        if (!_cargando && e.RowIndex >= 0)
            _sucio = true;
    }

    private void TextoCambiado(object? s, EventArgs e)
    {
        if (!_cargando)
            _sucio = true;
    }

    private void ErrorCelda(object? s, DataGridViewDataErrorEventArgs e)
    {
        e.ThrowException = false;
        MessageBox.Show(this, "Ingrese cantidades decimales válidas y un estado de la lista.");
    }

    private async void Guardar(object? s, EventArgs e) => await PersistirAsync(false);
    private async void Finalizar(object? s, EventArgs e) => await PersistirAsync(true);
    private async void Interrumpir(object? s, EventArgs e) => await PersistirAsync(true, true);
    private async Task PersistirAsync(bool finalizar, bool interrumpir = false)
    {
        if (_ocupado || _datos == null)
            return;
        if (!ValidateChildren() || !dgvItems.EndEdit())
            return;
        if (dgvItems.DataSource != null)
            BindingContext?[dgvItems.DataSource]?.EndCurrentEdit();
        if (finalizar && MessageBox.Show(this, interrumpir ? "¿Terminar como reparación interrumpida? Solo se liquidará lo autorizado y efectivamente realizado. Debe explicar la causa." : "¿Finalizar el trabajo? Resuelva todos los ítems como completados u omitidos con motivo.", "Terminar ejecución", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;
        _datos.Items = _items.ToList();
        _datos.Observaciones = txtObservaciones.Text;
        await OperarAsync(() => _servicio.GuardarAsync(_datos, finalizar, interrumpir));
    }

    private async void Seguimiento(object? s, EventArgs e)
    {
        if (_ocupado || _cargando)
            return;
        if (_sucio)
        {
            MessageBox.Show(this, "Guarde el avance o vuelva a abrir la ejecución antes de modificar el seguimiento.");
            return;
        }

        _ocupado = true;
        try
        {
            using var f = new FrmSeguimientoTrabajo(_id, Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<SeguimientoTrabajoServicio>(_proveedor), Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<EvaluacionServicio>(_proveedor), Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<PresupuestoServicio>(_proveedor));
            f.ShowDialog(this);
            await RecargarAsync();
        }
        catch (Exception ex)
        {
            Error(ex);
        }
        finally
        {
            _ocupado = false;
        }
    }

    private void Cerrar(object? s, EventArgs e) => Close();
    private void AlCerrar(object? s, FormClosingEventArgs e)
    {
        if (_ocupado || _cargando)
        {
            e.Cancel = true;
            return;
        }

        if (_sucio && MessageBox.Show(this, "¿Descartar los cambios sin guardar? La reserva y los avances guardados se conservan.", "Cerrar", MessageBoxButtons.YesNo) != DialogResult.Yes)
            e.Cancel = true;
    }

    private void Error(Exception ex) => Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Ejecución");
}
