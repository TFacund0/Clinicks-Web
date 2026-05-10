using ClinicksApi.Data.Entities;
using ClinicksApi.Business.Dtos;

namespace ClinicksApi.Business.Interfaces
{
    /// <summary>
    /// Contrato que define las operaciones de seguridad, autenticación y manejo de tokens.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Verifica las credenciales de un usuario y, si son válidas, emite un Token JWT.
        /// </summary>
        /// <param name="username">El nombre de usuario (ej. Matrícula).</param>
        /// <param name="password">La contraseña en texto plano para verificar.</param>
        Task<LoginResponseDto?> AuthenticateAsync(string username, string password);
        
        /// <summary>
        /// Encripta todas las contraseñas en la base de datos que aún se encuentren en texto plano.
        /// </summary>
        Task<int> HashExistingPasswordsAsync();
    }
}