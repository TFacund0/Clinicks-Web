using ClinicksApi.Business.Dtos;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ClinicksApi.Business.Interfaces;

/// <summary>
/// Contrato que define las operaciones de negocio permitidas para la entidad Paciente.
/// </summary>
public interface IPacienteService
{
    Task<IEnumerable<PacienteDto>> ObtenerListado();
    Task<PacienteDto?> ObtenerPorId(int id);
    Task<IEnumerable<PacienteDto>> ObtenerAtendidosPorMedico(int medicoId);

    Task<(bool Success, string Message)> ExistePaciente(string dni);
}