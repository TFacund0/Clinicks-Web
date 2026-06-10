using Xunit;
using Moq; // Funciona joya
using ClinicksApi.Business.Services;
using ClinicksApi.Business.DTOs;
using ClinicksApi.Data.Interfaces;
using ClinicksApi.Data.Entities;
using ClinicksApi.Constants;

namespace ClinicksApi.Tests
{
    public class PacienteServiceTests
    {
        // El Mock (Simulador) de la base de datos
        private readonly Mock<IPacienteRepository> _pacienteRepoMock;
        private readonly Mock<IConsultaRepository> _consultaRepoMock;
        private readonly Mock<IProcesoRepository> _procesoRepoMock;
        // El servicio REAL que queremos probar
        private readonly PacienteService _pacienteService;

        public PacienteServiceTests()
        {
            // 1. Inicializamos el simulador vacío
            _pacienteRepoMock = new Mock<IPacienteRepository>();
            _consultaRepoMock = new Mock<IConsultaRepository>();
            _procesoRepoMock = new Mock<IProcesoRepository>();
            // 2. Le pasamos el objeto simulado (.Object) a nuestro servicio real
            _pacienteService = new PacienteService(_pacienteRepoMock.Object, _consultaRepoMock.Object, _procesoRepoMock.Object);
        }

        [Fact]
        public async Task ObtenerPorId_DeberiaCalcularEdadCorrectamente_CuandoYaCumplioAnios()
        {
            // ARRANGE
            int idPrueba = 10;
            var añoActual = DateTime.Now.Year;

            var pacienteFake = new Paciente
            {
                IdPaciente = idPrueba,
                Nombre = "Carlos",
                Apellido = "Sánchez",
                Dni = "30111222",
                FechaNacimiento = new DateOnly(añoActual - 30, 1, 1) // Cumplió en enero
            };

            // ACÁ USAMOS MOQ: Le ordenamos qué responder
            _pacienteRepoMock.Setup(repo => repo.ObtenerPorIdAsync(idPrueba))
                             .ReturnsAsync(pacienteFake);

            // ACT
            var resultado = await _pacienteService.ObtenerPorId(idPrueba);

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Equal(30, resultado.Edad);
        }

        [Fact]
        public async Task ObtenerPorId_DeberiaRestarUnAnioALaEdad_CuandoAunNoPasoSuCumpleanios()
        {
            // ARRANGE
            int idPrueba = 20;
            var añoActual = DateTime.Now.Year;

            var pacienteFake = new Paciente
            {
                IdPaciente = idPrueba,
                Nombre = "Ana",
                Apellido = "Gómez",
                Dni = "40333444",
                FechaNacimiento = new DateOnly(añoActual - 30, 12, 31) // Cumple en diciembre
            };

            // ACÁ USAMOS MOQ OTRA VEZ
            _pacienteRepoMock.Setup(repo => repo.ObtenerPorIdAsync(idPrueba))
                             .ReturnsAsync(pacienteFake);

            // ACT
            var resultado = await _pacienteService.ObtenerPorId(idPrueba);

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Equal(29, resultado.Edad); // Debería dar 29 porque todavía no pasó el cumple
        }

        [Fact]
        public async Task ValidarPaciente_DeberiaDevolverNull_CuandoDniNoEstaRegistrado()
        {
            // ARRANGE
            string dniInexistente = "11222333";

            // MOQ simulando que la base de datos no encontró el DNI
            _pacienteRepoMock.Setup(repo => repo.ObtenerPorDniAsync(dniInexistente))
                             .ReturnsAsync((Paciente?)null);

            // ACT
            var resultado = await _pacienteService.ValidarPaciente(dniInexistente);

            // ASSERT
            Assert.Null(resultado);
        }
        [Fact]
        public async Task ObtenerAtendidosPorMedico_DeberiaDevolverLista_CuandoHayPacientes()
        {
            // ARRANGE
            int medicoId = 1;
            var añoActual = DateTime.Now.Year;
            var pacientesFake = new List<Paciente>
            {
                new Paciente { IdPaciente = 1, Nombre = "Juan", Apellido = "Pérez", Dni = "111", FechaNacimiento = new DateOnly(añoActual - 20, 1, 1) },
                new Paciente { IdPaciente = 2, Nombre = "Ana", Apellido = "Gómez", Dni = "222", FechaNacimiento = new DateOnly(añoActual - 25, 1, 1) }
            };

            _pacienteRepoMock.Setup(repo => repo.ObtenerAtendidosPorMedico(medicoId, null))
                             .ReturnsAsync(pacientesFake);

            // ACT
            var resultado = await _pacienteService.ObtenerAtendidosPorMedico(medicoId);

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("Juan Pérez", resultado.First().NombreCompleto);
        }

        [Fact]
        public async Task ObtenerAtendidosPorMedico_DeberiaDevolverListaVacia_CuandoNoHayPacientes()
        {
            // ARRANGE
            int medicoId = 1;
            _pacienteRepoMock.Setup(repo => repo.ObtenerAtendidosPorMedico(medicoId, null))
                             .ReturnsAsync(new List<Paciente>());

            // ACT
            var resultado = await _pacienteService.ObtenerAtendidosPorMedico(medicoId);

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task ObtenerListado_DeberiaDevolverTodosLosPacientes()
        {
            // ARRANGE
            var pacientesFake = new List<Paciente>
            {
                new Paciente { IdPaciente = 1, Nombre = "Juan", Apellido = "Pérez", Dni = "111", FechaNacimiento = new DateOnly(1990, 1, 1) }
            };

            _pacienteRepoMock.Setup(repo => repo.ObtenerTodosAsync())
                             .ReturnsAsync(pacientesFake);

            // ACT
            var resultado = await _pacienteService.ObtenerListado();

            // ASSERT
            Assert.Single(resultado);
        }

        [Fact]
        public async Task ObtenerPorId_DeberiaDevolverNull_CuandoNoExiste()
        {
            // ARRANGE
            int idPrueba = 99;
            _pacienteRepoMock.Setup(repo => repo.ObtenerPorIdAsync(idPrueba))
                             .ReturnsAsync((Paciente?)null);

            // ACT
            var resultado = await _pacienteService.ObtenerPorId(idPrueba);

            // ASSERT
            Assert.Null(resultado);
        }

        [Fact]
        public async Task ObtenerPorDni_DeberiaDevolverPacienteMapeado_CuandoExiste()
        {
            // ARRANGE
            string dni = "12345678";
            var pacienteFake = new Paciente
            {
                IdPaciente = 1,
                Nombre = "Luis",
                Apellido = "Marta",
                Dni = dni,
                FechaNacimiento = new DateOnly(2000, 1, 1),
                IdEstadoPacienteNavigation = new EstadoPaciente { Nombre = ConstantesGenerales.EstadosPaciente.Activo }
            };

            _pacienteRepoMock.Setup(repo => repo.ObtenerPorDniAsync(dni))
                             .ReturnsAsync(pacienteFake);

            // ACT
            var resultado = await _pacienteService.ObtenerPorDni(dni);

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Equal("Luis Marta", resultado.NombreCompleto);
            Assert.True(resultado.EstaActivo);
        }

        [Fact]
        public async Task ObtenerPorDni_DeberiaDevolverNull_CuandoNoExiste()
        {
            // ARRANGE
            string dni = "000";
            _pacienteRepoMock.Setup(repo => repo.ObtenerPorDniAsync(dni))
                             .ReturnsAsync((Paciente?)null);

            // ACT
            var resultado = await _pacienteService.ObtenerPorDni(dni);

            // ASSERT
            Assert.Null(resultado);
        }

        [Fact]
        public async Task ValidarPaciente_DeberiaDevolverPaciente_CuandoDniExiste()
        {
            // ARRANGE
            string dni = "12345678";
            var pacienteFake = new Paciente
            {
                IdPaciente = 1,
                Nombre = "Valid",
                Apellido = "User",
                Dni = dni,
                FechaNacimiento = new DateOnly(1980, 1, 1)
            };

            _pacienteRepoMock.Setup(repo => repo.ObtenerPorDniAsync(dni))
                             .ReturnsAsync(pacienteFake);

            // ACT
            var resultado = await _pacienteService.ValidarPaciente(dni);

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Equal("Valid User", resultado.NombreCompleto);
        }

        [Fact]
        public async Task ObtenerHistorialClinico_DeberiaDevolverNull_CuandoPacienteNoExiste()
        {
            // ARRANGE
            int idPrueba = 1;
            _pacienteRepoMock.Setup(repo => repo.ObtenerPorIdAsync(idPrueba))
                             .ReturnsAsync((Paciente?)null);

            // ACT
            var resultado = await _pacienteService.ObtenerHistorialClinico(idPrueba);

            // ASSERT
            Assert.Null(resultado);
        }

        [Fact]
        public async Task ObtenerHistorialClinico_DeberiaDevolverHistorialVacio_CuandoNoHayAtenciones()
        {
            // ARRANGE
            int idPrueba = 1;
            var pacienteFake = new Paciente
            {
                IdPaciente = idPrueba,
                Nombre = "Historial",
                Apellido = "Vacio",
                Dni = "111",
                FechaNacimiento = new DateOnly(1990, 1, 1)
            };

            _pacienteRepoMock.Setup(repo => repo.ObtenerPorIdAsync(idPrueba))
                             .ReturnsAsync(pacienteFake);
            
            _consultaRepoMock.Setup(repo => repo.ObtenerHistorialConsultasAsync(idPrueba))
                             .ReturnsAsync(new List<ConsultaMedica>());

            _procesoRepoMock.Setup(repo => repo.ObtenerHistorialProcedimientosAsync(idPrueba))
                             .ReturnsAsync(new List<Procedimiento>());

            // ACT
            var resultado = await _pacienteService.ObtenerHistorialClinico(idPrueba);

            // ASSERT
            Assert.NotNull(resultado);
            Assert.NotNull(resultado.Paciente);
            Assert.Empty(resultado.Consultas);
            Assert.Empty(resultado.Procedimientos);
        }

        [Fact]
        public async Task ObtenerHistorialClinico_DeberiaMapearHistorialCompleto_CuandoExistenAtenciones()
        {
            // ARRANGE
            int idPrueba = 1;
            var pacienteFake = new Paciente
            {
                IdPaciente = idPrueba,
                Nombre = "Con",
                Apellido = "Historial",
                Dni = "222",
                FechaNacimiento = new DateOnly(1990, 1, 1)
            };

            var consultasFake = new List<ConsultaMedica>
            {
                new ConsultaMedica 
                { 
                    IdConsulta = 10, 
                    Motivo = "Dolor", 
                    IdMedicoNavigation = new Medico { Nombre = "Dr.", Apellido = "House" } 
                }
            };

            var procesosFake = new List<Procedimiento>
            {
                new Procedimiento 
                { 
                    IdProcedimiento = 20, 
                    Tipo = "Rayos X", 
                    Turnos = new List<Turno> 
                    { 
                        new Turno { IdMedicoNavigation = new Medico { Nombre = "Dra.", Apellido = "Grey" } } 
                    } 
                }
            };

            _pacienteRepoMock.Setup(repo => repo.ObtenerPorIdAsync(idPrueba))
                             .ReturnsAsync(pacienteFake);
            
            _consultaRepoMock.Setup(repo => repo.ObtenerHistorialConsultasAsync(idPrueba))
                             .ReturnsAsync(consultasFake);

            _procesoRepoMock.Setup(repo => repo.ObtenerHistorialProcedimientosAsync(idPrueba))
                             .ReturnsAsync(procesosFake);

            // ACT
            var resultado = await _pacienteService.ObtenerHistorialClinico(idPrueba);

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Single(resultado.Consultas);
            Assert.Equal("Dr. House", resultado.Consultas.First().MedicoAtencion);
            
            Assert.Single(resultado.Procedimientos);
            Assert.Equal("Dra. Grey", resultado.Procedimientos.First().MedicoAtencion);
        }
        [Fact]
        public async Task ObtenerPorId_DeberiaMapearFechaUltimaConsulta_CuandoTieneTurnos()
        {
            // ARRANGE
            int idPrueba = 1;
            var fechaEsperada = DateTime.Now.AddDays(-2);
            var pacienteFake = new Paciente
            {
                IdPaciente = idPrueba,
                Nombre = "Test",
                Apellido = "Turnos",
                Dni = "111",
                FechaNacimiento = new DateOnly(2000, 1, 1),
                Turnos = new List<Turno>
                {
                    new Turno { FechaTurno = DateTime.Now.AddDays(-10) },
                    new Turno { FechaTurno = fechaEsperada } // El más reciente
                }
            };

            _pacienteRepoMock.Setup(repo => repo.ObtenerPorIdAsync(idPrueba))
                             .ReturnsAsync(pacienteFake);

            // ACT
            var resultado = await _pacienteService.ObtenerPorId(idPrueba);

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Equal(fechaEsperada.ToShortDateString(), resultado.FechaUltimaConsulta);
        }

        [Fact]
        public async Task ObtenerPorId_DeberiaMapearSinConsultas_CuandoNoTieneTurnos()
        {
            // ARRANGE
            int idPrueba = 1;
            var pacienteFake = new Paciente
            {
                IdPaciente = idPrueba,
                Nombre = "Test",
                Apellido = "SinTurnos",
                Dni = "111",
                FechaNacimiento = new DateOnly(2000, 1, 1),
                Turnos = new List<Turno>() // Lista vacía
            };

            _pacienteRepoMock.Setup(repo => repo.ObtenerPorIdAsync(idPrueba))
                             .ReturnsAsync(pacienteFake);

            // ACT
            var resultado = await _pacienteService.ObtenerPorId(idPrueba);

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Equal("Sin consultas", resultado.FechaUltimaConsulta);
        }

        [Fact]
        public async Task ObtenerHistorialClinico_DeberiaMapearMedicoDesconocido_CuandoConsultaNoTieneMedico()
        {
            // ARRANGE
            int idPrueba = 1;
            var pacienteFake = new Paciente { IdPaciente = idPrueba, Nombre = "Test", Apellido = "Test", Dni = "111", FechaNacimiento = new DateOnly(2000, 1, 1) };
            
            var consultasFake = new List<ConsultaMedica>
            {
                new ConsultaMedica { IdConsulta = 1, Motivo = "Prueba", IdMedicoNavigation = null } // Sin médico
            };

            _pacienteRepoMock.Setup(repo => repo.ObtenerPorIdAsync(idPrueba)).ReturnsAsync(pacienteFake);
            _consultaRepoMock.Setup(repo => repo.ObtenerHistorialConsultasAsync(idPrueba)).ReturnsAsync(consultasFake);
            _procesoRepoMock.Setup(repo => repo.ObtenerHistorialProcedimientosAsync(idPrueba)).ReturnsAsync(new List<Procedimiento>());

            // ACT
            var resultado = await _pacienteService.ObtenerHistorialClinico(idPrueba);

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Single(resultado.Consultas);
            Assert.Equal("Médico Desconocido", resultado.Consultas.First().MedicoAtencion);
        }

        [Fact]
        public async Task ObtenerHistorialClinico_DeberiaMapearMedicoNoRegistrado_CuandoProcedimientoNoTieneMedico()
        {
            // ARRANGE
            int idPrueba = 1;
            var pacienteFake = new Paciente { IdPaciente = idPrueba, Nombre = "Test", Apellido = "Test", Dni = "111", FechaNacimiento = new DateOnly(2000, 1, 1) };
            
            var procesosFake = new List<Procedimiento>
            {
                new Procedimiento { IdProcedimiento = 1, Tipo = "Prueba", Turnos = new List<Turno>() } // Sin turno -> sin médico
            };

            _pacienteRepoMock.Setup(repo => repo.ObtenerPorIdAsync(idPrueba)).ReturnsAsync(pacienteFake);
            _consultaRepoMock.Setup(repo => repo.ObtenerHistorialConsultasAsync(idPrueba)).ReturnsAsync(new List<ConsultaMedica>());
            _procesoRepoMock.Setup(repo => repo.ObtenerHistorialProcedimientosAsync(idPrueba)).ReturnsAsync(procesosFake);

            // ACT
            var resultado = await _pacienteService.ObtenerHistorialClinico(idPrueba);

            // ASSERT
            Assert.NotNull(resultado);
            Assert.Single(resultado.Procedimientos);
            Assert.Equal("No registrado", resultado.Procedimientos.First().MedicoAtencion);
        }
    }
}