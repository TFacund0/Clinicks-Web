using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Interfaces
{
    public interface IConsultaRepository
    {
        Task<List<ConsultaMedica>> ListaConsultas();
        Task<List<ConsultaMedica>> HistorialPaciente(int pacienteId);
        Task<ConsultaMedica> CrearConsulta(ConsultaMedica consulta);
    }
}