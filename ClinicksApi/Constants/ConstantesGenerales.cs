namespace ClinicksApi.Constants
{
    public static class ConstantesGenerales
    {
        public static class Roles
        {
            public const string Medico = "Medico";
            public const string Admin = "Admin";
        }

        public static class EstadosTurno
        {
            // IDs por defecto (Fallback)
            public const int RealizadoId = 1;
            public const int AtendidoId = 2;

            // Nombres para búsquedas
            public const string NombreAtendido = "atendido";
            public const string NombreRealizado = "realizado";
        }

        public static class EstadosPaciente
        {
            public const string Activo = "activo";
        }
    }
}
