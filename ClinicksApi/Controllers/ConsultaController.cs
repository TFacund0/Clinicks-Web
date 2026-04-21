using Microsoft.AspNetCore.Mvc;
using ClinicksApi.Business.Interfaces; // CORREGIDO: Apunta a la capa correcta
using ClinicksApi.Business.DTOs;

namespace ClinicksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultasController : ControllerBase
    {
        private readonly IConsultaService _consultaService;

        public ConsultasController(IConsultaService consultaService)
        {
            _consultaService = consultaService;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarConsulta([FromBody] ConsultaAltaDto dto)
        {
            // Mantenemos el ID hardcodeado temporalmente porque no hay login
            // (Asegurate de que el ID 2 o 1 exista en tu tabla "medico")
            int idMedicoPrueba = 1;

            var resultado = await _consultaService.RegistrarConsulta(dto, idMedicoPrueba);

            if (resultado.Success)
            {
                return Ok(new
                {
                    success = true,
                    mensaje = "Consulta guardada correctamente"
                });
            }

            return BadRequest(new
            {
                success = false,
                mensaje = resultado.Message
            });
        }

        [HttpGet("historial/{pacienteId}")]
        public async Task<IActionResult> GetHistorial(int pacienteId)
        {
            var historial = await _consultaService.ObtenerHistorialPaciente(pacienteId);
            return Ok(historial);
        }
    }
}