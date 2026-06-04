using System.ComponentModel.DataAnnotations;

namespace ClinicksApi.Business.DTOs
{
    /// <summary>
    /// Objeto de Transferencia de Datos (DTO) utilizado para RECIBIR los datos de un nuevo procedimiento médico.
    /// Las propiedades marcadas con <see cref="RequiredAttribute"/> son validadas automáticamente por ASP.NET
    /// antes de llegar al Controlador, gracias al check de <c>ModelState.IsValid</c>.
    /// </summary>
    public class ProcedimientoAltaDto
    {
        /// <summary>DNI del paciente al que se le realiza el procedimiento. Campo obligatorio.</summary>
        [Required(ErrorMessage = "El DNI del paciente es obligatorio")]
        public string dnipaciente { get; set; } = null!;

        /// <summary>
        /// Categoría del procedimiento (ej: "Cirugía Menor", "Análisis de Laboratorio").
        /// Debe coincidir con alguna de las opciones devueltas por el endpoint GET /api/procesos/tipos.
        /// </summary>
        [Required(ErrorMessage = "El tipo de proceso es obligatorio")]
        public string tipoproceso { get; set; } = null!;

        /// <summary>Descripción detallada del procedimiento realizado. Campo obligatorio.</summary>
        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string descripcion { get; set; } = null!;

        /// <summary>
        /// Fecha en que se realizó el procedimiento. Opcional; si no se envía, se usa la fecha actual del servidor.
        /// </summary>
        public DateTime? fechaproceso { get; set; }

        /// <summary>Resultado o conclusión del procedimiento. Opcional; si se omite, se registra como "Sin resultado ingresado".</summary>
        public string? resultado { get; set; }

        /// <summary>ID opcional del turno que se está atendiendo para vincularlo a este procedimiento.</summary>
        public int? idturno { get; set; }
    }
}
