using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ClinicksApi.Business.Services;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Interfaces;
using ClinicksApi.Data.Entities;
using ClinicksApi.Business.DTOs;
using ClinicksApi.Constants;

namespace ClinicksApi.Tests
{
    public class ProcesoServiceTests
    {
        private readonly Mock<IProcesoRepository> _procesoRepoMock;
        private readonly Mock<IPacienteService> _pacienteServiceMock;
        private readonly Mock<ITurnoRepository> _turnoRepoMock;
        private readonly Mock<ILogger<ProcesoService>> _loggerMock;
        private readonly ProcesoService _procesoService;

        public ProcesoServiceTests()
        {
            _procesoRepoMock = new Mock<IProcesoRepository>();
            _pacienteServiceMock = new Mock<IPacienteService>();
            _turnoRepoMock = new Mock<ITurnoRepository>();
            _loggerMock = new Mock<ILogger<ProcesoService>>();

            _procesoService = new ProcesoService(
                _procesoRepoMock.Object,
                _pacienteServiceMock.Object,
                _turnoRepoMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public void ObtenerTiposProceso_DeberiaDevolverListaConDatos()
        {
            // ACT
            var resultado = _procesoService.ObtenerTiposProceso();

            // ASSERT
            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado);
        }

        [Fact]
        public async Task RegistrarProcedimiento_DeberiaDarError_CuandoMedicoEsInvalido()
        {
            // ARRANGE
            var procDto = new ProcedimientoAltaDto { fechaproceso = DateTime.Now.AddDays(-1), dnipaciente = "111" };
            int idMedico = 0; // Inválido

            // ACT
            var resultado = await _procesoService.RegistrarProcedimiento(procDto, idMedico);

            // ASSERT
            Assert.False(resultado.Success);
            Assert.Equal("El médico logueado es inválido.", resultado.Message);
        }


        [Fact]
        public async Task RegistrarProcedimiento_DeberiaDarError_CuandoPacienteNoExiste()
        {
            // ARRANGE
            var procDto = new ProcedimientoAltaDto { fechaproceso = DateTime.Now.AddDays(-1), dnipaciente = "111" };
            int idMedico = 1;

            _pacienteServiceMock.Setup(service => service.ObtenerPorDni("111")).ReturnsAsync((PacienteDto?)null);

            // ACT
            var resultado = await _procesoService.RegistrarProcedimiento(procDto, idMedico);

            // ASSERT
            Assert.False(resultado.Success);
            Assert.Equal("Paciente no encontrado en la base de datos o no apto para procedimientos.", resultado.Message);
        }

        [Fact]
        public async Task RegistrarProcedimiento_DeberiaActualizarTurno_CuandoIdTurnoVieneEnDto()
        {
            // ARRANGE
            var procDto = new ProcedimientoAltaDto 
            { 
                fechaproceso = DateTime.Now.AddDays(-1), 
                dnipaciente = "111",
                idTurno = 10
            };
            int idMedico = 1;
            int estadoAtendidoId = ConstantesGenerales.EstadosTurno.AtendidoId;

            var pacienteMock = new PacienteDto { Id = 100 };
            var turnoExistente = new Turno { IdTurno = 10, IdEstadoTurno = 1 };
            var procGuardado = new Procedimiento { IdProcedimiento = 500 };

            _pacienteServiceMock.Setup(s => s.ObtenerPorDni("111")).ReturnsAsync(pacienteMock);
            _procesoRepoMock.Setup(repo => repo.RegistrarProcedimiento(It.IsAny<Procedimiento>())).ReturnsAsync(procGuardado);
            _turnoRepoMock.Setup(repo => repo.ObtenerIdEstadoPorNombreAsync("Atendido")).ReturnsAsync(estadoAtendidoId);
            _turnoRepoMock.Setup(repo => repo.ObtenerParaActualizarAsync(10)).ReturnsAsync(turnoExistente);

            // ACT
            var resultado = await _procesoService.RegistrarProcedimiento(procDto, idMedico);

            // ASSERT
            Assert.True(resultado.Success);
            Assert.Equal(500, turnoExistente.IdProcedimiento);
            Assert.Equal(estadoAtendidoId, turnoExistente.IdEstadoTurno);
            _turnoRepoMock.Verify(repo => repo.ActualizarTurnoAsync(turnoExistente), Times.Once);
            _turnoRepoMock.Verify(repo => repo.CrearTurnoAsync(It.IsAny<Turno>()), Times.Never);
        }

        [Fact]
        public async Task RegistrarProcedimiento_DeberiaCrearTurnoNuevo_CuandoIdTurnoVacioUrgencia()
        {
            // ARRANGE
            var procDto = new ProcedimientoAltaDto 
            { 
                fechaproceso = DateTime.Now.AddDays(-1), 
                dnipaciente = "111",
                idTurno = null, // Urgencia
                tipoproceso = "Curación"
            };
            int idMedico = 1;
            int estadoAtendidoId = ConstantesGenerales.EstadosTurno.AtendidoId; // Fallback simulación

            var pacienteMock = new PacienteDto { Id = 100 };
            var procGuardado = new Procedimiento { IdProcedimiento = 500 };

            _pacienteServiceMock.Setup(s => s.ObtenerPorDni("111")).ReturnsAsync(pacienteMock);
            _procesoRepoMock.Setup(repo => repo.RegistrarProcedimiento(It.IsAny<Procedimiento>())).ReturnsAsync(procGuardado);
            _turnoRepoMock.Setup(repo => repo.ObtenerIdEstadoPorNombreAsync("Atendido")).ReturnsAsync((int?)null);

            // ACT
            var resultado = await _procesoService.RegistrarProcedimiento(procDto, idMedico);

            // ASSERT
            Assert.True(resultado.Success);
            _turnoRepoMock.Verify(repo => repo.CrearTurnoAsync(It.Is<Turno>(t => 
                t.IdProcedimiento == 500 && 
                t.IdPaciente == 100 && 
                t.IdMedico == 1 && 
                t.IdEstadoTurno == estadoAtendidoId)), Times.Once);
            
            _turnoRepoMock.Verify(repo => repo.ActualizarTurnoAsync(It.IsAny<Turno>()), Times.Never);
        }

        [Fact]
        public async Task RegistrarProcedimiento_DeberiaIgnorarTurno_CuandoIdTurnoNoExisteEnBD()
        {
            // ARRANGE
            var procDto = new ProcedimientoAltaDto 
            { 
                fechaproceso = DateTime.Now.AddDays(-1), 
                dnipaciente = "111",
                idTurno = 9999
            };
            int idMedico = 1;

            var pacienteMock = new PacienteDto { Id = 100 };
            var procGuardado = new Procedimiento { IdProcedimiento = 500 };

            _pacienteServiceMock.Setup(s => s.ObtenerPorDni("111")).ReturnsAsync(pacienteMock);
            _procesoRepoMock.Setup(repo => repo.RegistrarProcedimiento(It.IsAny<Procedimiento>())).ReturnsAsync(procGuardado);
            _turnoRepoMock.Setup(repo => repo.ObtenerParaActualizarAsync(9999)).ReturnsAsync((Turno?)null);

            // ACT
            var resultado = await _procesoService.RegistrarProcedimiento(procDto, idMedico);

            // ASSERT
            Assert.True(resultado.Success);
            _turnoRepoMock.Verify(repo => repo.ActualizarTurnoAsync(It.IsAny<Turno>()), Times.Never);
            _turnoRepoMock.Verify(repo => repo.CrearTurnoAsync(It.IsAny<Turno>()), Times.Never);
        }

        [Fact]
        public async Task RegistrarProcedimiento_DeberiaUsarValoresPorDefecto_CuandoResultadoYFechaSonNulos()
        {
            // ARRANGE
            var procDto = new ProcedimientoAltaDto 
            { 
                fechaproceso = null, 
                dnipaciente = "111",
                resultado = null
            };
            int idMedico = 1;

            var pacienteMock = new PacienteDto { Id = 100 };
            var procGuardado = new Procedimiento { IdProcedimiento = 500 };

            _pacienteServiceMock.Setup(s => s.ObtenerPorDni("111")).ReturnsAsync(pacienteMock);
            
            _procesoRepoMock.Setup(repo => repo.RegistrarProcedimiento(It.IsAny<Procedimiento>()))
                            .Callback<Procedimiento>(p => 
                            {
                                Assert.Equal("Sin resultado ingresado", p.Resultado);
                                Assert.NotEqual(DateTime.MinValue, p.Fecha);
                            })
                            .ReturnsAsync(procGuardado);

            // ACT
            var resultado = await _procesoService.RegistrarProcedimiento(procDto, idMedico);

            // ASSERT
            Assert.True(resultado.Success);
            _procesoRepoMock.Verify(repo => repo.RegistrarProcedimiento(It.IsAny<Procedimiento>()), Times.Once);
        }

        [Fact]
        public async Task RegistrarProcedimiento_DeberiaCapturarExcepcionYDevolverErrorGenerico()
        {
            // ARRANGE
            var procDto = new ProcedimientoAltaDto { fechaproceso = DateTime.Now.AddDays(-1), dnipaciente = "111" };
            int idMedico = 1;
            var pacienteMock = new PacienteDto { Id = 100 };

            _pacienteServiceMock.Setup(s => s.ObtenerPorDni("111")).ReturnsAsync(pacienteMock);
            
            _procesoRepoMock.Setup(repo => repo.RegistrarProcedimiento(It.IsAny<Procedimiento>()))
                            .ThrowsAsync(new Exception("Error crítico BD"));

            // ACT
            var resultado = await _procesoService.RegistrarProcedimiento(procDto, idMedico);

            // ASSERT
            Assert.False(resultado.Success);
            Assert.Equal("Error interno al registrar el procedimiento médico. Por favor, intente nuevamente más tarde.", resultado.Message);
        }
    }
}
