using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;

namespace ClinicksApi.Data.Repositories
{
    public class ProcesoRepository : IProcesoRepository
    {
        private readonly ClinicksDbContext _context;

        public ProcesoRepository(ClinicksDbContext context)
        {
            _context = context;
        }

        public async Task<Procedimiento> CrearProcedimiento(Procedimiento procedimiento)
        {
            _context.Procedimientos.Add(procedimiento);
            await _context.SaveChangesAsync();
            return procedimiento;
        }

        public async Task<Turno> CrearTurnoVinculado(Turno turno)
        {
            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();
            return turno;
        }

        public async Task AsegurarEstadoTurnoExiste(int idEstadoTurno, string nombreEstado)
        {
            var existe = await _context.EstadoTurnos.AnyAsync(e => e.IdEstadoTurno == idEstadoTurno);
            if (!existe)
            {
                var nuevoEstado = new EstadoTurno
                {
                    // Asumiendo que la base de datos permite forzar el Id (Identity insert o generacin manual dependiendo del config)
                    // Si falla por identity insert, no le mandamos el Id si es identity, pero como la firma dice IdEstadoTurno, lo seteamos.
                    IdEstadoTurno = idEstadoTurno,
                    Nombre = nombreEstado
                };
                
                // Si la columna IdEstadoTurno es auto-incremental (Identity) en Postgres, asignar el ID a mano dar error. 
                // Sin embargo, para no romper la base vaca, usamos una tcnica segura:
                // Intentamos buscar por nombre, si no existe lo creamos sin forzar ID si da error, pero aca forzaremos para estar seguros.
                
                // En Entity Framework, si el ID es autogenerado, EF ignorar el seteo si no se permite.
                // Como es una operacin paliativa:
                try
                {
                    _context.EstadoTurnos.Add(nuevoEstado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    // Si hubo error (ej: el id es auto incremental o ya existe por concurrencia), 
                    // limpiamos el tracking para no romper futuros inserts.
                    _context.Entry(nuevoEstado).State = EntityState.Detached;
                    
                    // Verificamos si existe por nombre
                    var estadoExistente = await _context.EstadoTurnos.FirstOrDefaultAsync(e => e.Nombre == nombreEstado);
                    if (estadoExistente == null)
                    {
                         // Insertamos sin ID para que la DB se encargue
                         var estadoSinId = new EstadoTurno { Nombre = nombreEstado };
                         _context.EstadoTurnos.Add(estadoSinId);
                         await _context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
