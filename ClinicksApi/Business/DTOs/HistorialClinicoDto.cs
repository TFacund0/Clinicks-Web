using System.Collections.Generic;

namespace ClinicksApi.Business.DTOs
{
    /// <summary>
    /// DTO del historial clínico consolidado.
    /// </summary>
    public class HistorialClinicoDto
    {
        /// <summary>Datos personales del paciente.</summary>
        public PacienteDto Paciente { get; set; } = null!;

        /// <summary>Historial de consultas médicas del paciente.</summary>
        public List<ConsultaHistorialDto> Consultas { get; set; } = new();

        /// <summary>Historial de procedimientos médicos del paciente.</summary>
        public List<ProcedimientoHistorialDto> Procedimientos { get; set; } = new();
    }
}
