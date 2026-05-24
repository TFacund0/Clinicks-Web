namespace ClinicksApi.Business.DTOs
{
    /// <summary>
    /// Objeto de Transferencia de Datos (DTO) para ENVIAR la información de los turnos agendados al Frontend.
    /// </summary>
    public class TurnoAgendaDto
    {
        public int IdTurno { get; set; }
        public int IdPaciente { get; set; }
        public DateTime FechaTurno { get; set; }
        public string PacienteNombreCompleto { get; set; } = string.Empty;
        public string DniPaciente { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}