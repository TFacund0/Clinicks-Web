namespace ClinicksApi.Business.DTOs;

/// <summary>
/// Objeto de Transferencia de Datos (DTO) para enviar la información de los pacientes a React.
/// Omite datos confidenciales de la entidad base y agrega propiedades calculadas al vuelo como la Edad.
/// </summary>
public class PacienteDto
{
    /// <summary>Identificador único del paciente en la base de datos.</summary>
    public int Id { get; set; }

    /// <summary>Nombre y apellido concatenados. Calculado en el Servicio al mapear la entidad.</summary>
    public string NombreCompleto { get; set; } = string.Empty;

    /// <summary>Documento Nacional de Identidad del paciente.</summary>
    public string Dni { get; set; } = string.Empty;

    /// <summary>
    /// Edad en años completos. Calculada correctamente comparando mes y día del cumpleaños
    /// con la fecha actual para no adelantar un año antes del cumpleaños.
    /// </summary>
    public int Edad { get; set; }

    /// <summary>
    /// Fecha del turno más reciente del paciente, formateada como texto corto.
    /// Si no tiene turnos registrados, devuelve "Sin consultas".
    /// </summary>
    public string FechaUltimaConsulta { get; set; } = string.Empty;

    /// <summary>
    /// Verdadero si el estado del paciente en la base de datos es "activo".
    /// Se resuelve comparando el nombre del estado en minúsculas.
    /// </summary>
    public bool EstaActivo { get; set; }
}