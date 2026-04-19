using System;
using System.Collections.Generic;

namespace ClinicksApi.Data.Entities;

public partial class CodigoPostal
{
    public int IdCodigoPostal { get; set; }

    public string Codigo { get; set; } = null!;

    public int IdCiudad { get; set; }

    public virtual ICollection<Direccion> Direccions { get; set; } = new List<Direccion>();

    public virtual Ciudad IdCiudadNavigation { get; set; } = null!;
}
