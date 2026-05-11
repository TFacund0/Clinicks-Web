using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicksApi.Data.Repositories
{
    /// <summary>
    /// Implementación concreta del repositorio de Autenticación utilizando Entity Framework Core.
    /// Es la única clase autorizada para ejecutar consultas SQL sobre Usuarios y Médicos para el Login.
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        private readonly ClinicksDbContext _context;

        /// <summary>
        /// Inyecta el contexto de base de datos que representa la sesión física con PostgreSQL.
        /// </summary>
        public AuthRepository(ClinicksDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<Usuario?> GetUsuarioByUsernameAsync(string username)
        {
            var cleanUsername = username.Trim().ToLower();
            
            // 1. Buscamos por username
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username.Trim().ToLower() == cleanUsername);

            // 2. Si no existe, buscamos por matrícula
            if (usuario == null)
            {
                var medico = await _context.Medicos
                    .FirstOrDefaultAsync(m => m.Matricula.Trim().ToLower() == cleanUsername);
                
                if (medico != null)
                {
                    usuario = await _context.Usuarios
                        .FirstOrDefaultAsync(u => u.IdUsuario == medico.IdUsuario);
                }
            }

            return usuario;
        }

        public async Task<Medico?> GetMedicoByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Medicos
                .FirstOrDefaultAsync(m => m.IdUsuario == usuarioId);
        }

        public async Task<Medico?> GetFirstMedicoAsync()
        {
            return await _context.Medicos.FirstOrDefaultAsync();
        }
    }
}