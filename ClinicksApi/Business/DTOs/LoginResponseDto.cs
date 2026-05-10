namespace ClinicksApi.Business.Dtos
{
    /// <summary>
    /// Objeto de Transferencia de Datos (DTO) utilizado para ENVIAR información al Frontend
    /// tras un inicio de sesión exitoso. Contiene la información pública del médico y su Gafete (Token).
    /// </summary>
    public class LoginResponseDto
    {
        public int IdMedico { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Matricula { get; set; } = string.Empty;

        /// <summary>
        /// El Token JWT (Gafete) generado por el backend. React deberá guardarlo en el LocalStorage
        /// y enviarlo en los Headers de sus futuras peticiones.
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
