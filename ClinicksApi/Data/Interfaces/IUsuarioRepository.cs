using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Interfaces
{
    /// <summary>
    /// Contrato para el acceso a datos relacionados con los Usuarios.
    /// </summary>
    public interface IUsuarioRepository
    {
        /// <summary>
        /// Busca un usuario en la tabla Usuarios coincidiendo el nombre de usuario de forma exacta.
        /// </summary>
        /// <param name="username">El nombre de usuario a buscar.</param>
        /// <returns>La entidad Usuario encontrada o null si no existe.</returns>
        Task<Usuario?> ObtenerUsuarioPorUsernameAsync(string username);

        /// <summary>
        /// Busca un usuario a través de la matrícula de un médico asociado.
        /// </summary>
        /// <param name="matricula">La matrícula del médico a buscar.</param>
        /// <returns>La entidad Usuario asociada al médico encontrado o null si no existe.</returns>
        Task<Usuario?> ObtenerUsuarioPorMatriculaAsync(string matricula);

        /// <summary>Actualiza los datos de un usuario (usada para guardar el hash de contraseña).</summary>
        /// <param name="usuario">La entidad Usuario con los datos actualizados que se desea guardar en la base de datos.</param>
        Task ActualizarUsuarioAsync(Usuario usuario);
    }
}
