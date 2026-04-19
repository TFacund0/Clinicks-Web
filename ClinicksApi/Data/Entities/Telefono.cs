using System;
using System.Collections.Generic;

namespace ClinicksApi.Data.Entities;

public partial class Telefono
{
    public int IdTelefono { get; set; }

    public string NumeroTelefono { get; set; } = null!;

    public int IdPaciente { get; set; }

    public virtual Paciente IdPacienteNavigation { get; set; } = null!;
}
