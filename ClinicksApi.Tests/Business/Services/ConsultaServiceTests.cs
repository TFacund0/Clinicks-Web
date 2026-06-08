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
    public class ConsultaServiceTests
    {
        private readonly Mock<IConsultaRepository> _consultaRepoMock;
        private readonly Mock<IPacienteService> _pacienteServiceMock;
        private readonly Mock<ITurnoRepository> _turnoRepoMock;
        private readonly Mock<ILogger<ConsultaService>> _loggerMock;
        private readonly ConsultaService _consultaService;

        public ConsultaServiceTests()
        {
            _consultaRepoMock = new Mock<IConsultaRepository>();
            _pacienteServiceMock = new Mock<IPacienteService>();
            _turnoRepoMock = new Mock<ITurnoRepository>();
            _loggerMock = new Mock<ILogger<ConsultaService>>();

            _consultaService = new ConsultaService(
                _consultaRepoMock.Object,
                _pacienteServiceMock.Object,
                _turnoRepoMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task ObtenerListaConsultas_DeberiaDevolverListaMapeada_CuandoHayConsultas()
        {
            // ARRANGE
            var consultasFake = new List<ConsultaMedica>
            {
                new ConsultaMedica 
                { 
                    IdConsulta = 1, 
                    Motivo = "Fiebre", 
                    IdMedicoNavigation = new Medico { Nombre = "Dr.", Apellido = "Nick" } 
                },
                new ConsultaMedica 
                { 
                    IdConsulta = 2, 
                    Motivo = "Dolor", 
                    IdMedicoNavigation = null // Para probar "Médico Desconocido"
                }
            };

            _consultaRepoMock.Setup(repo => repo.ListaConsultas()).ReturnsAsync(consultasFake);

            // ACT
            var resultado = await _consultaService.ObtenerListaConsultas();

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Dr. Nick", resultado[0].MedicoAtencion);
            Assert.Equal("Médico Desconocido", resultado[1].MedicoAtencion);
        }

        [Fact]
        public async Task ObtenerListaConsultas_DeberiaDevolverVacia_CuandoNoHayConsultas()
        {
            // ARRANGE
            _consultaRepoMock.Setup(repo => repo.ListaConsultas()).ReturnsAsync(new List<ConsultaMedica>());

            // ACT
            var resultado = await _consultaService.ObtenerListaConsultas();

            // ASSERT
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task RegistrarConsulta_DeberiaDarError_CuandoFechaEsFutura()
        {
            // ARRANGE
            var consultaDto = new ConsultaAltaDto { fechaconsulta = DateTime.Now.AddDays(1), dnipaciente = "111" };
            int idMedico = 1;

            // ACT
            var resultado = await _consultaService.RegistrarConsulta(consultaDto, idMedico);

            // ASSERT
            Assert.False(resultado.Success);
            Assert.Equal("La fecha de consulta no puede ser futura.", resultado.Message);
        }

        [Fact]
        public async Task RegistrarConsulta_DeberiaDarError_CuandoMedicoEsInvalido()
        {
            // ARRANGE
            var consultaDto = new ConsultaAltaDto { fechaconsulta = DateTime.Now.AddDays(-1), dnipaciente = "111" };
            int idMedico = 0; // Inválido

            // ACT
            var resultado = await _consultaService.RegistrarConsulta(consultaDto, idMedico);

            // ASSERT
            Assert.False(resultado.Success);
            Assert.Equal("El Id del Médico logueado es obligatorio y debe ser mayor a cero.", resultado.Message);
        }

        [Fact]
        public async Task RegistrarConsulta_DeberiaDarError_CuandoPacienteNoExiste()
        {
            // ARRANGE
            var consultaDto = new ConsultaAltaDto { fechaconsulta = DateTime.Now.AddDays(-1), dnipaciente = "111" };
            int idMedico = 1;

            _pacienteServiceMock.Setup(service => service.ObtenerPorDni("111")).ReturnsAsync((PacienteDto?)null);

            // ACT
            var resultado = await _consultaService.RegistrarConsulta(consultaDto, idMedico);

            // ASSERT
            Assert.False(resultado.Success);
            Assert.Equal("Paciente no encontrado o no apto para consultas.", resultado.Message);
        }

        [Fact]
        public async Task RegistrarConsulta_DeberiaActualizarTurno_CuandoIdTurnoVieneEnDto()
        {
            // ARRANGE
            var consultaDto = new ConsultaAltaDto 
            { 
                fechaconsulta = DateTime.Now.AddDays(-1), 
                dnipaciente = "111",
                idTurno = 10 // Viene un ID de turno agendado
            };
            int idMedico = 1;
            int estadoAtendidoId = ConstantesGenerales.EstadosTurno.AtendidoId;

            var pacienteMock = new PacienteDto { Id = 100 };
            var turnoAActualizar = new Turno { IdTurno = 10, IdEstadoTurno = 1 };
            var consultaGuardada = new ConsultaMedica { IdConsulta = 500 };

            _pacienteServiceMock.Setup(s => s.ObtenerPorDni("111")).ReturnsAsync(pacienteMock);
            _consultaRepoMock.Setup(repo => repo.RegistrarConsulta(It.IsAny<ConsultaMedica>())).ReturnsAsync(consultaGuardada);
            _turnoRepoMock.Setup(repo => repo.ObtenerIdEstadoPorNombreAsync("Atendido")).ReturnsAsync(estadoAtendidoId);
            _turnoRepoMock.Setup(repo => repo.ObtenerParaActualizarAsync(10)).ReturnsAsync(turnoAActualizar);

            // ACT
            var resultado = await _consultaService.RegistrarConsulta(consultaDto, idMedico);

            // ASSERT
            Assert.True(resultado.Success);
            Assert.Equal(500, turnoAActualizar.IdConsulta);
            Assert.Equal(estadoAtendidoId, turnoAActualizar.IdEstadoTurno);
            _turnoRepoMock.Verify(repo => repo.ActualizarTurnoAsync(turnoAActualizar), Times.Once);
            _turnoRepoMock.Verify(repo => repo.CrearTurnoAsync(It.IsAny<Turno>()), Times.Never);
        }

        [Fact]
        public async Task RegistrarConsulta_DeberiaCrearTurnoNuevo_CuandoIdTurnoVacioUrgencia()
        {
            // ARRANGE
            var consultaDto = new ConsultaAltaDto 
            { 
                fechaconsulta = DateTime.Now.AddDays(-1), 
                dnipaciente = "111",
                idTurno = null // Urgencia espontánea
            };
            int idMedico = 1;
            int estadoAtendidoId = ConstantesGenerales.EstadosTurno.AtendidoId;

            var pacienteMock = new PacienteDto { Id = 100 };
            var consultaGuardada = new ConsultaMedica { IdConsulta = 500 };

            _pacienteServiceMock.Setup(s => s.ObtenerPorDni("111")).ReturnsAsync(pacienteMock);
            _consultaRepoMock.Setup(repo => repo.RegistrarConsulta(It.IsAny<ConsultaMedica>())).ReturnsAsync(consultaGuardada);
            // Simulamos fallback de estado en caso de que devuelva null la base de datos
            _turnoRepoMock.Setup(repo => repo.ObtenerIdEstadoPorNombreAsync("Atendido")).ReturnsAsync((int?)null);

            // ACT
            var resultado = await _consultaService.RegistrarConsulta(consultaDto, idMedico);

            // ASSERT
            Assert.True(resultado.Success);
            // Comprobamos que el repositorio crea un turno nuevo
            _turnoRepoMock.Verify(repo => repo.CrearTurnoAsync(It.Is<Turno>(t => 
                t.IdConsulta == 500 && 
                t.IdPaciente == 100 && 
                t.IdMedico == 1 && 
                t.IdEstadoTurno == estadoAtendidoId)), Times.Once);
            
            _turnoRepoMock.Verify(repo => repo.ActualizarTurnoAsync(It.IsAny<Turno>()), Times.Never);
        }

        [Fact]
        public async Task RegistrarConsulta_DeberiaCapturarExcepcionYDevolverErrorGenerico()
        {
            // ARRANGE
            var consultaDto = new ConsultaAltaDto { fechaconsulta = DateTime.Now.AddDays(-1), dnipaciente = "111" };
            int idMedico = 1;
            var pacienteMock = new PacienteDto { Id = 100 };

            _pacienteServiceMock.Setup(s => s.ObtenerPorDni("111")).ReturnsAsync(pacienteMock);
            
            // Forzamos un crash en la base de datos
            _consultaRepoMock.Setup(repo => repo.RegistrarConsulta(It.IsAny<ConsultaMedica>()))
                             .ThrowsAsync(new Exception("Error crítico BD"));

            // ACT
            var resultado = await _consultaService.RegistrarConsulta(consultaDto, idMedico);

            // ASSERT
            Assert.False(resultado.Success);
            Assert.Equal("Error interno al registrar la consulta médica. Por favor, intente nuevamente más tarde.", resultado.Message);
        }
        [Fact]
        public async Task RegistrarConsulta_DeberiaUsarValoresPorDefecto_CuandoDatosOpcionalesSonNulos()
        {
            // ARRANGE
            var consultaDto = new ConsultaAltaDto 
            { 
                fechaconsulta = null, 
                dnipaciente = "111",
                idTurno = null,
                tratamiento = null,
                observaciones = null,
                recomendacion = null
            };
            int idMedico = 1;
            var pacienteMock = new PacienteDto { Id = 100 };
            var consultaGuardada = new ConsultaMedica { IdConsulta = 500 };

            _pacienteServiceMock.Setup(s => s.ObtenerPorDni("111")).ReturnsAsync(pacienteMock);
            
            // Verificamos qué objeto se le pasa al repositorio
            _consultaRepoMock.Setup(repo => repo.RegistrarConsulta(It.IsAny<ConsultaMedica>()))
                             .Callback<ConsultaMedica>(c => 
                             {
                                 Assert.Equal("sin definir", c.Tratamiento);
                                 Assert.Equal("sin observaciones relevantes", c.Observacion);
                                 Assert.Equal("sin recomendaciones", c.Recomendacion);
                                 // La fecha debe haberse asignado a DateTime.Now
                                 Assert.NotEqual(DateTime.MinValue, c.FechaConsulta);
                             })
                             .ReturnsAsync(consultaGuardada);

            // ACT
            var resultado = await _consultaService.RegistrarConsulta(consultaDto, idMedico);

            // ASSERT
            Assert.True(resultado.Success);
            _consultaRepoMock.Verify(repo => repo.RegistrarConsulta(It.IsAny<ConsultaMedica>()), Times.Once);
        }

        [Fact]
        public async Task RegistrarConsulta_DeberiaIgnorarTurno_CuandoIdTurnoNoExisteEnBD()
        {
            // ARRANGE
            var consultaDto = new ConsultaAltaDto 
            { 
                fechaconsulta = DateTime.Now.AddDays(-1), 
                dnipaciente = "111",
                idTurno = 9999 // ID Turno proporcionado pero inexistente
            };
            int idMedico = 1;
            var pacienteMock = new PacienteDto { Id = 100 };
            var consultaGuardada = new ConsultaMedica { IdConsulta = 500 };

            _pacienteServiceMock.Setup(s => s.ObtenerPorDni("111")).ReturnsAsync(pacienteMock);
            _consultaRepoMock.Setup(repo => repo.RegistrarConsulta(It.IsAny<ConsultaMedica>())).ReturnsAsync(consultaGuardada);
            
            // Simulamos que el repositorio devuelve NULL indicando que no encontró el turno
            _turnoRepoMock.Setup(repo => repo.ObtenerParaActualizarAsync(9999)).ReturnsAsync((Turno?)null);

            // ACT
            var resultado = await _consultaService.RegistrarConsulta(consultaDto, idMedico);

            // ASSERT
            Assert.True(resultado.Success);
            // Comprobamos que, al devolver null, no intenta actualizar NADA y tampoco lo crea.
            _turnoRepoMock.Verify(repo => repo.ActualizarTurnoAsync(It.IsAny<Turno>()), Times.Never);
            _turnoRepoMock.Verify(repo => repo.CrearTurnoAsync(It.IsAny<Turno>()), Times.Never);
        }
    }
}
