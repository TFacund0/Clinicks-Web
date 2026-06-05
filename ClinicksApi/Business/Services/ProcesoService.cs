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
        
        /// <param name="pacienteService">Servicio para verificar la existencia del paciente por DNI antes de registrar.</param>
        /// <param name="logger">Logger de diagnóstico del servicio.</param>
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
                // 1. REGLAS DE NEGOCIO — Validaciones (los campos obligatorios ya se validan en el DTO).
                if (idMedico <= 0)
                    return (false, "El médico logueado es inválido.", null);

                if (procedimiento.fechaproceso != null && procedimiento.fechaproceso > DateTime.Now)
                    return (false, "La fecha del proceso no puede ser futura.", null);

                // 2. VERIFICACIÓN CRUZADA — Validamos que el DNI del paciente exista en el sistema.
                // A través del IPacienteService para respetar la separación de incumbencias (SoC).
                var pacienteDto = await _pacienteService.ObtenerPorDni(procedimiento.dnipaciente);
                if (pacienteDto == null)
                    return (false, "Paciente no encontrado en la base de datos o no apto para procedimientos.", null);

                // Si no se envió fecha, usamos la fecha y hora actual del servidor.
                var fechaAUsar = procedimiento.fechaproceso ?? DateTime.Now;

                // 3. MAPEO — Construimos la entidad Procedimiento a partir del DTO del frontend.
                var nuevoProcedimiento = new Procedimiento
                {
                    Tipo        = procedimiento.tipoproceso,
                    Descripcion = procedimiento.descripcion,
                    Resultado   = procedimiento.resultado ?? "Sin resultado ingresado",
                    Fecha       = fechaAUsar
                };

                // 4. GUARDADO — El repositorio ejecuta el INSERT en PostgreSQL vinculándolo a un turno.
                var procGuardado = await _procesoRepo.RegistrarProcedimiento(nuevoProcedimiento);

                if (procedimiento.idTurno.HasValue && procedimiento.idTurno.Value > 0)
                {
                    var turnoAActualizar = await _turnoRepository.ObtenerPorIdAsync(procedimiento.idTurno.Value);
                    if (turnoAActualizar != null)
                    {
                        turnoAActualizar.IdProcedimiento = procGuardado.IdProcedimiento;
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
                // Capturamos la causa raíz del error de base de datos en el log interno sin exponerla al cliente.
                _logger.LogError(ex, "Error al registrar el procedimiento médico. Paciente DNI: {DniPaciente}", procedimiento.dnipaciente);
                return (false, "Error interno al registrar el procedimiento médico. Por favor, intente nuevamente más tarde.", null);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<object> ObtenerTiposProceso()
        {
            return _tiposProceso;
        }

        /// <inheritdoc/>

    }
}
