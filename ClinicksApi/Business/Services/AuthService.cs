using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.DTOs;
using Microsoft.Extensions.Configuration;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;

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
            Console.WriteLine($"\n[LOGIN DEBUG] 1. Intentando buscar usuario: '{username}'");
            
            // PASO 1: Buscar el usuario
            var usuario = await _authRepository.GetUsuarioByUsernameAsync(username);

            if (usuario == null) 
            {
                Console.WriteLine("[LOGIN DEBUG] ERROR: El usuario retornó NULL. No existe en DB o falla EF Core.");
                return null;
            }

            Console.WriteLine($"[LOGIN DEBUG] 2. Usuario encontrado: ID {usuario.IdUsuario}. Verificando clave...");

            // PASO 2: Verificar contraseña (AQUÍ ESTABA EL MAYOR PROBLEMA)
            bool isPasswordValid = false;
            
            // Limpiamos los espacios en blanco que PostgreSQL pudo haber guardado por error
            string dbPassword = usuario.Password?.Trim() ?? "";
            string inputPassword = password?.Trim() ?? "";

            try
            {
                isPasswordValid = BCrypt.Net.BCrypt.Verify(inputPassword, dbPassword);
                Console.WriteLine($"[LOGIN DEBUG] 3. Verificación BCrypt exitosa: {isPasswordValid}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[LOGIN DEBUG] ADVERTENCIA: La clave no es BCrypt válido ({ex.Message}). Usando fallback a texto plano.");
                isPasswordValid = dbPassword == inputPassword;
            }

            if (!isPasswordValid) 
            {
                Console.WriteLine("[LOGIN DEBUG] ERROR: La contraseña es incorrecta.");
                return null;
            }

            // PASO 3: Buscar al médico vinculado
            Console.WriteLine($"[LOGIN DEBUG] 4. Clave correcta. Buscando Médico para id_usuario: {usuario.IdUsuario}");
            var medico = await _authRepository.GetMedicoByUsuarioIdAsync(usuario.IdUsuario);

            if (medico == null)
            {
                Console.WriteLine("[LOGIN DEBUG] ADVERTENCIA: El usuario no tiene médico vinculado (id_usuario nulo en tabla medico). Usando el primero...");
                medico = await _authRepository.GetFirstMedicoAsync();
            }

            if (medico == null) 
            {
                Console.WriteLine("[LOGIN DEBUG] ERROR: La tabla de Médicos está completamente vacía.");
                return null;
            }

            Console.WriteLine($"[LOGIN DEBUG] 5. ¡ÉXITO! Login autorizado para Dr/Dra. {medico.Apellido}");

            // PASO 4: Generar el Token JWT
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
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoginResponseDto
            {
                IdMedico  = medico.IdMedico,
                Nombre    = medico.Nombre,
                Apellido  = medico.Apellido,
                Matricula = medico.Matricula,
                Token     = tokenHandler.WriteToken(token)
            };
        }

        public async Task<int> HashExistingPasswordsAsync()
        {
            var usuarios = await _authRepository.GetAllUsuariosAsync();
            int count = 0;

            foreach (var usuario in usuarios)
            {
                if (!usuario.Password.StartsWith("$2"))
                {
                    usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password.Trim());
                    await _authRepository.UpdateUsuarioAsync(usuario);
                    count++;
                }
            }
            return count;
        }
    }
}