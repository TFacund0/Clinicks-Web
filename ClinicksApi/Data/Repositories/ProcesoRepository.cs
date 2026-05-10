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
        public async Task AsegurarEstadoTurnoExiste(int idEstadoTurno, string nombreEstado)
        {
            // Si el estado ya existe por ID, no hacemos nada.
            var existePorId = await _context.EstadoTurnos.AnyAsync(e => e.IdEstadoTurno == idEstadoTurno);
            if (existePorId) return;

            // Intentamos insertar el estado forzando el ID. En PostgreSQL con columna IDENTITY, 
            // esto puede fallar si la secuencia no lo permite, por eso capturamos el error.
            var nuevoEstado = new EstadoTurno
            {
                IdEstadoTurno = idEstadoTurno,
                Nombre        = nombreEstado
            };

            try
            {
                _context.EstadoTurnos.Add(nuevoEstado);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Si falló el insert con ID forzado (ej: columna auto-incremental), 
                // desvinculamos la entidad para no corromper el tracking de EF Core.
                _context.Entry(nuevoEstado).State = EntityState.Detached;

                // Como fallback, verificamos si existe por nombre y si no, lo creamos sin forzar el ID.
                var existePorNombre = await _context.EstadoTurnos.AnyAsync(e => e.Nombre == nombreEstado);
                if (!existePorNombre)
                {
                    _context.EstadoTurnos.Add(new EstadoTurno { Nombre = nombreEstado });
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
