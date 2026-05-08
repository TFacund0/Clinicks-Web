using ClinicksApi.Data.Entities;
using ClinicksApi.Business.Dtos;

namespace ClinicksApi.Business.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> AuthenticateAsync(string username, string password);
        
        // Método temporal para migración
        Task<int> HashExistingPasswordsAsync();
    }
}