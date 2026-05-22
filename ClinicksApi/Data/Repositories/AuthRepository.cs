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
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username.ToLower() == cleanUsername);
        }

        public async Task<Usuario?> GetUsuarioByMedicoMatriculaAsync(string matricula)
        {
            var cleanMatricula = matricula.Trim().ToLower();
            var medico = await _context.Medicos
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Matricula.ToLower() == cleanMatricula);

            if (medico != null && medico.IdUsuario.HasValue)
            {
                return await _context.Usuarios
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.IdUsuario == medico.IdUsuario.Value);
            }

            return null;
        }

        public async Task<Medico?> GetMedicoByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Medicos
                .AsNoTracking()
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