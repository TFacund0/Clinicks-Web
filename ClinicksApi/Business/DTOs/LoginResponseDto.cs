namespace ClinicksApi.Business.DTOs
{
    /// <summary>
    /// DTO para la respuesta del inicio de sesión.
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
        /// El Token JWT generado.
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
