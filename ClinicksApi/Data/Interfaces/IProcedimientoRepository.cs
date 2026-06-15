using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Interfaces
{
    /// <summary>
    /// Contrato (interfaz) que define las operaciones de acceso a datos para los procedimientos médicos.
    /// La implementación concreta es <see cref="ClinicksApi.Data.Repositories.ProcedimientoRepository"/>.
    /// </summary>
    public interface IProcedimientoRepository
    {
        /// <summary>
        /// Persiste un nuevo Procedimiento en la base de datos.
        /// </summary>
        /// <param name="procedimiento">La entidad Procedimiento a crear.</param>
        /// <returns>La entidad Procedimiento guardada con su ID asignado.</returns>
        Task<Procedimiento> RegistrarProcedimiento(Procedimiento procedimiento);
        /// <summary>
        /// Recupera el historial de procedimientos médicos de un paciente.
        /// </summary>
        Task<List<Procedimiento>> ObtenerHistorialProcedimientosAsync(int pacienteId);
    }
}
