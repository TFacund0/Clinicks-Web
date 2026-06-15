using System;
using ClinicksApi.Data.Entities;

namespace ClinicksApi.Business.States
{
    /// <summary>
    /// Clase abstracta base que representa el estado de un turno.
    /// Define los comportamientos permitidos y las transiciones del ciclo de vida del turno.
    /// </summary>
    public abstract class TurnoState
    {
        /// <summary>
        /// Intenta confirmar el turno.
        /// Lanza una excepción por defecto si el estado actual no lo permite.
        /// </summary>
        public virtual void Confirmar(Turno turno)
            => throw new InvalidOperationException("No se puede confirmar el turno en el estado actual.");

        /// <summary>
        /// Intenta iniciar la atención médica para el turno actual.
        /// Lanza una excepción por defecto si el estado actual no lo permite.
        /// </summary>
        public virtual void IniciarAtencion(Turno turno)
            => throw new InvalidOperationException("No se puede iniciar la atención en el estado actual del turno.");

        /// <summary>
        /// Intenta finalizar la atención médica (consulta o procedimiento) del turno.
        /// Lanza una excepción por defecto si el estado actual no lo permite.
        /// </summary>
        public virtual void FinalizarAtencion(Turno turno)
            => throw new InvalidOperationException("No se puede finalizar la atención en el estado actual del turno.");

        /// <summary>
        /// Intenta cancelar el turno.
        /// Lanza una excepción por defecto si el estado actual no lo permite.
        /// </summary>
        public virtual void Cancelar(Turno turno)
            => throw new InvalidOperationException("No se puede cancelar el turno en el estado actual.");
    }
}
