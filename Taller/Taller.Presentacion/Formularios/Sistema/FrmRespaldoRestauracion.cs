using Taller.Aplicacion.DTOs.Respaldos;
using Taller.Aplicacion.Servicios;
using Taller.Presentacion.Navegacion;
namespace Taller.Presentacion.Formularios.Sistema;
/// <summary>Administración real de copias, agente y recuperación. Las rutas se interpretan en SQL Server.</summary>
public partial class FrmRespaldoRestauracion : Form,IFormularioConOperacion
{
    private readonly RespaldoServicio _servicio;
    private readonly SesionUsuario _sesion;
    private ProgramaRespaldoDto _programa=new();
    private PlanRestauracionDto? _plan;
    private bool _configurando;
    private bool _reiniciar;
    public bool OperacionEnCurso {get;private set;}
    public FrmRespaldoRestauracion(RespaldoServicio servicio,SesionUsuario sesion){InitializeComponent();_servicio=servicio;_sesion=sesion;}
    private sealed record DiaOpcion(DayOfWeek Dia,string Nombre);
    private async void Formulario_Load(object? sender,EventArgs e)
    {
        _configurando=true;
        cboTipo.DataSource=Enum.GetValues<TipoRespaldo>();cboZona.Items.AddRange(["Argentina Standard Time","UTC"]);cboZona.SelectedIndex=0;
        cboDia.DisplayMember="Nombre";cboDia.ValueMember="Dia";
        cboDia.DataSource=Enum.GetValues<DayOfWeek>().Select(d=>new DiaOpcion(d,System.Globalization.CultureInfo.GetCultureInfo("es-AR").DateTimeFormat.GetDayName(d))).ToList();
        _configurando=false;
        if(!_servicio.Habilitado)
        {
            lblEstado.Text="Configuración técnica pendiente. Instale la base de control y habilite Respaldos en todos los clientes. El agente funciona separado de SGTM. Consulte docs/respaldos-restauracion.md con su técnico.";
            tabs.Enabled=btnActualizar.Enabled=false;return;
        }
        await ActualizarAsync();
    }
    private IProgress<string> Progreso()=>new Progress<string>(texto=>{if(!IsDisposed)lblEstado.Text=texto;});
    private async Task EjecutarAsync(Func<Task> tarea)
    {
        if(OperacionEnCurso)return;
        try{OperacionEnCurso=true;tabs.Enabled=btnActualizar.Enabled=false;Cursor=Cursors.WaitCursor;await tarea();}
        catch(Exception ex){lblEstado.Text="Operación no completada: "+ex.Message;MessageBox.Show(this,ex.Message,"Respaldos y recuperación",MessageBoxButtons.OK,MessageBoxIcon.Warning);}
        finally{_configurando=false;OperacionEnCurso=false;if(!IsDisposed){tabs.Enabled=btnActualizar.Enabled=true;Cursor=Cursors.Default;ActualizarAcciones();}}
    }
    private async Task CargarDatosAsync()
    {
        var estado=await _servicio.ConsultarAsync();_programa=await _servicio.ObtenerProgramaAsync();
        _configurando=true;
        dgvCopias.DataSource=estado.Copias.ToList();dgvHistorial.DataSource=estado.Historial.ToList();dgvCopias.ClearSelection();dgvHistorial.ClearSelection();
        chkProgramado.Checked=_programa.Activo;chkDiferenciales.Checked=_programa.Diferenciales;txtCarpetaAuto.Text=_programa.CarpetaServidor;
        if(txtCarpetaManual.Text.Length==0)txtCarpetaManual.Text=_programa.CarpetaServidor;
        dtpHora.Value=DateTime.Today.AddMinutes(_programa.MinutoDelDia);cboDia.SelectedValue=_programa.DiaCompleta;
        if(!cboZona.Items.Contains(_programa.ZonaHoraria))cboZona.Items.Add(_programa.ZonaHoraria);cboZona.SelectedItem=_programa.ZonaHoraria;
        string senal=_programa.UltimaSenalAgenteUtc is {} h?$"Última señal del agente: {h:dd/MM/yyyy HH:mm:ss} UTC":"Nunca se recibió una señal del agente";
        bool reciente=_programa.UltimaSenalAgenteUtc is {} ultima&&DateTimeOffset.UtcNow-ultima<TimeSpan.FromMinutes(3);
        lblPrograma.Text=$"{(reciente?"Agente con señal reciente":"ATENCIÓN: agente sin señal reciente; no se garantiza ejecución automática")}. {senal}.\r\nPróxima ejecución: {(_programa.ProximaUtc is {} p?TimeZoneInfo.ConvertTime(p,Taller.Aplicacion.Respaldos.PoliticaRespaldos.ResolverZona(_programa.ZonaHoraria)).ToString("dd/MM/yyyy HH:mm zzz"):"Desactivada")} · Zona: {_programa.ZonaHoraria}.\r\nÚltimo resultado: {_programa.UltimoResultado}";
        _plan=null;txtPlan.Text="Seleccione una copia registrada y presione Ensayar restauración. La completa base se incorpora automáticamente si selecciona una diferencial.";
        lblEstado.Text=$"Base: {estado.BaseDatos} · Generación {estado.Generacion} · {estado.Copias.Count} copias registradas. {estado.Detalle}";
        _configurando=false;
    }
    private async Task ActualizarAsync()=>await EjecutarAsync(CargarDatosAsync);
    private void ActualizarAcciones(){btnEnsayar.Enabled=dgvCopias.SelectedRows.Count==1;btnRestaurar.Enabled=_plan!=null;}
    private void DgvCopias_SelectionChanged(object? sender,EventArgs e)
    {if(_configurando)return;_plan=null;txtPlan.Text="Selección modificada: debe ensayar nuevamente antes de restaurar.";ActualizarAcciones();}
    private async void BtnActualizar_Click(object? sender,EventArgs e)=>await ActualizarAsync();
    private async void BtnCrear_Click(object? sender,EventArgs e)
    {
        await EjecutarAsync(async()=>{
            var copia=await _servicio.CrearAsync((TipoRespaldo)cboTipo.SelectedItem!,txtCarpetaManual.Text,Progreso());
            await CargarDatosAsync();MessageBox.Show(this,"Copia principal creada y verificada.\n"+copia.RutaServidor+"\n"+copia.Advertencia,"Respaldo",MessageBoxButtons.OK,copia.Advertencia.Length==0?MessageBoxIcon.Information:MessageBoxIcon.Warning);
        });
    }
    private async void BtnGuardarPrograma_Click(object? sender,EventArgs e)
    {
        await EjecutarAsync(async()=>{
            var p=_programa with {Activo=chkProgramado.Checked,Diferenciales=chkDiferenciales.Checked,DiaCompleta=(DayOfWeek)cboDia.SelectedValue!,MinutoDelDia=dtpHora.Value.Hour*60+dtpHora.Value.Minute,ZonaHoraria=cboZona.SelectedItem?.ToString()??"",CarpetaServidor=txtCarpetaAuto.Text};
            await _servicio.GuardarProgramaAsync(p);await CargarDatosAsync();
            MessageBox.Show(this,"Plan guardado. Revise la señal del agente: guardar el plan no instala ni inicia el servicio de Windows.","Programación");
        });
    }
    private async void BtnEnsayar_Click(object? sender,EventArgs e)
    {
        if(dgvCopias.SelectedRows.Count!=1||dgvCopias.SelectedRows[0].DataBoundItem is not CopiaRespaldoDto c)return;
        _plan=null;ActualizarAcciones();
        await EjecutarAsync(async()=>{_plan=await _servicio.ProbarRestauracionAsync(c.Id,Progreso());txtPlan.Text=_plan.Resumen+$"\r\nPlan válido hasta {_plan.VenceUtc:HH:mm:ss} UTC. La autorización exige dos cuentas distintas y una ventana de mantenimiento.";});
    }
    private async void BtnRestaurar_Click(object? sender,EventArgs e)
    {
        if(_plan is null)return;
        using var aprobacion=new FrmAprobarRestauracion(_sesion.UsuarioActual?.NombreUsuario??"",_plan.Resumen);
        if(aprobacion.ShowDialog(this)!=DialogResult.OK)return;
        try
        {
            await EjecutarAsync(async()=>{
                var resultado=await _servicio.RestaurarAsync(_plan.Token,aprobacion.UsuarioPrimero,aprobacion.ClavePrimero,aprobacion.UsuarioSegundo,aprobacion.ClaveSegundo,aprobacion.Motivo,aprobacion.ReconoceConsecuencias,Progreso());
                _reiniciar=true;OperacionEnCurso=false;
                MessageBox.Show(this,resultado.Mensaje+"\r\nCopia protectora: "+resultado.RutaProtectora,"Recuperación",MessageBoxButtons.OK,resultado.Restaurada?MessageBoxIcon.Information:MessageBoxIcon.Warning);
                Application.Exit();
            });
        }
        finally{aprobacion.LimpiarClaves();if(!_reiniciar){_plan=null;ActualizarAcciones();}}
    }
    private void Formulario_FormClosing(object? sender,FormClosingEventArgs e)
    {if(OperacionEnCurso){e.Cancel=true;MessageBox.Show(this,"Espere a que termine la operación. No apague el equipo durante una recuperación.","Operación en curso");}}
}
