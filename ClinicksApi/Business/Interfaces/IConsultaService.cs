using ClinicksApi.Business.DTOs;
using ClinicksApi.Data.Entities;

namespace ClinicksApi.Business.Interfaces
{
    /// <summary>
    /// Contrato (interfaz) que define las operaciones de negocio disponibles para las consultas médicas.
    /// Cualquier clase que implemente esta interfaz debe respetar la firma de estos métodos,
    /// lo que permite intercambiar implementaciones sin modificar el Controlador (Principio de Inversión de Dependencias).
    /// </summary>
    public interface IConsultaService
    {
        /// <summary>
        /// Obtiene el listado completo de todas las consultas médicas registradas en el sistema.
        /// </summary>
        /// <returns>Una lista de DTOs <see cref="ConsultaHistorialDto"/>.</returns>
        Task<List<ConsultaHistorialDto>> ObtenerListaConsultas();

        /// <summary>
        /// Obtiene todas las consultas médicas realizadas a un paciente específico, ordenadas cronológicamente.
        /// </summary>
        /// <param name="pacienteId">El identificador único del paciente en la base de datos.</param>
        /// <returns>Una lista de DTOs <see cref="ConsultaHistorialDto"/> del paciente, o una lista vacía si no tiene historial.</returns>
        Task<List<ConsultaHistorialDto>> ObtenerHistorialPaciente(int pacienteId);

        /// <summary>
        /// Aplica las reglas de negocio, valida los datos y persiste una nueva consulta médica en la base de datos.
        /// </summary>
        /// <param name="dto">DTO con los datos del formulario enviados desde el frontend.</param>
        /// <param name="idMedicoLogueado">ID del médico autenticado, extraído del Token JWT en el Controlador.</param>
        /// <returns>
        /// Una tupla con tres valores:
        /// <c>Success</c> (bool): indica si la operación fue exitosa.
        /// <c>Message</c> (string): mensaje descriptivo del resultado.
        /// <c>Data</c> (<see cref="ConsultaMedica"/>?): la entidad guardada, o null si hubo un error.
        /// </returns>
        Task<(bool Success, string Message, ConsultaMedica? Data)> RegistrarConsulta(ConsultaAltaDto consulta, int idMedicoLogueado);
    }
}