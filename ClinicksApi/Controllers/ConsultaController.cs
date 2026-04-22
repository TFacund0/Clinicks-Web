using Microsoft.AspNetCore.Mvc;
using ClinicksApi.Business.Interfaces;
using ClinicksApi.Business.DTOs;

namespace ClinicksApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // La URL base será /api/consultas

    // Controlador que recibe las peticiones de React (Frontend) y se las pasa al "Cerebro" (Service) de C#.
    public class ConsultasController : ControllerBase
    {
        private readonly IConsultaService _consultaService;

        // Inyección de dependencias: Le damos el servicio de consultas al controlador al momento de crearse.
        public ConsultasController(IConsultaService consultaService)
        {
            _consultaService = consultaService;
        }

        [HttpPost]
        // Recibe el JSON desde React (ConsultaAltaDto) y guarda una nueva atención médica.
        public async Task<IActionResult> RegistrarConsulta([FromBody] ConsultaAltaDto dto)
        {
<<<<<<< HEAD
            // Mantenemos el ID hardcodeado temporalmente porque no hay login conectado aún.
            int idMedicoPrueba = 1;
=======
            // Mantenemos el ID hardcodeado temporalmente porque no hay login
            // (Asegurate de que el ID 2 o 1 exista en tu tabla "medico")
            int idMedicoPrueba = 2;
>>>>>>> feature-ingreso-consulta

            // Delega la validación y guardado en base de datos al servicio de negocio.
            var resultado = await _consultaService.RegistrarConsulta(dto, idMedicoPrueba);

            // Si se guardó bien, devuelve un 200 OK con un mensaje de éxito.
            if (resultado.Success)
            {
                return Ok(new
                {
                    success = true,
                    mensaje = "Consulta guardada correctamente"
                });
            }

            // Si falló (ej: paciente no existe), devuelve un error 400 (Bad Request) para que React lo muestre.
            return BadRequest(new
            {
                success = false,
                mensaje = resultado.Message
            });
        }

        [HttpGet("historial/{pacienteId}")]
        // Endpoint dinámico (/api/consultas/historial/5) que devuelve todas las consultas previas de un paciente.
        public async Task<IActionResult> GetHistorial(int pacienteId)
        {
            var historial = await _consultaService.ObtenerHistorialPaciente(pacienteId);
            return Ok(historial); // Devuelve la lista en formato JSON al frontend.
        }
    }
}