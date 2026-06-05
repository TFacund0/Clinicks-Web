using System.ComponentModel.DataAnnotations;

namespace ClinicksApi.Business.DTOs
{
    /// <summary>
    /// DTO para el alta de consultas médicas.
    /// </summary>
    public class ConsultaAltaDto
    {
        /// <summary>Motivo de la consulta.</summary>
        [Required(ErrorMessage = "El Motivo es obligatorio.")]
        public string motivo { get; set; } = null!;

        /// <summary>Diagnóstico.</summary>
        [Required(ErrorMessage = "El Diagnóstico es obligatorio.")]
        public string diagnostico { get; set; } = null!;

        /// <summary>Plan de tratamiento.</summary>
        public string? tratamiento { get; set; }

        /// <summary>Observaciones adicionales.</summary>
        public string? observaciones { get; set; }

        /// <summary>Recomendaciones.</summary>
        public string? recomendacion { get; set; }

        /// <summary>
        /// Fecha de la consulta.
        /// </summary>
        public DateTime? fechaconsulta { get; set; }

        /// <summary>DNI del paciente.</summary>
        [Required(ErrorMessage = "El DNI del paciente es obligatorio.")]
        public required string dnipaciente { get; set; }

        /// <summary>ID del turno asociado.</summary>
        public int? idTurno { get; set; }
    }
}