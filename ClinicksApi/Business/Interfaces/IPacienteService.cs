using ClinicksApi.Business.Dtos;
using ClinicksApi.Business.Services;
using System.Threading.Tasks;

namespace ClinicksApi.Business.Interfaces;

/// <summary>
/// Contrato que define las operaciones de negocio permitidas para la entidad Paciente.
/// </summary>
public interface IPacienteService
{
    Task<IEnumerable<PacienteDto>> ObtenerListado();
    Task<PacienteDto?> ObtenerPorId(int id);
    Task<IEnumerable<PacienteDto>> ObtenerAtendidosPorMedico(int medicoId);
}