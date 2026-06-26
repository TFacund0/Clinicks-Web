using ClinicksApi.Business.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ClinicksApi.Business.Interfaces;

/// <summary>
/// Contrato que define las operaciones de negocio permitidas para la entidad Paciente.
/// </summary>
public interface IPacienteService
{
    /// <summary>
    /// Obtiene el listado completo de pacientes registrados.
    /// </summary>
    Task<IEnumerable<PacienteDto>> ObtenerTodos();

    /// <summary>
    /// Busca y obtiene un paciente específico por su ID.
    /// </summary>
    /// <param name="id">El ID numérico del paciente.</param>
    Task<PacienteDto?> ObtenerPorId(int id);

    /// <summary>
    /// Obtiene la lista de pacientes que han sido atendidos por un médico específico.
    /// </summary>
    /// <param name="medicoId">El ID del médico.</param>
    /// <param name="search">Cadena de texto opcional para filtrar los resultados por nombre o DNI.</param>
    Task<IEnumerable<PacienteDto>> ObtenerAtendidosPorMedico(int medicoId, string? search = null);

    /// <summary>
    /// Busca y obtiene un paciente específico por su número de DNI.
    /// </summary>
    /// <param name="dni">El DNI del paciente a buscar.</param>
    Task<PacienteDto?> ObtenerPorDni(string dni);

    /// <summary>
    /// Verifica la existencia de un paciente en el sistema a partir de su DNI.
    /// </summary>
    /// <param name="dni">El DNI del paciente a validar.</param>
    Task<PacienteDto?> ValidarPaciente(string dni);

    /// <summary>
    /// Obtiene el historial clínico completo e integral de un paciente (consultas y procedimientos).
    /// </summary>
    /// <param name="pacienteId">El ID numérico del paciente.</param>
    Task<HistorialClinicoDto?> ObtenerHistorialClinico(int pacienteId);
}