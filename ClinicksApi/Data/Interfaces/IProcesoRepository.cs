using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Interfaces
{
    /// <summary>
    /// Contrato (interfaz) que define las operaciones de acceso a datos para los procedimientos médicos.
    /// La implementación concreta es <see cref="ClinicksApi.Data.Repositories.ProcesoRepository"/>.
    /// </summary>
    public interface IProcesoRepository
    {
        /// <summary>
        /// Persiste de forma atómica (usando una transacción de base de datos) un nuevo Procedimiento
        /// y su Turno asociado, garantizando la consistencia e integridad de datos (Unit of Work).
        /// </summary>
        /// <param name="procedimiento">La entidad Procedimiento a crear.</param>
        /// <param name="turno">La entidad Turno a crear vinculada al procedimiento.</param>
        /// <returns>La entidad Procedimiento guardada con su ID asignado.</returns>
        Task<Procedimiento> CrearProcedimientoYTurnoVinculado(Procedimiento procedimiento, Turno turno);

        /// <summary>
        /// Persiste un nuevo Procedimiento y lo vincula a un Turno existente de forma atómica y transaccional.
        /// </summary>
        Task<Procedimiento> CrearProcedimientoYVincularATurnoExistente(Procedimiento procedimiento, int idTurno);
    }
}
