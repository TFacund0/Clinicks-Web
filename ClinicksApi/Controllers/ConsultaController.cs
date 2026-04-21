using Microsoft.AspNetCore.Mvc;
using ClinicksApi.Services.Interfaces;
using ClinicksApi.Business.DTOs;


namespace ClinicksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultasController : ControllerBase
    {
        private readonly IConsultaService _consultaService;

        // Inyectamos el servicio
        public ConsultasController(IConsultaService consultaService)
        {
            _consultaService = consultaService;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarConsulta([FromBody] ConsultaAltaDto dto)
        {
            // 1. El ID del médico es 1 por que no tenemos login
            int idMedicoPrueba = 2;

            var resultado = await _consultaService.RegistrarConsulta(dto, idMedicoPrueba);

            if (resultado.Success)
            {
                return Ok(new { 
                    success = true, 
                    mensaje = "Consulta guardada correctamente" 
                });
            }

                return BadRequest(new { 
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