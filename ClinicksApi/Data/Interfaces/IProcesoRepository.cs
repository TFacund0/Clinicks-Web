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
        /// Persiste un nuevo procedimiento médico en la tabla <c>procedimiento</c> de la base de datos.
        /// </summary>
        /// <param name="procedimiento">La entidad <see cref="Procedimiento"/> construida y validada por el Servicio.</param>
        /// <returns>La entidad guardada con el ID asignado por la base de datos.</returns>
        Task<Procedimiento> CrearProcedimiento(Procedimiento procedimiento);

        /// <summary>
        /// Persiste un Turno en la base de datos para vincular el procedimiento con el paciente y el médico.
        /// Es el registro que une las tres entidades en una sola transacción clínica.
        /// </summary>
        /// <param name="turno">La entidad <see cref="Turno"/> con los IDs del paciente, médico y procedimiento.</param>
        /// <returns>La entidad <see cref="Turno"/> guardada con el ID asignado por la base de datos.</returns>
        Task<Turno> CrearTurnoVinculado(Turno turno);

        /// <summary>
        /// Verifica si un estado de turno con el ID indicado existe en la base de datos y, si no, lo crea.
        /// Este método actúa como un mecanismo de inicialización para garantizar que los datos de referencia 
        /// (como el estado "Realizado") siempre estén presentes incluso en entornos recién creados.
        /// </summary>
        /// <param name="idEstadoTurno">El ID del estado de turno a verificar o crear.</param>
        /// <param name="nombreEstado">El nombre descriptivo del estado (ej: "Realizado").</param>
        Task AsegurarEstadoTurnoExiste(int idEstadoTurno, string nombreEstado);
    }
}
