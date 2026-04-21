using ClinicksApi.Data.Entities;
using ClinicksApi.Models;

namespace ClinicksApi.Data.Interfaces
{
    public interface IPacienteRepository
    {
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
