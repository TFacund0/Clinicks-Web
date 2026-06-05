using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicksApi.Data.Repositories
{
    /// <inheritdoc/>
    public class PacienteRepository : IPacienteRepository
    {
        private readonly ClinicksDbContext _context;

        /// <summary>
        /// Constructor del repositorio.
        /// </summary>
        public PacienteRepository(ClinicksDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Paciente>> ObtenerTodosAsync()
        {
            return await _context.Pacientes
                .AsNoTracking()
                .Include(p => p.IdEstadoPacienteNavigation) // <--- CARGA LA TABLA DE ESTADOS
                .Include(p => p.IdDireccionNavigation) // <--- CARGA LA TABLA DE DIRECCIONES
                .Include(p => p.Turnos.Where(t => t.FechaTurno <= DateTime.Now).OrderByDescending(t => t.FechaTurno).Take(1)) // <--- CARGA SOLO EL ÚLTIMO TURNO PASADO PARA EL DTO
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Paciente?> ObtenerPorIdAsync(int id)
        {
            return await _context.Pacientes
                .AsNoTracking()
                .Include(p => p.IdEstadoPacienteNavigation)
                .Include(p => p.IdDireccionNavigation)
                .Include(p => p.Turnos.Where(t => t.FechaTurno <= DateTime.Now).OrderByDescending(t => t.FechaTurno).Take(1))
                .FirstOrDefaultAsync(p => p.IdPaciente == id);
        }

        /// <inheritdoc/>
        public async Task<Paciente?> ObtenerPorDniAsync(string dni)
        {
            return await _context.Pacientes
                .AsNoTracking()
                .Include(p => p.IdEstadoPacienteNavigation)
                .Include(p => p.IdDireccionNavigation)
                .Include(p => p.Turnos.Where(t => t.FechaTurno <= DateTime.Now).OrderByDescending(t => t.FechaTurno).Take(1)) // Necesario para calcular FechaUltimaConsulta en el DTO
                .FirstOrDefaultAsync(p => p.Dni == dni);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Paciente>> ObtenerAtendidosPorMedico(int medicoId, string? search = null)
        {
            var query = _context.Pacientes
                .AsNoTracking()
                .Include(p => p.IdEstadoPacienteNavigation)
                .Include(p => p.IdDireccionNavigation)
                .Include(p => p.Turnos.Where(t => t.FechaTurno <= DateTime.Now).OrderByDescending(t => t.FechaTurno).Take(1))
                .Where(p => 
                    p.ConsultaMedicas.Any(c => c.IdMedico == medicoId) || 
                    p.Turnos.Any(t => t.IdMedico == medicoId && t.IdProcedimiento != null)
                );

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.ToLower();
                query = query.Where(p => 
                    p.Nombre.ToLower().Contains(searchLower) || 
                    p.Apellido.ToLower().Contains(searchLower) || 
                    p.Dni.Contains(searchLower)
                );
            }

            return await query.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> ValidarPaciente(string dni)
        {
            return await _context.Pacientes.AnyAsync(p => p.Dni == dni);
        }
    }
}