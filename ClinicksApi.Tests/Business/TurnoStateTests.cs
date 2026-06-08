using Xunit;
using System;
using ClinicksApi.Data.Entities;
using ClinicksApi.Business.States;
using ClinicksApi.Constants;

namespace ClinicksApi.Tests
{
    public class TurnoStateTests
    {
        [Fact]
        public void TurnoStateFactory_DeberiaMapearCorrectamenteLosIdsAEstados()
        {
            // ACT & ASSERT
            Assert.IsType<TurnoPendiente>(TurnoStateFactory.CrearEstado(ConstantesGenerales.EstadosTurno.PendienteId));
            Assert.IsType<TurnoConfirmado>(TurnoStateFactory.CrearEstado(ConstantesGenerales.EstadosTurno.ConfirmadoId));
            Assert.IsType<TurnoEnCurso>(TurnoStateFactory.CrearEstado(ConstantesGenerales.EstadosTurno.EnCursoId));
            Assert.IsType<TurnoAtendido>(TurnoStateFactory.CrearEstado(ConstantesGenerales.EstadosTurno.AtendidoId));
            Assert.IsType<TurnoCancelado>(TurnoStateFactory.CrearEstado(ConstantesGenerales.EstadosTurno.CanceladoId));
            
            // Fallback de seguridad
            Assert.IsType<TurnoPendiente>(TurnoStateFactory.CrearEstado(999));
        }

        [Fact]
        public void TurnoPendiente_DeberiaPermitirConfirmarYCancelarYFinalizar()
        {
            // ARRANGE
            var turno = new Turno { IdEstadoTurno = ConstantesGenerales.EstadosTurno.PendienteId };

            // ACT - Confirmar
            turno.Confirmar();
            Assert.IsType<TurnoConfirmado>(turno.EstadoActual);
            Assert.Equal(ConstantesGenerales.EstadosTurno.ConfirmadoId, turno.IdEstadoTurno);

            // Reset a Pendiente
            turno = new Turno { IdEstadoTurno = ConstantesGenerales.EstadosTurno.PendienteId };

            // ACT - Cancelar
            turno.Cancelar();
            Assert.IsType<TurnoCancelado>(turno.EstadoActual);
            Assert.Equal(ConstantesGenerales.EstadosTurno.CanceladoId, turno.IdEstadoTurno);

            // Reset a Pendiente
            turno = new Turno { IdEstadoTurno = ConstantesGenerales.EstadosTurno.PendienteId };

            // ACT - Finalizar
            turno.FinalizarAtencion();
            Assert.IsType<TurnoAtendido>(turno.EstadoActual);
            Assert.Equal(ConstantesGenerales.EstadosTurno.AtendidoId, turno.IdEstadoTurno);
        }

        [Fact]
        public void TurnoPendiente_NoDeberiaPermitirIniciarAtencion()
        {
            // ARRANGE
            var turno = new Turno { IdEstadoTurno = ConstantesGenerales.EstadosTurno.PendienteId };

            // ACT & ASSERT
            Assert.Throws<InvalidOperationException>(() => turno.IniciarAtencion());
        }

        [Fact]
        public void TurnoConfirmado_DeberiaPermitirIniciarAtencionYCancelarYFinalizar()
        {
            // ARRANGE
            var turno = new Turno { IdEstadoTurno = ConstantesGenerales.EstadosTurno.ConfirmadoId };

            // ACT - IniciarAtencion
            turno.IniciarAtencion();
            Assert.IsType<TurnoEnCurso>(turno.EstadoActual);
            Assert.Equal(ConstantesGenerales.EstadosTurno.EnCursoId, turno.IdEstadoTurno);

            // Reset
            turno = new Turno { IdEstadoTurno = ConstantesGenerales.EstadosTurno.ConfirmadoId };

            // ACT - Cancelar
            turno.Cancelar();
            Assert.IsType<TurnoCancelado>(turno.EstadoActual);
            Assert.Equal(ConstantesGenerales.EstadosTurno.CanceladoId, turno.IdEstadoTurno);

            // Reset
            turno = new Turno { IdEstadoTurno = ConstantesGenerales.EstadosTurno.ConfirmadoId };

            // ACT - Finalizar
            turno.FinalizarAtencion();
            Assert.IsType<TurnoAtendido>(turno.EstadoActual);
            Assert.Equal(ConstantesGenerales.EstadosTurno.AtendidoId, turno.IdEstadoTurno);
        }

        [Fact]
        public void TurnoConfirmado_NoDeberiaPermitirConfirmar()
        {
            // ARRANGE
            var turno = new Turno { IdEstadoTurno = ConstantesGenerales.EstadosTurno.ConfirmadoId };

            // ACT & ASSERT
            Assert.Throws<InvalidOperationException>(() => turno.Confirmar());
        }

        [Fact]
        public void TurnoEnCurso_DeberiaPermitirFinalizarAtencion()
        {
            // ARRANGE
            var turno = new Turno { IdEstadoTurno = ConstantesGenerales.EstadosTurno.EnCursoId };

            // ACT
            turno.FinalizarAtencion();

            // ASSERT
            Assert.IsType<TurnoAtendido>(turno.EstadoActual);
            Assert.Equal(ConstantesGenerales.EstadosTurno.AtendidoId, turno.IdEstadoTurno);
        }

        [Fact]
        public void TurnoEnCurso_NoDeberiaPermitirConfirmarOIniciarAtencionOCancelar()
        {
            // ARRANGE
            var turno = new Turno { IdEstadoTurno = ConstantesGenerales.EstadosTurno.EnCursoId };

            // ACT & ASSERT
            Assert.Throws<InvalidOperationException>(() => turno.Confirmar());
            Assert.Throws<InvalidOperationException>(() => turno.IniciarAtencion());
            Assert.Throws<InvalidOperationException>(() => turno.Cancelar());
        }

        [Fact]
        public void EstadosTerminales_NoDeberianPermitirNingunaTransicion()
        {
            // ARRANGE
            var turnoAtendido = new Turno { IdEstadoTurno = ConstantesGenerales.EstadosTurno.AtendidoId };
            var turnoCancelado = new Turno { IdEstadoTurno = ConstantesGenerales.EstadosTurno.CanceladoId };

            // ACT & ASSERT (Atendido)
            Assert.Throws<InvalidOperationException>(() => turnoAtendido.Confirmar());
            Assert.Throws<InvalidOperationException>(() => turnoAtendido.IniciarAtencion());
            Assert.Throws<InvalidOperationException>(() => turnoAtendido.FinalizarAtencion());
            Assert.Throws<InvalidOperationException>(() => turnoAtendido.Cancelar());

            // ACT & ASSERT (Cancelado)
            Assert.Throws<InvalidOperationException>(() => turnoCancelado.Confirmar());
            Assert.Throws<InvalidOperationException>(() => turnoCancelado.IniciarAtencion());
            Assert.Throws<InvalidOperationException>(() => turnoCancelado.FinalizarAtencion());
            Assert.Throws<InvalidOperationException>(() => turnoCancelado.Cancelar());
        }
    }
}
