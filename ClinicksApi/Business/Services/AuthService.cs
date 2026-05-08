using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.Dtos;
using Microsoft.Extensions.Configuration;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClinicksApi.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;

        public AuthService(IAuthRepository authRepository, IConfiguration config)
        {
            _authRepository = authRepository;
            _config = config;
        }

        public async Task<LoginResponseDto?> AuthenticateAsync(string username, string password)
        {
            // 1. Obtener el usuario por Username
            var usuario = await _authRepository.GetUsuarioByUsernameAsync(username);
            
            // Fallback por BD desconectada: Si el usuario ingresó una Matrícula (ej: MN-12345) 
            // en vez de un Username, validamos la contraseña usando la cuenta maestra "admin".
            if (usuario == null)
            {
                var checkMedico = await _authRepository.GetMedicoByMatriculaAsync(username);
                if (checkMedico != null)
                {
                    usuario = await _authRepository.GetUsuarioByUsernameAsync("admin");
                }
            }

            // 2. Si no existe o la contraseña no coincide con el Hash, denegar
            if (usuario == null) return null;
            
            bool isPasswordValid = false;
            try {
                isPasswordValid = BCrypt.Net.BCrypt.Verify(password, usuario.Password);
            } catch {
                // Fallback temporal si la DB sigue con texto plano (solo para que no crashee, pero obligará a migrar)
                isPasswordValid = usuario.Password == password;
            }

            if (!isPasswordValid) return null;

            // 3. Obtener los datos del médico
            var medico = await _authRepository.GetMedicoByMatriculaAsync(username);
            
            if (medico == null && username.ToLower() == "admin")
            {
                // Fallback (Regla de negocio para modo pruebas)
                medico = await _authRepository.GetFirstMedicoAsync();
            }

            if (medico == null) return null;

            // 4. Generar el Token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, medico.Matricula),
                    new Claim("idMedico", medico.IdMedico.ToString()),
                    new Claim(ClaimTypes.Role, "Medico")
                }),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponseDto
            {
                IdMedico = medico.IdMedico,
                Nombre = medico.Nombre,
                Apellido = medico.Apellido,
                Matricula = medico.Matricula,
                Token = tokenHandler.WriteToken(token)
            };
        }

        public async Task<int> HashExistingPasswordsAsync()
        {
            var usuarios = await _authRepository.GetAllUsuariosAsync();
            int count = 0;

            foreach (var usuario in usuarios)
            {
                // Si la contraseña no arranca con el patrón de BCrypt ($2a$, $2b$, $2y$), asumimos que es texto plano
                if (!usuario.Password.StartsWith("$2"))
                {
                    usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
                    await _authRepository.UpdateUsuarioAsync(usuario);
                    count++;
                }
            }

            return count;
        }
    }
}