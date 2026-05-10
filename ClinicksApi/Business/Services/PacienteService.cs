using ClinicksApi.Business.Dtos;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;

namespace ClinicksApi.Business.Services
{
    /// <summary>
    /// Servicio especialista en aplicar reglas de negocio sobre la información de los pacientes.
    /// Transforma los datos brutos de la Base de Datos (Entidades) en formatos ligeros (DTOs) para la web.
    /// </summary>
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _repository;

        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Lógica de negocio para buscar a los pacientes asignados a un médico.
        /// </summary>
        public async Task<IEnumerable<PacienteDto>> ObtenerAtendidosPorMedico(int medicoId)
        {
            // El estado 3 representa "Atendido" (regla de negocio en la capa de servicios)
            int estadoAtendido = 3;

            // Obtenemos los pacientes atendidos por el médico usando el repositorio
            var datos = await _repository.GetAtendidosByMedicoAsync(medicoId, estadoAtendido);

            // Mapeamos cada paciente a su DTO correspondiente
            return datos.Select(MapToDto);
        }

        /// <summary>
        /// Recupera y mapea la lista completa de pacientes activos e inactivos.
        /// </summary>
        public async Task<IEnumerable<PacienteDto>> ObtenerListado()
        {
            // Obtenemos todos los pacientes usando el repositorio
            var datos = await _repository.GetAllAsync();
            return datos.Select(MapToDto);
        }

        /// <summary>
        /// Recupera un paciente puntual y lo convierte en su DTO correspondiente.
        /// </summary>
        public async Task<PacienteDto?> ObtenerPorId(int id)
        {
            // Obtenemos el paciente por su ID usando el repositorio
            var dato = await _repository.GetByIdAsync(id);
            if (dato == null) return null;
            return MapToDto(dato);
        }

        /// <summary>
        /// Método auxiliar (Privado) que realiza la conversión de Entidad a DTO.
        /// Aquí se ejecutan cálculos al vuelo como la "Edad" basada en la fecha de nacimiento
        /// y la última fecha de turno usando LINQ.
        /// </summary>
        /// <param name="dato">Objeto pesado y completo de base de datos.</param>
        /// <returns>Objeto ligero listo para enviar por la web.</returns>
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
