using ClinicksApi.Business.DTOs;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;
using ClinicksApi.Constants;

namespace ClinicksApi.Business.Services
{
    /// <inheritdoc/>
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _repository;
        private readonly IConsultaRepository _consultaRepository;
        private readonly IProcedimientoRepository _procedimientoRepository;

        /// <summary>
        /// Constructor de PacienteService.
        /// </summary>
        public PacienteService(IPacienteRepository repository, IConsultaRepository consultaRepository, IProcedimientoRepository procedimientoRepository)
        {
            _repository = repository;
            _consultaRepository = consultaRepository;
            _procedimientoRepository = procedimientoRepository;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PacienteDto>> ObtenerAtendidosPorMedico(int medicoId, string? search = null)
        {
            var datos = await _repository.ObtenerAtendidosPorMedico(medicoId, search);
            return datos.Select(MapToDto);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<PacienteDto>> ObtenerListado()
        {
            var datos = await _repository.ObtenerTodosAsync();
            return datos.Select(MapToDto);
        }

        /// <inheritdoc/>
        public async Task<PacienteDto?> ObtenerPorId(int id)
        {
            var dato = await _repository.ObtenerPorIdAsync(id);
            if (dato == null) return null;

            return MapToDto(dato);
        }

        /// <inheritdoc/>
        public async Task<PacienteDto?> ObtenerPorDni(string dni)
        {
            var dato = await _repository.ObtenerPorDniAsync(dni);
            if (dato == null) return null;

            // Aquí se pueden agregar reglas de negocio (ej. retornar null si el paciente está inactivo)
            // if (dato.IdEstadoPacienteNavigation?.Nombre?.ToLower() != ConstantesGenerales.EstadosPaciente.Activo) return null;

            return MapToDto(dato);
        }

        /// <summary>
        /// Mapea la entidad Paciente a PacienteDto.
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
                FechaUltimaConsulta = (dato.Turnos != null && dato.Turnos.Any())
                    ? dato.Turnos.OrderByDescending(t => t.FechaTurno).First().FechaTurno.ToShortDateString()
                    : "Sin consultas",
                EstaActivo = dato.IdEstadoPacienteNavigation?.Nombre?.ToLower() == ConstantesGenerales.EstadosPaciente.Activo
            };
        }

        /// <inheritdoc/>
        public async Task<PacienteDto?> ValidarPaciente(string dni)
        { 
            return await ObtenerPorDni(dni);
        }

        /// <inheritdoc/>
        public async Task<HistorialClinicoDto?> ObtenerHistorialClinico(int pacienteId)
        {
            var pacienteDto = await ObtenerPorId(pacienteId);
            if (pacienteDto == null) return null;

            var consultasDirectas = await _consultaRepository.ObtenerHistorialConsultasAsync(pacienteId);
            var procedimientosDB = await _procedimientoRepository.ObtenerHistorialProcedimientosAsync(pacienteId);

            var consultas = consultasDirectas
                .Select(c => new ConsultaHistorialDto
                {
                    IdConsulta = c.IdConsulta,
                    Motivo = c.Motivo,
                    Diagnostico = c.Diagnostico,
                    Tratamiento = c.Tratamiento ?? string.Empty,
                    Observacion = c.Observacion ?? string.Empty,
                    Recomendacion = c.Recomendacion ?? string.Empty,
                    FechaConsulta = c.FechaConsulta,
                    MedicoAtencion = c.IdMedicoNavigation != null 
                        ? $"{c.IdMedicoNavigation.Nombre} {c.IdMedicoNavigation.Apellido}" 
                        : "Médico Desconocido"
                })
                .ToList();

            var procedimientos = procedimientosDB
                .Select(p => {
                    var turnoAsociado = p.Turnos?.FirstOrDefault();
                    return new ProcedimientoHistorialDto
                    {
                        IdProcedimiento = p.IdProcedimiento,
                        Tipo = p.Tipo,
                        Descripcion = p.Descripcion ?? string.Empty,
                        Resultado = p.Resultado ?? string.Empty,
                        Fecha = p.Fecha,
                        MedicoAtencion = turnoAsociado?.IdMedicoNavigation != null 
                            ? $"{turnoAsociado.IdMedicoNavigation.Nombre} {turnoAsociado.IdMedicoNavigation.Apellido}" 
                            : "No registrado"
                    };
                })
                .ToList();

            return new HistorialClinicoDto
            {
                Paciente = pacienteDto,
                Consultas = consultas,
                Procedimientos = procedimientos
            };
        }
    }
}