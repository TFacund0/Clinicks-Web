using ClinicksApi.Business.Dtos;
using ClinicksApi.Business.Services;
using System.Threading.Tasks;

namespace ClinicksApi.Business.Interfaces;

public interface IPacienteService
{
    // Método para obtener el listado de pacientes
    Task<IEnumerable<PacienteDto>> ObtenerListado();

    // Método para obtener un paciente por su ID
    Task<PacienteDto?> ObtenerPorId(int id);

    // Método para obtener pacientes atendidos por un médico específico
    Task<IEnumerable<PacienteDto>> ObtenerAtendidosPorMedico(int medicoId);
}