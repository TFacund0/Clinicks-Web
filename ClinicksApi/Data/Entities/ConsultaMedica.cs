using System;
using System.Collections.Generic;

namespace ClinicksApi.Data.Entities;

public partial class ConsultaMedica
{
    public int IdConsulta { get; set; }

    public string Motivo { get; set; } = null!;

    public string Diagnostico { get; set; } = null!;

    public string? Recomendacion { get; set; }

    public DateTime? FechaConsulta { get; set; }

    public string? Observacion { get; set; }

    public string? Tratamiento { get; set; }

    public int IdPaciente { get; set; }

    public int IdMedico { get; set; }

    public virtual Medico IdMedicoNavigation { get; set; } = null!;

    public virtual Paciente IdPacienteNavigation { get; set; } = null!;

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
