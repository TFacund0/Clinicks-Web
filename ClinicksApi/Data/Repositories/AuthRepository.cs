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
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<Medico?> GetMedicoByMatriculaAsync(string matricula)
        {
            return await _context.Medicos
                .Select(m => new Medico {
                    IdMedico = m.IdMedico,
                    Nombre = m.Nombre,
                    Apellido = m.Apellido,
                    Matricula = m.Matricula
                })
                .FirstOrDefaultAsync(m => m.Matricula.ToLower() == matricula.ToLower());
        }

        public async Task<Medico?> GetFirstMedicoAsync()
        {
            return await _context.Medicos
                .Select(m => new Medico {
                    IdMedico = m.IdMedico,
                    Nombre = m.Nombre,
                    Apellido = m.Apellido,
                    Matricula = m.Matricula
                })
                .FirstOrDefaultAsync();
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