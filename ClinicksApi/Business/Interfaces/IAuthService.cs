using ClinicksApi.Data.Entities;

namespace ClinicksApi.Business.Interfaces
{
    public interface IAuthService
    {
        // El servicio devuelve la entidad pero podría devolver un "LoginResponseDTO"
        Task<Medico?> AuthenticateAsync(string username, string password);
    }
}