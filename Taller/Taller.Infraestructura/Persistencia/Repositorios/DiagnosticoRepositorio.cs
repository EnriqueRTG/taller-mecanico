using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
using Taller.Aplicacion.DTOs.Diagnosticos;

namespace Taller.Infraestructura.Persistencia.Repositorios;

/// <summary>
/// Consulta los informes técnicos definitivos.
/// </summary>
/// <remarks>
/// Las consultas no mantienen seguimiento.
/// </remarks>
public sealed class DiagnosticoRepositorio : IDiagnosticoRepositorio
{
    private readonly TallerDbContext _contexto;

    public DiagnosticoRepositorio(
        TallerDbContext contexto)
    {
        _contexto = contexto
            ?? throw new ArgumentNullException(nameof(contexto));
    }

    public Task<DiagnosticoDetalleDto?> ObtenerDetalleAsync(
        int idDiagnostico)
    {
        return _contexto.Diagnosticos
            .AsNoTracking()
            .Where(d => d.IdDiagnostico == idDiagnostico)
            .Select(d => new DiagnosticoDetalleDto
            {
                IdDiagnostico = d.IdDiagnostico,
                IdAtencion = d.IdAtencion,
                FechaDiagnostico = d.FechaDiagnostico,

                TecnicoAutor =
                    d.TecnicoAutor.Nombre + " " +
                    d.TecnicoAutor.Apellido,

                FallaDetectada = d.FallaDetectada,
                TrabajoRecomendado = d.TrabajoRecomendado,
                Kilometraje = d.Kilometraje,
                Observaciones = d.Observaciones
            })
            .FirstOrDefaultAsync();
    }

}
