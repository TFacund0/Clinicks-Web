using System;
using System.Collections.Generic;

namespace ClinicksApi.Data.Entities;

public partial class Internacion
{
    public int IdInternacion { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public string Motivo { get; set; } = null!;

    public int IdPaciente { get; set; }

    public int IdMedico { get; set; }

    public int IdCama { get; set; }

    public virtual Cama IdCamaNavigation { get; set; } = null!;

    public virtual Medico IdMedicoNavigation { get; set; } = null!;

    public virtual Paciente IdPacienteNavigation { get; set; } = null!;

    public virtual ICollection<Medicacion> Medicacions { get; set; } = new List<Medicacion>();
}
