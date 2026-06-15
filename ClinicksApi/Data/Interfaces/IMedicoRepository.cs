using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Interfaces
{
    /// <summary>
    /// Contrato para el acceso a datos relacionados con los Médicos.
    /// </summary>
    public interface IMedicoRepository
    {
        /// <summary>
        /// Obtiene un médico por su identificador único.
        /// </summary>
        Task<Medico?> ObtenerPorIdAsync(int idMedico);

        /// <summary>
        /// Obtiene un médico por el ID del usuario asociado.
        /// </summary>
        Task<Medico?> ObtenerPorUsuarioIdAsync(int usuarioId);

    }
}
