using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.DTOs;
using ClinicksApi.Extensions;

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

        /// <summary>
        /// Constructor de ConsultasController.
        /// </summary>
        /// <param name="consultaService">Servicio con la lógica de negocio de consultas médicas.</param>
        public ConsultasController(IConsultaService consultaService)
        {
            _consultaService = consultaService;
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
        public async Task<IActionResult> RegistrarConsulta([FromBody] ConsultaAltaDto consulta)
        {
            var idMedico = User.GetMedicoId();
            if (idMedico == null)
                return Unauthorized(new { message = "No se pudo identificar al médico autenticado." });

            var resultado = await _consultaService.RegistrarConsulta(consulta, idMedico.Value);

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

    }
}