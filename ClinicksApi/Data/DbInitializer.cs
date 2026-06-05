using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClinicksApi.Data.Entities;
using ClinicksApi.Constants;

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
                
                // Limpiar la tabla si no es muy costoso, pero mejor simplemente asegurarnos que los 5 existan.
                // Como los IDs podrían estar tomados, usamos Upsert o verificamos uno a uno.

                var estados = new[]
                {
                    new EstadoTurno { IdEstadoTurno = ConstantesGenerales.EstadosTurno.PendienteId, Nombre = "Pendiente" },
                    new EstadoTurno { IdEstadoTurno = ConstantesGenerales.EstadosTurno.ConfirmadoId, Nombre = "Confirmado" },
                    new EstadoTurno { IdEstadoTurno = ConstantesGenerales.EstadosTurno.EnCursoId, Nombre = "En curso" },
                    new EstadoTurno { IdEstadoTurno = ConstantesGenerales.EstadosTurno.AtendidoId, Nombre = "Atendido" },
                    new EstadoTurno { IdEstadoTurno = ConstantesGenerales.EstadosTurno.CanceladoId, Nombre = "Cancelado" }
                };

                foreach (var estado in estados)
                {
                    var existe = await context.EstadoTurnos.AnyAsync(e => e.IdEstadoTurno == estado.IdEstadoTurno);
                    if (!existe)
                    {
                        context.EstadoTurnos.Add(estado);
                        Console.WriteLine($"[DB INITIALIZER] Seed de Estado '{estado.Nombre}' (ID={estado.IdEstadoTurno}) agregado.");
                    }
                    else
                    {
                        var eBD = await context.EstadoTurnos.FindAsync(estado.IdEstadoTurno);
                        if (eBD != null && eBD.Nombre != estado.Nombre)
                        {
                            eBD.Nombre = estado.Nombre;
                            Console.WriteLine($"[DB INITIALIZER] Seed de Estado (ID={estado.IdEstadoTurno}) actualizado a '{estado.Nombre}'.");
                        }
                    }
                }

                await context.SaveChangesAsync();

                var todosLosEstados = await context.EstadoTurnos.ToListAsync();
                Console.WriteLine("\n[DEBUG] --- ESTADOS DE TURNO EN LA BD ---");
                foreach (var est in todosLosEstados)
                {
                    Console.WriteLine($"ID: {est.IdEstadoTurno} | Nombre: {est.Nombre}");
                }
                Console.WriteLine("[DEBUG] ---------------------------------\n");
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
