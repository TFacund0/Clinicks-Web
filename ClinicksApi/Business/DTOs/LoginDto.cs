using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClinicksApi.Business.DTOs
{
    /// <summary>
    /// DTO para las credenciales de inicio de sesión.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// El nombre de usuario, correo o matrícula.
        /// </summary>
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [JsonPropertyName("username")]
        public string Username { get; set; } = null!;

        /// <summary>
        /// La contraseña.
        /// </summary>
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;
    }
}