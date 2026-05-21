using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.DTOs;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClinicksApi.Business.Services
{
    /// <summary>
    /// Servicio de autenticación.
    /// Contiene las reglas de negocio para validar credenciales de acceso, verificar roles y generar
    /// las sesiones seguras del personal médico a través de tokens JWT mediante ITokenService.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IAuthRepository authRepository, ITokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
        }

        /// <inheritdoc/>
        public async Task<LoginResponseDto?> AuthenticateAsync(string username, string password)
        {
            Console.WriteLine($"\n[LOGIN DEBUG] 1. Intentando buscar usuario: '{username}'");
            
            // PASO 1: Buscar el usuario por username o matrícula (fallback coordinado en la capa de servicios)
            var usuario = await _authRepository.GetUsuarioByUsernameAsync(username);

            if (usuario == null)
            {
                Console.WriteLine("[LOGIN DEBUG] Fallback: Intentando buscar por matrícula del médico");
                usuario = await _authRepository.GetUsuarioByMedicoMatriculaAsync(username);
            }

            if (usuario == null) 
            {
                Console.WriteLine("[LOGIN DEBUG] ERROR: El usuario no existe en DB por username ni por matrícula.");
                return null;
            }

            Console.WriteLine($"[LOGIN DEBUG] 2. Usuario encontrado: ID {usuario.IdUsuario}. Verificando clave...");

            // PASO 2: Verificar contraseña
            bool isPasswordValid = false;
            
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

            // CORRECCIÓN DE SEGURIDAD CRÍTICA:
            // Si el usuario no tiene médico vinculado, rechazamos el login de inmediato en lugar de usar un fallback
            // que suplantaría la identidad de otro médico.
            if (medico == null)
            {
                Console.WriteLine("[LOGIN DEBUG] ERROR CRÍTICO DE SEGURIDAD: El usuario no tiene médico vinculado en la base de datos.");
                return null;
            }

            Console.WriteLine($"[LOGIN DEBUG] 5. ¡ÉXITO! Login autorizado para Dr/Dra. {medico.Apellido}");

            // PASO 4: Generar el Token JWT usando el servicio abstraído
            var tokenString = _tokenService.GenerateToken(medico);

            return new LoginResponseDto
            {
                IdMedico  = medico.IdMedico,
                Nombre    = medico.Nombre,
                Apellido  = medico.Apellido,
                Matricula = medico.Matricula,
                Token     = tokenString
            };
        }

        /// <inheritdoc/>
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