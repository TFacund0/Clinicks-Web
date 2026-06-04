using ClinicksApi.Business.DTOs;
using ClinicksApi.Data.Entities;

namespace ClinicksApi.Business.Interfaces
{
    /// <summary>
    /// Contrato (interfaz) que define las operaciones de negocio disponibles para los procedimientos médicos.
    /// La implementación concreta es <see cref="ClinicksApi.Business.Services.ProcesoService"/>.
    /// </summary>
    public interface IProcesoService
    {
        /// <summary>
        /// Aplica las reglas de negocio y persiste un nuevo procedimiento médico en la base de datos,
        /// creando además el Turno de vinculación entre el procedimiento, el paciente y el médico.
        /// </summary>
        /// <param name="dto">DTO con los datos del formulario (tipo de proceso, descripción, DNI del paciente, etc.).</param>
        /// <param name="idMedicoLogueado">ID del médico autenticado, extraído del Token JWT en el Controlador.</param>
        /// <returns>
        /// Una tupla con tres valores:
        /// <c>Success</c> (bool): indica si la operación fue exitosa.
        /// <c>Message</c> (string): mensaje descriptivo del resultado.
        /// <c>Data</c> (<see cref="Procedimiento"/>?): la entidad guardada, o null si hubo un error.
        /// </returns>
        Task<(bool Success, string Message, Procedimiento? Data)> RegistrarProcedimiento(ProcedimientoAltaDto procedimiento, int idMedicoLogueado);

        /// <summary>
        /// Obtiene la lista estática o catálogo de tipos de procedimientos médicos permitidos en el sistema.
        /// Este catálogo se almacena centralizadamente en la capa de negocio respetando SoC.
        /// </summary>
        /// <returns>Colección de tipos de procedimientos con su ID y Nombre descriptivo.</returns>
        IEnumerable<object> ObtenerTiposProceso();

        /// <summary>
        /// Obtiene el historial clínico completo de procedimientos de un paciente ordenado por fecha descendente.
        /// </summary>
        /// <param name="pacienteId">El ID numérico del paciente.</param>
        /// <returns>Una lista de DTOs con el historial de procedimientos del paciente, o vacía si no tiene.</returns>
        Task<IEnumerable<ProcesoHistorialDto>> ObtenerHistorialPaciente(int pacienteId);
    }
}
