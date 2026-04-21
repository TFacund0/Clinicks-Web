using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Interfaces
{
    public interface IPacienteRepository
    {
            Task<Paciente?> BuscarPorDni(string dni);
    }
}
