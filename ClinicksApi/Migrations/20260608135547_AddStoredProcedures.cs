using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicksApi.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var createProcedure = @"
                CREATE OR REPLACE PROCEDURE ActualizarEstadoTurnoSP(
                    p_idTurno INT,
                    p_idEstado INT,
                    p_idConsulta INT DEFAULT NULL,
                    p_idProcedimiento INT DEFAULT NULL
                )
                LANGUAGE plpgsql
                AS $$
                BEGIN
                    UPDATE ""turno""
                    SET ""id_estado_turno"" = p_idEstado,
                        ""id_consulta"" = COALESCE(p_idConsulta, ""id_consulta""),
                        ""id_procedimiento"" = COALESCE(p_idProcedimiento, ""id_procedimiento"")
                    WHERE ""id_turno"" = p_idTurno;
                END;
                $$;";

            migrationBuilder.Sql(createProcedure);

            var createFunction = @"
                CREATE OR REPLACE FUNCTION ObtenerTurnosPorMedico(p_idMedico INT)
                RETURNS SETOF ""turno""
                LANGUAGE plpgsql
                AS $$
                BEGIN
                    RETURN QUERY
                    SELECT *
                    FROM ""turno""
                    WHERE ""id_medico"" = p_idMedico;
                END;
                $$;";

            migrationBuilder.Sql(createFunction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS ActualizarEstadoTurnoSP(INT, INT, INT, INT);");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS ObtenerTurnosPorMedico(INT);");
        }
    }
}
