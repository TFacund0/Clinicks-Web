using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClinicksApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "especialidad",
                columns: table => new
                {
                    id_especialidad = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("especialidad_pkey", x => x.id_especialidad);
                });

            migrationBuilder.CreateTable(
                name: "estado_cama",
                columns: table => new
                {
                    id_estado_cama = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("estado_cama_pkey", x => x.id_estado_cama);
                });

            migrationBuilder.CreateTable(
                name: "estado_paciente",
                columns: table => new
                {
                    id_estado_paciente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("estado_paciente_pkey", x => x.id_estado_paciente);
                });

            migrationBuilder.CreateTable(
                name: "estado_turno",
                columns: table => new
                {
                    id_estado_turno = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("estado_turno_pkey", x => x.id_estado_turno);
                });

            migrationBuilder.CreateTable(
                name: "estado_usuario",
                columns: table => new
                {
                    id_estado_usuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("estado_usuario_pkey", x => x.id_estado_usuario);
                });

            migrationBuilder.CreateTable(
                name: "pais",
                columns: table => new
                {
                    id_pais = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pais_pkey", x => x.id_pais);
                });

            migrationBuilder.CreateTable(
                name: "piso",
                columns: table => new
                {
                    id_piso = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nro_piso = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("piso_pkey", x => x.id_piso);
                });

            migrationBuilder.CreateTable(
                name: "procedimiento",
                columns: table => new
                {
                    id_procedimiento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tipo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: true),
                    fecha = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    resultado = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("procedimiento_pkey", x => x.id_procedimiento);
                });

            migrationBuilder.CreateTable(
                name: "rol",
                columns: table => new
                {
                    id_rol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("rol_pkey", x => x.id_rol);
                });

            migrationBuilder.CreateTable(
                name: "tipo_habitacion",
                columns: table => new
                {
                    id_tipo_habitacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tipo_habitacion_pkey", x => x.id_tipo_habitacion);
                });

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    apellido = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    correo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    id_estado_usuario = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("usuario_pkey", x => x.id_usuario);
                    table.ForeignKey(
                        name: "usuario_id_estado_usuario_fkey",
                        column: x => x.id_estado_usuario,
                        principalTable: "estado_usuario",
                        principalColumn: "id_estado_usuario");
                });

            migrationBuilder.CreateTable(
                name: "ciudad",
                columns: table => new
                {
                    id_ciudad = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    id_pais = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ciudad_pkey", x => x.id_ciudad);
                    table.ForeignKey(
                        name: "ciudad_id_pais_fkey",
                        column: x => x.id_pais,
                        principalTable: "pais",
                        principalColumn: "id_pais");
                });

            migrationBuilder.CreateTable(
                name: "habitacion",
                columns: table => new
                {
                    id_habitacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nro_habitacion = table.Column<int>(type: "integer", nullable: false),
                    id_piso = table.Column<int>(type: "integer", nullable: false),
                    id_tipo_habitacion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("habitacion_pkey", x => x.id_habitacion);
                    table.ForeignKey(
                        name: "habitacion_id_piso_fkey",
                        column: x => x.id_piso,
                        principalTable: "piso",
                        principalColumn: "id_piso");
                    table.ForeignKey(
                        name: "habitacion_id_tipo_habitacion_fkey",
                        column: x => x.id_tipo_habitacion,
                        principalTable: "tipo_habitacion",
                        principalColumn: "id_tipo_habitacion");
                });

            migrationBuilder.CreateTable(
                name: "usuario_rol",
                columns: table => new
                {
                    id_usuario = table.Column<int>(type: "integer", nullable: false),
                    id_rol = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("usuario_rol_pkey", x => new { x.id_usuario, x.id_rol });
                    table.ForeignKey(
                        name: "usuario_rol_id_rol_fkey",
                        column: x => x.id_rol,
                        principalTable: "rol",
                        principalColumn: "id_rol");
                    table.ForeignKey(
                        name: "usuario_rol_id_usuario_fkey",
                        column: x => x.id_usuario,
                        principalTable: "usuario",
                        principalColumn: "id_usuario");
                });

            migrationBuilder.CreateTable(
                name: "codigo_postal",
                columns: table => new
                {
                    id_codigo_postal = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    id_ciudad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("codigo_postal_pkey", x => x.id_codigo_postal);
                    table.ForeignKey(
                        name: "codigo_postal_id_ciudad_fkey",
                        column: x => x.id_ciudad,
                        principalTable: "ciudad",
                        principalColumn: "id_ciudad");
                });

            migrationBuilder.CreateTable(
                name: "cama",
                columns: table => new
                {
                    id_cama = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo_cama = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    id_estado_cama = table.Column<int>(type: "integer", nullable: false),
                    id_habitacion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("cama_pkey", x => x.id_cama);
                    table.ForeignKey(
                        name: "cama_id_estado_cama_fkey",
                        column: x => x.id_estado_cama,
                        principalTable: "estado_cama",
                        principalColumn: "id_estado_cama");
                    table.ForeignKey(
                        name: "cama_id_habitacion_fkey",
                        column: x => x.id_habitacion,
                        principalTable: "habitacion",
                        principalColumn: "id_habitacion");
                });

            migrationBuilder.CreateTable(
                name: "direccion",
                columns: table => new
                {
                    id_direccion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_calle = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    numero_calle = table.Column<int>(type: "integer", nullable: true),
                    id_codigo_postal = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("direccion_pkey", x => x.id_direccion);
                    table.ForeignKey(
                        name: "direccion_id_codigo_postal_fkey",
                        column: x => x.id_codigo_postal,
                        principalTable: "codigo_postal",
                        principalColumn: "id_codigo_postal");
                });

            migrationBuilder.CreateTable(
                name: "medico",
                columns: table => new
                {
                    id_medico = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    matricula = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    apellido = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    correo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    dni = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    id_especialidad = table.Column<int>(type: "integer", nullable: false),
                    id_direccion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("medico_pkey", x => x.id_medico);
                    table.ForeignKey(
                        name: "medico_id_direccion_fkey",
                        column: x => x.id_direccion,
                        principalTable: "direccion",
                        principalColumn: "id_direccion");
                    table.ForeignKey(
                        name: "medico_id_especialidad_fkey",
                        column: x => x.id_especialidad,
                        principalTable: "especialidad",
                        principalColumn: "id_especialidad");
                });

            migrationBuilder.CreateTable(
                name: "paciente",
                columns: table => new
                {
                    id_paciente = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    apellido = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    fecha_nacimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    dni = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    correo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    observaciones = table.Column<string>(type: "text", nullable: true),
                    fecha_registracion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    id_estado_paciente = table.Column<int>(type: "integer", nullable: false),
                    id_direccion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("paciente_pkey", x => x.id_paciente);
                    table.ForeignKey(
                        name: "paciente_id_direccion_fkey",
                        column: x => x.id_direccion,
                        principalTable: "direccion",
                        principalColumn: "id_direccion");
                    table.ForeignKey(
                        name: "paciente_id_estado_paciente_fkey",
                        column: x => x.id_estado_paciente,
                        principalTable: "estado_paciente",
                        principalColumn: "id_estado_paciente");
                });

            migrationBuilder.CreateTable(
                name: "consulta_medica",
                columns: table => new
                {
                    id_consulta = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    motivo = table.Column<string>(type: "text", nullable: false),
                    diagnostico = table.Column<string>(type: "text", nullable: false),
                    recomendacion = table.Column<string>(type: "text", nullable: true),
                    fecha_consulta = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    observacion = table.Column<string>(type: "text", nullable: true),
                    tratamiento = table.Column<string>(type: "text", nullable: true),
                    id_paciente = table.Column<int>(type: "integer", nullable: false),
                    id_medico = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("consulta_medica_pkey", x => x.id_consulta);
                    table.ForeignKey(
                        name: "consulta_medica_id_medico_fkey",
                        column: x => x.id_medico,
                        principalTable: "medico",
                        principalColumn: "id_medico");
                    table.ForeignKey(
                        name: "consulta_medica_id_paciente_fkey",
                        column: x => x.id_paciente,
                        principalTable: "paciente",
                        principalColumn: "id_paciente");
                });

            migrationBuilder.CreateTable(
                name: "internacion",
                columns: table => new
                {
                    id_internacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fecha_inicio = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    motivo = table.Column<string>(type: "text", nullable: false),
                    id_paciente = table.Column<int>(type: "integer", nullable: false),
                    id_medico = table.Column<int>(type: "integer", nullable: false),
                    id_cama = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("internacion_pkey", x => x.id_internacion);
                    table.ForeignKey(
                        name: "internacion_id_cama_fkey",
                        column: x => x.id_cama,
                        principalTable: "cama",
                        principalColumn: "id_cama");
                    table.ForeignKey(
                        name: "internacion_id_medico_fkey",
                        column: x => x.id_medico,
                        principalTable: "medico",
                        principalColumn: "id_medico");
                    table.ForeignKey(
                        name: "internacion_id_paciente_fkey",
                        column: x => x.id_paciente,
                        principalTable: "paciente",
                        principalColumn: "id_paciente");
                });

            migrationBuilder.CreateTable(
                name: "telefono",
                columns: table => new
                {
                    id_telefono = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    numero_telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    id_paciente = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("telefono_pkey", x => x.id_telefono);
                    table.ForeignKey(
                        name: "telefono_id_paciente_fkey",
                        column: x => x.id_paciente,
                        principalTable: "paciente",
                        principalColumn: "id_paciente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "turno",
                columns: table => new
                {
                    id_turno = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fecha_turno = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fecha_registracion = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    motivo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    id_paciente = table.Column<int>(type: "integer", nullable: false),
                    id_medico = table.Column<int>(type: "integer", nullable: false),
                    id_estado_turno = table.Column<int>(type: "integer", nullable: false),
                    id_procedimiento = table.Column<int>(type: "integer", nullable: true),
                    id_consulta = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("turno_pkey", x => x.id_turno);
                    table.ForeignKey(
                        name: "turno_id_consulta_fkey",
                        column: x => x.id_consulta,
                        principalTable: "consulta_medica",
                        principalColumn: "id_consulta");
                    table.ForeignKey(
                        name: "turno_id_estado_turno_fkey",
                        column: x => x.id_estado_turno,
                        principalTable: "estado_turno",
                        principalColumn: "id_estado_turno");
                    table.ForeignKey(
                        name: "turno_id_medico_fkey",
                        column: x => x.id_medico,
                        principalTable: "medico",
                        principalColumn: "id_medico");
                    table.ForeignKey(
                        name: "turno_id_paciente_fkey",
                        column: x => x.id_paciente,
                        principalTable: "paciente",
                        principalColumn: "id_paciente");
                    table.ForeignKey(
                        name: "turno_id_procedimiento_fkey",
                        column: x => x.id_procedimiento,
                        principalTable: "procedimiento",
                        principalColumn: "id_procedimiento");
                });

            migrationBuilder.CreateTable(
                name: "medicacion",
                columns: table => new
                {
                    id_medicacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_medicamento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    dosis = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    frecuencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    fecha_inicio = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fecha_fin = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    id_internacion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("medicacion_pkey", x => x.id_medicacion);
                    table.ForeignKey(
                        name: "medicacion_id_internacion_fkey",
                        column: x => x.id_internacion,
                        principalTable: "internacion",
                        principalColumn: "id_internacion");
                });

            migrationBuilder.CreateIndex(
                name: "cama_codigo_cama_key",
                table: "cama",
                column: "codigo_cama",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cama_id_estado_cama",
                table: "cama",
                column: "id_estado_cama");

            migrationBuilder.CreateIndex(
                name: "IX_cama_id_habitacion",
                table: "cama",
                column: "id_habitacion");

            migrationBuilder.CreateIndex(
                name: "IX_ciudad_id_pais",
                table: "ciudad",
                column: "id_pais");

            migrationBuilder.CreateIndex(
                name: "codigo_postal_codigo_key",
                table: "codigo_postal",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_codigo_postal_id_ciudad",
                table: "codigo_postal",
                column: "id_ciudad");

            migrationBuilder.CreateIndex(
                name: "IX_consulta_medica_id_medico",
                table: "consulta_medica",
                column: "id_medico");

            migrationBuilder.CreateIndex(
                name: "IX_consulta_medica_id_paciente",
                table: "consulta_medica",
                column: "id_paciente");

            migrationBuilder.CreateIndex(
                name: "IX_direccion_id_codigo_postal",
                table: "direccion",
                column: "id_codigo_postal");

            migrationBuilder.CreateIndex(
                name: "especialidad_nombre_key",
                table: "especialidad",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "estado_cama_nombre_key",
                table: "estado_cama",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "estado_paciente_nombre_key",
                table: "estado_paciente",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "estado_turno_nombre_key",
                table: "estado_turno",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "estado_usuario_nombre_key",
                table: "estado_usuario",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "habitacion_nro_habitacion_id_piso_key",
                table: "habitacion",
                columns: new[] { "nro_habitacion", "id_piso" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_habitacion_id_piso",
                table: "habitacion",
                column: "id_piso");

            migrationBuilder.CreateIndex(
                name: "IX_habitacion_id_tipo_habitacion",
                table: "habitacion",
                column: "id_tipo_habitacion");

            migrationBuilder.CreateIndex(
                name: "IX_internacion_id_cama",
                table: "internacion",
                column: "id_cama");

            migrationBuilder.CreateIndex(
                name: "IX_internacion_id_medico",
                table: "internacion",
                column: "id_medico");

            migrationBuilder.CreateIndex(
                name: "IX_internacion_id_paciente",
                table: "internacion",
                column: "id_paciente");

            migrationBuilder.CreateIndex(
                name: "IX_medicacion_id_internacion",
                table: "medicacion",
                column: "id_internacion");

            migrationBuilder.CreateIndex(
                name: "IX_medico_id_direccion",
                table: "medico",
                column: "id_direccion");

            migrationBuilder.CreateIndex(
                name: "IX_medico_id_especialidad",
                table: "medico",
                column: "id_especialidad");

            migrationBuilder.CreateIndex(
                name: "medico_correo_key",
                table: "medico",
                column: "correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "medico_dni_key",
                table: "medico",
                column: "dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "medico_matricula_key",
                table: "medico",
                column: "matricula",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_paciente_id_direccion",
                table: "paciente",
                column: "id_direccion");

            migrationBuilder.CreateIndex(
                name: "IX_paciente_id_estado_paciente",
                table: "paciente",
                column: "id_estado_paciente");

            migrationBuilder.CreateIndex(
                name: "paciente_correo_key",
                table: "paciente",
                column: "correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "paciente_dni_key",
                table: "paciente",
                column: "dni",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "pais_nombre_key",
                table: "pais",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "piso_nro_piso_key",
                table: "piso",
                column: "nro_piso",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "rol_nombre_key",
                table: "rol",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_telefono_id_paciente",
                table: "telefono",
                column: "id_paciente");

            migrationBuilder.CreateIndex(
                name: "tipo_habitacion_nombre_key",
                table: "tipo_habitacion",
                column: "nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_turno_id_consulta",
                table: "turno",
                column: "id_consulta");

            migrationBuilder.CreateIndex(
                name: "IX_turno_id_estado_turno",
                table: "turno",
                column: "id_estado_turno");

            migrationBuilder.CreateIndex(
                name: "IX_turno_id_medico",
                table: "turno",
                column: "id_medico");

            migrationBuilder.CreateIndex(
                name: "IX_turno_id_paciente",
                table: "turno",
                column: "id_paciente");

            migrationBuilder.CreateIndex(
                name: "IX_turno_id_procedimiento",
                table: "turno",
                column: "id_procedimiento");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_id_estado_usuario",
                table: "usuario",
                column: "id_estado_usuario");

            migrationBuilder.CreateIndex(
                name: "usuario_correo_key",
                table: "usuario",
                column: "correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "usuario_username_key",
                table: "usuario",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuario_rol_id_rol",
                table: "usuario_rol",
                column: "id_rol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "medicacion");

            migrationBuilder.DropTable(
                name: "telefono");

            migrationBuilder.DropTable(
                name: "turno");

            migrationBuilder.DropTable(
                name: "usuario_rol");

            migrationBuilder.DropTable(
                name: "internacion");

            migrationBuilder.DropTable(
                name: "consulta_medica");

            migrationBuilder.DropTable(
                name: "estado_turno");

            migrationBuilder.DropTable(
                name: "procedimiento");

            migrationBuilder.DropTable(
                name: "rol");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "cama");

            migrationBuilder.DropTable(
                name: "medico");

            migrationBuilder.DropTable(
                name: "paciente");

            migrationBuilder.DropTable(
                name: "estado_usuario");

            migrationBuilder.DropTable(
                name: "estado_cama");

            migrationBuilder.DropTable(
                name: "habitacion");

            migrationBuilder.DropTable(
                name: "especialidad");

            migrationBuilder.DropTable(
                name: "direccion");

            migrationBuilder.DropTable(
                name: "estado_paciente");

            migrationBuilder.DropTable(
                name: "piso");

            migrationBuilder.DropTable(
                name: "tipo_habitacion");

            migrationBuilder.DropTable(
                name: "codigo_postal");

            migrationBuilder.DropTable(
                name: "ciudad");

            migrationBuilder.DropTable(
                name: "pais");
        }
    }
}
