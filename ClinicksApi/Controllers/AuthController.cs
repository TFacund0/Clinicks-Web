using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ClinicksApi.Controllers
{
    /// <summary>
    /// Controlador responsable de gestionar la autenticación y seguridad de la API.
    /// Es el "Recepcionista" que verifica credenciales y entrega los Tokens JWT (Gafetes).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Constructor del controlador. Aquí se aplica la Inyección de Dependencias.
        /// </summary>
        /// <param name="authService">El servicio (Especialista) que contiene la lógica real de autenticación.</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Endpoint para iniciar sesión en el sistema.
        /// Recibe las credenciales y, si son válidas, devuelve los datos del médico junto con su Token JWT.
        /// </summary>
        /// <param name="request">DTO que contiene el Username y Password enviados por el cliente (ej. React).</param>
        /// <returns>
        /// <see cref="OkResult"/> (200) con los datos del médico si el login es exitoso.
        /// <see cref="BadRequestResult"/> (400) si el formato de la petición es incorrecto.
        /// <see cref="UnauthorizedResult"/> (401) si las credenciales son inválidas.
        /// </returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username))
                return BadRequest(new { message = "El nombre de usuario es obligatorio." });

            var medico = await _authService.AuthenticateAsync(request.Username, request.Password);

            if (medico == null)
                return Unauthorized(new { message = "Usuario o contraseña incorrectos." });

            return Ok(medico);
        }

        /// <summary>
        /// Endpoint temporal de mantenimiento para encriptar claves en texto plano.
        /// </summary>
        [HttpPost("encriptar-claves")]
        public async Task<IActionResult> EncriptarClaves()
        {
            var cantidad = await _authService.HashExistingPasswordsAsync();
            return Ok(new { message = $"Se han encriptado {cantidad} contraseñas exitosamente." });
        }
    }
}