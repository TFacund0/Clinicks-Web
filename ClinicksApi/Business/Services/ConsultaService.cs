using ClinicksApi.Business.DTOs;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using Microsoft.Extensions.Logging;
using ClinicksApi.Constants;

namespace ClinicksApi.Business.Services
{
    /// <inheritdoc/>
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaRepository _consultaRepo;
        private readonly IPacienteService _pacienteService;
        private readonly ITurnoRepository _turnoRepository;
        private readonly ILogger<ConsultaService> _logger;
        
        /// <summary>Constructor de ConsultaService</summary>
        /// <param name="consultaRepo">Repositorio de consultas</param>
        /// <param name="pacienteService">Servicio para verificar la existencia del paciente por DNI antes de registrar.</param>
        /// <param name="turnoRepository">Repositorio de turnos</param>
        /// <param name="logger">Logger de diagnóstico del servicio.</param>
        public ConsultaService(IConsultaRepository consultaRepo, IPacienteService pacienteService, ITurnoRepository turnoRepository, ILogger<ConsultaService> logger)
        {
            _consultaRepo = consultaRepo;
            _pacienteService = pacienteService;
            _turnoRepository = turnoRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<List<ConsultaHistorialDto>> ObtenerListaConsultas()
        {
            var consultas = await _consultaRepo.ListaConsultas();
            return consultas.Select(c => new ConsultaHistorialDto
            {
                IdConsulta = c.IdConsulta,
                Motivo = c.Motivo,
                Diagnostico = c.Diagnostico,
                Tratamiento = c.Tratamiento ?? string.Empty,
                Observacion = c.Observacion ?? string.Empty,
                Recomendacion = c.Recomendacion ?? string.Empty,
                FechaConsulta = c.FechaConsulta,
                MedicoAtencion = c.IdMedicoNavigation != null ? $"{c.IdMedicoNavigation.Nombre} {c.IdMedicoNavigation.Apellido}" : "Médico Desconocido"
            }).ToList();
        }

        /// <inheritdoc/>
        public async Task<(bool Success, string Message, ConsultaMedica? Data)> RegistrarConsulta(ConsultaAltaDto consulta, int idMedico)
        {
            try
            {

                if (consulta.fechaconsulta != null && consulta.fechaconsulta > DateTime.Now)
                    return (false, "La fecha de consulta no puede ser futura.", null);


                if (idMedico <= 0)
                    return (false, "El Id del Médico logueado es obligatorio y debe ser mayor a cero.", null);


                var pacienteDto = await _pacienteService.ObtenerPorDni(consulta.dnipaciente);
                if (pacienteDto == null)
                    return (false, "Paciente no encontrado o no apto para consultas.", null);


                var nuevaConsulta = new ConsultaMedica
                {
                    Motivo          = consulta.motivo,
                    Diagnostico     = consulta.diagnostico,

                    Tratamiento     = consulta.tratamiento   ?? "sin definir",
                    Observacion     = consulta.observaciones ?? "sin observaciones relevantes",
                    Recomendacion   = consulta.recomendacion ?? "sin recomendaciones",
                    FechaConsulta   = consulta.fechaconsulta ?? DateTime.Now,
                    IdMedico        = idMedico,

                    IdPaciente      = pacienteDto.Id
                };


                var resultado = await _consultaRepo.RegistrarConsulta(nuevaConsulta);

                int idEstadoHecho = await _turnoRepository.ObtenerIdEstadoPorNombreAsync("Atendido") 
                                 ?? ConstantesGenerales.EstadosTurno.AtendidoId;

                if (consulta.idTurno.HasValue && consulta.idTurno.Value > 0)
                {
                    var turnoAActualizar = await _turnoRepository.ObtenerParaActualizarAsync(consulta.idTurno.Value);
                    if (turnoAActualizar != null)
                    {
                        turnoAActualizar.IdConsulta = resultado.IdConsulta;
                        turnoAActualizar.IdEstadoTurno = idEstadoHecho;
                        await _turnoRepository.ActualizarTurnoAsync(turnoAActualizar);
                    }
                }
                else
                {
                    var nuevoTurno = new Turno
                    {
                        IdPaciente      = pacienteDto.Id,
                        IdMedico        = idMedico,
                        IdEstadoTurno   = idEstadoHecho,
                        FechaTurno      = nuevaConsulta.FechaConsulta ?? DateTime.Now,
                        Motivo          = $"Consulta: {consulta.motivo}",
                        IdConsulta      = resultado.IdConsulta
                    };
                    await _turnoRepository.CrearTurnoAsync(nuevoTurno);
                }

                return (true, "Consulta registrada exitosamente", resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar la consulta médica. Paciente DNI: {DniPaciente}", consulta.dnipaciente);
                return (false, "Error interno al registrar la consulta médica. Por favor, intente nuevamente más tarde.", null);
            }
        }
    }
}