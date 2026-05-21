using ClinicksApi.Business.DTOs;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;

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
        private readonly IPacienteRepository _pacienteRepo;

        /// <summary>
        /// Constructor del servicio. Recibe los repositorios inyectados por el contenedor de dependencias de .NET.
        /// </summary>
        /// <param name="procesoRepo">Repositorio que ejecuta operaciones SQL sobre las tablas procedimiento y turno.</param>
        /// <param name="pacienteRepo">Repositorio para verificar la existencia del paciente por DNI antes de registrar.</param>
        public ProcesoService(IProcesoRepository procesoRepo, IPacienteRepository pacienteRepo)
        {
            _procesoRepo = procesoRepo;
            _pacienteRepo = pacienteRepo;
        }

        /// <inheritdoc/>
        public async Task<(bool Success, string Message, Procedimiento? Data)> RegistrarProceso(ProcesoAltaDto dto, int idMedicoLogueado)
        {
            try
            {
                // 1. REGLAS DE NEGOCIO — Validaciones de campos obligatorios.
                if (string.IsNullOrWhiteSpace(dto.tipoproceso))
                    return (false, "El tipo de proceso es obligatorio.", null);

                if (string.IsNullOrWhiteSpace(dto.descripcion))
                    return (false, "La descripción es obligatoria.", null);

                if (idMedicoLogueado <= 0)
                    return (false, "El médico logueado es inválido.", null);

                // 2. VERIFICACIÓN CRUZADA — Validamos que el DNI del paciente exista en el sistema.
                var paciente = await _pacienteRepo.GetByDniAsync(dto.dnipaciente);
                if (paciente == null)
                    return (false, "Paciente no encontrado en la base de datos.", null);

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

                // 4. GUARDADO DEL PROCEDIMIENTO — El repositorio ejecuta el INSERT en PostgreSQL.
                var procGuardado = await _procesoRepo.CrearProcedimiento(nuevoProcedimiento);

                // 5. PREPARACIÓN DEL TURNO VINCULADO
                // Garantizamos que el estado "Realizado" exista en la tabla estado_turno y obtenemos su ID real.
                int idEstadoHecho = await _procesoRepo.AsegurarEstadoTurnoExiste("Realizado");

                // Construimos el Turno que une el procedimiento con el paciente y el médico.
                var nuevoTurno = new Turno
                {
                    IdPaciente      = paciente.IdPaciente,
                    IdMedico        = idMedicoLogueado,
                    IdProcedimiento = procGuardado.IdProcedimiento,
                    IdEstadoTurno   = idEstadoHecho,
                    FechaTurno      = fechaAUsar,
                    Motivo          = $"Procedimiento: {dto.tipoproceso}"
                };

                // 6. GUARDADO DEL TURNO — Persiste la relación entre las tres entidades.
                await _procesoRepo.CrearTurnoVinculado(nuevoTurno);

                return (true, "Procedimiento guardado y vinculado con éxito.", procGuardado);
            }
            catch (Exception ex)
            {
                // Capturamos la causa raíz del error de base de datos para un diagnóstico más preciso.
                var detalle = ex.InnerException != null ? $" Detalle: {ex.InnerException.Message}" : "";
                return (false, $"Error interno al registrar proceso: {ex.Message}{detalle}", null);
            }
        }
    }
}
