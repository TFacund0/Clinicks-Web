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
        /// Persiste una nueva consulta médica en la base de datos ejecutando un INSERT.
        /// </summary>
        /// <param name="consulta">La entidad <see cref="ConsultaMedica"/> ya construida y validada por el Servicio.</param>
        /// <returns>La entidad guardada con el ID asignado por la base de datos.</returns>
        Task<ConsultaMedica> CrearConsulta(ConsultaMedica consulta);

        /// <summary>
        /// Persiste un Turno en la base de datos.
        /// </summary>
        /// <param name="turno">El Turno a guardar.</param>
        Task CrearTurnoVinculado(Turno turno);

        /// <summary>
        /// Persiste una nueva consulta médica y crea su Turno vinculado de forma atómica y transaccional.
        /// </summary>
        Task<ConsultaMedica> CrearConsultaYTurnoVinculado(ConsultaMedica consulta, Turno turno);

        /// <summary>
        /// Persiste una nueva consulta médica y la vincula a un Turno existente de forma atómica y transaccional.
        /// </summary>
        Task<ConsultaMedica> CrearConsultaYVincularATurnoExistente(ConsultaMedica consulta, int idTurno);
    }
}