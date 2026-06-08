using ClinicksApi.Data.Entities;
using ClinicksApi.Constants;

namespace ClinicksApi.Business.States
{
    /// <summary>
    /// Representa un turno en estado "Pendiente".
    /// Se permite confirmar o cancelar el turno.
    /// </summary>
    public class TurnoPendiente : TurnoState
    {
        public override void Confirmar(Turno turno)
        {
            turno.CambiarEstado(new TurnoConfirmado(), ConstantesGenerales.EstadosTurno.ConfirmadoId);
        }

        public override void FinalizarAtencion(Turno turno)
        {
            turno.CambiarEstado(new TurnoAtendido(), ConstantesGenerales.EstadosTurno.AtendidoId);
        }

        public override void Cancelar(Turno turno)
        {
            turno.CambiarEstado(new TurnoCancelado(), ConstantesGenerales.EstadosTurno.CanceladoId);
        }
    }

    /// <summary>
    /// Representa un turno en estado "Confirmado".
    /// Se permite iniciar la atención del paciente o cancelar el turno.
    /// </summary>
    public class TurnoConfirmado : TurnoState
    {
        public override void IniciarAtencion(Turno turno)
        {
            turno.CambiarEstado(new TurnoEnCurso(), ConstantesGenerales.EstadosTurno.EnCursoId);
        }

        public override void FinalizarAtencion(Turno turno)
        {
            turno.CambiarEstado(new TurnoAtendido(), ConstantesGenerales.EstadosTurno.AtendidoId);
        }

        public override void Cancelar(Turno turno)
        {
            turno.CambiarEstado(new TurnoCancelado(), ConstantesGenerales.EstadosTurno.CanceladoId);
        }
    }

    /// <summary>
    /// Representa un turno en estado "En Curso" (el paciente está con el médico).
    /// Solo se permite finalizar la atención (al guardar la consulta o procedimiento).
    /// </summary>
    public class TurnoEnCurso : TurnoState
    {
        public override void FinalizarAtencion(Turno turno)
        {
            turno.CambiarEstado(new TurnoAtendido(), ConstantesGenerales.EstadosTurno.AtendidoId);
        }
    }

    /// <summary>
    /// Estado terminal "Atendido".
    /// No se permite ninguna transición posterior.
    /// </summary>
    public class TurnoAtendido : TurnoState
    {
        // No redefine métodos, por lo que hereda las excepciones por defecto de TurnoState
    }

    /// <summary>
    /// Estado terminal "Cancelado".
    /// No se permite ninguna transición posterior.
    /// </summary>
    public class TurnoCancelado : TurnoState
    {
        // No redefine métodos, por lo que hereda las excepciones por defecto de TurnoState
    }
}
