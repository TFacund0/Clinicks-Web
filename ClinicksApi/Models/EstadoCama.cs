using System;
using System.Collections.Generic;

namespace ClinicksApi.Models;

public partial class EstadoCama
{
    public int IdEstadoCama { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Cama> Camas { get; set; } = new List<Cama>();
}
