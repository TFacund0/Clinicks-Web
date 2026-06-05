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
        
        /// <param name="pacienteService">Servicio para verificar la existencia del paciente por DNI antes de registrar.</param>
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
                // 1. REGLAS DE NEGOCIO — Validaciones de negocio (los campos obligatorios ya se validan en el DTO).
                // Prevenimos viajes en el tiempo: la fecha no puede ser del futuro.
                if (consulta.fechaconsulta != null && consulta.fechaconsulta > DateTime.Now)
                    return (false, "La fecha de consulta no puede ser futura.", null);

                // El médico debe ser válido y provenir del Token JWT.
                if (idMedico <= 0)
                    return (false, "El Id del Médico logueado es obligatorio y debe ser mayor a cero.", null);

                // 2. VERIFICACIÓN CRUZADA — Validamos que el DNI ingresado exista realmente en el sistema.
                // Lo hacemos a través de IPacienteService para respetar SoC y aplicar sus reglas de negocio.
                var pacienteDto = await _pacienteService.ObtenerPorDni(consulta.dnipaciente);
                if (pacienteDto == null)
                    return (false, "Paciente no encontrado o no apto para consultas.", null);

                // 3. MAPEO — Convertimos el DTO (JSON de React) en una Entidad que entiende Entity Framework.
                var nuevaConsulta = new ConsultaMedica
                {
                    Motivo          = consulta.motivo,
                    Diagnostico     = consulta.diagnostico,
                    // Si el frontend no mandó estos campos, usamos valores por defecto con el operador "??".
                    Tratamiento     = consulta.tratamiento   ?? "sin definir",
                    Observacion     = consulta.observaciones ?? "sin observaciones relevantes",
                    Recomendacion   = consulta.recomendacion ?? "sin recomendaciones",
                    FechaConsulta   = consulta.fechaconsulta ?? DateTime.Now,
                    IdMedico        = idMedico,
                    // Usamos el ID real de la BD proveído por el DTO; no confiamos en el DNI como identificador.
                    IdPaciente      = pacienteDto.Id
                };

                // 4. GUARDADO — El repositorio ejecuta el INSERT en PostgreSQL vinculándolo a un turno.
                var resultado = await _consultaRepo.RegistrarConsulta(nuevaConsulta);

                if (consulta.idTurno.HasValue && consulta.idTurno.Value > 0)
                {
                    var turnoAActualizar = await _turnoRepository.ObtenerPorIdAsync(consulta.idTurno.Value);
                    if (turnoAActualizar != null)
                    {
                        turnoAActualizar.IdConsulta = resultado.IdConsulta;
                        turnoAActualizar.IdEstadoTurno = ConstantesGenerales.EstadosTurno.RealizadoId;
                        await _turnoRepository.ActualizarTurnoAsync(turnoAActualizar);
                    }
                }
                else
                {
                    int idEstadoHecho = ConstantesGenerales.EstadosTurno.RealizadoId;
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