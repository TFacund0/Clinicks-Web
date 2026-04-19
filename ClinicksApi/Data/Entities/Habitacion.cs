using System;
using System.Collections.Generic;

namespace ClinicksApi.Data.Entities;

public partial class Habitacion
{
    public int IdHabitacion { get; set; }

    public int NroHabitacion { get; set; }

    public int IdPiso { get; set; }

    public int IdTipoHabitacion { get; set; }

    public virtual ICollection<Cama> Camas { get; set; } = new List<Cama>();

    public virtual Piso IdPisoNavigation { get; set; } = null!;

    public virtual TipoHabitacion IdTipoHabitacionNavigation { get; set; } = null!;
}
