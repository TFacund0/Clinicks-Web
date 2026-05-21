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

        public async Task<Usuario?> GetUsuarioByUsernameAsync(string username)
        {
            var cleanUsername = username.Trim().ToLower();
            var usuario = await _context.Usuarios
                .Include(u => u.IdEstadoUsuarioNavigation)
                .FirstOrDefaultAsync(u => u.Username.ToLower() == cleanUsername);

            // Intento 2 (fallback)
            if (usuario == null)
            {
                var medico = await _context.Medicos
                    .FirstOrDefaultAsync(m => m.Matricula.ToLower() == cleanUsername);
                
                if (medico != null && medico.IdUsuario.HasValue)
                {
                    usuario = await _context.Usuarios
                        .Include(u => u.IdEstadoUsuarioNavigation)
                        .FirstOrDefaultAsync(u => u.IdUsuario == medico.IdUsuario.Value);
                }
            }

            return usuario;
        }

        public async Task<Medico?> GetMedicoByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Medicos
                .FirstOrDefaultAsync(m => m.IdUsuario == usuarioId);
        }

        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task UpdateUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }
    }
}