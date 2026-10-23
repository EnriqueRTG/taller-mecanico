using Taller.Aplicacion.DTOs.Respaldos;

namespace Taller.Aplicacion.Abstracciones.Persistencia;
/// <summary>Replica archivos ya verificados. Nunca elimina el original ni crea respaldos SQL.</summary>
public interface IArchivoRespaldoNube
{
    void ValidarConfiguracion();
    Task<CopiaRespaldoDto> SubirAsync(CopiaRespaldoDto copia, CancellationToken ct = default);
    Task RecuperarArchivoAsync(CopiaRespaldoDto copia, CancellationToken ct = default);
}
