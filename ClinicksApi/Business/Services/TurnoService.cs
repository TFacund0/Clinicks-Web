using ClinicksApi.Business.DTOs;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Interfaces;

namespace ClinicksApi.Business.Services
{
    /// <summary>
    /// Servicio especialista en aplicar reglas de negocio sobre la información de la Agenda y los Turnos.
    /// Transforma las entidades en DTOs listos para ser consumidos por la web.
    /// </summary>
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
            var turnosDB = await _turnoRepository.GetAllAsync();

            return turnosDB.Select(t => new TurnoAgendaDto
            {
                IdTurno = t.IdTurno,
                IdPaciente = t.IdPaciente,
                FechaTurno = t.FechaTurno,
                PacienteNombreCompleto = t.IdPacienteNavigation != null ? $"{t.IdPacienteNavigation.Nombre} {t.IdPacienteNavigation.Apellido}".Trim() : "Paciente desconocido",
                DniPaciente = t.IdPacienteNavigation?.Dni ?? string.Empty,
                Motivo = t.Motivo ?? string.Empty,
                Estado = t.IdEstadoTurnoNavigation.Nombre ?? string.Empty
            });
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TurnoAgendaDto>> ObtenerTurnosMedicoAsync(int idMedico, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var turnosDB = await _turnoRepository.GetTurnosByMedicoIdAsync(idMedico, fechaInicio, fechaFin);

            return turnosDB.Select(t => new TurnoAgendaDto
            {
                IdTurno = t.IdTurno,
                IdPaciente = t.IdPaciente,
                FechaTurno = t.FechaTurno,
                PacienteNombreCompleto = t.IdPacienteNavigation != null ? $"{t.IdPacienteNavigation.Nombre} {t.IdPacienteNavigation.Apellido}".Trim() : "Paciente desconocido",
                DniPaciente = t.IdPacienteNavigation?.Dni ?? string.Empty,
                Motivo = t.Motivo ?? string.Empty,
                Estado = t.IdEstadoTurnoNavigation?.Nombre ?? string.Empty
            });
        }
    }
}