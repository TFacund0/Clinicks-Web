using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicksApi.Data.repository
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
            // 1. Buscamos el usuario en la tabla de credenciales
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (usuario == null) return null;

            // 2. Buscamos el médico vinculado a ese usuario usando la FK física id_usuario
            return await _context.Medicos
                .FirstOrDefaultAsync(m => m.IdUsuario == usuario.IdUsuario);
        }
    }
}