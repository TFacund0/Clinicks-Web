using ClinicksApi.Business.DTOs;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace ClinicksApi.Business.Services
{
    /// <summary>
    /// Implementación del servicio de negocio para las consultas médicas.
    /// Es la capa intermedia entre el Controlador (que recibe las peticiones de React) 
    /// y el Repositorio (que habla directamente con PostgreSQL).
    /// Se encarga de aplicar las reglas de negocio y validaciones antes de que los datos toquen la base de datos.
    /// </summary>
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaRepository _consultaRepo;
        private readonly IPacienteService _pacienteService;
        private readonly ILogger<ConsultaService> _logger;
        
        /// <param name="pacienteService">Servicio para verificar la existencia del paciente por DNI antes de registrar.</param>
        /// <param name="logger">Logger de diagnóstico del servicio.</param>
        public ConsultaService(IConsultaRepository consultaRepo, IPacienteService pacienteService, ILogger<ConsultaService> logger)
        {
            _consultaRepo = consultaRepo;
            _pacienteService = pacienteService;
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
        public async Task<List<ConsultaHistorialDto>> ObtenerHistorialPaciente(int pacienteId)
        {
            var consultas = await _consultaRepo.HistorialPaciente(pacienteId);
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

        public async Task<(bool Success, string Message, ConsultaMedica? Data)> RegistrarConsulta(ConsultaAltaDto consulta, int idMedicoLogueado)
        {
            try
            {
                if (consulta.fechaconsulta != null && consulta.fechaconsulta > DateTime.Now)
                    return (false, "La fecha de consulta no puede ser futura.", null);
 
                if (idMedicoLogueado <= 0)
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
                    IdMedico        = idMedicoLogueado,
                    IdPaciente      = pacienteDto.Id
                };

                var resultado = await _consultaRepo.RegistrarConsulta(nuevaConsulta);

                if (consulta.idturno.HasValue && consulta.idturno.Value > 0)
                {
                    await _consultaRepo.ActualizarTurnoVinculado(consulta.idturno.Value, resultado.IdConsulta);
                }
                else
                {
                    int idEstadoHecho = 1; // ID 1 representa el estado "Realizado"
                    var nuevoTurno = new Turno
                    {
                        IdPaciente      = pacienteDto.Id,
                        IdMedico        = idMedicoLogueado,
                        IdEstadoTurno   = idEstadoHecho,
                        FechaTurno      = nuevaConsulta.FechaConsulta ?? DateTime.Now,
                        Motivo          = $"Consulta: {consulta.motivo}",
                        IdConsulta      = resultado.IdConsulta
                    };

                    await _consultaRepo.CrearTurnoVinculado(nuevoTurno);
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