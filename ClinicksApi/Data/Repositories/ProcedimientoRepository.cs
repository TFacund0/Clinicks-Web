using Microsoft.EntityFrameworkCore;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using ClinicksApi.Constants;

namespace ClinicksApi.Data.Repositories
{
    /// <inheritdoc/>
    public class ProcedimientoRepository : IProcedimientoRepository
    {
        private readonly ClinicksDbContext _context;

        /// <summary>
        /// Constructor del repositorio.
        /// </summary>
        public ProcedimientoRepository(ClinicksDbContext context)
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
