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

/// <summary>
/// Atiende el atajo F1 y abre el tema del manual correspondiente
/// al formulario que el usuario está utilizando.
/// </summary>
internal sealed class AyudaContextual : IMessageFilter
{
    // WM_KEYDOWN es el mensaje que Windows envía al presionar una tecla.
    private const int MensajeTeclaPresionada = 0x0100;

    /// <summary>
    /// Examina los mensajes de teclado antes de que lleguen al control de destino.
    /// El filtro debe estar registrado con Application.AddMessageFilter.
    /// </summary>
    /// <returns>
    /// true si el filtro consume el mensaje, o false si WinForms
    /// debe continuar procesándolo normalmente.
    /// </returns>
    public bool PreFilterMessage(ref Message mensaje)
    {
        // Solo interesa F1 sin modificadores como Ctrl, Shift o Alt.
        // WParam contiene el código de la tecla en un mensaje WM_KEYDOWN.
        if (mensaje.Msg != MensajeTeclaPresionada || mensaje.WParam.ToInt64() != (int)Keys.F1
            || Control.ModifierKeys != Keys.None)
            return false;

        // HWnd identifica la ventana que recibió la tecla. Puede pertenecer
        // a un control hijo, como una grilla o un cuadro de texto.
        // FindForm busca su formulario contenedor. Si no lo encuentra,
        // se intenta usar el formulario activo de WinForms.
        var formulario = Control.FromChildHandle(mensaje.HWnd)?.FindForm() ?? Form.ActiveForm;

        if (formulario is null)
            return false;

        // El bit 30 de LParam indica si la tecla ya estaba presionada.
        // Abrimos la ayuda solo al inicio de la pulsación y consumimos
        // también los mensajes repetidos mientras se mantiene F1.
        if ((mensaje.LParam.ToInt64() & (1L << 30)) == 0)
            AbrirTema(formulario);

        return true;
    }

    /// <summary>
    /// Abre el tema asociado al formulario indicado.
    /// </summary>
    internal static void AbrirTema(Form formulario)
    {
        Abrir(formulario, ResolverTema(formulario));
    }

    /// <summary>
    /// Abre el índice general del manual desde cualquier formulario.
    /// </summary>
    internal static void AbrirManual(Form formulario)
    {
        Abrir(formulario, "index");
    }

    /// <summary>
    /// Devuelve el nombre del archivo de ayuda sin la extensión .html.
    /// Si el formulario no tiene un tema propio, busca en sus propietarios.
    /// </summary>
    internal static string ResolverTema(Form formulario)
    {
        // Owner es el formulario propietario de la ventana actual.
        // Por ejemplo, un diálogo sin tema propio puede usar el tema
        // de la pantalla desde la cual se abrió, si esta es su propietaria.
        for (Form? actual = formulario; actual is not null; actual = actual.Owner)
        {
            // El switch identifica el tipo concreto de formulario.
            // Varias pantallas pueden compartir un tema. Las que exponen
            // TemaAyuda determinan su tema desde el propio formulario.
            var tema = actual switch
            {
                // La ventana principal aloja otras pantallas. Si hay una
                // activa, se resuelve la ayuda para esa pantalla.
                FrmPrincipal principal when principal.FormularioActivoParaAyuda is { } activo => ResolverTema(activo),

                FrmLogin => "inicio",
                FrmInicio inicio => inicio.TemaAyuda,
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

            // El primer tema encontrado tiene prioridad. Si el resultado
            // es null, la siguiente vuelta prueba con actual.Owner.
            if (tema is not null)
                return tema;
        }

        return "index";
    }

    /// <summary>
    /// Abre el HTML del tema con el programa asociado en Windows.
    /// Si el archivo del tema no está disponible, intenta abrir el índice.
    /// </summary>
    private static void Abrir(Form formulario, string tema)
    {
        // La carpeta Ayuda se busca junto a la aplicación en ejecución.
        // Desde Visual Studio, normalmente es la carpeta de salida
        // de la compilación, como bin\Debug\... .
        var carpeta = Path.Combine(AppContext.BaseDirectory, "Ayuda");
        var archivo = Path.Combine(carpeta, tema + ".html");
        if (!File.Exists(archivo))
            archivo = Path.Combine(carpeta, "index.html");

        try
        {
            // Si tampoco está el índice, se usa el mismo manejo de error
            // que para un fallo al intentar abrir el archivo.
            if (!File.Exists(archivo))
                throw new FileNotFoundException();

            // UseShellExecute permite que Windows abra el HTML con su
            // aplicación asociada. Dispose libera el objeto Process
            // devuelto por Start, sin cerrar el navegador.
            Process.Start(new ProcessStartInfo(archivo) { UseShellExecute = true })?.Dispose();
        }
        catch (Exception ex) when (ex is Win32Exception or IOException or InvalidOperationException)
        {
            // El registro conserva el detalle técnico y el tema solicitado.
            // El cuadro de diálogo muestra al usuario qué puede comprobar.
            Serilog.Log.Warning(ex, "No se pudo abrir el tema de ayuda {Tema}", tema);
            MessageBox.Show(formulario, "No se pudo abrir el manual. Compruebe que la carpeta Ayuda esté junto a la aplicación y que Windows tenga un navegador asociado a los archivos HTML.",
                "Ayuda del SGTM", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}