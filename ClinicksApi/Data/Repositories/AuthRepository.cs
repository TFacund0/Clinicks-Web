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
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }
    }
}