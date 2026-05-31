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
            // --- BYPASS TEMPORAL DE DESARROLLO ---
            if (username == "admin" && password == "admin")
            {
                Console.WriteLine("[LOGIN DEBUG] Usando bypass de desarrollo.");
                // Intentamos buscar cualquier médico existente para que la BD no falle por claves foráneas
                var medicoBypass = await _authRepository.GetFirstMedicoAsync();
                
                // Si la tabla de médicos también está vacía, usamos datos falsos en memoria
                if (medicoBypass == null)
                {
                    medicoBypass = new Medico { IdMedico = 1, Nombre = "Doctor", Apellido = "Prueba", Matricula = "MN-12345" };
                }

                var tokenStringBypass = _tokenService.GenerateToken(medicoBypass);
                return new LoginResponseDto
                {
                    IdMedico  = medicoBypass.IdMedico,
                    Nombre    = medicoBypass.Nombre ?? "Doctor",
                    Apellido  = medicoBypass.Apellido ?? "Prueba",
                    Matricula = medicoBypass.Matricula ?? "MN-12345",
                    Token     = tokenStringBypass
                };
            }
            // ------------------------------------

            // PASO 1: Buscar el usuario por username o matrícula (fallback coordinado en la capa de servicios)
            var usuario = await _authRepository.GetUsuarioByUsernameAsync(username);

            if (usuario == null)
            {
                usuario = await _authRepository.GetUsuarioByMedicoMatriculaAsync(username);
            }

            if (usuario == null) 
            {
                return null;
            }

            // PASO 2: Verificar contraseña
            bool isPasswordValid = false;
            
            string dbPassword = usuario.Password?.Trim() ?? "";
            string inputPassword = password?.Trim() ?? "";

            try
            {
                isPasswordValid = BCrypt.Net.BCrypt.Verify(inputPassword, dbPassword);
            }
            catch (Exception)
            {
                // VULNERABILIDAD CRÍTICA PARCHEADA:
                // Jamás hacer fallback a texto plano si falla la verificación del hash.
                isPasswordValid = false;
            }

            if (!isPasswordValid) 
            {
                return null;
            }

            // PASO 3: Buscar al médico vinculado
            var medico = await _authRepository.GetMedicoByUsuarioIdAsync(usuario.IdUsuario);

            // CORRECCIÓN DE SEGURIDAD CRÍTICA:
            // Si el usuario no tiene médico vinculado, rechazamos el login de inmediato en lugar de usar un fallback
            // que suplantaría la identidad de otro médico.
            if (medico == null)
            {
                return null;
            }

            // PASO 4: Generar el Token JWT usando el servicio abstraído
            var tokenString = _tokenService.GenerateToken(medico);

            return new LoginResponseDto
            {
                IdMedico  = medico.IdMedico,
                Nombre    = medico.Nombre ?? "Nombre",
                Apellido  = medico.Apellido ?? "Apellido",
                Matricula = medico.Matricula ?? "MN-000",
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