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
            // Buscar el usuario por username o matrícula (fallback coordinado en la capa de servicios)
            var usuario = await _authRepository.ObtenerUsuarioPorUsernameAsync(username);

            if (usuario == null)
            {
                usuario = await _authRepository.ObtenerUsuarioPorMatriculaAsync(username);
            }

            if (usuario == null)
            {
                return null;
            }

            // Verificar contraseña
            bool isPasswordValid = false;

            string dbPassword = usuario.Password?.Trim() ?? "";
            string inputPassword = password?.Trim() ?? "";

            try
            {
                isPasswordValid = BCrypt.Net.BCrypt.Verify(inputPassword, dbPassword);
            }
            catch (Exception)
            {
                isPasswordValid = false;
            }

            if (!isPasswordValid)
            {
                return null;
            }

            // Buscar al médico vinculado
            var medico = await _authRepository.ObtenerMedicoPorUsuarioIdAsync(usuario.IdUsuario);

            // Si el usuario no tiene médico vinculado, rechazamos el login de inmediato en lugar de usar un fallback
            // que suplantaría la identidad de otro médico.
            if (medico == null)
            {
                return null;
            }

            // Generar el Token JWT usando el servicio abstraído
            var tokenString = _tokenService.GenerateToken(medico);

            return new LoginResponseDto
            {
                IdMedico = medico.IdMedico,
                Nombre = medico.Nombre,
                Apellido = medico.Apellido,
                Matricula = medico.Matricula,
                Token = tokenString
            };
        }

        /// <inheritdoc/>
        public async Task<int> HashExistingPasswordsAsync()
        {
            var usuarios = await _authRepository.ObtenerTodosLosUsuariosAsync();
            int count = 0;

            foreach (var usuario in usuarios)
            {
                if (!usuario.Password.StartsWith("$2"))
                {
                    usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password.Trim());
                    await _authRepository.ActualizarUsuarioAsync(usuario);
                    count++;
                }
            }
            return count;
        }
    }
}