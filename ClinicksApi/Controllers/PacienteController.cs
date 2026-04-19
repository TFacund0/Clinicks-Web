using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.Dtos;               // Namespace de tus DTOs
using Microsoft.AspNetCore.Mvc;

namespace ClinicksApi.Controllers;

[ApiController]
[Route("api/[controller]")] // Esto hace que la URL sea: api/pacientes
public class PacientesController : ControllerBase
{
    private readonly IPacienteService _pacienteService;
    
    // El controlador pide la INTERFAZ por constructor
    public PacientesController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    // GET: api/pacientes
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // 1. Le pide al servicio la lista de pacientes (en formato DTO)
        var pacientes = await _pacienteService.ObtenerListado();

        // 2. Devuelve un 200 OK con los datos
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
}   