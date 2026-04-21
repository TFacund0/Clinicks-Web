using ClinicksApi.Business.Dtos;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;

namespace ClinicksApi.Business.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _repository;

        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        // Implementamos el método para obtener pacientes atendidos por un médico específico
        public async Task<IEnumerable<PacienteDto>> ObtenerAtendidosPorMedico(int medicoId)
        {
            // Obtenemos los pacientes atendidos por el médico usando el repositorio
            var datos = await _repository.GetAtendidosByMedicoAsync(medicoId);

            // Mapeamos cada paciente a su DTO correspondiente
            return datos.Select(MapToDto);
        }

        // Implementamos el método para obtener el listado de pacientes
        public async Task<IEnumerable<PacienteDto>> ObtenerListado()
        {
            // Obtenemos todos los pacientes usando el repositorio
            var datos = await _repository.GetAllAsync();
            return datos.Select(MapToDto);
        }

        public async Task<PacienteDto?> ObtenerPorId(int id)
        {
            // Obtenemos el paciente por su ID usando el repositorio
            var dato = await _repository.GetByIdAsync(id);
            if (dato == null) return null;
            return MapToDto(dato);
        }

        // Método privado para mapear un objeto Paciente a PacienteDto
        private PacienteDto MapToDto(Paciente dato)
        {
            return new PacienteDto
            {
                // Mapeamos las propiedades básicas
                Id = dato.IdPaciente,
                NombreCompleto = $"{dato.Nombre} {dato.Apellido}",
                Dni = dato.Dni,
                Edad = DateTime.Now.Year - dato.FechaNacimiento.Year,

                // Aquí calculamos la fecha de la última consulta si la relación de Turnos existe
                FechaUltimaConsulta = dato.Turnos?.Any() == true
                    ? dato.Turnos.OrderByDescending(t => t.FechaTurno).First().FechaTurno.ToShortDateString()
                    : "Sin consultas",

                // Determinamos si el paciente está activo basándonos en su estado
                EstaActivo = dato.IdEstadoPacienteNavigation?.Nombre.ToLower() == "activo"
            };
        }
    }
}
