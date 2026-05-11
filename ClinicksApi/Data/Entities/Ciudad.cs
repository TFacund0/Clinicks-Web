using System;
using System.Collections.Generic;

namespace ClinicksApi.Data.Entities;

public partial class Ciudad
{
    public int IdCiudad { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdPais { get; set; }

    public virtual ICollection<CodigoPostal> CodigoPostals { get; set; } = new List<CodigoPostal>();

    public virtual Pai IdPaisNavigation { get; set; } = null!;
}
