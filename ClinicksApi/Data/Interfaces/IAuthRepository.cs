using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Interfaces
{
    public interface IAuthRepository
    {
        // Devuelve el objeto Medico si las credenciales son correctas
        Task<Medico?> LoginAsync(string username, string password);
    }
}