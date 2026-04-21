using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicksApi.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ClinicksDbContext _context;

        public AuthRepository(ClinicksDbContext context)
        {
            _context = context;
        }

        public async Task<Medico?> LoginAsync(string username, string password)
        {
            // 1. Buscamos el usuario
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower() && u.Password == password);

            if (usuario == null)
            {
                Console.WriteLine($"---> REPO: No encontré al usuario '{username}' con esa contraseña.");
                return null;
            }

            Console.WriteLine($"---> REPO: Usuario encontrado! ID: {usuario.IdUsuario}. Buscando médico vinculado...");

            // 2. Buscamos el médico
            // NOTA: Asegurate que la propiedad en C# se llame IdUsuario (como en la DB)
            var medicoDb = await _context.Medicos
                .FirstOrDefaultAsync(m => m.IdUsuario == usuario.IdUsuario);

            if (medicoDb == null)
            {
                Console.WriteLine($"---> REPO: El usuario {usuario.IdUsuario} NO tiene un médico asociado en la columna 'id_usuario'.");

                // PRUEBA DE EMERGENCIA: ¿Hay algún médico con ese nombre?
                var algunMedico = await _context.Medicos.FirstOrDefaultAsync();
                Console.WriteLine($"---> REPO: Encontré un médico cualquiera en la DB? {(algunMedico != null ? "SÍ" : "NO, LA TABLA ESTÁ VACÍA PARA C#")}");

                return null;
            }

            return medicoDb;
        }
    }
}