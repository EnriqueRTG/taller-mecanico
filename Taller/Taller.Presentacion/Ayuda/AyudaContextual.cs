using System.ComponentModel;
using System.Diagnostics;
using Taller.Presentacion.Formularios.Atenciones;
using Taller.Presentacion.Formularios.Autenticacion;
using Taller.Presentacion.Formularios.Catalogo;
using Taller.Presentacion.Formularios.Clientes;
using Taller.Presentacion.Formularios.Comprobantes;
using Taller.Presentacion.Formularios.Diagnosticos;
using Taller.Presentacion.Formularios.Ejecuciones;
using Taller.Presentacion.Formularios.Presupuestos;
using Taller.Presentacion.Formularios.Principal;
using Taller.Presentacion.Formularios.Reportes;
using Taller.Presentacion.Formularios.Sistema;
using Taller.Presentacion.Formularios.Usuarios;
using Taller.Presentacion.Formularios.Vehiculos;

namespace Taller.Presentacion.Ayuda;

internal sealed class AyudaContextual : IMessageFilter
{
    private const int MensajeTeclaPresionada = 0x0100;

    public bool PreFilterMessage(ref Message mensaje)
    {
        if (mensaje.Msg != MensajeTeclaPresionada || mensaje.WParam.ToInt64() != (int)Keys.F1
            || Control.ModifierKeys != Keys.None)
            return false;

        var formulario = Control.FromChildHandle(mensaje.HWnd)?.FindForm() ?? Form.ActiveForm;
        if (formulario is null)
            return false;

        // Ignora las repeticiones si el usuario mantiene F1 presionada.
        if ((mensaje.LParam.ToInt64() & (1L << 30)) == 0)
            AbrirTema(formulario);

        return true;
    }

    internal static void AbrirTema(Form formulario)
    {
        Abrir(formulario, ResolverTema(formulario));
    }

    internal static void AbrirManual(Form formulario)
    {
        Abrir(formulario, "index");
    }

    internal static string ResolverTema(Form formulario)
    {
        for (Form? actual = formulario; actual is not null; actual = actual.Owner)
        {
            var tema = actual switch
            {
                FrmPrincipal principal when principal.FormularioActivoParaAyuda is { } activo => ResolverTema(activo),
                FrmLogin or FrmInicio => "inicio",
                FrmNuevaAtencion => "ingreso",
                FrmAtenciones or FrmDetalleAtencion => "atenciones",
                FrmSeguimientoTrabajo => "seguimiento",
                FrmEjecutarAtencion or FrmTrabajosEnCurso => "ejecucion",
                FrmClientes or FrmAltaCliente or FrmEditarCliente or FrmHistorialCliente => "clientes",
                FrmVehiculos or FrmAltaVehiculo or FrmEditarVehiculo or FrmHistorialVehiculo => "vehiculos",
                FrmCatalogo or FrmBuscarConcepto => "catalogo",
                FrmDiagnosticos or FrmDetalleDiagnostico or FrmEvaluarAtencion => "preparacion",
                FrmPresupuestos presupuestos => presupuestos.TemaAyuda,
                FrmConsultarPropuesta => "presupuestos",
                FrmDecisionPresupuesto => "decision",
                FrmPrepararPropuesta propuesta => propuesta.TemaAyuda,
                FrmComprobantes or FrmComprobantePagos => "liquidacion",
                FrmRegistrarPago => "cobro",
                FrmRegistrarEntrega => "entrega",
                FrmUsuarios or FrmAltaUsuario or FrmEditarUsuario or FrmGestionarCredenciales => "usuarios",
                FrmAuditoriaAccesos => "auditoria",
                FrmRespaldoRestauracion respaldo => respaldo.TemaAyuda,
                FrmAprobarRestauracion => "restauracion",
                FrmReportes => "reportes",
                _ => null
            };

            if (tema is not null)
                return tema;
        }

        return "index";
    }

    private static void Abrir(Form formulario, string tema)
    {
        var carpeta = Path.Combine(AppContext.BaseDirectory, "Ayuda");
        var archivo = Path.Combine(carpeta, tema + ".html");
        if (!File.Exists(archivo))
            archivo = Path.Combine(carpeta, "index.html");

        try
        {
            if (!File.Exists(archivo))
                throw new FileNotFoundException();

            Process.Start(new ProcessStartInfo(archivo) { UseShellExecute = true })?.Dispose();
        }
        catch (Exception ex) when (ex is Win32Exception or IOException or InvalidOperationException)
        {
            Serilog.Log.Warning(ex, "No se pudo abrir el tema de ayuda {Tema}", tema);
            MessageBox.Show(formulario, "No se pudo abrir el manual. Compruebe que la carpeta Ayuda esté junto a la aplicación y que Windows tenga un navegador asociado a los archivos HTML.",
                "Ayuda del SGTM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
