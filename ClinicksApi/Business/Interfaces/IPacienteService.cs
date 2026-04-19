using ClinicksApi.Business.Dtos;
using ClinicksApi.Business.Services;
using System.Threading.Tasks;

namespace ClinicksApi.Business.Interfaces;

public interface IPacienteService
{
    Task<IEnumerable<PacienteDto>> ObtenerListado();
    
    Task<PacienteDto?> ObtenerPorId(int id);
}