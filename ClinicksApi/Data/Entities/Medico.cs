using System;
using System.Collections.Generic;

namespace ClinicksApi.Data.Entities;

public partial class Medico
{
    public int IdMedico { get; set; }

    public string Matricula { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? Correo { get; set; }

    public string Dni { get; set; } = null!;

    public int IdEspecialidad { get; set; }

    public int IdDireccion { get; set; }

    public int? IdUsuario { get; set; }

    public virtual ICollection<ConsultaMedica> ConsultaMedicas { get; set; } = new List<ConsultaMedica>();

    public virtual Direccion IdDireccionNavigation { get; set; } = null!;

    public virtual Especialidad IdEspecialidadNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<Internacion> Internacions { get; set; } = new List<Internacion>();

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
