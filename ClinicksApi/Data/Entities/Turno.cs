using System;
using System.Collections.Generic;
using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Entities;

public partial class Turno
{
    public int IdTurno { get; set; }

    public DateTime FechaTurno { get; set; }

    public DateTime? FechaRegistracion { get; set; }

    public string? Motivo { get; set; }

    public int IdPaciente { get; set; }

    public int IdMedico { get; set; }

    public int IdEstadoTurno { get; set; }

    public int? IdProcedimiento { get; set; }

    public int? IdConsulta { get; set; }

    public virtual ConsultaMedica? IdConsultaNavigation { get; set; }

    public virtual EstadoTurno IdEstadoTurnoNavigation { get; set; } = null!;

    public virtual Medico IdMedicoNavigation { get; set; } = null!;

    public virtual Paciente IdPacienteNavigation { get; set; } = null!;

    public virtual Procedimiento? IdProcedimientoNavigation { get; set; }
}
