namespace ClinicksApi.Business.DTOs
{
    /// <summary>
    /// Objeto de Transferencia de Datos (DTO) para ENVIAR el historial de procedimientos al Frontend.
    /// </summary>
    public class ProcesoHistorialDto
    {
        public int IdProcedimiento { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Resultado { get; set; } = string.Empty;
        
        /// <summary>
        /// Nombre formateado del médico que realizó el procedimiento.
        /// Abstrae la relación con la tabla Medico extraída a través de Turno.
        /// </summary>
        public string MedicoAtencion { get; set; } = string.Empty;
    }
}
