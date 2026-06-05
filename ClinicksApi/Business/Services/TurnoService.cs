using ClinicksApi.Business.DTOs;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Interfaces;
using ClinicksApi.Data.Entities;

namespace ClinicksApi.Business.Services
{
    /// <inheritdoc/>
    public class TurnoService : ITurnoService
    {
        private readonly ITurnoRepository _turnoRepository;

        public TurnoService(ITurnoRepository turnoRepository)
        {
            _turnoRepository = turnoRepository;
        }
        
        /// <inheritdoc/>
        public async Task<IEnumerable<TurnoAgendaDto>> ObtenerTurnosAgendadosAsync()
        {
            var turnosDB = await _turnoRepository.ObtenerTodosAsync();

            return turnosDB.Select(MapToDto);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TurnoAgendaDto>> ObtenerTurnosMedicoAsync(int idMedico, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var turnosDB = await _turnoRepository.ObtenerTurnosMedicoAsync(idMedico, fechaInicio, fechaFin);

            return turnosDB.Select(MapToDto);
        }

        /// <inheritdoc/>
        public async Task<TurnoAgendaDto?> ObtenerTurnoPorIdAsync(int idTurno)
        {
            var t = await _turnoRepository.ObtenerPorIdAsync(idTurno);
            if (t == null) return null;

            return MapToDto(t);
        }

        /// <summary>
        /// Método auxiliar para mapear la entidad Turno a TurnoAgendaDto.
        /// </summary>
        private TurnoAgendaDto MapToDto(Turno t)
        {
            return new TurnoAgendaDto
            {
                IdTurno = t.IdTurno,
                IdPaciente = t.IdPaciente,
                FechaTurno = t.FechaTurno,
                PacienteNombreCompleto = t.IdPacienteNavigation != null ? $"{t.IdPacienteNavigation.Nombre} {t.IdPacienteNavigation.Apellido}".Trim() : "Paciente desconocido",
                DniPaciente = t.IdPacienteNavigation?.Dni ?? string.Empty,
                Motivo = t.Motivo ?? string.Empty,
                Estado = t.IdEstadoTurnoNavigation?.Nombre ?? string.Empty
            };
        }
    }
}