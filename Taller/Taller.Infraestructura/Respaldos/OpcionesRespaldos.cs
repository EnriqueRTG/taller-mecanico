using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Taller.Infraestructura.Respaldos;
/// <summary>Configuración técnica de instalación; la programación diaria se administra desde la interfaz.</summary>
public sealed class OpcionesRespaldos
{
    public bool Habilitado { get; }
    public int EsperaMantenimientoMs { get; init; } = 30000;
    public string BaseDatos { get; }
    public string BaseControl { get; }

    private readonly string _negocio;
    private readonly string _administracion;
    public OpcionesRespaldos(string negocio, string administracion, string baseControl, bool habilitado)
    {
        var n = new SqlConnectionStringBuilder(negocio);
        var a = new SqlConnectionStringBuilder(administracion);
        BaseDatos = n.InitialCatalog;
        BaseControl = baseControl;
        Habilitado = habilitado;
        _negocio = negocio;
        _administracion = administracion;
        SqlRespaldos.Identificador(BaseDatos);
        SqlRespaldos.Identificador(BaseControl);
        if (string.Equals(BaseDatos, BaseControl, StringComparison.OrdinalIgnoreCase) || !string.Equals(n.DataSource, a.DataSource, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("La conexión de mantenimiento debe apuntar al mismo servidor y la base de control debe ser distinta de la base de negocio.");
    }

    public static OpcionesRespaldos Desde(IConfiguration c, string negocio)
    {
        var db = new SqlConnectionStringBuilder(negocio).InitialCatalog;
        return new(negocio, Environment.GetEnvironmentVariable("SGTM_MANTENIMIENTO_CONN") ?? c.GetConnectionString("Mantenimiento") ?? negocio, c["Respaldos:BaseControl"] ?? db + "_Control", c.GetValue<bool>("Respaldos:Habilitado"));
    }

    private string Cadena(string original, string db) => new SqlConnectionStringBuilder(original)
    {
        InitialCatalog = db,
        Pooling = false,
        Enlist = false,
        ConnectTimeout = 15,
        ApplicationName = "SGTM-Mantenimiento"
    }.ConnectionString;
    public string Control(bool administracion = true) => Cadena(administracion ? _administracion : _negocio, BaseControl);
    public string Master() => Cadena(_administracion, "master");
    public string RecursoMantenimiento => "SGTM:Negocio:" + BaseDatos.ToUpperInvariant();
    public string RecursoCopias => "SGTM:Copias:" + BaseDatos.ToUpperInvariant();

    public void ExigirHabilitado()
    {
        if (!Habilitado)
            throw new InvalidOperationException("Respaldos no está habilitado. Un técnico debe instalar la base de control, configurar permisos y activar Respaldos:Habilitado en todos los clientes antes de utilizar el módulo.");
    }
}
