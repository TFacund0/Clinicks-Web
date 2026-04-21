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

        public async Task<Paciente?> BuscarPorDni(string dni)
        {
            return await _context.Pacientes.FirstOrDefaultAsync(p => p.Dni == dni);
        }
    }
}
