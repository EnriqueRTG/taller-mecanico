namespace Taller.Dominio.Tiempo;

/// <summary>
/// Tiempo operativo del taller argentino, independiente de la zona de cada PC.
/// Los DateTime de negocio representan hora civil de Argentina (Kind Unspecified);
/// los eventos técnicos DateTimeOffset conservan su instante UTC.
/// </summary>
public static class FechaHoraTaller
{
    public const string NombreZona = "Argentina";
    public static TimeZoneInfo Zona { get; } = ObtenerZona();
    public static DateTime Ahora => DesdeUtc(DateTimeOffset.UtcNow).DateTime;
    public static DateTime Hoy => Ahora.Date;
    public static DateTimeOffset DesdeUtc(DateTimeOffset instante) => TimeZoneInfo.ConvertTime(instante, Zona);

    /// <summary>Convierte una hora civil del taller en un instante, sin consultar la zona de la PC.</summary>
    public static DateTimeOffset AUTC(DateTime fechaLocal)
    {
        var civil = DateTime.SpecifyKind(fechaLocal, DateTimeKind.Unspecified);
        if (Zona.IsInvalidTime(civil) || Zona.IsAmbiguousTime(civil))
            throw new ArgumentException("La fecha es inexistente o ambigua en la zona horaria del taller.", nameof(fechaLocal));
        return new DateTimeOffset(TimeZoneInfo.ConvertTimeToUtc(civil, Zona));
    }

    private static TimeZoneInfo ObtenerZona()
    {
        if (TimeZoneInfo.TryFindSystemTimeZoneById("America/Argentina/Buenos_Aires", out var zona))
            return zona;
        return TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time");
    }
}
