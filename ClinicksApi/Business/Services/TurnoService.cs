using ClinicksApi.Business.DTOs;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Interfaces;

namespace ClinicksApi.Business.Services
{
    public class TurnoService : ITurnoService
    {
        private readonly ITurnoRepository _turnoRepository;

        public TurnoService(ITurnoRepository turnoRepository)
        {
            _turnoRepository = turnoRepository;
        }
        public async Task<IEnumerable<TurnoAgendaDto>> obtenerTurnosAgendadosAsync()
        {
            var turnosDB = await _turnoRepository.GetAllAsync();

            return turnosDB.Select(t => new TurnoAgendaDto
            {
                IdTurno = t.IdTurno,
                FechaTurno = t.FechaTurno,
                PacienteNombreCompleto = $"{t.IdPacienteNavigation.Nombre} {t.IdPacienteNavigation.Apellido}" ?? "Paciente desconocido",
                DniPaciente = t.IdPacienteNavigation.Dni ?? string.Empty,
                Motivo = t.Motivo ?? string.Empty,
                Estado = t.IdEstadoTurnoNavigation.Nombre ?? string.Empty
            });
        }

        public async Task<IEnumerable<TurnoAgendaDto>> ObtenerTurnosMedicoAsync(int idMedico, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var turnosDB = await _turnoRepository.GetTurnosByMedicoIdAsync(idMedico, fechaInicio, fechaFin);

            return turnosDB.Select(t => new TurnoAgendaDto
            {
                IdTurno = t.IdTurno,
                FechaTurno = t.FechaTurno,
                PacienteNombreCompleto = $"{t.IdPacienteNavigation?.Nombre} {t.IdPacienteNavigation?.Apellido}".Trim() ?? "Paciente desconocido",
                DniPaciente = t.IdPacienteNavigation?.Dni ?? string.Empty,
                Motivo = t.Motivo ?? string.Empty,
                Estado = t.IdEstadoTurnoNavigation?.Nombre ?? string.Empty
            });
        }
    }
}