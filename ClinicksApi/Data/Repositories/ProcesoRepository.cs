using Microsoft.EntityFrameworkCore;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;

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
        public async Task<Procedimiento> CrearProcedimiento(Procedimiento procedimiento)
        {
            _context.Procedimientos.Add(procedimiento);
            await _context.SaveChangesAsync();
            return procedimiento;
        }

        /// <inheritdoc/>
        public async Task<Turno> CrearTurnoVinculado(Turno turno)
        {
            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();
            return turno;
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
    }
}
