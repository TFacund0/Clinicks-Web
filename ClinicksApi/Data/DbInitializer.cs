using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data
{
    /// <summary>
    /// Clase helper responsable de la inicialización y el seeding de datos maestros de la base de datos.
    /// Se ejecuta durante el arranque de la aplicación (bootstrap) en Program.cs.
    /// </summary>
    public static class DbInitializer
    {
        public static async Task SeedAsync(ClinicksDbContext context)
        {
            try
            {
                // Asegurar que la base de datos esté creada (y aplicar migraciones si corresponde)
                // context.Database.Migrate(); // Opcional, pero útil si se ejecuta en entornos nuevos.
                
                // 1. Garantizar que el estado de turno 'Realizado' (ID = 1) exista.
                var existeRealizado = await context.EstadoTurnos.AnyAsync(e => e.IdEstadoTurno == 1);
                if (!existeRealizado)
                {
                    var realizado = new EstadoTurno { IdEstadoTurno = 1, Nombre = "Realizado" };
                    context.EstadoTurnos.Add(realizado);
                    Console.WriteLine("[DB INITIALIZER] Seed de Estado de Turno 'Realizado' (ID = 1) agregado.");
                }

                // 2. Garantizar que el estado de turno 'Atendido' (buscado por PacienteRepository) exista.
                var existeAtendido = await context.EstadoTurnos.AnyAsync(e => e.Nombre.ToLower() == "atendido");
                if (!existeAtendido)
                {
                    var atendido = new EstadoTurno { Nombre = "Atendido" };
                    context.EstadoTurnos.Add(atendido);
                    Console.WriteLine("[DB INITIALIZER] Seed de Estado de Turno 'Atendido' agregado.");
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB INITIALIZER] ERROR crítico al sembrar datos maestros: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"[DB INITIALIZER] Detalle del error interno: {ex.InnerException.Message}");
                }
            }
        }
    }
}
