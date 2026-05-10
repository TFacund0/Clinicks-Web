using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicksApi.Data.Repositories;

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
            .Include(p => p.IdEstadoPacienteNavigation) // <--- CARGA LA TABLA DE ESTADOS
            .Include(p => p.IdDireccionNavigation) // <--- CARGA LA TABLA DE DIRECCIONES
            .Include(p => p.Turnos) // <--- CARGA LOS TURNOS PARA EL DTO
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<Paciente?> GetByIdAsync(int id)
    {
        return await _context.Pacientes
            .Include(p => p.IdEstadoPacienteNavigation) // <--- CARGA LA TABLA DE ESTADOS
            .Include(p => p.IdDireccionNavigation) // <--- CARGA LA TABLA DE DIRECCIONES
            .FirstOrDefaultAsync(p => p.IdPaciente == id);
    }

    /// <inheritdoc/>
    public async Task<Paciente?> GetByDniAsync(string dni)
    {
        return await _context.Pacientes
            .Include(p => p.IdEstadoPacienteNavigation) // <--- CARGA LA TABLA DE ESTADOS
            .Include(p => p.IdDireccionNavigation) // <--- CARGA LA TABLA DE DIRECCIONES
            .FirstOrDefaultAsync(p => p.Dni == dni);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Paciente>> GetAtendidosByMedicoAsync(int medicoId, int estadoTurnoId)
    {
        return await _context.Pacientes
            .Include(p => p.IdEstadoPacienteNavigation)
            .Include(p => p.IdDireccionNavigation)
            .Include(p => p.Turnos) // Cargamos los turnos para poder filtrar por médico
            .Where(p => p.Turnos.Any(t => t.IdMedico == medicoId && t.IdEstadoTurno == estadoTurnoId))
            .ToListAsync();
    }
}