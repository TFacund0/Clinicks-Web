using System.Threading.Tasks;
using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Interfaces
{
    public interface IProcesoRepository
    {
        Task<Procedimiento> CrearProcedimiento(Procedimiento procedimiento);
        Task<Turno> CrearTurnoVinculado(Turno turno);
        Task AsegurarEstadoTurnoExiste(int idEstadoTurno, string nombreEstado);
    }
}
