using ClinicksApi.Business.DTOs;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Data.Entities;
using ClinicksApi.Data.Interfaces;

namespace ClinicksApi.Business.Services
{
    /// <inheritdoc/>
    public class MedicoService : IMedicoService
    {
        private readonly IMedicoRepository _medicoRepository;

        /// <summary>
        /// Constructor de MedicoService.
        /// </summary>
        public MedicoService(IMedicoRepository medicoRepository)
        {
            _medicoRepository = medicoRepository;
        }

        /// <inheritdoc/>
        public async Task<MedicoDto?> ObtenerPorIdAsync(int idMedico)
        {
            var medico = await _medicoRepository.ObtenerPorIdAsync(idMedico);
            if (medico == null) return null;

            return MapToDto(medico);
        }

        /// <inheritdoc/>
        public async Task<MedicoDto?> ObtenerPorUsuarioIdAsync(int usuarioId)
        {
            var medico = await _medicoRepository.ObtenerPorUsuarioIdAsync(usuarioId);
            if (medico == null) return null;

            return MapToDto(medico);
        }



        private MedicoDto MapToDto(Medico medico)
        {
            return new MedicoDto
            {
                IdMedico = medico.IdMedico,
                Matricula = medico.Matricula,
                Nombre = medico.Nombre,
                Apellido = medico.Apellido,
                Correo = medico.Correo,
                Dni = medico.Dni,
                Especialidad = medico.IdEspecialidadNavigation?.Nombre ?? "Sin Especialidad",
                IdUsuario = medico.IdUsuario
            };
        }
    }
}
