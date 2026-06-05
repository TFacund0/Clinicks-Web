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
            public const int PendienteId = 1;
            public const int ConfirmadoId = 2;
            public const int EnCursoId = 3;
            public const int AtendidoId = 4;
            public const int CanceladoId = 5;

            // Nombres para búsquedas
            public const string NombreAtendido = "atendido";
            public const string NombreCancelado = "cancelado";
        }

        public static class EstadosPaciente
        {
            public const string Activo = "activo";
        }
    }
}
