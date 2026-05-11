namespace ClinicksApi.Business.DTOs
{
    /// <summary>
    /// Objeto de Transferencia de Datos (DTO) utilizado para ENVIAR información al Frontend
    /// tras un inicio de sesión exitoso. Contiene la información pública del médico y su Token JWT.
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>Identificador único del médico autenticado en la base de datos.</summary>
        public int IdMedico { get; set; }

        /// <summary>Nombre de pila del médico, para personalizar el saludo en el Dashboard.</summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>Apellido del médico.</summary>
        public string Apellido { get; set; } = string.Empty;

        /// <summary>Matrícula médica única (ej: MN-12345). Se usa como identificador de sesión.</summary>
        public string Matricula { get; set; } = string.Empty;

        /// <summary>
        /// El Token JWT generado por el backend. React deberá guardarlo en el LocalStorage
        /// y enviarlo en el header <c>Authorization: Bearer [token]</c> en todas las peticiones futuras.
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
