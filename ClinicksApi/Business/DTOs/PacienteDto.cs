namespace ClinicksApi.Business.Dtos;

/// <summary>
/// Objeto de Transferencia de Datos (DTO) para enviar la información de los pacientes a React.
/// Observa que NO es igual a la tabla de la base de datos: omite datos confidenciales y agrega 
/// propiedades calculadas al vuelo como la Edad.
/// </summary>
public class PacienteDto
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    
    /// <summary>
    /// Propiedad calculada al vuelo en el Servicio restando el año de nacimiento al año actual.
    /// </summary>
    public int Edad { get; set; }
    
    /// <summary>
    /// Propiedad calculada buscando el turno más reciente del paciente usando LINQ.
    /// </summary>
    public string FechaUltimaConsulta { get; set; } = string.Empty;
    
    /// <summary>
    /// Verdadero si el estado del paciente en base de datos es "activo".
    /// </summary>
    public bool EstaActivo { get; set; }
}