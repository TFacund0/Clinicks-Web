using System;
using System.Collections.Generic;

namespace ClinicksApi.Data.Entities;

public partial class Procedimiento
{
    public int IdProcedimiento { get; set; }

    public string Tipo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime Fecha { get; set; }

    public string? Resultado { get; set; }

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
