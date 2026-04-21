using Microsoft.AspNetCore.Mvc;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.DTOs; // CORREGIDO: DTOs en mayúscula

namespace ClinicksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;

        public PacientesController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        // GET: api/pacientes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pacientes = await _pacienteService.ObtenerListado();
            return Ok(pacientes);
        }

        // GET: api/pacientes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var paciente = await _pacienteService.ObtenerPorId(id);

            if (paciente == null)
                return NotFound(new { message = "Paciente no encontrado" });

            return Ok(paciente);
        }

        // GET: api/pacientes/atendidos/1
        [HttpGet("atendidos/{medicoId}")]
        public async Task<IActionResult> GetAtendidosByMedico(int medicoId)
        {
            var pacientes = await _pacienteService.ObtenerAtendidosPorMedico(medicoId);
            return Ok(pacientes);
        }
    }
}