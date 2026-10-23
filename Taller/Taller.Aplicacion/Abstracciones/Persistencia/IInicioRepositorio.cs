using Taller.Aplicacion.DTOs.Inicio;

namespace Taller.Aplicacion.Abstracciones.Persistencia;
public interface IInicioRepositorio
{
    Task<List<PendienteInicioDto>> ListarPendientesAsync(int rol, int usuario);
}
