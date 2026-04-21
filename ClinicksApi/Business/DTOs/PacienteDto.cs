namespace ClinicksApi.Business.Dtos;

public class PacienteDto
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public int Edad { get; set; }
    public string FechaUltimaConsulta { get; set; } = string.Empty;
    public bool EstaActivo { get; set; }
}