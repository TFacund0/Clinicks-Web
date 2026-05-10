using ClinicksApi.Business.DTOs;
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

        /// <summary>
        /// Inyección de dependencias: ASP.NET nos da el repositorio listo para usar.
        /// </summary>
        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Lógica de negocio para buscar a los pacientes asignados a un médico.
        /// </summary>
        public async Task<IEnumerable<PacienteDto>> ObtenerAtendidosPorMedico(int medicoId)
        {
            var datos = await _repository.GetAtendidosByMedicoAsync(medicoId);
            return datos.Select(MapToDto);
        }

        /// <summary>
        /// Recupera y mapea la lista completa de pacientes activos e inactivos.
        /// </summary>
        public async Task<IEnumerable<PacienteDto>> ObtenerListado()
        {
            var datos = await _repository.GetAllAsync();
            return datos.Select(MapToDto);
        }

        /// <summary>
        /// Recupera un paciente puntual y lo convierte en su DTO correspondiente.
        /// </summary>
        public async Task<PacienteDto?> ObtenerPorId(int id)
        {
            var dato = await _repository.GetByIdAsync(id);
            if (dato == null) return null;

            return MapToDto(dato);
        }

        /// <summary>
        /// Método auxiliar (Privado) que realiza la conversión de Entidad a DTO.
        /// </summary>
        private PacienteDto MapToDto(Paciente dato)
        {
            // Calcula la edad correctamente: resta 1 si el cumpleaños todavía no pasó este año.
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            var edad = hoy.Year - dato.FechaNacimiento.Year;
            if (dato.FechaNacimiento > hoy.AddYears(-edad)) edad--;

            return new PacienteDto
            {
                Id              = dato.IdPaciente,
                NombreCompleto  = $"{dato.Nombre} {dato.Apellido}",
                Dni             = dato.Dni,
                Edad            = edad,
                FechaUltimaConsulta = dato.Turnos?.Any() == true
                    ? dato.Turnos.OrderByDescending(t => t.FechaTurno).First().FechaTurno.ToShortDateString()
                    : "Sin consultas",
                EstaActivo = dato.IdEstadoPacienteNavigation?.Nombre.ToLower() == "activo"
            };
        }

        /// <summary>
        /// Verifica la existencia de un paciente por su DNI.
        /// </summary>
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