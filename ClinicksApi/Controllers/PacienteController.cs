using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ClinicksApi.Controllers;

// Controlador para manejar las operaciones relacionadas con los pacientes
[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly IPacienteService _pacienteService;

    // Inyectamos el servicio de pacientes para poder usarlo en los endpoints
    public PacientesController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    // Endpoint para obtener el listado completo de pacientes
    // GET: api/pacientes
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pacientes = await _pacienteService.ObtenerListado();
        return Ok(pacientes);
    }

    // Endpoint para obtener un paciente por su ID
    // GET: api/pacientes/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var paciente = await _pacienteService.ObtenerPorId(id);

        if (paciente == null)
            return NotFound(new { message = "Paciente no encontrado" });

        return Ok(paciente);
    }

    // Endpoint para obtener los pacientes atendidos por un médico específico
    // GET: api/pacientes/atendidos/1
    [HttpGet("atendidos/{medicoId}")]
    public async Task<IActionResult> GetAtendidosByMedico(int medicoId)
    {
        // Obtenemos los pacientes atendidos por el médico usando el servicio
        var pacientes = await _pacienteService.ObtenerAtendidosPorMedico(medicoId);

        // Si no se encuentran pacientes, devolvemos una lista vacía (en lugar de un error)
        return Ok(pacientes);
    }
}