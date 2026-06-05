using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicksApi.Data.Repositories
{
    /// <summary>
    /// Implementación concreta del repositorio de autenticación, responsable de acceder a la tabla Usuarios y su relación con Medicos en PostgreSQL.
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
        private readonly ClinicksDbContext _context;

        /// <summary>
        /// Constructor del repositorio de autenticación. Recibe el contexto de base de datos inyectado por .NET, que representa la sesión activa con PostgreSQL.
        /// </summary>
        /// <param name="context"></param>
        public AuthRepository(ClinicksDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<Usuario?> ObtenerUsuarioPorUsernameAsync(string username)
        {
            var cleanUsername = username.Trim().ToLower();
            return await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username.ToLower() == cleanUsername);
        }

        /// <inheritdoc/>
        public async Task<Usuario?> ObtenerUsuarioPorMatriculaAsync(string matricula)
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

        /// <inheritdoc/>
        public async Task<Medico?> ObtenerMedicoPorUsuarioIdAsync(int usuarioId)
        {
            return await _context.Medicos
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdUsuario == usuarioId);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Usuario>> ObtenerTodosLosUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task ActualizarUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }


    }
}