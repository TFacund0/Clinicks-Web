using Microsoft.AspNetCore.Mvc;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.DTOs;

namespace ClinicksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // La URL base será /api/pacientes

    // Controlador que gestiona las peticiones desde React (Frontend) relacionadas con los pacientes.
    public class PacientesController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;

        // Inyección de dependencias: Recibimos el servicio (el "Cerebro") configurado desde Program.cs.
        public PacientesController(IPacienteService pacienteService)
        {
            _pacienteService = pacienteService;
        }

        // GET: api/pacientes
        [HttpGet]
        // Endpoint genérico para traer a TODOS los pacientes de la base de datos.
        public async Task<IActionResult> GetAll()
        {
            var pacientes = await _pacienteService.ObtenerListado();
            return Ok(pacientes); // Devuelve HTTP 200 con el listado en formato JSON.
        }

        // GET: api/pacientes/5
        [HttpGet("{id}")]
        // Endpoint para buscar a un único paciente por su número de ID.
        public async Task<IActionResult> GetById(int id)
        {
            var paciente = await _pacienteService.ObtenerPorId(id);

            // Si el servicio no encuentra a nadie con ese ID, cortamos acá y mandamos un error 404.
            if (paciente == null)
                return NotFound(new { message = "Paciente no encontrado" });

            return Ok(paciente);
        }

        // GET: api/pacientes/atendidos/1
        [HttpGet("atendidos/{medicoId}")]
        // Endpoint personalizado para la vista del Dashboard. Devuelve solo los pacientes que fueron vistos por un médico específico.
        public async Task<IActionResult> GetAtendidosByMedico(int medicoId)
        {
            var pacientes = await _pacienteService.ObtenerAtendidosPorMedico(medicoId);
            return Ok(pacientes);
        }

        // GET: api/pacientes/buscar?dni=123
        [HttpGet("buscar")]
        // Endpoint para buscar pacientes por DNI. Recibe el DNI como query parameter (?dni=123).
        public async Task<IActionResult> BuscarPorDni([FromQuery] string dni)
        {
            // Si el usuario borra todo y el DNI viene vacío, devolvemos una lista vacía para no procesar nada.
            if (string.IsNullOrWhiteSpace(dni))
                return Ok(new List<PacienteDto>());

            var resultados = await _pacienteService.BuscarPorDniPartial(dni);

            // Respondemos con los 5 pacientes encontrados
            return Ok(resultados);
        }
    }
}