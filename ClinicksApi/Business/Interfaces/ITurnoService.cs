using ClinicksApi.Business.DTOs;

namespace ClinicksApi.Business.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de Turnos, que define los métodos relacionados con la gestión de turnos en la clínica.
    /// </summary>
    public interface ITurnoService
    {
        /// <summary>
        /// Obtiene la lista de turnos agendados, incluyendo información del paciente, motivo y estado del turno.
        /// </summary>
        /// <returns>Una lista de objetos TurnoAgendaDto con la información de los turnos agendados.</returns>
        Task<IEnumerable<TurnoAgendaDto>> ObtenerTurnosAgendadosAsync();

        /// <summary>
        /// Obtiene la lista de turnos para un médico específico, con filtros opcionales por fechas.
        /// </summary>
        /// <param name="idMedico">El ID del médico autenticado.</param>
        /// <param name="fechaInicio">Filtro opcional para la fecha de inicio del período.</param>
        /// <param name="fechaFin">Filtro opcional para la fecha de fin del período.</param>
        /// <returns>Una lista de objetos TurnoAgendaDto con la información de los turnos del médico.</returns>
        Task<IEnumerable<TurnoAgendaDto>> ObtenerTurnosMedicoAsync(int idMedico, DateTime? fechaInicio, DateTime? fechaFin);

        /// <summary>Obtiene un turno mapeado a DTO por su ID.</summary>
        Task<TurnoAgendaDto?> ObtenerTurnoPorIdAsync(int idTurno);

        /// <summary>
        /// Busca todos los turnos anteriores a hoy que sigan Pendientes o Agendados y los marca como Cancelados.
        /// Retorna la cantidad de turnos que fueron cancelados.
        /// </summary>
        Task<int> CancelarTurnosVencidosAsync();
    }
}