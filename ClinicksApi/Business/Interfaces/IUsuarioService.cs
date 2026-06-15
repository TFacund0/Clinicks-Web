using ClinicksApi.Business.DTOs;

namespace ClinicksApi.Business.Interfaces
{
    /// <summary>
    /// Contrato que define las operaciones de seguridad, autenticación y manejo de tokens para los Usuarios.
    /// </summary>
    public interface IUsuarioService
    {
        /// <summary>
        /// Verifica las credenciales de un usuario y, si son válidas, emite un Token JWT.
        /// </summary>
        /// <param name="username">El nombre de usuario o matrícula.</param>
        /// <param name="password">La contraseña en texto plano para verificar.</param>
        Task<LoginResponseDto?> AutenticarAsync(string username, string password);

        /// <summary>
        /// Escanea todos los usuarios y encripta con BCrypt las contraseñas en texto plano.
        /// </summary>
        Task<int> EncriptarClavesExistentesAsync();
    }
}
