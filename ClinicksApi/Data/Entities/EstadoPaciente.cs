using System;
using System.Collections.Generic;

namespace ClinicksApi.Data.Entities;

public partial class EstadoPaciente
{
    public int IdEstadoPaciente { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
}
