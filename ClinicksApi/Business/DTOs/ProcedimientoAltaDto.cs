using System.ComponentModel.DataAnnotations;

namespace ClinicksApi.Business.DTOs
{
    /// <summary>
    /// DTO para el alta de un nuevo procedimiento médico.
    /// </summary>
    public class ProcedimientoAltaDto
    {
        /// <summary>DNI del paciente.</summary>
        [Required(ErrorMessage = "El DNI del paciente es obligatorio")]
        public string dnipaciente { get; set; } = null!;

        /// <summary>
        /// Categoría del procedimiento (ej: "Cirugía Menor", "Análisis de Laboratorio").
        /// </summary>
        [Required(ErrorMessage = "El tipo de proceso es obligatorio")]
        public string tipoproceso { get; set; } = null!;

        /// <summary>Descripción del procedimiento.</summary>
        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string descripcion { get; set; } = null!;

        /// <summary>
        /// Fecha del procedimiento.
        /// </summary>
        public DateTime? fechaproceso { get; set; }

        /// <summary>Resultado del procedimiento.</summary>
        public string? resultado { get; set; }

        /// <summary>ID del turno asociado.</summary>
        public int? idTurno { get; set; }
    }
}
