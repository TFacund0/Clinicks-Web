using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using ClinicksApi.Data.Interfaces;
using ClinicksApi.Constants;
using Microsoft.EntityFrameworkCore;

namespace ClinicksApi.Data.Repositories
{
    /// <inheritdoc/>
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly ClinicksDbContext _context;

        /// <summary>
        /// Constructor del repositorio.
        /// </summary>
        public ConsultaRepository(ClinicksDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<List<ConsultaMedica>> ListaConsultas()
        {
            return await _context.ConsultaMedicas.AsNoTracking().ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ConsultaMedica> RegistrarConsulta(ConsultaMedica consulta)
        {
            _context.ConsultaMedicas.Add(consulta);
            await _context.SaveChangesAsync();
            return consulta;
        }

        /// <inheritdoc/>
        public async Task<List<ConsultaMedica>> ObtenerHistorialConsultasAsync(int pacienteId)
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