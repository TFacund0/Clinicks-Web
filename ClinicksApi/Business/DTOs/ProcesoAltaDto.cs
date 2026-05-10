using System;
using System.ComponentModel.DataAnnotations;

namespace ClinicksApi.Business.DTOs
{
    public class ProcesoAltaDto
    {
        [Required(ErrorMessage = "El DNI del paciente es obligatorio")]
        public string dnipaciente { get; set; } = null!;

        [Required(ErrorMessage = "El tipo de proceso es obligatorio")]
        public string tipoproceso { get; set; } = null!;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string descripcion { get; set; } = null!;

        public DateTime? fechaproceso { get; set; }

        public string? resultado { get; set; }
    }
}
