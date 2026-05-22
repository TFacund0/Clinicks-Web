namespace ClinicksApi.Business.DTOs
{
    /// <summary>
    /// Objeto de Transferencia de Datos (DTO) para ENVIAR el historial de consultas al Frontend.
    /// Aísla la base de datos (ConsultaMedica) de la web, previniendo Entity Exposure y bucles JSON.
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
        /// Nombre formateado del médico que atendió la consulta.
        /// Abstrae la relación con la tabla Medico.
        /// </summary>
        public string MedicoAtencion { get; set; } = string.Empty;
    }
}
