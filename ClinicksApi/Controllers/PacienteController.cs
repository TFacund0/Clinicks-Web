using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.Dtos;
using System.Threading.Tasks;

namespace ClinicksApi.Controllers;

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
        var pacientes = await _pacienteService.ObtenerAtendidosPorMedico(medicoId);
        return Ok(pacientes);
    }

    /// <summary>
    /// Verifica si existe un paciente registrado en la base de datos a partir de su DNI.
    /// </summary>
    /// <param name="dni">DNI del paciente.</param>
    [HttpGet("validar/{dni}")]
    public async Task<IActionResult> ValidarPaciente(string dni)
    {
        var resultado = await _pacienteService.ExistePaciente(dni);

        if (resultado.Success)
        {
            return Ok(new { 
                success = true, 
                mensaje = "Paciente verificado correctamente." 
            });
        }

        return NotFound(new { 
            success = false, 
            mensaje = "El DNI ingresado no corresponde a un paciente registrado." 
        });
    }
}