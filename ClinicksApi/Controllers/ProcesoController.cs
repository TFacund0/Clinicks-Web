using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.DTOs;

namespace ClinicksApi.Controllers
{
    /// <summary>
    /// Controlador responsable de gestionar los procedimientos médicos del sistema.
    /// Un procedimiento es una intervención clínica (cirugía, estudio de imagen, etc.) 
    /// que se registra vinculada a un paciente y un médico mediante un Turno.
    /// Requiere autenticación con Token JWT válido para acceder a cualquier endpoint.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProcesosController : ControllerBase
    {
        private readonly IProcesoService _procesoService;

        /// <summary>
        /// Constructor del controlador. Recibe el servicio inyectado por el contenedor de dependencias de .NET.
        /// </summary>
        /// <param name="procesoService">Servicio con la lógica de negocio de procedimientos médicos.</param>
        public ProcesosController(IProcesoService procesoService)
        {
            _procesoService = procesoService;
        }

        /// <summary>
        /// Registra un nuevo procedimiento médico vinculado al paciente y al médico autenticado.
        /// Crea automáticamente un Turno en la base de datos para asociar el procedimiento con ambas partes.
        /// </summary>
        /// <param name="dto">DTO con los datos del procedimiento (tipo, descripción, DNI del paciente, fecha, resultado).</param>
        /// <returns>
        /// <see cref="OkResult"/> (200) con un mensaje de éxito si el procedimiento se registró correctamente.
        /// <see cref="BadRequestResult"/> (400) si los datos son inválidos o el paciente no existe.
        /// <see cref="UnauthorizedResult"/> (401) si el token JWT no es válido o no se proporcionó.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> RegistrarProceso([FromBody] ProcesoAltaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Lee el ID del médico directamente del Token JWT para evitar suplantaciones.
            var idMedicoStr = User.FindFirst("idMedico")?.Value;
            if (!int.TryParse(idMedicoStr, out int idMedico))
                return Unauthorized(new { message = "No se pudo identificar al médico autenticado." });

            var resultado = await _procesoService.RegistrarProceso(dto, idMedico);

            if (resultado.Success)
            {
                return Ok(new
                {
                    success = true,
                    mensaje = resultado.Message
                });
            }

            return BadRequest(new
            {
                success = false,
                mensaje = resultado.Message
            });
        }

        /// <summary>
        /// Devuelve el catálogo de tipos de procedimiento disponibles en el sistema.
        /// El catálogo es provisto por la capa de negocio centralizadamente.
        /// </summary>
        /// <returns><see cref="OkResult"/> (200) con la lista de tipos de procedimiento disponibles.</returns>
        [HttpGet("tipos")]
        public IActionResult GetTiposProceso()
        {
            var tipos = _procesoService.ObtenerTiposProceso();
            return Ok(tipos);
        }

        /// <summary>
        /// Obtiene el historial clínico completo de procedimientos de un paciente ordenado por fecha descendente.
        /// </summary>
        /// <param name="pacienteId">El ID numérico del paciente.</param>
        /// <returns>
        /// <see cref="OkResult"/> (200) con la lista de procedimientos del paciente.
        /// <see cref="BadRequestResult"/> (400) si el ID es inválido.
        /// </returns>
        [HttpGet("historial/{pacienteId}")]
        public async Task<IActionResult> GetHistorial(int pacienteId)
        {
            if (pacienteId <= 0)
            {
                return BadRequest(new
                {
                    success = false,
                    mensaje = "El Id del paciente debe ser mayor a cero."
                });
            }

            var historial = await _procesoService.ObtenerHistorialPaciente(pacienteId);
            return Ok(historial);
        }
    }
}
