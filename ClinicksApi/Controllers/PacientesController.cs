using Microsoft.AspNetCore.Mvc;
using ClinicksAPI.Models;

namespace ClinicksAPI.Controllers
{
    [ApiController]
    [Route("api/pacientes")]
    public class PacientesController : ControllerBase
    {
        // Simulación de base de datos
        private static List<Paciente> pacientes = new List<Paciente>();

        // 🔹 GET: api/pacientes
        [HttpGet]
        public List<Paciente> ObtenerPacientes()
        {
            return pacientes;
        }

        // 🔹 POST: api/pacientes
        [HttpPost]
        public Paciente CrearPaciente([FromBody] Paciente paciente)
        {
            paciente.Id = pacientes.Count + 1;
            pacientes.Add(paciente);
            return paciente;
        }
    }
}