using ClinicksApi.Business.DTOs;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using Microsoft.Extensions.Logging;
using ClinicksApi.Constants;

namespace ClinicksApi.Business.Services
{
    /// <inheritdoc/>
    public class ProcesoService : IProcesoService
    {
        private readonly IProcesoRepository _procesoRepo;
        private readonly IPacienteService _pacienteService;
        private readonly ITurnoRepository _turnoRepository;
        private readonly ILogger<ProcesoService> _logger;
        

        private static readonly List<object> _tiposProceso = new List<object>
        {
            new { id = 1, nombre = "Cirugía Menor" },
            new { id = 2, nombre = "Estudio de Imagen (Rayos X, MRI)" },
            new { id = 3, nombre = "Análisis de Laboratorio" },
            new { id = 4, nombre = "Rehabilitación Física" },
            new { id = 5, nombre = "Consulta Especializada" },
            new { id = 6, nombre = "Procedimiento Odontológico" },
            new { id = 7, nombre = "Curación de Heridas" },
            new { id = 8, nombre = "Chequeo General" },
            new { id = 9, nombre = "Otro" }
        };

        /// <summary>Constructor de ProcesoService</summary>
        /// <param name="procesoRepo">Repositorio de procesos</param>
        /// <param name="pacienteService">Servicio para verificar la existencia del paciente por DNI antes de registrar.</param>
        /// <param name="turnoRepository">Repositorio de turnos</param>
        /// <param name="logger">Logger de diagnóstico del servicio.</param>
        public ProcesoService(IProcesoRepository procesoRepo, IPacienteService pacienteService, ITurnoRepository turnoRepository, ILogger<ProcesoService> logger)
        {
            _procesoRepo = procesoRepo;
            _pacienteService = pacienteService;
            _turnoRepository = turnoRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<(bool Success, string Message, Procedimiento? Data)> RegistrarProcedimiento(ProcedimientoAltaDto procedimiento, int idMedico)
        {
            try
            {

                if (idMedico <= 0)
                    return (false, "El médico logueado es inválido.", null);

                if (procedimiento.fechaproceso != null && procedimiento.fechaproceso > DateTime.Now)
                    return (false, "La fecha del proceso no puede ser futura.", null);


                var pacienteDto = await _pacienteService.ObtenerPorDni(procedimiento.dnipaciente);
                if (pacienteDto == null)
                    return (false, "Paciente no encontrado en la base de datos o no apto para procedimientos.", null);


                var fechaAUsar = procedimiento.fechaproceso ?? DateTime.Now;


                var nuevoProcedimiento = new Procedimiento
                {
                    Tipo        = procedimiento.tipoproceso,
                    Descripcion = procedimiento.descripcion,
                    Resultado   = procedimiento.resultado ?? "Sin resultado ingresado",
                    Fecha       = fechaAUsar
                };


                int idEstadoHecho = await _turnoRepository.ObtenerIdEstadoPorNombreAsync("Atendido") 
                                 ?? ConstantesGenerales.EstadosTurno.AtendidoId;


                var procGuardado = await _procesoRepo.RegistrarProcedimiento(nuevoProcedimiento);

                if (procedimiento.idTurno.HasValue && procedimiento.idTurno.Value > 0)
                {
                    var turnoExistente = await _turnoRepository.ObtenerParaActualizarAsync(procedimiento.idTurno.Value);
                    if (turnoExistente != null)
                    {
                        turnoExistente.IdProcedimiento = procGuardado.IdProcedimiento;
                        turnoExistente.FinalizarAtencion();
                        await _turnoRepository.ActualizarTurnoAsync(turnoExistente);
                    }
                }
                else
                {
                    var nuevoTurno = new Turno
                    {
                        IdPaciente      = pacienteDto.Id,
                        IdMedico        = idMedico,
                        IdEstadoTurno   = idEstadoHecho,
                        FechaTurno      = fechaAUsar,
                        Motivo          = $"Procedimiento: {procedimiento.tipoproceso}",
                        IdProcedimiento = procGuardado.IdProcedimiento
                    };

                    await _turnoRepository.CrearTurnoAsync(nuevoTurno);
                }

                return (true, "Procedimiento registrado exitosamente", procGuardado);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error al registrar el procedimiento médico. Paciente DNI: {DniPaciente}", procedimiento.dnipaciente);
                return (false, "Error interno al registrar el procedimiento médico. Por favor, intente nuevamente más tarde.", null);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<object> ObtenerTiposProceso()
        {
            return _tiposProceso;
        }

    }
}
