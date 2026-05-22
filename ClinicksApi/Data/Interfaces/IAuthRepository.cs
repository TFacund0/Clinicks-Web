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
        Task<Usuario?> GetUsuarioByUsernameAsync(string username);

        /// <summary>
        /// Busca un usuario a través de la matrícula de un médico asociado.
        /// </summary>
        Task<Usuario?> GetUsuarioByMedicoMatriculaAsync(string matricula);

        /// <summary>
        /// Busca el médico asociado a un usuario dado su ID de usuario (FK id_usuario).
        /// Esta es la relación correcta entre las tablas 'usuario' y 'medico'.
        /// </summary>
        Task<Medico?> GetMedicoByUsuarioIdAsync(int usuarioId);

        /// <summary>Obtiene todos los usuarios (para el proceso de migración de contraseñas).</summary>
        Task<IEnumerable<Usuario>> GetAllUsuariosAsync();

        /// <summary>Actualiza los datos de un usuario (usada para guardar el hash de contraseña).</summary>
        Task UpdateUsuarioAsync(Usuario usuario);
    }
}