using ClinicksApi.Business.DTOs;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;

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
        private readonly IPacienteRepository _pacienteRepo;

        /// <summary>
        /// Constructor del servicio. Recibe los repositorios inyectados por el contenedor de dependencias de .NET.
        /// </summary>
        /// <param name="consultaRepo">Repositorio que ejecuta operaciones SQL sobre la tabla consulta_medica.</param>
        /// <param name="pacienteRepo">Repositorio para verificar la existencia del paciente por DNI antes de registrar.</param>
        public ConsultaService(IConsultaRepository consultaRepo, IPacienteRepository pacienteRepo)
        {
            _consultaRepo = consultaRepo;
            _pacienteRepo = pacienteRepo;
        }

        /// <inheritdoc/>
        public async Task<List<ConsultaMedica>> ObtenerListaConsultas()
        {
            return await _consultaRepo.ListaConsultas();
        }

        /// <inheritdoc/>
        public async Task<List<ConsultaMedica>> ObtenerHistorialPaciente(int pacienteId)
        {
            return await _consultaRepo.HistorialPaciente(pacienteId);
        }

        /// <inheritdoc/>
        public async Task<(bool Success, string Message, ConsultaMedica? Data)> RegistrarConsulta(ConsultaAltaDto dto, int idMedicoLogueado)
        {
            try
            {
                // 1. REGLAS DE NEGOCIO — Validaciones de campos obligatorios.
                if (string.IsNullOrWhiteSpace(dto.motivo))
                    return (false, "El Motivo es obligatorio.", null);

                if (string.IsNullOrWhiteSpace(dto.diagnostico))
                    return (false, "El Diagnóstico es obligatorio.", null);

                // Prevenimos viajes en el tiempo: la fecha no puede ser del futuro.
                if (dto.fechaconsulta != null && dto.fechaconsulta > DateTime.Now)
                    return (false, "La fecha de consulta no puede ser futura.", null);

                // El médico debe ser válido y provenir del Token JWT.
                if (idMedicoLogueado <= 0)
                    return (false, "El Id del Médico logueado es obligatorio y debe ser mayor a cero.", null);

                // 2. VERIFICACIÓN CRUZADA — Validamos que el DNI ingresado exista realmente en el sistema.
                var paciente = await _pacienteRepo.GetByDniAsync(dto.dnipaciente);
                if (paciente == null)
                    return (false, "Paciente no encontrado.", null);

                // 3. MAPEO — Convertimos el DTO (JSON de React) en una Entidad que entiende Entity Framework.
                var nuevaConsulta = new ConsultaMedica
                {
                    Motivo          = dto.motivo,
                    Diagnostico     = dto.diagnostico,
                    // Si el frontend no mandó estos campos, usamos valores por defecto con el operador "??".
                    Tratamiento     = dto.tratamiento   ?? "sin definir",
                    Observacion     = dto.observaciones ?? "sin observaciones relevantes",
                    Recomendacion   = dto.recomendacion ?? "sin recomendaciones",
                    FechaConsulta   = dto.fechaconsulta ?? DateTime.Now,
                    IdMedico        = idMedicoLogueado,
                    // Usamos el ID real de la BD; no confiamos en el DNI como identificador.
                    IdPaciente      = paciente.IdPaciente
                };

                // 4. GUARDADO — El repositorio ejecuta el INSERT en PostgreSQL.
                var resultado = await _consultaRepo.CrearConsulta(nuevaConsulta);

                return (true, "Consulta registrada con éxito.", resultado);
            }
            catch (Exception ex)
            {
                // Capturamos la causa raíz del error de base de datos (ej: violación de clave foránea).
                var detalle = ex.InnerException != null ? $" Detalle: {ex.InnerException.Message}" : "";
                return (false, $"Error interno: {ex.Message}{detalle}", null);
            }
        }
    }
}