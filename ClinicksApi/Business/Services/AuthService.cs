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
        /// Contiene lógicas de fallback por si la base de datos está en proceso de migración de seguridad.
        /// </summary>
        /// <param name="username">Nombre de usuario o matrícula ingresada.</param>
        /// <param name="password">Contraseña en texto plano a verificar.</param>
        /// <returns>DTO con los datos del médico y el Token JWT, o null si falla.</returns>
        public async Task<LoginResponseDto?> AuthenticateAsync(string username, string password)
        {
            // 1. Buscar el usuario en la tabla 'usuario' por su nombre de usuario exacto.
            var usuario = await _authRepository.GetUsuarioByUsernameAsync(username);
            if (usuario == null) return null;

            bool isPasswordValid = false;
            try {
                isPasswordValid = BCrypt.Net.BCrypt.Verify(password, usuario.Password);
            } catch {
                // Si el hash en BD no tiene formato BCrypt válido, denegamos el acceso.
                // Ejecutar GET /api/Auth/hash-passwords para migrar las contraseñas.
                isPasswordValid = false;
            }

            if (!isPasswordValid) return null;

            // 3. Obtener los datos del médico asociado al usuario por su matrícula.
            var medico = await _authRepository.GetMedicoByMatriculaAsync(username);

            // Si el usuario no tiene un médico asociado (ej: usuario administrativo sin matrícula),
            // obtenemos el primer médico disponible como representación de la sesión.
            if (medico == null)
                medico = await _authRepository.GetFirstMedicoAsync();

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

        /// <summary>
        /// Tarea de mantenimiento que escanea todos los usuarios y encripta con BCrypt
        /// las contraseñas que detecta como texto plano.
        /// </summary>
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