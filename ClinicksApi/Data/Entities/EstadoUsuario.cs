using System;
using System.Collections.Generic;

namespace ClinicksApi.Data.Entities;

public partial class EstadoUsuario
{
    public int IdEstadoUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
