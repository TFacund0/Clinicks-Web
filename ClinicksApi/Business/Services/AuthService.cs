using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.DTOs;
using Microsoft.Extensions.Configuration;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClinicksApi.Business.Services
{
    /// <summary>
    /// Servicio centralizado para manejar la seguridad, encriptación y emisión de tokens.
    /// Representa la "Recepción" del sistema, donde se validan los credenciales antes de permitir acceso.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;

        /// <summary>
        /// Constructor del servicio. Recibe las dependencias inyectadas por el contenedor de .NET.
        /// </summary>
        /// <param name="authRepository">Repositorio que ejecuta las consultas SQL de autenticación.</param>
        /// <param name="config">Configuración de la aplicación, usada para leer las claves JWT del appsettings.</param>
        public AuthService(IAuthRepository authRepository, IConfiguration config)
        {
            _authRepository = authRepository;
            _config = config;
        }

        /// <summary>
        /// Proceso principal de inicio de sesión.
        /// Valida las credenciales contra la BD y genera un Token JWT firmado si son correctas.
        /// </summary>
        /// <param name="username">Nombre de usuario o matrícula ingresada.</param>
        /// <param name="password">Contraseña en texto plano a verificar.</param>
        /// <returns>DTO con los datos del médico y el Token JWT, o null si falla.</returns>
        public async Task<LoginResponseDto?> AuthenticateAsync(string username, string password)
        {
            try {
                // 1. Buscar el usuario (por username o matrícula)
                var usuario = await _authRepository.GetUsuarioByUsernameAsync(username);
                if (usuario == null) return null;

                // 2. Validar contraseña con BCrypt estricto
                if (!BCrypt.Net.BCrypt.Verify(password, usuario.Password)) return null;

                // 3. Obtener el médico asociado
                var medico = await _authRepository.GetMedicoByUsuarioIdAsync(usuario.IdUsuario);
                
                // Fallback de seguridad: si no hay médico vinculado, no permitimos acceso a funciones médicas
                if (medico == null) return null;

                // 4. Generar Token JWT Real
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
            } catch (Exception) {
                return null;
            }
        }

    }
}