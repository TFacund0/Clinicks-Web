using ClinicksApi.Business.DTOs;
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

        public async Task<IEnumerable<PacienteDto>> ObtenerAtendidosPorMedico(int medicoId)
        {
            var datos = await _repository.GetAtendidosByMedicoAsync(medicoId);
            return datos.Select(MapToDto);
        }

        public async Task<IEnumerable<PacienteDto>> ObtenerListado()
        {
            var datos = await _repository.GetAllAsync();
            return datos.Select(MapToDto);
        }

        public async Task<PacienteDto?> ObtenerPorId(int id)
        {
            var dato = await _repository.GetByIdAsync(id);
            if (dato == null) return null;
            return MapToDto(dato);
        }

        private PacienteDto MapToDto(Paciente dato)
        {
            return new PacienteDto
            {
                Id = dato.IdPaciente,
                NombreCompleto = $"{dato.Nombre} {dato.Apellido}",
                Dni = dato.Dni,
                Edad = DateTime.Now.Year - dato.FechaNacimiento.Year,
                FechaUltimaConsulta = dato.Turnos?.Any() == true
                    ? dato.Turnos.OrderByDescending(t => t.FechaTurno).First().FechaTurno.ToShortDateString()
                    : "Sin consultas",
                EstaActivo = dato.IdEstadoPacienteNavigation?.Nombre.ToLower() == "activo"
            };
        }

        public async Task<(bool Success, string Message)> ExistePaciente(string dni)
{ 
    var existe = await _repository.ExistePacientePorDniAsync(dni);

    if (existe)
    {
        return (true, "Paciente encontrado");
    }

    return (false, "Paciente no encontrado");
}
    }
}