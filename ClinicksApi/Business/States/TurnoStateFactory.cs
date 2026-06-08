using ClinicksApi.Constants;

namespace ClinicksApi.Business.States
{
    /// <summary>
    /// Factoría encargada de instanciar la clase de estado correcta (TurnoState)
    /// basada en el identificador numérico de la base de datos (IdEstadoTurno).
    /// </summary>
    public static class TurnoStateFactory
    {
        /// <summary>
        /// Crea una instancia concreta de TurnoState según el id del estado del turno.
        /// </summary>
        /// <param name="idEstadoTurno">ID numérico asignado al estado del turno en la base de datos.</param>
        /// <returns>Una instancia de la subclase correspondiente de TurnoState.</returns>
        public static TurnoState CrearEstado(int idEstadoTurno)
        {
            return idEstadoTurno switch
            {
                ConstantesGenerales.EstadosTurno.PendienteId => new TurnoPendiente(),
                ConstantesGenerales.EstadosTurno.ConfirmadoId => new TurnoConfirmado(),
                ConstantesGenerales.EstadosTurno.EnCursoId => new TurnoEnCurso(),
                ConstantesGenerales.EstadosTurno.AtendidoId => new TurnoAtendido(),
                ConstantesGenerales.EstadosTurno.CanceladoId => new TurnoCancelado(),
                _ => new TurnoPendiente() // En caso de inconsistencias en la base de datos, inicia en Pendiente
            };
        }
    }
}
