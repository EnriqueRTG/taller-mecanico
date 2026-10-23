using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Aplicacion.DTOs.Respaldos;
using Taller.Aplicacion.Excepciones;
using Taller.Aplicacion.Respaldos;
using Taller.Dominio.Constantes;
using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Servicios;
/// <summary>Autoriza la administración de copias y exige dos identidades reautenticadas para reemplazar datos.</summary>
public sealed class RespaldoServicio(IRespaldoMotor motor, IUsuarioRepositorio usuarios, AutenticacionServicio autenticacion, SesionUsuario sesion, PlanesRestauracion planes, TimeProvider reloj)
{
    public bool Habilitado => motor.Habilitado;

    private async Task<Usuario> AdministradorAsync()
    {
        var u = sesion.UsuarioActual is { } actual ? await usuarios.ObtenerPorIdAsync(actual.Id) : null;
        if (u is null || !u.Activo || u.RolId != RolesSistema.AdministradorId || u.Rol is null || !u.Rol.Activo || u.Rol.Nombre != RolesSistema.Administrador)
            throw new ValidacionException("Se requiere una sesión de administrador activa.");
        return u;
    }

    public async Task<EstadoRespaldosDto> ConsultarAsync()
    {
        await AdministradorAsync();
        return await motor.ConsultarAsync();
    }

    public async Task<ProgramaRespaldoDto> ObtenerProgramaAsync()
    {
        await AdministradorAsync();
        return await motor.ObtenerProgramaAsync();
    }

    public async Task<CopiaRespaldoDto> CrearAsync(TipoRespaldo tipo, string carpeta, IProgress<string>? progreso = null, bool copiarAzure = false)
    {
        var u = await AdministradorAsync();
        PoliticaRespaldos.Tipo(tipo);
        carpeta = PoliticaRespaldos.Carpeta(carpeta);
        return await motor.CrearConDestinoAsync(tipo, carpeta, $"{u.Id} · {u.Apellido}, {u.Nombre}", copiarAzure, progreso);
    }

    public async Task ReintentarAzureAsync(Guid id)
    {
        var u = await AdministradorAsync();
        await motor.ReintentarAzureAsync(id, $"{u.Id} · {u.Apellido}, {u.Nombre}");
    }

    public async Task GuardarProgramaAsync(ProgramaRespaldoDto p)
    {
        var u = await AdministradorAsync();
        PoliticaRespaldos.Zona(p);
        await motor.GuardarProgramaAsync(p with { CarpetaServidor = PoliticaRespaldos.Carpeta(p.CarpetaServidor), IdAdministrador = u.Id, Autor = $"{u.Id} · {u.Apellido}, {u.Nombre}", ProximaUtc = p.Activo ? PoliticaRespaldos.Proxima(p, reloj.GetUtcNow()) : null });
    }

    public async Task<PlanRestauracionDto> ProbarRestauracionAsync(Guid copia, IProgress<string>? progreso = null)
    {
        var u = await AdministradorAsync();
        return planes.Agregar(await motor.ProbarRestauracionAsync(copia, u.Id, progreso));
    }

    public async Task<ResultadoRestauracionDto> RestaurarAsync(Guid token, string usuarioPrimero, string clavePrimero, string usuarioSegundo, string claveSegundo, string motivo, bool reconoceConsecuencias, IProgress<string>? progreso = null)
    {
        var solicitante = await AdministradorAsync();
        planes.Obtener(token, solicitante.Id);
        if (!reconoceConsecuencias)
            throw new ValidacionException("Debe reconocer que se reemplazarán los datos y que se requiere una ventana de mantenimiento.");
        motivo = (motivo ?? "").Trim();
        if (motivo.Length is < 10 or > 1000)
            throw new ValidacionException("Indique un motivo entre 10 y 1000 caracteres.");
        var primero = await autenticacion.AutenticarAsync(usuarioPrimero, clavePrimero);
        var segundo = await autenticacion.AutenticarAsync(usuarioSegundo, claveSegundo);
        if (primero is null || segundo is null || primero.Id != solicitante.Id)
            throw new ValidacionException("No se pudo validar la doble aprobación. El primer usuario debe ser el administrador de esta sesión.");
        static AprobadorRestauracionDto Datos(Usuario u) => new(u.Id, u.RolId, $"{u.Apellido}, {u.Nombre}", PoliticaRespaldos.Huella(u.PasswordHash));
        var a = Datos(primero);
        var b = Datos(segundo);
        PoliticaRespaldos.Aprobadores(a, b);
        return await motor.RestaurarAsync(planes.Consumir(token, solicitante.Id), a, b, motivo, progreso);
    }
}
