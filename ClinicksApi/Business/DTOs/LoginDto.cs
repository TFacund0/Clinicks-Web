using System.Text.Json.Serialization;

namespace ClinicksApi.Business.Dtos
{
    /// <summary>
    /// Objeto de Transferencia de Datos (DTO) utilizado para RECIBIR información desde el Frontend.
    /// Representa el formulario de inicio de sesión que llena el usuario.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// El nombre de usuario, correo o matrícula. 
        /// El atributo JsonPropertyName asegura que si React envía "username" en minúscula, C# lo entienda.
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; } = null!;

        /// <summary>
        /// La contraseña ingresada por el usuario.
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;
    }
}