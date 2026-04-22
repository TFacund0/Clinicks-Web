using ClinicksApi.Business.DTOs;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;

namespace ClinicksApi.Business.Services
{
    // El "Cerebro" de las consultas médicas. 
    // Esta capa de Servicio se encarga de aplicar las reglas de negocio y validaciones antes de dejar que los datos toquen la base de datos.
    public class ConsultaService : IConsultaService
    {
        // Traemos a los "obreros" (Repositorios) que saben cómo hablar con PostgreSQL.
        private readonly IConsultaRepository _consultaRepo;
        private readonly IPacienteRepository _pacienteRepo;

        // Inyección de dependencias. ASP.NET nos pasa automáticamente los repositorios configurados.
        public ConsultaService(IConsultaRepository consultaRepo, IPacienteRepository pacienteRepo)
        {
            _consultaRepo = consultaRepo;
            _pacienteRepo = pacienteRepo;
        }

        // Pasa directamente la petición al repositorio para traer todas las consultas.
        public async Task<List<ConsultaMedica>> ObtenerListaConsultas()
        {
            return await _consultaRepo.ListaConsultas();
        }

        // Pasa la petición al repositorio para traer la línea de tiempo de un paciente específico.
        public async Task<List<ConsultaMedica>> ObtenerHistorialPaciente(int pacienteId)
        {
            return await _consultaRepo.HistorialPaciente(pacienteId);
        }

        // El método más importante: Valida, prepara y guarda una nueva atención médica.
        // Devuelve una "Tupla" (tres valores a la vez) para que el Controlador sepa qué responder a React.
        public async Task<(bool Success, string Message, ConsultaMedica? Data)> RegistrarConsulta(ConsultaAltaDto dto, int idMedicoLogueado)
        {
            try
            {
                // 1. REGLAS DE NEGOCIO (Validaciones de seguridad)
                // Chequeamos que React no nos haya mandado campos vacíos que son obligatorios.
                if (string.IsNullOrWhiteSpace(dto.motivo))
                    return (false, "El Motivo es obligatorio.", null);

                if (string.IsNullOrWhiteSpace(dto.diagnostico))
                    return (false, "El Diagnóstico es obligatorio.", null);

                // Prevenimos viajes en el tiempo: la fecha no puede ser del futuro.
                if (dto.fechaconsulta != null && dto.fechaconsulta > DateTime.Now)
                    return (false, "La fecha de consulta no puede ser futura.", null);

                // El médico debe existir y ser válido.
                if (idMedicoLogueado <= 0)
                    return (false, "El Id del Médico logueado es obligatorio y debe ser mayor a cero.", null);

                // 2. VERIFICACIÓN CRUZADA
                // Usamos el repo de pacientes para ver si el DNI que tipeó el médico realmente existe.
                var paciente = await _pacienteRepo.GetByDniAsync(dto.dnipaciente);

                if (paciente == null)
                    return (false, "Paciente no encontrado", null); // Rebotamos la petición si el DNI es falso.

                // 3. MAPEO (Transformación)
                // Convertimos el "DTO" (el JSON que mandó React) en una "Entidad" (El objeto que entiende Entity Framework/PostgreSQL).
                var nuevaConsulta = new ConsultaMedica
                {
                    Motivo = dto.motivo,
                    Diagnostico = dto.diagnostico,
                    // Si mandaron estos campos nulos, les ponemos un texto por defecto usando el operador "??".
                    Tratamiento = dto.tratamiento ?? "sin definir",
                    Observacion = dto.observaciones ?? "sin observaciones relevantes",
                    Recomendacion = dto.recomendacion ?? "sin recomendaciones",
                    FechaConsulta = dto.fechaconsulta ?? DateTime.Now,
                    IdMedico = idMedicoLogueado,
                    IdPaciente = paciente.IdPaciente // Sacamos el ID real de la base de datos, no confiamos en el DNI.
                };

                // 4. GUARDADO
                // Le damos la entidad ya perfecta al Repositorio para que haga el "INSERT" en SQL.
                var resultado = await _consultaRepo.CrearConsulta(nuevaConsulta);

                // Respondemos que todo salió espectacular.
                return (true, "Consulta registrada con éxito", resultado);
            }
            catch (Exception ex)
            {
                // Si la base de datos explota (ej: se cortó internet), atrapamos el error y avisamos sin romper la API.
                return (false, $"Error interno: {ex.Message}", null);
            }
        }
    }
}