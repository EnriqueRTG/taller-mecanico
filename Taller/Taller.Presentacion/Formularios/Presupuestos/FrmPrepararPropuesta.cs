using System.ComponentModel;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.DTOs.Presupuestos;
using Taller.Aplicacion.DTOs.Ejecuciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Enumeraciones;
using Taller.Presentacion.Errores;

namespace Taller.Presentacion.Formularios.Presupuestos;
/// <summary>El técnico prepara el alcance viable; recepción deberá presentarlo y registrar nueva aceptación.</summary>
public partial class FrmPrepararPropuesta : Form
{
    private readonly int _atencion;
    private readonly int? _presupuesto;
    private readonly int? _hallazgo;
    private readonly SeguimientoTrabajoServicio _servicio;
    private readonly EvaluacionServicio _evaluacion;
    private readonly PresupuestoServicio _presupuestos;
    private PresupuestoGestionDto? _revision;
    private byte[] _version = [];
    private BindingList<ItemPropuestaDto> _items = [];
    private List<ConceptoPropuestaDto> _catalogo = [];
    private bool _ocupado = true, _sucio;
    public FrmPrepararPropuesta(int atencion, SeguimientoTrabajoServicio servicio, EvaluacionServicio evaluacion, PresupuestoServicio presupuestos, int? presupuesto = null, int? hallazgo = null)
    {
        InitializeComponent();
        _atencion = atencion;
        _servicio = servicio;
        _evaluacion = evaluacion;
        _presupuestos = presupuestos;
        _presupuesto = presupuesto;
        _hallazgo = hallazgo;
    }

    private async void Cargar(object? s, EventArgs e)
    {
        Taller.Presentacion.Estilos.DisposicionVentanas.AjustarAlAreaDisponible(this);
        tlpPrincipal.Enabled = false;
        try
        {
            _catalogo = await _evaluacion.ListarCatalogoAsync();
            _version = (await _servicio.ObtenerAsync(_atencion)).Version;
            if (_presupuesto is int id)
            {
                _revision = await _presupuestos.ObtenerAsync(id);
                if (_revision.Estado != EstadoPresupuesto.PendienteRevisionTecnica || !_revision.Vigente)
                    throw new InvalidOperationException("La selección ya no espera revisión.");
                _items = new(_revision.Items.Where(i => i.Aceptar).Select(i => new ItemPropuestaDto { IdConcepto = i.IdConcepto, Descripcion = i.Descripcion, Cantidad = i.Cantidad, PrecioUnitario = i.PrecioUnitario, OrigenMaterial = i.OrigenMaterial }).ToList());
                lblAyuda.Text = "Selección solicitada por el cliente. Revise dependencias, quite o agregue tareas y justifique la viabilidad. Se conservará el original y se requerirá una nueva aceptación. Solicitud: " + _revision.Observaciones;
            }
            else
                lblAyuda.Text = "Adicional independiente: no sustituye lo autorizado. Incluya solo trabajos/materiales nuevos. Recepción comunicará esta propuesta; no se ejecuta hasta su aceptación.";
            dgvItems.DataSource = _items;
            Filtrar(s, e);
            ActualizarTotal();
        }
        catch (Exception ex)
        {
            ErroresInterfaz.Mostrar(this, ex, "Preparar propuesta");
            _ocupado = false;
            Close();
        }
        finally
        {
            _ocupado = false;
            tlpPrincipal.Enabled = true;
        }
    }

    private void Filtrar(object? s, EventArgs e)
    {
        var q = txtBuscar.Text.Trim();
        cboConcepto.DisplayMember = nameof(ConceptoPropuestaDto.Etiqueta);
        cboConcepto.DataSource = _catalogo.Where(c => Taller.Aplicacion.Consultas.BusquedaTexto.Coincide($"{c.Nombre} {c.Categoria} {c.Descripcion}", q)).ToList();
    }

    private void SeleccionarConcepto(object? s, EventArgs e)
    {
        if (cboConcepto.SelectedItem is ConceptoPropuestaDto c)
        {
            txtDescripcion.Text = c.Nombre;
            nudPrecio.Value = Math.Min(nudPrecio.Maximum, c.PrecioReferencia ?? 0);
        }
    }

    private void Agregar(object? s, EventArgs e)
    {
        if (cboConcepto.SelectedItem is not ConceptoPropuestaDto c || string.IsNullOrWhiteSpace(txtDescripcion.Text))
            return;
        if (_items.Any(i => i.IdConcepto == c.IdConcepto))
        {
            MessageBox.Show(this, "El concepto ya está agregado. Quite la fila para reemplazarlo.");
            return;
        }

        var origen = cboOrigen.SelectedIndex == 1 ? OrigenMaterial.Cliente : OrigenMaterial.Taller;
        if (origen == OrigenMaterial.Cliente && c.Tipo == TipoConcepto.Servicio)
        {
            MessageBox.Show(this, "El cliente puede aportar materiales, no servicios del taller.");
            return;
        }

        _items.Add(new() { IdConcepto = c.IdConcepto, Descripcion = txtDescripcion.Text.Trim(), Cantidad = nudCantidad.Value, PrecioUnitario = origen == OrigenMaterial.Cliente ? 0 : nudPrecio.Value, OrigenMaterial = origen });
        _sucio = true;
        ActualizarTotal();
    }

    private void Quitar(object? s, EventArgs e)
    {
        if (dgvItems.CurrentRow?.DataBoundItem is ItemPropuestaDto i)
        {
            _items.Remove(i);
            _sucio = true;
            ActualizarTotal();
        }
    }

    private void ActualizarTotal() => lblTotal.Text = $"Total propuesto: {_items.Sum(i => i.Subtotal):N2} ARS · Materiales del cliente sin cargo de venta.";
    private void CambiarOrigen(object? s, EventArgs e)
    {
        nudPrecio.Enabled = cboOrigen.SelectedIndex != 1;
        if (!nudPrecio.Enabled)
            nudPrecio.Value = 0;
    }

    private async void Guardar(object? s, EventArgs e)
    {
        if (_ocupado)
            return;
        if (MessageBox.Show(this, "¿Confirmar alcance técnico y enviarlo a recepción para presentación?", "Preparar propuesta", MessageBoxButtons.YesNo) != DialogResult.Yes)
            return;
        _ocupado = true;
        tlpPrincipal.Enabled = false;
        try
        {
            int id = _revision != null ? await _servicio.RevisarSeleccionAsync(_revision.IdPresupuesto, _revision.Version, _items.ToList(), txtMotivo.Text) : await _servicio.CrearAdicionalAsync(new PropuestaAdicionalDto { IdAtencion = _atencion, Version = _version, IdHallazgo = _hallazgo, Motivo = txtMotivo.Text, Items = _items.ToList() });
            MessageBox.Show(this, $"Propuesta {id:D6} preparada. Recepción debe comunicarla y registrar la decisión del cliente.");
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
            ErroresInterfaz.Mostrar(this, ex, "Guardar propuesta");
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
            e.Cancel = true;
        else if (DialogResult != DialogResult.OK && (_sucio || txtMotivo.TextLength > 0) && MessageBox.Show(this, "¿Descartar la propuesta sin guardar?", "Cerrar", MessageBoxButtons.YesNo) != DialogResult.Yes)
            e.Cancel = true;
    }
}
