using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicksApi.Data.Repositories
{
    /// <summary>
    /// Implementación concreta del repositorio de Pacientes.
    /// Utiliza Entity Framework Core para traducir llamadas de C# a comandos SQL en PostgreSQL.
    /// </summary>
    public class PacienteRepository : IPacienteRepository
    {
        private readonly ClinicksDbContext _context;

        /// <summary>
        /// Inyecta el contexto de base de datos (sesión física con PostgreSQL).
        /// </summary>
        public PacienteRepository(ClinicksDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Paciente>> GetAllAsync()
        {
            return await _context.Pacientes
                .AsNoTracking()
                .Include(p => p.IdEstadoPacienteNavigation) // <--- CARGA LA TABLA DE ESTADOS
                .Include(p => p.IdDireccionNavigation) // <--- CARGA LA TABLA DE DIRECCIONES
                .Include(p => p.Turnos.Where(t => t.FechaTurno <= DateTime.Now).OrderByDescending(t => t.FechaTurno).Take(1)) // <--- CARGA SOLO EL ÚLTIMO TURNO PASADO PARA EL DTO
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Paciente?> GetByIdAsync(int id)
        {
            return await _context.Pacientes
                .AsNoTracking()
                .Include(p => p.IdEstadoPacienteNavigation)
                .Include(p => p.IdDireccionNavigation)
                .Include(p => p.Turnos.Where(t => t.FechaTurno <= DateTime.Now).OrderByDescending(t => t.FechaTurno).Take(1))
                .FirstOrDefaultAsync(p => p.IdPaciente == id);
        }

        /// <inheritdoc/>
        public async Task<Paciente?> GetByDniAsync(string dni)
        {
            return await _context.Pacientes
                .AsNoTracking()
                .Include(p => p.IdEstadoPacienteNavigation)
                .Include(p => p.IdDireccionNavigation)
                .Include(p => p.Turnos.Where(t => t.FechaTurno <= DateTime.Now).OrderByDescending(t => t.FechaTurno).Take(1)) // Necesario para calcular FechaUltimaConsulta en el DTO
                .FirstOrDefaultAsync(p => p.Dni == dni);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Paciente>> GetAtendidosByMedicoAsync(int medicoId, string? searchTerm = null)
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

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var search = searchTerm.ToLower();
                query = query.Where(p => 
                    p.Nombre.ToLower().Contains(search) || 
                    p.Apellido.ToLower().Contains(search) || 
                    p.Dni.Contains(search)
                );
            }

            return await query.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> ExistePacientePorDniAsync(string dni)
        {
            return await _context.Pacientes.AnyAsync(p => p.Dni == dni);
        }

        /// <inheritdoc/>
        public async Task<List<Turno>> GetHistorialTurnosAsync(int pacienteId)
        {
            return await _context.Turnos
                .AsNoTracking()
                .Include(t => t.IdProcedimientoNavigation)
                .Include(t => t.IdMedicoNavigation)
                .Where(t => t.IdPaciente == pacienteId && t.IdProcedimiento != null)
                .OrderByDescending(t => t.FechaTurno)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<ConsultaMedica>> GetHistorialConsultasAsync(int pacienteId)
        {
            return await _context.ConsultaMedicas
                .AsNoTracking()
                .Include(c => c.IdMedicoNavigation)
                .Where(c => c.IdPaciente == pacienteId)
                .OrderByDescending(c => c.FechaConsulta)
                .ToListAsync();
        }
    }
}