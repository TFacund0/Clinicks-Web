using System;
using System.Collections.Generic;

namespace ClinicksApi.Models;

public partial class EstadoTurno
{
    public int IdEstadoTurno { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
