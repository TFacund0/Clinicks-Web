using System;
using System.Collections.Generic;

namespace ClinicksApi.Data.Entities;

public partial class Piso
{
    public int IdPiso { get; set; }

    public string NroPiso { get; set; } = null!;

    public virtual ICollection<Habitacion> Habitacions { get; set; } = new List<Habitacion>();
}
