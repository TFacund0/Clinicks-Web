using System;
using System.Collections.Generic;

namespace ClinicksApi.Data.Entities;

public partial class Cama
{
    public int IdCama { get; set; }

    public string CodigoCama { get; set; } = null!;

    public int IdEstadoCama { get; set; }

    public int IdHabitacion { get; set; }

    public virtual EstadoCama IdEstadoCamaNavigation { get; set; } = null!;

    public virtual Habitacion IdHabitacionNavigation { get; set; } = null!;

    public virtual ICollection<Internacion> Internacions { get; set; } = new List<Internacion>();
}
