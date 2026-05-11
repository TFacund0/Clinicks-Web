using System;
using System.Collections.Generic;

namespace ClinicksApi.Data.Entities;

public partial class Pai
{
    public int IdPais { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Ciudad> Ciudads { get; set; } = new List<Ciudad>();
}
