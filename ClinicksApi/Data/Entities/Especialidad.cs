using System;
using System.Collections.Generic;
using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Entities;

public partial class Especialidad
{
    public int IdEspecialidad { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Medico> Medicos { get; set; } = new List<Medico>();
}
