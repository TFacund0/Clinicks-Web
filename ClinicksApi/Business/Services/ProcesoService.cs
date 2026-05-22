using ClinicksApi.Business.DTOs;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace ClinicksApi.Business.Services
{
    /// <summary>
    /// Implementación del servicio de negocio para los procedimientos médicos.
    /// Orquesta el flujo de registro: valida los datos del formulario, verifica al paciente,
    /// persiste el procedimiento y crea el Turno que vincula todas las entidades.
    /// </summary>
    public class ProcesoService : IProcesoService
    {
        private readonly IProcesoRepository _procesoRepo;
        private readonly IPacienteService _pacienteService;
        private readonly ILogger<ProcesoService> _logger;
        /// <param name="pacienteService">Servicio para verificar la existencia del paciente por DNI antes de registrar.</param>
        /// <param name="logger">Logger de diagnóstico del servicio.</param>
        public ProcesoService(IProcesoRepository procesoRepo, IPacienteService pacienteService, ILogger<ProcesoService> logger)
        {
            _procesoRepo = procesoRepo;
            _pacienteService = pacienteService;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<(bool Success, string Message, Procedimiento? Data)> RegistrarProceso(ProcesoAltaDto dto, int idMedicoLogueado)
        {
            try
            {
                // 1. REGLAS DE NEGOCIO — Validaciones (los campos obligatorios ya se validan en el DTO).
                if (idMedicoLogueado <= 0)
                    return (false, "El médico logueado es inválido.", null);

                // 2. VERIFICACIÓN CRUZADA — Validamos que el DNI del paciente exista en el sistema.
                // A través del IPacienteService para respetar la separación de incumbencias (SoC).
                var pacienteDto = await _pacienteService.ObtenerPorDni(dto.dnipaciente);
                if (pacienteDto == null)
                    return (false, "Paciente no encontrado en la base de datos o no apto para procedimientos.", null);

                // Si no se envió fecha, usamos la fecha y hora actual del servidor.
                var fechaAUsar = dto.fechaproceso ?? DateTime.Now;

                // 3. MAPEO — Construimos la entidad Procedimiento a partir del DTO del frontend.
                var nuevoProcedimiento = new Procedimiento
                {
                    Tipo        = dto.tipoproceso,
                    Descripcion = dto.descripcion,
                    Resultado   = dto.resultado ?? "Sin resultado ingresado",
                    Fecha       = fechaAUsar
                };

                // 4. PREPARACIÓN DEL TURNO VINCULADO
                // El estado "Realizado" tiene ID = 1 y ya está garantizado por el DbInitializer al iniciar la app.
                int idEstadoHecho = 1;

                // Construimos el Turno que une el procedimiento con el paciente y el médico.
                var nuevoTurno = new Turno
                {
                    IdPaciente      = pacienteDto.Id,
                    IdMedico        = idMedicoLogueado,
                    IdEstadoTurno   = idEstadoHecho,
                    FechaTurno      = fechaAUsar,
                    Motivo          = $"Procedimiento: {dto.tipoproceso}"
                };

                // 5. GUARDADO ATÓMICO (TRANSACCIONAL) — Persistimos procedimiento y turno en una sola transacción
                var procGuardado = await _procesoRepo.CrearProcedimientoYTurnoVinculado(nuevoProcedimiento, nuevoTurno);

                return (true, "Procedimiento guardado y vinculado con éxito.", procGuardado);
            }
            catch (Exception ex)
            {
                // Capturamos la causa raíz del error de base de datos en el log interno sin exponerla al cliente.
                _logger.LogError(ex, "Error al registrar proceso médico. Paciente DNI: {DniPaciente}", dto.dnipaciente);
                return (false, "Error interno al registrar el proceso en el sistema. Por favor, intente nuevamente más tarde.", null);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<object> ObtenerTiposProceso()
        {
            return new List<object>
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
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ProcesoHistorialDto>> ObtenerHistorialPaciente(int pacienteId)
        {
            var procesos = await _procesoRepo.HistorialPaciente(pacienteId);

            return procesos.Select(p =>
            {
                // Extraer el nombre del médico desde el turno vinculado (si existe)
                var turnoAsociado = p.Turnos?.FirstOrDefault();
                var nombreMedico = turnoAsociado?.IdMedicoNavigation != null
                    ? $"Dr/Dra. {turnoAsociado.IdMedicoNavigation.Nombre} {turnoAsociado.IdMedicoNavigation.Apellido}"
                    : "No registrado";

                return new ProcesoHistorialDto
                {
                    IdProcedimiento = p.IdProcedimiento,
                    Tipo            = p.Tipo,
                    Descripcion     = p.Descripcion ?? string.Empty,
                    Resultado       = p.Resultado ?? string.Empty,
                    Fecha           = p.Fecha,
                    MedicoAtencion  = nombreMedico
                };
            });
        }
    }
}
