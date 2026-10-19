using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Abstracciones.Persistencia;

/// <summary>
/// Define las operaciones de persistencia necesarias
/// para trabajar con las atenciones dentro de la aplicación.
/// </summary>
public interface IAtencionRepositorio
{
    Task<Atencion?> ObtenerPorIdAsync(
        int idAtencion);
    Task<List<Atencion>> ListarActivasAsync();
    Task<List<Atencion>> ListarTodasAsync();
    Task<List<Atencion>> ListarPorClienteAsync(
        int idCliente);
    Task<List<Atencion>> ListarPorVehiculoAsync(
        int idVehiculo);
    Task AgregarAsync(
        Atencion atencion);
    Task ActualizarAsync(Atencion atencion);

    /// <summary>
    /// Actualiza la atención y registra el evento correspondiente
    /// en una misma operación de persistencia.
    /// </summary>
    Task ActualizarConHistorialAsync(
        Atencion atencion,
        HistorialAtencion historial);

    /// <summary>
    /// Prepara la actualización de la reserva técnica y agrega
    /// su evento al historial, sin confirmar los cambios.
    /// </summary>
    void PrepararCambioReservaConHistorial(
        Atencion atencion,
        HistorialAtencion historial);

    Task<bool> ExisteAtencionActivaParaVehiculoAsync(
        int idVehiculo);

    /// <summary>
    /// Guarda el envío a diagnóstico, el presupuesto inicial
    /// con su detalle y el evento del historial conjuntamente.
    /// </summary>
    Task EnviarADiagnosticoAsync(
        Atencion atencion,
        Presupuesto presupuesto,
        HistorialAtencion historial);
}
