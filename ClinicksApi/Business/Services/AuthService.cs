using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;

namespace ClinicksApi.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<Medico?> AuthenticateAsync(string username, string password)
        {
            // Aquí podrías agregar lógica extra en el futuro:
            // 1. Encriptar la password antes de mandarla al repo.
            // 2. Verificar si el usuario no está bloqueado.

            return await _authRepository.LoginAsync(username, password);
        }
    }
}