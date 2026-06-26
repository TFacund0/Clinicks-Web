using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ClinicksApi.Controllers
{
    /// <summary>
    /// Controlador responsable de gestionar la autenticación y seguridad de los Usuarios de la API.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        /// <summary>
        /// Constructor del controlador de usuarios.
        /// </summary>
        /// <param name="usuarioService">El servicio de usuarios/autenticación.</param>
        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
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
        public async Task<IActionResult> IniciarSesion([FromBody] LoginRequest request)
        {
            var medico = await _usuarioService.AutenticarAsync(request.Username, request.Password);

            if (medico == null)
                return Unauthorized(new { message = "Usuario o contraseña incorrectos." });

            return Ok(medico);
        }
    }
}
