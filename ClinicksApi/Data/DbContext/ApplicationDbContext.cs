using Microsoft.EntityFrameworkCore;
// Asegúrate de usar el namespace de tu proyecto
namespace ClinicksApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Aquí es donde luego agregaremos los "DbSet" (las tablas como Pacientes, Médicos)
    }
}