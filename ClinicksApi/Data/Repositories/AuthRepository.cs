using ClinicksApi.Data.Entities; 
using ClinicksApi.Data.Interfaces;
using ClinicksApi.Models;   
using Microsoft.EntityFrameworkCore;

namespace ClinicksApi.Data.Respositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ClinicksDbContext _context;

        public AuthRepository(ClinicksDbContext context)
        {
            _context = context;
        }

        public async Task<Entities.Medico?> LoginAsync(string username, string password)
        {
            // 1. Buscamos el usuario en la tabla de credenciales (Model)
            var usuarioModel = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

            if (usuarioModel == null) return null;

            // 2. Buscamos el médico vinculado a ese usuario (Model)
            var medicoModel = await _context.Medicos
                .FirstOrDefaultAsync(m => m.IdUsuario == usuarioModel.IdUsuario);

            if (medicoModel == null) return null;

            // 3. MAPEAMOS de Model (EF) a Entity (Negocio)
            // Esto lo podrías hacer con AutoMapper, pero acá lo hacemos a mano para probar
            return new Entities.Medico
            {
                IdMedico = medicoModel.IdMedico,
                Nombre = medicoModel.Nombre,
                Apellido = medicoModel.Apellido,
                Matricula = medicoModel.Matricula,
                IdUsuario = medicoModel.IdUsuario
            };
        }
    }
}