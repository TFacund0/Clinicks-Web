namespace ClinicksApi.Business.Dtos
{
    public class LoginResponseDto
    {
        public int IdMedico { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Matricula { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
