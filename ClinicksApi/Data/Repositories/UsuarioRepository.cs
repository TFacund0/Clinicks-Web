using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicksApi.Data.Repositories
{
    /// <inheritdoc/>
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ClinicksDbContext _context;

        /// <summary>
        /// Constructor del repositorio.
        /// </summary>
        public UsuarioRepository(ClinicksDbContext context)
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
        public async Task ActualizarUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
