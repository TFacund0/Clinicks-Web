using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Interfaces
{
    /// <summary>
    /// Contrato para el acceso a datos relacionados con la autenticación y seguridad.
    /// Define cómo el sistema debe interactuar con la base de datos para temas de login.
    /// </summary>
    public interface IAuthRepository
    {
        /// <summary>
        /// Busca un usuario en la tabla Usuarios coincidiendo el nombre de usuario de forma exacta.
        /// </summary>
        /// <param name="username">El nombre de usuario a buscar.</param>
        /// <returns>La entidad Usuario encontrada o null si no existe.</returns>
        Task<Usuario?> GetUsuarioByUsernameAsync(string username);

        /// <summary>
        /// Busca un usuario a través de la matrícula de un médico asociado.
        /// </summary>
        /// <param name="matricula">La matrícula del médico a buscar.</param>
        /// <returns>La entidad Usuario asociada al médico encontrado o null si no existe.</
        Task<Usuario?> GetUsuarioByMedicoMatriculaAsync(string matricula);

        /// <summary>
        /// Busca el médico asociado a un usuario dado su ID de usuario (FK id_usuario).
        /// Esta es la relación correcta entre las tablas 'usuario' y 'medico'.
        /// </summary>
        /// <param name="usuarioId">El ID del usuario para el cual se busca el médico asociado.</param>
        /// <returns>La entidad Medico asociada al usuario o null si no existe.</
        Task<Medico?> GetMedicoByUsuarioIdAsync(int usuarioId);

        /// <summary>Obtiene todos los usuarios (para el proceso de migración de contraseñas).</summary>
        /// <returns>Una lista de entidades Usuario.</returns>
        Task<IEnumerable<Usuario>> GetAllUsuariosAsync();

        /// <summary>Actualiza los datos de un usuario (usada para guardar el hash de contraseña).</summary>
        /// <param name="usuario">La entidad Usuario con los datos actualizados que se desea guardar en la base de datos.</param>
        Task UpdateUsuarioAsync(Usuario usuario);
    }
}