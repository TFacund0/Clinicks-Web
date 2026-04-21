using System;
using System.Collections.Generic;

namespace ClinicksApi.Models;

public partial class Medicacion
{
    public int IdMedicacion { get; set; }

    public string NombreMedicamento { get; set; } = null!;

    public string Dosis { get; set; } = null!;

    public string Frecuencia { get; set; } = null!;

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public int IdInternacion { get; set; }

    public virtual Internacion IdInternacionNavigation { get; set; } = null!;
}
