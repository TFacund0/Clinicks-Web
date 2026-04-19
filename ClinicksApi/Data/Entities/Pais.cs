using System;
using System.Collections.Generic;
using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Entities;

public partial class Pais
{
    public int IdPais { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Ciudad> Ciudads { get; set; } = new List<Ciudad>();
}
