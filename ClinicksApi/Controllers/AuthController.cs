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
            var medico = await _authService.AuthenticateAsync(request.Username, request.Password);

            if (medico == null)
            {
                return Unauthorized(new { message = "Usuario o contraseña incorrectos" });
            }

            return Ok(new
            {
                idMedico = medico.IdMedico,
                nombre = medico.Nombre,
                apellido = medico.Apellido,
                matricula = medico.Matricula
            });
        }
    }
}