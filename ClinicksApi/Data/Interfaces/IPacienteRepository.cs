using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Interfaces
{
    public interface IPacienteRepository
    {
        Task<Paciente?> BuscarPorDni(string dni);
        // Obtener todos los pacientes
        Task<IEnumerable<Paciente>> GetAllAsync();

        // Obtener un paciente por su ID (Primary Key)
        Task<Paciente?> GetByIdAsync(int id);

        // Buscar un paciente por DNI
        Task<Paciente?> GetByDniAsync(string dni);

        // Obtener pacientes atendidos por un médico específico (medicoId)
        Task<IEnumerable<Paciente>> GetAtendidosByMedicoAsync(int medicoId);
    }
}
