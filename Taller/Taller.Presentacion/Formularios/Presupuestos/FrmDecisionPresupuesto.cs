using System.ComponentModel;
using Taller.Aplicacion.DTOs.Presupuestos;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Enumeraciones;

namespace Taller.Presentacion.Formularios.Presupuestos;
/// <summary>Recepción registra el alcance comunicado por el cliente, conservando cada ítem original.</summary>
public partial class FrmDecisionPresupuesto : Form
{
    private readonly PresupuestoServicio? _servicio;
    private readonly int _id;
    private PresupuestoGestionDto? _datos;
    private BindingList<DecisionItemDto> _items = [];
    private bool _ocupado, _hayCambios;
    public FrmDecisionPresupuesto()
    {
        InitializeComponent();
    }

    public FrmDecisionPresupuesto(int id, PresupuestoServicio servicio) : this()
    {
        _id = id;
        _servicio = servicio;
    }

    private async void Cargar(object? s, EventArgs e)
    {
        _ocupado = true;
        tlpPrincipal.Enabled = false;
        try
        {
            _datos = await (_servicio ?? throw new InvalidOperationException("Seleccione una propuesta real.")).ObtenerAsync(_id);
            if (!_datos.Vigente || _datos.Estado != EstadoPresupuesto.PendienteDecision)
                throw new InvalidOperationException("La propuesta no está esperando una decisión.");
            lblResumen.Text = $"Presupuesto {_id:D6} · {_datos.Resumen}\nQuite la marca de los ítems que el cliente NO acepta. Nada se elimina del documento original.";
            _items = new BindingList<DecisionItemDto>(_datos.Items);
            dgvItems.DataSource = _items;
            ActualizarTotal();
        }
        catch (Exception ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Cargar decisión");
            _ocupado = false;
            Close();
        }
        finally
        {
            _ocupado = false;
            tlpPrincipal.Enabled = true;
        }
    }

    private void CommitCelda(object? s, EventArgs e)
    {
        if (dgvItems.IsCurrentCellDirty)
            dgvItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void Cambio(object? s, DataGridViewCellEventArgs e)
    {
        if (!_ocupado && e.RowIndex >= 0)
            _hayCambios = true;
        ActualizarTotal();
    }

    private void TextoCambiado(object? s, EventArgs e)
    {
        if (!_ocupado)
            _hayCambios = true;
    }

    private void ActualizarTotal() => lblTotal.Text = $"Aceptados: {_items.Count(x => x.Aceptar)}/{_items.Count} · Total aceptado: {_items.Where(x => x.Aceptar).Sum(x => x.Subtotal):N2}. Sin marcas equivale a rechazo completo.";
    private void RechazarTodo(object? s, EventArgs e)
    {
        dgvItems.EndEdit();
        foreach (var i in _items)
            i.Aceptar = false;
        _items.ResetBindings();
        ActualizarTotal();
    }

    private async void Guardar(object? s, EventArgs e)
    {
        if (_ocupado || _datos == null || _servicio == null)
            return;
        dgvItems.EndEdit();
        if (MessageBox.Show(this, "¿Registrar esta decisión del cliente? Una selección parcial queda pendiente de revisión técnica y nueva aceptación; todavía no autoriza trabajos.", "Registrar decisión", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        _ocupado = true;
        tlpPrincipal.Enabled = false;
        try
        {
            await _servicio.DecidirAsync(new DecisionPresupuestoDto { IdPresupuesto = _id, Version = _datos.Version, ItemsAceptados = _items.Where(x => x.Aceptar).Select(x => x.IdDetalle).ToList(), Observaciones = txtMotivo.Text });
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Decisión");
        }
        finally
        {
            _ocupado = false;
            tlpPrincipal.Enabled = true;
        }

        if (DialogResult == DialogResult.OK)
            Close();
    }

    private void Cerrar(object? s, EventArgs e) => Close();
    private void AlCerrar(object? s, FormClosingEventArgs e)
    {
        if (_ocupado)
        {
            e.Cancel = true;
            return;
        }

        if (_hayCambios && DialogResult != DialogResult.OK && MessageBox.Show(this, "¿Descartar la decisión preparada sin guardarla?", "Cerrar decisión", MessageBoxButtons.YesNo) != DialogResult.Yes)
            e.Cancel = true;
    }
}
