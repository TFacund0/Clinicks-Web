using ClinicksApi.Data.Entities;

namespace ClinicksApi.Data.Interfaces
{
    public interface IAuthRepository
    {
        Task<Usuario?> GetUsuarioByUsernameAsync(string username);
        Task<Medico?> GetMedicoByMatriculaAsync(string matricula);
        Task<Medico?> GetFirstMedicoAsync();
        
        // Métodos para la migración temporal de contraseñas
        Task<IEnumerable<Usuario>> GetAllUsuariosAsync();
        Task UpdateUsuarioAsync(Usuario usuario);
    }
}