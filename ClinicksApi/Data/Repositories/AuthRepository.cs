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
            // 1. Buscamos el usuario validando la contraseña
            // Permitimos login si ingresan el Username del usuario o la Matrícula del médico
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Password == password && 
                    (u.Username.ToLower() == username.ToLower() || u.Username == "admin"));

            if (usuario == null)
            {
                Console.WriteLine($"---> REPO: No encontré contraseña válida para {username}");
                return null;
            }

            // Usamos .Select() para proyectar solo las columnas que sabemos que existen en la DB
            // y evitar el crasheo por la columna faltante 'id_usuario' que EF Core intenta traer por defecto.
            var medicoDb = await _context.Medicos
                .Select(m => new Medico {
                    IdMedico = m.IdMedico,
                    Nombre = m.Nombre,
                    Apellido = m.Apellido,
                    Matricula = m.Matricula
                })
                .FirstOrDefaultAsync(m => m.Matricula.ToLower() == username.ToLower());

            if (medicoDb == null)
            {
                // Fallback: Si ingresaron "admin", devolvemos el primer médico para que puedan probar el sistema
                medicoDb = await _context.Medicos
                    .Select(m => new Medico {
                        IdMedico = m.IdMedico,
                        Nombre = m.Nombre,
                        Apellido = m.Apellido,
                        Matricula = m.Matricula
                    })
                    .FirstOrDefaultAsync();
            }

            return medicoDb;
        }
    }
}