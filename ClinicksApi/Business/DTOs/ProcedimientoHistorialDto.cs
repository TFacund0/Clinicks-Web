namespace ClinicksApi.Business.DTOs
{
    /// <summary>
    /// DTO para el historial de procedimientos.
    /// </summary>
    public class ProcedimientoHistorialDto
    {
        public int IdProcedimiento { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Resultado { get; set; } = string.Empty;
        
        /// <summary>
        /// Nombre completo del médico.
        /// </summary>
        public string MedicoAtencion { get; set; } = string.Empty;
    }
}
