using System;
using System.Collections.Generic;
using ClinicksApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicksApi.Data;

public partial class ClinicksDbContext : DbContext
{
    public ClinicksDbContext()
    {
    }

    public ClinicksDbContext(DbContextOptions<ClinicksDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cama> Camas { get; set; }

    public virtual DbSet<Ciudad> Ciudads { get; set; }

    public virtual DbSet<CodigoPostal> CodigoPostals { get; set; }

    public virtual DbSet<ConsultaMedica> ConsultaMedicas { get; set; }

    public virtual DbSet<Direccion> Direccions { get; set; }

    public virtual DbSet<Especialidad> Especialidads { get; set; }

    public virtual DbSet<EstadoCama> EstadoCamas { get; set; }

    public virtual DbSet<EstadoPaciente> EstadoPacientes { get; set; }

    public virtual DbSet<EstadoTurno> EstadoTurnos { get; set; }

    public virtual DbSet<EstadoUsuario> EstadoUsuarios { get; set; }

    public virtual DbSet<Habitacion> Habitacions { get; set; }

    public virtual DbSet<Internacion> Internacions { get; set; }

    public virtual DbSet<Medicacion> Medicacions { get; set; }

    public virtual DbSet<Medico> Medicos { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<Pais> Pais { get; set; }

    public virtual DbSet<Piso> Pisos { get; set; }

    public virtual DbSet<Procedimiento> Procedimientos { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Telefono> Telefonos { get; set; }

    public virtual DbSet<TipoHabitacion> TipoHabitacions { get; set; }

    public virtual DbSet<Turno> Turnos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cama>(entity =>
        {
            entity.HasKey(e => e.IdCama).HasName("cama_pkey");

            entity.ToTable("cama");

            entity.HasIndex(e => e.CodigoCama, "cama_codigo_cama_key").IsUnique();

            entity.Property(e => e.IdCama).HasColumnName("id_cama");
            entity.Property(e => e.CodigoCama)
                .HasMaxLength(30)
                .HasColumnName("codigo_cama");
            entity.Property(e => e.IdEstadoCama).HasColumnName("id_estado_cama");
            entity.Property(e => e.IdHabitacion).HasColumnName("id_habitacion");

            entity.HasOne(d => d.IdEstadoCamaNavigation).WithMany(p => p.Camas)
                .HasForeignKey(d => d.IdEstadoCama)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cama_id_estado_cama_fkey");

            entity.HasOne(d => d.IdHabitacionNavigation).WithMany(p => p.Camas)
                .HasForeignKey(d => d.IdHabitacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cama_id_habitacion_fkey");
        });

        modelBuilder.Entity<Ciudad>(entity =>
        {
            entity.HasKey(e => e.IdCiudad).HasName("ciudad_pkey");

            entity.ToTable("ciudad");

            entity.Property(e => e.IdCiudad).HasColumnName("id_ciudad");
            entity.Property(e => e.IdPais).HasColumnName("id_pais");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.Ciudads)
                .HasForeignKey(d => d.IdPais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ciudad_id_pais_fkey");
        });

        modelBuilder.Entity<CodigoPostal>(entity =>
        {
            entity.HasKey(e => e.IdCodigoPostal).HasName("codigo_postal_pkey");

            entity.ToTable("codigo_postal");

            entity.HasIndex(e => e.Codigo, "codigo_postal_codigo_key").IsUnique();

            entity.Property(e => e.IdCodigoPostal).HasColumnName("id_codigo_postal");
            entity.Property(e => e.Codigo)
                .HasMaxLength(15)
                .HasColumnName("codigo");
            entity.Property(e => e.IdCiudad).HasColumnName("id_ciudad");

            entity.HasOne(d => d.IdCiudadNavigation).WithMany(p => p.CodigoPostals)
                .HasForeignKey(d => d.IdCiudad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("codigo_postal_id_ciudad_fkey");
        });

        modelBuilder.Entity<ConsultaMedica>(entity =>
        {
            entity.HasKey(e => e.IdConsulta).HasName("consulta_medica_pkey");

            entity.ToTable("consulta_medica");

            entity.Property(e => e.IdConsulta).HasColumnName("id_consulta");
            entity.Property(e => e.Diagnostico).HasColumnName("diagnostico");
            entity.Property(e => e.FechaConsulta)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_consulta");
            entity.Property(e => e.IdMedico).HasColumnName("id_medico");
            entity.Property(e => e.IdPaciente).HasColumnName("id_paciente");
            entity.Property(e => e.Motivo).HasColumnName("motivo");
            entity.Property(e => e.Observacion).HasColumnName("observacion");
            entity.Property(e => e.Recomendacion).HasColumnName("recomendacion");
            entity.Property(e => e.Tratamiento).HasColumnName("tratamiento");

            entity.HasOne(d => d.IdMedicoNavigation).WithMany(p => p.ConsultaMedicas)
                .HasForeignKey(d => d.IdMedico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("consulta_medica_id_medico_fkey");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.ConsultaMedicas)
                .HasForeignKey(d => d.IdPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("consulta_medica_id_paciente_fkey");
        });

        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.HasKey(e => e.IdDireccion).HasName("direccion_pkey");

            entity.ToTable("direccion");

            entity.Property(e => e.IdDireccion).HasColumnName("id_direccion");
            entity.Property(e => e.IdCodigoPostal).HasColumnName("id_codigo_postal");
            entity.Property(e => e.NombreCalle)
                .HasMaxLength(100)
                .HasColumnName("nombre_calle");
            entity.Property(e => e.NumeroCalle).HasColumnName("numero_calle");

            entity.HasOne(d => d.IdCodigoPostalNavigation).WithMany(p => p.Direccions)
                .HasForeignKey(d => d.IdCodigoPostal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("direccion_id_codigo_postal_fkey");
        });

        modelBuilder.Entity<Especialidad>(entity =>
        {
            entity.HasKey(e => e.IdEspecialidad).HasName("especialidad_pkey");

            entity.ToTable("especialidad");

            entity.HasIndex(e => e.Nombre, "especialidad_nombre_key").IsUnique();

            entity.Property(e => e.IdEspecialidad).HasColumnName("id_especialidad");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<EstadoCama>(entity =>
        {
            entity.HasKey(e => e.IdEstadoCama).HasName("estado_cama_pkey");

            entity.ToTable("estado_cama");

            entity.HasIndex(e => e.Nombre, "estado_cama_nombre_key").IsUnique();

            entity.Property(e => e.IdEstadoCama).HasColumnName("id_estado_cama");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<EstadoPaciente>(entity =>
        {
            entity.HasKey(e => e.IdEstadoPaciente).HasName("estado_paciente_pkey");

            entity.ToTable("estado_paciente");

            entity.HasIndex(e => e.Nombre, "estado_paciente_nombre_key").IsUnique();

            entity.Property(e => e.IdEstadoPaciente).HasColumnName("id_estado_paciente");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<EstadoTurno>(entity =>
        {
            entity.HasKey(e => e.IdEstadoTurno).HasName("estado_turno_pkey");

            entity.ToTable("estado_turno");

            entity.HasIndex(e => e.Nombre, "estado_turno_nombre_key").IsUnique();

            entity.Property(e => e.IdEstadoTurno).HasColumnName("id_estado_turno");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<EstadoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdEstadoUsuario).HasName("estado_usuario_pkey");

            entity.ToTable("estado_usuario");

            entity.HasIndex(e => e.Nombre, "estado_usuario_nombre_key").IsUnique();

            entity.Property(e => e.IdEstadoUsuario).HasColumnName("id_estado_usuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Habitacion>(entity =>
        {
            entity.HasKey(e => e.IdHabitacion).HasName("habitacion_pkey");

            entity.ToTable("habitacion");

            entity.HasIndex(e => new { e.NroHabitacion, e.IdPiso }, "habitacion_nro_habitacion_id_piso_key").IsUnique();

            entity.Property(e => e.IdHabitacion).HasColumnName("id_habitacion");
            entity.Property(e => e.IdPiso).HasColumnName("id_piso");
            entity.Property(e => e.IdTipoHabitacion).HasColumnName("id_tipo_habitacion");
            entity.Property(e => e.NroHabitacion).HasColumnName("nro_habitacion");

            entity.HasOne(d => d.IdPisoNavigation).WithMany(p => p.Habitacions)
                .HasForeignKey(d => d.IdPiso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("habitacion_id_piso_fkey");

            entity.HasOne(d => d.IdTipoHabitacionNavigation).WithMany(p => p.Habitacions)
                .HasForeignKey(d => d.IdTipoHabitacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("habitacion_id_tipo_habitacion_fkey");
        });

        modelBuilder.Entity<Internacion>(entity =>
        {
            entity.HasKey(e => e.IdInternacion).HasName("internacion_pkey");

            entity.ToTable("internacion");

            entity.Property(e => e.IdInternacion).HasColumnName("id_internacion");
            entity.Property(e => e.FechaFin)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_inicio");
            entity.Property(e => e.IdCama).HasColumnName("id_cama");
            entity.Property(e => e.IdMedico).HasColumnName("id_medico");
            entity.Property(e => e.IdPaciente).HasColumnName("id_paciente");
            entity.Property(e => e.Motivo).HasColumnName("motivo");

            entity.HasOne(d => d.IdCamaNavigation).WithMany(p => p.Internacions)
                .HasForeignKey(d => d.IdCama)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("internacion_id_cama_fkey");

            entity.HasOne(d => d.IdMedicoNavigation).WithMany(p => p.Internacions)
                .HasForeignKey(d => d.IdMedico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("internacion_id_medico_fkey");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.Internacions)
                .HasForeignKey(d => d.IdPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("internacion_id_paciente_fkey");
        });

        modelBuilder.Entity<Medicacion>(entity =>
        {
            entity.HasKey(e => e.IdMedicacion).HasName("medicacion_pkey");

            entity.ToTable("medicacion");

            entity.Property(e => e.IdMedicacion).HasColumnName("id_medicacion");
            entity.Property(e => e.Dosis)
                .HasMaxLength(50)
                .HasColumnName("dosis");
            entity.Property(e => e.FechaFin)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_fin");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_inicio");
            entity.Property(e => e.Frecuencia)
                .HasMaxLength(100)
                .HasColumnName("frecuencia");
            entity.Property(e => e.IdInternacion).HasColumnName("id_internacion");
            entity.Property(e => e.NombreMedicamento)
                .HasMaxLength(100)
                .HasColumnName("nombre_medicamento");

            entity.HasOne(d => d.IdInternacionNavigation).WithMany(p => p.Medicacions)
                .HasForeignKey(d => d.IdInternacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("medicacion_id_internacion_fkey");
        });

        modelBuilder.Entity<Medico>(entity =>
        {
            entity.HasKey(e => e.IdMedico).HasName("medico_pkey");

            entity.ToTable("medico");

            entity.HasIndex(e => e.Correo, "medico_correo_key").IsUnique();

            entity.HasIndex(e => e.Dni, "medico_dni_key").IsUnique();

            entity.HasIndex(e => e.Matricula, "medico_matricula_key").IsUnique();

            entity.Property(e => e.IdMedico).HasColumnName("id_medico");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .HasColumnName("apellido");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.Dni)
                .HasMaxLength(15)
                .HasColumnName("dni");
            entity.Property(e => e.IdDireccion).HasColumnName("id_direccion");
            entity.Property(e => e.IdEspecialidad).HasColumnName("id_especialidad");
            entity.Property(e => e.Matricula)
                .HasMaxLength(30)
                .HasColumnName("matricula");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.Medicos)
                .HasForeignKey(d => d.IdDireccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("medico_id_direccion_fkey");

            entity.HasOne(d => d.IdEspecialidadNavigation).WithMany(p => p.Medicos)
                .HasForeignKey(d => d.IdEspecialidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("medico_id_especialidad_fkey");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.IdPaciente).HasName("paciente_pkey");

            entity.ToTable("paciente");

            entity.HasIndex(e => e.Correo, "paciente_correo_key").IsUnique();

            entity.HasIndex(e => e.Dni, "paciente_dni_key").IsUnique();

            entity.Property(e => e.IdPaciente).HasColumnName("id_paciente");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .HasColumnName("apellido");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.Dni)
                .HasMaxLength(15)
                .HasColumnName("dni");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.FechaRegistracion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_registracion");
            entity.Property(e => e.IdDireccion).HasColumnName("id_direccion");
            entity.Property(e => e.IdEstadoPaciente).HasColumnName("id_estado_paciente");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Observaciones).HasColumnName("observaciones");

            entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.IdDireccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("paciente_id_direccion_fkey");

            entity.HasOne(d => d.IdEstadoPacienteNavigation).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.IdEstadoPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("paciente_id_estado_paciente_fkey");
        });

        modelBuilder.Entity<Pais>(entity =>
        {
            entity.HasKey(e => e.IdPais).HasName("pais_pkey");

            entity.ToTable("pais");

            entity.HasIndex(e => e.Nombre, "pais_nombre_key").IsUnique();

            entity.Property(e => e.IdPais).HasColumnName("id_pais");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Piso>(entity =>
        {
            entity.HasKey(e => e.IdPiso).HasName("piso_pkey");

            entity.ToTable("piso");

            entity.HasIndex(e => e.NroPiso, "piso_nro_piso_key").IsUnique();

            entity.Property(e => e.IdPiso).HasColumnName("id_piso");
            entity.Property(e => e.NroPiso)
                .HasMaxLength(10)
                .HasColumnName("nro_piso");
        });

        modelBuilder.Entity<Procedimiento>(entity =>
        {
            entity.HasKey(e => e.IdProcedimiento).HasName("procedimiento_pkey");

            entity.ToTable("procedimiento");

            entity.Property(e => e.IdProcedimiento).HasColumnName("id_procedimiento");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Fecha)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha");
            entity.Property(e => e.Resultado).HasColumnName("resultado");
            entity.Property(e => e.Tipo)
                .HasMaxLength(100)
                .HasColumnName("tipo");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("rol_pkey");

            entity.ToTable("rol");

            entity.HasIndex(e => e.Nombre, "rol_nombre_key").IsUnique();

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Telefono>(entity =>
        {
            entity.HasKey(e => e.IdTelefono).HasName("telefono_pkey");

            entity.ToTable("telefono");

            entity.Property(e => e.IdTelefono).HasColumnName("id_telefono");
            entity.Property(e => e.IdPaciente).HasColumnName("id_paciente");
            entity.Property(e => e.NumeroTelefono)
                .HasMaxLength(20)
                .HasColumnName("numero_telefono");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.Telefonos)
                .HasForeignKey(d => d.IdPaciente)
                .HasConstraintName("telefono_id_paciente_fkey");
        });

        modelBuilder.Entity<TipoHabitacion>(entity =>
        {
            entity.HasKey(e => e.IdTipoHabitacion).HasName("tipo_habitacion_pkey");

            entity.ToTable("tipo_habitacion");

            entity.HasIndex(e => e.Nombre, "tipo_habitacion_nombre_key").IsUnique();

            entity.Property(e => e.IdTipoHabitacion).HasColumnName("id_tipo_habitacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Turno>(entity =>
        {
            entity.HasKey(e => e.IdTurno).HasName("turno_pkey");

            entity.ToTable("turno");

            entity.Property(e => e.IdTurno).HasColumnName("id_turno");
            entity.Property(e => e.FechaRegistracion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_registracion");
            entity.Property(e => e.FechaTurno)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_turno");
            entity.Property(e => e.IdConsulta).HasColumnName("id_consulta");
            entity.Property(e => e.IdEstadoTurno).HasColumnName("id_estado_turno");
            entity.Property(e => e.IdMedico).HasColumnName("id_medico");
            entity.Property(e => e.IdPaciente).HasColumnName("id_paciente");
            entity.Property(e => e.IdProcedimiento).HasColumnName("id_procedimiento");
            entity.Property(e => e.Motivo)
                .HasMaxLength(200)
                .HasColumnName("motivo");

            entity.HasOne(d => d.IdConsultaNavigation).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.IdConsulta)
                .HasConstraintName("turno_id_consulta_fkey");

            entity.HasOne(d => d.IdEstadoTurnoNavigation).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.IdEstadoTurno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("turno_id_estado_turno_fkey");

            entity.HasOne(d => d.IdMedicoNavigation).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.IdMedico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("turno_id_medico_fkey");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.IdPaciente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("turno_id_paciente_fkey");

            entity.HasOne(d => d.IdProcedimientoNavigation).WithMany(p => p.Turnos)
                .HasForeignKey(d => d.IdProcedimiento)
                .HasConstraintName("turno_id_procedimiento_fkey");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("usuario_pkey");

            entity.ToTable("usuario");

            entity.HasIndex(e => e.Correo, "usuario_correo_key").IsUnique();

            entity.HasIndex(e => e.Username, "usuario_username_key").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .HasColumnName("apellido");
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .HasColumnName("correo");
            entity.Property(e => e.IdEstadoUsuario).HasColumnName("id_estado_usuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.IdEstadoUsuarioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdEstadoUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("usuario_id_estado_usuario_fkey");

            entity.HasMany(d => d.IdRols).WithMany(p => p.IdUsuarios)
                .UsingEntity<Dictionary<string, object>>(
                    "UsuarioRol",
                    r => r.HasOne<Rol>().WithMany()
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("usuario_rol_id_rol_fkey"),
                    l => l.HasOne<Usuario>().WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("usuario_rol_id_usuario_fkey"),
                    j =>
                    {
                        j.HasKey("IdUsuario", "IdRol").HasName("usuario_rol_pkey");
                        j.ToTable("usuario_rol");
                        j.IndexerProperty<int>("IdUsuario").HasColumnName("id_usuario");
                        j.IndexerProperty<int>("IdRol").HasColumnName("id_rol");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
