using ClinicksApi.Business.DTOs;
using ClinicksApi.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ClinicksApi.Extensions;
using ClinicksApi.Constants;

namespace ClinicksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    /// <summary>
    /// Controlador responsable de gestionar la agenda de turnos en la clínica.
    /// Proporciona endpoints para obtener la lista de turnos agendados, con detalles del paciente, motivo y estado del turno.
    /// Es el "Recepcionista" que muestra la agenda diaria al médico.
    /// </summary>
    [Authorize] // Requiere autenticación para acceder a los endpoints de este controlador
    public class AgendaController : ControllerBase
    {
        private readonly ITurnoService _turnoService;

        /// <summary>
        /// Constructor del controlador. Aquí se aplica la Inyección de Dependencias para obtener una instancia del servicio de turnos.
        /// </summary>
        /// <param name="turnoService">El servicio (Especialista) que contiene la lógica real para obtener los turnos agendados.</param>
        public AgendaController(ITurnoService turnoService)
        {
            _turnoService = turnoService;
        }

        /// <summary>
        /// Endpoint para obtener la lista de turnos agendados. Devuelve información detallada de cada turno, incluyendo el nombre completo del paciente, su DNI, el motivo del turno y su estado actual.
        /// </summary>
        /// <returns>
        /// <see cref="OkResult"/> (200) con una lista de objetos TurnoAgendaDto si la operación es exitosa.
        /// <see cref="UnauthorizedResult"/> (401) si el usuario no está autenticado.
        /// </returns>
        [HttpGet("turnos-agendados")]
        public async Task<IActionResult> ObtenerTurnosAgendadosAsync()
        {
            var miAgenda = await _turnoService.ObtenerTurnosAgendadosAsync();

            return Ok(miAgenda);
        }

        /// <summary>
        /// Endpoint para que un médico pueda obtener su propia agenda de turnos, opcionalmente filtrada por un rango de fechas.
        /// </summary>
        /// <param name="fechaInicio">Fecha inicial del período (opcional).</param>
        /// <param name="fechaFin">Fecha final del período (opcional).</param>
        /// <returns>
        /// <see cref="OkResult"/> (200) con la lista de turnos del médico.
        /// <see cref="UnauthorizedResult"/> (401) si no es un médico autenticado.
        /// </returns>
        [Authorize(Roles = ConstantesGenerales.Roles.Medico)]
        [HttpGet("turnos-medico")]
        public async Task<IActionResult> ObtenerTurnosMedicoAsync([FromQuery] DateTime? fechaInicio, [FromQuery] DateTime? fechaFin)
        {
            var idMedico = User.GetMedicoId();
            if (idMedico == null)
            {
                return Unauthorized("No se pudo identificar al médico autenticado.");
            }

            var misTurnos = await _turnoService.ObtenerTurnosMedicoAsync(idMedico.Value, fechaInicio, fechaFin);

            if (!misTurnos.Any())
            {
                return Ok(new
                {
                    mensaje = "No hay turnos registrados para el período seleccionado",
                    turnos = misTurnos
                });
            }

            return Ok(new
            {
                turnos = misTurnos
            });
        }

        /// <summary>
        /// Endpoint para obtener un turno por su ID.
        /// </summary>
        /// <param name="id">El ID del turno.</param>
        /// <returns>El turno si existe, de lo contrario NotFound.</returns>
        [Authorize(Roles = ConstantesGenerales.Roles.Medico)]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerTurnoPorIdAsync(int id)
        {
            var turno = await _turnoService.ObtenerTurnoPorIdAsync(id);
            if (turno == null)
            {
                return NotFound($"No se encontró el turno con ID {id}.");
            }

            return Ok(turno);
        }
    }
}
