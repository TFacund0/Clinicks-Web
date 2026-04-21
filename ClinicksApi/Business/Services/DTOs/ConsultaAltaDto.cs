
namespace ClinicksApi.Business.Services.DTOs
{
    public class ConsultaAltaDto
    {
        public string motivo { get; set; } = null!;
        public string diagnostico { get; set; } = null!;
        public string? tratamiento { get; set; }
        public string? observaciones { get; set; }
        public string? recomendacion { get; set; }
        public DateTime? fechaconsulta { get; set; }
        public required string dnipaciente { get; set; }
        
    }
}