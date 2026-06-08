using Microsoft.EntityFrameworkCore;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;

namespace ClinicksApi.Data.Repositories
{
    /// <inheritdoc/>
    public class TurnoRepository : ITurnoRepository
    {
        private readonly ClinicksDbContext _context;

        /// <summary>
        /// Constructor del repositorio.
        /// </summary>
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
            // Funcion Almacenada
            var query = _context.Turnos
                .FromSqlRaw("SELECT * FROM ObtenerTurnosPorMedico({0})", idMedico)
                .Include(t => t.IdPacienteNavigation)
                .Include(t => t.IdEstadoTurnoNavigation)
                .AsQueryable();

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

        /// <inheritdoc/>
        public async Task<Turno?> ObtenerParaActualizarAsync(int idTurno)
        {
            return await _context.Turnos
                .FirstOrDefaultAsync(t => t.IdTurno == idTurno);
        }

        /// <inheritdoc/>
        public async Task<Turno?> ObtenerTurnoPendienteDelDiaAsync(int idPaciente, int idMedico, DateTime fecha)
        {
            return await _context.Turnos
                .Where(t => t.IdPaciente == idPaciente 
                         && t.IdMedico == idMedico 
                         && t.FechaTurno.Date == fecha.Date
                         && (t.IdEstadoTurnoNavigation.Nombre.ToLower() == "confirmado" || t.IdEstadoTurnoNavigation.Nombre.ToLower() == "pendiente"))
                .OrderBy(t => t.FechaTurno)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task CrearTurnoAsync(Turno turno)
        {
            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task ActualizarTurnoAsync(Turno turno)
        {
            // Procedimiento Almacenado
            await _context.Database.ExecuteSqlRawAsync(
                "CALL ActualizarEstadoTurnoSP({0}, {1}, {2}, {3})", 
                turno.IdTurno, 
                turno.IdEstadoTurno,
                turno.IdConsulta,
                turno.IdProcedimiento
            );
        }

        /// <inheritdoc/>
        public async Task<int?> ObtenerIdEstadoPorNombreAsync(string nombre)
        {
            var estado = await _context.EstadoTurnos.FirstOrDefaultAsync(e => e.Nombre.ToLower() == nombre.ToLower());
            return estado?.IdEstadoTurno;
        }

        /// <inheritdoc/>
        public async Task<List<int>> ObtenerIdsEstadosPorNombresAsync(List<string> nombres)
        {
            var nombresLower = nombres.Select(n => n.ToLower()).ToList();
            return await _context.EstadoTurnos
                .Where(e => nombresLower.Contains(e.Nombre.ToLower()))
                .Select(e => e.IdEstadoTurno)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Turno>> ObtenerTurnosPorFechaYEstadosAsync(DateTime fechaLimite, List<int> estadosIds)
        {
            return await _context.Turnos
                .Where(t => t.FechaTurno.Date < fechaLimite && estadosIds.Contains(t.IdEstadoTurno))
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task ActualizarLoteTurnosAsync(List<Turno> turnos)
        {
            await _context.SaveChangesAsync();
        }
    }
}