using Taller.Aplicacion.DTOs.Diagnosticos;
using Taller.Aplicacion.Servicios;
using Taller.Dominio.Constantes;
using Taller.Dominio.Enumeraciones;
using Taller.Presentacion.Estilos;
using Taller.Presentacion.Formularios.Atenciones;

namespace Taller.Presentacion.Formularios.Diagnosticos;

/// <summary>
/// Presenta las actividades de diagnóstico y los informes
/// iniciales registrados.
/// </summary>
public partial class FrmDiagnosticos : Form
{
    private readonly DiagnosticoServicio _diagnosticoServicio;
    private readonly AtencionServicio _atencionServicio;
    private readonly SesionUsuario _sesionUsuario;

    private List<DiagnosticoBandejaDto> _datos = [];
    private int? _idAtencionParaAbrir;
    private bool _inicializando;
    private bool _cargando;
    private bool _operando;

    private sealed record TecnicoFiltro(
        int? Id, 
        string Nombre)
    {
        public override string ToString() => Nombre;
    }

    public FrmDiagnosticos(
        DiagnosticoServicio diagnosticoServicio,
        AtencionServicio atencionServicio,
        SesionUsuario sesionUsuario)
    {
        InitializeComponent();

        _diagnosticoServicio = diagnosticoServicio;
        _atencionServicio = atencionServicio;
        _sesionUsuario = sesionUsuario;
    }

    /// <summary>
    /// Define la atención que se abrirá después de cargar la bandeja.
    /// Debe invocarse antes de mostrar el formulario.
    /// </summary>
    public void PrepararAperturaDiagnostico(
        int idAtencion)
    {
        if (idAtencion <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(idAtencion));
        }

        _idAtencionParaAbrir = idAtencion;
    }

    private async void FrmDiagnosticos_Load(
        object? sender,
        EventArgs e)
    {
        AplicarEstilos();

        _inicializando = true;

        cboEstado.Items.Clear();
        cboEstado.Items.AddRange(
        [
            "Todos",
            "Pendiente",
            "En diagnóstico",
            "Registrado",
            "Atención finalizada"
        ]);

        if (EsTecnico())
        {
            cboEstado.Items.Remove("Atención finalizada");
        }

        cboEstado.SelectedIndex = 0;

        _inicializando = false;

        await CargarDatosAsync();

        if (_idAtencionParaAbrir is int idAtencion)
        {
            _idAtencionParaAbrir = null;

            var fila = _datos.FirstOrDefault(d =>
                d.IdAtencion == idAtencion);

            if (fila is null)
            {
                MessageBox.Show(
                    this,
                    "La atención no está disponible en la bandeja. " +
                    "Actualice la información antes de continuar.",
                    "Diagnósticos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            AplicarFiltros(idAtencion);
            await RegistrarDiagnosticoAsync(fila);
        }
    }

    private async Task CargarDatosAsync(
        int? seleccionarAtencion = null)
    {
        if (_cargando)
        {
            return;
        }

        _cargando = true;
        ActualizarDisponibilidad();

        try
        {
            var datos = await _diagnosticoServicio.ListarBandejaAsync();

            if (IsDisposed)
            {
                return;
            }

            _datos = EsTecnico()
                ? datos.Where(d => !EsFinalizada(d)).ToList()
                : datos;

            CargarFiltroTecnicos();
            AplicarFiltros(seleccionarAtencion);
        }
        catch (Exception ex)
        {
            if (!IsDisposed)
            {
                _datos = [];
                dgvDiagnosticos.Rows.Clear();
                lblCantidad.Text = "No se pudo cargar la bandeja";
                lblResumen.Text = string.Empty;

                MessageBox.Show(
                    this,
                    ex.Message,
                    "Diagnósticos",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        finally
        {
            _cargando = false;

            if (!IsDisposed)
            {
                ActualizarDisponibilidad();
            }
        }
    }

    private void CargarFiltroTecnicos()
    {
        int? idSeleccionado =
            (cboTecnico.SelectedItem as TecnicoFiltro)?.Id;

        _inicializando = true;

        try
        {
            var tecnicos = _datos
                .SelectMany(d => new[]
                {
                    new TecnicoFiltro(
                        d.IdTecnicoEnActividad,
                        d.TecnicoEnActividad ?? string.Empty),

                    new TecnicoFiltro(
                        d.IdTecnicoAutor,
                        d.TecnicoAutor ?? string.Empty)
                })
                .Where(t => t.Id.HasValue)
                .GroupBy(t => t.Id)
                .Select(g => g.First())
                .OrderBy(t => t.Nombre)
                .ToList();

            cboTecnico.Items.Clear();
            cboTecnico.Items.Add(new TecnicoFiltro(null, "Todos"));

            foreach (var tecnico in tecnicos)
            {
                cboTecnico.Items.Add(tecnico);
            }

            cboTecnico.SelectedIndex = 0;

            if (idSeleccionado.HasValue)
            {
                for (int i = 1; i < cboTecnico.Items.Count; i++)
                {
                    if (((TecnicoFiltro)cboTecnico.Items[i]).Id
                        == idSeleccionado)
                    {
                        cboTecnico.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
        finally
        {
            _inicializando = false;
        }
    }

    private static bool EsFinalizada(
        DiagnosticoBandejaDto fila)
    {
        return fila.EstadoAtencion == EstadoAtencion.Cerrada
            || fila.EstadoAtencion == EstadoAtencion.Cancelada;
    }

    private static string ObtenerSituacion(
        DiagnosticoBandejaDto fila)
    {
        if (EsFinalizada(fila))
        {
            return "Atención finalizada";
        }

        if (fila.IdDiagnostico.HasValue)
        {
            return "Registrado";
        }

        if (fila.ActividadTecnica == ActividadTecnica.Diagnostico
            && fila.IdTecnicoEnActividad.HasValue)
        {
            return "En diagnóstico";
        }

        return fila.EstadoAtencion == EstadoAtencion.PendienteDiagnostico
            ? "Pendiente"
            : "Revisar atención";
    }

    private int ObtenerGrupo(
        DiagnosticoBandejaDto fila)
    {
        if (EsFinalizada(fila))
        {
            return 9;
        }

        if (PuedeRetomar(fila))
        {
            return 0;
        }

        if (PuedeTomar(fila))
        {
            return 1;
        }

        if (!fila.IdDiagnostico.HasValue)
        {
            return 2;
        }

        return 3;
    }

    private void AplicarFiltros(
        int? seleccionarAtencion = null)
    {
        if (_inicializando)
        {
            return;
        }

        string texto = txtBuscar.Text.Trim();
        string estado = cboEstado.SelectedItem?.ToString() ?? "Todos";
        int? idTecnico = (cboTecnico.SelectedItem as TecnicoFiltro)?.Id;

        IEnumerable<DiagnosticoBandejaDto> consulta = _datos;

        if (!string.IsNullOrWhiteSpace(texto))
        {
            consulta = consulta.Where(d =>
                d.IdAtencion.ToString("D6").Contains(
                    texto, StringComparison.OrdinalIgnoreCase)
                || d.Cliente.Contains(
                    texto, StringComparison.OrdinalIgnoreCase)
                || d.Vehiculo.Contains(
                    texto, StringComparison.OrdinalIgnoreCase)
                || d.Dominio.Contains(
                    texto, StringComparison.OrdinalIgnoreCase)
                || d.MotivoConsulta.Contains(
                    texto, StringComparison.OrdinalIgnoreCase));
        }

        if (estado != "Todos")
        {
            consulta = consulta.Where(d =>
                ObtenerSituacion(d) == estado);
        }

        if (idTecnico.HasValue)
        {
            consulta = consulta.Where(d =>
                d.IdTecnicoEnActividad == idTecnico
                || d.IdTecnicoAutor == idTecnico);
        }

        var pendientes = consulta
            .Where(d => !EsFinalizada(d) && !d.IdDiagnostico.HasValue)
            .OrderBy(ObtenerGrupo)
            .ThenBy(d => d.FechaTomaDiagnostico ?? d.FechaApertura)
            .ThenBy(d => d.IdAtencion);

        var registrados = consulta
            .Where(d => !EsFinalizada(d) && d.IdDiagnostico.HasValue)
            .OrderByDescending(d => d.FechaDiagnostico)
            .ThenByDescending(d => d.IdDiagnostico);

        var finalizados = consulta
            .Where(EsFinalizada)
            .OrderByDescending(d => d.FechaDiagnostico ?? d.FechaApertura)
            .ThenByDescending(d => d.IdAtencion);

        var resultado = pendientes
            .Concat(registrados)
            .Concat(finalizados)
            .ToList();

        dgvDiagnosticos.Rows.Clear();

        foreach (var fila in resultado)
        {
            DateTime fecha =
                fila.FechaDiagnostico
                ?? fila.FechaTomaDiagnostico
                ?? fila.FechaApertura;

            string tecnico =
                fila.IdDiagnostico.HasValue
                    ? fila.TecnicoAutor ?? "Sin autor"
                    : fila.TecnicoEnActividad ?? "Sin reserva";

            int indice = dgvDiagnosticos.Rows.Add(
                $"A-{fila.IdAtencion:D6}",
                fecha.ToString("dd/MM/yyyy HH:mm"),
                fila.Cliente,
                $"{fila.Vehiculo} · {fila.Dominio}",
                fila.MotivoConsulta,
                tecnico,
                ObtenerSituacion(fila));

            dgvDiagnosticos.Rows[indice].Tag = fila;
        }

        dgvDiagnosticos.ClearSelection();
        dgvDiagnosticos.CurrentCell = null;

        if (seleccionarAtencion.HasValue)
        {
            foreach (DataGridViewRow row in dgvDiagnosticos.Rows)
            {
                if (row.Tag is DiagnosticoBandejaDto fila
                    && fila.IdAtencion == seleccionarAtencion.Value)
                {
                    dgvDiagnosticos.CurrentCell = row.Cells[0];
                    row.Selected = true;
                    break;
                }
            }
        }

        int pendientesTotales = _datos.Count(d =>
            !EsFinalizada(d) && !d.IdDiagnostico.HasValue);

        lblResumen.Text =
            $"{pendientesTotales} informes iniciales pendientes";

        lblCantidad.Text = $"{resultado.Count} atenciones";

        ActualizarAcciones();
    }

    private DiagnosticoBandejaDto? Seleccionado()
    {
        return dgvDiagnosticos.CurrentRow?.Tag as DiagnosticoBandejaDto;
    }

    private bool EsTecnico()
    {
        return _sesionUsuario.UsuarioActual?.RolId
            == RolesSistema.TecnicoId;
    }

    private static bool PuedeTomar(
        DiagnosticoBandejaDto fila)
    {
        return !fila.IdDiagnostico.HasValue
            && fila.EstadoAtencion == EstadoAtencion.PendienteDiagnostico
            && fila.IdTecnicoEnActividad is null
            && fila.ActividadTecnica is null;
    }

    private bool PuedeRetomar(
        DiagnosticoBandejaDto fila)
    {
        return EsTecnico()
            && !fila.IdDiagnostico.HasValue
            && fila.EstadoAtencion == EstadoAtencion.EnEvaluacionTecnica
            && fila.ActividadTecnica == ActividadTecnica.Diagnostico
            && fila.IdTecnicoEnActividad
                == _sesionUsuario.UsuarioActual?.Id;
    }

    private void ActualizarAcciones()
    {
        var fila = Seleccionado();
        bool disponible = !_cargando && !_operando;

        btnRegistrar.Text =
            fila is not null && PuedeRetomar(fila)
                ? "Registrar diagnóstico"
                : "Tomar diagnóstico";

        btnRegistrar.Enabled =
            disponible
            && EsTecnico()
            && fila is not null
            && (PuedeTomar(fila) || PuedeRetomar(fila));

        btnLiberarDiagnostico.Enabled =
            disponible
            && fila is not null
            && PuedeRetomar(fila);

        btnVerDiagnostico.Enabled =
            disponible
            && fila?.IdDiagnostico is not null;

        btnVerAtencion.Enabled = disponible && fila is not null;
    }

    private void ActualizarDisponibilidad()
    {
        bool disponible = !_cargando && !_operando;

        txtBuscar.Enabled = disponible;
        cboEstado.Enabled = disponible;
        cboTecnico.Enabled = disponible;
        btnActualizar.Enabled = disponible;
        btnLimpiar.Enabled = disponible;
        dgvDiagnosticos.Enabled = disponible;

        ActualizarAcciones();
    }

    private async void BtnRegistrar_Click(
        object? sender, 
        EventArgs e)
    {
        var fila = Seleccionado();

        if (fila is not null)
        {
            await RegistrarDiagnosticoAsync(fila);
        }
    }

    private async Task RegistrarDiagnosticoAsync(
        DiagnosticoBandejaDto fila)
    {
        if (_operando || _cargando || !EsTecnico()
            || (!PuedeTomar(fila) && !PuedeRetomar(fila)))
        {
            return;
        }

        _operando = true;
        ActualizarDisponibilidad();

        try
        {
            if (PuedeTomar(fila))
            {
                await _atencionServicio.TomarDiagnosticoAsync(
                    fila.IdAtencion);
            }

            using var formulario = new FrmRegistrarDiagnostico(
                fila.IdAtencion,
                _atencionServicio,
                _diagnosticoServicio,
                _sesionUsuario);

            formulario.ShowDialog(this);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                this,
                ex.Message,
                "Diagnóstico",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        finally
        {
            try
            {
                await CargarDatosAsync(fila.IdAtencion);
            }
            finally
            {
                _operando = false;

                if (!IsDisposed)
                {
                    ActualizarDisponibilidad();
                }
            }
        }
    }

    private async void BtnLiberar_Click(
        object? sender, 
        EventArgs e)
    {
        var fila = Seleccionado();

        if (_operando || _cargando || fila is null || !PuedeRetomar(fila))
        {
            return;
        }

        if (MessageBox.Show(
                this,
                "¿Desea liberar la actividad para que otro técnico pueda tomarla? " +
                "Esto no cancela la atención ni el cargo inicial.",
                "Liberar diagnóstico",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        {
            return;
        }

        _operando = true;
        ActualizarDisponibilidad();

        try
        {
            await _atencionServicio.LiberarDiagnosticoAsync(
                fila.IdAtencion);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                this, ex.Message, "Liberar diagnóstico",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            try
            {
                await CargarDatosAsync(fila.IdAtencion);
            }
            finally
            {
                _operando = false;
                if (!IsDisposed) ActualizarDisponibilidad();
            }
        }
    }

    private async void BtnVerAtencion_Click(
        object? sender, 
        EventArgs e)
    {
        var fila = Seleccionado();

        if (_operando || _cargando || fila is null)
        {
            return;
        }

        _operando = true;
        ActualizarDisponibilidad();

        try
        {
            using var formulario = new FrmDetalleAtencion(
                fila.IdAtencion,
                _atencionServicio,
                _sesionUsuario);

            formulario.ShowDialog(this);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                this, ex.Message, "Detalle de atención",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            try
            {
                await CargarDatosAsync(fila.IdAtencion);
            }
            finally
            {
                _operando = false;
                if (!IsDisposed) ActualizarDisponibilidad();
            }
        }
    }

    private void BtnVerDiagnostico_Click(
        object? sender, 
        EventArgs e)
    {
        var fila = Seleccionado();

        if (_operando || _cargando
            || fila?.IdDiagnostico is not int idDiagnostico)
        {
            return;
        }

        _operando = true;
        ActualizarDisponibilidad();

        try
        {
            using var formulario = new FrmDetalleDiagnostico(
                idDiagnostico,
                _diagnosticoServicio);

            formulario.ShowDialog(this);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                this,
                ex.Message,
                "Consultar diagnóstico",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
        finally
        {
            _operando = false;
            ActualizarDisponibilidad();
        }
    }

    private void DgvDiagnosticos_CellDoubleClick(
        object? sender,
        DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            BtnVerAtencion_Click(sender, EventArgs.Empty);
        }
    }

    private void Filtros_Changed(
        object? sender, 
        EventArgs e)
    {
        if (!_inicializando && !_cargando && !_operando)
        {
            AplicarFiltros();
        }
    }

    private void BtnLimpiar_Click(
        object? sender, 
        EventArgs e)
    {
        _inicializando = true;

        txtBuscar.Clear();
        cboEstado.SelectedIndex = 0;
        cboTecnico.SelectedIndex = 0;

        _inicializando = false;
        AplicarFiltros();
    }

    private async void BtnActualizar_Click(
        object? sender, 
        EventArgs e)
    {
        if (!_operando && !_cargando)
        {
            await CargarDatosAsync();
        }
    }

    private void DgvDiagnosticos_SelectionChanged(
        object? sender,
        EventArgs e)
    {
        ActualizarAcciones();
    }

    private void AplicarEstilos()
    {
        EstilosInterfaz.AplicarBotonPrimario(btnRegistrar);
        EstilosInterfaz.AplicarBotonSecundario(btnLiberarDiagnostico);
        EstilosInterfaz.AplicarBotonSecundario(btnVerAtencion);
        EstilosInterfaz.AplicarBotonSecundario(btnActualizar);
        EstilosInterfaz.AplicarBotonSecundario(btnLimpiar);
        EstilosInterfaz.AplicarGrilla(dgvDiagnosticos);
        EstilosInterfaz.AplicarBotonSecundario(btnVerDiagnostico);
    }
}