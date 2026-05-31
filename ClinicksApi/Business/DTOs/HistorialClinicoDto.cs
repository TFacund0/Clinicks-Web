using System.Collections.Generic;

namespace ClinicksApi.Business.DTOs
{
    /// <summary>
    /// Objeto de Transferencia de Datos (DTO) que consolida el expediente clínico de un paciente.
    /// Combina los datos personales del paciente con su historial completo de consultas y procedimientos.
    /// </summary>
    public class HistorialClinicoDto
    {
        /// <summary>Datos personales del paciente.</summary>
        public PacienteDto Paciente { get; set; } = null!;

        /// <summary>Historial de consultas médicas del paciente.</summary>
        public List<ConsultaHistorialDto> Consultas { get; set; } = new();

        /// <summary>Historial de procedimientos médicos del paciente.</summary>
        public List<ProcesoHistorialDto> Procedimientos { get; set; } = new();
    }
}
