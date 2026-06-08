using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicksApi.Business.Services;
using ClinicksApi.Data.Interfaces;
using ClinicksApi.Data.Entities;
using ClinicksApi.Constants;

namespace ClinicksApi.Tests
{
    public class TurnoServiceTests
    {
        private readonly Mock<ITurnoRepository> _turnoRepoMock;
        private readonly TurnoService _turnoService;

        public TurnoServiceTests()
        {
            _turnoRepoMock = new Mock<ITurnoRepository>();
            _turnoService = new TurnoService(_turnoRepoMock.Object);
        }

        [Fact]
        public async Task ObtenerTurnosAgendadosAsync_DeberiaDevolverLista_CuandoExistenTurnos()
        {
            // ARRANGE
            var turnosFake = new List<Turno>
            {
                new Turno { IdTurno = 1, IdPacienteNavigation = new Paciente { Nombre = "Juan", Apellido = "Pérez" } },
                new Turno { IdTurno = 2, IdPacienteNavigation = new Paciente { Nombre = "Ana", Apellido = "López" } }
            };

            _turnoRepoMock.Setup(repo => repo.ObtenerTodosAsync()).ReturnsAsync(turnosFake);

            // ACT
            var resultado = await _turnoService.ObtenerTurnosAgendadosAsync();

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("Juan Pérez", resultado.First().PacienteNombreCompleto);
        }

        [Fact]
        public async Task ObtenerTurnosAgendadosAsync_DeberiaDevolverVacio_CuandoNoHayTurnos()
        {
            // ARRANGE
            _turnoRepoMock.Setup(repo => repo.ObtenerTodosAsync()).ReturnsAsync(new List<Turno>());

            // ACT
            var resultado = await _turnoService.ObtenerTurnosAgendadosAsync();

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task ObtenerTurnosMedicoAsync_DeberiaDevolverListaFiltrada()
        {
            // ARRANGE
            int idMedico = 1;
            var fechaInicio = DateTime.Now;
            var fechaFin = DateTime.Now.AddDays(7);

            var turnosFake = new List<Turno>
            {
                new Turno { IdTurno = 10, FechaTurno = DateTime.Now.AddDays(1) }
            };

            _turnoRepoMock.Setup(repo => repo.ObtenerTurnosMedicoAsync(idMedico, fechaInicio, fechaFin)).ReturnsAsync(turnosFake);

            // ACT
            var resultado = await _turnoService.ObtenerTurnosMedicoAsync(idMedico, fechaInicio, fechaFin);

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Single(resultado);
            Assert.Equal(10, resultado.First().IdTurno);
        }

        [Fact]
        public async Task ObtenerTurnoPorIdAsync_DeberiaDevolverTurno_CuandoExiste()
        {
            // ARRANGE
            int idTurno = 5;
            var turnoFake = new Turno { IdTurno = idTurno, Motivo = "Consulta general" };

            _turnoRepoMock.Setup(repo => repo.ObtenerPorIdAsync(idTurno)).ReturnsAsync(turnoFake);

            // ACT
            var resultado = await _turnoService.ObtenerTurnoPorIdAsync(idTurno);

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Equal("Consulta general", resultado.Motivo);
        }

        [Fact]
        public async Task ObtenerTurnoPorIdAsync_DeberiaDevolverNull_CuandoNoExiste()
        {
            // ARRANGE
            int idTurno = 99;
            _turnoRepoMock.Setup(repo => repo.ObtenerPorIdAsync(idTurno)).ReturnsAsync((Turno?)null);

            // ACT
            var resultado = await _turnoService.ObtenerTurnoPorIdAsync(idTurno);

            // ASSERT
            Assert.Null(resultado);
        }

        [Fact]
        public async Task CancelarTurnosVencidosAsync_DeberiaCancelarTurnos_CuandoHayVencidos()
        {
            // ARRANGE
            var turnosVencidosFake = new List<Turno>
            {
                new Turno { IdTurno = 1, IdEstadoTurno = 1 },
                new Turno { IdTurno = 2, IdEstadoTurno = 1 }
            };

            _turnoRepoMock.Setup(repo => repo.ObtenerIdsEstadosPorNombresAsync(It.IsAny<List<string>>()))
                          .ReturnsAsync(new List<int> { 1, 2 });
            
            _turnoRepoMock.Setup(repo => repo.ObtenerIdEstadoPorNombreAsync("cancelado"))
                          .ReturnsAsync(ConstantesGenerales.EstadosTurno.CanceladoId);

            _turnoRepoMock.Setup(repo => repo.ObtenerTurnosPorFechaYEstadosAsync(It.IsAny<DateTime>(), It.IsAny<List<int>>()))
                          .ReturnsAsync(turnosVencidosFake);

            // ACT
            var resultado = await _turnoService.CancelarTurnosVencidosAsync();

            // ASSERT
            Assert.Equal(2, resultado);
            Assert.Equal(ConstantesGenerales.EstadosTurno.CanceladoId, turnosVencidosFake[0].IdEstadoTurno);
            Assert.Equal(ConstantesGenerales.EstadosTurno.CanceladoId, turnosVencidosFake[1].IdEstadoTurno);
            _turnoRepoMock.Verify(repo => repo.ActualizarLoteTurnosAsync(turnosVencidosFake), Times.Once);
        }

        [Fact]
        public async Task CancelarTurnosVencidosAsync_DeberiaDevolverCero_CuandoNoHayVencidos()
        {
            // ARRANGE
            _turnoRepoMock.Setup(repo => repo.ObtenerIdsEstadosPorNombresAsync(It.IsAny<List<string>>()))
                          .ReturnsAsync(new List<int> { 1, 2 });
            
            _turnoRepoMock.Setup(repo => repo.ObtenerIdEstadoPorNombreAsync("cancelado"))
                          .ReturnsAsync(ConstantesGenerales.EstadosTurno.CanceladoId);

            _turnoRepoMock.Setup(repo => repo.ObtenerTurnosPorFechaYEstadosAsync(It.IsAny<DateTime>(), It.IsAny<List<int>>()))
                          .ReturnsAsync(new List<Turno>());

            // ACT
            var resultado = await _turnoService.CancelarTurnosVencidosAsync();

            // ASSERT
            Assert.Equal(0, resultado);
            _turnoRepoMock.Verify(repo => repo.ActualizarLoteTurnosAsync(It.IsAny<List<Turno>>()), Times.Never);
        }

        [Fact]
        public async Task CancelarTurnosVencidosAsync_DeberiaUsarIdsPorDefecto_CuandoBdDevuelveNulos()
        {
            // ARRANGE
            var turnosVencidosFake = new List<Turno>
            {
                new Turno { IdTurno = 1, IdEstadoTurno = 1 }
            };

            // Simulamos que la BD no encuentra los IDs de estado y devuelve vacío/nulo
            _turnoRepoMock.Setup(repo => repo.ObtenerIdsEstadosPorNombresAsync(It.IsAny<List<string>>()))
                          .ReturnsAsync(new List<int>());
            
            _turnoRepoMock.Setup(repo => repo.ObtenerIdEstadoPorNombreAsync("cancelado"))
                          .ReturnsAsync((int?)null);

            // El servicio usará ConstantesGenerales.EstadosTurno.ConfirmadoId para la búsqueda
            // y ConstantesGenerales.EstadosTurno.CanceladoId para la actualización
            _turnoRepoMock.Setup(repo => repo.ObtenerTurnosPorFechaYEstadosAsync(It.IsAny<DateTime>(), It.IsAny<List<int>>()))
                          .ReturnsAsync(turnosVencidosFake);

            // ACT
            var resultado = await _turnoService.CancelarTurnosVencidosAsync();

            // ASSERT
            Assert.Equal(1, resultado);
            Assert.Equal(ConstantesGenerales.EstadosTurno.CanceladoId, turnosVencidosFake[0].IdEstadoTurno);
        }

        [Fact]
        public async Task MapToDto_DeberiaMapearPacienteDesconocido_CuandoPacienteEsNulo()
        {
            // ARRANGE
            int idTurno = 1;
            var turnoFake = new Turno { IdTurno = idTurno, IdPacienteNavigation = null }; // Sin paciente

            _turnoRepoMock.Setup(repo => repo.ObtenerPorIdAsync(idTurno)).ReturnsAsync(turnoFake);

            // ACT
            var resultado = await _turnoService.ObtenerTurnoPorIdAsync(idTurno);

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Equal("Paciente desconocido", resultado.PacienteNombreCompleto);
        }
    }
}
