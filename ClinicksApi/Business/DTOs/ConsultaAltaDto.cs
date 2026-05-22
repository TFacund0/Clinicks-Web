using System.ComponentModel.DataAnnotations;

namespace ClinicksApi.Business.DTOs
{
    /// <summary>
    /// Objeto de Transferencia de Datos (DTO) utilizado para RECIBIR los datos de una nueva consulta médica.
    /// Los campos con <c>null!</c> son obligatorios; los <c>?</c> son opcionales y tienen valores por defecto en el Servicio.
    /// </summary>
    public class ConsultaAltaDto
    {
        /// <summary>Motivo principal por el que el paciente asiste a la consulta. Campo obligatorio.</summary>
        [Required(ErrorMessage = "El Motivo es obligatorio.")]
        public string motivo { get; set; } = null!;

        /// <summary>Diagnóstico del médico al finalizar la consulta. Campo obligatorio.</summary>
        [Required(ErrorMessage = "El Diagnóstico es obligatorio.")]
        public string diagnostico { get; set; } = null!;

        /// <summary>Plan de tratamiento indicado. Opcional; si se omite, se registra como "sin definir".</summary>
        public string? tratamiento { get; set; }

        /// <summary>Observaciones clínicas adicionales. Opcional; si se omite, se registra como "sin observaciones relevantes".</summary>
        public string? observaciones { get; set; }

        /// <summary>Recomendaciones para el paciente. Opcional; si se omite, se registra como "sin recomendaciones".</summary>
        public string? recomendacion { get; set; }

        /// <summary>
        /// Fecha de la consulta. Opcional; si no se envía, se usa la fecha y hora actuales del servidor.
        /// No puede ser una fecha futura; esta validación se aplica en el Servicio.
        /// </summary>
        public DateTime? fechaconsulta { get; set; }

        /// <summary>DNI del paciente a registrar en la consulta. Se usa para buscarlo en la BD y obtener su ID real.</summary>
        [Required(ErrorMessage = "El DNI del paciente es obligatorio.")]
        public required string dnipaciente { get; set; }
    }
}