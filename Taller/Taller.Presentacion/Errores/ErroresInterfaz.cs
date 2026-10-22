using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Taller.Aplicacion.Excepciones;

namespace Taller.Presentacion.Errores;
/// <summary>Presenta reglas conocidas y conserva el detalle técnico solo en el registro local, sin mensajes SQL al operador.</summary>
internal static class ErroresInterfaz
{
    public static void Mostrar(IWin32Window? propietario, Exception error, string operacion)
    {
        string referencia = Guid.NewGuid().ToString("N")[..10];
        string mensaje = error switch
        {
            ConflictoConcurrenciaException => error.Message + " Cierre esta edición, actualice y vuelva a abrirla; no se sobrescribieron cambios.",
            ValidacionException => error.Message,
            ArgumentException => error.Message,
            SqlException => "No se pudo completar la operación en la base de datos. Revise la conexión e intente actualizar antes de repetir un guardado.",
            DbUpdateException => "No se pudieron guardar los datos. Actualice la información; si el problema continúa, contacte al personal técnico.",
            _ => "No se pudo completar la operación. Actualice los datos y, si continúa, solicite asistencia técnica."
        };
        if (error is not (ValidacionException or ArgumentException or ConflictoConcurrenciaException))
        {
            Log.Error(error, "Fallo en {Operacion}; referencia {Referencia}", operacion, referencia);
            mensaje += $"\nReferencia: {referencia}";
        }

        MessageBox.Show(propietario, mensaje, operacion, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
