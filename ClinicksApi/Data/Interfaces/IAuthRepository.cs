using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Interfaces
{
    /// <summary>
    /// Contrato para el acceso a datos relacionados con la autenticación y seguridad.
    /// Define cómo el sistema debe interactuar con la base de datos para temas de login.
    /// </summary>
    public interface IAuthRepository
    {
        /// <summary>Busca un usuario en la tabla Usuarios coincidiendo el nombre de usuario de forma exacta.</summary>
        Task<Usuario?> GetUsuarioByUsernameAsync(string username);

        /// <summary>Busca un médico en la tabla Medicos filtrando por su matrícula.</summary>
        Task<Medico?> GetMedicoByMatriculaAsync(string matricula);

        /// <summary>Método de fallback temporal para obtener el primer médico disponible en desarrollo.</summary>
        Task<Medico?> GetFirstMedicoAsync();
        
        /// <summary>Obtiene absolutamente todos los usuarios. Usado únicamente para el script de migración a BCrypt.</summary>
        Task<IEnumerable<Usuario>> GetAllUsuariosAsync();

        /// <summary>Actualiza los datos de un usuario existente (ej. guardar su nueva contraseña hasheada).</summary>
        Task UpdateUsuarioAsync(Usuario usuario);
    }
}