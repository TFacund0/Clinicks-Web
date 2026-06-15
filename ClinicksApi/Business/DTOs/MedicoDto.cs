namespace ClinicksApi.Business.DTOs
{
    /// <summary>
    /// DTO para transferir información detallada del médico.
    /// </summary>
    public class MedicoDto
    {
        public int IdMedico { get; set; }
        public string Matricula { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Correo { get; set; }
        public string Dni { get; set; } = string.Empty;
        public string Especialidad { get; set; } = string.Empty;
        public int? IdUsuario { get; set; }
    }
}
