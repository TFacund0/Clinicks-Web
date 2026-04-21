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
                Console.WriteLine("DEBUG: El servicio no encontró al médico en la DB.");
                return Unauthorized(new { message = "Usuario o contraseña incorrectos" });
            }

            return Ok(new { /* info del medico */ });
        }
    }
}