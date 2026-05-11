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
        Task<ConsultaMedica> CrearConsulta(ConsultaMedica consulta);
    }
}