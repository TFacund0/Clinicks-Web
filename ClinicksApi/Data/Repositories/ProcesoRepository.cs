using Microsoft.EntityFrameworkCore;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using ClinicksApi.Constants;

namespace ClinicksApi.Data.Repositories
{
    /// <summary>
    /// Implementación concreta del repositorio de procedimientos médicos utilizando Entity Framework Core.
    /// Gestiona la persistencia en las tablas <c>procedimiento</c>, <c>turno</c> y <c>estado_turno</c>.
    /// </summary>
    public class ProcesoRepository : IProcesoRepository
    {
        private readonly ClinicksDbContext _context;

        /// <summary>
        /// Constructor del repositorio. Recibe el contexto de base de datos inyectado por .NET.
        /// </summary>
        /// <param name="context">El contexto de EF Core que representa la sesión activa con PostgreSQL.</param>
        public ProcesoRepository(ClinicksDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<Procedimiento> RegistrarProcedimiento(Procedimiento procedimiento)
        {
            _context.Procedimientos.Add(procedimiento);
            await _context.SaveChangesAsync();
            return procedimiento;
        }

        /// <inheritdoc/>
        public async Task<List<Procedimiento>> ObtenerHistorialProcedimientosAsync(int pacienteId)
        {
            return await _context.Procedimientos
                .AsNoTracking()
                .Where(p => p.Turnos.Any(t => t.IdPaciente == pacienteId))
                .Include(p => p.Turnos) // Incluir turnos para acceder al médico
                    .ThenInclude(t => t.IdMedicoNavigation)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();
        }
    }
}
