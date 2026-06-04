using Xunit;
using Moq; // Funciona joya
using ClinicksApi.Business.Services;
using ClinicksApi.Business.DTOs;
using ClinicksApi.Data.Interfaces;
using ClinicksApi.Data.Entities;

namespace ClinicksApi.Tests
{
    public class PacienteServiceTests
    {
        // El Mock (Simulador) de la base de datos
        private readonly Mock<IPacienteRepository> _pacienteRepoMock;
        // El servicio REAL que queremos probar
        private readonly PacienteService _pacienteService;

        public PacienteServiceTests()
        {
            // 1. Inicializamos el simulador vacío
            _pacienteRepoMock = new Mock<IPacienteRepository>();
            // 2. Le pasamos el objeto simulado (.Object) a nuestro servicio real
            _pacienteService = new PacienteService(_pacienteRepoMock.Object);
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
            _pacienteRepoMock.Setup(repo => repo.GetByIdAsync(idPrueba))
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
            _pacienteRepoMock.Setup(repo => repo.GetByIdAsync(idPrueba))
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
            _pacienteRepoMock.Setup(repo => repo.GetByDniAsync(dniInexistente))
                             .ReturnsAsync((Paciente?)null);

            // ACT
            var resultado = await _pacienteService.ValidarPaciente(dniInexistente);

            // ASSERT
            Assert.Null(resultado);
        }
    }
}