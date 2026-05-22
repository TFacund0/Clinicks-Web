using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClinicksApi.Business.DTOs
{
    /// <summary>
    /// Objeto de Transferencia de Datos (DTO) utilizado para RECIBIR las credenciales del Frontend.
    /// Representa el formulario de inicio de sesión que completa el usuario en React.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// El nombre de usuario, correo o matrícula ingresada por el médico.
        /// El atributo <see cref="JsonPropertyNameAttribute"/> permite que React envíe "username"
        /// en minúscula y C# lo mapee correctamente sin distinción de mayúsculas.
        /// </summary>
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [JsonPropertyName("username")]
        public string Username { get; set; } = null!;

        /// <summary>
        /// La contraseña en texto plano ingresada por el usuario.
        /// Se verifica contra el hash BCrypt almacenado en la base de datos; nunca se guarda en texto plano.
        /// </summary>
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;
    }
}