using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Interfaces
{
    /// <summary>
    /// Contrato exclusivo para el acceso a la tabla Turnos en PostgreSQL.
    /// </summary>
    public interface ITurnoRepository
    {
        /// <summary> Ejecuta un SELECT * FROM Turnos incluyendo las tablas relacionadas por defecto.</summary>
        Task<IEnumerable<Turno>> GetAllAsync();

        /// <summary>
        /// Obtiene los turnos de un médico específico, con filtros opcionales por rango de fechas.
        /// </summary>
        /// <param name="idMedico">El ID del médico.</param>
        /// <param name="fechaInicio">La fecha de inicio opcional para el filtro.</param>
        /// <param name="fechaFin">La fecha de fin opcional para el filtro.</param>
        /// <returns>Una lista de turnos correspondientes al médico y período especificados.</returns>
        Task<IEnumerable<Turno>> GetTurnosByMedicoIdAsync(int idMedico, DateTime? fechaInicio, DateTime? fechaFin);

        /// <summary>Obtiene un turno por su ID.</summary>
        Task<Turno?> GetByIdAsync(int idTurno);

        Task CrearTurnoAsync(Turno turno);
        Task ActualizarTurnoAsync(Turno turno);
    }
}