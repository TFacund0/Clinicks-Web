using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicksApi.Data.Repositories
{
    /// <inheritdoc/>
    public class MedicoRepository : IMedicoRepository
    {
        private readonly ClinicksDbContext _context;

        /// <summary>
        /// Constructor del repositorio.
        /// </summary>
        public MedicoRepository(ClinicksDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<Medico?> ObtenerPorIdAsync(int idMedico)
        {
            return await _context.Medicos
                .AsNoTracking()
                .Include(m => m.IdEspecialidadNavigation)
                .FirstOrDefaultAsync(m => m.IdMedico == idMedico);
        }

        /// <inheritdoc/>
        public async Task<Medico?> ObtenerPorUsuarioIdAsync(int usuarioId)
        {
            return await _context.Medicos
                .AsNoTracking()
                .Include(m => m.IdEspecialidadNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == usuarioId);
        }

    }
}
