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

        /// <inheritdoc/>
        public async Task<int> AsegurarEstadoTurnoExiste(string nombreEstado)
        {
            var estadoExistente = await _context.EstadoTurnos
                .FirstOrDefaultAsync(e => e.Nombre.ToLower() == nombreEstado.ToLower());
            
            if (estadoExistente != null)
            {
                return estadoExistente.IdEstadoTurno;
            }

            var nuevoEstado = new EstadoTurno
            {
                Nombre = nombreEstado
            };

            _context.EstadoTurnos.Add(nuevoEstado);
            await _context.SaveChangesAsync();

            return nuevoEstado.IdEstadoTurno;
        }

        /// <inheritdoc/>
        public async Task CrearTurnoVinculado(Turno turno)
        {
            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<ConsultaMedica> CrearConsultaYTurnoVinculado(ConsultaMedica consulta, Turno turno)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Guardar la consulta
                _context.ConsultaMedicas.Add(consulta);
                await _context.SaveChangesAsync(); // Guarda y obtiene el ID autoincremental de consulta

                // 2. Vincular el ID de la consulta generada al Turno
                turno.IdConsulta = consulta.IdConsulta;

                // 3. Guardar el turno
                _context.Turnos.Add(turno);
                await _context.SaveChangesAsync();

                // 4. Confirmar la transacción
                await transaction.CommitAsync();

                return consulta;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<ConsultaMedica> CrearConsultaYVincularATurnoExistente(ConsultaMedica consulta, int idTurno)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Guardar la consulta
                _context.ConsultaMedicas.Add(consulta);
                await _context.SaveChangesAsync(); // Guarda y obtiene el ID autoincremental de consulta

                // 2. Buscar el turno existente
                var turnoExistente = await _context.Turnos.FirstOrDefaultAsync(t => t.IdTurno == idTurno);
                if (turnoExistente == null)
                {
                    throw new Exception($"El turno con ID {idTurno} no existe.");
                }

                // 3. Vincular la consulta y actualizar el estado a "Atendido" (ID = 2)
                turnoExistente.IdConsulta = consulta.IdConsulta;
                var estadoAtendido = await _context.EstadoTurnos.FirstOrDefaultAsync(e => e.Nombre.ToLower() == "atendido");
                turnoExistente.IdEstadoTurno = estadoAtendido?.IdEstadoTurno ?? 2;

                _context.Turnos.Update(turnoExistente);
                await _context.SaveChangesAsync();

                // 4. Confirmar la transacción
                await transaction.CommitAsync();

                return consulta;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}