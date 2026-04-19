using ClinicksApi.Business.Dtos;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Interfaces;

namespace ClinicksApi.Business.Services
{
    // Capa de Negocio (Business/Services)
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _repository;

        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PacienteDto>> ObtenerListado()
        {
            var datos = await _repository.GetAllAsync();

            return datos.Select(dato => new PacienteDto
            {
                Id = dato.IdPaciente,
                NombreCompleto = dato.Nombre + " " + dato.Apellido,
                Dni = dato.Dni,
                Edad = DateTime.Now.Year - dato.FechaNacimiento.Year,
                FechaUltimaConsulta = "Sin datos", // Aquí podrías calcular la fecha de la última consulta médica
                EstaActivo = dato.IdEstadoPacienteNavigation.Nombre.ToLower() == "activo"
            });
        }

        public async Task<PacienteDto?> ObtenerPorId(int id)
        {
            var dato = await _repository.GetByIdAsync(id);
            if (dato == null) return null;

            // Convertimos la entidad a DTO
            return new PacienteDto
            {
                Id = dato.IdPaciente,
                NombreCompleto = dato.Nombre + " " + dato.Apellido,
                Dni = dato.Dni,
                Edad = DateTime.Now.Year - dato.FechaNacimiento.Year,
                FechaUltimaConsulta = "Sin datos", // Aquí podrías calcular la fecha de la última consulta médica
                EstaActivo = dato.IdEstadoPacienteNavigation.Nombre.ToLower() == "activo"
            };
        }
    }
}
