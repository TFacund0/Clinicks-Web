using ClinicksApi.Business.DTOs;

namespace ClinicksApi.Business.Interfaces
{
    public interface IPacienteService
    {
        Task<IEnumerable<PacienteDto>> ObtenerListado();
        Task<PacienteDto?> ObtenerPorId(int id);
        Task<IEnumerable<PacienteDto>> ObtenerAtendidosPorMedico(int medicoId);

        Task<(bool Success, string Message)> ExistePaciente(string dni);
    }
}