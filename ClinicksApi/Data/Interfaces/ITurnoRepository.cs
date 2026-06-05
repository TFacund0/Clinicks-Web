using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Interfaces
{
    /// <summary>
    /// Contrato exclusivo para el acceso a la tabla Turnos en PostgreSQL.
    /// </summary>
    public interface ITurnoRepository
    {
        /// <summary> Ejecuta un SELECT * FROM Turnos incluyendo las tablas relacionadas por defecto.</summary>
        Task<IEnumerable<Turno>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene los turnos de un médico específico, con filtros opcionales por rango de fechas.
        /// </summary>
        /// <param name="idMedico">El ID del médico.</param>
        /// <param name="fechaInicio">La fecha de inicio opcional para el filtro.</param>
        /// <param name="fechaFin">La fecha de fin opcional para el filtro.</param>
        /// <returns>Una lista de turnos correspondientes al médico y período especificados.</returns>
        Task<IEnumerable<Turno>> ObtenerTurnosMedicoAsync(int idMedico, DateTime? fechaInicio, DateTime? fechaFin);

        /// <summary>Obtiene un turno por su ID.</summary>
        Task<Turno?> ObtenerPorIdAsync(int idTurno);

        /// <summary>Obtiene un turno por su ID sin relaciones de navegacion para evitar conflictos al actualizar.</summary>
        Task<Turno?> ObtenerParaActualizarAsync(int idTurno);

        /// <summary>
        /// Crea y guarda un nuevo turno en la base de datos.
        /// </summary>
        /// <param name="turno">La entidad turno a persistir.</param>
        Task CrearTurnoAsync(Turno turno);

        /// <summary>
        /// Actualiza un turno existente en la base de datos.
        /// </summary>
        /// <param name="turno">La entidad turno a actualizar.</param>
        Task ActualizarTurnoAsync(Turno turno);

        /// <summary>
        /// Obtiene dinámicamente el ID de un estado a partir de su nombre.
        /// </summary>
        Task<int?> ObtenerIdEstadoPorNombreAsync(string nombre);

        /// <summary>
        /// Obtiene dinámicamente los IDs de varios estados a partir de sus nombres.
        /// </summary>
        Task<List<int>> ObtenerIdsEstadosPorNombresAsync(List<string> nombres);

        /// <summary>
        /// Obtiene los turnos anteriores a una fecha límite que tengan alguno de los IDs de estado proporcionados.
        /// </summary>
        Task<List<Turno>> ObtenerTurnosPorFechaYEstadosAsync(DateTime fechaLimite, List<int> estadosIds);

        /// <summary>
        /// Actualiza un lote de turnos y guarda los cambios en la base de datos.
        /// </summary>
        Task ActualizarLoteTurnosAsync(List<Turno> turnos);
    }
}