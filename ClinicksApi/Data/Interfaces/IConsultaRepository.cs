using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Interfaces
{
    /// <summary>
    /// Contrato (interfaz) que define las operaciones de acceso a datos para las consultas médicas.
    /// La implementación concreta es <see cref="ClinicksApi.Data.Repositories.ConsultaRepository"/>.
    /// Separa la lógica de negocio del acceso directo a la base de datos (Principio de Responsabilidad Única).
    /// </summary>
    public interface IConsultaRepository
    {
        /// <summary>
        /// Recupera todas las consultas médicas almacenadas en la base de datos.
        /// </summary>
        /// <returns>Una lista de entidades <see cref="ConsultaMedica"/>.</returns>
        Task<List<ConsultaMedica>> ListaConsultas();

        /// <summary>
        /// Recupera todas las consultas médicas realizadas a un paciente, ordenadas de la más reciente a la más antigua.
        /// Incluye los datos del médico que atendió cada consulta (JOIN con tabla medico).
        /// </summary>
        /// <param name="pacienteId">El identificador único del paciente en la base de datos.</param>
        /// <returns>Lista de consultas del paciente con datos del médico incluidos.</returns>
        Task<List<ConsultaMedica>> HistorialPaciente(int pacienteId);

        /// <summary>
        /// Persiste una nueva consulta médica en la base de datos ejecutando un INSERT.
        /// </summary>
        /// <param name="consulta">La entidad <see cref="ConsultaMedica"/> ya construida y validada por el Servicio.</param>
        /// <returns>La entidad guardada con el ID asignado por la base de datos.</returns>
        Task<ConsultaMedica> RegistrarConsulta(ConsultaMedica consulta);

        /// <summary>
        /// Garantiza que un estado de turno con el nombre indicado exista en la base de datos y retorna su ID.
        /// </summary>
        /// <param name="nombreEstado">El nombre descriptivo del estado (ej: "Atendido").</param>
        /// <returns>El ID del estado de turno (existente o recién creado).</returns>
        Task<int> AsegurarEstadoTurnoExiste(string nombreEstado);

        /// <summary>
        /// Persiste un Turno en la base de datos.
        /// </summary>
        /// <param name="turno">El Turno a guardar.</param>
        Task CrearTurnoVinculado(Turno turno);

        /// <summary>
        /// Asocia una consulta a un Turno existente y lo marca como Realizado.
        /// </summary>
        /// <param name="idTurno">El identificador del turno a actualizar.</param>
        /// <param name="idConsulta">El identificador de la consulta a asociar.</param>
        Task ActualizarTurnoVinculado(int idTurno, int idConsulta);
    }
}