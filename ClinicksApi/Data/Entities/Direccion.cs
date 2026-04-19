using System;
using System.Collections.Generic;

namespace ClinicksApi.Data.Entities;

public partial class Direccion
{
    public int IdDireccion { get; set; }

    public string NombreCalle { get; set; } = null!;

    public int? NumeroCalle { get; set; }

    public int IdCodigoPostal { get; set; }

    public virtual CodigoPostal IdCodigoPostalNavigation { get; set; } = null!;

    public virtual ICollection<Medico> Medicos { get; set; } = new List<Medico>();

    public virtual ICollection<Paciente> Pacientes { get; set; } = new List<Paciente>();
}
