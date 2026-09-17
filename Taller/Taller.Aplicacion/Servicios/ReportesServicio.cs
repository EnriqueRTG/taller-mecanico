using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;

namespace Taller.Aplicacion.Servicios
{
    public class ReporteServicio
    {
        private readonly IReporteRepositorio _reporteRepositorio;

        public ReporteServicio(
            IReporteRepositorio reporteRepositorio)
        {
            _reporteRepositorio = reporteRepositorio;
        }

        public async Task<List<Usuario>> ObtenerUsuariosReporteAsync(
            string busqueda,
            string estado,
            DateTime fechaDesde,
            DateTime fechaHasta)
        {
            if (fechaDesde.Date > fechaHasta.Date)
            {
                throw new ArgumentException(
                    "La fecha desde no puede ser mayor que la fecha hasta.");
            }

            return await _reporteRepositorio
                .ObtenerUsuariosReporteAsync(
                    busqueda,
                    estado,
                    fechaDesde,
                    fechaHasta);
        }
    }
}