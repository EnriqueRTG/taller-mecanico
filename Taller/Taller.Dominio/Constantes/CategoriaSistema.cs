namespace Taller.Dominio.Constantes;

/// <summary>
/// Define las categorias fijas por el sistema.
/// </summary>
/// 
public static class CategoriaSistema
{
    public const int MantenimientoPreventivoId = 1;
    public const int ReparacionMecanicaId = 2;
    public const int DiagnosticoElectronicoId = 3;
    public const int SistemaFrenosId = 4;
    public const int SuspensionId = 5;
    public const int DireccionId = 6;
    public const int TransmisionId = 7;
    public const int EmbragueId = 8;
    public const int SistemaRefrigeracionId = 9;
    public const int DiagnosticoComputarizadoId = 10;
    public const int SistemaArranqueCargaId = 11;
    public const int BateriasId = 12;
    public const int ElectricidadIluminacionId = 13;
    public const int AireAcondicionadoId = 14;
    public const int CalefaccionId = 15;
    public const int DiagnosticoTecnicoInicialId = 16;


    public const string MantenimientoPreventivo = "Mantenimiento preventivo";
    public const string ReparacionMecanica = "Reparación mecánica";
    public const string DiagnosticoElectronico = "Diagnóstico electrónico";
    public const string SistemaFrenos = "Sistema de frenos";
    public const string Suspension = "Suspensión";
    public const string Direccion = "Dirección";
    public const string Transmision = "Transmisión";
    public const string Embrague = "Embrague";
    public const string SistemaRefrigeracion = "Sistema de refrigeración";
    public const string DiagnosticoComputarizado = "Diagnóstico computarizado";
    public const string SistemaArranqueCarga = "Sistema de arranque y carga";
    public const string Baterias = "Baterías";
    public const string ElectricidadIluminacion = "Electricidad e iluminación";
    public const string AireAcondicionado = "Aire acondicionado";
    public const string Calefaccion = "Calefacción";
    public const string DiagnosticoTecnicoInicial = "Diagnóstico técnico inicial";

    //ver como agregar por defecto ese, ya que si o si es un presupuesto
    //public const string DiagnosticoInspeccion = "Servicios de Diagnóstico e Inspección";

    /// <summary>
    /// Indica si el identificador corresponde a una de las categorias definidos por el sistema.
    /// </summary>
    public static bool EsCategoriaValida(
        int categoriaId)
    {
        return categoriaId == MantenimientoPreventivoId
        || categoriaId == ReparacionMecanicaId
        || categoriaId == DiagnosticoElectronicoId
        || categoriaId == SistemaFrenosId
        || categoriaId == SuspensionId
        || categoriaId == DireccionId
        || categoriaId == TransmisionId
        || categoriaId == EmbragueId
        || categoriaId == SistemaRefrigeracionId
        || categoriaId == DiagnosticoComputarizadoId
        || categoriaId == SistemaArranqueCargaId
        || categoriaId == BateriasId
        || categoriaId == ElectricidadIluminacionId
        || categoriaId == AireAcondicionadoId
        || categoriaId == CalefaccionId
        || categoriaId == DiagnosticoTecnicoInicialId;
    }

    /// <summary>
    /// Obtiene el nombre esperado para una categoria del sistema.
    /// </summary>
    public static string? ObtenerNombreEsperado(
        int categoriaId)
    {
        return categoriaId switch
        {
            MantenimientoPreventivoId => MantenimientoPreventivo,
            ReparacionMecanicaId => ReparacionMecanica,
            DiagnosticoElectronicoId => DiagnosticoElectronico,
            SistemaFrenosId => SistemaFrenos,
            SuspensionId => Suspension,
            DireccionId => Direccion,
            TransmisionId => Transmision,
            EmbragueId => Embrague,
            SistemaRefrigeracionId => SistemaRefrigeracion,
            DiagnosticoComputarizadoId => DiagnosticoComputarizado,
            SistemaArranqueCargaId => SistemaArranqueCarga,
            BateriasId => Baterias,
            ElectricidadIluminacionId => ElectricidadIluminacion,
            AireAcondicionadoId => AireAcondicionado,
            CalefaccionId => Calefaccion,
            DiagnosticoTecnicoInicialId => DiagnosticoTecnicoInicial,
            _ => null
        };
    }
}
