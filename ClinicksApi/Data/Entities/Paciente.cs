using System;
using System.Collections.Generic;

namespace ClinicksApi.Data.Entities;

public partial class Paciente
{
    public int IdPaciente { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string Dni { get; set; } = null!;

    public string? Correo { get; set; }

    public string? Observaciones { get; set; }

    public DateTime? FechaRegistracion { get; set; }

    public int IdEstadoPaciente { get; set; }

    public int IdDireccion { get; set; }

    public virtual ICollection<ConsultaMedica> ConsultaMedicas { get; set; } = new List<ConsultaMedica>();

    public virtual Direccion IdDireccionNavigation { get; set; } = null!;

    public virtual EstadoPaciente IdEstadoPacienteNavigation { get; set; } = null!;

    public virtual ICollection<Internacion> Internacions { get; set; } = new List<Internacion>();

    public virtual ICollection<Telefono> Telefonos { get; set; } = new List<Telefono>();

    public virtual ICollection<Turno> Turnos { get; set; } = new List<Turno>();
}
