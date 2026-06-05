using Microsoft.EntityFrameworkCore;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;

namespace ClinicksApi.Data.Repositories
{
    /// <summary>
    /// Implementación concreta del repositorio para la entidad Turno, utilizando Entity Framework Core para acceder a la base de datos PostgreSQL.
    /// </summary>
    public class TurnoRepository : ITurnoRepository
    {
        private readonly ClinicksDbContext _context;

        /// <summary>
        /// Constructor del repositorio de Turnos. Recibe el contexto de base de datos inyectado por .NET, que representa la sesión activa con PostgreSQL.
        /// </summary>
        /// <param name="context">El contexto de EF Core para acceder a la base de datos.</param>
        public TurnoRepository(ClinicksDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Turno>> ObtenerTodosAsync()
        {
            var turnos = await _context.Turnos
            .Include(t => t.IdPacienteNavigation)
            .Include(t => t.IdMedicoNavigation)
            .Include(t => t.IdEstadoTurnoNavigation)
            .Include(t => t.IdProcedimientoNavigation)
            .Include(t => t.IdConsultaNavigation)
            .ToListAsync();

            return turnos;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Turno>> ObtenerTurnosMedicoAsync(int idMedico, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var query = _context.Turnos
                .Include(t => t.IdPacienteNavigation)
                .Include(t => t.IdEstadoTurnoNavigation)
                .Where(t => t.IdMedico == idMedico);

            if (fechaInicio.HasValue)
            {
                query = query.Where(t => t.FechaTurno.Date >= fechaInicio.Value.Date);
            }

            if (fechaFin.HasValue)
            {
                query = query.Where(t => t.FechaTurno.Date <= fechaFin.Value.Date);
            }

            return await query.OrderBy(t => t.FechaTurno).ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Turno?> ObtenerPorIdAsync(int idTurno)
        {
            return await _context.Turnos
                .Include(t => t.IdPacienteNavigation)
                .Include(t => t.IdMedicoNavigation)
                .Include(t => t.IdEstadoTurnoNavigation)
                .FirstOrDefaultAsync(t => t.IdTurno == idTurno);
        }

        public async Task CrearTurnoAsync(Turno turno)
        {
            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarTurnoAsync(Turno turno)
        {
            _context.Turnos.Update(turno);
            await _context.SaveChangesAsync();
        }
    }
}