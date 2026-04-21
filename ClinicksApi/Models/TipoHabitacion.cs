using System;
using System.Collections.Generic;

namespace ClinicksApi.Models;

public partial class TipoHabitacion
{
    public int IdTipoHabitacion { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Habitacion> Habitacions { get; set; } = new List<Habitacion>();
}
