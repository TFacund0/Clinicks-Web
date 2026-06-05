using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using ClinicksApi.Data.Interfaces;
using ClinicksApi.Constants;
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
        /// <inheritdoc/>
        public async Task<ConsultaMedica> RegistrarConsulta(ConsultaMedica consulta)
        {
            _context.ConsultaMedicas.Add(consulta);
            await _context.SaveChangesAsync();
            return consulta;
        }

        /// <inheritdoc/>
        public async Task CrearTurnoVinculado(Turno turno)
        {
            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task ActualizarTurnoVinculado(int idTurno, int idConsulta)
        {
            var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.IdTurno == idTurno);
            if (turno != null)
            {
                turno.IdConsulta = idConsulta;
                turno.IdEstadoTurno = ConstantesGenerales.EstadosTurno.RealizadoId;
                await _context.SaveChangesAsync();
            }
        }
    }
}