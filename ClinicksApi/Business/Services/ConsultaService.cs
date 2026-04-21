using ClinicksApi.Business.DTOs;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using ClinicksApi.Services.Interfaces;

namespace ClinicksApi.Services
{
    public class ConsultaService : IConsultaService
    {
        private readonly IConsultaRepository _consultaRepo;
        private readonly IPacienteRepository _pacienteRepo;

        public ConsultaService(IConsultaRepository consultaRepo, IPacienteRepository pacienteRepo)
        {
            _consultaRepo = consultaRepo;
            _pacienteRepo = pacienteRepo;
        }

        public async Task<List<ConsultaMedica>> ObtenerListaConsultas()
        {
            return await _consultaRepo.ListaConsultas();
        }

        public async Task<List<ConsultaMedica>> ObtenerHistorialPaciente(int pacienteId)
        {
            return await _consultaRepo.HistorialPaciente(pacienteId);
        }

        public async  Task<(bool Success, string Message, ConsultaMedica? Data)> RegistrarConsulta(ConsultaAltaDto dto, int idMedicoLogueado)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.motivo))
                    return (false, "El Motivo es obligatorio.", null);
                if (string.IsNullOrWhiteSpace(dto.diagnostico))
                    return (false, "El Diagnóstico es obligatorio.", null);
                
                if (dto.fechaconsulta != null && dto.fechaconsulta > DateTime.Now)
                    return (false, "La fecha de consulta no puede ser futura.", null);
                if (idMedicoLogueado <= 0)
                    return (false, "El Id del Médico logueado es obligatorio y debe ser mayor a cero.", null);
                
                var paciente = await _pacienteRepo.BuscarPorDni(dto.dnipaciente);
                    if (paciente == null) return (false, "Paciente no encontrado", null);

                
                var nuevaConsulta = new ConsultaMedica
                {
                    Motivo = dto.motivo,
                    Diagnostico = dto.diagnostico,
                    Tratamiento = dto.tratamiento ?? "sin definir",
                    Observacion = dto.observaciones ?? "sin observaciones relevantes",
                    Recomendacion = dto.recomendacion ?? "sin recomendaciones",
                    FechaConsulta = dto.fechaconsulta ?? DateTime.Now, 
                    IdMedico = idMedicoLogueado,
                    IdPaciente = paciente.IdPaciente
                };

                var resultado = await _consultaRepo.CrearConsulta(nuevaConsulta);

                return (true, "Consulta registrada con éxito", resultado);
            }
            catch (Exception ex)
            {
                return (false, $"Error interno: {ex.Message}", null);
            }
        }
    }
}           