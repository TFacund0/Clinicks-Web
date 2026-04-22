using ClinicksApi.Data.Entities; 

namespace ClinicksApi.Data.Interfaces
{
    public interface IPacienteRepository
    {
        Task<IEnumerable<Paciente>> GetAllAsync();
        Task<Paciente?> GetByIdAsync(int id);
        Task<Paciente?> GetByDniAsync(string dni);
        Task<IEnumerable<Paciente>> GetAtendidosByMedicoAsync(int medicoId);
        Task<IEnumerable<Paciente>> GetByDniPartialAsync(string dni);
    }
}