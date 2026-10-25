using System.ComponentModel;
using Taller.Aplicacion.DTOs.Atenciones;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;

namespace Taller.Presentacion.Formularios.Atenciones;
/// <summary>Consulta la recepción y prepara servicios iniciales opcionales antes de solicitar evaluación.</summary>
public partial class FrmDetalleAtencion : Form
{
    private readonly int _id;
    private readonly AtencionServicio _atenciones;
    private readonly EvaluacionServicio _evaluaciones;
    private readonly SesionUsuario _sesion;
    private Atencion? _atencion;
    private BindingList<ServicioPreparado> _items = [];
    private bool _ocupado;
    private sealed class ServicioPreparado
    {
        public int IdConcepto { get; init; }
        public string Descripcion { get; init; } = "";
        public decimal Cantidad { get; init; }
        public decimal PrecioUnitario { get; init; }
        public decimal Subtotal => decimal.Round(Cantidad * PrecioUnitario, 2, MidpointRounding.AwayFromZero);
    }

    public FrmDetalleAtencion(int idAtencion, AtencionServicio atencionServicio, SesionUsuario sesionUsuario, EvaluacionServicio evaluaciones)
    {
        InitializeComponent();
        _id = idAtencion;
        _atenciones = atencionServicio;
        _sesion = sesionUsuario;
        _evaluaciones = evaluaciones;
    }

    private async void Cargar(object? s, EventArgs e) => await RecargarAsync();
    private async Task RecargarAsync()
    {
        _ocupado = true;
        tlpPrincipal.Enabled = false;
        try
        {
            _atencion = await _atenciones.ObtenerPorIdAsync(_id) ?? throw new InvalidOperationException("La atención no existe.");
            lblResumen.Text = $"Atención {_id:D6} · {_atencion.Estado} · Solicitud: {_atencion.TipoSolicitud}\n{_atencion.Cliente.Apellido}, {_atencion.Cliente.Nombre} · {_atencion.Vehiculo.Dominio} · Apertura {_atencion.FechaApertura:dd/MM/yyyy HH:mm}";
            txtMotivo.Text = (_atencion.Solicitudes.Count == 0 ? _atencion.MotivoConsulta : string.Join(Environment.NewLine, _atencion.Solicitudes.OrderBy(s => s.Orden).Select(s => $"{s.Orden}. {(s.Tipo == Taller.Dominio.Enumeraciones.TipoSolicitudIngreso.TrabajoSolicitado ? "Solicita" : "Informa")}: {s.Descripcion}")))
                + $"\r\nKilometraje de ingreso: {_atencion.KilometrajeRecepcion?.ToString("N0") ?? "No informado"}\r\nObservaciones: {_atencion.ObservacionesRecepcion}";
            bool recepcion = _sesion.UsuarioActual?.RolId == RolesSistema.AdministrativoId;
            grpIngreso.Visible = recepcion && _atencion.Estado == Taller.Dominio.Enumeraciones.EstadoAtencion.Abierta;
            btnConfirmarIngreso.Visible = grpIngreso.Visible;
            dgvServicios.DataSource = null;
            if (grpIngreso.Visible)
            {
                cboServicioIngreso.DisplayMember = nameof(ServicioIngresoDto.Nombre);
                cboServicioIngreso.DataSource = await _atenciones.ListarServiciosDisponiblesParaIngresoAsync();
                dgvServicios.DataSource = _items;
            }
            else
            {
                var presupuestos = await _evaluaciones.ListarPresupuestosAsync();
                dgvServicios.DataSource = presupuestos.Where(p => p.IdAtencion == _id && p.Tipo == Taller.Dominio.Enumeraciones.TipoPresupuesto.Diagnostico).SelectMany(p => p.Items).ToList();
            }

            EstadoAutorizacion(null, EventArgs.Empty);
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

    private void ServicioElegido(object? s, EventArgs e)
    {
        if (cboServicioIngreso.SelectedItem is ServicioIngresoDto c)
            nudImporteIngreso.Value = Math.Min(nudImporteIngreso.Maximum, c.PrecioReferencia ?? 0);
    }

    private void ServicioConfirmado(object? s, EventArgs e)
    {
        nudImporteIngreso.Focus();
        nudImporteIngreso.Select(0, nudImporteIngreso.Text.Length);
    }

    private void Agregar(object? s, EventArgs e)
    {
        ValidateChildren();
        if (cboServicioIngreso.SelectedItem is not ServicioIngresoDto c)
            return;
        if (_items.Any(i => i.IdConcepto == c.IdConcepto))
        {
            MessageBox.Show(this, "El servicio ya está agregado. Quite la fila para modificarlo.");
            return;
        }

        _items.Add(new ServicioPreparado { IdConcepto = c.IdConcepto, Descripcion = c.Nombre, Cantidad = nudCantidadIngreso.Value, PrecioUnitario = nudImporteIngreso.Value });
        chkAutorizaIngreso.Checked = false;
        EstadoAutorizacion(null, EventArgs.Empty);
    }

    private void Quitar(object? s, EventArgs e)
    {
        if (dgvServicios.CurrentRow?.DataBoundItem is ServicioPreparado i)
            _items.Remove(i);
        chkAutorizaIngreso.Checked = false;
        EstadoAutorizacion(null, EventArgs.Empty);
    }

    private void EstadoAutorizacion(object? s, EventArgs e)
    {
        chkAutorizaIngreso.Enabled = _items.Count > 0;
        btnConfirmarIngreso.Enabled = _items.Count == 0 || chkAutorizaIngreso.Checked;
        btnConfirmarIngreso.Text = _items.Count == 0 ? "Enviar a taller sin cargos iniciales" : "Autorizar inspección y enviar a taller";
        lblTotal.Text = $"Servicios iniciales: {_items.Sum(i => i.Subtotal):N2}. No equivale a ejecución ni pago. Puede continuar sin agregar servicios.";
    }

    private async void Confirmar(object? s, EventArgs e)
    {
        if (_ocupado || _atencion == null)
            return;
        if (MessageBox.Show(this, _items.Count == 0 ? "Se enviará al técnico sin servicios iniciales cobrables. No autoriza una reparación. ¿Continuar?" : "¿El cliente autorizó los servicios e importes listados? Solo se registrará esa autorización, no su ejecución ni cobro.", "Solicitar evaluación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        _ocupado = true;
        tlpPrincipal.Enabled = false;
        try
        {
            if (_items.Count == 0)
                await _evaluaciones.EnviarAEvaluacionAsync(_id, _atencion.TipoSolicitud, _atencion.Version);
            else
                await _atenciones.ConfirmarIngresoAsync(new ConfirmarIngresoDto { IdAtencion = _id, VersionAtencion = _atencion.Version, ClienteAutoriza = chkAutorizaIngreso.Checked, Servicios = _items.Select(i => new ServicioIngresoSeleccionadoDto { IdConcepto = i.IdConcepto, Cantidad = i.Cantidad, PrecioUnitario = i.PrecioUnitario }).ToList() });
            _items.Clear();
            DialogResult = DialogResult.OK;
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

        if (_items.Count > 0 && MessageBox.Show(this, "¿Descartar los servicios preparados sin confirmar?", "Cerrar atención", MessageBoxButtons.YesNo) != DialogResult.Yes)
            e.Cancel = true;
    }

    private void Error(Exception ex) => Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Detalle de atención");
}
