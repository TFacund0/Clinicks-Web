using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Extensions;

namespace ClinicksApi.Controllers
{
    /// <summary>
    /// Controlador responsable de gestionar la información de los médicos del sistema.
    /// Requiere autenticación con Token JWT válido.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MedicosController : ControllerBase
    {
        private readonly IMedicoService _medicoService;

        /// <summary>
        /// Constructor de MedicosController.
        /// </summary>
        public MedicosController(IMedicoService medicoService)
        {
            _medicoService = medicoService;
        }

        /// <summary>
        /// Obtiene la información de perfil del médico que se encuentra actualmente autenticado.
        /// </summary>
        [HttpGet("perfil")]
        public async Task<IActionResult> ObtenerPerfil()
        {
            var idMedico = User.GetMedicoId();
            if (idMedico == null)
                return Unauthorized(new { message = "No se pudo identificar al médico autenticado." });

            var medico = await _medicoService.ObtenerPorIdAsync(idMedico.Value);
            if (medico == null)
                return NotFound(new { message = "Médico no encontrado." });

            return Ok(medico);
        }

    }
}
