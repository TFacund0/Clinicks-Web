using ClinicksApi.Business.DTOs;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;

namespace ClinicksApi.Business.Services
{
    // El "Cerebro" de los pacientes. 
    // Se encarga de pedirle datos a la base de datos y "traducirlos" a un formato que React pueda entender y dibujar fácilmente.
    public class PacienteService : IPacienteService
    {
        // Traemos al "obrero" (Repositorio) que sabe cómo hacer los SELECT en PostgreSQL.
        private readonly IPacienteRepository _repository;

        // Inyección de dependencias: ASP.NET nos da el repositorio listo para usar.
        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        // Pide los pacientes de un médico específico y los convierte al formato DTO para el frontend.
        public async Task<IEnumerable<PacienteDto>> ObtenerAtendidosPorMedico(int medicoId)
        {
            var datos = await _repository.GetAtendidosByMedicoAsync(medicoId);

            // .Select(MapToDto) agarra cada paciente de la lista y lo pasa por la función "traductora" de abajo.
            return datos.Select(MapToDto);
        }

        // Trae absolutamente a todos los pacientes del hospital y los convierte a DTO.
        public async Task<IEnumerable<PacienteDto>> ObtenerListado()
        {
            var datos = await _repository.GetAllAsync();
            return datos.Select(MapToDto);
        }

        // Busca a un solo paciente. Si no lo encuentra devuelve null, si lo encuentra lo traduce a DTO.
        public async Task<PacienteDto?> ObtenerPorId(int id)
        {
            var dato = await _repository.GetByIdAsync(id);
            if (dato == null) return null;

            return MapToDto(dato);
        }

        // LA FUNCIÓN TRADUCTORA (Mapeo)
        // Transforma la "Entidad" pesada de la base de datos en un "DTO" (Data Transfer Object) liviano y masticado para React.
        private PacienteDto MapToDto(Paciente dato)
        {
            return new PacienteDto
            {
                Id = dato.IdPaciente,

                // Unimos Nombre y Apellido en un solo campo para que React no tenga que hacerlo.
                NombreCompleto = $"{dato.Nombre} {dato.Apellido}",

                Dni = dato.Dni,

                // Calculamos la edad al vuelo restando el año actual menos el año de nacimiento.
                Edad = DateTime.Now.Year - dato.FechaNacimiento.Year,

                // Lógica para la fecha: Si tiene turnos, agarramos el más reciente y lo pasamos a texto ("dd/mm/yyyy"). Si no, mandamos "Sin consultas".
                FechaUltimaConsulta = dato.Turnos?.Any() == true
                    ? dato.Turnos.OrderByDescending(t => t.FechaTurno).First().FechaTurno.ToShortDateString()
                    : "Sin consultas",

                // Verificamos si la palabra "activo" está en la tabla de estados asociada a este paciente.
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