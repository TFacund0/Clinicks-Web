using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.DTOs;

namespace ClinicksApi.Controllers
{
    /// <summary>
    /// Controlador responsable de gestionar las consultas médicas del sistema.
    /// Permite registrar nuevas atenciones y consultar el historial clínico de un paciente.
    /// Requiere autenticación con Token JWT válido para acceder a cualquier endpoint.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultasController : ControllerBase
    {
        private readonly IConsultaService _consultaService;
        private readonly IPacienteService _pacienteService;

        /// <summary>
        /// Constructor del controlador. Recibe los servicios inyectados por el contenedor de dependencias de .NET.
        /// </summary>
        /// <param name="consultaService">Servicio con la lógica de negocio de consultas médicas.</param>
        /// <param name="pacienteService">Servicio para validar la existencia del paciente antes de registrar una consulta.</param>
        public ConsultasController(IConsultaService consultaService, IPacienteService pacienteService)
        {
            _consultaService = consultaService;
            _pacienteService = pacienteService;
        }

        /// <summary>
        /// Registra una nueva consulta médica en el sistema.
        /// Valida que el paciente exista por DNI antes de persistir la información.
        /// </summary>
        /// <param name="dto">DTO con los datos de la consulta enviados por el frontend (motivo, diagnóstico, DNI del paciente, etc.).</param>
        /// <returns>
        /// <see cref="OkResult"/> (200) con un mensaje de éxito si la consulta se registró correctamente.
        /// <see cref="BadRequestResult"/> (400) si el paciente no existe o faltan campos obligatorios.
        /// <see cref="UnauthorizedResult"/> (401) si el token JWT no es válido o no se proporcionó.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> RegistrarConsulta([FromBody] ConsultaAltaDto dto)
        {
            // Lee el ID del médico directamente del Token JWT (claim "idMedico") para evitar suplantaciones.
            var idMedicoStr = User.FindFirst("idMedico")?.Value;
            if (!int.TryParse(idMedicoStr, out int idMedico))
                return Unauthorized(new { message = "No se pudo identificar al médico autenticado." });

            // Delega la validación completa (incluyendo verificar si el paciente existe) y el guardado en base de datos al servicio de negocio.
            var resultado = await _consultaService.RegistrarConsulta(dto, idMedico);

            if (resultado.Success)
            {
                return Ok(new
                {
                    success = true,
                    mensaje = "Consulta guardada correctamente."
                });
            }

            return BadRequest(new
            {
                success = false,
                mensaje = resultado.Message
            });
        }

        /// <summary>
        /// Obtiene el historial clínico completo de un paciente ordenado por fecha descendente.
        /// </summary>
        /// <param name="pacienteId">El ID numérico del paciente (extraído de la URL, ej: /api/consultas/historial/5).</param>
        /// <returns>
        /// <see cref="OkResult"/> (200) con la lista de consultas del paciente (puede ser vacía si no tiene).
        /// <see cref="BadRequestResult"/> (400) si el ID proporcionado es inválido (menor o igual a cero).
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

            var historial = await _consultaService.ObtenerHistorialPaciente(pacienteId);
            return Ok(historial);
        }
    }
}