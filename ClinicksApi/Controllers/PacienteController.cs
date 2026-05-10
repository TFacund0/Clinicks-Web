using Microsoft.AspNetCore.Authorization;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ClinicksApi.Controllers;

// Controlador para manejar las operaciones relacionadas con los pacientes
/// <summary>
/// Controlador responsable de gestionar la información de los pacientes de la clínica.
/// Requiere que el usuario esté autenticado con un Token JWT válido para acceder a cualquier método.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly IPacienteService _pacienteService;

    /// <summary>
    /// Constructor del controlador. Recibe el servicio inyectado por .NET.
    /// </summary>
    /// <param name="pacienteService">Servicio que contiene las reglas de negocio para los pacientes.</param>
    public PacientesController(IPacienteService pacienteService)
    {
        _pacienteService = pacienteService;
    }

    /// <summary>
    /// Obtiene el listado completo de todos los pacientes registrados en el sistema.
    /// </summary>
    /// <returns>Una lista de DTOs de pacientes con código de estado 200 (OK).</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pacientes = await _pacienteService.ObtenerListado();
        return Ok(pacientes);
    }

    /// <summary>
    /// Busca un paciente específico utilizando su identificador único.
    /// </summary>
    /// <param name="id">El ID numérico del paciente a buscar (extraído de la URL).</param>
    /// <returns>
    /// El paciente encontrado (200 OK) o un mensaje de error (404 Not Found) si no existe.
    /// </returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var paciente = await _pacienteService.ObtenerPorId(id);

        if (paciente == null)
            return NotFound(new { message = "Paciente no encontrado" });

        return Ok(paciente);
    }

    /// <summary>
    /// Obtiene una lista de los pacientes que han sido atendidos por un médico en particular.
    /// </summary>
    /// <param name="medicoId">El ID numérico del médico (extraído de la URL).</param>
    /// <returns>Lista de pacientes filtrados. Si el médico no tiene pacientes, devuelve una lista vacía (200 OK).</returns>
    [HttpGet("atendidos/{medicoId}")]
    public async Task<IActionResult> GetAtendidosByMedico(int medicoId)
    {
        // Obtenemos los pacientes atendidos por el médico usando el servicio
        var pacientes = await _pacienteService.ObtenerAtendidosPorMedico(medicoId);

        // Si no se encuentran pacientes, devolvemos una lista vacía (en lugar de un error)
        return Ok(pacientes);
    }
}