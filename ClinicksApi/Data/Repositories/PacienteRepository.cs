using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicksApi.Data.Repositories;

public class PacienteRepository : IPacienteRepository
{
    private readonly ClinicksDbContext _context;

    // Inyectamos el DbContext que configuramos antes
    public PacienteRepository(ClinicksDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Paciente>> GetAllAsync()
    {
        return await _context.Pacientes
            .Include(p => p.IdEstadoPacienteNavigation) // <--- CARGA LA TABLA DE ESTADOS
            .Include(p => p.IdDireccionNavigation) // <--- CARGA LA TABLA DE DIRECCIONES
            .ToListAsync();
    }

    public async Task<Paciente?> GetByIdAsync(int id)
    {
        return await _context.Pacientes
            .Include(p => p.IdEstadoPacienteNavigation) // <--- CARGA LA TABLA DE ESTADOS
            .Include(p => p.IdDireccionNavigation) // <--- CARGA LA TABLA DE DIRECCIONES
            .FirstOrDefaultAsync(p => p.IdPaciente == id);
    }

    public async Task<Paciente?> GetByDniAsync(string dni)
    {
        return await _context.Pacientes
            .Include(p => p.IdEstadoPacienteNavigation) // <--- CARGA LA TABLA DE ESTADOS
            .Include(p => p.IdDireccionNavigation) // <--- CARGA LA TABLA DE DIRECCIONES
            .FirstOrDefaultAsync(p => p.Dni == dni);
    }

    public async Task<IEnumerable<Paciente>> GetAtendidosByMedicoAsync(int medicoId)
    {
        // Buscamos los pacientes que tengan al menos un turno con ese médicoId
        return await _context.Pacientes
            .Include(p => p.IdEstadoPacienteNavigation)
            .Include(p => p.IdDireccionNavigation)
            .Include(p => p.Turnos) // Cargamos los turnos para poder filtrar por médico
            .Where(p => p.Turnos.Any(t => t.IdMedico == medicoId && t.IdEstadoTurno == 3))
            .ToListAsync();
    }
}