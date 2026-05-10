using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.DTOs;

namespace ClinicksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // La URL base será /api/procesos
    public class ProcesosController : ControllerBase
    {
        private readonly IProcesoService _procesoService;

        public ProcesosController(IProcesoService procesoService)
        {
            _procesoService = procesoService;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarProceso([FromBody] ProcesoAltaDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mantenemos el ID hardcodeado temporalmente porque no hay login conectado aún.
            int idMedicoPrueba = 2;

            var resultado = await _procesoService.RegistrarProceso(dto, idMedicoPrueba);

            if (resultado.Success)
            {
                return Ok(new
                {
                    success = true,
                    mensaje = resultado.Message
                });
            }

            return BadRequest(new
            {
                success = false,
                mensaje = resultado.Message
            });
        }

        [HttpGet("tipos")]
        public IActionResult GetTiposProceso()
        {
            // Devolvemos una lista harcodeada ya que la base de datos no tiene una tabla específica para esto
            var tipos = new List<object>
            {
                new { id = 1, nombre = "Cirugía Menor" },
                new { id = 2, nombre = "Estudio de Imagen (Rayos X, MRI)" },
                new { id = 3, nombre = "Análisis de Laboratorio" },
                new { id = 4, nombre = "Rehabilitación Física" },
                new { id = 5, nombre = "Consulta Especializada" },
                new { id = 6, nombre = "Procedimiento Odontológico" },
                new { id = 7, nombre = "Curación de Heridas" },
                new { id = 8, nombre = "Chequeo General" },
                new { id = 9, nombre = "Otro" }
            };

            return Ok(tipos);
        }
    }
}
