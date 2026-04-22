using ClinicksApi.Business.DTOs;

namespace ClinicksApi.Business.Interfaces
{
    public interface IPacienteService
    {
        Task<IEnumerable<PacienteDto>> ObtenerListado();
        Task<PacienteDto?> ObtenerPorId(int id);
        Task<IEnumerable<PacienteDto>> ObtenerAtendidosPorMedico(int medicoId);
        Task<IEnumerable<PacienteDto>> BuscarPorDniPartial(string dni);
    }
}