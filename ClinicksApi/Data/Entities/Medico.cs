using System;
using System.Collections.Generic;

namespace ClinicksApi.Data.Entities
{
    public class Medico
    {
        public int IdMedico { get; set; }
        public string Matricula { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string? Correo { get; set; }
        public string Dni { get; set; } = null!;
        public int IdEspecialidad { get; set; }
        public int IdDireccion { get; set; }

        // PROPIEDAD CLAVE PARA EL LOGIN
        public int? IdUsuario { get; set; }

        // Propiedades de navegación (opcionales dependiendo de tu lógica de negocio)
        public virtual Usuario? Usuario { get; set; }
    }
}