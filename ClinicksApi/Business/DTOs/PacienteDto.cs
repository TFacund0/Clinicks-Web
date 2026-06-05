namespace ClinicksApi.Business.DTOs;

/// <summary>
/// DTO de paciente.
/// </summary>
public class PacienteDto
{
    /// <summary>Identificador único del paciente en la base de datos.</summary>
    public int Id { get; set; }

    /// <summary>Nombre y apellido concatenados.</summary>
    public string NombreCompleto { get; set; } = string.Empty;

    /// <summary>Documento Nacional de Identidad del paciente.</summary>
    public string Dni { get; set; } = string.Empty;

    /// <summary>
    /// Edad en años completos.
    /// </summary>
    public int Edad { get; set; }

    /// <summary>
    /// Fecha del turno más reciente del paciente, formateada como texto corto.
    /// Si no tiene turnos registrados, devuelve "Sin consultas".
    /// </summary>
    public string FechaUltimaConsulta { get; set; } = string.Empty;

    /// <summary>
    /// Verdadero si el paciente está activo.
    /// </summary>
    public bool EstaActivo { get; set; }
}