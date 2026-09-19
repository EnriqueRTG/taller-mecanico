using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Abstracciones.Persistencia;

public interface IReporteRepositorio
{
    Task<List<Usuario>> ObtenerUsuariosReporteAsync(
            string busqueda,
            string estado,
            DateTime fechaDesde,
            DateTime fechaHasta);
}
