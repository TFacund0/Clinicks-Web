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

        /// <inheritdoc/>
        public async Task<int> CancelarTurnosVencidosAsync()
        {
            var fechaLimite = DateTime.Now.Date;

            var estadosPendientesNombres = new List<string> { "pendiente", "confirmado" };
            var estadosPendientesIds = await _turnoRepository.ObtenerIdsEstadosPorNombresAsync(estadosPendientesNombres);

            if (!estadosPendientesIds.Any())
            {
                estadosPendientesIds.Add(ClinicksApi.Constants.ConstantesGenerales.EstadosTurno.ConfirmadoId);
            }

            var idCancelado = await _turnoRepository.ObtenerIdEstadoPorNombreAsync("cancelado");
            if (idCancelado == null || idCancelado == 0)
            {
                idCancelado = ClinicksApi.Constants.ConstantesGenerales.EstadosTurno.CanceladoId;
            }

            var turnosVencidos = await _turnoRepository.ObtenerTurnosPorFechaYEstadosAsync(fechaLimite, estadosPendientesIds);

            if (turnosVencidos.Any())
            {
                foreach (var turno in turnosVencidos)
                {
                    turno.IdEstadoTurno = idCancelado.Value;
                }

                await _turnoRepository.ActualizarLoteTurnosAsync(turnosVencidos);
            }

            return turnosVencidos.Count;
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
                Estado = t.IdEstadoTurnoNavigation?.Nombre ?? string.Empty,
                IdConsulta = t.IdConsulta,
                IdProcedimiento = t.IdProcedimiento
            };
        }
    }
}