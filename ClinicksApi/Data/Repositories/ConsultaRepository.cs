using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicksApi.Data.Repositories
{
    /// <summary>
    /// Implementacion concreta del repositorio de consultas medicas utilizando Entity Framework Core.
    /// Es la unica clase autorizada para ejecutar consultas SQL directas sobre la tabla consulta_medica.
    /// </summary>
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly ClinicksDbContext _context;

        /// <summary>
        /// Constructor del repositorio. Recibe el contexto de base de datos inyectado por .NET.
        /// </summary>
        /// <param name="context">El contexto de EF Core que representa la sesion activa con PostgreSQL.</param>
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
        public async Task<List<ConsultaMedica>> HistorialPaciente(int pacienteId)
        {
            return await _context.ConsultaMedicas
                .AsNoTracking()
                .Where(c => c.IdPaciente == pacienteId)
                .Include(c => c.IdMedicoNavigation) // JOIN con la tabla medico para traer nombre y apellido
                .OrderByDescending(c => c.FechaConsulta)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<ConsultaMedica> CrearConsulta(ConsultaMedica consulta)
        {
            _context.ConsultaMedicas.Add(consulta);
            await _context.SaveChangesAsync();
            return consulta;
        }
    }
}