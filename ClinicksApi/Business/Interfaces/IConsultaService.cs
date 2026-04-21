using ClinicksApi.Business.DTOs;
using ClinicksApi.Data.Entities;

namespace ClinicksApi.Services.Interfaces
{
    public interface IConsultaService
    {
        Task<List<ConsultaMedica>> ObtenerListaConsultas();
        Task<List<ConsultaMedica>> ObtenerHistorialPaciente(int pacienteId);
        Task<(bool Success, string Message, ConsultaMedica? Data)> RegistrarConsulta(ConsultaAltaDto dto, int idMedicoLogueado);
    }
}