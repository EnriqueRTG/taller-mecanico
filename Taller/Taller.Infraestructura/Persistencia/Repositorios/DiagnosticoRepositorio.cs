using Microsoft.EntityFrameworkCore;
using Taller.Aplicacion.Abstracciones.Persistencia;
using Taller.Dominio.Entidades;
using Taller.Dominio.Enumeraciones;
using Taller.Aplicacion.DTOs.Diagnosticos;

namespace Taller.Infraestructura.Persistencia.Repositorios;

/// <summary>
/// Implementa las consultas y la preparación de nuevos
/// informes técnicos.
/// </summary>
/// <remarks>
/// Las consultas no mantienen seguimiento.
/// Las inserciones se confirman mediante la unidad de trabajo.
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

    public Task<Diagnostico?> ObtenerPorIdAsync(
        int idDiagnostico)
    {
        return _contexto.Diagnosticos
            .AsNoTracking()
            .FirstOrDefaultAsync(d =>
                d.IdDiagnostico == idDiagnostico);
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
                Observaciones = d.Observaciones,
                Tipo = d.Tipo,
                ImpideContinuar = d.ImpideContinuar
            })
            .FirstOrDefaultAsync();
    }

    public Task<List<Diagnostico>> ListarPorAtencionAsync(
        int idAtencion)
    {
        return _contexto.Diagnosticos
            .AsNoTracking()
            .Where(d => d.IdAtencion == idAtencion)
            .OrderBy(d => d.FechaDiagnostico)
            .ThenBy(d => d.IdDiagnostico)
            .ToListAsync();
    }

    public Task<List<DiagnosticoBandejaDto>> ListarBandejaAsync()
    {
        var consulta =
            from atencion in _contexto.Atenciones.AsNoTracking()

            join diagnostico in _contexto.Diagnosticos
                    .AsNoTracking()
                    .Where(d => d.Tipo == TipoDiagnostico.Inicial)
                on atencion.IdAtencion equals diagnostico.IdAtencion
                into informes

            from diagnostico in informes.DefaultIfEmpty()

            where
                atencion.Estado == EstadoAtencion.PendienteDiagnostico
                || atencion.ActividadTecnica == ActividadTecnica.Diagnostico
                || diagnostico != null

            select new DiagnosticoBandejaDto
            {
                IdAtencion = atencion.IdAtencion,

                IdDiagnostico = diagnostico == null
                    ? (int?)null
                    : diagnostico.IdDiagnostico,

                Cliente =
                    atencion.Cliente.Apellido + ", " +
                    atencion.Cliente.Nombre,

                Vehiculo =
                    atencion.Vehiculo.Modelo.Marca.Nombre + " " +
                    atencion.Vehiculo.Modelo.Nombre,

                Dominio = atencion.Vehiculo.Dominio,
                MotivoConsulta = atencion.MotivoConsulta,
                EstadoAtencion = atencion.Estado,

                IdTecnicoEnActividad = atencion.IdTecnicoEnActividad,
                ActividadTecnica = atencion.ActividadTecnica,

                TecnicoEnActividad = atencion.TecnicoEnActividad == null
                    ? null
                    : atencion.TecnicoEnActividad.Nombre + " " +
                      atencion.TecnicoEnActividad.Apellido,

                IdTecnicoAutor = diagnostico == null
                    ? (int?)null
                    : diagnostico.IdTecnicoAutor,

                TecnicoAutor = diagnostico == null
                    ? null
                    : diagnostico.TecnicoAutor.Nombre + " " +
                      diagnostico.TecnicoAutor.Apellido,

                FechaApertura = atencion.FechaApertura,

                FechaTomaDiagnostico =
                    atencion.ActividadTecnica == ActividadTecnica.Diagnostico
                    && atencion.IdTecnicoEnActividad != null
                        ? atencion.Historial
                            .Where(h =>
                                h.Accion == "ActividadTomada"
                                && h.EstadoAnterior
                                    == EstadoAtencion.PendienteDiagnostico
                                && h.EstadoNuevo
                                    == EstadoAtencion.EnEvaluacionTecnica
                                && h.IdUsuario
                                    == atencion.IdTecnicoEnActividad)
                            .OrderByDescending(h => h.Fecha)
                            .ThenByDescending(h => h.IdHistorialAtencion)
                            .Select(h => (DateTime?)h.Fecha)
                            .FirstOrDefault()
                        : null,

                FechaDiagnostico = diagnostico == null
                    ? (DateTime?)null
                    : diagnostico.FechaDiagnostico
            };

        return consulta
            .OrderBy(d => d.IdAtencion)
            .ToListAsync();
    }

    public Task<bool> ExisteInicialAsync(
        int idAtencion)
    {
        return _contexto.Diagnosticos
            .AnyAsync(d =>
                d.IdAtencion == idAtencion
                && d.Tipo == TipoDiagnostico.Inicial);
    }

    public void Agregar(
        Diagnostico diagnostico)
    {
        ArgumentNullException.ThrowIfNull(diagnostico);

        if (diagnostico.IdDiagnostico != 0)
        {
            throw new ArgumentException(
                "El diagnóstico debe ser nuevo.",
                nameof(diagnostico));
        }

        // Marca únicamente el informe como nuevo,
        // sin incorporar todo su grafo de navegaciones.
        _contexto.Entry(diagnostico).State = EntityState.Added;
    }
}