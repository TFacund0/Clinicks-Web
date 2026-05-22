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
        public async Task<Procedimiento> CrearProcedimientoYTurnoVinculado(Procedimiento procedimiento, Turno turno)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Guardar el procedimiento
                _context.Procedimientos.Add(procedimiento);
                await _context.SaveChangesAsync(); // Guarda y obtiene el ID autoincremental de procedimiento

                // 2. Vincular el ID del procedimiento generado al Turno
                turno.IdProcedimiento = procedimiento.IdProcedimiento;

                // 3. Guardar el turno
                _context.Turnos.Add(turno);
                await _context.SaveChangesAsync();

                // 4. Confirmar la transacción
                await transaction.CommitAsync();

                return procedimiento;
            }
            catch (Exception)
            {
                // Revertir cambios en caso de error
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
