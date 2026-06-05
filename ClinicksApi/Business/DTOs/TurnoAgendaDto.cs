namespace ClinicksApi.Business.DTOs
{
    /// <summary>
    /// DTO para los turnos agendados.
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