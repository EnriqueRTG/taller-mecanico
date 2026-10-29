using Taller.Dominio.Tiempo;
using Taller.Aplicacion.DTOs.Respaldos;
using Taller.Aplicacion.Servicios;
using Taller.Presentacion.Navegacion;

namespace Taller.Presentacion.Formularios.Sistema;
/// <summary>Administración real de copias, agente y recuperación. Las rutas se interpretan en SQL Server.</summary>
public partial class FrmRespaldoRestauracion : Form, IFormularioConOperacion
{
    private readonly RespaldoServicio _servicio;
    private readonly SesionUsuario _sesion;
    private ProgramaRespaldoDto _programa = new();
    private PlanRestauracionDto? _plan;
    private bool _configurando;
    private bool _reiniciar;
    private bool _puedeExplorarCarpetaLocal;
    public bool OperacionEnCurso { get; private set; }

    public FrmRespaldoRestauracion(RespaldoServicio servicio, SesionUsuario sesion)
    {
        InitializeComponent();
        _servicio = servicio;
        _sesion = sesion;
    }

    private sealed record DiaOpcion(DayOfWeek Dia, string Nombre);
    private async void Formulario_Load(object? sender, EventArgs e)
    {
        _configurando = true;
        cboTipo.DataSource = Enum.GetValues<TipoRespaldo>();
        cboZona.Items.AddRange(["Argentina Standard Time", "UTC"]);
        cboZona.SelectedIndex = 0;
        cboDia.DisplayMember = "Nombre";
        cboDia.ValueMember = "Dia";
        cboDia.DataSource = Enum.GetValues<DayOfWeek>().Select(d => new DiaOpcion(d, System.Globalization.CultureInfo.GetCultureInfo("es-AR").DateTimeFormat.GetDayName(d))).ToList();
        _configurando = false;
        if (!_servicio.Habilitado)
        {
            lblEstado.Text = "Configuración técnica pendiente. Para Demo local, cierre SGTM y ejecute scripts/Preparar-RespaldosDemo.ps1 en el servidor como administrador de Windows. Luego reinicie Visual Studio. Para otras instalaciones consulte docs/respaldos/operacion.md.";
            tabs.Enabled = btnActualizar.Enabled = false;
            return;
        }

        await ActualizarAsync();
    }

    private void ElegirCarpetaManual_Click(object? sender, EventArgs e) => ElegirCarpeta(txtCarpetaManual);
    private void ElegirCarpetaAuto_Click(object? sender, EventArgs e) => ElegirCarpeta(txtCarpetaAuto);
    private void ElegirCarpeta(TextBox destino)
    {
        if (OperacionEnCurso || !_puedeExplorarCarpetaLocal) return;
        try
        {
            using var dialogo = new FolderBrowserDialog
            {
                Description = "Seleccione la carpeta donde SQL Server guardará los respaldos",
                UseDescriptionForTitle = true,
                ShowNewFolderButton = true,
                SelectedPath = Directory.Exists(destino.Text.Trim()) ? destino.Text.Trim() : string.Empty
            };
            if (dialogo.ShowDialog(this) != DialogResult.OK) return;
            if (dialogo.SelectedPath.Length > destino.MaxLength)
            {
                MessageBox.Show(this, $"La ruta admite hasta {destino.MaxLength} caracteres. Elija una carpeta con una ruta más corta.", "Carpeta de respaldo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            destino.Text = dialogo.SelectedPath;
        }
        catch (Exception ex)
        {
            Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Seleccionar carpeta de respaldo");
        }
    }

    private IProgress<string> Progreso() => new Progress<string>(texto =>
    {
        if (!IsDisposed)
            lblEstado.Text = texto;
    });
    private async Task EjecutarAsync(Func<Task> tarea)
    {
        if (OperacionEnCurso)
            return;
        try
        {
            OperacionEnCurso = true;
            tabs.Enabled = btnActualizar.Enabled = false;
            Cursor = Cursors.WaitCursor;
            await tarea();
        }
        catch (Exception ex)
        {
            if (ex is Microsoft.Data.SqlClient.SqlException or Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                lblEstado.Text = "SQL Server no completó la operación. Compruebe la base de control, permisos SQL y permisos de la cuenta del servicio SQL sobre la carpeta del servidor. Consulte la referencia de asistencia antes de repetir una recuperación.";
                Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Respaldos y recuperación");
            }
            else
            {
                if (ex is InvalidOperationException)
                {
                    lblEstado.Text = "Operación no completada: " + ex.Message;
                    MessageBox.Show(this, ex.Message, "Respaldos y recuperación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    Taller.Presentacion.Errores.ErroresInterfaz.Mostrar(this, ex, "Respaldos y recuperación");
            }
        }
        finally
        {
            _configurando = false;
            OperacionEnCurso = false;
            if (!IsDisposed)
            {
                tabs.Enabled = btnActualizar.Enabled = true;
                Cursor = Cursors.Default;
                ActualizarAcciones();
            }
        }
    }

    private async Task CargarDatosAsync()
    {
        var estado = await _servicio.ConsultarAsync();
        _puedeExplorarCarpetaLocal = estado.PuedeExplorarCarpetaLocal;
        btnCarpetaManual.Visible = btnCarpetaAuto.Visible = _puedeExplorarCarpetaLocal;
        lblManual.Text = _puedeExplorarCarpetaLocal
            ? "Elija una carpeta de este equipo o escriba su ruta. SQL Server debe tener permiso de escritura, también en unidades externas. Completa copia toda la base; diferencial necesita su completa base. Elegir una carpeta no crea el respaldo ni concede permisos."
            : $"SQL Server está en otro equipo o sistema ({estado.EquipoServidor}). Escriba la ruta del servidor: el explorador de esta PC no representa su disco. Para elegir una carpeta visualmente, abra SGTM en el servidor Windows. SQL Server debe tener permiso de escritura.";
        _programa = await _servicio.ObtenerProgramaAsync();
        _configurando = true;
        dgvCopias.DataSource = estado.Copias.ToList();
        dgvHistorial.DataSource = estado.Historial.Select(x => new { FechaLocal = FechaHoraTaller.DesdeUtc(x.FechaUtc).DateTime, x.Accion, x.Resultado, x.Autor, x.SegundoAprobador, x.Detalle }).ToList();
        dgvCopias.ClearSelection();
        dgvHistorial.ClearSelection();
        chkAzureAuto.Checked = _programa.CopiarAzure;
        chkProgramado.Checked = _programa.Activo;
        chkDiferenciales.Checked = _programa.Diferenciales;
        txtCarpetaAuto.Text = _programa.CarpetaServidor;
        if (txtCarpetaManual.Text.Length == 0)
            txtCarpetaManual.Text = _programa.CarpetaServidor;
        dtpHora.Value = FechaHoraTaller.Hoy.AddMinutes(_programa.MinutoDelDia);
        cboDia.SelectedValue = _programa.DiaCompleta;
        if (!cboZona.Items.Contains(_programa.ZonaHoraria))
            cboZona.Items.Add(_programa.ZonaHoraria);
        cboZona.SelectedItem = _programa.ZonaHoraria;
        string senal = _programa.UltimaSenalAgenteUtc is { } h ? $"Última señal del agente: {FechaHoraTaller.DesdeUtc(h):dd/MM/yyyy HH:mm:ss zzz} (Argentina)" : "Nunca se recibió una señal del agente";
        bool reciente = _programa.UltimaSenalAgenteUtc is { } ultima && DateTimeOffset.UtcNow - ultima < TimeSpan.FromMinutes(3);
        txtEstadoPrograma.Text = $"{(reciente ? "Agente con señal reciente" : "ATENCIÓN: agente sin señal reciente; no se garantiza ejecución automática")}. {senal}.\r\nPróxima ejecución: {(_programa.ProximaUtc is { } p ? TimeZoneInfo.ConvertTime(p, Taller.Aplicacion.Respaldos.PoliticaRespaldos.ResolverZona(_programa.ZonaHoraria)).ToString("dd/MM/yyyy HH:mm zzz") : "Desactivada")} · Zona: {_programa.ZonaHoraria}.\r\nÚltimo resultado: {_programa.UltimoResultado}";
        _plan = null;
        txtPlan.Text = "Seleccione una copia registrada y presione Ensayar restauración. La completa base se incorpora automáticamente si selecciona una diferencial.";
        lblEstado.Text = $"Base: {estado.BaseDatos} · Generación {estado.Generacion} · {estado.Copias.Count} copias registradas. {estado.Detalle}";
        _configurando = false;
    }

    private async Task ActualizarAsync() => await EjecutarAsync(CargarDatosAsync);
    private void ActualizarAcciones()
    {
        btnEnsayar.Enabled = dgvCopias.SelectedRows.Count == 1;
        btnRestaurar.Enabled = _plan != null;
        Taller.Presentacion.Estilos.AccionesContextuales.Actualizar(btnEnsayar, btnRestaurar);
    }

    private void DgvCopias_SelectionChanged(object? sender, EventArgs e)
    {
        if (_configurando)
            return;
        _plan = null;
        txtPlan.Text = "Selección modificada: debe ensayar nuevamente antes de restaurar.";
        ActualizarAcciones();
    }

    private async void BtnActualizar_Click(object? sender, EventArgs e) => await ActualizarAsync();
    private async void BtnCrear_Click(object? sender, EventArgs e)
    {
        await EjecutarAsync(async () =>
        {
            var copia = await _servicio.CrearAsync((TipoRespaldo)cboTipo.SelectedItem!, txtCarpetaManual.Text, Progreso(), chkAzureManual.Checked);
            await CargarDatosAsync();
            MessageBox.Show(this, "Copia principal creada y verificada.\n" + copia.RutaServidor + "\n" + copia.Advertencia + "\n" + copia.AdvertenciaAzure, "Respaldo", MessageBoxButtons.OK, copia.Advertencia.Length == 0 && copia.AdvertenciaAzure.Length == 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        });
    }

    private async void BtnReintentarAzure_Click(object? sender, EventArgs e)
    {
        if (dgvCopias.SelectedRows.Count != 1 || dgvCopias.SelectedRows[0].DataBoundItem is not CopiaRespaldoDto copia)
            return;
        await EjecutarAsync(async () =>
        {
            await _servicio.ReintentarAzureAsync(copia.Id);
            await CargarDatosAsync();
        });
    }

    private async void BtnGuardarPrograma_Click(object? sender, EventArgs e)
    {
        await EjecutarAsync(async () =>
        {
            var p = _programa with
            {
                CopiarAzure = chkAzureAuto.Checked,
                Activo = chkProgramado.Checked,
                Diferenciales = chkDiferenciales.Checked,
                DiaCompleta = (DayOfWeek)cboDia.SelectedValue!,
                MinutoDelDia = dtpHora.Value.Hour * 60 + dtpHora.Value.Minute,
                ZonaHoraria = cboZona.SelectedItem?.ToString() ?? "",
                CarpetaServidor = txtCarpetaAuto.Text
            };
            await _servicio.GuardarProgramaAsync(p);
            await CargarDatosAsync();
            MessageBox.Show(this, "Plan guardado. Revise la señal del agente: guardar el plan no instala ni inicia el servicio de Windows.", "Programación");
        });
    }

    private async void BtnEnsayar_Click(object? sender, EventArgs e)
    {
        if (dgvCopias.SelectedRows.Count != 1 || dgvCopias.SelectedRows[0].DataBoundItem is not CopiaRespaldoDto c)
            return;
        _plan = null;
        ActualizarAcciones();
        await EjecutarAsync(async () =>
        {
            _plan = await _servicio.ProbarRestauracionAsync(c.Id, Progreso());
            txtPlan.Text = _plan.Resumen + $"\r\nPlan válido hasta {FechaHoraTaller.DesdeUtc(_plan.VenceUtc):dd/MM/yyyy HH:mm:ss zzz} (Argentina). La autorización exige dos cuentas distintas y una ventana de mantenimiento.";
        });
    }

    private async void BtnRestaurar_Click(object? sender, EventArgs e)
    {
        if (_plan is null)
            return;
        using var aprobacion = new FrmAprobarRestauracion(_sesion.UsuarioActual?.NombreUsuario ?? "", _plan.Resumen);
        if (aprobacion.ShowDialog(this) != DialogResult.OK)
            return;
        try
        {
            await EjecutarAsync(async () =>
            {
                var resultado = await _servicio.RestaurarAsync(_plan.Token, aprobacion.UsuarioPrimero, aprobacion.ClavePrimero, aprobacion.UsuarioSegundo, aprobacion.ClaveSegundo, aprobacion.Motivo, aprobacion.ReconoceConsecuencias, Progreso());
                _reiniciar = true;
                OperacionEnCurso = false;
                MessageBox.Show(this, resultado.Mensaje + "\r\nCopia protectora: " + resultado.RutaProtectora, "Recuperación", MessageBoxButtons.OK, resultado.Restaurada ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
                Application.Exit();
            });
        }
        finally
        {
            aprobacion.LimpiarClaves();
            if (!_reiniciar)
            {
                _plan = null;
                ActualizarAcciones();
            }
        }
    }

    private void Formulario_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (OperacionEnCurso)
        {
            e.Cancel = true;
            MessageBox.Show(this, "Espere a que termine la operación. No apague el equipo durante una recuperación.", "Operación en curso");
        }
    }
}
