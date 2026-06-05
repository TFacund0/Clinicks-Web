namespace ClinicksApi.Business.DTOs
{
    /// <summary>
    /// DTO para el historial de consultas médicas.
    /// </summary>
    public class ConsultaHistorialDto
    {
        public int IdConsulta { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string Diagnostico { get; set; } = string.Empty;
        public string Tratamiento { get; set; } = string.Empty;
        public string Observacion { get; set; } = string.Empty;
        public string Recomendacion { get; set; } = string.Empty;
        public DateTime? FechaConsulta { get; set; }
        
        /// <summary>
        /// Nombre completo del médico que atendió la consulta.
        /// </summary>
        public string MedicoAtencion { get; set; } = string.Empty;
    }
}
