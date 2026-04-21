using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicksApi.Data.Repositories
{
    public class ConsultaRepository : IConsultaRepository
    {
        private readonly ClinicksDbContext _context;

        public ConsultaRepository(ClinicksDbContext context)
        {
            _context = context;
        }

        public async Task<List<ConsultaMedica>> ListaConsultas()
        {
            return await _context.ConsultaMedicas.ToListAsync();
        }

        public async Task<List<ConsultaMedica>> HistorialPaciente(int pacienteId)
        {
            return await _context.ConsultaMedicas
                .Where(c => c.IdPaciente == pacienteId)
                .Include(c => c.IdMedicoNavigation) // Trae los datos del médico que lo atendió
                .OrderByDescending(c => c.FechaConsulta)
                .ToListAsync();
        }

        public async Task<ConsultaMedica> CrearConsulta(ConsultaMedica consulta)
        {
            try
            {
                _context.ConsultaMedicas.Add(consulta);
                await _context.SaveChangesAsync();
                return consulta;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR DE BD: " + ex.InnerException?.Message);
                throw;
            }
        }
    }
}