using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicksApi.Data.Repositories
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly ClinicksDbContext _context;

        public PacienteRepository(ClinicksDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Paciente>> GetAllAsync()
        {
            return await _context.Pacientes
                .Include(p => p.IdEstadoPacienteNavigation)
                .Include(p => p.IdDireccionNavigation)
                .ToListAsync();
        }

        public async Task<Paciente?> GetByIdAsync(int id)
        {
            return await _context.Pacientes
                .Include(p => p.IdEstadoPacienteNavigation)
                .Include(p => p.IdDireccionNavigation)
                .FirstOrDefaultAsync(p => p.IdPaciente == id);
        }

        public async Task<Paciente?> GetByDniAsync(string dni)
        {
            return await _context.Pacientes
                .Include(p => p.IdEstadoPacienteNavigation)
                .Include(p => p.IdDireccionNavigation)
                .FirstOrDefaultAsync(p => p.Dni == dni);
        }

        public async Task<IEnumerable<Paciente>> GetAtendidosByMedicoAsync(int medicoId)
        {
            return await _context.Pacientes
                .Include(p => p.IdEstadoPacienteNavigation)
                .Include(p => p.IdDireccionNavigation)
                .Include(p => p.Turnos)
                .Where(p => p.Turnos.Any(t => t.IdMedico == medicoId && t.IdEstadoTurno == 3))
                .ToListAsync();
        }
    }
}