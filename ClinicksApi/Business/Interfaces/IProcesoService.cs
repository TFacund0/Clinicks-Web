using System.Threading.Tasks;
using ClinicksApi.Business.DTOs;
using ClinicksApi.Data.Entities;

namespace ClinicksApi.Business.Interfaces
{
    public interface IProcesoService
    {
        Task<(bool Success, string Message, Procedimiento? Data)> RegistrarProceso(ProcesoAltaDto dto, int idMedicoLogueado);
    }
}
