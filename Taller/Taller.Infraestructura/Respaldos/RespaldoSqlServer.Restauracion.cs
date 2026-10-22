using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient;
using Taller.Aplicacion.DTOs.Respaldos;
using Taller.Aplicacion.Respaldos;
using Taller.Dominio.Constantes;

namespace Taller.Infraestructura.Respaldos;
public sealed partial class RespaldoSqlServer
{
    private async Task<(CopiaRespaldoDto Completa, CopiaRespaldoDto? Diferencial, string Esquema)> ComprobarPlanAsync(SqlConnection master, SqlConnection control, Guid copiaId, CancellationToken ct = default)
    {
        var copias = await _registro.CopiasAsync(control, ct);
        var seleccionada = copias.SingleOrDefault(c => c.Id == copiaId) ?? throw new InvalidOperationException("La copia no está registrada en esta instalación.");
        CopiaRespaldoDto? diferencial = seleccionada.Tipo == TipoRespaldo.Diferencial ? seleccionada : null;
        var completa = diferencial is null ? seleccionada : copias.SingleOrDefault(c => c.Id == diferencial.BaseId) ?? throw new InvalidOperationException("Falta la copia completa de la que depende esta diferencial.");
        var esquema = await EsquemaAsync(master, opciones.BaseDatos, ct);
        var familia = await FamiliaAsync(master, ct);
        if (completa.Tipo != TipoRespaldo.Completa || completa.Familia != familia || completa.EsquemaHash != esquema || diferencial != null && (completa.SoloCopia || diferencial.Familia != familia || diferencial.EsquemaHash != esquema))
            throw new InvalidOperationException("La familia o la versión de SGTM no coincide. Para copias de otra versión se requiere recuperar en otra base y revisar con personal especializado.");
        await VerificarArchivoAsync(master, completa, ct);
        if (diferencial != null)
        {
            await VerificarArchivoAsync(master, diferencial, ct);
            if ((await LeerCabeceraAsync(master, diferencial.RutaServidor, ct)).Base != completa.BackupSetGuid)
                throw new InvalidOperationException("La diferencial no depende de la completa seleccionada.");
        }

        return (completa, diferencial, esquema);
    }

    public async Task<PlanRestauracionDto> ProbarRestauracionAsync(Guid copiaId, int solicitante, IProgress<string>? progreso = null, CancellationToken cancelacion = default)
    {
        await using var control = await ControlAsync(cancelacion);
        await using var bloqueo = await SqlRespaldos.BloquearAsync(opciones, opciones.RecursoCopias, "Exclusive", 0, cancelacion);
        await using var acceso = await AccesoNegocioAsync(cancelacion);
        await using var master = await SqlRespaldos.AbrirAsync(opciones.Master(), cancelacion);
        var p = await ComprobarPlanAsync(master, control, copiaId, cancelacion);
        var archivos = await ArchivosActualesAsync(master, cancelacion);
        await ComprobarArchivosAsync(master, p.Completa, archivos, cancelacion);
        if (p.Diferencial != null)
            await ComprobarArchivosAsync(master, p.Diferencial, archivos, cancelacion);
        string temporal = "SGTM_Verificacion_" + Guid.NewGuid().ToString("N");
        static string Carpeta(string ruta) => ruta[..Math.Max(ruta.LastIndexOf('/'), ruta.LastIndexOf('\\'))];
        var temporales = archivos.Select((a, i) => new ArchivoSql(a.Nombre, PoliticaRespaldos.Archivo(Carpeta(a.Ruta), temporal + "_" + i + (a.Tipo == 1 ? ".ldf" : ".mdf")), a.Tipo)).ToList();
        bool creada = false;
        try
        {
            await _registro.EventoAsync(control, "EnsayoRestauracion", "Iniciado", solicitante.ToString(), "", temporal + " · " + copiaId, cancelacion);
            progreso?.Report("Restaurando en una base temporal separada. Los datos de negocio no se reemplazan en este ensayo.");
            creada = true;
            await RestaurarNativaAsync(master, temporal, p.Completa, temporales, p.Diferencial == null, cancelacion);
            if (p.Diferencial != null)
                await RestaurarNativaAsync(master, temporal, p.Diferencial, temporales, true, cancelacion);
            progreso?.Report("Ejecutando CHECKDB y comparando el esquema en la base temporal.");
            await IntegridadAsync(master, temporal, cancelacion);
            if (await EsquemaAsync(master, temporal, cancelacion) != p.Esquema)
                throw new InvalidOperationException("El esquema recuperado no es compatible con la instalación actual.");
            await _registro.EventoAsync(control, "EnsayoRestauracion", "Correcto", solicitante.ToString(), "", copiaId + " · CHECKDB y esquema validados", cancelacion);
            return new PlanRestauracionDto
            {
                IdSolicitante = solicitante,
                Completa = p.Completa,
                Diferencial = p.Diferencial,
                EsquemaEsperado = p.Esquema,
                Resumen = $"Base: {opciones.BaseDatos}. Punto de copia: {(p.Diferencial ?? p.Completa).FechaServidor:dd/MM/yyyy HH:mm:ss} (hora del servidor SQL). Se reemplazarán TODOS los datos de negocio posteriores a ese punto, incluidos cobros, usuarios y contraseñas. Se ensayó la recuperación en otra base y CHECKDB no informó errores. La operación real detendrá temporalmente las operaciones, creará una copia protectora y exigirá reiniciar todas las sesiones."};
        }
        catch (Exception e)
        {
            await _registro.EventoAsync(control, "EnsayoRestauracion", "Fallido", solicitante.ToString(), "", e.Message, CancellationToken.None);
            throw;
        }
        finally
        {
            if (creada)
            {
                // Solo puede eliminarse el nombre temporal generado aquí, nunca un nombre proporcionado por el usuario.
                try
                {
                    await SqlRespaldos.EjecutarAsync(master, $"IF DB_ID(@db) IS NOT NULL DROP DATABASE {SqlRespaldos.Identificador(temporal)}", CancellationToken.None, ("@db", temporal));
                }
                catch (Exception e)
                {
                    await _registro.EventoAsync(control, "LimpiezaEnsayo", "Fallida", solicitante.ToString(), "", $"Revisar base temporal {temporal}: {e.Message}", CancellationToken.None);
                    throw new InvalidOperationException("No se pudo retirar la base temporal de verificación. Contacte al técnico antes de continuar.", e);
                }
            }
        }
    }

    private async Task RevalidarAprobadorAsync(SqlConnection master, AprobadorRestauracionDto u)
    {
        using var cmd = SqlRespaldos.Comando(master, $"SELECT u.RolId,u.Activo,u.PasswordHash,r.Activo,r.Nombre FROM {Db}.dbo.Usuarios u JOIN {Db}.dbo.Roles r ON r.Id=u.RolId WHERE u.Id=@id", ("@id", u.Id));
        using var rd = await cmd.ExecuteReaderAsync();
        if (!await rd.ReadAsync() || rd.GetInt32(0) != u.RolId || !rd.GetBoolean(1) || !rd.GetBoolean(3) || rd.GetString(4) != RolesSistema.ObtenerNombreEsperado(u.RolId) || !CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(PoliticaRespaldos.Huella(rd.GetString(2))), Encoding.UTF8.GetBytes(u.HuellaCredencial)))
            throw new InvalidOperationException("Una aprobación fue revocada o sus credenciales cambiaron. No se reemplazaron datos.");
    }

    private sealed class ConfirmacionRecuperacionException(string mensaje, Exception causa) : InvalidOperationException(mensaje, causa);
    private async Task ConfirmarFinAsync(Guid operacion, string resultado, AprobadorRestauracionDto primero, AprobadorRestauracionDto segundo, string detalle, string estado)
    {
        Exception? ultimo = null;
        // Si se perdió la respuesta después del commit, el mismo identificador confirma sin volver a incrementar la generación.
        for (int intento = 0; intento < 2; intento++)
        {
            try
            {
                await using var c = await ControlAsync();
                await _registro.ConfirmarFinAsync(c, operacion, resultado, primero.Nombre, segundo.Nombre, detalle, estado);
                return;
            }
            catch (Exception e)
            {
                ultimo = e;
            }
        }

        throw new ConfirmacionRecuperacionException("Los datos fueron recuperados y verificados, pero no se pudo confirmar el estado de control. NO se intentará otro reemplazo ni una reversión a ciegas. Cierre las sesiones y contacte al técnico para revisar la operación " + operacion + ". " + detalle, ultimo!);
    }

    public async Task<ResultadoRestauracionDto> RestaurarAsync(PlanRestauracionDto plan, AprobadorRestauracionDto primero, AprobadorRestauracionDto segundo, string motivo, IProgress<string>? progreso = null)
    {
        PoliticaRespaldos.Aprobadores(primero, segundo);
        if (plan.IdSolicitante != primero.Id || plan.VenceUtc <= reloj.GetUtcNow())
            throw new InvalidOperationException("El plan de recuperación venció o no pertenece al solicitante.");
        await using var control = await ControlAsync();
        await using var bloqueo = await SqlRespaldos.BloquearAsync(opciones, opciones.RecursoCopias, "Exclusive", 0);
        if ((await _registro.ComprobarAsync(control)).Mantenimiento)
            throw new InvalidOperationException("Existe un mantenimiento pendiente. Solicite asistencia técnica.");
        await using var master = await SqlRespaldos.AbrirAsync(opciones.Master());
        var p = await ComprobarPlanAsync(master, control, (plan.Diferencial ?? plan.Completa).Id);
        if (p.Esquema != plan.EsquemaEsperado || p.Completa.BackupSetGuid != plan.Completa.BackupSetGuid || p.Diferencial?.BackupSetGuid != plan.Diferencial?.BackupSetGuid)
            throw new InvalidOperationException("El plan cambió. Vuelva a verificarlo.");
        var archivos = await ArchivosActualesAsync(master);
        await ComprobarArchivosAsync(master, p.Completa, archivos);
        if (p.Diferencial != null)
            await ComprobarArchivosAsync(master, p.Diferencial, archivos);
        var operacion = Guid.NewGuid();
        bool tocoDestino = false, terminada = false;
        CopiaRespaldoDto? protectora = null;
        await _registro.EventoAsync(control, "RestauracionAprobada", "Preparada", $"{primero.Id} · {primero.Nombre}", $"{segundo.Id} · {segundo.Nombre}", $"Operación {operacion} · Copia {(p.Diferencial ?? p.Completa).Id}. Motivo: {motivo}");
        await _registro.EstadoAsync(control, true, "Restauración aprobada; esperando operaciones en curso", operacion: operacion);
        try
        {
            progreso?.Report("Entrando en mantenimiento. Esperando a que terminen las conexiones de negocio, sin forzar cancelaciones.");
            await using var exclusivo = await SqlRespaldos.BloquearAsync(opciones, opciones.RecursoMantenimiento, "Exclusive", opciones.EsperaMantenimientoMs);
            await RevalidarAprobadorAsync(master, primero);
            await RevalidarAprobadorAsync(master, segundo);
            if (await EsquemaAsync(master, opciones.BaseDatos) != plan.EsquemaEsperado)
                throw new InvalidOperationException("El esquema cambió después del ensayo. No se reemplazaron datos.");
            int sesiones = await SqlRespaldos.EscalarAsync<int>(master, "SELECT COUNT(*) FROM sys.dm_exec_sessions WHERE is_user_process=1 AND database_id=DB_ID(@db) AND session_id<>@@SPID", default, ("@db", opciones.BaseDatos));
            if (sesiones > 0)
                throw new InvalidOperationException("Hay conexiones externas abiertas en la base. Cierre las otras aplicaciones o SSMS y coordine una ventana de mantenimiento. No se desconectó a nadie.");
            string modo = await SqlRespaldos.EscalarAsync<string>(master, "SELECT user_access_desc FROM sys.databases WHERE name=@db AND state_desc='ONLINE' AND is_read_only=0", default, ("@db", opciones.BaseDatos));
            if (modo != "MULTI_USER")
                throw new InvalidOperationException("La base no está en estado ONLINE/MULTI_USER de lectura y escritura. Debe intervenir personal técnico.");
            string carpeta = p.Completa.RutaServidor[..Math.Max(p.Completa.RutaServidor.LastIndexOf('/'), p.Completa.RutaServidor.LastIndexOf('\\'))];
            progreso?.Report("Creando y verificando una copia protectora del estado actual, sin cambiar la base de las diferenciales.");
            protectora = await CrearNativaAsync(master, control, TipoRespaldo.Completa, carpeta, $"Protección previa · {primero.Nombre}", true, progreso, default);
            await _registro.EventoAsync(control, "ProteccionPrevia", "Correcto", primero.Nombre, segundo.Nombre, protectora.RutaServidor);
            await _registro.EstadoAsync(control, true, "Reemplazo en curso. Copia protectora: " + protectora.RutaServidor, operacion: operacion);
            // Desde antes del envío tratamos el destino como potencialmente modificado:
            // una pérdida de respuesta puede ocurrir después de aplicar SINGLE_USER.
            tocoDestino = true;
            await SqlRespaldos.EjecutarAsync(master, $"ALTER DATABASE {Db} SET SINGLE_USER WITH NO_WAIT");
            progreso?.Report("Restaurando el punto aprobado. Mantenga el equipo encendido; no cierre el proceso.");
            await RestaurarNativaAsync(master, opciones.BaseDatos, p.Completa, archivos, p.Diferencial == null);
            progreso?.Report("Copia completa aplicada; completando el punto aprobado y verificando integridad.");
            if (p.Diferencial != null)
                await RestaurarNativaAsync(master, opciones.BaseDatos, p.Diferencial, archivos, true);
            await IntegridadAsync(master, opciones.BaseDatos);
            if (await EsquemaAsync(master, opciones.BaseDatos) != plan.EsquemaEsperado)
                throw new InvalidOperationException("El esquema restaurado no coincide con el verificado.");
            await SqlRespaldos.EjecutarAsync(master, $"ALTER DATABASE {Db} SET MULTI_USER WITH NO_WAIT");
            await ConfirmarFinAsync(operacion, "Correcta", primero, segundo, $"Operación {operacion}. Protectora: {protectora.RutaServidor}", "Restauración correcta; reiniciar todas las sesiones");
            terminada = true;
            return new(true, false, protectora.RutaServidor, "Restauración completada y CHECKDB correcto. Cierre y vuelva a iniciar SGTM en todos los equipos. Las credenciales son las del punto recuperado.");
        }
        catch (ConfirmacionRecuperacionException)
        {
            terminada = true;
            throw;
        }
        catch (Exception error)
        {
            if (tocoDestino && protectora != null)
            {
                // Los demás clientes siguen bloqueados por el estado durable incluso si el bloqueo SQL del try ya se liberó.
                try
                {
                    progreso?.Report("La recuperación solicitada falló. Se está recuperando la copia protectora del estado anterior.");
                    await VerificarArchivoAsync(master, protectora);
                    string estadoDestino = await SqlRespaldos.EscalarAsync<string>(master,
                        "SELECT state_desc FROM sys.databases WHERE name=@db", default, ("@db", opciones.BaseDatos));
                    // NORECOVERY deja la base en RESTORING, donde ALTER DATABASE no está permitido.
                    // En ese estado no hay conexiones de negocio: se aplica directamente la completa protectora.
                    if (estadoDestino == "ONLINE")
                        await SqlRespaldos.EjecutarAsync(master, $"ALTER DATABASE {Db} SET SINGLE_USER WITH NO_WAIT");
                    else if (estadoDestino != "RESTORING")
                        throw new InvalidOperationException("La base quedó en estado " + estadoDestino + "; requiere recuperación técnica.");
                    await RestaurarNativaAsync(master, opciones.BaseDatos, protectora, archivos, true);
                    await IntegridadAsync(master, opciones.BaseDatos);
                    if (await EsquemaAsync(master, opciones.BaseDatos) != plan.EsquemaEsperado)
                        throw new InvalidOperationException("La copia protectora no recuperó el esquema esperado.");
                    await SqlRespaldos.EjecutarAsync(master, $"ALTER DATABASE {Db} SET MULTI_USER WITH NO_WAIT");
                    await ConfirmarFinAsync(operacion, "Anterior recuperada", primero, segundo, $"Error: {error.Message}. Protectora: {protectora.RutaServidor}", "Fallo de restauración; estado anterior recuperado; reiniciar sesiones");
                    terminada = true;
                    return new(false, true, protectora.RutaServidor, "Falló la restauración solicitada. Se recuperó y verificó el estado anterior. Reinicie las sesiones y contacte al técnico. Error: " + error.Message);
                }
                catch (ConfirmacionRecuperacionException)
                {
                    terminada = true;
                    throw;
                }
                catch (Exception recuperacion)
                {
                    await _registro.EventoAsync(control, "Restauracion", "Requiere asistencia", primero.Nombre, segundo.Nombre, $"Fallo: {error.Message}. Recuperación protectora fallida: {recuperacion.Message}. Archivo: {protectora.RutaServidor}");
                    await _registro.EstadoAsync(control, true, "Recuperación incompleta. NO operar. Copia protectora: " + protectora.RutaServidor, operacion: operacion);
                    terminada = true;
                    throw new InvalidOperationException("La recuperación no pudo completarse. SGTM permanece bloqueado para proteger los datos. Contacte al personal técnico. Copia protectora: " + protectora.RutaServidor, recuperacion);
                }
            }

            await _registro.EventoAsync(control, "Restauracion", "Cancelada antes del reemplazo", primero.Nombre, segundo.Nombre, error.Message);
            throw;
        }
        finally
        {
            if (!terminada && !tocoDestino)
                await _registro.EstadoAsync(control, false, "No se reemplazaron datos; restauración interrumpida antes de modificar la base");
        }
    }
}
