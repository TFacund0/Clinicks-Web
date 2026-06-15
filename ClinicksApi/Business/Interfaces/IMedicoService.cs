using ClinicksApi.Business.DTOs;

namespace ClinicksApi.Business.Interfaces
{
    /// <summary>
    /// Contrato para las reglas de negocio del módulo de Médicos.
    /// </summary>
    public interface IMedicoService
    {
        /// <summary>
        /// Obtiene un médico por su ID.
        /// </summary>
        Task<MedicoDto?> ObtenerPorIdAsync(int idMedico);

        /// <summary>
        /// Obtiene un médico por el ID del usuario asociado.
        /// </summary>
        Task<MedicoDto?> ObtenerPorUsuarioIdAsync(int usuarioId);

    }
}
