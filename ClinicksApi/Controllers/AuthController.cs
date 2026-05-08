using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ClinicksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // ESTO TE VA A DECIR TODO:
            Console.WriteLine($"DEBUG: Recibido User='{request.Username}' Pass='{request.Password}'");

            if (string.IsNullOrEmpty(request.Username))
            {
                return BadRequest(new { message = "C# recibió el nombre de usuario VACÍO. Revisá el JsonPropertyName." });
            }

            var medico = await _authService.AuthenticateAsync(request.Username, request.Password);

            if (medico == null)
            {
                Console.WriteLine("DEBUG: El servicio no encontró al médico en la DB o las credenciales son inválidas.");
                return Unauthorized(new { message = "Usuario o contraseña incorrectos" });
            }

            // Devolvemos el DTO proporcionado por la capa de negocio
            return Ok(medico);
        }

        [HttpGet("hash-passwords")]
        public async Task<IActionResult> HashPasswords()
        {
            try
            {
                int count = await _authService.HashExistingPasswordsAsync();
                return Ok(new { message = $"Se han encriptado {count} contraseñas exitosamente. Ya puedes iniciar sesión con normalidad." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al migrar contraseñas", detail = ex.Message });
            }
        }

    }
}