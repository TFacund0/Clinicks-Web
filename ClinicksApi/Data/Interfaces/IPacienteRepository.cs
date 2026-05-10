using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Interfaces
{
    /// <summary>
    /// Contrato exclusivo para el acceso a la tabla Pacientes en PostgreSQL.
    /// </summary>
    public interface IPacienteRepository
    {
        /// <summary>Ejecuta un SELECT * FROM Pacientes incluyendo las tablas relacionadas por defecto.</summary>
        Task<IEnumerable<Paciente>> GetAllAsync();

        /// <summary>Busca un paciente único por su Primary Key (IdPaciente).</summary>
        Task<Paciente?> GetByIdAsync(int id);

        /// <summary>Busca un paciente filtrando por la columna DNI.</summary>
        Task<Paciente?> GetByDniAsync(string dni);

        /// <summary>
        /// Busca pacientes cruzando la tabla Turnos para encontrar aquellos que han sido
        /// atendidos por un médico específico y que su turno tenga un estado en particular.
        /// </summary>
        /// <param name="medicoId">Primary key del médico a filtrar.</param>
        Task<IEnumerable<Paciente>> GetAtendidosByMedicoAsync(int medicoId);

        /// <summary>Verifica si existe un paciente registrado con el DNI proporcionado.</summary>
        Task<bool> ExistePacientePorDniAsync(string dni);
    }
}