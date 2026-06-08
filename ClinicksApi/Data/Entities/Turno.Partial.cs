using System.ComponentModel.DataAnnotations.Schema;
using ClinicksApi.Business.States;

namespace ClinicksApi.Data.Entities
{
    /// <summary>
    /// Extensión parcial de la clase Turno para incorporar el patrón de diseño State.
    /// Define la propiedad EstadoActual y delega el comportamiento dinámico.
    /// </summary>
    public partial class Turno
    {
        private TurnoState? _estadoActual;

        /// <summary>
        /// Obtiene el estado actual del turno de manera diferida (Lazy Loading).
        /// Esto evita NullReferenceException durante la materialización de EF Core.
        /// </summary>
        [NotMapped]
        public TurnoState EstadoActual
        {
            get
            {
                if (_estadoActual == null)
                {
                    _estadoActual = TurnoStateFactory.CrearEstado(IdEstadoTurno);
                }
                return _estadoActual;
            }
        }

        /// <summary>
        /// Cambia el estado del turno internamente.
        /// Sincroniza la clase de comportamiento con el ID persistido.
        /// </summary>
        public void CambiarEstado(TurnoState nuevoEstado, int nuevoEstadoId)
        {
            _estadoActual = nuevoEstado;
            IdEstadoTurno = nuevoEstadoId;
        }

        /// <summary>
        /// Confirma el turno si el estado actual lo permite.
        /// </summary>
        public void Confirmar()
        {
            EstadoActual.Confirmar(this);
        }

        /// <summary>
        /// Inicia la atención del turno si el estado actual lo permite.
        /// </summary>
        public void IniciarAtencion()
        {
            EstadoActual.IniciarAtencion(this);
        }

        /// <summary>
        /// Finaliza la atención del turno si el estado actual lo permite.
        /// </summary>
        public void FinalizarAtencion()
        {
            EstadoActual.FinalizarAtencion(this);
        }

        /// <summary>
        /// Cancela el turno si el estado actual lo permite.
        /// </summary>
        public void Cancelar()
        {
            EstadoActual.Cancelar(this);
        }
    }
}
